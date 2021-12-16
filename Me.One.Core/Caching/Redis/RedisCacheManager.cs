using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Me.One.Core.Configuration;
using Me.One.Core.Security;
using Me.One.Core.Threading;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Me.One.Core.Caching.Redis
{
    public class RedisCacheManager : IStaticCacheManager
    {
        private readonly AppStartupConfig _config;
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db;
        private readonly PerRequestCache _perRequestCache;
        private bool _disposed;

        public RedisCacheManager(
            IHttpContextAccessor httpContextAccessor,
            IRedisConnectionWrapper connectionWrapper,
            AppStartupConfig config)
        {
            _config = !string.IsNullOrEmpty(config.RedisConnectionString)
                ? config
                : throw new System.Exception("Redis connection string is empty");
            _connectionWrapper = connectionWrapper;
            _db = _connectionWrapper.GetDatabase(config.RedisDatabaseId ?? 1);
            _perRequestCache = new PerRequestCache(httpContextAccessor);
        }

        public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
        {
            if (await IsSetAsync(key))
            {
                return await GetAsync<T>(key);
            }
            var result = await acquire();
            if (key.CacheTime > 0)
            {
                await SetAsync(key.Key, result, key.CacheTime);
            }
            return result;
        }

        public virtual T Get<T>(CacheKey key, Func<T> acquire)
        {
            if (IsSet(key))
            {
                var obj = Get<T>(key);
                if (obj != null && !obj.Equals(default(T)))
                {
                    return obj;
                }
            }

            var obj1 = acquire();
            if (key.CacheTime > 0)
            {
                Set(key, obj1);
            }
            return obj1;
        }

        public virtual void Set(CacheKey key, object data)
        {
            if (data == null)
            {
                return;
            }
            var expiresIn = TimeSpan.FromMinutes(key.CacheTime);
            var serializedItem = JsonConvert.SerializeObject(data);
            TryPerformAction(() => _db.StringSet((RedisKey) key.Key, (RedisValue) serializedItem, expiresIn));
            _perRequestCache.Set(key.Key, data);
        }

        public virtual bool IsSet(CacheKey key)
        {
            if (_perRequestCache.IsSet(key.Key))
            {
                return true;
            }
            var tuple = TryPerformAction(() => _db.KeyExists((RedisKey) key.Key));
            return tuple.Item1 & tuple.Item2;
        }

        public virtual void Remove(CacheKey key)
        {
            if (key.Key.Equals(AppDataProtectionDefaults.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            TryPerformAction(() => _db.KeyDelete((RedisKey) key.Key));
            _perRequestCache.Remove(key.Key);
        }

        public virtual void RemoveByPrefix(string prefix)
        {
            _perRequestCache.RemoveByPrefix(prefix);
            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var keys = GetKeys(endPoint, prefix);
                TryPerformAction(() => _db.KeyDelete(keys.ToArray()));
            }
        }

        public virtual void Clear()
        {
            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var keys = GetKeys(endPoint).ToArray();
                foreach (var redisKey in keys)
                {
                    _perRequestCache.Remove(redisKey.ToString());
                }
                TryPerformAction(() => _db.KeyDelete(keys.ToArray()));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual IEnumerable<RedisKey> GetKeys(
            EndPoint endPoint,
            string prefix = null)
        {
            return _connectionWrapper.GetServer(endPoint)
                .Keys(_db.Database, (RedisValue) (string.IsNullOrEmpty(prefix) ? null : prefix + "*"), 250).Where(
                    (Func<RedisKey, bool>) (key =>
                        !key.ToString().Equals(AppDataProtectionDefaults.RedisDataProtectionKey,
                            StringComparison.OrdinalIgnoreCase)));
        }

        protected virtual async Task<T> GetAsync<T>(CacheKey key)
        {
            if (_perRequestCache.IsSet(key.Key))
            {
                return _perRequestCache.Get(key.Key, (Func<T>) (() => default));
            }
            var async = await _db.StringGetAsync((RedisKey) key.Key);
            if (!async.HasValue)
            {
                return default;
            }
            var obj = JsonConvert.DeserializeObject<T>(async);
            if (obj == null)
            {
                return default;
            }
            _perRequestCache.Set(key.Key, obj);
            return obj;
        }

        protected virtual async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }
            var timeSpan = TimeSpan.FromMinutes(cacheTime);
            var str = JsonConvert.SerializeObject(data);
            var num = await _db.StringSetAsync((RedisKey) key, (RedisValue) str, timeSpan) ? 1 : 0;
        }

        protected virtual async Task<bool> IsSetAsync(CacheKey key)
        {
            return _perRequestCache.IsSet(key.Key) || await _db.KeyExistsAsync((RedisKey) key.Key);
        }

        protected virtual (bool, T) TryPerformAction<T>(Func<T> action)
        {
            try
            {
                return (true, action());
            }
            catch (RedisTimeoutException ex)
            {
                if (_config.IgnoreRedisTimeoutException)
                {
                    return (false, default);
                }
                throw;
            }
        }

        public virtual T Get<T>(CacheKey key)
        {
            return _perRequestCache.IsSet(key.Key)
                ? _perRequestCache.Get(key.Key, (Func<T>) (() => default))
                : TryPerformAction(() =>
                {
                    var redisValue = _db.StringGet((RedisKey) key.Key);
                    if (!redisValue.HasValue)
                        return default;
                    var obj = JsonConvert.DeserializeObject<T>(redisValue);
                    if (obj == null)
                        return default;
                    _perRequestCache.Set(key.Key, obj);
                    return obj;
                }).Item2;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
        }

        protected class PerRequestCache
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ReaderWriterLockSlim _locker;

            public PerRequestCache(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
                _locker = new ReaderWriterLockSlim();
            }

            protected virtual IDictionary<object, object> GetItems()
            {
                return _httpContextAccessor.HttpContext?.Items;
            }

            public virtual T Get<T>(string key, Func<T> acquire)
            {
                IDictionary<object, object> items;
                using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.Read))
                {
                    items = GetItems();
                    if (items == null)
                    {
                        return acquire();
                    }

                    if (items[key] != null)
                    {
                        return (T) items[key];
                    }
                }

                var obj = acquire();
                using (new ReaderWriteLockDisposable(_locker))
                {
                    items[key] = obj;
                }

                return obj;
            }

            public virtual void Set(string key, object data)
            {
                if (data == null)
                {
                    return;
                }
                using (new ReaderWriteLockDisposable(_locker))
                {
                    var items = GetItems();
                    if (items == null)
                    {
                        return;
                    }
                    items[key] = data;
                }
            }

            public virtual bool IsSet(string key)
            {
                using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.Read))
                {
                    return GetItems()?[key] != null;
                }
            }

            public virtual void Remove(string key)
            {
                using (new ReaderWriteLockDisposable(_locker))
                {
                    GetItems()?.Remove(key);
                }
            }

            public virtual void RemoveByPrefix(string prefix)
            {
                using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.UpgradeableRead))
                {
                    var items = GetItems();
                    if (items == null)
                    {
                        return;
                    }
                    var regex = new Regex(prefix,
                        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                    var list = items.Keys.Select((Func<object, string>) (p => p.ToString()))
                        .Where((Func<string, bool>) (key => regex.IsMatch(key))).ToList();
                    if (!list.Any())
                    {
                        return;
                    }
                    using (new ReaderWriteLockDisposable(_locker))
                    {
                        foreach (var str in list)
                        {
                            items.Remove(str);
                        }
                    }
                }
            }

            public virtual void Clear()
            {
                using (new ReaderWriteLockDisposable(_locker))
                {
                    GetItems()?.Clear();
                }
            }
        }
    }
}
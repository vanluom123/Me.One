using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Me.One.Core.Caching
{
    public class MemoryCacheManager : ICacheLocker, IStaticCacheManager, IDisposable
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _prefixes = new();
        private static CancellationTokenSource _clearToken = new();
        private readonly IMemoryCache _memoryCache;
        private bool _disposed;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool PerformActionWithLock(string key, TimeSpan expirationTime, Action action)
        {
            if (IsSet(new CacheKey(key, Array.Empty<string>())))
                return false;
            try
            {
                _memoryCache.Set(key, key, expirationTime);
                action();
                return true;
            }
            finally
            {
                Remove(key);
            }
        }

        public T Get<T>(CacheKey key, Func<T> acquire)
        {
            if (key.CacheTime <= 0)
                return acquire();
            var obj = _memoryCache.GetOrCreate(key.Key, entry =>
            {
                entry.SetOptions(PrepareEntryOptions(key));
                return acquire();
            });
            if (obj != null)
                return obj;
            Remove(key);
            return obj;
        }

        public void Remove(CacheKey key)
        {
            _memoryCache.Remove(key.Key);
        }

        public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
        {
            if (key.CacheTime <= 0)
                return await acquire();
            var async = await _memoryCache.GetOrCreateAsync(key.Key, async entry =>
            {
                entry.SetOptions(PrepareEntryOptions(key));
                return await acquire();
            });
            if (async == null)
                Remove(key);
            return async;
        }

        public void Set(CacheKey key, object data)
        {
            if (key.CacheTime <= 0 || data == null)
                return;
            _memoryCache.Set(key.Key, data, PrepareEntryOptions(key));
        }

        public bool IsSet(CacheKey key)
        {
            return _memoryCache.TryGetValue(key.Key, out var _);
        }

        public void RemoveByPrefix(string prefix)
        {
            CancellationTokenSource cancellationTokenSource;
            _prefixes.TryRemove(prefix, out cancellationTokenSource);
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }

        public void Clear()
        {
            _clearToken.Cancel();
            _clearToken.Dispose();
            _clearToken = new CancellationTokenSource();
            foreach (var key in _prefixes.Keys.ToList())
            {
                CancellationTokenSource cancellationTokenSource;
                _prefixes.TryRemove(key, out cancellationTokenSource);
                cancellationTokenSource?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(key.CacheTime)
            };
            options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
            foreach (var key1 in key.Prefixes.ToList())
            {
                var orAdd = _prefixes.GetOrAdd(key1, new CancellationTokenSource());
                options.AddExpirationToken(new CancellationChangeToken(orAdd.Token));
            }

            return options;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
                _memoryCache.Dispose();
            _disposed = true;
        }
    }
}
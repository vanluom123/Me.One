using System;
using System.Collections.Generic;
using System.Linq;
using Me.One.Core.Cryptography;

namespace Me.One.Core.Caching
{
    public class DefaultCacheKeyService : ICacheKeyService
    {
        private readonly CachingSettings _cachingSettings;

        public DefaultCacheKeyService(CachingSettings cachingSettings)
        {
            _cachingSettings = cachingSettings;
        }

        public virtual CacheKey PrepareKey(CacheKey cacheKey, params object[] keyObjects)
        {
            return FillCacheKey(cacheKey, keyObjects);
        }

        public virtual CacheKey PrepareKeyForDefaultCache(
            CacheKey cacheKey,
            params object[] keyObjects)
        {
            var cacheKey1 = FillCacheKey(cacheKey, keyObjects);
            cacheKey1.CacheTime = _cachingSettings.DefaultCacheTime;
            return cacheKey1;
        }

        public virtual CacheKey PrepareKeyForShortTermCache(
            CacheKey cacheKey,
            params object[] keyObjects)
        {
            var cacheKey1 = FillCacheKey(cacheKey, keyObjects);
            cacheKey1.CacheTime = _cachingSettings.ShortTermCacheTime;
            return cacheKey1;
        }

        public virtual string PrepareKeyPrefix(string keyFormatter, params object[] keyObjects)
        {
            return keyObjects == null || !keyObjects.Any()
                ? keyFormatter
                : string.Format(keyFormatter,
                    keyObjects
                        .Select(CreateCacheKeyParameters).ToArray());
        }

        protected virtual string CreateIdsHash(IEnumerable<string> ids)
        {
            var list = ids.ToList();
            return !list.Any()
                ? string.Empty
                : HashUtilities.Hash(string.Join(", ", list.OrderBy((Func<string, string>) (id => id))), HashType.SHA1);
        }

        protected virtual object CreateCacheKeyParameters(object parameter)
        {
            switch (parameter)
            {
                case null:
                    return "null";
                case IEnumerable<string> ids:
                    return CreateIdsHash(ids);
                case IEnumerable<BaseEntity> source:
                    return CreateIdsHash(source.Select((Func<BaseEntity, string>) (e => e.Id)));
                case BaseEntity baseEntity:
                    return baseEntity.Id;
                default:
                    return parameter.ToString();
            }
        }

        protected virtual CacheKey FillCacheKey(CacheKey cacheKey, params object[] keyObjects)
        {
            return new(cacheKey, CreateCacheKeyParameters, keyObjects);
        }
    }
}
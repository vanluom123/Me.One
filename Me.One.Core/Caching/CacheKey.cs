using System;
using System.Collections.Generic;
using System.Linq;

namespace Me.One.Core.Caching
{
    public class CacheKey
    {
        protected string _keyFormat = "";

        public CacheKey(
            CacheKey cacheKey,
            Func<object, object> createCacheKeyParameters,
            params object[] keyObjects)
        {
            Init(cacheKey.Key, cacheKey.CacheTime, cacheKey.Prefixes.ToArray());
            if (!keyObjects.Any())
                return;
            Key = string.Format(_keyFormat, keyObjects.Select(createCacheKeyParameters).ToArray());
            for (var index = 0; index < Prefixes.Count; ++index)
                Prefixes[index] = string.Format(Prefixes[index], keyObjects.Select(createCacheKeyParameters).ToArray());
        }

        public CacheKey(string cacheKey, int? cacheTime = null, params string[] prefixes)
        {
            Init(cacheKey, cacheTime, prefixes);
        }

        public CacheKey(string cacheKey, params string[] prefixes)
        {
            Init(cacheKey, new int?(), prefixes);
        }

        public string Key { get; protected set; }

        public List<string> Prefixes { get; protected set; } = new();

        public int CacheTime { get; set; } = AppCachingDefaults.CacheTime;

        protected void Init(string cacheKey, int? cacheTime = null, params string[] prefixes)
        {
            Key = cacheKey;
            _keyFormat = cacheKey;
            if (cacheTime.HasValue)
                CacheTime = cacheTime.Value;
            Prefixes.AddRange(prefixes.Where((Func<string, bool>) (prefix => !string.IsNullOrEmpty(prefix))));
        }
    }
}
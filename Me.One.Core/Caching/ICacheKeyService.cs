namespace Me.One.Core.Caching
{
    public interface ICacheKeyService
    {
        CacheKey PrepareKey(CacheKey cacheKey, params object[] keyObjects);

        CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] keyObjects);

        CacheKey PrepareKeyForShortTermCache(CacheKey cacheKey, params object[] keyObjects);

        string PrepareKeyPrefix(string keyFormatter, params object[] keyObjects);
    }
}
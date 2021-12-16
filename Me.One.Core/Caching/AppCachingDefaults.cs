namespace Me.One.Core.Caching
{
    public static class AppCachingDefaults
    {
        public static int CacheTime => 60;

        public static string AppEntityCacheKey => "App.{0}.id-{1}";
    }
}
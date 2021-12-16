using Me.One.Core.Configuration;

namespace Me.One.Core.Caching
{
    public class CachingSettings : ISettings
    {
        public int DefaultCacheTime { get; set; }

        public int ShortTermCacheTime { get; set; }

        public int BundledFilesCacheTime { get; set; }
    }
}
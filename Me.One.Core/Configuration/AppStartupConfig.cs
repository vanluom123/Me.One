namespace Me.One.Core.Configuration
{
    public class AppStartupConfig
    {
        public bool DisplayFullErrorStack { get; set; }

        public bool RedisEnabled { get; set; }

        public bool UseRedisForCaching { get; set; }

        public string RedisConnectionString { get; set; }

        public int? RedisDatabaseId { get; set; }

        public bool IgnoreRedisTimeoutException { get; set; }
    }
}
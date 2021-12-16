namespace Me.One.Core.Caching.Redis
{
    public class CacheMessageEvent : ICacheMessageEvent
    {
        public string Key { get; set; }

        public int MessageType { get; set; }
    }
}
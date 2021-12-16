namespace Me.One.Core.Caching.Redis
{
    public interface ICacheMessageEvent
    {
        string Key { get; set; }

        int MessageType { get; set; }
    }
}
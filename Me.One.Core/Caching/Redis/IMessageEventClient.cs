namespace Me.One.Core.Caching.Redis
{
    public interface IMessageEventClient : ICacheMessageEvent
    {
        string ClientId { get; set; }
    }
}
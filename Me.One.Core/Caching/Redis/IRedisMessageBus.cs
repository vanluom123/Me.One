using System.Threading.Tasks;

namespace Me.One.Core.Caching.Redis
{
    public interface IRedisMessageBus
    {
        Task PublishAsync<TMessage>(TMessage msg) where TMessage : ICacheMessageEvent;

        Task SubscribeAsync();

        Task OnSubscriptionChanged(ICacheMessageEvent message);
    }
}
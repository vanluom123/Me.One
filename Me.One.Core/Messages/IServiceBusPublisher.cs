using System.Threading.Tasks;

namespace Me.One.Core.Messages
{
    public interface IServiceBusPublisher
    {
        Task SendQueueMessage(IBusinessEvent data, string messageId);

        Task SendQueueMessage(object data, string messageId);

        Task SendTopicMessage(IBusinessEvent data, string messageId);

        Task SendTopicMessage(object data, string messageId);
    }
}
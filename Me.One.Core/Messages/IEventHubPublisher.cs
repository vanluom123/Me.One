using System.Threading.Tasks;

namespace Me.One.Core.Messages
{
    public interface IEventHubPublisher
    {
        Task SendMessage(IBusinessEvent data, string messageId);

        Task SendMessage(object data, string messageId);
    }
}
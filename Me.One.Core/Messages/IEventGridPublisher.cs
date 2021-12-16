using System.Collections.Generic;
using System.Threading.Tasks;

namespace Me.One.Core.Messages
{
    public interface IEventGridPublisher
    {
        Task SendMessage(ICollection<IBusinessEvent> data, string messageId, string subject);

        Task SendMessage(ICollection<object> data, string messageId, string subject);
    }
}
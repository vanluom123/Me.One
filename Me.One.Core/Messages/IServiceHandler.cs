using System.Threading.Tasks;
using Me.One.Core.Exception;

namespace Me.One.Core.Messages
{
    public interface IServiceHandler
    {
        Task HandleAsync(string message, string messageId);

        Task HandleError(CustomEventHubException ex);
    }
}
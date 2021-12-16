using System.Threading.Tasks;
using Me.One.Core.Exception;

namespace Me.One.Core.Messages
{
    public abstract class BaseServiceHandler : IServiceHandler
    {
        public abstract Task HandleAsync(string message, string messageId);

        public virtual async Task HandleError(CustomEventHubException ex)
        {
            await Task.CompletedTask;
        }
    }
}
using System;
using System.Threading.Tasks;

namespace Me.One.Core.Messages
{
    public interface IEventGridSubscriber
    {
        Task RegisterHandler<T>(string queueName) where T : IServiceHandler;

        Task RegisterHandler(string queueName, Func<string, string, Task> callback);
    }
}
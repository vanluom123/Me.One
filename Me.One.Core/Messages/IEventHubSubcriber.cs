using System;
using System.Threading.Tasks;

namespace Me.One.Core.Messages
{
    public interface IEventHubSubcriber : IDisposable
    {
        Task RegisterHandler<T>(string key, string storeConnection, string container) where T : IServiceHandler;

        Task RegisterHandler(
            string key,
            string storeConnection,
            string container,
            Func<string, string, Task> callback);

        Task Unregister();
    }
}
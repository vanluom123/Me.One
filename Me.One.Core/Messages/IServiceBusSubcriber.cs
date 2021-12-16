using System;
using System.Threading.Tasks;

namespace Me.One.Core.Messages
{
    public interface IServiceBusSubcriber : IDisposable
    {
        Task RegisterQueueHandler<T>(string queueName) where T : IServiceHandler;

        Task RegisterQueueHandler(string queueName, Func<string, string, Task> callback);

        Task RegisterTopicHandler<T>(string topicName, string subcriptionId) where T : IServiceHandler;

        Task RegisterTopicHandler(
            string topicName,
            string subcriptionId,
            Func<string, string, Task> callback);
    }
}
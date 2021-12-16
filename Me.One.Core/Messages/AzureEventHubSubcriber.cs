using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace Me.One.Core.Messages
{
    public class AzureEventHubSubcriber : IEventHubSubcriber, IDisposable
    {
        private readonly ConcurrentDictionary<string, EventProcessorHost> _clients;
        private readonly string _hubName;
        private readonly string _strConnection;

        public AzureEventHubSubcriber(string strConnection, string hubName)
        {
            _strConnection = strConnection;
            _hubName = hubName;
            _clients = new ConcurrentDictionary<string, EventProcessorHost>();
        }

        public void Dispose()
        {
        }

        public async Task RegisterHandler<T>(string key, string storeConnection, string container)
            where T : IServiceHandler
        {
            var eventProcessorHost = new EventProcessorHost(_hubName, PartitionReceiver.DefaultConsumerGroupName,
                _strConnection, storeConnection, container);
            await eventProcessorHost.RegisterEventProcessorAsync<EventProcessor<T>>(
                EventProcessorOptions.DefaultOptions);
            _clients.TryAdd(key, eventProcessorHost);
            eventProcessorHost = null;
        }

        public async Task RegisterHandler(
            string key,
            string storeConnection,
            string container,
            Func<string, string, Task> callback)
        {
            var eventProcessorHost = new EventProcessorHost(_hubName, PartitionReceiver.DefaultConsumerGroupName,
                _strConnection, storeConnection, container);
            await eventProcessorHost.RegisterEventProcessorFactoryAsync(new EventProcessorFactory(callback),
                EventProcessorOptions.DefaultOptions);
            _clients.TryAdd(key, eventProcessorHost);
            eventProcessorHost = null;
        }

        public async Task Unregister()
        {
            if (_clients == null || _clients.Count <= 0)
                return;
            BaseEventProcessor.Processors.ForEach((Action<BaseEventProcessor>) (p => p.Dispose()));
            BaseEventProcessor.Processors.Clear();
            Task.WaitAll(_clients
                .Select((Func<KeyValuePair<string, EventProcessorHost>, Task>) (x =>
                    x.Value.UnregisterEventProcessorAsync())).ToArray());
            await Task.CompletedTask;
        }
    }
}
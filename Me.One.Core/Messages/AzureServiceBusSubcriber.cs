using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Me.One.Core.DependencyInjection;
using Microsoft.Azure.ServiceBus;

namespace Me.One.Core.Messages
{
    public class AzureServiceBusSubcriber : IServiceBusSubcriber, IDisposable
    {
        private const string SET_SUBCRIPTION_RULE = "SetSubcriptionRule";
        private readonly Dictionary<string, Func<string, string, Task>> _queueCallbacks;
        private readonly Dictionary<string, IQueueClient> _queueClients;
        private readonly string _strConnection;
        private readonly Dictionary<string, ISubscriptionClient> _subcriptionClients;
        private readonly Dictionary<string, Func<string, string, Task>> _topicCallbacks;

        public AzureServiceBusSubcriber(string connection)
        {
            _strConnection = connection;
            _queueClients = new Dictionary<string, IQueueClient>();
            _subcriptionClients = new Dictionary<string, ISubscriptionClient>();
            _queueCallbacks = new Dictionary<string, Func<string, string, Task>>();
            _topicCallbacks = new Dictionary<string, Func<string, string, Task>>();
        }

        public async Task RegisterQueueHandler<T>(string queueName) where T : IServiceHandler
        {
            var obj = DependencyResolver.Instance.Resolve<T>();
            await RegisterQueueHandler(queueName, obj.HandleAsync);
        }

        public async Task RegisterQueueHandler(
            string queueName,
            Func<string, string, Task> callback)
        {
            var serviceBusSubcriber = this;
            serviceBusSubcriber.AddOrUpdateDictionary(serviceBusSubcriber._queueCallbacks, queueName, callback);
            var messageHandlerOptions = new MessageHandlerOptions(serviceBusSubcriber.ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            var queueClient = new QueueClient(serviceBusSubcriber._strConnection, queueName);
            queueClient.RegisterMessageHandler(serviceBusSubcriber.ProcessQueueMessagesAsync, messageHandlerOptions);
            serviceBusSubcriber.AddOrUpdateDictionary(serviceBusSubcriber._queueClients, queueName, queueClient);
        }

        public async Task RegisterTopicHandler<T>(string topicName, string subcriptionId) where T : IServiceHandler
        {
            var obj = DependencyResolver.Instance.Resolve<T>();
            await RegisterTopicHandler(topicName, subcriptionId, obj.HandleAsync);
        }

        public async Task RegisterTopicHandler(
            string topicName,
            string subcriptionId,
            Func<string, string, Task> callback)
        {
            var serviceBusSubcriber = this;
            var key = topicName + "_" + subcriptionId;
            serviceBusSubcriber.AddOrUpdateDictionary(serviceBusSubcriber._topicCallbacks, key, callback);
            var messageHandlerOptions = new MessageHandlerOptions(serviceBusSubcriber.ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            var client = new SubscriptionClient(serviceBusSubcriber._strConnection, topicName, subcriptionId);
            var rules = await client.GetRulesAsync();
            if (rules.Any((Func<RuleDescription, bool>) (x => x.Name == "SetSubcriptionRule")))
                await client.RemoveRuleAsync("SetSubcriptionRule");
            if (rules.Any((Func<RuleDescription, bool>) (x => x.Name == "$Default")))
                await client.RemoveRuleAsync("$Default");
            await client.AddRuleAsync(new RuleDescription
            {
                Action = new SqlRuleAction("SET SubcriptionId='" + subcriptionId + "'"),
                Name = "SetSubcriptionRule"
            });
            client.RegisterMessageHandler(serviceBusSubcriber.ProcessTopicMessagesAsync, messageHandlerOptions);
            serviceBusSubcriber.AddOrUpdateDictionary(serviceBusSubcriber._subcriptionClients, key, client);
            key = null;
            messageHandlerOptions = null;
            client = null;
            rules = null;
        }

        public void Dispose()
        {
            if (_subcriptionClients == null || _subcriptionClients.Count <= 0)
                return;
            foreach (var subcriptionClient in _subcriptionClients)
                subcriptionClient.Value.CloseAsync().Wait();
        }

        private async Task ProcessTopicMessagesAsync(Message message, CancellationToken arg2)
        {
            var key = (message.UserProperties["TopicName"] as string) + "_" +
                      (message.UserProperties["SubcriptionId"] as string);
            try
            {
                if (_topicCallbacks.ContainsKey(key))
                {
                    var str = Encoding.UTF8.GetString(message.Body);
                    await _topicCallbacks[key](str, "");
                }
            }
            finally
            {
                await _subcriptionClients[key].CompleteAsync(message.SystemProperties.LockToken);
            }

            key = null;
        }

        private async Task ProcessQueueMessagesAsync(Message message, CancellationToken arg2)
        {
            var queueName = message.UserProperties["QueueName"] as string;
            try
            {
                var str = Encoding.UTF8.GetString(message.Body);
                var userProperty = message.UserProperties["UId"] as string;
                if (queueName != null)
                    if (_queueCallbacks.ContainsKey(queueName))
                        await _queueCallbacks[queueName](str, userProperty);
            }
            finally
            {
                if (queueName != null && _queueClients.ContainsKey(queueName))
                    await _queueClients[queueName].CompleteAsync(message.SystemProperties.LockToken);
            }

            queueName = null;
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private void AddOrUpdateDictionary<T>(IDictionary<string, T> dict, string key, T value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
            else
                dict[key] = value;
        }
    }
}
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Me.One.Core.Messages
{
    public class AzureServiceBusPublisher : IServiceBusPublisher
    {
        private readonly QueueClient _client;
        private readonly TopicClient _topicClient;

        public AzureServiceBusPublisher(string connection, string queueName, string topicName)
        {
            if (!string.IsNullOrEmpty(queueName))
                _client = new QueueClient(connection, queueName);
            if (string.IsNullOrEmpty(topicName))
                return;
            _topicClient = new TopicClient(connection, topicName);
        }

        public async Task SendQueueMessage(IBusinessEvent data, string messageId)
        {
            await SendQueueMessage((object) data, messageId);
        }

        public async Task SendQueueMessage(object data, string messageId)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            await _client.SendAsync(new Message(bytes)
            {
                UserProperties =
                {
                    {
                        "QueueName",
                        (object) _client.QueueName
                    },
                    {
                        "UId",
                        (object) (messageId ?? Guid.NewGuid().ToString())
                    }
                }
            });
        }

        public async Task SendTopicMessage(IBusinessEvent data, string messageId)
        {
            await SendTopicMessage((object) data, messageId);
        }

        public async Task SendTopicMessage(object data, string messageId)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            await _topicClient.SendAsync(new Message(bytes)
            {
                UserProperties =
                {
                    {
                        "TopicName",
                        (object) _topicClient.TopicName
                    },
                    {
                        "UId",
                        (object) (messageId ?? Guid.NewGuid().ToString())
                    }
                }
            });
        }
    }
}
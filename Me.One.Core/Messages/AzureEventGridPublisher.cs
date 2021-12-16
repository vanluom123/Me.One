using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace Me.One.Core.Messages
{
    public class AzureEventGridPublisher : IEventGridPublisher
    {
        private readonly EventGridClient _client;
        private readonly string _endpoint;

        public AzureEventGridPublisher(string endpoint, string key)
        {
            _client = new EventGridClient(new TopicCredentials(key), Array.Empty<DelegatingHandler>());
            _endpoint = endpoint;
        }

        public async Task SendMessage(
            ICollection<IBusinessEvent> data,
            string messageId,
            string subject)
        {
            await SendMessage((ICollection<object>) data.ToList(), messageId, subject);
        }

        public async Task SendMessage(ICollection<object> data, string messageId, string subject)
        {
            await _client.PublishEventsAsync(new Uri(_endpoint).Host, data
                .Select((Func<object, EventGridEvent>) (x => new EventGridEvent
                {
                    Data = x,
                    Id = messageId,
                    EventTime = DateTime.UtcNow,
                    EventType = data.GetType().FullName,
                    Subject = subject,
                    DataVersion = "1.0"
                })).ToList());
        }
    }
}
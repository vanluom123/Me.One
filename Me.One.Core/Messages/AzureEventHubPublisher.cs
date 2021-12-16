using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace Me.One.Core.Messages
{
    public class AzureEventHubPublisher : IEventHubPublisher
    {
        private readonly EventHubClient _client;

        public AzureEventHubPublisher(string strConnection, string hubName)
        {
            _client = EventHubClient.CreateFromConnectionString(new EventHubsConnectionStringBuilder(strConnection)
            {
                EntityPath = hubName
            }.ToString());
        }

        public async Task SendMessage(IBusinessEvent data, string messageId)
        {
            await SendMessage((object) data, messageId);
        }

        public async Task SendMessage(object data, string messageId)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            await _client.SendAsync(new EventData(bytes)
            {
                Properties =
                {
                    {
                        "UId",
                        (object) (messageId ?? Guid.NewGuid().ToString())
                    }
                }
            });
        }
    }
}
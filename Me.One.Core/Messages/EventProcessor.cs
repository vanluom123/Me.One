using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;

namespace Me.One.Core.Messages
{
    public class EventProcessor : BaseEventProcessor, IEventProcessor
    {
        private Func<string, string, Task> _callback;

        public EventProcessor(Func<string, string, Task> callback)
        {
            _callback = callback;
            Processors.Add(this);
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("Processor Shutting Down. Partition '{0}', Reason: '{1}'.", context.PartitionId, reason);
            await Task.CompletedTask;
        }

        public async Task OpenAsync(PartitionContext context)
        {
            await Task.CompletedTask;
        }

        public async Task ProcessErrorAsync(PartitionContext context, System.Exception error)
        {
            Console.WriteLine("Processor Shutting Down. Partition '" + context.PartitionId + "', Reason: '" +
                              error.Message + "'.");
            await Task.CompletedTask;
        }

        public async Task ProcessEventsAsync(
            PartitionContext context,
            IEnumerable<EventData> messages)
        {
            try
            {
                foreach (var message in messages)
                {
                    var utF8 = Encoding.UTF8;
                    var body = message.Body;
                    var array = body.Array;
                    body = message.Body;
                    var offset = body.Offset;
                    body = message.Body;
                    var count = body.Count;
                    var str = utF8.GetString(array, offset, count);
                    object obj = "";
                    message.Properties.TryGetValue("UId", out obj);
                    if (_callback != null)
                    {
                        var eventGridEventList = JsonConvert.DeserializeObject<List<EventGridEvent>>(str);
                        await _callback(eventGridEventList[0].Data.ToString(), eventGridEventList[0].Id);
                    }
                }
            }
            finally
            {
                await context.CheckpointAsync();
            }
        }

        public override void Dispose()
        {
            _callback = null;
        }
    }
}
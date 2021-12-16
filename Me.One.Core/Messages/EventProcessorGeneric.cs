using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Me.One.Core.DependencyInjection;
using Me.One.Core.Exception;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;

namespace Me.One.Core.Messages
{
    public class EventProcessor<T> : BaseEventProcessor, IEventProcessor
        where T : IServiceHandler
    {
        private T _handler;
        private string currentData = "";

        public EventProcessor()
        {
            Processors.Add(this);
        }

        public string Id { get; } = Guid.NewGuid().ToString();

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            await Task.CompletedTask;
        }

        public async Task OpenAsync(PartitionContext context)
        {
            await Task.CompletedTask;
        }

        public async Task ProcessErrorAsync(PartitionContext context, System.Exception error)
        {
            if (_handler != null)
                await _handler.HandleError(new CustomEventHubException
                {
                    Data = currentData,
                    Exception = error
                });
            else
                Console.WriteLine(JsonConvert.SerializeObject(error));
        }

        public async Task ProcessEventsAsync(
            PartitionContext context,
            IEnumerable<EventData> messages)
        {
            try
            {
                currentData = JsonConvert.SerializeObject(messages);
                foreach (var message in messages)
                {
                    var utF8 = Encoding.UTF8;
                    var array = message.Body.Array;
                    var body = message.Body;
                    var offset = body.Offset;
                    body = message.Body;
                    var count = body.Count;
                    var str = utF8.GetString(array, offset, count);
                    object obj = "";
                    message.Properties.TryGetValue("UId", out obj);
                    var scope = ((CoreContainerAdapter) DependencyResolver.Instance).ContainerInstance
                        .BeginLifetimeScope();
                    try
                    {
                        _handler = scope.Resolve<T>();
                        var eventGridEventList = JsonConvert.DeserializeObject<List<EventGridEvent>>(str);
                        await _handler.HandleAsync(eventGridEventList[0].Data.ToString(), eventGridEventList[0].Id);
                    }
                    finally
                    {
                        scope.Dispose();
                    }

                    scope = null;
                }
            }
            finally
            {
                await context.CheckpointAsync();
            }
        }

        public override void Dispose()
        {
            _handler = default;
        }
    }
}
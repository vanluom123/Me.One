// Decompiled with JetBrains decompiler
// Type: Me.One.Core.Messages.EventProcessorFactory
// Assembly: Me.One.Core, Version=3.1.2.1, Culture=neutral, PublicKeyToken=null
// MVID: 90201C9C-A5B5-4CA5-A5F9-D53E3BE35690
// Assembly location: C:\Users\Admin\source\repos\aia-framework-training\Sources\AIA.Training\bin\Release\netcoreapp3.1\Me.One.Core.dll

using System;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs.Processor;

namespace Me.One.Core.Messages
{
    public class EventProcessorFactory : IEventProcessorFactory
    {
        private readonly Func<string, string, Task> _callback;

        public EventProcessorFactory(Func<string, string, Task> callback)
        {
            _callback = callback;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new EventProcessor(_callback);
        }
    }
}
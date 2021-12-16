using System;
using System.Collections.Generic;

namespace Me.One.Core.Messages
{
    public class BaseEventProcessor : IDisposable
    {
        public static List<BaseEventProcessor> Processors = new();

        public virtual void Dispose()
        {
        }
    }
}
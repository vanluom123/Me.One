using System;

namespace Me.One.Core.Messages
{
    public class BaseEvent : IBusinessEvent
    {
        public string Module { get; set; }

        public string EventCode { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public SenderInfo Sender { get; set; }

        public DateTime? Time { get; set; }

        public string TimeZone { get; set; }

        public object Data { get; set; }
    }
}
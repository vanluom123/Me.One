using System;
using Kledex.Events;

namespace Me.One.Core.CQRS.Models
{
    public class BaseEvent : IEvent
    {
        public string ProducerCode { get; set; }

        public string Username { get; set; }

        public string Channel { get; set; }

        public string[] Roles { get; set; }

        public string UserId { get; set; }

        public string Source { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
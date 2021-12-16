using System;

namespace Me.One.Core.Messages
{
    public interface IBusinessEvent
    {
        string Module { get; set; }

        string EventCode { get; set; }

        string Id { get; set; }

        string Name { get; }

        SenderInfo Sender { get; }

        DateTime? Time { get; set; }

        string TimeZone { get; set; }

        object Data { get; set; }
    }
}
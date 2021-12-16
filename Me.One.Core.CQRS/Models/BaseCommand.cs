using System;
using Kledex.Commands;

namespace Me.One.Core.CQRS.Models
{
    public class BaseCommand : ICommand
    {
        public string ProducerCode { get; set; }

        public string Username { get; set; }

        public string Channel { get; set; }

        public string OfficeCode { get; set; }

        public string[] Roles { get; set; }

        public string[] Positions { get; set; }

        public string[] Channels { get; set; }
        public bool? PublishEvents { get; set; }

        public string UserId { get; set; }

        public string Source { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool? ValidateCommand { get; set; }
    }
}
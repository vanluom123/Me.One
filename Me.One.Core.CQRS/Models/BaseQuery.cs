using System.Collections.Generic;

namespace Me.One.Core.CQRS.Models
{
    public class BaseQuery
    {
        public int PageSize { get; set; } = 20;

        public int PageIndex { get; set; } = 1;

        public string OfficeCode { get; set; }

        public string ProducerCode { get; set; }

        public string Username { get; set; }

        public string Channel { get; set; }

        public string[] Roles { get; set; }

        public string[] Positions { get; set; }

        public string[] Channels { get; set; }

        public List<OrderBy> OrderBy { get; set; }
    }
}
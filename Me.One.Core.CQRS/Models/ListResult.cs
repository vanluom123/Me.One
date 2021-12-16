using System.Collections.Generic;

namespace Me.One.Core.CQRS.Models
{
    public class ListResult<T>
    {
        public List<T> Items { get; set; }

        public long TotalRecords { get; set; }
    }
}
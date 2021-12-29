using Demo.Core.CQRS.Result;
using Me.One.Core.CQRS.Models;

namespace Demo.Core.CQRS.Queries
{
    public class GetCourseByStudent : BaseQuery<ListResult<GetCourseResult>>
    {
        public string Id { get; set; }
    }
}
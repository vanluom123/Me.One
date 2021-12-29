using Demo.Core.CQRS.Result;
using Me.One.Core.CQRS.Models;

namespace Demo.Core.CQRS.Queries
{
    public class GetCourse : BaseQuery<GetCourseResult>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
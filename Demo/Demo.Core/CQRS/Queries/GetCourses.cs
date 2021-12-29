using Demo.Core.CQRS.Result;
using Me.One.Core.CQRS.Models;

namespace Demo.Core.CQRS.Queries
{
    public class GetCourses : BaseQuery<ListResult<GetCourseResult>>
    {
    }
}
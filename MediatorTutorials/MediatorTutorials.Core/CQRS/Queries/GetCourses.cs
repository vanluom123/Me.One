using Me.One.Core.CQRS.Models;
using MediatorTutorials.Core.CQRS.Result;

namespace MediatorTutorials.Core.CQRS.Queries
{
    public class GetCourses : BaseQuery<ListResult<GetCourseResult>>
    {
    }
}
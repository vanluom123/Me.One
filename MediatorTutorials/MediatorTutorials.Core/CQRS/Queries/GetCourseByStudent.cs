using Me.One.Core.CQRS.Models;
using MediatorTutorials.Core.CQRS.Result;

namespace MediatorTutorials.Core.CQRS.Queries
{
    public class GetCourseByStudent : BaseQuery<ListResult<GetCourseResult>>
    {
        public string Id { get; set; }
    }
}
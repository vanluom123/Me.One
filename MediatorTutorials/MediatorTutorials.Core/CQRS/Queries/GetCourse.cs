using Me.One.Core.CQRS.Models;
using MediatorTutorials.Core.CQRS.Result;

namespace MediatorTutorials.Core.CQRS.Queries
{
    public class GetCourse : BaseQuery<GetCourseResult>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
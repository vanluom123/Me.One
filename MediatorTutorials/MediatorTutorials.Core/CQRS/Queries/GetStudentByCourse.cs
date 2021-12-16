using Me.One.Core.CQRS.Models;
using MediatorTutorials.Core.CQRS.Result;

namespace MediatorTutorials.Core.CQRS.Queries
{
    public class GetStudentByCourse : BaseQuery<ListResult<GetStudentResult>>
    {
        public string Id { get; set; }
    }
}
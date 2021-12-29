using Demo.Core.CQRS.Result;
using Me.One.Core.CQRS.Models;

namespace Demo.Core.CQRS.Queries
{
    public class GetStudentByCourse : BaseQuery<ListResult<GetStudentResult>>
    {
        public string Id { get; set; }
    }
}
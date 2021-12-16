using System.Threading.Tasks;
using Kledex.Queries;
using Me.One.Core.CQRS.Models;
using MediatorTutorials.Core.Contacts.Business;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.CQRS.Result;

namespace MediatorTutorials.QueryHandler
{
    public class StudentQueryHandler
        : IQueryHandlerAsync<GetStudent, ListResult<GetStudentResult>>,
            IQueryHandlerAsync<GetStudentByCourse, ListResult<GetStudentResult>>
    {
        private readonly IStudentBusiness _studentBusiness;

        public StudentQueryHandler(IStudentBusiness studentBusiness)
        {
            _studentBusiness = studentBusiness;
        }

        public async Task<ListResult<GetStudentResult>> HandleAsync(GetStudent query)
        {
            return await _studentBusiness.GetAllStudents(query);
        }

        public async Task<ListResult<GetStudentResult>> HandleAsync(GetStudentByCourse query)
        {
            return await _studentBusiness.GetStudentByCourse(query);
        }
    }
}
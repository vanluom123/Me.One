using System.Threading.Tasks;
using Me.One.Core.CQRS.Models;
using MediatorTutorials.Core.CQRS.Commands;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.CQRS.Result;

namespace MediatorTutorials.Core.Contacts.Business
{
    public interface IStudentBusiness
    {
        Task<ListResult<GetStudentResult>> GetAllStudents(GetStudent query);
        Task<ListResult<GetStudentResult>> GetStudentByCourse(GetStudentByCourse query);
        Task CreateOrUpdate(CreateOrUpdateStudent command);
    }
}
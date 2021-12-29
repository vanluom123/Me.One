using System.Threading.Tasks;
using Demo.Core.CQRS.Commands;
using Demo.Core.CQRS.Queries;
using Demo.Core.CQRS.Result;
using Me.One.Core.CQRS.Models;

namespace Demo.Core.Contacts.Business
{
    public interface IStudentBusiness
    {
        Task<ListResult<GetStudentResult>> GetAllStudents(GetStudent query);
        Task<ListResult<GetStudentResult>> GetStudentByCourse(GetStudentByCourse query);
        Task CreateOrUpdate(CreateOrUpdateStudent command);
    }
}
using System.Threading.Tasks;
using Me.One.Core.CQRS.Models;
using MediatorTutorials.Core.CQRS.Commands;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.CQRS.Result;

namespace MediatorTutorials.Core.Contacts.Business
{
    public interface ICourseBusiness
    {
        Task Create(CreateCourse command);
        Task<ListResult<GetCourseResult>> GetAllCourses(GetCourses query);
        Task<ListResult<GetCourseResult>> GetListCoursesByStudent(GetCourseByStudent query);
        Task<GetCourseResult> GetCourseById (GetCourse query);
    }
}
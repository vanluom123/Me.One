using System.Threading.Tasks;
using Demo.Core.CQRS.Commands;
using Demo.Core.CQRS.Queries;
using Demo.Core.CQRS.Result;
using Me.One.Core.CQRS.Models;

namespace Demo.Core.Contacts.Business
{
    public interface ICourseBusiness
    {
        Task Create(CreateCourse command);
        Task<ListResult<GetCourseResult>> GetAllCourses(GetCourses query);
        Task<ListResult<GetCourseResult>> GetListCoursesByStudent(GetCourseByStudent query);
        Task<GetCourseResult> GetCourseById (GetCourse query);
    }
}
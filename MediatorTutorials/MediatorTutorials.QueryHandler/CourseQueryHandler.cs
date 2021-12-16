using System.Threading.Tasks;
using Kledex.Queries;
using Me.One.Core.CQRS.Models;
using MediatorTutorials.Core.Contacts.Business;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.CQRS.Result;

namespace MediatorTutorials.QueryHandler
{
    public class CourseQueryHandler
        : IQueryHandlerAsync<GetCourses, ListResult<GetCourseResult>>,
            IQueryHandlerAsync<GetCourseByStudent, ListResult<GetCourseResult>>
    {
        private readonly ICourseBusiness _courseBusiness;

        public CourseQueryHandler(ICourseBusiness courseBusiness)
        {
            _courseBusiness = courseBusiness;
        }

        public async Task<ListResult<GetCourseResult>> HandleAsync(GetCourseByStudent query)
        {
            return await _courseBusiness.GetListCoursesByStudent(query);
        }

        public async Task<ListResult<GetCourseResult>> HandleAsync(GetCourses query)
        {
            return await _courseBusiness.GetAllCourses(query);
        }
    }
}
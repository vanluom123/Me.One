using System.Threading.Tasks;
using Demo.Core.Contacts.Business;
using Demo.Core.CQRS.Queries;
using Demo.Core.CQRS.Result;
using Kledex.Queries;
using Me.One.Core.CQRS.Models;

namespace Demo.QueryHandler
{
    public class CourseQueryHandler
        : IQueryHandlerAsync<GetCourses, ListResult<GetCourseResult>>,
            IQueryHandlerAsync<GetCourseByStudent, ListResult<GetCourseResult>>,
            IQueryHandlerAsync<GetCourse, GetCourseResult>
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

        public async Task<GetCourseResult> HandleAsync(GetCourse query)
        {
            return await _courseBusiness.GetCourseById(query);
        }
    }
}
using Demo.Core.CQRS.Queries;
using Demo.Core.CQRS.Result;
using Demo.Core.Dependencies;
using Kledex.Queries;
using Me.One.Core.CQRS.Models;
using Me.One.Core.DependencyInjection;

namespace Demo.QueryHandler
{
    public class Bootstrapper : BaseDependenciesWireUp
    {
        public Bootstrapper(IRegisterDependencies registerDependencies) : base(registerDependencies)
        {
        }

        public override void WireUp()
        {
            _registerDependencies
                .Register<IQueryHandlerAsync<GetStudent, ListResult<GetStudentResult>>, StudentQueryHandler>();
            _registerDependencies
                .Register<IQueryHandlerAsync<GetStudentByCourse, ListResult<GetStudentResult>>, StudentQueryHandler>();
            _registerDependencies
                .Register<IQueryHandlerAsync<GetCourses, ListResult<GetCourseResult>>, CourseQueryHandler>();
            _registerDependencies
                .Register<IQueryHandlerAsync<GetCourseByStudent, ListResult<GetCourseResult>>, CourseQueryHandler>();
        }
    }
}
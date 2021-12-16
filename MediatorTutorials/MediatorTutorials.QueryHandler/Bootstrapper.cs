using Kledex.Queries;
using Me.One.Core.CQRS.Models;
using Me.One.Core.DependencyInjection;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.CQRS.Result;
using MediatorTutorials.Core.Dependencies;

namespace MediatorTutorials.QueryHandler
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
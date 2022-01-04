using Autofac;
using Demo.Core.CQRS.Queries;
using Demo.Core.CQRS.Result;
using Demo.Core.Dependencies;
using Kledex.Queries;
using Me.One.Core.CQRS.Models;
using Me.One.Core.DependencyInjection;
using System.Reflection;

namespace Demo.QueryHandler
{
    public class Bootstrapper : BaseDependenciesWireUp
    {
        private Bootstrapper(IRegisterDependencies registerDependencies) : base(registerDependencies)
        {
        }

        public static Bootstrapper CreateBootstrapper(IRegisterDependencies registerDependencies)
        {
            return new Bootstrapper(registerDependencies);
        }

        public override void WireUp()
        {
            _registerDependencies.RegisterAssemblyTypes(typeof(StudentQueryHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));

            _registerDependencies.RegisterAssemblyTypes(typeof(CourseQueryHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));
        }
    }
}
using Autofac;
using Demo.Core.CQRS.Commands;
using Demo.Core.Dependencies;
using Kledex.Commands;
using Me.One.Core.DependencyInjection;
using System.Reflection;

namespace Demo.CommandHandler
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

            _registerDependencies.RegisterAssemblyTypes(typeof(CourseCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
            
            _registerDependencies.RegisterAssemblyTypes(typeof(StudentCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
        }
    }
}
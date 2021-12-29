using Demo.Core.CQRS.Commands;
using Demo.Core.Dependencies;
using Kledex.Commands;
using Me.One.Core.DependencyInjection;

namespace Demo.CommandHandler
{
    public class Bootstrapper : BaseDependenciesWireUp
    {
        public Bootstrapper(IRegisterDependencies registerDependencies) : base(registerDependencies)
        {
        }

        public override void WireUp()
        {
            _registerDependencies.Register<ICommandHandlerAsync<CreateCourse>, CourseCommandHandler>();
            _registerDependencies.Register<ICommandHandlerAsync<CreateOrUpdateStudent>, StudentCommandHandler>();
        }
    }
}
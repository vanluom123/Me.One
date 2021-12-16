using Kledex.Commands;
using Me.One.Core.DependencyInjection;
using MediatorTutorials.Core.CQRS.Commands;
using MediatorTutorials.Core.Dependencies;

namespace MediatorTutorials.CommandHander
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
using Me.One.Core.DependencyInjection;

namespace MediatorTutorials.Core.Dependencies
{
    public abstract class BaseDependenciesWireUp : IWireUpDependencies
    {
        protected readonly IRegisterDependencies _registerDependencies;

        protected BaseDependenciesWireUp(IRegisterDependencies registerDependencies)
        {
            _registerDependencies = registerDependencies;
        }

        public abstract void WireUp();
    }
}
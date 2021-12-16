using Me.One.Core.DependencyInjection;
using MediatorTutorials.Core.Contacts.Business;
using MediatorTutorials.Core.Dependencies;

namespace MediatorTutorials.Business
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
            _registerDependencies.Register<IStudentBusiness, StudentBusiness>();
            _registerDependencies.Register<ICourseBusiness, CourseBusiness>();
        }
    }
}
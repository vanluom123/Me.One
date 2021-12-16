using Me.One.Core.DependencyInjection;
using MediatorTutorials.Core.Contacts.Repository;
using MediatorTutorials.Core.Dependencies;

namespace MediatorTutorials.Data
{
    public class Bootstrapper : BaseDependenciesWireUp
    {
        public Bootstrapper(IRegisterDependencies registerDependencies) : base(registerDependencies)
        {
        }

        public static Bootstrapper CreateBootstrapper(IRegisterDependencies registerDependencies)
        {
            return new Bootstrapper(registerDependencies);
        }

        public override void WireUp()
        {
            _registerDependencies.Register<IStudentRepository, StudentRepository>();
            _registerDependencies.Register<ICourseRepository, CourseRepository>();
        }
    }
}
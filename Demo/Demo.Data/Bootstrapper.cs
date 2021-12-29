using Demo.Core.Contacts.Repository;
using Demo.Core.Dependencies;
using Me.One.Core.DependencyInjection;

namespace Demo.Data
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
            _registerDependencies.Register<IStudentRepository, StudentRepository>();
            _registerDependencies.Register<ICourseRepository, CourseRepository>();
        }
    }
}
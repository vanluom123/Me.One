using System.Security.Principal;

namespace Me.One.Core.Exception
{
    public class SecurityException : System.Exception
    {
        public SecurityException()
        {
        }

        public SecurityException(string message)
            : base(message)
        {
        }

        public string PermissionName { get; set; }

        public string RoleName { get; set; }

        public IIdentity User { get; set; }
    }
}
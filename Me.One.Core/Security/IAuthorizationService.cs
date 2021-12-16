using System.Security.Principal;

namespace Me.One.Core.Security
{
    public interface IAuthorizationService
    {
        void CheckAccess(string permissionName, IIdentity user);

        bool TryCheckAccess(string permissionName, IIdentity user);

        void CheckRole(string roleName, IIdentity user);

        bool TryCheckRole(string roleName, IIdentity user);
    }
}
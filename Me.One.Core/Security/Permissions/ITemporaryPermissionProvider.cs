using System.Security.Principal;

namespace Me.One.Core.Security.Permissions
{
    public interface ITemporaryPermissionProvider
    {
        string[] GetTemporaryPermissions(IIdentity identity);
    }
}
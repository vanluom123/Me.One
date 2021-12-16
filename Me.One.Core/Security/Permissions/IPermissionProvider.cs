using System.Collections.Generic;

namespace Me.One.Core.Security.Permissions
{
    public interface IPermissionProvider
    {
        string Name { get; }

        IEnumerable<Permission> GetPermissions();

        PermissionModule GetPermissionModule();
    }
}
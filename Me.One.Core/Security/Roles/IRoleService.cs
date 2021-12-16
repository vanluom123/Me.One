using System.Collections.Generic;
using Me.One.Core.Security.Permissions;

namespace Me.One.Core.Security.Roles
{
    public interface IRoleService
    {
        List<IPermissionProvider> GetPermissionModules();

        Permission GetPermissionByName(string permissionName);
    }
}
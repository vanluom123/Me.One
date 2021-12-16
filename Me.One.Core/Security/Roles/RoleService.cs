using System;
using System.Collections.Generic;
using System.Linq;
using Me.One.Core.DependencyInjection;
using Me.One.Core.Security.Permissions;

namespace Me.One.Core.Security.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IResolveDependencies _resolveDependencies;
        private List<Permission> _listPermission;

        public RoleService(IResolveDependencies resolveDependencies)
        {
            this._resolveDependencies = resolveDependencies;
        }

        public List<IPermissionProvider> GetPermissionModules()
        {
            return _resolveDependencies.ResolveAll<IPermissionProvider>().ToList();
        }

        public Permission GetPermissionByName(string permissionName)
        {
            if (_listPermission == null)
                _listPermission = GetAllPermissions();
            return _listPermission.FirstOrDefault((Func<Permission, bool>) (p => p.Name == permissionName)) ??
                   Permission.Named(permissionName);
        }

        private List<Permission> GetAllPermissions()
        {
            return GetPermissionModules()
                .SelectMany((Func<IPermissionProvider, IEnumerable<Permission>>) (m => m.GetPermissions()))
                .ToList();
        }
    }
}
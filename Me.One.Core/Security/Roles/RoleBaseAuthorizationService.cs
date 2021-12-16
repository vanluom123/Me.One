using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Me.One.Core.DependencyInjection;
using Me.One.Core.Exception;
using Me.One.Core.Security.Permissions;

namespace Me.One.Core.Security.Roles
{
    public class RoleBaseAuthorizationService : IAuthorizationService
    {
        private readonly IResolveDependencies _resolveDependencies;
        private readonly IRoleService _roleService;

        public RoleBaseAuthorizationService(
            IRoleService roleService,
            IResolveDependencies resolveDependencies)
        {
            _roleService = roleService;
            _resolveDependencies = resolveDependencies;
        }

        public void CheckAccess(string permissionName, IIdentity user)
        {
            if (!TryCheckAccess(permissionName, user))
                throw new SecurityException("A security exception occurred in the content management system.")
                {
                    PermissionName = permissionName,
                    User = user
                };
        }

        public void CheckRole(string roleName, IIdentity user)
        {
            if (!TryCheckRole(roleName, user))
                throw new SecurityException("A security exception occurred in the content management system.")
                {
                    RoleName = roleName,
                    User = user
                };
        }

        public bool TryCheckAccess(string permissionName, IIdentity user)
        {
            if (user is ClaimsIdentity claimsIdentity && claimsIdentity.IsAuthenticated)
            {
                if (claimsIdentity.Claims
                    .Where((Func<Claim, bool>) (c =>
                        c.Type == "https://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                    .Select((Func<Claim, string>) (c => c.Value)).ToList().Contains("SuperAdmin"))
                    return true;
                var array1 = PermissionNames(_roleService.GetPermissionByName(permissionName), new List<string>())
                    .Distinct().ToArray();
                var array2 = claimsIdentity.Claims
                    .Where((Func<Claim, bool>) (c =>
                        c.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/permission"))
                    .Select((Func<Claim, string>) (c => c.Value)).ToArray();
                if (CheckingPermissions(array1, array2))
                    return true;
                foreach (var permissionProvider in _resolveDependencies.ResolveAll<ITemporaryPermissionProvider>())
                {
                    var temporaryPermissions = permissionProvider.GetTemporaryPermissions(user);
                    if (CheckingPermissions(array1, temporaryPermissions))
                        return true;
                }
            }

            return false;
        }

        public bool TryCheckRole(string roleName, IIdentity user)
        {
            if (!(user is ClaimsIdentity claimsIdentity) || !claimsIdentity.IsAuthenticated)
                return false;
            var list = claimsIdentity.Claims
                .Where((Func<Claim, bool>) (c =>
                    c.Type == "https://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                .Select((Func<Claim, string>) (c => c.Value)).ToList();
            return list.Contains("SuperAdmin") || list.Contains(roleName);
        }

        private bool CheckingPermissions(string[] grantingNames, string[] permissions)
        {
            foreach (var permission in permissions)
            {
                var possessedName = permission;
                if (grantingNames.Any((Func<string, bool>) (grantingName =>
                    string.Equals(possessedName, grantingName, StringComparison.OrdinalIgnoreCase))))
                    return true;
            }

            return false;
        }

        private static IEnumerable<string> PermissionNames(
            Permission permission,
            ICollection<string> stack)
        {
            if (permission == null || string.IsNullOrWhiteSpace(permission.Name)) yield break;
            yield return permission.Name;
            if (!permission.ImpliedBy.Any()) yield break;
            foreach (var permission1 in permission.ImpliedBy.Where(
                (Func<Permission, bool>) (impliedBy => !stack.Contains(impliedBy.Name))))
            {
                stack.Add(permission.Name);
                foreach (var permissionName in PermissionNames(permission1, stack))
                    yield return permissionName;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IAuthorizationService = Me.One.Core.Security.IAuthorizationService;

namespace Me.One.Core.Web.Authorization
{
    public class RolePermissionHandler : IAuthorizationHandler
    {
        private readonly IAuthorizationService _authorizationService;

        public RolePermissionHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var list = context.PendingRequirements.ToList();
            var flag = true;
            foreach (var requirement in list)
                if (requirement is RoleRequirement roleRequirement)
                {
                    if (!CheckRoles(context.User.Identity, roleRequirement.Roles))
                    {
                        flag = false;
                        break;
                    }

                    context.Succeed(roleRequirement);
                }
                else if (requirement is PermissionRequirement permissionRequirement)
                {
                    if (!CheckPermissions(context.User.Identity, permissionRequirement.Permissions))
                    {
                        flag = false;
                        break;
                    }

                    context.Succeed(permissionRequirement);
                }

            if (!flag)
                context.Fail();
            return Task.CompletedTask;
        }

        protected bool CheckPermissions(IIdentity identity, IEnumerable<string> requirePermissions)
        {
            if (!identity.IsAuthenticated) return false;
            if (requirePermissions is not IList<string> source2)
                source2 = requirePermissions.ToList();
            return source2.Any((Func<string, bool>) (p => _authorizationService.TryCheckAccess(p, identity)));
        }

        protected bool CheckRoles(IIdentity identity, IEnumerable<string> requireRoles)
        {
            if (!identity.IsAuthenticated) return false;
            if (requireRoles is not IList<string> source2)
                source2 = requireRoles.ToList();
            return source2.Any((Func<string, bool>) (r => _authorizationService.TryCheckRole(r, identity)));
        }
    }
}
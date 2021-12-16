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
                if (requirement is RoleRequirement)
                {
                    if (!CheckRoles(context.User.Identity, ((RoleRequirement) requirement).Roles))
                    {
                        flag = false;
                        break;
                    }

                    context.Succeed(requirement);
                }
                else if (requirement is PermissionRequirement)
                {
                    if (!CheckPermissions(context.User.Identity, ((PermissionRequirement) requirement).Permissions))
                    {
                        flag = false;
                        break;
                    }

                    context.Succeed(requirement);
                }

            if (!flag)
                context.Fail();
            return Task.CompletedTask;
        }

        protected bool CheckPermissions(IIdentity identity, IEnumerable<string> requirePermissions)
        {
            if (identity.IsAuthenticated)
            {
                if (!(requirePermissions is IList<string> source2))
                    source2 = requirePermissions.ToList();
                if (source2.Any((Func<string, bool>) (p => _authorizationService.TryCheckAccess(p, identity))))
                    return true;
            }

            return false;
        }

        protected bool CheckRoles(IIdentity identity, IEnumerable<string> requireRoles)
        {
            if (identity.IsAuthenticated)
            {
                if (!(requireRoles is IList<string> source2))
                    source2 = requireRoles.ToList();
                if (source2.Any((Func<string, bool>) (r => _authorizationService.TryCheckRole(r, identity))))
                    return true;
            }

            return false;
        }
    }
}
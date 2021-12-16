using Microsoft.AspNetCore.Authorization;

namespace Me.One.Core.Web.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(params string[] permissions)
        {
            Permissions = permissions;
        }

        public string[] Permissions { get; set; }
    }
}
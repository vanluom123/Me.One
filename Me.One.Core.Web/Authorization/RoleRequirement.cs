using Microsoft.AspNetCore.Authorization;

namespace Me.One.Core.Web.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(params string[] roles)
        {
            Roles = roles;
        }

        public string[] Roles { get; set; }
    }
}
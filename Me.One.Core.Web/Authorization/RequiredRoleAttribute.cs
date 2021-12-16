using Microsoft.AspNetCore.Authorization;

namespace Me.One.Core.Web.Authorization
{
    public class RequiredRoleAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "AIAONE.ROLE:";

        public RequiredRoleAttribute(params string[] roles)
        {
            Policy = "AIAONE.ROLE:" + string.Join(",", roles);
        }
    }
}
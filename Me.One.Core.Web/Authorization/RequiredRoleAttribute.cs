using Microsoft.AspNetCore.Authorization;

namespace Me.One.Core.Web.Authorization
{
    public class RequiredRoleAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "MEONE.ROLE:";

        public RequiredRoleAttribute(params string[] roles)
        {
            Policy = "MEONE.ROLE:" + string.Join(",", roles);
        }
    }
}
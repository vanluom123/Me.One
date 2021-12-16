using Microsoft.AspNetCore.Authorization;

namespace Me.One.Core.Web.Authorization
{
    public class RequiredPermissionAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "AIAONE.PERMISSION:";

        public RequiredPermissionAttribute(params string[] permissions)
        {
            Policy = "AIAONE.PERMISSION:" + string.Join(",", permissions);
        }
    }
}
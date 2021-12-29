using Microsoft.AspNetCore.Authorization;

namespace Me.One.Core.Web.Authorization
{
    public class RequiredPermissionAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "MEONE.PERMISSION:";

        public RequiredPermissionAttribute(params string[] permissions)
        {
            Policy = "MEONE.PERMISSION:" + string.Join(",", permissions);
        }
    }
}
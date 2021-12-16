using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Me.One.Core.Web.Authorization
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override Task<AuthorizationPolicy> GetPolicyAsync(
            string policyName)
        {
            if (policyName.StartsWith("AIAONE.PERMISSION:", StringComparison.OrdinalIgnoreCase))
            {
                var strArray = policyName.Substring("AIAONE.PERMISSION:".Length).Split(',');
                return Task.FromResult(new AuthorizationPolicyBuilder(Array.Empty<string>())
                    .AddRequirements(new PermissionRequirement(strArray)).Build());
            }

            if (!policyName.StartsWith("AIAONE.ROLE:", StringComparison.OrdinalIgnoreCase))
                return base.GetPolicyAsync(policyName);
            var strArray1 = policyName.Substring("AIAONE.ROLE:".Length).Split(',');
            return Task.FromResult(new AuthorizationPolicyBuilder(Array.Empty<string>())
                .AddRequirements(new RoleRequirement(strArray1)).Build());
        }
    }
}
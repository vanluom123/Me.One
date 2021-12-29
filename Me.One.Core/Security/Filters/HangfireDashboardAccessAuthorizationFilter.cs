using System;
using System.Linq;
using System.Security.Claims;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Me.One.Core.Security.Filters
{
    public class HangfireDashboardAccessAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _allowRoleName;

        public HangfireDashboardAccessAuthorizationFilter(string allowRoleName)
        {
            _allowRoleName = allowRoleName;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            if (context.GetHttpContext().User.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
                return false;
            var list = identity.Claims
                .Where((Func<Claim, bool>) (c =>
                    c.Type == "https://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                .Select((Func<Claim, string>) (c => c.Value)).ToList();
            return list.Contains("BackgroundJobMonitorAdmin") || !string.IsNullOrEmpty(_allowRoleName) ||
                   list.Contains(_allowRoleName);
        }
    }
}
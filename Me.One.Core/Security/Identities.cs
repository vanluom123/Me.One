using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Me.One.Core.Security
{
    public sealed class Identities : ClaimsIdentity
    {
        public Identities(IEnumerable<Claim> claims)
        {
            AddClaims(claims);
        }

        public Identities(IIdentity identity)
            : base(identity)
        {
        }

        public string Identity => Email;

        public string CompanyCode => GetClaimValue("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/companycode");

        public string SalesOrgCode =>
            GetClaimValue("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/salesorgcode");

        public string Avatar => GetClaimValue("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/avatar");

        public string Username => GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn");

        public string LastName => GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");

        public string FirstName => GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");

        public string Email => GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

        public string Timezone => GetClaimValue("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/timezone");

        public string Language
        {
            get => GetClaimValue("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/language");
            set => AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/language", value));
        }

        public bool IsSuperAdmin
        {
            get
            {
                bool result;
                return bool.TryParse(
                           GetClaimValue("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/issuperadmin"),
                           out result) &
                       result;
            }
        }

        public string[] Roles =>
            Claims.Where((Func<Claim, bool>) (c =>
                    c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                .Select((Func<Claim, string>) (c => c.Value)).ToArray();

        public string[] Positions =>
            Claims.Where((Func<Claim, bool>) (c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/position"))
                .Select((Func<Claim, string>) (c => c.Value)).ToArray();

        public string[] Channels =>
            Claims.Where((Func<Claim, bool>) (c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/channelcode"))
                .Select((Func<Claim, string>) (c => c.Value)).ToArray();

        private string GetClaimValue(string claimType)
        {
            var first = FindFirst(claimType);
            return first != null ? first.Value : string.Empty;
        }
    }
}
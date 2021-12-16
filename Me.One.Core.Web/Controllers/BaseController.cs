using System.Collections.Generic;
using System.Security.Claims;
using Me.One.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Me.One.Core.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private Identities _identity = new(new List<Claim>());

        public Identities Identity
        {
            get
            {
                if (User.Identity != null && User.Identity.IsAuthenticated)
                    _identity = new Identities(User.Identity);
                return _identity;
            }
        }
    }
}
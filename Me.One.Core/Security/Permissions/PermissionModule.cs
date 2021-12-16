using System;
using System.Collections.Generic;
using System.Linq;

namespace Me.One.Core.Security.Permissions
{
    public class PermissionModule
    {
        public string ModuleName { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }

        public IEnumerable<Permission> VisiblePermissions =>
            Permissions == null ? null : Permissions.Where((Func<Permission, bool>) (p => !p.Hidden));
    }
}
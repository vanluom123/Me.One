using System.Collections.Generic;

namespace Me.One.Core.Security.Permissions
{
    public class Permission
    {
        private string _displayName;

        public Permission()
        {
            ImpliedBy = new List<Permission>();
            Description = string.Empty;
        }

        public string DisplayName
        {
            get => !string.IsNullOrEmpty(_displayName) ? _displayName : Name;
            set => _displayName = value;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool Hidden { get; set; }

        public ICollection<Permission> ImpliedBy { get; set; }

        public bool RequiresOwnership { get; set; }

        public static Permission Named(string name)
        {
            return new()
            {
                Name = name
            };
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Me.One.Core
{
    public abstract class BaseEntity : BaseGenericEntity<string>
    {
        protected BaseEntity()
        {
            TypeName = GetType().Name;
        }

        [Key] [MaxLength(255)] public new virtual string Id { get; set; }

        public virtual DateTime CreatedDateTime { get; set; }

        public virtual DateTime LastUpdatedDateTime { get; set; }

        public virtual int RowVersion { get; set; }

        public virtual bool Deleted { get; set; }

        public virtual string Source { get; set; }

        [MaxLength(255)] public virtual string CreatedBy { get; set; }

        [MaxLength(255)] public virtual string LastUpdatedBy { get; set; }

        public virtual string TypeName { get; }

        public virtual long Ticks { get; set; }
    }
}
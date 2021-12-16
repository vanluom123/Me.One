using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Me.One.Core
{
    public abstract class BaseEntityS : BaseEntity
    {
        protected BaseEntityS()
        {
            TypeName = GetType().Name;
        }

        [Key] [MaxLength(255)] [Column("ID")] public override string Id { get; set; }

        [Column("CREATEDDATETIME")] public override DateTime CreatedDateTime { get; set; }

        [Column("LASTUPDATEDDATETIME")] public override DateTime LastUpdatedDateTime { get; set; }

        [Column("ROWVERSION")] public override int RowVersion { get; set; }

        [Column("DELETED")] public override bool Deleted { get; set; }

        [Column("SOURCE")] public override string Source { get; set; }

        [MaxLength(255)] [Column("CREATEDBY")] public override string CreatedBy { get; set; }

        [MaxLength(255)]
        [Column("LASTUPDATEDBY")]
        public override string LastUpdatedBy { get; set; }

        [Column("TYPENAME")] public override string TypeName { get; }

        [Column("TICKS")] public override long Ticks { get; set; }
    }
}
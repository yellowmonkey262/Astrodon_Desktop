using Astrodon.Data.Base;
using Astrodon.Data.SupplierData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.BankData
{
    [Table("Bank")]
    public class Bank:DbEntity
    {
        [Index("IDX_Bank", Order = 0, IsUnique = true)]
        [Required]
        [MaxLength(200)]
        public virtual string Name { get; set; }

        [MaxLength(200)]
        public virtual string BranchName { get; set; }

        [MaxLength(200)]
        public virtual string BranchCode { get; set; }

        public virtual bool IsActive { get; set; }

        [NotMapped]
        public string ActiveString { get { return IsActive ? "Yes" : "No"; } }

        public virtual ICollection<BankAudit> AuditRecords { get; set; }

        public virtual ICollection<SupplierBuilding> SupplierBuildings { get; set; }
    }
}

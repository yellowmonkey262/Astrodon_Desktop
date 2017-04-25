using Astrodon.Data.BankData;
using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace Astrodon.Data.SupplierData
{

    [Table("SupplierBuilding")]
    public class SupplierBuilding:DbEntity
    {
        [Index("IDX_SupplierBuilding", Order = 0, IsUnique = true)]
        public virtual int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [Index("IDX_SupplierBuilding", Order = 1,IsUnique =true)]
        public virtual int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        #region Banking Details

        public virtual int BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual Bank Bank { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual string BranchName { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual string BranceCode { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual string AccountNumber { get; set; }

        #endregion

        public virtual ICollection<SupplierBuildingAudit> AuditRecords { get; set; }

     }
}

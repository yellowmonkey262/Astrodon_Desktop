using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.SupplierData
{
    [Table("Supplier")]
    public class Supplier : DbEntity
    {
        [MaxLength(200)]
        [Required]
        public virtual string CompanyName { get; set; }

        [MaxLength(200)]
        public virtual string CompanyRegistration { get; set; }

        [MaxLength(200)]
        public virtual string VATNumber { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual string ContactPerson { get; set; }

        public virtual string EmailAddress { get; set; }

        [MaxLength(200)]
        public virtual string ContactNumber { get; set; }

     

        #region Blacklisted

        public virtual bool BlackListed { get; set; }

        public virtual string BlackListReason { get; set; }

        public virtual int? BlacklistedUserId { get; set; }
        [ForeignKey("BlacklistedUserId")]
        public virtual tblUser BlacklistedUser { get; set; }

        #endregion

        public virtual ICollection<MaintenanceData.Maintenance> Maintenance { get; set; }
        public virtual ICollection<SupplierBuilding> Buildings { get; set; }
        public virtual ICollection<SupplierAudit> AuditRecords { get; set; }
        public virtual ICollection<tblRequisition> Requisitions { get; set; }
    }
}

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

        #region Banking Details

        [MaxLength(200)]
        [Required]
        public virtual string BankName { get; set; }

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

        #region Blacklisted

        public virtual bool BlackListed { get; set; }

        public virtual string BlackListReason { get; set; }
        
        #endregion

        public virtual ICollection<MaintenanceData.Maintenance> Maintenance { get; set; }

        public virtual ICollection<SupplierAudit> AuditRecords { get; set; }
        public virtual ICollection<tblRequisition> Requisitions { get; set; }
    }
}

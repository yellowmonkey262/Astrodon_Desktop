namespace Astrodon.Data
{
    using MaintenanceData;
    using RequisitionData;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRequisition")]
    public partial class tblRequisition
    {
        public int id { get; set; }

        public int userID { get; set; }

        public DateTime trnDate { get; set; }

        public int building { get; set; }

        [Required]
        [StringLength(50)]
        public string account { get; set; }

        public string ledger { get; set; }

        [NotMapped]
        public string LedgerAccountNumber
        {
            get
            {
                if (String.IsNullOrWhiteSpace(ledger))
                    return ledger;
                if (!ledger.Contains(":"))
                    return ledger;

                return ledger.Substring(0, ledger.IndexOf(":"));
            }
        }

        [Required]
        [StringLength(50)]
        public string reference { get; set; }

        public string payreference { get; set; }

        [StringLength(50)]
        public string contractor { get; set; }

        [Column(TypeName = "ntext")]
        public string notes { get; set; }

        public decimal amount { get; set; }

        public bool processed { get; set; }

        public bool paid { get; set; }

        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual SupplierData.Supplier Supplier { get; set; }

        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }

        [Index("IDX_tblRequisitionPastelLink", Order = 0)]
        public int? PastelLedgerAutoNumber { get; set; } //linked pastel auto number

        [Index("IDX_tblRequisitionPastelLink", Order = 1)]
        [MaxLength(200)]
        public string PastelDataPath { get; set; }

        public virtual ICollection<RequisitionDocument> Documents { get; set; }
    }
}

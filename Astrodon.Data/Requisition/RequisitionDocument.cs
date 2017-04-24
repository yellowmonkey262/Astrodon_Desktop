using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace Astrodon.Data.MaintenanceData
{
    [Table("RequisitionDocument")]
    public class RequisitionDocument : DbEntity
    {
        [Index("IDX_RequisitionDocument")]
        public virtual int RequisitionId { get; set; }
        [ForeignKey("RequisitionId")]
        public virtual tblRequisition Requisition { get; set; }

        [MaxLength(500)]
        [Required]
        public virtual string FileName { get; set; }

        public byte[] FileData { get; set; }

        public virtual bool IsInvoice { get; set; }
    }
}

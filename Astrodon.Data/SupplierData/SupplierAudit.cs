using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.SupplierData
{
    [Table("SupplierAudit")]
    public class SupplierAudit : DbEntity
    {
        [Index("IDX_SupplierAudit", Order = 0)]
        public virtual int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [Index("IDX_SupplierAudit", Order = 1)]
        public virtual DateTime AuditTimeStamp { get; set; }

        public virtual int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual tblUser User { get; set; }

        public virtual string FieldName { get; set; }

        public virtual string OldValue { get; set; }

        public virtual string NewValue { get; set; }
    }
}

using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.DebitOrder
{
    [Table("DebitOrderDocument")]
    public class DebitOrderDocument:DbEntity
    {
        [Index("IDX_DebitOrderDocument", Order = 0, IsUnique = true)]
        public virtual int CustomerDebitOrderId { get; set; }
        [ForeignKey("CustomerDebitOrderId")]
        public virtual CustomerDebitOrder CustomerDebitOrder { get; set; }

        [Index("IDX_DebitOrderDocument", Order = 1, IsUnique = true)]
        public virtual DebitOrderDocumentType DocumentType { get; set; }

        [Required]
        public string FileName { get; set; }

        public byte[] FileData { get; set; }
    }
}

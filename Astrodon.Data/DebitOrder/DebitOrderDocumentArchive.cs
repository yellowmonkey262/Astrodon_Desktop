using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.DebitOrder
{
    [Table("DebitOrderDocumentArchive")]
    public class DebitOrderDocumentArchive : DbEntity
    {
        [Index("IDX_DDebitOrderDocumentArchive", Order = 0)]
        public virtual int CustomerDebitOrderArchiveId { get; set; }
        [ForeignKey("CustomerDebitOrderArchiveId")]
        public virtual CustomerDebitOrderArchive CustomerDebitOrderArchive { get; set; }

        [Index("IDX_DDebitOrderDocumentArchive", Order = 1)]
        public virtual DebitOrderDocumentType DocumentType { get; set; }

        [Required]
        public string FileName { get; set; }

        public byte[] FileData { get; set; }
    }
}

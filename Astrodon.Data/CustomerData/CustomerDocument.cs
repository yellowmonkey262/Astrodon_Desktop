using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.CustomerData
{
    [Table("CustomerDocument")]
    public class CustomerDocument : DbEntity
    {
        [Index("IDX_CustomerDocument",Order =0)]
        public virtual int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [Index("IDX_MaintenanceDocument", Order = 0)]
        public virtual int CustomerDocumentTypeId { get; set; }
        [ForeignKey("CustomerDocumentTypeId")]
        public virtual CustomerDocumentType CustomerDocumentType { get; set; }

        [MaxLength(500)]
        [Required]
        public virtual string FileName { get; set; }

        [MaxLength(500)]
        [Required]
        public virtual string Notes { get; set; }


        public byte[] FileData { get; set; }

        public DateTime Uploaded { get; set; }

        public virtual int UploadedUserId { get; set; }
        [ForeignKey("UploadedUserId")]
        public virtual tblUser UploadedUser { get; set; }

    }
}

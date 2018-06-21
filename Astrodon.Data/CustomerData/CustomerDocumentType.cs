using Astrodon.Data.Base;
using Astrodon.Data.SupplierData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.CustomerData
{
    [Table("CustomerDocumentType")]
    public class CustomerDocumentType: DbEntity
    {
        [Index("IDX_CustomerDocumentType", Order = 0, IsUnique = true)]
        [Required]
        [MaxLength(200)]
        public virtual string Name { get; set; }

        public virtual bool IsActive { get; set; }

    }
}

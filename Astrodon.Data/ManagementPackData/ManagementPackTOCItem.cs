using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.ManagementPackData
{
    [Table("ManagementPackTOCItem")]
    public class ManagementPackTOCItem : DbEntity
    {
        [Required]
        [MaxLength(200)]
        [Index("IDX_ManagementPackTOCItem",IsUnique =true)]
        public string Description { get; set; }
    }
}

using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace Astrodon.Data.MaintenanceData
{
    [Table("MaintenanceDocument")]
    public class MaintenanceDocument:DbEntity
    {
        [Index("IDX_MaintenanceDocument")]
        public virtual int MaintenanceId { get; set; }
        [ForeignKey("MaintenanceId")]
        public virtual Maintenance Maintenance { get; set; }

        [MaxLength(500)]
        [Required]
        public virtual string FileName { get; set; }

        public byte[] FileData { get; set; }

    }
}

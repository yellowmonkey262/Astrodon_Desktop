using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.MaintenanceData
{
    [Table("BuildingMaintenanceConfiguration")]
    public class BuildingMaintenanceConfiguration : DbEntity
    {
        [Index("UIDX_BuildingMaintenanceConfiguration",IsUnique =true,Order =0)]
        public virtual int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [MaxLength(200)]
        [Index("UIDX_BuildingMaintenanceConfiguration", IsUnique = true, Order = 1)]
        [Required]
        public virtual string PastelAccountNumber { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual string Name { get; set; }

        public MaintenanceClassificationType MaintenanceClassificationType { get; set; }

        public virtual ICollection<Maintenance> MaintenanceItems { get; set; }

    }
}

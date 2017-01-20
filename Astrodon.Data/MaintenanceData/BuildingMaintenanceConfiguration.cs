using Astradon.Data.Utility;
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

        public virtual string PastelAccountName { get; set; }

        [NotMapped]
        public string PastelAccount { get { return PastelAccountNumber + " - " + PastelAccountName; } }

        [MaxLength(200)]
        [Required]
        public virtual string Name { get; set; }

        public virtual MaintenanceClassificationType MaintenanceClassificationType { get; set; }

        [NotMapped]
        public string Classification
        {
            get
            {
                return NameSplitting.SplitCamelCase(MaintenanceClassificationType);
            }
        }

        public virtual ICollection<Maintenance> MaintenanceItems { get; set; }

    }
}

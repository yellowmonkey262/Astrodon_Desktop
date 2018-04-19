using Astradon.Data.Utility;
using Astrodon.Data.BankData;
using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.MaintenanceData
{
    [Table("BuildingUnit")]
    public class BuildingUnit : DbEntity
    {
        [Index("UIDX_BuildingUnit", IsUnique = true, Order = 0)]
        public virtual int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [MaxLength(200)]
        [Index("UIDX_BuildingUnit", IsUnique = true, Order = 1)]
        [Required]
        public virtual string UnitNo { get; set; }
        
        public decimal SquareMeters { get; set; }

        public decimal AdditionalInsurance { get; set; }

        public decimal PQRating { get; set; }

        public decimal UnitPremium { get; set; }

        public decimal AdditionalPremium { get; set; }

        public bool BondOriginatorInterestNoted { get; set; }

        public virtual int? BondOriginatorId { get; set; }
        [ForeignKey(nameof(BondOriginatorId))]
        public virtual BondOriginator BondOriginator { get; set; }

        [MaxLength(1500)]
        public string Notes { get; set; }

      
    }
}

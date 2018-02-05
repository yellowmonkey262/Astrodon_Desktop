using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.CustomerData
{
    [Table("Customer")]
    public class Customer : DbEntity
    {
        [Index("UIDX_CustomerUnit", IsUnique = true, Order = 0)]
        public virtual int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [MaxLength(200)]
        [Required]
        [Index("UIDX_CustomerUnit", IsUnique = true, Order = 1)]
        public virtual string AccountNumber { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsTrustee { get; set; }
        public DateTime Created { get; set; }
    }
}

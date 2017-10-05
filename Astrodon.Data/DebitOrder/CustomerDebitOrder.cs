using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.DebitOrder
{
    [Table("CustomerDebitOrder")]
    public class CustomerDebitOrder:
        DbEntity
    {
        [Index("UIDX_CustomerDebitOrder", IsUnique = true, Order = 0)]
        public virtual int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual tblBuilding Building { get; set; }

        [MaxLength(200)]
        [Index("UIDX_CustomerDebitOrder", IsUnique = true, Order = 1)]
        [Required]
        public virtual string CustomerCode { get; set; }

        [Required]
        public virtual string BranchCode { get; set; }

        public virtual AccountTypeType AccountType { get; set; }

        [Required]
        public virtual string AccountNumber { get; set; }

        public virtual DebitOrderDayType DebitOrderCollectionDay { get; set; }

        public virtual ICollection<DebitOrderDocument> Documents { get; set; }
    }
}


//m.CustomerCode, m.CustomerDesc
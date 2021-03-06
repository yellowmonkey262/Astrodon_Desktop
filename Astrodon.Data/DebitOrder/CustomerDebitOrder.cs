﻿using Astrodon.Data.BankData;
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


        #region Banking Details

        public virtual int BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual Bank Bank { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual string BranceCode { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual string AccountNumber { get; set; }

        public virtual AccountTypeType AccountType { get; set; }

        #endregion

        public virtual DebitOrderDayType DebitOrderCollectionDay { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual ICollection<DebitOrderDocument> Documents { get; set; }

        public virtual int LastUpdatedByUserId { get; set; }
        [ForeignKey("LastUpdatedByUserId")]
        public virtual tblUser UserUpdate { get; set; }

        public virtual DateTime LastUpdateDate { get; set; }

        public virtual bool IsDebitOrderFeeDisabled { get; set; }

        public virtual DateTime? DebitOrderCancelDate { get; set; }
        public virtual bool DebitOrderCancelled { get; set; }

        [DataType(DataType.Currency)]
        public virtual decimal MaxDebitAmount { get; set; }
    }
}


//m.CustomerCode, m.CustomerDesc
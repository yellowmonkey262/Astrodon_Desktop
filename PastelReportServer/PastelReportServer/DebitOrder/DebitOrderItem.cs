using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.Data.DebitOrder;
using System.Runtime.Serialization;

namespace Astrodon.DebitOrder
{
    [DataContract]
    public class DebitOrderItem
    {
        [DataMember]
        public int BuildingId { get; set; }
        [DataMember]
        public bool IsDebitOrderFeeDisabledOnBuilding { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public AccountTypeType AccountTypeId { get; set; }
        [DataMember]
        public string BranchCode { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public DateTime CollectionDay { get; set; }
        [DataMember]
        public DebitOrderDayType DebitOrderCollectionDay { get; set; }

        [DataMember]
        public decimal DebitOrderFee { get; set; }

        [DataMember]
        public decimal LevyRollDue { get; set; }

        [DataMember]
        public decimal Payments { get; set; }

        [DataMember]
        public decimal AmountDue { get; set; }

        [DataMember]
        public bool DebitOrderFeeDisabled { get; set; }


        [DataMember]
        public DateTime? DebitOrderCancelDate { get; set; }

        [DataMember]
        public bool DebitOrderCancelled { get; set; }

        public bool DebitOrderActive
        {
            get
            {
                if (DebitOrderCancelled == false)
                    return true;

                if (DebitOrderCancelDate == null)
                    return true;

                return DateTime.Today <= DebitOrderCancelDate.Value.Date;
            }
        }

        #region ExportField
        public string SupplierId { get { return string.Empty; } }
        public string Reference { get { return "D/" + CustomerCode; } }
        public string SupplierName { get { return "ASTRODON"; } }
        public string Holnes { get { return CustomerCode + " " + CustomerName; } }
        public string Description { get { return "ASTRODON"; } }

        public decimal ExportDebitOrderFee
        {
            get
            {
                if (IsDebitOrderFeeDisabledOnBuilding || DebitOrderFeeDisabled)
                    return 0;
                else
                    return DebitOrderFee;
            }
        }

        public decimal CollectionAmount
        {
            get
            {
                var amount = AmountDue + ExportDebitOrderFee;
                if (MaxDebitOrderAmount >= 0 && MaxDebitOrderAmount < amount)
                    return MaxDebitOrderAmount;

                return amount;
            }
        }

        public string AccountType { get { return ((int)AccountTypeId).ToString(); } }

        #endregion

        [DataMember]
        public decimal MaxDebitOrderAmount { get; set; }

    }
}

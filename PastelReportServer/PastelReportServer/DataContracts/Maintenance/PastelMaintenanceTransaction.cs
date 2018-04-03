using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Astrodon.DataContracts.Maintenance
{
    [DataContract]
    public class PastelMaintenanceTransaction: PervasiveItem
    {
        public PastelMaintenanceTransaction()
        {

        }

        public PastelMaintenanceTransaction(DataRow row,  string dataPath)
        {
            AutoNumber = ReadInt(row, "AutoNumber");
            TransactionDate = ReadDate(row, "TransactionDate");
            Account = ReadString(row, "Account");
            AccountName = ReadString(row, "AccountName");
            Reference = ReadString(row, "Reference");
            Description = ReadString(row, "Description");
            Amount = ReadDecimal(row, "Amount");
            DataPath = dataPath;
            AccountType =  "OWN";
        }

        [DataMember]
        public int AutoNumber { get; set; }

        [DataMember]
        public DateTime TransactionDate { get; set; }

        [DataMember]
        public string Account { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public bool TrustAccount { get; set; }

        [DataMember]
        public string AccountType { get; set; }

        [DataMember]
        public string DataPath { get; set; }
    }
}

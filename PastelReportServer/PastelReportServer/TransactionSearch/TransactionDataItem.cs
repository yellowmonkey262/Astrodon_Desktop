using Astrodon.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Astrodon.TransactionSearch
{
    [DataContract]
    public class TransactionDataItem : PervasiveDataItem
    {
        public TransactionDataItem(DataRow row,string buildingPath)
        {
            BuildingPath = buildingPath;
            AccountNumber = (string)row["AccNumber"];
            LinkAccount = (string)row["LinkAcc"];
            Refrence = (string)row["Refrence"];
            Description = (string)row["Description"];
            TransactionDate = (DateTime)row["DDate"];
            Amount = ReadDecimal(row, "Amount");
        }

        public TransactionDataItem()
        {

        }

        [DataMember]
        public string BuildingPath { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string LinkAccount { get; set; }
        [DataMember]
        public DateTime TransactionDate { get; set; }
        [DataMember]
        public string Refrence { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Amount { get; set; }

    }
}

//AccNumber,LinkAcc,DDate,Refrence,Amount,Description
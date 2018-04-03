using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.DataContracts
{
    public class PaymentTransaction : PervasiveItem
    {
        public PaymentTransaction()
        {

        }

        public PaymentTransaction(DataRow row, string dataPath)
        {
            AutoNumber = ReadInt(row, "AutoNumber");
            TransactionDate = ReadDate(row, "TransactionDate");
            LedgerAccount = ReadString(row, "LedgerAccount");
            LedgerAccountName = ReadString(row, "LedgerAccountName");
            Reference = ReadString(row, "Reference");
            Description = ReadString(row, "Description");
            Amount = ReadDecimal(row, "Amount");
            DataPath = dataPath;
            AccountType =  "OWN";
        }

        public int AutoNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string LedgerAccount { get; set; }
        public string LedgerAccountName { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string AccountType { get; set; }
        public string DataPath { get; set; }
    }
}

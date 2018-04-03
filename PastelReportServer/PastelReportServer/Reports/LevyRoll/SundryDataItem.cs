using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.LevyRoll
{
    public class SundryDataItem : PervasiveDataItem
    {
        public SundryDataItem(DataRow row)
        {
            AccountNumber = (string)row["AccNumber"];
            AccountName = (string)row["AccDesc"];
            CustomerNumber = (string)row["CustomerCode"];
            CustomerName = (string)row["CustomerDesc"];
            TransactionDate = (DateTime)row["DDate"];
            Description = (string)row["Description"];
            Amount = ReadDecimal(row, "Amount");
        }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateStr { get { return FormatDate(TransactionDate); } }

        public string Description { get; set; }

        public decimal Amount { get; set; }
        public string AmountStr { get { return FormatCurrency(Amount); } }


    }
}

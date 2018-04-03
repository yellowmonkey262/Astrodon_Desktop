using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.RequisitionBatch
{
    public class RequisitionBatchReportDataItem : ReportDataBase
    {
        public DateTime Created { get; set; }
        public int Number { get; set; }
        public string Bank { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string SupplierName { get; set; }
        public string TrustAccount { get; set; }
        public string Ledger
        {
            get
            {
                if (String.IsNullOrWhiteSpace(LedgerAccount))
                    return LedgerAccount;
                if (!LedgerAccount.Contains(":"))
                    return LedgerAccount;

                string temp = LedgerAccount.Substring(0, LedgerAccount.IndexOf(":"));
                if(!string.IsNullOrWhiteSpace(temp) && temp.Length > 4)
                {
                    temp = temp.Insert(4, "/");
                }
                return temp;
            }
        }
        public string LedgerAccount { get; set; }
        public decimal Amount { get; set; }
        public string SupplierReference { get; set; }
        public string InvoiceNumber { get; set; }
        public string Abbr { get;  set; }

        public bool UseNedbankCSV { get;  set; }
        public string CSVDescription {  get { return UseNedbankCSV ? "CSV File Payments" : "Manual Payments"; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.SupplierReport
{
    public class SupplierReportDataItem : ReportDataBase
    {
        public int SupplierId { get; set; }

        public string BlackList { get;  set; }
        public string CompanyName { get;  set; }
        public string ContactPerson { get;  set; }
        public string Email { get;  set; }
        public DateTime LastUsed { get;  set; }
        public string Phone { get;  set; }
        public int Projects { get;  set; }
        public string Registration { get;  set; }
        public string Building { get;  set; }
        public string BankName { get;  set; }
        public string AccountNumber { get;  set; }
        public decimal Amount { get;  set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.MonthlyReport
{
    public class MonthlyReportItem
    {
        public string Building { get; set; }
        public int BuildingId { get; set; }
        public string Code { get; set; }
        public DateTime FinancialPeriod { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; internal set; }
    }
}

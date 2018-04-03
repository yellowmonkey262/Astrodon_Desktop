using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.InsuranceSchedule
{
    public class InsuranceScheduleDataItem : ReportDataBase
    {
        public string Unit { get; set; }
        public string PQPersentage { get; set; }
        public string AdditionalCost { get; set; }
        public string UnitCost { get; set; }
        public string TotalCost { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class MonthReport
    {
        public String Building { get; set; }

        public String Code { get; set; }

        public String User { get; set; }

        public String finPeriod
        {
            get
            {
                return FinDate.ToString("yyyy-MM");
            }
        }

        public String prcDate
        {
            get
            {
                return CompletedDate == null ? "" : CompletedDate.Value.ToString("yyyy-MM-dd");
            }
        }

        public DateTime? CompletedDate { get; set; }
        public DateTime FinDate { get; set; }
    }
}
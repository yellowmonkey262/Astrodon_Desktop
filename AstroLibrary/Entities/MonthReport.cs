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

        public string Period { get; set; }

        public void CalculatePeriod()
        {
            if (!String.IsNullOrWhiteSpace(this.Period))
            {
                _PeriodCalculated = true;
                var intPeriod = Convert.ToInt32(this.Period);

                int month = 2; //feb
                for (int x = 0; x < intPeriod; x++)
                {
                    month++;
                    if (month > 12)
                        month = 1;
                }

                _FinancialPeriodEnd = new DateTime(DateTime.Now.Year + 1, month, 1);
                _FinancialPeriodStart = _FinancialPeriodEnd.AddMonths(-11);
                _FinancialPeriodEnd = _FinancialPeriodEnd.AddMonths(1).AddDays(-1);

                if (_FinancialPeriodStart > DateTime.Today)
                {
                    _FinancialPeriodStart = _FinancialPeriodStart.AddYears(-1);
                    _FinancialPeriodEnd = _FinancialPeriodEnd.AddYears(-1);
                }
            }
        }

        private bool _PeriodCalculated = false;

        private DateTime _FinancialPeriodStart;
        public DateTime FinancialPeriodStart
        {
            get
            {
                if (!_PeriodCalculated)
                    CalculatePeriod();
                return _FinancialPeriodStart;
            }

        }

        private DateTime _FinancialPeriodEnd;
        public DateTime FinancialPeriodEnd
        {
            get
            {
                if (!_PeriodCalculated)
                    CalculatePeriod();
                return _FinancialPeriodEnd;
            }

        }

        public string YearEnd
        {
            get
            {
                return FinancialPeriodStart.ToString("MMM yyyy") + " - " + FinancialPeriodEnd.ToString("MMM yyyy");
            }
        }
    }
}
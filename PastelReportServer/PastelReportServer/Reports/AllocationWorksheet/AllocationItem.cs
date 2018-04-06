using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.AllocationWorksheet
{
    class AllocationItem
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int Priority { get; set; }

        public string BuildingCode { get; set; }

        public int BuildingId { get; set; }

        public string BuildingName { get; set; }

        public DateTime OrderDate { get; set; }

        public string Reason { get; set; }

        public DateTime? ReasonDate { get; set; }

        public string AllocationReason
        {
            get
            {
                string result = "";
                if (ReasonDate != null)
                     result = Reason + " " + ReasonDate.Value.ToString("yyyy/MM/dd");
                else
                    result = Reason + " no reason date";

                if (FinancialStartDate != null)
                    result = result + " Financial Start: " + FinancialStartDate.Value.ToString("yyyy/MM/dd");


                if (FinancialEndDate != null)
                    result = result + " Financial End: " + FinancialEndDate.Value.ToString("yyyy/MM/dd");

                return result;
            }
        }

        public DateTime? FinancialStartDate { get;  set; }
        public DateTime? FinancialEndDate { get; set; }


        public bool IsFinancialReady
        {
            get
            {
                if (ReasonDate == null)
                    return true;

                bool canSend = true;
                var checkResonDateMax = new DateTime(ReasonDate.Value.Year, ReasonDate.Value.Month, 1).AddMonths(1).AddDays(-1);

                if (FinancialStartDate != null && ReasonDate != null && FinancialStartDate > checkResonDateMax)
                {                                          //31-03-2018                          01-03-2018
                    Console.WriteLine("Cansend false 1 " + FinancialStartDate.ToString() + " " + ReasonDate.ToString());
                    canSend = false;
                }

                if (FinancialEndDate != null && ReasonDate != null && FinancialEndDate < ReasonDate)
                {
                    Console.WriteLine("Cansend false 2 " + FinancialEndDate.ToString() + ReasonDate.ToString());
                    canSend = false;
                }
                return canSend;

            }
        }

        public DateTime PeriodStart { get; internal set; }
        public DateTime PeriodEnd { get; internal set; }
        public double YearEndDays { get; internal set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.MaintenanceReport
{
    public class PervasiveAccount:PervasiveDataItem
    {
        public PervasiveAccount(DataRow row, int period, DateTime dt)
        {
            AccNumber = (string)row["AccNumber"];
            ClosingBalance = CalcClosing(period, row);
            Budget = CurrentBudget(period, row);
            BudgetAvailable = CalcBudget(period, row);
            PeriodMonth = new DateTime(dt.Year, dt.Month,1);
        }

        public DateTime PeriodMonth { get; private set; }
        public string AccNumber { get; private set; }
        public decimal ClosingBalance { get; private set; }
        public decimal Budget { get; private set; }
        public decimal BudgetAvailable { get; private set; }

        private decimal CalcClosing(int period, DataRow row)
        {
            decimal sumLast13 = 0;
            for (int x = 1; x <= 13; x++)
            {
                sumLast13 += ReadDecimal(row, "BalanceLast" + x.ToString().PadLeft(2, '0'));
            }
            if (period > 100)
            {
                var fld = period - 100;

                for (int x = 1; x <= fld; x++)
                {
                    sumLast13 += ReadDecimal(row, "BalanceThis" + x.ToString().PadLeft(2, '0'));
                }
            }
            else
            {
                sumLast13 = 0;
                var fld = period;
                for (int x = 1; x <= fld; x++)
                {
                    sumLast13 += ReadDecimal(row, "BalanceLast" + x.ToString().PadLeft(2, '0'));
                }
            }
            return sumLast13;
        }

        private decimal CalcBudget(int period, DataRow row)
        {
            //using equals instead of += to find the budget for the month and not the total available budget
            decimal sumLast13 = 0;
            for (int x = 1; x <= 13; x++)
            {
                sumLast13 = ReadDecimal(row, "BudgetLast" + x.ToString().PadLeft(2, '0'));
            }
            if (period > 100)
            {
                var fld = period - 100;

                for (int x = 1; x <= fld; x++)
                {
                    sumLast13 = ReadDecimal(row, "BudgetThis" + x.ToString().PadLeft(2, '0'));
                }
            }
            else
            {
                sumLast13 = 0;
                var fld = period;
                for (int x = 1; x <= fld; x++)
                {
                    sumLast13 = ReadDecimal(row, "BudgetLast" + x.ToString().PadLeft(2, '0'));
                }
            }
            return sumLast13;
        }

        private decimal CurrentBudget(int period, DataRow row)
        {
            //using equals instead of += to find the budget for the month and not the total available budget
            decimal sumLast13 = 0;
            for (int x = 1; x <= 13; x++)
            {
                sumLast13 = ReadDecimal(row, "BudgetLast" + x.ToString().PadLeft(2, '0'));
            }
            if (period > 100)
            {
                var fld = period - 100;

                for (int x = 1; x <= fld; x++)
                {
                    sumLast13 = ReadDecimal(row, "BudgetThis" + x.ToString().PadLeft(2, '0'));
                }
            }
            else
            {
                sumLast13 = 0;
                var fld = period;
                for (int x = 1; x <= fld; x++)
                {
                    sumLast13 = ReadDecimal(row, "BudgetLast" + x.ToString().PadLeft(2, '0'));
                }
            }
            return sumLast13;
        }
    }
}

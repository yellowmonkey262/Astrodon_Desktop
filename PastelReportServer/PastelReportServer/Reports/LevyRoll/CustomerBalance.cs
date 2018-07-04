using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.LevyRoll
{
    public class CustomerBalance : PervasiveDataItem
    {
        private DataRow _Row;

        public CustomerBalance(DataRow row)
        {
            _Row = row;

            CustomerCode = (string)row["CustomerCode"];
            CustomerDesc = (string)row["CustomerDesc"];

        }

        public decimal CalcOpening(int period)
        {
            DataRow row = _Row;

            decimal sumLast13 = 0;
            for (int x = 1; x <= 13; x++)
            {
                sumLast13 += ReadDecimal(row, "BalanceLast" + x.ToString().PadLeft(2, '0'));
            }
            if (period > 100)
            {
                var fld = period - 100;
                if (fld == 1)
                    return sumLast13;

                for (int x = 1; x < fld; x++)
                {
                    sumLast13 += ReadDecimal(row, "BalanceThis" + x.ToString().PadLeft(2, '0'));
                }
            }
            else
            {
                sumLast13 = 0;
                var fld = period;
                if (fld == 1)
                    return sumLast13;

                for (int x = 1; x < fld; x++)
                {
                    sumLast13 += ReadDecimal(row, "BalanceLast" + x.ToString().PadLeft(2, '0'));
                }
            }
            return sumLast13;
        }

        public decimal CalcClosing(int period)
        {
            DataRow row = _Row;

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

        public string CustomerCode { get; set; }
        public string CustomerDesc { get; set; }

    }
}

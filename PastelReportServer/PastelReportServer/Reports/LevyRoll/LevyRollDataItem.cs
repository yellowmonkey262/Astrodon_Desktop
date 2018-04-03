using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.LevyRoll
{
    public class LevyRollDataItem:PervasiveDataItem
    {
        public LevyRollDataItem(DataRow row, int period)
        {
            CustomerCode = (string)row["CustomerCode"];
            CustomerDesc = (string)row["CustomerDesc"];

            Levy = ReadDecimal(row, "Levy");
            Water = ReadDecimal(row, "Water");
            Electricity = ReadDecimal(row, "Electricity");
            Interest = ReadDecimal(row, "Interest");
            Legal = ReadDecimal(row, "Legal");
            Sewer = ReadDecimal(row, "Sewer");
            Payments = ReadDecimal(row, "Payments");
            Sundries = ReadDecimal(row, "Sundries");
            CSOS = ReadDecimal(row, "CSOS");

            OpeningBalance = CalcOpening(period,row);
            ClosingBalance = CalcClosing(period, row);
        }

        private decimal CalcOpening(int period, DataRow row)
        {
            decimal sumLast13 = 0;
            for(int x=1; x<=13; x++)
            {
                sumLast13 += ReadDecimal(row,"BalanceLast" + x.ToString().PadLeft(2, '0'));
            }
            if(period > 100)
            {
                var fld = period - 100;
                if (fld == 1)
                    return sumLast13;

                for(int x=1; x < fld;x++)
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

        public string CustomerCode { get; set; }
        public string CustomerDesc { get; set; }

        public decimal OpeningBalance { get; set; }
        public string OpeningBalanceStr { get { return FormatCurrency(OpeningBalance); } }

        public decimal Levy { get; set; }
        public string LevyStr { get { return FormatCurrency(Levy); } }

        public decimal Water { get; set; }
        public string WaterStr { get { return FormatCurrency(Water); } }

        public decimal Electricity { get; set; }
        public string ElectricityStr { get { return FormatCurrency(Electricity); } }

        public decimal Interest { get; set; }
        public string InterestStr { get { return FormatCurrency(Interest); } }

        public decimal Legal { get; set; }
        public string LegalStr { get { return FormatCurrency(Legal); } }

        public decimal Sewer { get; set; }
        public string SewerStr { get { return FormatCurrency(Sewer); } }

        public decimal Payments { get; set; }
        public string PaymentsStr { get { return FormatCurrency(Payments); } }

        public decimal Sundries { get; set; }
        public string SundriesStr { get { return FormatCurrency(Sundries); } }

        public decimal CSOS { get; set; }
        public string CSOSStr { get { return FormatCurrency(CSOS); } }

        public decimal ClosingBalance { get; set; }
        public string ClosingBalanceStr { get { return FormatCurrency(ClosingBalance); } }

        public decimal Due
        {
            get
            {
                return OpeningBalance + Levy + Water + Electricity + Interest + Legal + Sewer + Payments + CSOS + Sundries;
            }
        }
        public string DueStr { get { return FormatCurrency(Due); } }
    }
}

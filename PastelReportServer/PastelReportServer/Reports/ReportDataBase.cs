using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports
{
    public class ReportDataBase
    {
        private DateTime _ReportDate = DateTime.Now;

        public string ReportDateFormatted { get { return FormatDate(_ReportDate); } }

        public string ReportDateTimeFormatted { get { return FormatDateTime(_ReportDate); } }

        #region Formatting

        public static string FormatDate(DateTime? date)
        {
            if (date == null)
                return " ";

            return date.Value.ToString("dd/MM/yyyy");
        }

        public static string FormatDateTime(DateTime? date)
        {
            if (date == null)
                return null;
            return date.Value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string FormatNumber(decimal? number)
        {
            if (number == null)
                return null;

            return number.Value.ToString("#,###,###,##0.00");
        }

        public static string FormatCurrency(decimal? number)
        {
            if (number == null)
                return string.Empty;
            return number.Value.ToString("R#,###,###,##0.00");
        }

        public static string FormatBlankCurrency(decimal? number)
        {
            if (number == null || number == 0)
                return String.Empty;
            else
                return FormatCurrency(number);
        }

        public static string FormatInteger(long? number)
        {
            if (number == null)
                return string.Empty;
            else
                return number.Value.ToString("###,###,###,##0");
        }

        #endregion


        internal static string FormatString(string v)
        {
            if (string.IsNullOrWhiteSpace(v))
                return " ";
            else
                return v;
        }
    }
}
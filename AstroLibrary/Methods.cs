using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library
{
    public class Methods
    {
        public static int getPeriod(DateTime trnDate, int sbPeriod, out int bPeriod)
        {
            int myMonth = trnDate.Month;
            myMonth = myMonth - 2;
            myMonth = (myMonth < 1 ? myMonth + 12 : myMonth); //12
            bPeriod = (myMonth - sbPeriod < 1 ? myMonth - sbPeriod + 12 : myMonth - sbPeriod);
            return myMonth;
        }

        public static int getPeriod(DateTime trnDate)
        {
            int myMonth = trnDate.Month;
            myMonth = myMonth - 2;
            myMonth = (myMonth < 1 ? myMonth + 12 : myMonth);
            return myMonth;
        }

        public static String cleanDate(String rawDate)
        {
            String Numbers = "0123456789";
            rawDate = rawDate.Replace("-", "").Replace("/", "");
            if (Numbers.Contains(rawDate.Substring(1, 1))) { } else { rawDate = "0" + rawDate; }
            String day = rawDate.Substring(0, 2);
            String month = "";
            int yearX = 0;
            if (!Numbers.Contains(rawDate.Substring(2, 1)))
            {
                month = rawDate.Substring(2, 3);
                switch (month)
                {
                    case "Jan":
                        month = "01";
                        break;

                    case "Feb":
                        month = "02";
                        break;

                    case "Mar":
                        month = "03";
                        break;

                    case "Apr":
                        month = "04";
                        break;

                    case "May":
                        month = "05";
                        break;

                    case "Jun":
                        month = "06";
                        break;

                    case "Jul":
                        month = "07";
                        break;

                    case "Aug":
                        month = "08";
                        break;

                    case "Sep":
                        month = "09";
                        break;

                    case "Oct":
                        month = "10";
                        break;

                    case "Nov":
                        month = "11";
                        break;

                    case "Dec":
                        month = "12";
                        break;
                }
                yearX = 5;
            }
            else
            {
                month = rawDate.Substring(2, 2);
                yearX = 4;
            }
            int yearLength = rawDate.Length - yearX;
            String year = "";
            if (yearLength == 2) { year = "20" + rawDate.Substring(yearX, 2); } else { year = rawDate.Substring(yearX, 4); }
            String cleanDate = day + "/" + month + "/" + year;
            return cleanDate;
        }

        public static String cleanDescription(String rawDescription)
        {
            rawDescription = rawDescription.Replace("+", " ");
            if (rawDescription == "PRIME PROPERTIES") { rawDescription += " KB000"; }
            try
            {
                if (rawDescription.Substring(0, 13) == "CASHFOCUS MDM")
                {
                    String subNumber = rawDescription.Substring(15, 3);
                    rawDescription = rawDescription.Substring(0, 13) + subNumber;
                }
            }
            catch { }
            Char[] illegalChars = "!@#$%^&*{}[]\"'_+<>?".ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Char ch in rawDescription.ToCharArray()) { if (Array.IndexOf(illegalChars, ch) == -1) { sb.Append(ch); } }
            rawDescription = sb.ToString();
            return rawDescription;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.LevyRoll
{
    public class PeriodDataItem
    {

        private List<PeriodItem> _ItemList;
        private int _maxPeriodThis;
        private int _maxPeriodLast;


        public PeriodDataItem(DataRow row)
        {
            //NumberPeriodsThis,NumberPeriodsLast,
            _maxPeriodThis = ReadShort(row["NumberPeriodsThis"],12);
            _maxPeriodLast = ReadShort(row["NumberPeriodsLast"], 12);
            _ItemList = new List<PeriodItem>();
            //Period this
            for (int x=1; x <= 20; x++)
            {
                if (x <= _maxPeriodThis)
                {
                    var itm = new PeriodItem();
                    itm.PeriodNumber = 100 + x;
                    itm.Start = ReadDate(row["PerStartThis" + x.ToString().PadLeft(2, '0')]);
                    itm.End = ReadDate(row["PerEndThis" + x.ToString().PadLeft(2, '0')]);
                    _ItemList.Add(itm);
                }

                if (x <= _maxPeriodLast)
                {
                    var itm = new PeriodItem();
                    itm.PeriodNumber = x;
                    itm.Start = ReadDate(row["PerStartLast" + x.ToString().PadLeft(2, '0')]);
                    itm.End = ReadDate(row["PerEndLast" + x.ToString().PadLeft(2, '0')]);
                    _ItemList.Add(itm);
                }

            }
        }

        private int ReadShort(object cell, int def)
        {
            try
            {
                return (short)cell;
            }
            catch
            {
                return def;
            }
        }

        private int ReadInt(object cell, int def)
        {
            try
            {
                return (int)cell;
            }
            catch(Exception e)
            {
                return def;
            }
        }

        private DateTime? ReadDate(object value)
        {
            if (value == null || value is DBNull)
                return null;
            return (DateTime)value;
        }

        public int PeriodNumberLookup(DateTime date)
        {
            var x = _ItemList.Where(a => a.Start == date).FirstOrDefault();
            if (x == null)
            {
                string errorMessage = "Period not found Start: " + _ItemList.Where(a => a.Start != null).Min(a => a.Start).Value.ToString("yyyyMMdd")
                                                                 + " - " + _ItemList.Where(a => a.Start != null).Max(a => a.Start).Value.ToString("yyyyMMdd");
                throw new Exception(errorMessage + " for " + date.ToString("yyyyMMdd"));
            }
            return x.PeriodNumber;
        }
    }

    public class PeriodItem
    {
        public int PeriodNumber { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}

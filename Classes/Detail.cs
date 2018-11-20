using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Classes
{
    public class Detail
    {
        private String _description, _reference;
        private DateTime _trnDate;
        private double _amt;
        public bool deleteMe = false;

        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public String Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        public DateTime TrnDate
        {
            get { return _trnDate; }
            set { _trnDate = value; }
        }

        public double Amt
        {
            get { return _amt; }
            set { _amt = value; }
        }

        public Detail(String description, String reference, DateTime trnDate, double amt)
        {
            Description = description;
            Reference = reference;
            TrnDate = trnDate;
            Amt = amt;
        }

        public bool validDetail = true;

        //public Detail(String transString, DateTime pStart, DateTime pEnd, int journal)
        //{
        //    try
        //    {
        //        String[] splitter = new string[] { "|" };
        //        String[] transBits = transString.Split(splitter, StringSplitOptions.None);
        //        TrnDate = Controller.pastel.GetDate(transBits[7]);
        //        if (TrnDate < pStart || TrnDate > pEnd) { deleteMe = true; }
        //        String eType = transBits[8];
        //        if (eType != journal.ToString()) { deleteMe = true; }
        //        Reference = transBits[9];
        //        if (!double.TryParse(transBits[11], out _amt)) { Amt = 0; }
        //        Description = transBits[18];
        //    }
        //    catch (Exception ex)
        //    {
        //        deleteMe = true;
        //    }
        //}
    }
}
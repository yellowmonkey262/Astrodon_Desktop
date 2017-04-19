using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class Trns
    {
        public String Date { get; set; }

        public String Description { get; set; }

        public String Reference { get; set; }

        public String Amount { get; set; }

        public String period { get; set; }

        public String Cumulative { get; set; }
    }

    public class Transaction
    {
        public DateTime TrnDate { get; set; }

        public String Reference { get; set; }

        public String Description { get; set; }

        public double TrnAmt { get; set; }

        public double AccAmt { get; set; }
    }

    public class Transactions
    {
        private DateTime trnDate;
        private String reference;
        private double amt;
        private double accAmt;
        private String description;

        public DateTime TrnDate
        {
            get { return trnDate; }
            set { trnDate = value; }
        }

        public String Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        public double Amt
        {
            get { return amt; }
            set { amt = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public double AccAmt
        {
            get { return accAmt; }
            set { accAmt = value; }
        }
    }
}
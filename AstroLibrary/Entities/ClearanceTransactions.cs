using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class ClearanceTransactions
    {
        private double qty = 1;
        private double rate = 0;
        private double mu = 0;
        private double amt = 0;

        public String Description { get; set; }

        public double Qty
        {
            get { return qty; }
            set
            {
                qty = value;
                amt = qty * rate * (1 + (mu / 100));
            }
        }

        public double Rate
        {
            get { return rate; }
            set
            {
                rate = value;
                amt = qty * rate * (1 + (mu / 100));
            }
        }

        public double Markup_Percentage
        {
            get { return mu; }
            set
            {
                mu = value;
                amt = qty * rate * (1 + (mu / 100));
            }
        }

        public double Amount
        {
            get { return amt; }
            set { amt = value; }
        }
    }
}
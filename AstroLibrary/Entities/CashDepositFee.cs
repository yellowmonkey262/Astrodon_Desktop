using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class CashDepositFee
    {
        public int ID { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }

        public double Amt { get; set; }
    }
}
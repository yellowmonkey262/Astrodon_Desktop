using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class Requisition
    {
        public String ID { get; set; }

        public DateTime trnDate { get; set; }

        public String building { get; set; }

        public String reference { get; set; }

        public String payreference { get; set; }

        public String account { get; set; }

        public String ledger { get; set; }

        public double accBalance { get; set; }

        public double amount { get; set; }
    }
}
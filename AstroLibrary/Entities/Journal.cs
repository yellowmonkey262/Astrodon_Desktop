using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class Journal
    {
        public DateTime trnDate { get; set; }

        public int buildPeriod { get; set; }

        public String trustPath { get; set; }

        public String buildPath { get; set; }

        public int trustType { get; set; }

        public int buildType { get; set; }

        public String bc { get; set; }

        public String buildAcc { get; set; }

        public String trustContra { get; set; }

        public String buildContra { get; set; }

        public String reference { get; set; }

        public String description { get; set; }

        public String amt { get; set; }

        public String trustAcc { get; set; }

        public String rAcc { get; set; }
    }
}
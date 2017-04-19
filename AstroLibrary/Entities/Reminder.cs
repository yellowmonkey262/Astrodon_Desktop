using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class Reminder
    {
        public int id { get; set; }

        public String User { get; set; }

        public DateTime remDate { get; set; }

        public String note { get; set; }

        public bool action { get; set; }
    }
}
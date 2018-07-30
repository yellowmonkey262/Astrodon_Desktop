using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class CustomerDocument
    {
        public DateTime tstamp { get; set; }

        public String subject { get; set; }

        public String title { get; set; }

        public String file { get; set; }

        public Guid Id { get; set; }
    }
}
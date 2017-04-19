using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Astro.Library.Entities
{
    public class WebReportData
    {
        public String building { get; set; }

        public Image picture { get; set; }

        public Dictionary<String, List<String>> files { get; set; }
    }
}
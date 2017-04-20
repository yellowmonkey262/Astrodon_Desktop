using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class WebBuildingPrint
    {
        public String BuildingName { get; set; }

        public String Debtor { get; set; }

        public String PM { get; set; }

        public String Address { get; set; }

        public String Image { get; set; }

        public String ImageUpload { get; set; }

        public Dictionary<String, List<String>> Files { get; set; }
    }
}
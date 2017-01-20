using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Data.Base
{
    public class StringKeyValue
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public string Display { get { return Id + " - " + Value; } }
    }
}

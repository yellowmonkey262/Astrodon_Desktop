using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Forms
{
    public class CustomerDocument
    {
        public bool Select { get; set; }

        public String FileID { get; set; }

        public String Customer { get; set; }

        public String Title { get; set; }

        public DateTime Upload_Date { get; set; }

        public String FileName { get; set; }
    }
}
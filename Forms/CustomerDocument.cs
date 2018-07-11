using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Forms
{
    public class CustomerDocument
    {
        public bool Select { get; set; }

        public Guid FileID { get; set; }

        public string Customer { get; set; }

        public string Title { get; set; }

        public DateTime Upload_Date { get; set; }

        public string FileName { get; set; }
    }
}
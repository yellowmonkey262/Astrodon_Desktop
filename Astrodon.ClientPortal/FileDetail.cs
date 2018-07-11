using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.ClientPortal
{
    public class FileDetail
    {
        public Guid Id { get; set; }

        public DateTime DocumentDate { get; set; }

        public string Subject { get; set; }

        public string Title { get; set; }

        public string File { get; set; }

        public string AccountNumber { get; set; }

        public override string ToString()
        {
            return File;
        }
    }
}

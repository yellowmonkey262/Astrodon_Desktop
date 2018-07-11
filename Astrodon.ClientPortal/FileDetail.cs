using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.ClientPortal
{
    public class FileDetail: DataItemBase
    {
        public FileDetail(DataRow row)
            :base(row)
        {

        }

        [FromDB]
        public Guid Id { get; set; }

        [FromDB]
        public DateTime DocumentDate { get; set; }

        [FromDB]
        public string Title { get; set; }

        [FromDB]
        public string File { get; set; }

        [FromDB]
        public string AccountNumber { get; set; }

        public override string ToString()
        {
            return File;
        }
    }
}

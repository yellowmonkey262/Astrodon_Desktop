using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Astrodon.Reports.ManagementReportCoverPage
{
    [DataContract]
    public class TOCDataItem
    {
        [DataMember]
        public string ItemNumber { get; set; }

        [DataMember]
        public string ItemDescription { get; set; }

        [DataMember]
        public int PageNumber { get; set; }
    }
}

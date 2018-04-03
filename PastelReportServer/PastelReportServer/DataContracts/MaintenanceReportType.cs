using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Astrodon.DataContracts
{
    [DataContract]
    public enum MaintenanceReportType
    {
        [EnumMember]
        SummaryReport,
        [EnumMember]
        DetailedReport,
        [EnumMember]
        DetailedReportWithSupportingDocuments
    }
}

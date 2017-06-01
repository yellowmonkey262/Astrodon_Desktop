using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.ReportService
{
    public partial class PastelMaintenanceTransaction : Astrodon.ReportService.PervasiveItem
    {
        public string AccountDesc
        {
            get
            {
                return Account + ": " + AccountName;
            }
        }
    }
}

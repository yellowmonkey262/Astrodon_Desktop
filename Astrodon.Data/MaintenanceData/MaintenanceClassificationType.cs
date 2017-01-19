using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.MaintenanceData
{
    public enum MaintenanceClassificationType
    {
        RemedialMaintenance,
        MaintenancePlan,
        Project,
        Insurance
    }
}

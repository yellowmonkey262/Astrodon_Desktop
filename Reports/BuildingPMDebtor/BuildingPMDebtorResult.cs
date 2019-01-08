using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.BuildingPMDebtor
{
    public class BuildingPMDebtorResult
    {
        public int BuildingId { get; set; }

        public string BuildingName { get; set; }

        public string BuildingCode { get; set; }

        public string PortfolioManager { get; set; }

        public string PortfolioManagerEmail { get; set; }

        public string Debtor { get; set; }

        public string DebtorEmail { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.Data;
using System.Data.Entity;
using System.Collections;
using System.Globalization;

namespace Astrodon.Reports.InsuranceSchedule
{
    public class InsuranceScheduleReport
    {
        private DataContext _context;

        public InsuranceScheduleReport(DataContext context)
        {
            _context = context;
        }

        public byte[] RunReport(int buildingId)
        {
            var building = _context.tblBuildings.Include(a => a.InsuranceBroker).Single(a => a.id == buildingId);

            var buildingUnits = _context.BuildingUnitSet.Where(a => a.BuildingId == buildingId && a.SquareMeters > 0).ToList();

            var reportDataSet = buildingUnits.Select(a=> new InsuranceScheduleDataItem()
            {
                Unit = a.UnitNo,
                PQPersentage = (a.PQRating * 100).ToString(),
                AdditionalCost = a.AdditionalInsurance.ToString(),
                UnitCost = (building.UnitReplacementCost * a.PQRating).ToString(),
                TotalCost = ((building.UnitReplacementCost * a.PQRating) + a.AdditionalInsurance).ToString()
            });

            Dictionary<string, IEnumerable> reportData = new Dictionary<string, IEnumerable>();
            Dictionary<string, string> reportParams = new Dictionary<string, string>();

            reportParams.Add("Created", DateTime.Now.ToShortDateString());

            reportParams.Add("BuildingName", building.Building);
            reportParams.Add("Address1", building.addy1);
            reportParams.Add("Address2", building.addy2);
            reportParams.Add("Address3", building.addy3);
            reportParams.Add("Address4", building.addy4);
            reportParams.Add("Address5", building.addy5);

            reportParams.Add("Trust", building.bank);
            reportParams.Add("Bank", building.bankName);
            reportParams.Add("AccountName", building.accName);
            reportParams.Add("AccountNumber", building.bankAccNumber);
            reportParams.Add("BranchCode", building.branch);

            reportParams.Add("BrokerAccountNumber", building.PolicyNumber);
            if (building.InsuranceBroker != null)
            {
                reportParams.Add("BrokerCompany", building.InsuranceBroker.CompanyName);
                reportParams.Add("BrokerName", building.InsuranceBroker.ContactPerson);
                reportParams.Add("BrokerTel", building.InsuranceBroker.ContactNumber);
                reportParams.Add("BrokerEmail", building.InsuranceBroker.EmailAddress);
            }

            reportParams.Add("CommonPropertyDimension", building.CommonPropertyDimensions.ToString());
            if (building.CommonPropertyReplacementCost > 0 && !building.InsuranceReplacementValueIncludesCommonProperty)
                reportParams.Add("CommonReplacementValue", building.CommonPropertyReplacementCost.ToString("#,##0.00"));
            else
            {
                if(building.InsuranceReplacementValueIncludesCommonProperty)
                  reportParams.Add("CommonReplacementValue", "Included in Replacement Value");
            }

            reportParams.Add("UnitPropertyDimension", building.UnitPropertyDimensions.ToString());

            string replacementString = building.UnitReplacementCost.ToString("#,##0.00");
            reportParams.Add("UnitReplacementValue", replacementString);

            reportData.Add("dsInsuranceData", reportDataSet);

            string rdlcPath = "Astrodon.Reports.InsuranceSchedule.InsuranceScheduleReport.rdlc";
            byte[] report = null;

            using (RdlcHelper rdlcHelper = new RdlcHelper(rdlcPath, reportData, reportParams))
            {
                rdlcHelper.Report.EnableExternalImages = true;
                report = rdlcHelper.GetReportAsFile();
            }
            return report;
        }
    }
}

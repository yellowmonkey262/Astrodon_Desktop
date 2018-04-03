using Astrodon.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.SupplierReport
{
    public class SupplierReport 
    {
        private DataContext _dataContext;
        public SupplierReport(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        internal byte[] RunReport(DateTime fromDate, DateTime toDate, int? buildingId, int? supplierId)
        {
            //setup query
            if (buildingId == null)
                buildingId = 0;
            if (supplierId == null)
                supplierId = 0;

            DateTime startDate = new DateTime(fromDate.Year, fromDate.Month, 1);
            DateTime endDate = new DateTime(toDate.Year, toDate.Month, 1).AddMonths(1).AddSeconds(-1);

            var reportData = (from s in _dataContext.SupplierSet
                              from m in s.Maintenance
                              where m.DateLogged >= startDate && m.DateLogged <= endDate
                              && (buildingId == 0 || m.Requisition.building == buildingId)
                              && (supplierId == 0 ||s.id == supplierId)
                              group m by new
                              {
                                  s.id,
                                  s.CompanyName,
                                  s.CompanyRegistration,
                                  s.ContactPerson,
                                  s.EmailAddress,
                                  s.ContactNumber,
                                  s.BlackListed,
                                  m.BuildingMaintenanceConfiguration.Building.Building,
                                  m.Requisition.BankName,
                                  m.Requisition.AccountNumber
                              } into grouped
                              select new SupplierReportDataItem
                              {
                                  SupplierId = grouped.Key.id,
                                  CompanyName = grouped.Key.CompanyName,
                                  Building = grouped.Key.Building,
                                  Registration = grouped.Key.CompanyRegistration,
                                  ContactPerson = grouped.Key.ContactPerson,
                                  Email = grouped.Key.EmailAddress,
                                  Phone = grouped.Key.ContactNumber,
                                  BlackList = grouped.Key.BlackListed ? "Yes" : "No",
                                  LastUsed = grouped.Max(m => m.DateLogged),
                                  BankName = grouped.Key.BankName,
                                  AccountNumber = grouped.Key.AccountNumber,
                                  Projects = grouped.Count(),
                                  Amount = grouped.Sum(a => a.TotalAmount)
                              }).OrderBy(a => a.CompanyName).ToList();

            if (reportData.Count == 0)
                return null;

            return RunReportToPdf(reportData,fromDate,toDate);
        }

        private byte[] RunReportToPdf(List<SupplierReportDataItem> data,  DateTime fromDate,DateTime toDate)
        {
            string rdlcPath = "Astrodon.Reports.SupplierReport.SupplierReport.rdlc";
            byte[] report = null;

            Dictionary<string, IEnumerable> reportData = new Dictionary<string, IEnumerable>();
            Dictionary<string, string> reportParams = new Dictionary<string, string>();

            string period = fromDate.ToString("MMM yyyy") + " - " + toDate.ToString("MMM yyyy");

            reportParams.Add("Period", period);

            reportData.Add("SupplierData", data);

            using (RdlcHelper rdlcHelper = new RdlcHelper(rdlcPath, reportData, reportParams))
            {
                rdlcHelper.Report.EnableExternalImages = true;
                report = rdlcHelper.GetReportAsFile();
            }

            return report;
        }
    }
}

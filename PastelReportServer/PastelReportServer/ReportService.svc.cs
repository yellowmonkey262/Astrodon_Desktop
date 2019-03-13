using Astrodon;
using Astrodon.Reports.LevyRoll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Astrodon.DataContracts;
using Astrodon.Data;
using Astrodon.Reports.MaintenanceReport;
using Astrodon.Reports.SupplierReport;
using Astrodon.DataContracts.Maintenance;
using Astrodon.DataProcessor;
using Astrodon.Reports.RequisitionBatch;
using Astrodon.Reports.ManagementReportCoverPage;
using Astrodon.Reports.InsuranceSchedule;
using Astrodon.Data.DebitOrder;
using Astrodon.DebitOrder;
using Astrodon.Reports.MonthlyReport;
using Astrodon.Reports.AllocationWorksheet;
using Astrodon.CustomerMaintenance;
using Desktop.Lib.Pervasive;
using System.Data;
using Astrodon.TransactionSearch;

namespace PastelDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReportService.svc or ReportService.svc.cs at the Solution Explorer and start debugging.
    public class ReportService : IReportService
    {
        public byte[] LevyRollReport(DateTime processMonth, string buildingName, string dataPath)
        {
            var lr = new LevyRollReport();
            return lr.RunReport(processMonth, buildingName, dataPath, true);
        }

        public byte[] LevyRollExcludeSundries(DateTime processMonth, string buildingName, string dataPath)
        {
            var lr = new LevyRollReport();
            return lr.RunReport(processMonth, buildingName, dataPath, false);
        }

        public byte[] MaintenanceReport(string sqlConnectionString, MaintenanceReportType reportType,DateTime fromDate, DateTime toDate, int buildingId, string buildingName, string dataPath)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new MaintenanceReport(dc);

                return rp.RunReport(reportType, fromDate,toDate, buildingId, buildingName, dataPath);
                
            }
        }
      

        public byte[] SupplierReport(string sqlConnectionString, DateTime fromDate, DateTime toDate, int? buildingId, int? supplierId)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new SupplierReport(dc);

                return rp.RunReport(fromDate,toDate,buildingId,supplierId);
            }
        }


        public ICollection<PastelMaintenanceTransaction> MissingMaintenanceRecordsGet(string sqlConnectionString, int buildingId)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new MaintenanceProcessor(dc, buildingId);

                return rp.MissingMaintenanceRecordsGet();
            }
        }

        public byte[] RequisitionBatchReport(string sqlConnectionString, int requisitionBatchId)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new RequisitionBatchReport(dc);

                return rp.RunReport(requisitionBatchId, sqlConnectionString);
            }
        }

        public byte[] ManagementPackCoverPage(DateTime processMonth, string buildingName, string agent, List<TOCDataItem> tocDataItems)
        {
            var rp = new ManagementReportCoverPage();
            return rp.RunReport(processMonth, buildingName,agent, tocDataItems);
        }

        public byte[] InsuranceSchedule(string sqlConnectionString, int buildingId)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new InsuranceScheduleReport(dc);

                return rp.RunReport(buildingId);
            }
        }


        public byte[] MonthlyReport(string sqlConnectionString, DateTime processMonth, bool completedItems, int? userId)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new MonthlyReportExport(dc);

                return rp.RunReport(processMonth, completedItems, userId);
            }
        }

        public List<DebitOrderItem> RunDebitOrderForBuilding(string sqlConnectionString, int buildingId, DateTime processMonth, bool showFeeBreakdown)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new DebitOrderExcel(dc);

                return rp.RunDebitOrderForBuilding(buildingId, processMonth, showFeeBreakdown);
            }
        }

        public void RequestAllocations(string sqlConnectionString, int userId)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new AllocationWorksheetReport(dc);

                 rp.EmailAllocations(userId);
            }
        }

        
        public List<PeriodItem> CustomerStatementParameterLookup(string sqlConnectionString, int buildingId, string customerCode, DateTime endDate, int numberOfMonths)
        {
            using (var dc = new DataContext(sqlConnectionString))
            {
                var rp = new DebitOrderExcel(dc);

                 return rp.CustomerStatementParameterLookup(buildingId,customerCode, endDate,numberOfMonths);
            }
        }

        public List<BuildingClosingBalance> BuildingBalancesGet(DateTime processMonth, string buildingDataPath)
        {
            var lr = new LevyRollReport();
            return lr.BuildingBalancesGet(processMonth, buildingDataPath);
        }

        public List<CustomerCategory> GetCustomerCategories(string buildPath)
        {
               bool isRental = buildPath.ToUpper().StartsWith("RENTAL");

                string qry = "select  CCCode,CCDesc from [DataSet].CustomerCategories";
                if (isRental)
                    qry = qry + " where CCCode > 100 or CCCode = 0 ";
                qry = qry + " Order by CCCode";
                qry = PervasiveSqlUtilities.SetDataSource(qry, buildPath);
            try
            {

                List<CustomerCategory> result = new List<CustomerCategory>();

                var data = PervasiveSqlUtilities.FetchPervasiveData(qry);
                foreach (DataRow row in data.Rows)
                {
                    CustomerCategory c = new CustomerCategory(row);
                    result.Add(c);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " -> [" + qry + "]");
            }
        }

        public List<TransactionDataItem> SearchPastel(string buildingPath, DateTime fromDate, DateTime toDate, string reference, string description, decimal? minimumAmount, decimal? maximumAmount)
        {
            return TransactionSearch.SearchPastel(buildingPath, fromDate, toDate, reference, description, minimumAmount, maximumAmount);
        }
    }
}

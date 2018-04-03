using Astrodon.DataContracts;
using Astrodon.DataContracts.Maintenance;
using Astrodon.DebitOrder;
using Astrodon.Reports.ManagementReportCoverPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PastelDataService
{
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        byte[] LevyRollReport(DateTime processMonth, string buildingName, string dataPath);

        [OperationContract]
        byte[] LevyRollExcludeSundries(DateTime processMonth, string buildingName, string dataPath);


        [OperationContract]
        byte[] SupplierReport(string sqlConnectionString, DateTime fromDate, DateTime toDate, int? buildingId, int? supplierId);

        [OperationContract]
        byte[] MaintenanceReport(string sqlConnectionString, MaintenanceReportType reportType, DateTime fromDate,DateTime toDate, int buildingId, string buildingName, string dataPath);

        [OperationContract]
        ICollection<PastelMaintenanceTransaction> MissingMaintenanceRecordsGet(string sqlConnectionString, int buildingId);

        [OperationContract]
        byte[] RequisitionBatchReport(string sqlConnectionString, int requisitionBatchId);

        [OperationContract]
        byte[] ManagementPackCoverPage(DateTime processMonth, string buildingName, string agent, List<TOCDataItem> tocDataItems);

        [OperationContract]
        byte[] InsuranceSchedule(string sqlConnectionString, int buildingId);

        [OperationContract]
        List<DebitOrderItem> RunDebitOrderForBuilding(string sqlConnectionString, int buildingId, DateTime processMonth, bool showFeeBreakdown);

        [OperationContract]
        byte[] MonthlyReport(string sqlConnectionString, DateTime processMonth, bool completedItems, int? userId);

        [OperationContract]
        void RequestAllocations(string sqlConnectionString, int userId);


    }
}

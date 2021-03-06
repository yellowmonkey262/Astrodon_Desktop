﻿
//using Astrodon.Data.Maintenance;
using Astrodon.Data.BankData;
using Astrodon.Data.Calendar;
using Astrodon.Data.CustomerData;
using Astrodon.Data.DebitOrder;
using Astrodon.Data.InsuranceData;
using Astrodon.Data.Log;
using Astrodon.Data.MaintenanceData;
using Astrodon.Data.ManagementPackData;
using Astrodon.Data.NotificationTemplateData;
using Astrodon.Data.RequisitionData;
using Astrodon.Data.SupplierData;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;

namespace Astrodon.Data
{
    public partial class DataContext : DbContext
    {
        #region Ctor

        public DataContext()
            : base("DataContext")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public DataContext(DbConnection connection)
           : base(connection, true)
        {
        }

        public DataContext(string connectionString)
            : base(connectionString)
        {

        }

        #endregion

        #region Setup

        public static void Setup(string connectionString)
        {
            Migrations.Configuration.MigrationConnectionString = connectionString;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
            using (var context = new DataContext(Migrations.Configuration.MigrationConnectionString))
            {
                try
                {
                    context.Database.Initialize(false);

                }
                catch (InvalidOperationException ex)
                {
                    throw ex;
                }
                catch (ModelValidationException ex)
                {
                    throw ex;
                }
            }
        }

        public static void HouseKeepSystemLog(string connectionString)
        {
            using (var dbContext = new DataContext(connectionString))
            {
                dbContext.Database.ExecuteSqlCommand("delete from SystemLogs where EventTime < GetDate() - 30");
            }
        }

        #endregion

        public DbSet<BuildingMaintenanceConfiguration> BuildingMaintenanceConfigurationSet { get; set; }
        public DbSet<SupplierBuilding> SupplierBuildingSet { get; set; }
        public DbSet<SupplierBuildingAudit> SupplierBuildingAuditSet { get; set; }

        public DbSet<Supplier> SupplierSet { get; set; }
        public DbSet<SupplierAudit> SupplierAuditSet { get; set; }

        public DbSet<Maintenance> MaintenanceSet { get; set; }
        public DbSet<MaintenanceDocument> MaintenanceDocumentSet { get; set; }
        public DbSet<MaintenanceDetailItem> MaintenanceDetailItemSet { get; set; }

        public DbSet<RequisitionDocument> RequisitionDocumentSet { get; set; }
        public DbSet<RequisitionBatch> RequisitionBatchSet { get; set; }

        public DbSet<Bank> BankSet { get; set; }
        public DbSet<BankAudit> BankAuditSet { get; set; }
        public DbSet<SystemLog> SystemLogSet { get; set; }
        public DbSet<BondOriginator> BondOriginatorSet { get; set; }

        public DbSet<BuildingUnit> BuildingUnitSet { get; set; }
        public DbSet<BuildingDocument> BuildingDocumentSet { get; set; }
        public DbSet<InsuranceBroker> InsuranceBrokerSet { get; set; }

        #region Management Pack

        public DbSet<ManagementPackTOCItem> ManagementPackTOCItemSet { get; set; }
        public DbSet<ManagementPack> ManagementPackSet { get; set; }
        public DbSet<ManagementPackReportItem> ManagementPackReportItemSet { get; set; }

        #endregion

        #region Debit Orders

        public DbSet<CustomerDebitOrder> CustomerDebitOrderSet { get; set; }
        public DbSet<DebitOrderDocument> DebitOrderDocumentSet { get; set; }
        public DbSet<CustomerDebitOrderArchive> CustomerDebitOrderArchiveSet { get; set; }
        public DbSet<DebitOrderDocumentArchive> DebitOrderDocumentArchiveSet { get; set; }
        public DbSet<Customer> CustomerSet { get; set; }
        public DbSet<CustomerDocumentType> CustomerDocumentTypeSet { get; set; }
        public DbSet<CustomerDocument> CustomerDocumentSet { get; set; }

        #endregion

        #region Calendar
        public DbSet<BuildingCalendarEntry> BuildingCalendarEntrySet { get; set; }
        public DbSet<CalendarEntryAttachment> CalendarEntryAttachmentSet { get; set; }
        public DbSet<CalendarUserInvite> CalendarUserInviteSet { get; set; }
        public DbSet<PublicHoliday> PublicHolidaySet { get; set; }
        public DbSet<MeetingRoom> MeetingRoomSet { get; set; }


        #endregion

        #region Notification Data
        public DbSet<NotificationTemplate> NotificationTemplateSet { get; set; }
        #endregion

        public void ClearChanges()
        {
            var changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public void ClearStuckRequisitons(int buildingId)
        {
            Database.ExecuteSqlCommand("update tblRequisition set RequisitionBatchId = null where processed = 0 and RequisitionBatchId is not null and building = " + buildingId.ToString());
        }

        public void CommitRequisitionBatch(int requisitionBatchId)
        {
            Database.ExecuteSqlCommand("update tblRequisition set processed = 1 where RequisitionBatchId = " + requisitionBatchId.ToString());
        }

        public void RequisitionBatchRollback(int requisitionBatchId)
        {
            Database.ExecuteSqlCommand("update tblRequisition set processed = 0, RequisitionBatchId = null where RequisitionBatchId = " + requisitionBatchId.ToString());
            Database.ExecuteSqlCommand("delete from RequisitionBatch where id =" + requisitionBatchId.ToString());
        }

        public void DeleteMaintenance(int maintenanceId)
        {
            Database.ExecuteSqlCommand("delete from MaintenanceDocument where MaintenanceId=" + maintenanceId.ToString());
            Database.ExecuteSqlCommand("delete from MaintenanceDetailItem where MaintenanceId=" + maintenanceId.ToString());
            Database.ExecuteSqlCommand("delete from Maintenance where Id=" + maintenanceId.ToString());
        }

        public void CustomerDocumentNotificationSent(int documentId)
        {
            Database.ExecuteSqlCommand("update CustomerDocument set ExpireNotification = GetDate() where id = " + documentId.ToString());

        }

        public void DeleteRequisition(int requisitionId)
        {
            //all maintenance records
            foreach(var maintenanceId in this.MaintenanceSet.Where(a => a.RequisitionId == requisitionId).Select(a => a.id).ToList())
            {
                DeleteMaintenance(maintenanceId);
            }

            Database.ExecuteSqlCommand("delete from RequisitionDocument where RequisitionId=" + requisitionId.ToString());
            Database.ExecuteSqlCommand("delete from tblRequisition where Id=" + requisitionId.ToString());
        }


        public void WriteStatementRunLog(string accNumber, string name, string v)
        {
            var sysLog = new SystemLog()
            {
                EventTime = DateTime.Now,
                Message = "Statement Run: " + accNumber + " - " + name + " -> " + v,
                StackTrace = accNumber
            };
            SystemLogSet.Add(sysLog);
            SaveChanges();
        }
    }
}

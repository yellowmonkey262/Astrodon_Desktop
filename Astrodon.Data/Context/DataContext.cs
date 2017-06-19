
//using Astrodon.Data.Maintenance;
using Astrodon.Data.BankData;
using Astrodon.Data.Log;
using Astrodon.Data.MaintenanceData;
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

        public static void HouseKeepSystemLog()
        {
            using (var dbContext = new DataContext())
            {
                dbContext.Database.ExecuteSqlCommand("delete from SystemLogs where EventTime < GetDate() - 100");
            }
        }



        #endregion

        public DbSet<BuildingMaintenanceConfiguration> BuildingMaintenanceConfigurationSet { get; set; }
        public DbSet<SupplierBuilding> SupplierBuildingSet { get; set; }
        public DbSet<SupplierBuildingAudit> SupplierBuildingAuditSet { get; set; }
        public DbSet<MaintenanceDetailItem> MaintenanceDetailItemSet { get; set; }

        public DbSet<Supplier> SupplierSet { get; set; }
        public DbSet<SupplierAudit> SupplierAuditSet { get; set; }
        public DbSet<Maintenance> MaintenanceSet { get; set; }
        public DbSet<MaintenanceDocument> MaintenanceDocumentSet { get; set; }

        public DbSet<RequisitionDocument> RequisitionDocumentSet { get; set; }
        public DbSet<RequisitionBatch> RequisitionBatchSet { get; set; }

        public DbSet<Bank> BankSet { get; set; }
        public DbSet<BankAudit> BankAuditSet { get; set; }
        public DbSet<SystemLog> SystemLogSet { get; set; }

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


        public void CommitRequisitionBatch(int requisitionBatchId)
        {
            Database.ExecuteSqlCommand("update tblRequisition set processed = 1 where RequisitionBatchId = " + requisitionBatchId.ToString());
        }

        public void RequisitionBatchRollback(int requisitionBatchId)
        {
            Database.ExecuteSqlCommand("update tblRequisition set processed = 0, RequisitionBatchId = null where RequisitionBatchId = " + requisitionBatchId.ToString());
            Database.ExecuteSqlCommand("delete from RequisitionBatch where id =" + requisitionBatchId.ToString());
        }
    }
}

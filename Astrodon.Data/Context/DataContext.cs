
//using Astrodon.Data.Maintenance;
using Astrodon.Data.MaintenanceData;
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



        #endregion

        public DbSet<BuildingMaintenanceConfiguration> BuildingMaintenanceConfigurationSet { get; set; }
        public DbSet<Supplier> SupplierSet { get; set; }
        public DbSet<SupplierAudit> SupplierAuditSet { get; set; }
        public DbSet<Maintenance> MaintenanceSet { get; set; }
        public DbSet<MaintenanceDocument> MaintenanceDocumentSet { get; set; }
    }
}

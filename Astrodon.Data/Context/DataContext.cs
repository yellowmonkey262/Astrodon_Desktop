
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
            var connection = connectionString;
            using (DbConnection setupCon = new SqlConnection(connectionString))
            {
                Setup(setupCon);
            }
        }

        private static void Setup(DbConnection dbConnection, bool initDefaultData = true)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
            //using (var context = new DataContext(dbConnection))
            //{
            //    try
            //    {
            //        context.Database.Initialize(true);
                
            //    }
            //    catch (InvalidOperationException ex)
            //    {
            //        throw ex;
            //    }
            //    catch (ModelValidationException ex)
            //    {
            //        throw ex;
            //    }
            //}
        }

        #endregion

//        public DbSet<BuildingMaintenanceConfiguration> BuildingMaintenanceConfigurationSet { get; set; }
    }
}

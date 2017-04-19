namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Astrodon.Data.DataContext>
    {
        public static string MigrationConnectionString = "";

        public Configuration()
        {
            if(!string.IsNullOrEmpty(Configuration.MigrationConnectionString))
               this.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(Configuration.MigrationConnectionString, "System.Data.SqlClient");

            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Astrodon.Data.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

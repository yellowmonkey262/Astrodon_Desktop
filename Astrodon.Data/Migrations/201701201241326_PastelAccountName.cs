namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PastelAccountName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuildingMaintenanceConfiguration", "PastelAccountName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuildingMaintenanceConfiguration", "PastelAccountName");
        }
    }
}

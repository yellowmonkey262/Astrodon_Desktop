namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotifyTrustees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuildingCalendarEntry", "TrusteesNotified", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuildingCalendarEntry", "TrusteesNotified");
        }
    }
}

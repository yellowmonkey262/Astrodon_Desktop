namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingMeetingSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "FinancialMeetingEvent", c => c.String());
            AddColumn("dbo.tblBuildings", "FinancialMeetingVenue", c => c.String());
            AddColumn("dbo.tblBuildings", "FinancialMeetingBCC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "FinancialMeetingBCC");
            DropColumn("dbo.tblBuildings", "FinancialMeetingVenue");
            DropColumn("dbo.tblBuildings", "FinancialMeetingEvent");
        }
    }
}

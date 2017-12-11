namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingMeetingScheduleMeetingTimes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "FinancialMeetingStartTime", c => c.DateTime());
            AddColumn("dbo.tblBuildings", "FinancialMeetingEndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "FinancialMeetingEndTime");
            DropColumn("dbo.tblBuildings", "FinancialMeetingStartTime");
        }
    }
}

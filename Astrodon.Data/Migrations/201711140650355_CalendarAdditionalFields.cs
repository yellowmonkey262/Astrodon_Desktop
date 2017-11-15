namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalendarAdditionalFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuildingCalendarEntry", "EventToDate", c => c.DateTime());
            AddColumn("dbo.BuildingCalendarEntry", "NotifyTrustees", c => c.Boolean(nullable: false));
            AddColumn("dbo.BuildingCalendarEntry", "BCCEmailAddress", c => c.String());
            AddColumn("dbo.BuildingCalendarEntry", "InviteSubject", c => c.String());
            AddColumn("dbo.BuildingCalendarEntry", "InviteBody", c => c.String());

          
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuildingCalendarEntry", "InviteBody");
            DropColumn("dbo.BuildingCalendarEntry", "InviteSubject");
            DropColumn("dbo.BuildingCalendarEntry", "BCCEmailAddress");
            DropColumn("dbo.BuildingCalendarEntry", "NotifyTrustees");
            DropColumn("dbo.BuildingCalendarEntry", "EventToDate");
        }
    }
}

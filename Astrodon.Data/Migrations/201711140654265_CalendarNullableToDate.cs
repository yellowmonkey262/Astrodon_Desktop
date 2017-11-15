namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalendarNullableToDate : DbMigration
    {
        public override void Up()
        {
            Sql("Update BuildingCalendarEntry set EventToDate = dateadd(hour,2,EntryDate)  where EventToDate is null");
            AlterColumn("dbo.BuildingCalendarEntry", "EventToDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BuildingCalendarEntry", "EventToDate", c => c.DateTime());
        }
    }
}

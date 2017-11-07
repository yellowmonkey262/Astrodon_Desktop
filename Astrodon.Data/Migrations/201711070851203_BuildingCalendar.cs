namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingCalendar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BuildingCalendarEntry",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        EntryDate = c.DateTime(nullable: false),
                        Event = c.String(nullable: false, maxLength: 50),
                        Venue = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .ForeignKey("dbo.tblUsers", t => t.UserId)
                .Index(t => new { t.BuildingId, t.UserId }, name: "IDX_BuildingCalendarEntry");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BuildingCalendarEntry", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.BuildingCalendarEntry", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.BuildingCalendarEntry", "IDX_BuildingCalendarEntry");
            DropTable("dbo.BuildingCalendarEntry");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingFinancialCycle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalendarEntryAttachment",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingCalendarEntryId = c.Int(nullable: false),
                        FileData = c.Binary(),
                        FileName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.BuildingCalendarEntry", t => t.BuildingCalendarEntryId)
                .Index(t => t.BuildingCalendarEntryId, name: "IDX_CalendarAttachment");
            
            CreateTable(
                "dbo.PublicHoliday",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        HolidayName = c.String(nullable: false, maxLength: 300),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.Date, unique: true, name: "IDX_PublicHoliday");
            
            AddColumn("dbo.tblBuildings", "FinancialDayOfMonth", c => c.Int(nullable: false, defaultValue:1));
            AddColumn("dbo.tblBuildings", "IsFixed", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CalendarEntryAttachment", "BuildingCalendarEntryId", "dbo.BuildingCalendarEntry");
            DropIndex("dbo.PublicHoliday", "IDX_PublicHoliday");
            DropIndex("dbo.CalendarEntryAttachment", "IDX_CalendarAttachment");
            DropColumn("dbo.tblBuildings", "IsFixed");
            DropColumn("dbo.tblBuildings", "FinancialDayOfMonth");
            DropTable("dbo.PublicHoliday");
            DropTable("dbo.CalendarEntryAttachment");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeetingRoom : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeetingRoom",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        NumberOfSeats = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.Name, unique: true, name: "IDX_MeetingRoom");
            
            AddColumn("dbo.BuildingCalendarEntry", "MeetingRoomId", c => c.Int());
            CreateIndex("dbo.BuildingCalendarEntry", "MeetingRoomId", name: "IDX_BuildingCalendarMeetingRoom");
            AddForeignKey("dbo.BuildingCalendarEntry", "MeetingRoomId", "dbo.MeetingRoom", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BuildingCalendarEntry", "MeetingRoomId", "dbo.MeetingRoom");
            DropIndex("dbo.MeetingRoom", "IDX_MeetingRoom");
            DropIndex("dbo.BuildingCalendarEntry", "IDX_BuildingCalendarMeetingRoom");
            DropColumn("dbo.BuildingCalendarEntry", "MeetingRoomId");
            DropTable("dbo.MeetingRoom");
        }
    }
}

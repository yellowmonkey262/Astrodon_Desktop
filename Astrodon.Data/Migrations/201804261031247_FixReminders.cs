namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixReminders : DbMigration
    {
        public override void Up()
        {
            Sql("Delete from tblReminders");

            AddColumn("dbo.tblReminders", "BuildingId", c => c.Int(nullable: false));
            CreateIndex("dbo.tblReminders", "UserId");
            CreateIndex("dbo.tblReminders", "BuildingId", name: "UIDX_tblReminderBuilding");
            AddForeignKey("dbo.tblReminders", "BuildingId", "dbo.tblBuildings", "id");
            AddForeignKey("dbo.tblReminders", "UserId", "dbo.tblUsers", "id");
            DropColumn("dbo.tblReminders", "building");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblReminders", "building", c => c.String(nullable: false, maxLength: 50));
            DropForeignKey("dbo.tblReminders", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.tblReminders", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.tblReminders", "UIDX_tblReminderBuilding");
            DropIndex("dbo.tblReminders", new[] { "UserId" });
            DropColumn("dbo.tblReminders", "BuildingId");
        }
    }
}

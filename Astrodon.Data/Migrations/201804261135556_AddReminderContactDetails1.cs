namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReminderContactDetails1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.tblReminders", new[] { "UserId" });
            CreateIndex("dbo.tblReminders", new[] { "UserId", "remDate", "action" }, name: "UIDX_tblReminderUser");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tblReminders", "UIDX_tblReminderUser");
            CreateIndex("dbo.tblReminders", "UserId");
        }
    }
}

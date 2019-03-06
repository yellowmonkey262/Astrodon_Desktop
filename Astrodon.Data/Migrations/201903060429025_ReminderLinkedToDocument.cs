namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReminderLinkedToDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblReminders", "CustomerDocumentId", c => c.Int());
            CreateIndex("dbo.tblReminders", "CustomerDocumentId");
            AddForeignKey("dbo.tblReminders", "CustomerDocumentId", "dbo.CustomerDocument", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblReminders", "CustomerDocumentId", "dbo.CustomerDocument");
            DropIndex("dbo.tblReminders", new[] { "CustomerDocumentId" });
            DropColumn("dbo.tblReminders", "CustomerDocumentId");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerDocumentsNotifications : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CustomerDocument", "IDX_MaintenanceDocument");
            AddColumn("dbo.CustomerDocument", "DocumentExpires", c => c.DateTime());
            AddColumn("dbo.CustomerDocument", "ExpireNotification", c => c.DateTime());
            AddColumn("dbo.CustomerDocument", "ExpireNotificationDisabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomerDocumentType", "SetExpiry", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomerDocumentType", "DefaultExpiryMonths", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomerDocument", "CustomerDocumentTypeId", name: "IDX_MaintenanceDocument");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CustomerDocument", "IDX_MaintenanceDocument");
            DropColumn("dbo.CustomerDocumentType", "DefaultExpiryMonths");
            DropColumn("dbo.CustomerDocumentType", "SetExpiry");
            DropColumn("dbo.CustomerDocument", "ExpireNotificationDisabled");
            DropColumn("dbo.CustomerDocument", "ExpireNotification");
            DropColumn("dbo.CustomerDocument", "DocumentExpires");
            CreateIndex("dbo.CustomerDocument", "CustomerDocumentTypeId", name: "IDX_MaintenanceDocument");
        }
    }
}

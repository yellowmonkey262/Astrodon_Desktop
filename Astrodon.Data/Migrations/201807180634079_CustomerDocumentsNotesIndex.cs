namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerDocumentsNotesIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CustomerDocument", "IDX_CustomerDocument");
            DropIndex("dbo.CustomerDocument", "IDX_MaintenanceDocument");
            CreateIndex("dbo.CustomerDocument", new[] { "CustomerId", "CustomerDocumentTypeId", "DocumentExpires", "ExpireNotificationDisabled" }, name: "IDX_CustomerDocument");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CustomerDocument", "IDX_CustomerDocument");
            CreateIndex("dbo.CustomerDocument", "CustomerDocumentTypeId", name: "IDX_MaintenanceDocument");
            CreateIndex("dbo.CustomerDocument", "CustomerId", name: "IDX_CustomerDocument");
        }
    }
}

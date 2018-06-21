namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerDocument",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CustomerDocumentTypeId = c.Int(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 500),
                        Notes = c.String(nullable: false, maxLength: 500),
                        FileData = c.Binary(),
                        Uploaded = c.DateTime(nullable: false),
                        UploadedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .ForeignKey("dbo.CustomerDocumentType", t => t.CustomerDocumentTypeId)
                .ForeignKey("dbo.tblUsers", t => t.UploadedUserId)
                .Index(t => t.CustomerId, name: "IDX_CustomerDocument")
                .Index(t => t.CustomerDocumentTypeId, name: "IDX_MaintenanceDocument")
                .Index(t => t.UploadedUserId);
            
            CreateTable(
                "dbo.CustomerDocumentType",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.Name, unique: true, name: "IDX_CustomerDocumentType");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerDocument", "UploadedUserId", "dbo.tblUsers");
            DropForeignKey("dbo.CustomerDocument", "CustomerDocumentTypeId", "dbo.CustomerDocumentType");
            DropForeignKey("dbo.CustomerDocument", "CustomerId", "dbo.Customer");
            DropIndex("dbo.CustomerDocumentType", "IDX_CustomerDocumentType");
            DropIndex("dbo.CustomerDocument", new[] { "UploadedUserId" });
            DropIndex("dbo.CustomerDocument", "IDX_MaintenanceDocument");
            DropIndex("dbo.CustomerDocument", "IDX_CustomerDocument");
            DropTable("dbo.CustomerDocumentType");
            DropTable("dbo.CustomerDocument");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaintenanceTracking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BuildingMaintenanceConfiguration",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        PastelAccountNumber = c.String(nullable: false, maxLength: 200),
                        Name = c.String(nullable: false, maxLength: 200),
                        MaintenanceClassificationType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .Index(t => new { t.BuildingId, t.PastelAccountNumber }, unique: true, name: "UIDX_BuildingMaintenanceConfiguration");
            
            CreateTable(
                "dbo.Maintenance",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RequisitionId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        BuildingMaintenanceConfigurationId = c.Int(nullable: false),
                        CustomerAccount = c.String(maxLength: 10),
                        IsForBodyCorporate = c.Boolean(nullable: false),
                        DateLogged = c.DateTime(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        InvoiceNumber = c.String(),
                        WarrantyDuration = c.Int(),
                        WarrantyDurationType = c.Int(),
                        WarrentyExpires = c.DateTime(),
                        WarrantySerialNumber = c.String(),
                        WarrantyNotes = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.BuildingMaintenanceConfiguration", t => t.BuildingMaintenanceConfigurationId)
                .ForeignKey("dbo.tblRequisition", t => t.RequisitionId)
                .ForeignKey("dbo.Supplier", t => t.SupplierId)
                .Index(t => t.RequisitionId, unique: true, name: "UIDX_Maintenance")
                .Index(t => t.SupplierId)
                .Index(t => t.BuildingMaintenanceConfigurationId);
            
            CreateTable(
                "dbo.MaintenanceDocument",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        MaintenanceId = c.Int(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 500),
                        FileData = c.Binary(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Maintenance", t => t.MaintenanceId)
                .Index(t => t.MaintenanceId, name: "IDX_MaintenanceDocument");
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 200),
                        CompanyRegistration = c.String(maxLength: 200),
                        VATNumber = c.String(maxLength: 200),
                        ContactPerson = c.String(nullable: false, maxLength: 200),
                        EmailAddress = c.String(),
                        ContactNumber = c.String(maxLength: 200),
                        BankName = c.String(nullable: false, maxLength: 200),
                        BranchName = c.String(nullable: false, maxLength: 200),
                        BranceCode = c.String(nullable: false, maxLength: 200),
                        AccountNumber = c.String(nullable: false, maxLength: 200),
                        BlackListed = c.Boolean(nullable: false),
                        BlackListReason = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SupplierAudit",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        AuditTimeStamp = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        FieldName = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Supplier", t => t.SupplierId)
                .ForeignKey("dbo.tblUsers", t => t.UserId)
                .Index(t => new { t.SupplierId, t.AuditTimeStamp }, name: "IDX_SupplierAudit")
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Maintenance", "SupplierId", "dbo.Supplier");
            DropForeignKey("dbo.SupplierAudit", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.SupplierAudit", "SupplierId", "dbo.Supplier");
            DropForeignKey("dbo.Maintenance", "RequisitionId", "dbo.tblRequisition");
            DropForeignKey("dbo.MaintenanceDocument", "MaintenanceId", "dbo.Maintenance");
            DropForeignKey("dbo.Maintenance", "BuildingMaintenanceConfigurationId", "dbo.BuildingMaintenanceConfiguration");
            DropForeignKey("dbo.BuildingMaintenanceConfiguration", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.SupplierAudit", new[] { "UserId" });
            DropIndex("dbo.SupplierAudit", "IDX_SupplierAudit");
            DropIndex("dbo.MaintenanceDocument", "IDX_MaintenanceDocument");
            DropIndex("dbo.Maintenance", new[] { "BuildingMaintenanceConfigurationId" });
            DropIndex("dbo.Maintenance", new[] { "SupplierId" });
            DropIndex("dbo.Maintenance", "UIDX_Maintenance");
            DropIndex("dbo.BuildingMaintenanceConfiguration", "UIDX_BuildingMaintenanceConfiguration");
            DropTable("dbo.SupplierAudit");
            DropTable("dbo.Supplier");
            DropTable("dbo.MaintenanceDocument");
            DropTable("dbo.Maintenance");
            DropTable("dbo.BuildingMaintenanceConfiguration");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaintenanceReportPhase2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAudit",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BankId = c.Int(nullable: false),
                        AuditTimeStamp = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        FieldName = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Bank", t => t.BankId)
                .ForeignKey("dbo.tblUsers", t => t.UserId)
                .Index(t => new { t.BankId, t.AuditTimeStamp }, name: "IDX_BankAudit")
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Bank",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        BranchName = c.String(maxLength: 200),
                        BranchCode = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.Name, unique: true, name: "IDX_Bank");
            
            CreateTable(
                "dbo.SupplierBuilding",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        BuildingId = c.Int(nullable: false),
                        BankId = c.Int(nullable: false),
                        BranchName = c.String(nullable: false, maxLength: 200),
                        BranceCode = c.String(nullable: false, maxLength: 200),
                        AccountNumber = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Bank", t => t.BankId)
                .ForeignKey("dbo.tblBuildings", t => t.SupplierId)
                .ForeignKey("dbo.Supplier", t => t.SupplierId)
                .Index(t => new { t.SupplierId, t.BuildingId }, unique: true, name: "IDX_SupplierBuilding")
                .Index(t => t.BankId);
            
            CreateTable(
                "dbo.RequisitionDocument",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RequisitionId = c.Int(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 500),
                        FileData = c.Binary(),
                        IsInvoice = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblRequisition", t => t.RequisitionId)
                .Index(t => t.RequisitionId, name: "IDX_RequisitionDocument");
            
            AddColumn("dbo.Supplier", "BlacklistedUserId", c => c.Int());
            CreateIndex("dbo.Supplier", "BlacklistedUserId");
            AddForeignKey("dbo.Supplier", "BlacklistedUserId", "dbo.tblUsers", "id");
            DropColumn("dbo.Supplier", "BankName");
            DropColumn("dbo.Supplier", "BranchName");
            DropColumn("dbo.Supplier", "BranceCode");
            DropColumn("dbo.Supplier", "AccountNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Supplier", "AccountNumber", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Supplier", "BranceCode", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Supplier", "BranchName", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Supplier", "BankName", c => c.String(nullable: false, maxLength: 200));
            DropForeignKey("dbo.BankAudit", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.RequisitionDocument", "RequisitionId", "dbo.tblRequisition");
            DropForeignKey("dbo.SupplierBuilding", "SupplierId", "dbo.Supplier");
            DropForeignKey("dbo.Supplier", "BlacklistedUserId", "dbo.tblUsers");
            DropForeignKey("dbo.SupplierBuilding", "SupplierId", "dbo.tblBuildings");
            DropForeignKey("dbo.SupplierBuilding", "BankId", "dbo.Bank");
            DropForeignKey("dbo.BankAudit", "BankId", "dbo.Bank");
            DropIndex("dbo.RequisitionDocument", "IDX_RequisitionDocument");
            DropIndex("dbo.Supplier", new[] { "BlacklistedUserId" });
            DropIndex("dbo.SupplierBuilding", new[] { "BankId" });
            DropIndex("dbo.SupplierBuilding", "IDX_SupplierBuilding");
            DropIndex("dbo.Bank", "IDX_Bank");
            DropIndex("dbo.BankAudit", new[] { "UserId" });
            DropIndex("dbo.BankAudit", "IDX_BankAudit");
            DropColumn("dbo.Supplier", "BlacklistedUserId");
            DropTable("dbo.RequisitionDocument");
            DropTable("dbo.SupplierBuilding");
            DropTable("dbo.Bank");
            DropTable("dbo.BankAudit");
        }
    }
}

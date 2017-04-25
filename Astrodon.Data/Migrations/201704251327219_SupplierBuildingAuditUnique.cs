namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierBuildingAuditUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SupplierBuildingAudit", "IDX_SupplierBuildingAudit");
            CreateIndex("dbo.SupplierBuildingAudit", new[] { "SupplierBuildingId", "AuditTimeStamp" }, name: "IDX_SupplierBuildingAudit");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SupplierBuildingAudit", "IDX_SupplierBuildingAudit");
            CreateIndex("dbo.SupplierBuildingAudit", new[] { "SupplierBuildingId", "AuditTimeStamp" }, unique: true, name: "IDX_SupplierBuildingAudit");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierBuildingAudit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupplierBuildingAudit",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        SupplierBuildingId = c.Int(nullable: false),
                        AuditTimeStamp = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        FieldName = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.SupplierBuilding", t => t.SupplierBuildingId)
                .ForeignKey("dbo.tblUsers", t => t.UserId)
                .Index(t => new { t.SupplierBuildingId, t.AuditTimeStamp }, unique: true, name: "IDX_SupplierBuildingAudit")
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierBuildingAudit", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.SupplierBuildingAudit", "SupplierBuildingId", "dbo.SupplierBuilding");
            DropIndex("dbo.SupplierBuildingAudit", new[] { "UserId" });
            DropIndex("dbo.SupplierBuildingAudit", "IDX_SupplierBuildingAudit");
            DropTable("dbo.SupplierBuildingAudit");
        }
    }
}

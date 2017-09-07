namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagementPackImprovements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManagementPack",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        Period = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        ReportData = c.Binary(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .ForeignKey("dbo.tblUsers", t => t.UserId)
                .Index(t => new { t.BuildingId, t.Period }, unique: true, name: "UIDX_ManagementPack")
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ManagementPackTOCItem",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.Description, unique: true, name: "IDX_ManagementPackTOCItem");
            
            AddColumn("dbo.tblMonthFin", "CheckListPDF", c => c.Binary());
            CreateIndex("dbo.tblMonthFin", new[] { "buildingID", "year" }, name: "IDX_tblMonthFinBuilding");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ManagementPack", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.ManagementPack", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.tblMonthFin", "IDX_tblMonthFinBuilding");
            DropIndex("dbo.ManagementPackTOCItem", "IDX_ManagementPackTOCItem");
            DropIndex("dbo.ManagementPack", new[] { "UserId" });
            DropIndex("dbo.ManagementPack", "UIDX_ManagementPack");
            DropColumn("dbo.tblMonthFin", "CheckListPDF");
            DropTable("dbo.ManagementPackTOCItem");
            DropTable("dbo.ManagementPack");
        }
    }
}

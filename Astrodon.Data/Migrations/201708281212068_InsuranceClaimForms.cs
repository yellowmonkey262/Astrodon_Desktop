namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsuranceClaimForms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BuildingDocument",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        DocumentType = c.Int(nullable: false),
                        FileName = c.String(nullable: false),
                        FileData = c.Binary(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .Index(t => new { t.BuildingId, t.DocumentType }, unique: true, name: "IDX_BuildingDocumentBuildingId");
            
            DropColumn("dbo.tblBuildings", "InsuranceContract");
            DropColumn("dbo.tblBuildings", "InsuranceClaimForm");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblBuildings", "InsuranceClaimForm", c => c.Binary());
            AddColumn("dbo.tblBuildings", "InsuranceContract", c => c.Binary());
            DropForeignKey("dbo.BuildingDocument", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.BuildingDocument", "IDX_BuildingDocumentBuildingId");
            DropTable("dbo.BuildingDocument");
        }
    }
}

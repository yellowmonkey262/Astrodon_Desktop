namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingDocumentHistoryUploaded : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BuildingDocument", "IDX_BuildingDocumentBuildingId");
            AlterColumn("dbo.BuildingDocument", "DateUploaded", c => c.DateTime(nullable: false));
            CreateIndex("dbo.BuildingDocument", new[] { "BuildingId", "DocumentType", "DateUploaded" }, unique: true, name: "IDX_BuildingDocumentBuildingId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BuildingDocument", "IDX_BuildingDocumentBuildingId");
            AlterColumn("dbo.BuildingDocument", "DateUploaded", c => c.DateTime());
            CreateIndex("dbo.BuildingDocument", new[] { "BuildingId", "DocumentType" }, unique: true, name: "IDX_BuildingDocumentBuildingId");
        }
    }
}

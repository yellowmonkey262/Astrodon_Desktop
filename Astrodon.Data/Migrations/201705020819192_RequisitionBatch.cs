namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionBatch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequisitionBatch",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        BatchNumber = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        Entries = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .ForeignKey("dbo.tblUsers", t => t.UserId)
                .Index(t => new { t.BuildingId, t.BatchNumber }, unique: true, name: "IDX_RequisitionBatch")
                .Index(t => t.UserId);
            
            AddColumn("dbo.tblRequisition", "RequisitionBatchId", c => c.Int());
            CreateIndex("dbo.tblRequisition", "RequisitionBatchId", name: "IDX_tblRequisitionBatchId");
            AddForeignKey("dbo.tblRequisition", "RequisitionBatchId", "dbo.RequisitionBatch", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequisitionBatch", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.tblRequisition", "RequisitionBatchId", "dbo.RequisitionBatch");
            DropForeignKey("dbo.RequisitionBatch", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.RequisitionBatch", new[] { "UserId" });
            DropIndex("dbo.RequisitionBatch", "IDX_RequisitionBatch");
            DropIndex("dbo.tblRequisition", "IDX_tblRequisitionBatchId");
            DropColumn("dbo.tblRequisition", "RequisitionBatchId");
            DropTable("dbo.RequisitionBatch");
        }
    }
}

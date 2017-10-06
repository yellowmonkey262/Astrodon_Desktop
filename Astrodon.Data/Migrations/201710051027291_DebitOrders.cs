namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DebitOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerDebitOrder",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        CustomerCode = c.String(nullable: false, maxLength: 200),
                        BranchCode = c.String(nullable: false),
                        AccountType = c.Int(nullable: false),
                        AccountNumber = c.String(nullable: false),
                        DebitOrderCollectionDay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .Index(t => new { t.BuildingId, t.CustomerCode }, unique: true, name: "UIDX_CustomerDebitOrder");
            
            CreateTable(
                "dbo.DebitOrderDocument",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CustomerDebitOrderId = c.Int(nullable: false),
                        DocumentType = c.Int(nullable: false),
                        FileName = c.String(nullable: false),
                        FileData = c.Binary(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CustomerDebitOrder", t => t.CustomerDebitOrderId)
                .Index(t => new { t.CustomerDebitOrderId, t.DocumentType }, unique: true, name: "IDX_DebitOrderDocument");
            
            AddColumn("dbo.tblBuildingSettings", "DebitOrderFee", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue:41.54M));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DebitOrderDocument", "CustomerDebitOrderId", "dbo.CustomerDebitOrder");
            DropForeignKey("dbo.CustomerDebitOrder", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.DebitOrderDocument", "IDX_DebitOrderDocument");
            DropIndex("dbo.CustomerDebitOrder", "UIDX_CustomerDebitOrder");
            DropColumn("dbo.tblBuildingSettings", "DebitOrderFee");
            DropTable("dbo.DebitOrderDocument");
            DropTable("dbo.CustomerDebitOrder");
        }
    }
}

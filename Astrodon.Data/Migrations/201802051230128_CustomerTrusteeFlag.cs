namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerTrusteeFlag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        AccountNumber = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        IsTrustee = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .Index(t => new { t.BuildingId, t.AccountNumber }, unique: true, name: "UIDX_CustomerUnit");
            
            AddColumn("dbo.ManagementPack", "SubmitForApproval", c => c.Boolean(nullable: false,defaultValue:false));
            AddColumn("dbo.ManagementPack", "Submitted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.Customer", "UIDX_CustomerUnit");
            DropColumn("dbo.ManagementPack", "Submitted");
            DropColumn("dbo.ManagementPack", "SubmitForApproval");
            DropTable("dbo.Customer");
        }
    }
}

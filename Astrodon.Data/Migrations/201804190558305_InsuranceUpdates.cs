namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsuranceUpdates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BondOriginator",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 300),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.CompanyName, unique: true, name: "IDX_BondOriginator");
            
            AddColumn("dbo.tblBuildings", "InsurancePolicyRenewalDate", c => c.DateTime());
            AddColumn("dbo.tblBuildings", "ExcessStructures", c => c.String());
            AddColumn("dbo.BuildingUnit", "BondOriginatorInterestNoted", c => c.Boolean(nullable: false,defaultValue:false));
            AddColumn("dbo.BuildingUnit", "BondOriginatorId", c => c.Int());
            CreateIndex("dbo.InsuranceBroker", "CompanyName", unique: true, name: "IDX_InsuarnceBroker");
            CreateIndex("dbo.BuildingUnit", "BondOriginatorId");
            AddForeignKey("dbo.BuildingUnit", "BondOriginatorId", "dbo.BondOriginator", "id");
            DropColumn("dbo.tblBuildings", "BondHolderInterestNotedOnPolicy");
            DropColumn("dbo.tblBuildings", "InsuranceBondHolder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblBuildings", "InsuranceBondHolder", c => c.String());
            AddColumn("dbo.tblBuildings", "BondHolderInterestNotedOnPolicy", c => c.Boolean(nullable: false,defaultValue:false));
            DropForeignKey("dbo.BuildingUnit", "BondOriginatorId", "dbo.BondOriginator");
            DropIndex("dbo.BuildingUnit", new[] { "BondOriginatorId" });
            DropIndex("dbo.BondOriginator", "IDX_BondOriginator");
            DropIndex("dbo.InsuranceBroker", "IDX_InsuarnceBroker");
            DropColumn("dbo.BuildingUnit", "BondOriginatorId");
            DropColumn("dbo.BuildingUnit", "BondOriginatorInterestNoted");
            DropColumn("dbo.tblBuildings", "ExcessStructures");
            DropColumn("dbo.tblBuildings", "InsurancePolicyRenewalDate");
            DropTable("dbo.BondOriginator");
        }
    }
}

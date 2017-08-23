namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBuildingEntities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "CommonPropertyDimensions", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tblBuildings", "UnitPropertyDimensions", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tblBuildings", "UnitReplacementCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tblBuildings", "CommonPropertyReplacementCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tblBuildings", "InsuranceCompanyName", c => c.String());
            AddColumn("dbo.tblBuildings", "InsuranceAccountNumber", c => c.String());
            AddColumn("dbo.tblBuildings", "BrokerName", c => c.String());
            AddColumn("dbo.tblBuildings", "BrokerTelNumber", c => c.String());
            AddColumn("dbo.tblBuildings", "BrokerEmail", c => c.String());
            AddColumn("dbo.tblBuildings", "InsuranceContract", c => c.Binary());
            AddColumn("dbo.tblBuildings", "InsuranceClaimForm", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "InsuranceClaimForm");
            DropColumn("dbo.tblBuildings", "InsuranceContract");
            DropColumn("dbo.tblBuildings", "BrokerEmail");
            DropColumn("dbo.tblBuildings", "BrokerTelNumber");
            DropColumn("dbo.tblBuildings", "BrokerName");
            DropColumn("dbo.tblBuildings", "InsuranceAccountNumber");
            DropColumn("dbo.tblBuildings", "InsuranceCompanyName");
            DropColumn("dbo.tblBuildings", "CommonPropertyReplacementCost");
            DropColumn("dbo.tblBuildings", "UnitReplacementCost");
            DropColumn("dbo.tblBuildings", "UnitPropertyDimensions");
            DropColumn("dbo.tblBuildings", "CommonPropertyDimensions");
        }
    }
}

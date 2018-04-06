namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnitPremiumCalculation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "MonthlyInsurancePremium", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue:0));
            AddColumn("dbo.BuildingUnit", "UnitPremium", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuildingUnit", "UnitPremium");
            DropColumn("dbo.tblBuildings", "MonthlyInsurancePremium");
        }
    }
}

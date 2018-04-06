namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnitAdditionalPremium : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuildingUnit", "AdditionalPremium", c => c.Decimal(nullable: false, precision: 18, scale: 2,defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuildingUnit", "AdditionalPremium");
        }
    }
}

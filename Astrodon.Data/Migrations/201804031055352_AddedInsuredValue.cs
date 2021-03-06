namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInsuredValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "AdditionalInsuredValueCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "AdditionalInsuredValueCost");
        }
    }
}

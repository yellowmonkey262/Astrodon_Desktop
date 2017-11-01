namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingFinancialsEnabled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "BuildingFinancialsEnabled", c => c.Boolean(nullable: false,defaultValue:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "BuildingFinancialsEnabled");
        }
    }
}

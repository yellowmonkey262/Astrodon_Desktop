namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingActiveFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "BuildingDisabled", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "BuildingDisabled");
        }
    }
}

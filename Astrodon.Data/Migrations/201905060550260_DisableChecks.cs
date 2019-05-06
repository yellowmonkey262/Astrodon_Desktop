namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisableChecks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "DisableCSOSCheck", c => c.Boolean(nullable: false,defaultValue:false));
            AddColumn("dbo.tblBuildings", "DisableInsuranceCheck", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "DisableInsuranceCheck");
            DropColumn("dbo.tblBuildings", "DisableCSOSCheck");
        }
    }
}

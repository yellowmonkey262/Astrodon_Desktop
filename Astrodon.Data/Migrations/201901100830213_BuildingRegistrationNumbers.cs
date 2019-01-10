namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingRegistrationNumbers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "BuildingRegistrationNumber", c => c.String(maxLength: 200));
            AddColumn("dbo.tblBuildings", "CSOSRegistrationNumber", c => c.String(maxLength: 200));
            AddColumn("dbo.tblBuildings", "ODBCConnectionOK", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.tblBuildings", "LastODBConnectionTest", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "LastODBConnectionTest");
            DropColumn("dbo.tblBuildings", "ODBCConnectionOK");
            DropColumn("dbo.tblBuildings", "CSOSRegistrationNumber");
            DropColumn("dbo.tblBuildings", "BuildingRegistrationNumber");
        }
    }
}

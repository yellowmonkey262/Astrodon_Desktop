namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Astrodon2018 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tblBuildings", "Code", unique: true, name: "IDX_BuildingCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tblBuildings", "IDX_BuildingCode");
        }
    }
}

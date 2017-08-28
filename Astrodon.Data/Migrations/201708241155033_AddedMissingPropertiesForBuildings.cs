namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMissingPropertiesForBuildings : DbMigration
    {
        public override void Up()
        {
            try
            {
            //    AddColumn("dbo.tblBuildings", "limitM", c => c.String());
            //    AddColumn("dbo.tblBuildings", "limitW", c => c.String());
            //    AddColumn("dbo.tblBuildings", "limitD", c => c.String());
            }
            catch (Exception)
            {
            }
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBuildings", "limitD");
            DropColumn("dbo.tblBuildings", "limitW");
            DropColumn("dbo.tblBuildings", "limitM");
        }
    }
}

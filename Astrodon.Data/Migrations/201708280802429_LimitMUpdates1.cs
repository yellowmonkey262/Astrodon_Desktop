namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LimitMUpdates1 : DbMigration
    {
        public override void Up()
        {
            try {
            //    AlterColumn("dbo.tblBuildings", "limitM", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            //    AlterColumn("dbo.tblBuildings", "limitW", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            //    AlterColumn("dbo.tblBuildings", "limitD", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            }
            catch
            {

            }
        }
        public override void Down()
        {
            AlterColumn("dbo.tblBuildings", "limitD", c => c.String());
            AlterColumn("dbo.tblBuildings", "limitW", c => c.String());
            AlterColumn("dbo.tblBuildings", "limitM", c => c.String());
        }
    }
}

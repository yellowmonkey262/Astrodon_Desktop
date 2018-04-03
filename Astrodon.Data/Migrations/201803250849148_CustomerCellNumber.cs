namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerCellNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "CellNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "CellNumber");
        }
    }
}

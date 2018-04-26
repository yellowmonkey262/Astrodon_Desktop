namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalCommentsOnAllocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMonthFin", "AdditionalComments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMonthFin", "AdditionalComments");
        }
    }
}

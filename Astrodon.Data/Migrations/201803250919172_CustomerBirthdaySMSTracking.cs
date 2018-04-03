namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerBirthdaySMSTracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "BirthdaySMSText", c => c.String());
            AddColumn("dbo.Customer", "BirthDaySMSStatus", c => c.String());
            AddColumn("dbo.Customer", "BirthDaySMSBatch", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "BirthDaySMSBatch");
            DropColumn("dbo.Customer", "BirthDaySMSStatus");
            DropColumn("dbo.Customer", "BirthdaySMSText");
        }
    }
}

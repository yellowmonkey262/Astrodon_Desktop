namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerEmailAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "EmailAddress1", c => c.String(maxLength: 200));
            AddColumn("dbo.Customer", "EmailAddress2", c => c.String(maxLength: 200));
            AddColumn("dbo.Customer", "EmailAddress3", c => c.String(maxLength: 200));
            AddColumn("dbo.Customer", "EmailAddress4", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "EmailAddress4");
            DropColumn("dbo.Customer", "EmailAddress3");
            DropColumn("dbo.Customer", "EmailAddress2");
            DropColumn("dbo.Customer", "EmailAddress1");
        }
    }
}

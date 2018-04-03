namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerBirthdays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "SendBirthdayNotification", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customer", "IDNumber", c => c.String());
            AddColumn("dbo.Customer", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.Customer", "CustomerFullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "CustomerFullName");
            DropColumn("dbo.Customer", "DateOfBirth");
            DropColumn("dbo.Customer", "IDNumber");
            DropColumn("dbo.Customer", "SendBirthdayNotification");
        }
    }
}

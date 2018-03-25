namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerBirthdayNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "LastBirthdayNotificationSent", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "LastBirthdayNotificationSent");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReminderContactDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblReminders", "Contacts", c => c.String());
            AddColumn("dbo.tblReminders", "Phone", c => c.String());
            AddColumn("dbo.tblReminders", "Fax", c => c.String());
            AddColumn("dbo.tblReminders", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblReminders", "Email");
            DropColumn("dbo.tblReminders", "Fax");
            DropColumn("dbo.tblReminders", "Phone");
            DropColumn("dbo.tblReminders", "Contacts");
        }
    }
}

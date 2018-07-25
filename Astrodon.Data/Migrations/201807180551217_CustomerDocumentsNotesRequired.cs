namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerDocumentsNotesRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomerDocument", "Notes", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerDocument", "Notes", c => c.String(nullable: false, maxLength: 500));
        }
    }
}

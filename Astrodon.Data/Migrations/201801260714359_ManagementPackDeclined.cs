namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagementPackDeclined : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManagementPack", "Declined", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManagementPack", "Declined");
        }
    }
}

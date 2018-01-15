namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagementPackPublishedFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManagementPack", "Published", c => c.Boolean(nullable: false,defaultValue:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManagementPack", "Published");
        }
    }
}

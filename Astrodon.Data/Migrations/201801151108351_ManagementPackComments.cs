namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagementPackComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManagementPack", "Commments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManagementPack", "Commments");
        }
    }
}

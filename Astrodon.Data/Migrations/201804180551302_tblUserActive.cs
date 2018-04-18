namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblUserActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblUsers", "Active", c => c.Boolean(nullable: false, defaultValue:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblUsers", "Active");
        }
    }
}

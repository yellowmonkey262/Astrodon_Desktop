namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DebitOrderRemoveBranchName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CustomerDebitOrder", "BranchName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerDebitOrder", "BranchName", c => c.String(nullable: false, maxLength: 200));
        }
    }
}

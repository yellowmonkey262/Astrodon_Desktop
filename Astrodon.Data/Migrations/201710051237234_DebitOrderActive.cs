namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DebitOrderActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerDebitOrder", "IsActive", c => c.Boolean(nullable: false,defaultValue:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerDebitOrder", "IsActive");
        }
    }
}

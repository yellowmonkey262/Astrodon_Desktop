namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DebitOrderMaxAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerDebitOrder", "MaxDebitAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2,defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerDebitOrder", "MaxDebitAmount");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DebitOrderCancelDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerDebitOrder", "DebitOrderCancelDate", c => c.DateTime());
            AddColumn("dbo.CustomerDebitOrder", "DebitOrderCancelled", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerDebitOrder", "DebitOrderCancelled");
            DropColumn("dbo.CustomerDebitOrder", "DebitOrderCancelDate");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisableDebitOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildings", "IsDebitOrderFeeDisabled", c => c.Boolean(nullable: false,defaultValue:false));
            AddColumn("dbo.CustomerDebitOrder", "IsDebitOrderFeeDisabled", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerDebitOrder", "IsDebitOrderFeeDisabled");
            DropColumn("dbo.tblBuildings", "IsDebitOrderFeeDisabled");
        }
    }
}

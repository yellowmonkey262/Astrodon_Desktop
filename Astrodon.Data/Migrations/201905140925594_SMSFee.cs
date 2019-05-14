namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SMSFee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBuildingSettings", "SMSFee", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue:5.30M));
            AddColumn("dbo.tblSettings", "DefaultSMSFee", c => c.Decimal(nullable: false, precision: 18, scale: 2,defaultValue:5.30M));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblSettings", "DefaultSMSFee");
            DropColumn("dbo.tblBuildingSettings", "SMSFee");
        }
    }
}

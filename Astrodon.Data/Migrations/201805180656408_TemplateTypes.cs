namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationTemplate",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        TemplateType = c.Int(nullable: false),
                        TemplateName = c.String(nullable: false, maxLength: 50),
                        MessageText = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.TemplateType, name: "IDX_NotificationTemplateType");
            
            AddColumn("dbo.tblBuildings", "AdditionalPremiumValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tblBuildings", "InsurancePolicyExpiryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropIndex("dbo.NotificationTemplate", "IDX_NotificationTemplateType");
            DropColumn("dbo.tblBuildings", "InsurancePolicyExpiryDate");
            DropColumn("dbo.tblBuildings", "AdditionalPremiumValue");
            DropTable("dbo.NotificationTemplate");
        }
    }
}

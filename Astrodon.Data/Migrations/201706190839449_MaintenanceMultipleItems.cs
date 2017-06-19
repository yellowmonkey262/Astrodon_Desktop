namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaintenanceMultipleItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaintenanceDetailItem",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        MaintenanceId = c.Int(nullable: false),
                        CustomerAccount = c.String(nullable: false, maxLength: 50),
                        IsForBodyCorporate = c.Boolean(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Maintenance", t => t.MaintenanceId)
                .Index(t => new { t.MaintenanceId, t.CustomerAccount }, unique: true, name: "IDX_MaintenanceDetail");

            Sql("insert into MaintenanceDetailItem (MaintenanceId,CustomerAccount,IsForBodyCorporate,Amount) select id, case  IsForBodyCorporate  when 1 then 'BODY-CORPORATE'  else CustomerAccount end as CustomerAccount, IsForBodyCorporate, TotalAmount from Maintenance");

            DropColumn("dbo.Maintenance", "CustomerAccount");
            DropColumn("dbo.Maintenance", "IsForBodyCorporate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Maintenance", "IsForBodyCorporate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Maintenance", "CustomerAccount", c => c.String(maxLength: 10));
            DropForeignKey("dbo.MaintenanceDetailItem", "MaintenanceId", "dbo.Maintenance");
            DropIndex("dbo.MaintenanceDetailItem", "IDX_MaintenanceDetail");


            //DropTable("dbo.MaintenanceDetailItem"); --do not delete the data
        }
    }
}

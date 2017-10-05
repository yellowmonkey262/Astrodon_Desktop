namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DebitOrderAuditiing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerDebitOrder", "BankId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerDebitOrder", "BranchName", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.CustomerDebitOrder", "BranceCode", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.CustomerDebitOrder", "LastUpdatedByUserId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerDebitOrder", "LastUpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CustomerDebitOrder", "AccountNumber", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("dbo.CustomerDebitOrder", "BankId");
            CreateIndex("dbo.CustomerDebitOrder", "LastUpdatedByUserId");
            AddForeignKey("dbo.CustomerDebitOrder", "BankId", "dbo.Bank", "id");
            AddForeignKey("dbo.CustomerDebitOrder", "LastUpdatedByUserId", "dbo.tblUsers", "id");
            DropColumn("dbo.CustomerDebitOrder", "BranchCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerDebitOrder", "BranchCode", c => c.String(nullable: false));
            DropForeignKey("dbo.CustomerDebitOrder", "LastUpdatedByUserId", "dbo.tblUsers");
            DropForeignKey("dbo.CustomerDebitOrder", "BankId", "dbo.Bank");
            DropIndex("dbo.CustomerDebitOrder", new[] { "LastUpdatedByUserId" });
            DropIndex("dbo.CustomerDebitOrder", new[] { "BankId" });
            AlterColumn("dbo.CustomerDebitOrder", "AccountNumber", c => c.String(nullable: false));
            DropColumn("dbo.CustomerDebitOrder", "LastUpdateDate");
            DropColumn("dbo.CustomerDebitOrder", "LastUpdatedByUserId");
            DropColumn("dbo.CustomerDebitOrder", "BranceCode");
            DropColumn("dbo.CustomerDebitOrder", "BranchName");
            DropColumn("dbo.CustomerDebitOrder", "BankId");
        }
    }
}

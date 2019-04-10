namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerProfile : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SupplierBuilding", new[] { "BankId" });
            AddColumn("dbo.SupplierBuilding", "SpecialInstructions", c => c.String());
            AddColumn("dbo.Customer", "Portfolio", c => c.String());
            AlterColumn("dbo.SupplierBuilding", "BankId", c => c.Int());
            AlterColumn("dbo.SupplierBuilding", "BranchName", c => c.String(maxLength: 200));
            AlterColumn("dbo.SupplierBuilding", "BranceCode", c => c.String(maxLength: 200));
            AlterColumn("dbo.SupplierBuilding", "AccountNumber", c => c.String(maxLength: 200));
            CreateIndex("dbo.SupplierBuilding", "BankId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SupplierBuilding", new[] { "BankId" });
            AlterColumn("dbo.SupplierBuilding", "AccountNumber", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.SupplierBuilding", "BranceCode", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.SupplierBuilding", "BranchName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.SupplierBuilding", "BankId", c => c.Int(nullable: false));
            DropColumn("dbo.Customer", "Portfolio");
            DropColumn("dbo.SupplierBuilding", "SpecialInstructions");
            CreateIndex("dbo.SupplierBuilding", "BankId");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierBankDetailsBenificiary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupplierBuilding", "BeneficiarReferenceNumber", c => c.String(maxLength: 10));
            DropColumn("dbo.Supplier", "BeneficiarReferenceNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Supplier", "BeneficiarReferenceNumber", c => c.String(maxLength: 10));
            DropColumn("dbo.SupplierBuilding", "BeneficiarReferenceNumber");
        }
    }
}

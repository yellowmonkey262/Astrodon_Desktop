namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SupplierBankDetailsBenificiarySpellingError : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SupplierBuilding", "BeneficiarReferenceNumber");
            AddColumn("dbo.SupplierBuilding", "BeneficiaryReferenceNumber", c => c.String(maxLength: 10));
        }

        public override void Down()
        {
        }
    }
}

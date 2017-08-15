namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionCSV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Supplier", "BeneficiarReferenceNumber", c => c.String(maxLength: 10));
            AddColumn("dbo.tblRequisition", "NotifySupplierByEmail", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.tblRequisition", "NotifyEmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequisition", "NotifyEmailAddress");
            DropColumn("dbo.tblRequisition", "NotifySupplierByEmail");
            DropColumn("dbo.Supplier", "BeneficiarReferenceNumber");
        }
    }
}

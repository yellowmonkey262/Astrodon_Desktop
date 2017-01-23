namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionSupplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequisition", "SupplierId", c => c.Int());
            AddColumn("dbo.tblRequisition", "InvoiceNumber", c => c.String());
            AddColumn("dbo.tblRequisition", "InvoiceDate", c => c.DateTime());
            AddColumn("dbo.tblRequisition", "BankName", c => c.String());
            AddColumn("dbo.tblRequisition", "BranchCode", c => c.String());
            AddColumn("dbo.tblRequisition", "AccountNumber", c => c.String());
            CreateIndex("dbo.tblRequisition", "SupplierId");
            AddForeignKey("dbo.tblRequisition", "SupplierId", "dbo.Supplier", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRequisition", "SupplierId", "dbo.Supplier");
            DropIndex("dbo.tblRequisition", new[] { "SupplierId" });
            DropColumn("dbo.tblRequisition", "AccountNumber");
            DropColumn("dbo.tblRequisition", "BranchCode");
            DropColumn("dbo.tblRequisition", "BankName");
            DropColumn("dbo.tblRequisition", "InvoiceDate");
            DropColumn("dbo.tblRequisition", "InvoiceNumber");
            DropColumn("dbo.tblRequisition", "SupplierId");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionTransactionMatching : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequisition", "PastelLedgerAutoNumber", c => c.Int());
            AddColumn("dbo.tblRequisition", "PastelDataPath", c => c.String(maxLength: 200));
            AddColumn("dbo.tblRequisition", "PaymentLedgerAutoNumber", c => c.Int());
            AddColumn("dbo.tblRequisition", "PaymentDataPath", c => c.String(maxLength: 200));
            CreateIndex("dbo.tblRequisition", new[] { "PastelLedgerAutoNumber", "PastelDataPath" }, name: "IDX_tblRequisitionPastelLink");
            CreateIndex("dbo.tblRequisition", new[] { "PaymentLedgerAutoNumber", "PaymentDataPath" }, name: "IDX_tblRequisitionPaymentPastelLink");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tblRequisition", "IDX_tblRequisitionPaymentPastelLink");
            DropIndex("dbo.tblRequisition", "IDX_tblRequisitionPastelLink");
            DropColumn("dbo.tblRequisition", "PaymentDataPath");
            DropColumn("dbo.tblRequisition", "PaymentLedgerAutoNumber");
            DropColumn("dbo.tblRequisition", "PastelDataPath");
            DropColumn("dbo.tblRequisition", "PastelLedgerAutoNumber");
        }
    }
}

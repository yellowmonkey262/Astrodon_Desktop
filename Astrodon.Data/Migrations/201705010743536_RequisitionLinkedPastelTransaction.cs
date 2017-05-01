namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionLinkedPastelTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequisition", "PastelLedgerAutoNumber", c => c.Int());
            AddColumn("dbo.tblRequisition", "PastelDataPath", c => c.String(maxLength: 200));
            CreateIndex("dbo.tblRequisition", new[] { "PastelLedgerAutoNumber", "PastelDataPath" }, name: "IDX_tblRequisitionPastelLink");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tblRequisition", "IDX_tblRequisitionPastelLink");
            DropColumn("dbo.tblRequisition", "PastelDataPath");
            DropColumn("dbo.tblRequisition", "PastelLedgerAutoNumber");
        }
    }
}

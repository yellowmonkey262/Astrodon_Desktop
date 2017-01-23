namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionSupplierBankDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequisition", "BranchName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequisition", "BranchName");
        }
    }
}

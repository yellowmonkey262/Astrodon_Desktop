namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionEnabledForCSV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequisition", "NedbankCSVBenificiaryReferenceNumber", c => c.String());
            AddColumn("dbo.tblRequisition", "UseNedbankCSV", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequisition", "UseNedbankCSV");
            DropColumn("dbo.tblRequisition", "NedbankCSVBenificiaryReferenceNumber");
        }
    }
}

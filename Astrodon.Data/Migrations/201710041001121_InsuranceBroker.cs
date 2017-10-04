namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsuranceBroker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InsuranceBroker",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 200),
                        CompanyRegistration = c.String(maxLength: 200),
                        VATNumber = c.String(maxLength: 200),
                        ContactPerson = c.String(nullable: false, maxLength: 200),
                        EmailAddress = c.String(),
                        ContactNumber = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.tblBuildings", "PolicyNumber", c => c.String());
            AddColumn("dbo.tblBuildings", "InsuranceReplacementValueIncludesCommonProperty", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblBuildings", "BondHolderInterestNotedOnPolicy", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblBuildings", "InsuranceBondHolder", c => c.String());
            AddColumn("dbo.tblBuildings", "InsuranceBrokerId", c => c.Int());
            CreateIndex("dbo.tblBuildings", "InsuranceBrokerId");
            AddForeignKey("dbo.tblBuildings", "InsuranceBrokerId", "dbo.InsuranceBroker", "id");
            DropColumn("dbo.tblBuildings", "InsuranceCompanyName");
            DropColumn("dbo.tblBuildings", "InsuranceAccountNumber");
            DropColumn("dbo.tblBuildings", "BrokerName");
            DropColumn("dbo.tblBuildings", "BrokerTelNumber");
            DropColumn("dbo.tblBuildings", "BrokerEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblBuildings", "BrokerEmail", c => c.String());
            AddColumn("dbo.tblBuildings", "BrokerTelNumber", c => c.String());
            AddColumn("dbo.tblBuildings", "BrokerName", c => c.String());
            AddColumn("dbo.tblBuildings", "InsuranceAccountNumber", c => c.String());
            AddColumn("dbo.tblBuildings", "InsuranceCompanyName", c => c.String());
            DropForeignKey("dbo.tblBuildings", "InsuranceBrokerId", "dbo.InsuranceBroker");
            DropIndex("dbo.tblBuildings", new[] { "InsuranceBrokerId" });
            DropColumn("dbo.tblBuildings", "InsuranceBrokerId");
            DropColumn("dbo.tblBuildings", "InsuranceBondHolder");
            DropColumn("dbo.tblBuildings", "BondHolderInterestNotedOnPolicy");
            DropColumn("dbo.tblBuildings", "InsuranceReplacementValueIncludesCommonProperty");
            DropColumn("dbo.tblBuildings", "PolicyNumber");
            DropTable("dbo.InsuranceBroker");
        }
    }
}

namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersistManagementReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManagementPackReportItems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ManagementPackId = c.Int(nullable: false),
                        Path = c.String(nullable: false),
                        File = c.String(nullable: false),
                        Description = c.String(),
                        Description2 = c.String(),
                        Pages = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        FileDate = c.DateTime(nullable: false),
                        IsTempFile = c.Boolean(nullable: false),
                        FileData = c.Binary(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ManagementPack", t => t.ManagementPackId)
                .Index(t => t.ManagementPackId, name: "IDX_ManagementPackReportItem");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ManagementPackReportItems", "ManagementPackId", "dbo.ManagementPack");
            DropIndex("dbo.ManagementPackReportItems", "IDX_ManagementPackReportItem");
            DropTable("dbo.ManagementPackReportItems");
        }
    }
}

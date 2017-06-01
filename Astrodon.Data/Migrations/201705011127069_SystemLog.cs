namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemLogs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventTime = c.DateTime(nullable: false),
                        Message = c.String(nullable: false),
                        StackTrace = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemLogs");
        }
    }
}

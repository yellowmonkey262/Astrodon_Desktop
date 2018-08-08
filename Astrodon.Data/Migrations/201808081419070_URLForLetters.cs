namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class URLForLetters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblLetterRun", "URL", c => c.String());
            AddColumn("dbo.tblStatementRun", "URL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblStatementRun", "URL");
            DropColumn("dbo.tblLetterRun", "URL");
        }
    }
}

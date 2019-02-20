namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubmitLettersForReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblUsers", "SubmitLettersForReview", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblUsers", "SubmitLettersForReview");
        }
    }
}

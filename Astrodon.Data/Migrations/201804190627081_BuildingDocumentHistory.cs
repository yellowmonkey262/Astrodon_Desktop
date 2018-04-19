namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuildingDocumentHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuildingDocument", "DateUploaded", c => c.DateTime());
            Sql("Update BuildingDocument set DateUploaded = GetDate()");
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuildingDocument", "DateUploaded");
        }
    }
}

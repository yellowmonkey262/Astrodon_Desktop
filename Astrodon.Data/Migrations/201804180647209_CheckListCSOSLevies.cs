namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckListCSOSLevies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMonthFin", "csosLeviesExpense", c => c.Int());
            AddColumn("dbo.tblMonthFin", "csosLeviesNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMonthFin", "csosLeviesNotes");
            DropColumn("dbo.tblMonthFin", "csosLeviesExpense");
        }
    }
}

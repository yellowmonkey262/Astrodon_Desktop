namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckListProcessing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblUsers", "ProcessCheckLists", c => c.Boolean(nullable: false,defaultValue:true));
            AlterColumn("dbo.tblMonthFin", "completeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblMonthFin", "completeDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.tblUsers", "ProcessCheckLists");
        }
    }
}

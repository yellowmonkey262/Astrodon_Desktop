namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerDateOfBirth : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Customer", "DateOfBirth", name: "IDX_CustomerDateOfBirth");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customer", "IDX_CustomerDateOfBirth");
        }
    }
}

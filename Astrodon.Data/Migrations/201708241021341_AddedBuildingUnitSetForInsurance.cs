namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBuildingUnitSetForInsurance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BuildingUnit",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        UnitNo = c.String(nullable: false, maxLength: 200),
                        SquareMeters = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdditionalInsurance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PQRating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 1500),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .Index(t => new { t.BuildingId, t.UnitNo }, unique: true, name: "UIDX_BuildingUnit");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BuildingUnit", "BuildingId", "dbo.tblBuildings");
            DropIndex("dbo.BuildingUnit", "UIDX_BuildingUnit");
            DropTable("dbo.BuildingUnit");
        }
    }
}

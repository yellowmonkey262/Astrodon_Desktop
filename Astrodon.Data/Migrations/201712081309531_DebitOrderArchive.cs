namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DebitOrderArchive : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BuildingCalendarEntry", "IDX_BuildingCalendarEntry");
            CreateTable(
                "dbo.CalendarUserInvite",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CalendarEntryId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.BuildingCalendarEntry", t => t.CalendarEntryId)
                .ForeignKey("dbo.tblUsers", t => t.UserId)
                .Index(t => t.CalendarEntryId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CustomerDebitOrderArchive",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BuildingId = c.Int(nullable: false),
                        CustomerCode = c.String(nullable: false, maxLength: 200),
                        DateArchived = c.DateTime(nullable: false),
                        BankId = c.Int(nullable: false),
                        BranceCode = c.String(nullable: false, maxLength: 200),
                        AccountNumber = c.String(nullable: false, maxLength: 200),
                        AccountType = c.Int(nullable: false),
                        DebitOrderCollectionDay = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastUpdatedByUserId = c.Int(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                        IsDebitOrderFeeDisabled = c.Boolean(nullable: false),
                        DebitOrderCancelDate = c.DateTime(),
                        DebitOrderCancelled = c.Boolean(nullable: false),
                        MaxDebitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Bank", t => t.BankId)
                .ForeignKey("dbo.tblBuildings", t => t.BuildingId)
                .ForeignKey("dbo.tblUsers", t => t.LastUpdatedByUserId)
                .Index(t => new { t.BuildingId, t.CustomerCode }, name: "UIDX_CustomerDebitOrder")
                .Index(t => t.BankId)
                .Index(t => t.LastUpdatedByUserId);
            
            CreateTable(
                "dbo.DebitOrderDocumentArchive",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CustomerDebitOrderArchiveId = c.Int(nullable: false),
                        DocumentType = c.Int(nullable: false),
                        FileName = c.String(nullable: false),
                        FileData = c.Binary(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CustomerDebitOrderArchive", t => t.CustomerDebitOrderArchiveId)
                .Index(t => new { t.CustomerDebitOrderArchiveId, t.DocumentType }, name: "IDX_DDebitOrderDocumentArchive");
            
            AddColumn("dbo.BuildingCalendarEntry", "CalendarEntryType", c => c.Int(nullable: false, defaultValue:0));
            AddColumn("dbo.tblBuildings", "FinancialStartDate", c => c.DateTime());
            AddColumn("dbo.tblBuildings", "FinancialEndDate", c => c.DateTime());
            AddColumn("dbo.tblBuildings", "FixedMonthyFinancialMeetingEnabled", c => c.Boolean(nullable: false,defaultValue:false));
            AddColumn("dbo.tblBuildings", "FinancialMeetingDayOfMonth", c => c.Int());
            AddColumn("dbo.tblBuildings", "FinancialMeetingSubject", c => c.String());
            AddColumn("dbo.tblBuildings", "FinancialMeetingBodyText", c => c.String());
            AddColumn("dbo.tblBuildings", "FinancialMeetingSendInviteToAllTrustees", c => c.Boolean(nullable: false,defaultValue:false));
            AlterColumn("dbo.BuildingCalendarEntry", "BuildingId", c => c.Int());
            CreateIndex("dbo.BuildingCalendarEntry", new[] { "CalendarEntryType", "BuildingId", "UserId" }, name: "IDX_BuildingCalendarEntry");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerDebitOrderArchive", "LastUpdatedByUserId", "dbo.tblUsers");
            DropForeignKey("dbo.DebitOrderDocumentArchive", "CustomerDebitOrderArchiveId", "dbo.CustomerDebitOrderArchive");
            DropForeignKey("dbo.CustomerDebitOrderArchive", "BuildingId", "dbo.tblBuildings");
            DropForeignKey("dbo.CustomerDebitOrderArchive", "BankId", "dbo.Bank");
            DropForeignKey("dbo.CalendarUserInvite", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.CalendarUserInvite", "CalendarEntryId", "dbo.BuildingCalendarEntry");
            DropIndex("dbo.DebitOrderDocumentArchive", "IDX_DDebitOrderDocumentArchive");
            DropIndex("dbo.CustomerDebitOrderArchive", new[] { "LastUpdatedByUserId" });
            DropIndex("dbo.CustomerDebitOrderArchive", new[] { "BankId" });
            DropIndex("dbo.CustomerDebitOrderArchive", "UIDX_CustomerDebitOrder");
            DropIndex("dbo.CalendarUserInvite", new[] { "UserId" });
            DropIndex("dbo.CalendarUserInvite", new[] { "CalendarEntryId" });
            DropIndex("dbo.BuildingCalendarEntry", "IDX_BuildingCalendarEntry");
            AlterColumn("dbo.BuildingCalendarEntry", "BuildingId", c => c.Int(nullable: false));
            DropColumn("dbo.tblBuildings", "FinancialMeetingSendInviteToAllTrustees");
            DropColumn("dbo.tblBuildings", "FinancialMeetingBodyText");
            DropColumn("dbo.tblBuildings", "FinancialMeetingSubject");
            DropColumn("dbo.tblBuildings", "FinancialMeetingDayOfMonth");
            DropColumn("dbo.tblBuildings", "FixedMonthyFinancialMeetingEnabled");
            DropColumn("dbo.tblBuildings", "FinancialEndDate");
            DropColumn("dbo.tblBuildings", "FinancialStartDate");
            DropColumn("dbo.BuildingCalendarEntry", "CalendarEntryType");
            DropTable("dbo.DebitOrderDocumentArchive");
            DropTable("dbo.CustomerDebitOrderArchive");
            DropTable("dbo.CalendarUserInvite");
            CreateIndex("dbo.BuildingCalendarEntry", new[] { "BuildingId", "UserId" }, name: "IDX_BuildingCalendarEntry");
        }
    }
}

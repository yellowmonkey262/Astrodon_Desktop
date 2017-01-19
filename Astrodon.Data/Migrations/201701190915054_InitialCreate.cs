namespace Astrodon.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblAttachments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        jobID = c.Int(nullable: false),
                        fileName = c.String(nullable: false),
                        fileRTF = c.String(storeType: "ntext"),
                        fileContent = c.Binary(),
                        attachmentType = c.Int(nullable: false),
                        justify = c.Boolean(nullable: false),
                        letterhead = c.Boolean(nullable: false),
                        customer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblBankCharges",
                c => new
                    {
                        BankChargesId = c.Int(nullable: false, identity: true),
                        CashDeposit = c.Decimal(precision: 18, scale: 2),
                        DebitOrder = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.BankChargesId);
            
            CreateTable(
                "dbo.tblBuildings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Building = c.String(),
                        Code = c.String(maxLength: 50),
                        AccNumber = c.String(),
                        DataPath = c.String(),
                        Period = c.String(maxLength: 50),
                        Acc = c.String(maxLength: 50),
                        Contra = c.String(maxLength: 50),
                        ownbank = c.String(maxLength: 50),
                        cashbook3 = c.String(maxLength: 50),
                        payments = c.Int(),
                        receipts = c.Int(),
                        journals = c.Int(),
                        bc = c.String(maxLength: 50),
                        centrec = c.String(maxLength: 50),
                        business = c.String(maxLength: 50),
                        bank = c.String(maxLength: 50),
                        pm = c.String(),
                        bankName = c.String(),
                        accName = c.String(),
                        bankAccNumber = c.String(),
                        branch = c.String(),
                        isBuilding = c.Boolean(nullable: false),
                        addy1 = c.String(),
                        addy2 = c.String(),
                        addy3 = c.String(),
                        addy4 = c.String(),
                        addy5 = c.String(),
                        web = c.String(maxLength: 255),
                        letterName = c.String(maxLength: 255),
                        pid = c.String(maxLength: 50),
                        hoa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblBuildingSettings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        buildingID = c.Int(nullable: false),
                        reminderFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        reminderSplit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        finalFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        finalSplit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        disconnectionNoticefee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        disconnectionNoticeSplit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        summonsFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        summonsSplit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        disconnectionFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        disconnectionSplit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        handoverFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        handoverSplit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        reminderTemplate = c.String(),
                        finalTemplate = c.String(),
                        diconnectionNoticeTemplate = c.String(),
                        summonsTemplate = c.String(),
                        reminderSMS = c.String(),
                        finalSMS = c.String(),
                        disconnectionNoticeSMS = c.String(),
                        summonsSMS = c.String(),
                        disconnectionSMS = c.String(),
                        handoverSMS = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblCashDeposits",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        min = c.Decimal(precision: 18, scale: 2),
                        max = c.Decimal(precision: 18, scale: 2),
                        amount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblClearances",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        buildingCode = c.String(nullable: false, maxLength: 50),
                        customerCode = c.String(nullable: false, maxLength: 50),
                        preparedBy = c.String(),
                        trfAttorneys = c.String(),
                        attReference = c.String(),
                        fax = c.String(),
                        certDate = c.DateTime(),
                        complex = c.String(),
                        unitNo = c.String(),
                        seller = c.String(),
                        purchaser = c.String(),
                        purchaserAddress = c.String(),
                        purchaserTel = c.String(),
                        purchaserEmail = c.String(),
                        regDate = c.DateTime(),
                        notes = c.String(),
                        clearanceFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        astrodonTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        validDate = c.DateTime(),
                        processed = c.Boolean(nullable: false),
                        paid = c.Boolean(nullable: false),
                        bc = c.Boolean(nullable: false),
                        hoa = c.Boolean(nullable: false),
                        registered = c.Boolean(nullable: false),
                        journal = c.Boolean(),
                        extClearance = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblClearanceTransactions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        clearanceID = c.Int(nullable: false),
                        description = c.String(nullable: false),
                        qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        markup = c.Decimal(nullable: false, precision: 18, scale: 2),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblCustomerNotes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        customer = c.String(nullable: false, maxLength: 50),
                        notes = c.String(nullable: false, storeType: "ntext"),
                        noteDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblDebtors",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        buildingID = c.Int(nullable: false),
                        completeDate = c.DateTime(nullable: false),
                        category = c.String(maxLength: 50),
                        lettersupdated = c.Boolean(nullable: false),
                        lettersageanalysis = c.Boolean(nullable: false),
                        lettersprintemail = c.Boolean(nullable: false),
                        lettersfiled = c.Boolean(nullable: false),
                        stmtupdated = c.Boolean(nullable: false),
                        stmtinterest = c.Boolean(nullable: false),
                        stmtprintemail = c.Boolean(nullable: false),
                        stmtfiled = c.Boolean(nullable: false),
                        meupdate = c.Boolean(nullable: false),
                        meinvest = c.Boolean(nullable: false),
                        me9990 = c.Boolean(nullable: false),
                        me4000 = c.Boolean(nullable: false),
                        mepettycash = c.Boolean(nullable: false),
                        dailytrust = c.Boolean(nullable: false),
                        dailyown = c.Boolean(nullable: false),
                        dailyfile = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblDevision",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Date = c.String(maxLength: 50),
                        Description = c.String(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Balance = c.Decimal(precision: 18, scale: 2),
                        FromAccNumber = c.String(maxLength: 50),
                        AccDescription = c.String(),
                        StatementNr = c.String(maxLength: 50),
                        Allocate = c.String(maxLength: 10, fixedLength: true),
                        Reference = c.String(),
                        Building = c.String(),
                        AccNumber = c.String(maxLength: 50),
                        posted = c.Boolean(nullable: false),
                        period = c.Int(),
                        lid = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblEmails",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        customer = c.String(nullable: false, maxLength: 50),
                        shouldBeSent = c.Boolean(nullable: false),
                        sent = c.Boolean(nullable: false),
                        fromEmail = c.String(nullable: false),
                        toEmail = c.String(nullable: false),
                        subject = c.String(nullable: false),
                        hasAttachment = c.Boolean(nullable: false),
                        sentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblEmbedType",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        attType = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblExport",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        lid = c.Int(nullable: false),
                        trnDate = c.String(maxLength: 255),
                        amount = c.Decimal(precision: 18, scale: 2),
                        building = c.String(maxLength: 255),
                        code = c.String(maxLength: 255),
                        description = c.String(maxLength: 255),
                        reference = c.String(maxLength: 255),
                        accnumber = c.String(maxLength: 255),
                        contra = c.String(maxLength: 255),
                        datapath = c.String(maxLength: 255),
                        period = c.Int(),
                        trustpost = c.Boolean(nullable: false),
                        buspost = c.Boolean(),
                        buildpost = c.Boolean(),
                        una = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblJobAttachments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        jobID = c.Int(nullable: false),
                        fileType = c.Int(nullable: false),
                        fileName = c.String(nullable: false, maxLength: 50),
                        fileContent = c.Binary(),
                        fileString = c.String(),
                        creator = c.Int(nullable: false),
                        createDate = c.DateTime(nullable: false),
                        attType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblJobCustomer",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        jobID = c.Int(nullable: false),
                        customerID = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblJob",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        creator = c.Int(nullable: false),
                        processor = c.Int(nullable: false),
                        createDate = c.DateTime(nullable: false),
                        buildingID = c.Int(nullable: false),
                        subject = c.String(nullable: false, maxLength: 255),
                        topic = c.String(maxLength: 255),
                        notes = c.String(),
                        emailBody = c.String(),
                        upload = c.Boolean(nullable: false),
                        email = c.Boolean(nullable: false),
                        currentStatus = c.String(nullable: false, maxLength: 50),
                        customerAction = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblJobStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        jobID = c.Int(nullable: false),
                        status = c.String(nullable: false, maxLength: 50),
                        statusDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblJobUpdate",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pmLastUpdated = c.DateTime(nullable: false),
                        pmID = c.Int(nullable: false),
                        paLastUpdated = c.DateTime(nullable: false),
                        paID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblJournal",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        trnDate = c.String(maxLength: 255),
                        amount = c.Decimal(precision: 18, scale: 2),
                        building = c.String(maxLength: 255),
                        code = c.String(maxLength: 255),
                        description = c.String(maxLength: 255),
                        reference = c.String(maxLength: 255),
                        accnumber = c.String(maxLength: 255),
                        contra = c.String(maxLength: 255),
                        datapath = c.String(maxLength: 255),
                        period = c.Int(),
                        post = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblLedgerTransactions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Date = c.String(maxLength: 50),
                        Description = c.String(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Balance = c.Decimal(precision: 18, scale: 2),
                        AccNumber = c.String(),
                        AccDescription = c.String(),
                        StatementNr = c.String(),
                        Allocate = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblLetterRun",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fromEmail = c.String(nullable: false),
                        toEmail = c.String(nullable: false),
                        subject = c.String(nullable: false),
                        message = c.String(nullable: false),
                        html = c.Boolean(),
                        addcc = c.Boolean(),
                        readreceipt = c.Boolean(),
                        attachment = c.String(),
                        queueDate = c.DateTime(nullable: false),
                        sentDate = c.DateTime(),
                        unitno = c.String(nullable: false, maxLength: 50),
                        errorMessage = c.String(),
                        status = c.String(),
                        cc = c.String(),
                        bcc = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblMatch",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        statementRef = c.String(nullable: false),
                        astroRef = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblMonthFin",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        completeDate = c.DateTime(nullable: false),
                        buildingID = c.String(nullable: false, maxLength: 50),
                        userID = c.Int(nullable: false),
                        finPeriod = c.Int(),
                        finMonth = c.Int(),
                        year = c.Int(nullable: false),
                        findate = c.DateTime(nullable: false),
                        levies = c.Int(nullable: false),
                        leviesReason = c.String(),
                        sewage = c.Int(nullable: false),
                        sewageNotes = c.String(),
                        electricity = c.Int(nullable: false),
                        electricityNotes = c.String(),
                        water = c.Int(nullable: false),
                        waterNotes = c.String(),
                        specialLevies = c.Int(nullable: false),
                        specialLevyNotes = c.String(),
                        otherIncomeDescription = c.String(),
                        otherIncome = c.Int(),
                        otherIncomeNotes = c.String(),
                        memberInterest = c.Int(nullable: false),
                        memberInterestNotes = c.String(),
                        bankInterest = c.Int(nullable: false),
                        bankInterestNotes = c.String(),
                        accountingFees = c.Int(nullable: false),
                        accountingFeesNotes = c.String(),
                        bankCharges = c.Int(),
                        bankChargesNotes = c.String(),
                        sewageExpense = c.Int(),
                        sewageExpenseNotes = c.String(),
                        deliveries = c.Int(),
                        deliveriesNotes = c.String(),
                        electricityExpense = c.Int(),
                        electricityExpenseNotes = c.String(),
                        gardens = c.Int(),
                        gardensNotes = c.String(),
                        insurance = c.Int(),
                        insuranceNotes = c.String(),
                        interestPaid = c.Int(),
                        interestPaidNotes = c.String(),
                        managementFees = c.Int(),
                        managementFeesNotes = c.String(),
                        meterReading = c.Int(),
                        meterReadingNotes = c.String(),
                        printing = c.Int(),
                        printingNotes = c.String(),
                        post = c.Int(),
                        postNotes = c.String(),
                        repairs = c.Int(),
                        repairsNotes = c.String(),
                        refuse = c.Int(),
                        refuseNotes = c.String(),
                        salaries = c.Int(),
                        salariesNotes = c.String(),
                        security = c.Int(),
                        securityNotes = c.String(),
                        telephone = c.Int(),
                        telephoneNotes = c.String(),
                        waterExpense = c.Int(),
                        waterExpenseNotes = c.String(),
                        municipal = c.Int(),
                        municipalReason = c.String(),
                        trust = c.Int(),
                        trustNotes = c.String(),
                        own = c.Int(),
                        ownNotes = c.String(),
                        investment = c.Int(),
                        investmentNotes = c.String(),
                        sundy = c.Int(),
                        sundryNotes = c.String(),
                        assets = c.Int(),
                        assetsNotes = c.String(),
                        debtors = c.Int(),
                        debtorsNotes = c.String(),
                        municipalAccounts = c.Int(),
                        municipalAccountsNotes = c.String(),
                        owners = c.Int(),
                        ownersNotes = c.String(),
                        suppliers = c.Int(),
                        suppliersNotes = c.String(),
                        liabilities = c.Int(),
                        liabilitiesNotes = c.String(),
                        electricityRecon = c.Int(nullable: false),
                        waterRecon = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblMsgData",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        msgID = c.Int(nullable: false),
                        Name = c.String(),
                        ContentType = c.String(maxLength: 50),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblMsgRecipients",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        msgID = c.Int(nullable: false),
                        recipient = c.String(nullable: false, maxLength: 50),
                        accNo = c.String(nullable: false, maxLength: 50),
                        queueDate = c.DateTime(nullable: false),
                        sentDate = c.DateTime(),
                        billCustomer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblMsg",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        buildingID = c.Int(nullable: false),
                        fromAddress = c.String(nullable: false),
                        incBCC = c.Boolean(nullable: false),
                        bccAddy = c.String(maxLength: 50),
                        subject = c.String(),
                        message = c.String(),
                        billBuilding = c.Boolean(nullable: false),
                        billAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        queue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblPAStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        paID = c.Int(nullable: false),
                        paStatus = c.Boolean(nullable: false),
                        availableSince = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblPMCustomers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        jobID = c.Int(nullable: false),
                        account = c.String(nullable: false, maxLength: 50),
                        sendMail1 = c.Boolean(nullable: false),
                        sendMail2 = c.Boolean(nullable: false),
                        sendMail3 = c.Boolean(nullable: false),
                        sendMail4 = c.Boolean(nullable: false),
                        sendSMS = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblPMJob",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        creator = c.Int(nullable: false),
                        processedBy = c.Int(),
                        buildingCode = c.String(nullable: false, maxLength: 50),
                        buildingUpload = c.Boolean(nullable: false),
                        inboxUpload = c.Boolean(nullable: false),
                        buildingFolder = c.String(),
                        topic = c.String(),
                        instructions = c.String(),
                        notes = c.String(),
                        cc = c.String(),
                        bcc = c.String(),
                        subject = c.String(),
                        body = c.String(),
                        sms = c.String(),
                        billcustomer = c.Boolean(nullable: false),
                        status = c.String(nullable: false, maxLength: 50),
                        createDate = c.DateTime(nullable: false),
                        assignedDate = c.DateTime(),
                        completeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblPMJobStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        jobID = c.Int(nullable: false),
                        actioned = c.Int(nullable: false),
                        status = c.String(nullable: false, maxLength: 50),
                        actionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblPMQCustomers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pmQID = c.Int(nullable: false),
                        customerCode = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblPMQStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pmQID = c.Int(nullable: false),
                        statusDate = c.DateTime(nullable: false),
                        status = c.String(nullable: false),
                        notes = c.String(storeType: "ntext"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblPMQueue",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pm = c.Int(nullable: false),
                        createDate = c.DateTime(nullable: false),
                        building = c.Int(nullable: false),
                        customer = c.String(maxLength: 50),
                        subject = c.String(nullable: false),
                        message = c.String(nullable: false),
                        assigned = c.Int(nullable: false),
                        currentStatus = c.Int(nullable: false),
                        emailMe = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblPMStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        status = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblReminders",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userID = c.Int(nullable: false),
                        customer = c.String(nullable: false, maxLength: 50),
                        building = c.String(nullable: false, maxLength: 50),
                        remDate = c.DateTime(nullable: false),
                        remNote = c.String(nullable: false, storeType: "ntext"),
                        action = c.Boolean(nullable: false),
                        actionDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblRentalAccounts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        drAccount = c.String(maxLength: 50),
                        drContra = c.String(maxLength: 50),
                        crAccount = c.String(maxLength: 50),
                        crContra = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblRentalRecon",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        rentalId = c.Int(nullable: false),
                        trnDate = c.DateTime(nullable: false),
                        value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        account = c.String(nullable: false, maxLength: 50),
                        contra = c.String(nullable: false, maxLength: 50),
                        posted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblRentals",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        trnDate = c.DateTime(),
                        reference = c.String(maxLength: 255),
                        description = c.String(),
                        drValue = c.Decimal(precision: 18, scale: 2),
                        crValue = c.Decimal(precision: 18, scale: 2),
                        cumValue = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblRequisition",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userID = c.Int(nullable: false),
                        trnDate = c.DateTime(nullable: false),
                        building = c.Int(nullable: false),
                        account = c.String(nullable: false, maxLength: 50),
                        ledger = c.String(),
                        reference = c.String(nullable: false, maxLength: 50),
                        payreference = c.String(),
                        contractor = c.String(maxLength: 50),
                        notes = c.String(storeType: "ntext"),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        processed = c.Boolean(nullable: false),
                        paid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblRunConfig",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        stmtRunStatus = c.Boolean(nullable: false),
                        letterRunStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblSettings",
                c => new
                    {
                        templatedir = c.String(nullable: false, maxLength: 200),
                        minbal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rem_admin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        sum_admin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        recon_fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        discon_admin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fd_admin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        clearance = c.Decimal(precision: 18, scale: 2),
                        ex_clearance = c.Decimal(precision: 18, scale: 2),
                        reminder_fee = c.Decimal(precision: 18, scale: 2),
                        final_fee = c.Decimal(precision: 18, scale: 2),
                        summons_fee = c.Decimal(precision: 18, scale: 2),
                        discon_notice_fee = c.Decimal(precision: 18, scale: 2),
                        discon_fee = c.Decimal(precision: 18, scale: 2),
                        handover_fee = c.Decimal(precision: 18, scale: 2),
                        recon_split = c.Decimal(precision: 18, scale: 2),
                        debit_order = c.Decimal(precision: 18, scale: 2),
                        ret_debit_order = c.Decimal(precision: 18, scale: 2),
                        eft_fee = c.Decimal(precision: 18, scale: 2),
                        monthly_journal = c.Decimal(precision: 18, scale: 2),
                        trust = c.String(),
                        centrec = c.String(),
                        business = c.String(),
                        rental = c.String(),
                    })
                .PrimaryKey(t => new { t.templatedir, t.minbal, t.rem_admin, t.sum_admin, t.recon_fee, t.discon_admin, t.fd_admin });
            
            CreateTable(
                "dbo.tblSMS",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        building = c.String(nullable: false, maxLength: 50),
                        customer = c.String(nullable: false, maxLength: 50),
                        currentBalance = c.Decimal(precision: 18, scale: 2),
                        smsType = c.String(maxLength: 50),
                        number = c.String(maxLength: 50),
                        reference = c.String(maxLength: 255),
                        message = c.String(nullable: false),
                        billable = c.Boolean(nullable: false),
                        bulkbillable = c.Boolean(nullable: false),
                        sent = c.DateTime(nullable: false),
                        sender = c.String(nullable: false),
                        astStatus = c.String(),
                        batchID = c.String(),
                        status = c.String(nullable: false),
                        nextPolled = c.DateTime(nullable: false),
                        pollCount = c.Int(nullable: false),
                        bsStatus = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblStatementRun",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        email1 = c.String(nullable: false),
                        sentDate1 = c.DateTime(),
                        queueDate = c.DateTime(nullable: false),
                        fileName = c.String(nullable: false),
                        debtorEmail = c.String(),
                        unit = c.String(nullable: false),
                        attachment = c.String(),
                        errorMessage = c.String(),
                        subject = c.String(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblStatements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        building = c.String(nullable: false),
                        lastProcessed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblTemplates",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        buildingID = c.Int(nullable: false),
                        templateName = c.String(nullable: false),
                        templateContent = c.String(nullable: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblUnallocated",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        lid = c.Int(nullable: false),
                        trnDate = c.String(maxLength: 255),
                        amount = c.Decimal(precision: 18, scale: 2),
                        building = c.String(maxLength: 255),
                        code = c.String(maxLength: 255),
                        description = c.String(maxLength: 255),
                        reference = c.String(maxLength: 255),
                        accnumber = c.String(maxLength: 255),
                        contra = c.String(maxLength: 255),
                        datapath = c.String(maxLength: 255),
                        period = c.Int(),
                        trustpost = c.Boolean(),
                        buspost = c.Boolean(),
                        buildpost = c.Boolean(),
                        allocated = c.Boolean(),
                        allocatedDate = c.DateTime(),
                        allocatedBuilding = c.String(maxLength: 255),
                        allocatedCode = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblUserBuildings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userid = c.Int(nullable: false),
                        buildingid = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblUserLog",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userid = c.Int(nullable: false),
                        buildingid = c.Int(nullable: false),
                        customercode = c.String(nullable: false, maxLength: 50),
                        trnDate = c.DateTime(nullable: false),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        description = c.String(nullable: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.tblUsers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 50),
                        password = c.String(nullable: false, maxLength: 50),
                        admin = c.Boolean(nullable: false),
                        email = c.String(maxLength: 255),
                        name = c.String(maxLength: 50),
                        phone = c.String(maxLength: 50),
                        fax = c.String(maxLength: 50),
                        usertype = c.Int(nullable: false),
                        pmSignature = c.Binary(storeType: "image"),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblUsers");
            DropTable("dbo.tblUserLog");
            DropTable("dbo.tblUserBuildings");
            DropTable("dbo.tblUnallocated");
            DropTable("dbo.tblTemplates");
            DropTable("dbo.tblStatus");
            DropTable("dbo.tblStatements");
            DropTable("dbo.tblStatementRun");
            DropTable("dbo.tblSMS");
            DropTable("dbo.tblSettings");
            DropTable("dbo.tblRunConfig");
            DropTable("dbo.tblRequisition");
            DropTable("dbo.tblRentals");
            DropTable("dbo.tblRentalRecon");
            DropTable("dbo.tblRentalAccounts");
            DropTable("dbo.tblReminders");
            DropTable("dbo.tblPMStatus");
            DropTable("dbo.tblPMQueue");
            DropTable("dbo.tblPMQStatus");
            DropTable("dbo.tblPMQCustomers");
            DropTable("dbo.tblPMJobStatus");
            DropTable("dbo.tblPMJob");
            DropTable("dbo.tblPMCustomers");
            DropTable("dbo.tblPAStatus");
            DropTable("dbo.tblMsg");
            DropTable("dbo.tblMsgRecipients");
            DropTable("dbo.tblMsgData");
            DropTable("dbo.tblMonthFin");
            DropTable("dbo.tblMatch");
            DropTable("dbo.tblLetterRun");
            DropTable("dbo.tblLedgerTransactions");
            DropTable("dbo.tblJournal");
            DropTable("dbo.tblJobUpdate");
            DropTable("dbo.tblJobStatus");
            DropTable("dbo.tblJob");
            DropTable("dbo.tblJobCustomer");
            DropTable("dbo.tblJobAttachments");
            DropTable("dbo.tblExport");
            DropTable("dbo.tblEmbedType");
            DropTable("dbo.tblEmails");
            DropTable("dbo.tblDevision");
            DropTable("dbo.tblDebtors");
            DropTable("dbo.tblCustomerNotes");
            DropTable("dbo.tblClearanceTransactions");
            DropTable("dbo.tblClearances");
            DropTable("dbo.tblCashDeposits");
            DropTable("dbo.tblBuildingSettings");
            DropTable("dbo.tblBuildings");
            DropTable("dbo.tblBankCharges");
            DropTable("dbo.tblAttachments");
        }
    }
}

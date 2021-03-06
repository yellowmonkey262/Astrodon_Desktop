namespace Astrodon.Data
{
    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Data.Entity.Migrations.History;

    /*
     
    NB!!! DO NOT EDIT THIS FILE, IT WAS GENERATED BY A TOOL AND MAY HAVE TO BE GENERATED AGAIN!     
         
    */
    public partial class DataContext : DbContext
    {
        public virtual DbSet<tblAttachment> tblAttachments { get; set; }
        public virtual DbSet<tblBankCharge> tblBankCharges { get; set; }
        public virtual DbSet<tblBuilding> tblBuildings { get; set; }
        public virtual DbSet<tblBuildingSetting> tblBuildingSettings { get; set; }
        public virtual DbSet<tblCashDeposit> tblCashDeposits { get; set; }
        public virtual DbSet<tblClearance> tblClearances { get; set; }
        public virtual DbSet<tblClearanceTransaction> tblClearanceTransactions { get; set; }
        public virtual DbSet<tblCustomerNote> tblCustomerNotes { get; set; }
        public virtual DbSet<tblDebtor> tblDebtors { get; set; }
        public virtual DbSet<tblDevision> tblDevisions { get; set; }
        public virtual DbSet<tblEmail> tblEmails { get; set; }
        public virtual DbSet<tblEmbedType> tblEmbedTypes { get; set; }
        public virtual DbSet<tblExport> tblExports { get; set; }
        public virtual DbSet<tblJob> tblJobs { get; set; }
        public virtual DbSet<tblJobAttachment> tblJobAttachments { get; set; }
        public virtual DbSet<tblJobCustomer> tblJobCustomers { get; set; }
        public virtual DbSet<tblJobStatu> tblJobStatus { get; set; }
        public virtual DbSet<tblJobUpdate> tblJobUpdates { get; set; }
        public virtual DbSet<tblJournal> tblJournals { get; set; }
        public virtual DbSet<tblLedgerTransaction> tblLedgerTransactions { get; set; }
        public virtual DbSet<tblLetterRun> tblLetterRuns { get; set; }
        public virtual DbSet<tblMatch> tblMatches { get; set; }
        public virtual DbSet<tblMonthFin> tblMonthFins { get; set; }
        public virtual DbSet<tblMsg> tblMsgs { get; set; }
        public virtual DbSet<tblMsgData> tblMsgDatas { get; set; }
        public virtual DbSet<tblMsgRecipient> tblMsgRecipients { get; set; }
        public virtual DbSet<tblPAStatu> tblPAStatus { get; set; }
        public virtual DbSet<tblPMCustomer> tblPMCustomers { get; set; }
        public virtual DbSet<tblPMJob> tblPMJobs { get; set; }
        public virtual DbSet<tblPMJobStatu> tblPMJobStatus { get; set; }
        public virtual DbSet<tblPMQCustomer> tblPMQCustomers { get; set; }
        public virtual DbSet<tblPMQStatu> tblPMQStatus { get; set; }
        public virtual DbSet<tblPMQueue> tblPMQueues { get; set; }
        public virtual DbSet<tblPMStatu> tblPMStatus { get; set; }
        public virtual DbSet<tblReminder> tblReminders { get; set; }
        public virtual DbSet<tblRentalAccount> tblRentalAccounts { get; set; }
        public virtual DbSet<tblRentalRecon> tblRentalRecons { get; set; }
        public virtual DbSet<tblRental> tblRentals { get; set; }
        public virtual DbSet<tblRequisition> tblRequisitions { get; set; }
        public virtual DbSet<tblRunConfig> tblRunConfigs { get; set; }
        public virtual DbSet<tblSM> tblSMS { get; set; }
        public virtual DbSet<tblStatementRun> tblStatementRuns { get; set; }
        public virtual DbSet<tblStatement> tblStatements { get; set; }
        public virtual DbSet<tblStatu> tblStatus { get; set; }
        public virtual DbSet<tblTemplate> tblTemplates { get; set; }
        public virtual DbSet<tblUnallocated> tblUnallocateds { get; set; }
        public virtual DbSet<tblUserBuilding> tblUserBuildings { get; set; }
        public virtual DbSet<tblUserLog> tblUserLogs { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblSetting> tblSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             InitialModelCreate(modelBuilder);
        }

        private void InitialModelCreate(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<tblDevision>()
                .Property(e => e.Allocate)
                .IsFixedLength();
        }

       

    }

   
}

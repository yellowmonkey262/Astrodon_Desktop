using Astrodon.Controls.Bank;
using Astrodon.Controls.Insurance;
using Astrodon.Controls.Maintenance;
using Astrodon.Controls.Requisitions;
using Astrodon.Controls.Supplier;
using Astrodon.Controls.SystemConfig;
using Astrodon.Data;
using Astrodon.Reports;
using Astrodon.Reports.Calendar;
using Astrodon.Reports.DebitOrder;
using Astrodon.Reports.MaintenanceReport;
using Astrodon.Reports.ManagementPack;
using Astrodon.Reports.SupplierReport;
using NotificationWindow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using Astrodon.Controls.Web;
using Astrodon.Letter;
using Astrodon.Reports.BuildingPMDebtor;
using Astrodon.Reports.TransactionSearch;
using Astrodon.Reports.TrusteeReport;

namespace Astrodon
{
    public partial class frmMain : Form
    {
        private Timer tmrRem = new Timer();
        private DataContext _DataContext;
        private PopupNotifier popup = null;
        private Astrodon.Controls.usrPMJobs jobList;

        public delegate void PopupDelegate(String notification);

        public frmMain()
        {
            InitializeComponent();
            upgradeDatabaseToolStripMenuItem.Visible = false;
            _DataContext = SqlDataHandler.GetDataContext();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version;
            upgradeDatabaseToolStripMenuItem.Visible = Controller.user.username == "sheldon";
            if (Controller.user.usertype != 1 && Controller.user.usertype != 2)
            {
                systemToolStripMenuItem.Enabled = false;
                clearancesToolStripMenuItem1.Enabled = false;
                importStatementsToolStripMenuItem.Enabled = false;
                allocationsToolStripMenuItem.Enabled = false;
            }

            reportingToolStripMenuItem.Enabled = true;

            if (Controller.user.usertype != 1 && Controller.user.usertype != 2)
            {
                buildingPMReportMenuItem.Enabled = true;
                summaryToolStripMenuItem.Enabled = false;
                emailToolStripMenuItem.Enabled = false;
                pAPMToolStripMenuItem.Enabled = false;
                statementRunToolStripMenuItem.Enabled = false;
                trustToolStripMenuItem.Enabled = false;
                supplierReportToolStripMenuItem1.Enabled = false;
                maintenanceReportToolStripMenuItem.Enabled = false;
                insuranceScheduleToolStripMenuItem.Enabled = false;
            }

            List<int> allowedUsers = new List<int>() { 43 };
            if (allowedUsers.Contains(Controller.user.id)) { reportingToolStripMenuItem.Enabled = true; }
            if (Controller.user.usertype == 1 || Controller.user.usertype == 2) { pMJobListToolStripMenuItem.Enabled = true; }
            else if (Controller.user.usertype == 4)
            {
                pMJobListToolStripMenuItem.Enabled = false;
                Controller.AssignJob();
                jobListToolStripMenuItem_Click(this, new EventArgs());
            }
            else
            {
                pMPAToolStripMenuItem.Enabled = false;
            }
            List<int> allowReqUsers = new List<int>() { 15 };
            if (allowReqUsers.Contains(Controller.user.id))
            {
                pMPAToolStripMenuItem.Enabled = true;
                pMJobListToolStripMenuItem.Enabled = false;
                this.jobListToolStripMenuItem.Enabled = false;
                this.webMaintenanceToolStripMenuItem.Enabled = false;
                this.bulkEmailToolStripMenuItem.Enabled = false;
                this.managementReportToolStripMenuItem.Enabled = false;
                this.requisitionsToolStripMenuItem.Enabled = true;
                this.buildingMaintenanceToolStripMenuItem.Enabled = false;
                this.suppliersToolStripMenuItem1.Enabled = false;
            }
            Controller.DependencyInitialization();

            timer1.Enabled = false;
            timer1.Interval = 3000;
            timer1.Enabled = true;
            notifyIcon1.Visible = false;

            unPaidRequisitionsMenuItem.Enabled = Controller.UserIsSheldon(); //Sheldon and Tertia
            publishManagementPackToolStripMenuItem.Enabled = Controller.UserIsSheldon();

        }

        public void LoadReminders()
        {
            try
            {
                timer1.Interval = 60*60*5*1000;

                int count = _DataContext.tblReminders.Where(a => a.UserId == Controller.user.id && !a.action && a.remDate <= DateTime.Now).Count();

                if (count > 0)
                {
                    tmrRem.Interval = 250;
                    tmrRem.Tick += tmrRem_Tick;
                    tmrRem.Enabled = true;
                }
                else
                {
                    tmrRem.Enabled = false;
                    remindersToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
                }
            }
            catch
            {
                //Datacontext is out of sync and i have to dot he db upgrade first
            }
        }

        private void tmrRem_Tick(object sender, EventArgs e)
        {
            if (remindersToolStripMenuItem.ForeColor == System.Drawing.Color.Black)
            {
                remindersToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                remindersToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Controller.DependencyTermination();
        //    if (Controller.commClient != null) { Controller.commClient.Disconnect(); }
            Application.Exit();
            Environment.Exit(0);
        }

        public void SetNotifications(String notification)
        {
            lblNotifications.Text = notification;
        }

        public void PopupNotification(String notification)
        {
            if (InvokeRequired)
            {
                Invoke(new PopupDelegate(PopupNotification), notification);
            }
            else
            {
                try
                {
                    if (popup == null) { popup = new PopupNotifier(); }
                    popup.TitleText = "Message from Astrodon Server";
                    popup.ContentText = notification;
                    popup.ShowCloseButton = false;
                    popup.ShowOptionsButton = false;
                    popup.ShowGrip = true;
                    popup.Delay = 10000;
                    popup.AnimationInterval = 5;
                    popup.AnimationDuration = 15;
                    popup.TitlePadding = new Padding(3);
                    popup.ContentPadding = new Padding(3);
                    popup.ImagePadding = new Padding(3);
                    popup.Scroll = true;
                    popup.Popup();
                }
                catch { }
            }
        }

        private void AddNewControl(Control control, String controlName)
        {
            pnlContents.Controls.Clear();
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = controlName;
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewControl(new usrConfig { Dock = DockStyle.Fill }, "Config");
        }

        private void buildingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewControl(new usrBuildings { Dock = DockStyle.Fill }, "Buildings");
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrUsers userControl = new usrUsers { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(userControl);
            toolStripStatusLabel1.Text = "Users";
        }

        private void summaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrSummaryReport summControl = new usrSummaryReport { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(summControl);
            toolStripStatusLabel1.Text = "Centrec Report";
        }

        private void sMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrEmail emailControl = new Controls.usrEmail { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(emailControl);
            toolStripStatusLabel1.Text = "Email Reports";
        }

        private void pMPAToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void bulkSMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBulkSMS smsControl = new usrBulkSMS { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(smsControl);
            toolStripStatusLabel1.Text = "SMS";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void rentalImportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrImports importControl = new usrImports { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(importControl);
            toolStripStatusLabel1.Text = "Rental Imports";
        }

        private void lettersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrLetters letterControl = new usrLetters { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(letterControl);
            toolStripStatusLabel1.Text = "Letters";
        }

        private void clearancesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            frmClearance clearanceF = new frmClearance();
            clearanceF.Show();
        }

        private void bulkStatementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrStatements statementControl = new usrStatements { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(statementControl);
            toolStripStatusLabel1.Text = "Statements";
        }

        private void individualStatementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrIndStatements statementControl = new usrIndStatements { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(statementControl);
            toolStripStatusLabel1.Text = "Statements";
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrCredits creditControl = new usrCredits { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(creditControl);
            toolStripStatusLabel1.Text = "Credits";
        }

        private void sendBulkMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBulkEmail emailControl = new usrBulkEmail { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(emailControl);
            toolStripStatusLabel1.Text = "Bulk Email";
        }

        private void importStatementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrImportBank importControl = new usrImportBank { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(importControl);
            toolStripStatusLabel1.Text = "Import Statements";
        }

        private void allocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrAllocations allocationControl = new usrAllocations { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(allocationControl);
            toolStripStatusLabel1.Text = "Allocations";
        }

        private void journalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrJournal journalControl = new Controls.usrJournal { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(journalControl);
            toolStripStatusLabel1.Text = "Journals";
        }

        private void pMJobListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            if (jobList != null) { jobList = null; }
            Astrodon.Controls.usrJob jobControl = new Astrodon.Controls.usrJob(0) { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(jobControl);
            toolStripStatusLabel1.Text = "Jobs";
        }

        public void ShowJob(int jid)
        {
            pnlContents.Controls.Clear();
            if (jobList != null) { jobList = null; }
            Astrodon.Controls.usrJob jobControl = new Astrodon.Controls.usrJob(jid) { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(jobControl);
            toolStripStatusLabel1.Text = "Jobs";
        }

        private void pAPMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrJobReport jobReport = new Astrodon.Controls.usrJobReport { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(jobReport);
            toolStripStatusLabel1.Text = "Jobs";
        }

        private void jobListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Controller.ShowingJobList = true;
            jobList = new Controls.usrPMJobs { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(jobList);
            toolStripStatusLabel1.Text = "Job List";
        }

        public void ShowJobs()
        {
            jobListToolStripMenuItem_Click(this, new EventArgs());
        }

        private void webMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            var upload = new ucBuildingDocuments() { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(upload);
            toolStripStatusLabel1.Text = "Web Maintenance";
        }

        private void remindersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrReminders reminders = new Controls.usrReminders { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(reminders);
            toolStripStatusLabel1.Text = "Reminders";
        }

        private void printEnvelopesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrEnvelopes envelopes = new Controls.usrEnvelopes { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(envelopes);
            toolStripStatusLabel1.Text = "Envelopes";
        }

        private void bulkEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBulkEmail bulkMailer = new usrBulkEmail { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(bulkMailer);
            toolStripStatusLabel1.Text = "Bulk Email";
        }

        private void debtorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrDebtor debtorCtl = new Controls.usrDebtor { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(debtorCtl);
            toolStripStatusLabel1.Text = "Debtors Reports";
        }

        private void reportingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrDebtorReport dbReport = new Controls.usrDebtorReport { Dock = DockStyle.Fill };
            pnlContents.Controls.Add(dbReport);
            toolStripStatusLabel1.Text = "Debtors Report";
        }

       

        private void managementReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Management Report";
            pnlContents.Controls.Clear();
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
            Astrodon.Controls.usrBuilding buildCtl = new Controls.usrBuilding();
            buildCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(buildCtl);
            this.Cursor = Cursors.Arrow;
        }

        private void newPaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrJob jobCtl = new Controls.usrJob(2);
            jobCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(jobCtl);
            toolStripStatusLabel1.Text = "Job";
        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrAccounts accountCtl = new Controls.usrAccounts();
            accountCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(accountCtl);
            toolStripStatusLabel1.Text = "Monthly Financial Checklist";
        }

        private void paymentRequisitionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrRequisition accountCtl = new Controls.usrRequisition();
            accountCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(accountCtl);
            toolStripStatusLabel1.Text = "Payment Requisition";
        }

        private void unpaidRequisitionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrPaidRequisitions accountCtl = new Controls.usrPaidRequisitions();
            accountCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(accountCtl);
            toolStripStatusLabel1.Text = "Payment Requisitions";
        }

        private void customerFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrCustomer customerControl = new usrCustomer();
            customerControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(customerControl);
            toolStripStatusLabel1.Text = "Customers";
        }

        private void emailCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrEmailCustomer customerControl = new Astrodon.Controls.usrEmailCustomer();
            customerControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(customerControl);
            toolStripStatusLabel1.Text = "Customers";
        }

        private void searchCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrSearch customerControl = new Astrodon.Controls.usrSearch();
            customerControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(customerControl);
            toolStripStatusLabel1.Text = "Customer Search";
        }

        private void checklistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrMonthly monthlyCtl = new Controls.usrMonthly();
            monthlyCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(monthlyCtl);
            toolStripStatusLabel1.Text = "Monthly Financial Checklist";
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrMonthReport monthlyCtl = new Astrodon.Controls.usrMonthReport();
            monthlyCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(monthlyCtl);
            toolStripStatusLabel1.Text = "Monthly Financial Report";
        }

        private void pnlContents_ControlRemoved(object sender, ControlEventArgs e)
        {
            Controller.ShowingJobList = false;
        }

        private void statementRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrStatementRun stmtCtl = new Astrodon.Controls.usrStatementRun();
            stmtCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(stmtCtl);
            toolStripStatusLabel1.Text = "Statement Run";
        }

        private void trustToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrTrust trustCtl = new Astrodon.Controls.usrTrust();
            trustCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(trustCtl);
            toolStripStatusLabel1.Text = "Trust Transactions";
        }

        private void levyRollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            LevyRollUserControl trustCtl = new LevyRollUserControl();
            trustCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(trustCtl);
            toolStripStatusLabel1.Text = "Levy Roll Report";
        }

        private void tbMaintenanceConfig_Click(object sender, EventArgs e)
        {
           
        }

        private void supplierReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrSupplierReport suppReport = new usrSupplierReport();
            suppReport.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(suppReport);
            toolStripStatusLabel1.Text = "Supplier Report";
        }

        private void maintenanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrMaintenanceReport report = new usrMaintenanceReport();
            report.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(report);
            toolStripStatusLabel1.Text = "Maintenance Report";
        }

        private void suppliersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrSupplierLookup supplierLookup = new usrSupplierLookup(_DataContext, null);
            supplierLookup.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(supplierLookup);
            toolStripStatusLabel1.Text = "Supplier Maintenance";
        }

        private void buildingMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            var maintenance = new usrMaintenance(_DataContext);
            maintenance.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(maintenance);
            toolStripStatusLabel1.Text = "Building Maintenance";
        }

        private void bankConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void missingMaintenanceRequisitionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrMissingRequisitions control = new usrMissingRequisitions(_DataContext);
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Missing Maintenance Requisitions";
        }

        private void downloadRequisitionBatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrRequisitionBatch control = new usrRequisitionBatch();
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Requisition Batch";
        }

        private void supplierBatchRequisitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrSupplierBatchRequisition control = new usrSupplierBatchRequisition();
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Supplier Bulk Requisition";
        }

        private void upgradeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlDataHandler.MigrateEFDataBase();
            Controller.ShowMessage("Migration completed");
        }

        private void unPaidRequisitionsMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrUnpaidRequisitions control = new usrUnpaidRequisitions();
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Unpaid Requisitions";
        }

        private void nedbankBeneficiaryMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBuildingBenificiaries control = new usrBuildingBenificiaries();
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Building Benificiaries";
        }

        private void managementPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ManangementPackUserControl trustCtl = new ManangementPackUserControl();
            trustCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(trustCtl);
            toolStripStatusLabel1.Text = "Manangement Pack Report";
        }

        private void insuranceScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            InsuranceScheduleUserControl control = new InsuranceScheduleUserControl();
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Insurance Schedule";
        }

        private void managementPackTOCDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucTOCItem control = new ucTOCItem(_DataContext);
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Management Pack Descriptions";
        }

        private void insuranceBrokerMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrInsuranceBrokerLookup lookup = new usrInsuranceBrokerLookup(_DataContext);
            lookup.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(lookup);
            toolStripStatusLabel1.Text = "Insurance Broker Maintenance";
        }

        private void sAPORDebitOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            DebitOrderUserControl dt = new DebitOrderUserControl();
            dt.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(dt);
            toolStripStatusLabel1.Text = "SAPOR Debit Order";
        }

        private void calendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucPrintCalendar dt = new ucPrintCalendar();
            dt.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(dt);
            toolStripStatusLabel1.Text = "Calendar";
        }

        private void allocationSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Controller.AskQuestion("Are you sure you want to request more allocations?"))
            {
                using (var reportService = ReportService.ReportServiceClient.CreateInstance())
                {
                    reportService.RequestAllocations(SqlDataHandler.GetConnectionString(), Controller.user.id);
                    Controller.ShowMessage("The server will email your next set of allocations.");
                }
            }
        }

        private void publicHolidaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucPublicHoliday dt = new ucPublicHoliday(_DataContext);
            dt.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(dt);
            toolStripStatusLabel1.Text = "Public Holidays";
        }

        private void publishManagementPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucPublishManagementPack dt = new ucPublishManagementPack();
            dt.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(dt);
            toolStripStatusLabel1.Text = "Publish Management Pack";
        }

        private void meetingRoomConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usMeetingVenue dt = new usMeetingVenue(_DataContext);
            dt.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(dt);
            toolStripStatusLabel1.Text = "Meeting Rooms";
        }

        private void bondOriginatorConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucBondOriginator dt = new ucBondOriginator(_DataContext);
            dt.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(dt);
            toolStripStatusLabel1.Text = "Bond Originators";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadReminders();
        }

        private void notificationTemplatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucNotificationTemplate dt = new ucNotificationTemplate(_DataContext);
            dt.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(dt);
            toolStripStatusLabel1.Text = "Notification Templates";
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBuildingMaintenanceConfiguration buildingMaintenance = new usrBuildingMaintenanceConfiguration(_DataContext);
            buildingMaintenance.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(buildingMaintenance);
            toolStripStatusLabel1.Text = "Building Maintenance Configuration";
        }

        private void bankConfigurationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBankConfiguration control = new usrBankConfiguration(_DataContext);
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Bank Configuration";
        }

        private void customerDocumentTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucCustomerDocumentType control = new ucCustomerDocumentType(_DataContext);
            control.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(control);
            toolStripStatusLabel1.Text = "Customer Document Type Configuration";
        }

        private void buildingPMDebtorListMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucBuildingPMDebtorList buildingPMDebtorList = new ucBuildingPMDebtorList();
            buildingPMDebtorList.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(buildingPMDebtorList);
            toolStripStatusLabel1.Text = "Building PM Debtor List";
        }

        private void transactionSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucTransactionSearch transactionSearch = new ucTransactionSearch();
            transactionSearch.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(transactionSearch);
            toolStripStatusLabel1.Text = "Transaction Search";
        }

        private void trusteeReportListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            ucTrusteeReport trusteeReport = new ucTrusteeReport();
            trusteeReport.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(trusteeReport);
            toolStripStatusLabel1.Text = "Trustee Report";
        }

        private void preferredSuppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrPreferredSuppliers preferredSuppliers = new usrPreferredSuppliers();
            preferredSuppliers.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(preferredSuppliers);
            toolStripStatusLabel1.Text = "Preferred Suppliers";
        }
    }

}
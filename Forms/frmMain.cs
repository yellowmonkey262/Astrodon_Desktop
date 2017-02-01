using Astrodon.Controls.Maintenance;
using Astrodon.Controls.Supplier;
using Astrodon.Data;
using Astrodon.Reports;
using Astrodon.Reports.MaintenanceReport;
using Astrodon.Reports.SupplierReport;
using NotificationWindow;
using System;
using System.Data;
using System.Windows.Forms;

namespace Astrodon
{

    public partial class frmMain : Form
    {

        private DataContext _DataContext;

        public frmMain()
        {
            InitializeComponent();
            _DataContext = SqlDataHandler.GetDataContext();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (Controller.user.usertype != 1 && Controller.user.usertype != 2)
            {
                systemToolStripMenuItem.Enabled = false;
                clearancesToolStripMenuItem1.Enabled = false;
                importStatementsToolStripMenuItem.Enabled = false;
                allocationsToolStripMenuItem.Enabled = false;
            }
            if (Controller.user.usertype != 1 && Controller.user.usertype != 2)
            {
                reportingToolStripMenuItem.Enabled = false;
            }
            if (Controller.user.usertype == 1 || Controller.user.usertype == 2)
            {
                pMJobListToolStripMenuItem.Enabled = true;
            }
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
            Controller.DependencyInitialization();
            LoadReminders();
            notifyIcon1.Visible = false;
        }

        private Timer tmrRem = new Timer();

        public void LoadReminders()
        {
            String remQuery = "SELECT COUNT(*) as rems FROM tblReminders WHERE userid = " + Controller.user.id.ToString() + " AND action = 'False' AND remDate <= getdate()";
            SqlDataHandler dh = new SqlDataHandler();
            String status;
            DataSet dsRems = dh.GetData(remQuery, null, out status);
            if (dsRems != null && dsRems.Tables.Count > 0 && dsRems.Tables[0].Rows.Count > 0)
            {
                int count = int.Parse(dsRems.Tables[0].Rows[0]["rems"].ToString());
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
            if (Controller.commClient != null) { Controller.commClient.Disconnect(); }
            _DataContext.Dispose();
            Application.Exit();
            Environment.Exit(0);
        }

        public void SetNotifications(String notification)
        {
            lblNotifications.Text = notification;
        }

        public delegate void PopupDelegate(String notification);

        public void PopupNotification(String notification)
        {
            if (InvokeRequired)
            {
                Invoke(new PopupDelegate(PopupNotification), notification);
            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
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
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrConfig config = new usrConfig();
            config.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(config);
            toolStripStatusLabel1.Text = "Config";
        }

        private void buildingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBuildings buildingControl = new usrBuildings();
            buildingControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(buildingControl);
            toolStripStatusLabel1.Text = "Buildings";
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrUsers userControl = new usrUsers();
            userControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(userControl);
            toolStripStatusLabel1.Text = "Users";
        }

        private void summaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrSummaryReport summControl = new usrSummaryReport();
            summControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(summControl);
            toolStripStatusLabel1.Text = "Summary Report";
        }

        private void sMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrEmail emailControl = new Controls.usrEmail();
            emailControl.Dock = DockStyle.Fill;
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
            usrBulkSMS smsControl = new usrBulkSMS();
            smsControl.Dock = DockStyle.Fill;
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
            usrImports importControl = new usrImports();
            importControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(importControl);
            toolStripStatusLabel1.Text = "Rental Imports";
        }

        private void lettersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrLetters letterControl = new usrLetters();
            letterControl.Dock = DockStyle.Fill;
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
            usrStatements statementControl = new usrStatements();
            statementControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(statementControl);
            toolStripStatusLabel1.Text = "Statements";
        }

        private void individualStatementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrIndStatements statementControl = new usrIndStatements();
            statementControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(statementControl);
            toolStripStatusLabel1.Text = "Statements";
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrCredits creditControl = new usrCredits();
            creditControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(creditControl);
            toolStripStatusLabel1.Text = "Credits";
        }

        private void sendBulkMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBulkEmail emailControl = new usrBulkEmail();
            emailControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(emailControl);
            toolStripStatusLabel1.Text = "Bulk Email";
        }

        private void importStatementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrImportBank importControl = new usrImportBank();
            importControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(importControl);
            toolStripStatusLabel1.Text = "Import Statements";
        }

        private void allocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrAllocations allocationControl = new usrAllocations();
            allocationControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(allocationControl);
            toolStripStatusLabel1.Text = "Allocations";
        }

        private void journalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrJournal journalControl = new Controls.usrJournal();
            journalControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(journalControl);
            toolStripStatusLabel1.Text = "Journals";
        }

        private void pMJobListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            if (jobList != null) { jobList = null; }
            Astrodon.Controls.usrJob jobControl = new Astrodon.Controls.usrJob(0);
            jobControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(jobControl);
            toolStripStatusLabel1.Text = "Jobs";
        }

        public void ShowJob(int jid)
        {
            pnlContents.Controls.Clear();
            if (jobList != null) { jobList = null; }
            Astrodon.Controls.usrJob jobControl = new Astrodon.Controls.usrJob(jid);
            jobControl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(jobControl);
            toolStripStatusLabel1.Text = "Jobs";
        }

        private void pAPMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrJobReport jobReport = new Astrodon.Controls.usrJobReport();
            jobReport.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(jobReport);
            toolStripStatusLabel1.Text = "Jobs";
        }

        private Astrodon.Controls.usrPMJobs jobList;

        private void jobListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Controller.ShowingJobList = true;
            jobList = new Controls.usrPMJobs();
            jobList.Dock = DockStyle.Fill;
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
            Astrodon.Controls.usrWebDocs upload = new Controls.usrWebDocs();
            upload.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(upload);
            toolStripStatusLabel1.Text = "Web Maintenance";
        }

        private void remindersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrReminders reminders = new Controls.usrReminders();
            reminders.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(reminders);
            toolStripStatusLabel1.Text = "Reminders";
        }

        private void printEnvelopesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrEnvelopes envelopes = new Controls.usrEnvelopes();
            envelopes.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(envelopes);
            toolStripStatusLabel1.Text = "Envelopes";
        }

        private void bulkEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            usrBulkEmail bulkMailer = new usrBulkEmail();
            bulkMailer.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(bulkMailer);
            toolStripStatusLabel1.Text = "Bulk Email";
        }

        private void debtorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrDebtor debtorCtl = new Controls.usrDebtor();
            debtorCtl.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(debtorCtl);
            toolStripStatusLabel1.Text = "Debtors Reports";
        }

        private void reportingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlContents.Controls.Clear();
            Astrodon.Controls.usrDebtorReport dbReport = new Controls.usrDebtorReport();
            dbReport.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(dbReport);
            toolStripStatusLabel1.Text = "Debtors Report";
        }

        private void webReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            pnlContents.Controls.Clear();
            usrBuildingMaintenanceConfiguration buildingMaintenance = new usrBuildingMaintenanceConfiguration(_DataContext);
            buildingMaintenance.Dock = DockStyle.Fill;
            pnlContents.Controls.Add(buildingMaintenance);
            toolStripStatusLabel1.Text = "Building Maintenance Configuration";
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
            usrSupplierLookup supplierLookup = new usrSupplierLookup(_DataContext);
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
    }
}
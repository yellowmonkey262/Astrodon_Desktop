using Astrodon.Reports;
using NotificationWindow;
using System;
using System.Data;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class frmMain : Form
    {
        private Timer tmrRem = new Timer();

        public delegate void PopupDelegate(String notification);

        public frmMain()
        {
            InitializeComponent();
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
            if (Controller.user.usertype != 1 && Controller.user.usertype != 2) { reportingToolStripMenuItem.Enabled = false; }
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
                using (PopupNotifier popup = new PopupNotifier())
                {
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
            toolStripStatusLabel1.Text = "Summary Report";
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
            using (frmClearance clearanceF = new frmClearance()) { clearanceF.Show(); }
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

        private Astrodon.Controls.usrPMJobs jobList;

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
            Astrodon.Controls.usrWebDocs upload = new Controls.usrWebDocs { Dock = DockStyle.Fill };
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
    }
}
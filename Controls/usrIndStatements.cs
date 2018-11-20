using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using Astrodon.ClientPortal;

namespace Astrodon
{
    public partial class usrIndStatements : UserControl
    {
        private Building building;
        private Customer customer;
        private List<Building> buildings;
        private List<Customer> customers;
        private StatementBuilding stmtBuilding;
        private Statement stmt;
        private AstrodonClientPortal _ClientPortal = new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString());


        public usrIndStatements()
        {
            InitializeComponent();
            List<Building> allBuildings = new Buildings(false).buildings;
            buildings = new List<Building>();
            foreach (int bid in Controller.user.buildings)
            {
                foreach (Building b in allBuildings)
                {
                    if (bid == b.ID && !buildings.Contains(b))
                    {
                        buildings.Add(b);
                        break;
                    }
                }
            }
            buildings = buildings.OrderBy(c => c.Name).ToList();
        }

        private void usrIndStatements_Load(object sender, EventArgs e)
        {
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.DataSource = buildings;
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.SelectedIndex = -1;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbCustomer.SelectedIndexChanged -= cmbCustomer_SelectedIndexChanged;
                building = buildings[cmbBuilding.SelectedIndex];
                customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath,true);
                cmbCustomer.DataSource = null;
                cmbCustomer.Items.Clear();
                cmbCustomer.DataSource = customers;
                cmbCustomer.DisplayMember = "accNumber";
                cmbCustomer.ValueMember = "accNumber";
                cmbCustomer.SelectedIndex = -1;
                cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;

                stmtBuilding = new StatementBuilding(building.ID, building.Name, building.DataPath, building.Period, DateTime.Now, Controller.UserIsSheldon());
                stmt = new Statement
                {
                    BuildingId = building.ID,
                    pm = building.PM,
                    bankName = building.Bank_Name,
                    accName = building.Acc_Name,
                    BankAccountNumber = building.Bank_Acc_Number,
                    branch = building.Branch_Code,
                    DebtorEmail = building.Debtor
                };
            }
            catch { }

            if (String.IsNullOrWhiteSpace(stmt.DebtorEmail))
                Controller.HandleError("Debtor not configured on this building. Please check building configuration.");
                    
        }

        private void btnGenView_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            DateTime statementDate;
            if (CreateStatement(true, out fileName, out statementDate))
            {
                Process.Start(fileName);
            }
            else
            {
               Controller.HandleError("Unable to create statement");
            }
        }

        private void btnGenSend_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            DateTime statementDate;
            if (CreateStatement(true, out fileName, out statementDate))
            {
                List<String> attachments = new List<string>();
                attachments.Add(fileName);
                String attachment = txtAttachment.Text;
                if (!attachment.StartsWith("K:") && !String.IsNullOrEmpty(attachment))
                {
                    File.Copy(attachment, Path.Combine(Pastel.PastelRoot + "Debtors System\\statement test", Path.GetFileName(attachment)), true);
                    attachment = Path.GetFileName(attachment);
                }
                if (!String.IsNullOrEmpty(attachment)) { attachments.Add(attachment); }
                String[] att = attachments.ToArray();
                String status = String.Empty;
                string emailAddress = "";
                if (stmt != null && stmt.email1 != null && stmt.email1.Length > 0)
                    emailAddress = stmt.email1[0];

                var statementURL = _ClientPortal.InsertStatement(stmt.BuildingId, stmt.AccNo, stmt.StmtDate, fileName, File.ReadAllBytes(fileName), emailAddress);

                bool isRental = stmt.BuildingName == "ASTRODON RENTALS";
                if(Email.EmailProvider.SendStatement(stmt.DebtorEmail, stmt.email1, stmt.AccNo, fileName, stmt.StmtDate, statementURL, isRental))
                {
                    MessageBox.Show("Message Sent");
                }
                else
                {
                    Controller.HandleError("Unable to send mail: " + status);
                }

            }
            else
            {
                Controller.HandleError("Unable to create statement");
            }
        }

     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            dgTransactions.DataSource = null;
            customer = null;
            building = null;
            cmbBuilding.SelectedIndex = -1;
            cmbCustomer.SelectedIndex = -1;
        }

        private bool CreateStatement(bool makeFile, out String fileName,out DateTime statementDate, bool excludeStationery = false)
        {
            statementDate = stmtDatePicker.Value;
            this.Cursor = Cursors.WaitCursor;
            bool success = false;
            try
            {
                double totalDue = 0;
                String trnMsg;
                stmt.Transactions = (new Classes.LoadTrans()).LoadTransactions(building, customer, statementDate, out totalDue, out trnMsg);
                //MessageBox.Show(trnMsg);
                bool isStd = building.Bank_Name.ToLower().Contains("standard");
                stmt.totalDue = totalDue;
                stmt.AccNo = customer.accNumber;
                List<String> addList = new List<string>();
                addList.Add(customer.description);
                foreach (String addy in customer.address) { addList.Add(addy); }
                stmt.Address = addList.ToArray();
                stmt.DebtorEmail = building.Debtor;
                if (String.IsNullOrWhiteSpace(stmt.DebtorEmail))
                    Controller.HandleError("Debtor not configured on this building. Please check building configuration.");

                stmt.PrintMe = (customer.statPrintorEmail == 2 || customer.statPrintorEmail == 4 ? false : true);
                try
                {
                    if (customer.Email != null && customer.Email.Length > 0)
                    {
                        List<String> newEmails = new List<string>();
                        foreach (String emailAddress in customer.Email)
                        {
                             newEmails.Add(emailAddress); 
                        }
                        stmt.email1 = newEmails.ToArray();
                    }
                    else if (makeFile && MessageBox.Show("This customer has no email address. Continue?", "Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        stmt.email1 = new String[] { "" };
                    }
                }
                catch(Exception ex)
                {
                    Controller.HandleError(ex);

                    if (makeFile && MessageBox.Show("This customer has no email address. Continue?", "Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        stmt.email1 = new String[] { "" };
                    }
                    else if (!makeFile)
                    {
                        stmt.email1 = new String[] { "" };
                    }
                }
                stmt.BankDetails = Controller.pastel.GetBankDetails(stmtBuilding.DataPath);
                stmt.BuildingName = building.Name;
                stmt.BuildingId = building.ID;
                stmt.LevyMessage1 = (stmtBuilding.HOA ? HOAMessage1 : BCMessage1);
                stmt.LevyMessage2 = Message2;
                stmt.Message = txtMessage.Text;
                stmt.StmtDate = stmtDatePicker.Value;
                fileName = String.Empty;
                if (makeFile)
                {
                    PDF generator = new PDF(true);
                    generator.CreateStatement(stmt, stmt.BuildingName != "ASTRODON RENTALS" ? true : false, out fileName, isStd);
                    if (!String.IsNullOrEmpty(fileName)) {
                        success = true;
                    }else
                    {
                        Controller.HandleError("Unable to create statement");
                    }
                }
                else
                {
                    success = true;
                    dgTransactions.DataSource = stmt.Transactions;
                    dgTransactions.Columns[3].DefaultCellStyle.Format = "N2";
                    dgTransactions.Columns[4].DefaultCellStyle.Format = "N2";
                    lblOS.Text = totalDue.ToString("#,##0.00");
                    if (totalDue > 0) { lblOS.ForeColor = System.Drawing.Color.Red; } else { lblOS.ForeColor = System.Drawing.Color.Black; }
                }
            }
            catch (Exception ex)
            {
                Controller.HandleError(ex.Message);
                fileName = String.Empty;
            }
            this.Cursor = Cursors.Arrow;
            return success;
        }

        private String HOAMessage1
        {
            get
            {
                return "Levies are due and payable on the 1st of every month in advance.  Failure to compy will result in penalties being charged and electricity supply to the unit being suspended and or restricted.";
            }
        }

        private String BCMessage1
        {
            get
            {
                return "Levies are due and payable on the 1st of every month in advance.  Failure to compy will result in penalties being charged and electricity supply to the unit being suspended and or restricted.";
            }
        }

        private String Message2
        {
            get
            {
                return "***PLEASE ENSURE THAT ALL PAYMENTS REFLECTS IN OUR NOMINATED ACCOUNT ON OR BEFORE DUE DATE TO AVOID ANY PENALTIES, REFER TO TERMS AND CONDITIONS.***";
            }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                customer = customers[cmbCustomer.SelectedIndex];
                String fileName = String.Empty;
                DateTime statementDate;
                CreateStatement(false, out fileName, out statementDate);
            }
            catch { }
        }
    }
}
using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;

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
                customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath);
                cmbCustomer.DataSource = null;
                cmbCustomer.Items.Clear();
                cmbCustomer.DataSource = customers;
                cmbCustomer.DisplayMember = "accNumber";
                cmbCustomer.ValueMember = "accNumber";
                cmbCustomer.SelectedIndex = -1;
                cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;

                stmtBuilding = new StatementBuilding(building.ID,building.Name, building.DataPath, building.Period, DateTime.Now);
                stmt = new Statement
                {
                    BuildingId = building.ID,
                    pm = building.PM,
                    bankName = building.Bank_Name,
                    accName = building.Acc_Name,
                    accNumber = building.Bank_Acc_Number,
                    branch = building.Branch_Code,
                    DebtorEmail = building.Debtor
                };
            }
            catch { }
        }

        private void btnGenView_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            if (CreateStatement(true, out fileName)) { Process.Start(fileName); } else { MessageBox.Show("Unable to create statement"); }
        }

        private void btnGenSend_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            if (CreateStatement(true, out fileName))
            {
                List<String> attachments = new List<string>();
                attachments.Add(fileName);
                String attachment = txtAttachment.Text;
                if (!attachment.StartsWith("K:") && !String.IsNullOrEmpty(attachment))
                {
                    File.Copy(attachment, Path.Combine("K:\\Debtors System\\statement test", Path.GetFileName(attachment)), true);
                    attachment = Path.GetFileName(attachment);
                }
                if (!String.IsNullOrEmpty(attachment)) { attachments.Add(attachment); }
                String[] att = attachments.ToArray();
                String status = String.Empty;
                if (Mailer.SendMail("noreply@astrodon.co.za", stmt.email1, "Customer Statements", CustomerMessage(stmt.accNumber, stmt.DebtorEmail), false, false, false, out status, att))
                {
                    MessageBox.Show("Message Sent");
                }
                else
                {
                    MessageBox.Show("Unable to send mail: " + status);
                }
            }
            else
            {
                MessageBox.Show("Unable to create statement");
            }
        }

        private String CustomerMessage(String accNumber, String debtorEmail)
        {
            String message = "Dear Owner," + Environment.NewLine + Environment.NewLine;
            message += "Please find attached your statement." + Environment.NewLine + Environment.NewLine;
            message += "Remember, you can access your statements online. Paste the link below into your browser to access your online statements." + Environment.NewLine + Environment.NewLine;
            message += "www.astrodon.co.za" + Environment.NewLine + Environment.NewLine;
            message += "Regards" + Environment.NewLine + Environment.NewLine;
            message += "Astrodon (Pty) Ltd" + Environment.NewLine;
            message += "You're in Good Hands" + Environment.NewLine + Environment.NewLine;
            message += "Account #: " + accNumber + " For any queries on your statement, please email:" + debtorEmail + Environment.NewLine + Environment.NewLine;
            message += "Do not reply to this e-mail address";

            return message;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dgTransactions.DataSource = null;
            customer = null;
            building = null;
            cmbBuilding.SelectedIndex = -1;
            cmbCustomer.SelectedIndex = -1;
        }

        private bool CreateStatement(bool makeFile, out String fileName, bool excludeStationery = false)
        {
            this.Cursor = Cursors.WaitCursor;
            bool success = false;
            try
            {
                double totalDue = 0;
                String trnMsg;
                stmt.Transactions = (new Classes.LoadTrans()).LoadTransactions(building, customer, stmtDatePicker.Value, out totalDue, out trnMsg);
                //MessageBox.Show(trnMsg);
                bool isStd = building.Bank_Name.ToLower().Contains("standard");
                stmt.totalDue = totalDue;
                stmt.AccNo = customer.accNumber;
                List<String> addList = new List<string>();
                addList.Add(customer.description);
                foreach (String addy in customer.address) { addList.Add(addy); }
                stmt.Address = addList.ToArray();
                stmt.DebtorEmail = building.Debtor;
                stmt.PrintMe = (customer.statPrintorEmail == 2 || customer.statPrintorEmail == 4 ? false : true);
                try
                {
                    if (customer.Email != null && customer.Email.Length > 0)
                    {
                        List<String> newEmails = new List<string>();
                        foreach (String emailAddress in customer.Email)
                        {
                            if (!emailAddress.Contains("@imp.ad-one.co.za")) { newEmails.Add(emailAddress); }
                        }
                        stmt.email1 = newEmails.ToArray();
                    }
                    else if (makeFile && MessageBox.Show("This customer has no email address. Continue?", "Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        stmt.email1 = new String[] { "" };
                    }
                }
                catch
                {
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
                stmt.LevyMessage1 = (stmtBuilding.HOA ? HOAMessage1 : BCMessage1);
                stmt.LevyMessage2 = Message2;
                stmt.Message = txtMessage.Text;
                stmt.StmtDate = stmtDatePicker.Value;
                fileName = String.Empty;
                if (makeFile)
                {
                    PDF generator = new PDF(true);
                    generator.CreateStatement(stmt, stmt.BuildingName != "ASTRODON RENTALS" ? true : false, out fileName, isStd);
                    //generator.CreateStatement(stmt, out fileName);
                    if (!String.IsNullOrEmpty(fileName)) { success = true; }
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
                MessageBox.Show("Create statement :" + ex.Message);
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
                CreateStatement(false, out fileName);
            }
            catch { }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            buildings.Sort(new BuildingComparer("Name", SortOrder.Ascending));
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

                stmtBuilding = new StatementBuilding(building.Name, building.DataPath, building.Period, DateTime.Now);
                stmt = new Statement();
                stmt.pm = building.PM;
                stmt.bankName = building.Bank_Name;
                stmt.accName = building.Acc_Name;
                stmt.accNumber = building.Bank_Acc_Number;
                stmt.branch = building.Branch_Code;
                stmt.DebtorEmail = building.Debtor;
            }
            catch { }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
        }

        private void btnGenView_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            if (CreateStatement(true, out fileName))
            {
                Process.Start(fileName);
            }
            else
            {
                MessageBox.Show("Unable to create statement");
            }
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
            int lineNo = 123;
            this.Cursor = Cursors.WaitCursor;
            bool success = false;
            try
            {
                double totalDue = 0;
                lineNo = 128;
                stmt.Transactions = (new Classes.LoadTrans()).LoadTransactions(building, customer, stmtDatePicker.Value, out totalDue);
                bool isStd = building.Bank_Name.ToLower().Contains("standard");
                stmt.totalDue = totalDue;
                stmt.AccNo = customer.accNumber;
                List<String> addList = new List<string>();
                lineNo = 133;
                addList.Add(customer.description);
                foreach (String addy in customer.address) { addList.Add(addy); }
                stmt.Address = addList.ToArray();
                lineNo = 137;
                stmt.DebtorEmail = building.Debtor;
                stmt.PrintMe = (customer.statPrintorEmail == 2 || customer.statPrintorEmail == 4 ? false : true);
                lineNo = 140;
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
                lineNo = 148;
                stmt.BankDetails = Controller.pastel.GetBankDetails(stmtBuilding.DataPath);
                stmt.BuildingName = building.Name;
                stmt.LevyMessage1 = (stmtBuilding.HOA ? HOAMessage1 : BCMessage1);
                stmt.LevyMessage2 = Message2;
                stmt.Message = txtMessage.Text;
                stmt.StmtDate = stmtDatePicker.Value;
                lineNo = 155;
                fileName = String.Empty;
                if (makeFile)
                {
                    lineNo = 158;
                    PDF generator = new PDF(true);
                    generator.CreateStatement(stmt, stmt.BuildingName != "ASTRODON RENTALS" ? true : false, out fileName, isStd);
                    //generator.CreateStatement(stmt, out fileName);
                    if (!String.IsNullOrEmpty(fileName)) { success = true; }
                }
                else
                {
                    lineNo = 164;
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
                MessageBox.Show("Create statement - " + lineNo.ToString() + ":" + ex.Message);
                fileName = String.Empty;
            }
            this.Cursor = Cursors.Arrow;
            return success;
        }

        private String HOAMessage1
        {
            get
            {
                String hoaMessage = "Levies are due and payable on the 1st of every month in advance in terms of the Sectional Titles Act 95 of 1986 as amended and or the Articles of Association of the H.O.A.  Failure to compy will result in penalties being charged and electricity supply to the unit being suspended.";
                return hoaMessage;
            }
        }

        private String BCMessage1
        {
            get
            {
                String hoaMessage = "Levies are due and payable on the 1st of every month in advance in terms of the Sectional Titles Act 95 of 1986 as amended.  Failure to compy will result in penalties being charged and electricity supply to the unit being suspended.";
                return hoaMessage;
            }
        }

        private String Message2
        {
            get
            {
                String hoaMessage = "***PLEASE ENSURE THAT ALL PAYMENTS REFLECTS IN OUR NOMINATED ACCOUNT ON OR BEFORE DUE DATE TO AVOID ANY PENALTIES, REFER TO TERMS AND CONDITIONS.***";
                return hoaMessage;
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

        private void stmtDatePicker_ValueChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            if (CreateStatement(true, out fileName, true))
            {
                Process.Start(fileName);
            }
            else
            {
                MessageBox.Show("Unable to create statement");
            }
        }
    }
}
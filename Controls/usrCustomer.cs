using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class usrCustomer : UserControl
    {
        private List<Building> buildings;
        private List<Customer> customers;
        private List<CustomerDocument> docs;
        private Building building;
        private Customer customer;
        private BindingSource bsDocs = new BindingSource();
        private BindingSource bsReminders = new BindingSource();
        private SqlDataHandler dh = new SqlDataHandler();
        private bool[] sortOrder = new bool[4];
        private Dictionary<int, String> categories;

        public usrCustomer()
        {
            InitializeComponent();
            buildings = new Buildings(false).buildings;
        }

        private void usrCustomer_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            for (int i = 0; i < 4; i++) { sortOrder[i] = true; }
            dgDocs.DataSource = bsDocs;
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            dataGridView1.DataSource = bsReminders;
        }

        private void LoadBuildings()
        {
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.Items.Clear();
            cmbBuilding.DataSource = buildings;
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.SelectedIndex = -1;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        private void LoadCustomers(int selectedIndex)
        {
            cmbCustomer.SelectedIndexChanged -= cmbCustomer_SelectedIndexChanged;
            cmbCustomer.DataSource = null;
            cmbCustomer.Items.Clear();
            if (selectedIndex > -1) { customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath); }
            cmbCustomer.DataSource = customers;
            cmbCustomer.DisplayMember = "accNumber";
            cmbCustomer.ValueMember = "accNumber";
            if (selectedIndex > -1)
            {
                cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
                cmbCustomer.SelectedIndex = selectedIndex;
            }
            else
            {
                cmbCustomer.SelectedIndex = selectedIndex;
                cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                building = buildings[cmbBuilding.SelectedIndex];
                customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath);
                txtAccount.Text = txtAddress1.Text = txtAddress2.Text = txtAddress3.Text = txtAddress4.Text = txtAddress5.Text = String.Empty;
                txtCell.Text = txtContact.Text = txtDescription.Text = txtEmail.Text = txtFax.Text = txtTelephone.Text = String.Empty;
                customer = null;
                ClearCustomer();
                dgTransactions.DataSource = null;
                LoadCustomers(-1);
                categories = Controller.pastel.GetCustomerCategories(building.DataPath);
                Categories stdCat = new Categories
                {
                    categoryID = 0,
                    CategoryName = "None / Standard"
                };
                categories.Add(stdCat.categoryID, stdCat.CategoryName);

                List<Categories> myCats = new List<Categories>();
                foreach (KeyValuePair<int, String> category in categories)
                {
                    Categories cat = new Categories
                    {
                        categoryID = category.Key,
                        CategoryName = category.Value
                    };
                    myCats.Add(cat);
                }
                cmbCategory.DataSource = myCats;
                cmbCategory.ValueMember = "categoryID";
                cmbCategory.DisplayMember = "categoryName";
                cmbCategory.SelectedIndex = -1;
            }
            catch { }
        }

        private void ClearCustomer()
        {
            txtAccount.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtAddress4.Text = "";
            txtAddress5.Text = "";
            txtCell.Text = "";
            txtContact.Text = "";
            txtDescription.Text = "";
            txtEmail.Text = "";
            txtFax.Text = "";
            txtTelephone.Text = "";
            dgTransactions.DataSource = null;
            bsDocs.Clear();
            if (tabControl1.TabPages.Count > 1)
            {
                for (int i = tabControl1.TabPages.Count - 1; i > 0; i--) { tabControl1.TabPages.RemoveAt(i); }
            }
            bsReminders.Clear();
            txtNotes.Text = "";
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                customer = customers[cmbCustomer.SelectedIndex];
                txtAccount.Text = customer.accNumber;
                txtAddress1.Text = customer.address[0];
                txtAddress2.Text = customer.address[1];
                txtAddress3.Text = customer.address[2];
                txtAddress4.Text = customer.address[3];
                txtAddress5.Text = customer.address[4];
                txtCell.Text = customer.CellPhone;
                txtContact.Text = customer.Contact;
                txtDescription.Text = customer.description;
                String email = String.Empty;
                String emailTo = String.Empty;
                var builder = new System.Text.StringBuilder();
                builder.Append(emailTo);
                foreach (String cemail in customer.Email)
                {
                    if (!String.IsNullOrEmpty(cemail) && !cemail.Contains("imp.ad-one"))
                    {
                        email = cemail;
                        builder.Append(cemail + ";");
                        break;
                    }
                }
                emailTo = builder.ToString();
                txtEmail.Text = email;
                txtEmailTo.Text = emailTo;
                txtFax.Text = customer.Fax;
                txtTelephone.Text = customer.Telephone;
                String category;
                txtCategory.Text = (categories.TryGetValue(int.Parse(customer.category), out category) ? category : "-");
                try { cmbCategory.SelectedValue = int.Parse(customer.category); } catch { }
                LoadTransactions();
                LoadDocuments();
                LoadAddress();
                LoadReminders();
                LoadNotes();
                LoadMaintenance();
                    
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                this.Cursor = Cursors.Default;
            }
        }

        private List<UnitMaintenance> _UnitMaintenance;
        private void LoadMaintenance()
        {
            DateTime cutoff = DateTime.Today.AddYears(-2);
            using (var dataContext = SqlDataHandler.GetDataContext())
            {
                var q = from m in dataContext.MaintenanceSet
                        where m.BuildingMaintenanceConfiguration.BuildingId == building.ID
                        && m.CustomerAccount == customer.accNumber
                        && (m.WarrentyExpires > DateTime.Today || m.DateLogged >= cutoff)
                        select new UnitMaintenance
                        {
                            MaintenanceId = m.id,
                            DateLogged = m.DateLogged,
                            IsForBodyCorporate = m.IsForBodyCorporate,
                            TotalAmount = m.TotalAmount,
                            Description = m.Description,
                            WarrentyExpires = m.WarrentyExpires,
                            WarrantyNotes = m.WarrantyNotes,
                            SupplierName = m.Supplier.CompanyName
                        };

                _UnitMaintenance = q.OrderByDescending(a => a.DateLogged).ToList();
            }
            BindMaintenanceDataGrid();
        }

        private void BindMaintenanceDataGrid()
        {
            dgMaintenance.ClearSelection();
            dgMaintenance.MultiSelect = false;
            dgMaintenance.AutoGenerateColumns = false;

            dgMaintenance.Columns.Clear();
            dgMaintenance.DataSource = null;

            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            if (_UnitMaintenance.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = _UnitMaintenance;
                dgMaintenance.DataSource = bs;

                dgMaintenance.Columns.Add(new DataGridViewButtonColumn()
                {
                    HeaderText = "Action",
                    DataPropertyName = "ButtonText",
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "DateLoggedDisplay",
                    HeaderText = "Date",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "SupplierName",
                    HeaderText = "Supplier",
                    ReadOnly = true
                });


                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Description",
                    HeaderText = "Description",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "IsBodyCorporate",
                    HeaderText = "Body Corporate",
                    ReadOnly = true
                });


                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "TotalAmount",
                    HeaderText = "Amount",
                    ReadOnly = true,
                    DefaultCellStyle = currencyColumnStyle
                });


                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "WarrentyExpiresDisplay",
                    HeaderText = "Warrenty Expires",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "WarrantyNotes",
                    HeaderText = "Warranty Notes",
                    ReadOnly = true
                });

            
         

                dgMaintenance.AutoResizeColumns();
            }
        }


        private void LoadAddress()
        {
            List<AdditionalAddress> aas = Controller.pastel.GetDeliveryInfo(building.DataPath, customer.accNumber);
            if (tabControl1.TabPages.Count > 1)
            {
                for (int i = tabControl1.TabPages.Count - 1; i > 0; i--) { tabControl1.TabPages.RemoveAt(i); }
            }
            if (aas.Count > 0)
            {
                int addCount = 1;
                foreach (AdditionalAddress aa in aas)
                {
                    TabPage tbAA = new TabPage();
                    Panel p = new Panel { Dock = DockStyle.Fill };
                    Astrodon.Controls.usrDelAddress delControl = new Controls.usrDelAddress(aa) { Dock = DockStyle.Fill };
                    p.Controls.Add(delControl);
                    tbAA.Controls.Add(p);
                    tbAA.Text = "Additional Address " + addCount.ToString();
                    tabControl1.TabPages.Add(tbAA);
                    addCount++;
                }
                tabControl1.Invalidate();
            }
        }

        private void LoadReminders()
        {
            bsReminders.Clear();
            String remQuery = "SELECT r.id, u.name, r.remDate, r.remNote, r.action FROM tblReminders r  INNER JOIN tblUsers u ON r.userid = u.id WHERE r.customer = '" + customer.accNumber + "' AND action = 'False' ORDER BY remDate";

            String status;
            DataSet dsRem = dh.GetData(remQuery, null, out status);
            if (dsRem != null && dsRem.Tables.Count > 0 && dsRem.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drRem in dsRem.Tables[0].Rows)
                {
                    Reminder r = new Reminder
                    {
                        action = bool.Parse(drRem["action"].ToString()),
                        User = drRem["name"].ToString(),
                        id = int.Parse(drRem["id"].ToString()),
                        note = drRem["remNote"].ToString(),
                        remDate = DateTime.Parse(drRem["remDate"].ToString())
                    };
                    bsReminders.Add(r);
                    dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                    dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
                }
            }
        }

        private void LoadNotes()
        {
            txtNotes.Text = "";
            String noteQuery = "SELECT * FROM tblCustomerNotes WHERE customer = '" + customer.accNumber + "' ORDER BY noteDate desc";
            String status;
            DataSet dsNotes = dh.GetData(noteQuery, null, out status);
            if (dsNotes != null && dsNotes.Tables.Count > 0 && dsNotes.Tables[0].Rows.Count > 0)
            {
                var builder = new System.Text.StringBuilder();
                builder.Append(txtNotes.Text);
                foreach (DataRow drNote in dsNotes.Tables[0].Rows)
                {
                    builder.Append(DateTime.Parse(drNote["noteDate"].ToString()).ToString("yyyy/MM/dd HH:mm") + ": " + drNote["notes"].ToString() + Environment.NewLine);
                }
                txtNotes.Text = builder.ToString();
            }
        }

        private void LoadTransactions()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cmbCustomer.SelectedIndex >= 0)
                {
                    customer = customers[cmbCustomer.SelectedIndex];
                }
                else
                {
                    MessageBox.Show("Please select a customer", "Statements", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                double totalDue = 0;
                String trnMsg;
                List<Transaction> transactions = (new Classes.LoadTrans()).LoadTransactions(building, customer, DateTime.Now, out totalDue, out trnMsg);
                if (transactions != null && transactions.Count > 0)
                {
                    dgTransactions.DataSource = transactions;
                    dgTransactions.Invalidate();
                    Application.DoEvents();
                    if (transactions.Count > 0)
                    {
                        dgTransactions.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgTransactions.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                else
                {
                    totalDue = 0;
                    dgTransactions.DataSource = null;
                }
                lblOS.Text = totalDue.ToString("#,##0.00");
                if (totalDue > 0) { lblOS.ForeColor = System.Drawing.Color.Red; } else { lblOS.ForeColor = System.Drawing.Color.Black; }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String[] delAddress = customer.getDelAddress();
            String[] uDef = customer.userDefined;

            if (delAddress.Length < 5)
            {
                delAddress = new string[] { "", "", "", "", "" };
                customer.SetDelAddress(delAddress);
            }
            if (uDef.Length < 5)
            {
                uDef = new string[] { "", "", "", "", "" };
                customer.userDefined = uDef;
            }

            String customerString = txtAccount.Text + "|" + txtDescription.Text + "|" + txtAddress1.Text + "|" + txtAddress2.Text + "|" + txtAddress3.Text;
            customerString += "|" + txtAddress4.Text + "|" + txtAddress5.Text + "|" + txtTelephone.Text + "|" + txtFax.Text + "|" + txtContact.Text + "|";
            customerString += customer.overRideTax + "|" + customer.settlementTerms.ToString() + "|" + customer.priceRegime.ToString();
            customerString += "|" + customer.Salesanalysis + "|" + delAddress[0] + "|" + delAddress[1] + "|" + delAddress[2] + "|" + delAddress[3] + "|" + delAddress[4];
            customerString += "|" + customer.blocked + "|" + (customer.discount / 100).ToString("#0.00") + "|N|" + customer.statPrintorEmail.ToString();
            String newCat = "";
            if (cmbCategory.SelectedItem != null)
            {
                int catID = (int)cmbCategory.SelectedValue;
                newCat = catID.ToString();
            }
            customerString += "|N|" + newCat + "|" + customer.currencyCode.ToString() + "|" + customer.paymentTerms.ToString();
            customerString += "|" + customer.creditLimit.ToString() + "|" + uDef[0] + "|" + uDef[1] + "|" + uDef[2] + "|" + uDef[3] + "|" + uDef[4];
            customerString += "|" + customer.monthOrDay + "|" + customer.statPrintorEmail.ToString() + "|" + customer.docPrintorEmail.ToString();
            customerString += "|" + txtCell.Text + "|" + txtEmail.Text + "|" + customer.freight + "|" + customer.ship + "|";
            customerString += customer.taxCode.ToString() + "|" + customer.cashAccount;
            txtEntry.Text = customerString;
            String result = Controller.pastel.UpdateCustomer(customerString, building.DataPath);
            if (result == "0")
            {
                MySqlConnector mySqlConn = new MySqlConnector();
                String status = String.Empty;
                mySqlConn.ToggleConnection(true);
                if (customer.Email.Length == 0) { customer.Email = new string[] { customer.accNumber + "@astrodon.co.za" }; }
                String[] emails = { txtEmail.Text };
                bool updatedWeb = mySqlConn.UpdateWebCustomer(building.Name, customer.accNumber, emails);
                //mySqlConn.InsertCustomer(building, txtAccount.Text, new string[] { txtEmail.Text }, out status);
                mySqlConn.ToggleConnection(false);
                MessageBox.Show(!updatedWeb ? "Pastel updated! Cannot save customer on web!" : "Customer updated!");
            }
            else
            {
                MessageBox.Show("Cannot save customer: " + result);
            }
            LoadCustomers(cmbCustomer.SelectedIndex);
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            if (building != null)
            {
                txtAddress1.Text = building.addy1;
                txtAddress2.Text = building.addy2;
                txtAddress3.Text = building.addy3;
                txtAddress4.Text = building.addy4;
                txtAddress5.Text = building.addy5;
            }
        }

        private void dgTransactions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //3 & 4
            dgTransactions.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgTransactions.Columns[3].DefaultCellStyle.Format = "N2";
            dgTransactions.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgTransactions.Columns[4].DefaultCellStyle.Format = "N2";
        }

        private void LoadDocuments()
        {
            try
            {
                String customerCode = customer.accNumber;
                MySqlConnector mySqlConnector = new MySqlConnector();
                DataSet dsDocs = building.Name != "ASTRODON RENTALS" ? mySqlConnector.GetFiles(customerCode, building.Name) : mySqlConnector.GetFilesRental(customerCode);
                docs = new List<CustomerDocument>();
                if (dsDocs != null && dsDocs.Tables.Count > 0 && dsDocs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drDoc in dsDocs.Tables[0].Rows)
                    {
                        CustomerDocument crDoc = new CustomerDocument();
                        crDoc.tstamp = UnixTimeStampToDateTime(double.Parse(drDoc["tstamp"].ToString()));
                        crDoc.title = drDoc["title"].ToString();
                        if (crDoc.title.ToUpper().Contains("REMINDER"))
                        {
                            crDoc.subject = "Reminder";
                        }
                        else if (crDoc.title.ToUpper().Contains("FINALDEMAND"))
                        {
                            crDoc.subject = "Final Demand";
                        }
                        else if (crDoc.title.ToUpper().Contains("SUMMONS"))
                        {
                            crDoc.subject = "Summons Pending";
                        }
                        else if (crDoc.title.ToUpper().Contains("DISCONNECT"))
                        {
                            crDoc.subject = "Restriction Notice";
                        }
                        else if (crDoc.title.ToUpper().Contains("STATEMENT"))
                        {
                            crDoc.subject = "Statement";
                        }
                        else
                        {
                            crDoc.subject = "Other";
                        }
                        crDoc.file = drDoc["file"].ToString();
                        docs.Add(crDoc);
                    }
                }
                docs = docs.OrderBy(c => c.tstamp).ToList();
                bsDocs.Clear();
                foreach (CustomerDocument doc in docs) { bsDocs.Add(doc); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void dgDocs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIdx = e.ColumnIndex;
            if (e.RowIndex >= 0 && colIdx < 3)
            {
                try
                {
                    CustomerDocument cd = docs[e.RowIndex];
                    if (download(cd.file))
                    {
                        if (colIdx == 0)
                        {
                            System.Diagnostics.Process.Start(Path.Combine(Path.GetTempPath(), cd.file));
                        }
                        else if (colIdx == 2)
                        {
                            using (Forms.frmPrompt prompt = new Forms.frmPrompt("Password", "Please enter password"))
                            {
                                if (prompt.ShowDialog() == DialogResult.OK && prompt.fileName == "45828")
                                {
                                    deleteFile(cd.file);
                                }
                            }
                        }
                        else
                        {
                            String status;
                            String[] att = { Path.Combine(Path.GetTempPath(), cd.file) };
                            String[] emailTo = txtEmailTo.Text.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            if (Mailer.SendMail("noreply@astrodon.co.za", emailTo, "Customer Statements", CustomerMessage(customer.accNumber, building.Debtor), false, false, false, out status, att))
                            {
                                MessageBox.Show("Message Sent");
                            }
                            else
                            {
                                MessageBox.Show("Unable to send mail: " + status);
                            }
                        }
                    }
                }
                catch { }
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

        private bool download(String fileName)
        {
            Classes.Sftp sftpClient = new Classes.Sftp(String.Empty, true);
            String status = "";
            bool success = sftpClient.Download(Path.Combine(Path.GetTempPath(), fileName), fileName, false, out status);
            if (!success) { MessageBox.Show(status); }
            return success;
        }

        private bool deleteFile(String fileName)
        {
            Classes.Sftp sftpClient = new Classes.Sftp(String.Empty, true);
            String status = "";
            bool success = sftpClient.DeleteFile(fileName, false);
            if (!success)
            {
                MessageBox.Show(status);
                return success;
            }
            else
            {
                MySqlConnector mySql = new MySqlConnector();
                return mySql.SetData("DELETE FROM tx_astro_docs WHERE file = '" + fileName + "'", null, out status);
            }
        }

        private void dgDocs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int colIdx = e.ColumnIndex;
            if (colIdx > -1)
            {
                String memberName = "";
                switch (colIdx)
                {
                    case 0:
                        memberName = "Date";
                        break;

                    case 1:
                        memberName = "Subject";
                        break;

                    case 2:
                        memberName = "Title";
                        break;

                    case 3:
                        memberName = "File";
                        break;
                }
                if (!String.IsNullOrEmpty(memberName))
                {
                    sortOrder[colIdx] = !sortOrder[colIdx];
                    docs.Sort(new DocsComparer(memberName, (sortOrder[colIdx] ? SortOrder.Ascending : SortOrder.Descending)));
                    bsDocs.Clear();
                    foreach (CustomerDocument doc in docs) { bsDocs.Add(doc); }
                }
            }
        }

        private void btnSaveReminder_Click(object sender, EventArgs e)
        {
            DateTime remDate = new DateTime(dtRemDate.Value.Year, dtRemDate.Value.Month, dtRemDate.Value.Day, dtRemTime.Value.Hour, dtRemTime.Value.Minute, 0);
            String note = txtNote.Text;
            if (!String.IsNullOrEmpty(note))
            {
                String insertRemQuery = "INSERT INTO tblReminders(userid, customer, building, remDate, remNote) VALUES(@userid, @building, @customer, @remDate, @remNote)";
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                sqlParms.Add("@customer", customer.accNumber);
                sqlParms.Add("@building", building.ID);
                sqlParms.Add("@userid", Controller.user.id);
                sqlParms.Add("@remDate", remDate);
                sqlParms.Add("@remNote", note);
                String status;
                dh.SetData(insertRemQuery, sqlParms, out status);
                dtRemDate.Value = DateTime.Now;
                dtRemTime.Value = DateTime.Now;
                txtNote.Text = "";
                LoadReminders();
            }
            else
            {
                MessageBox.Show("Please enter a note for your reminder");
            }
        }

        private void btnSaveNote_Click(object sender, EventArgs e)
        {
            if (customer == null || String.IsNullOrEmpty(customer.accNumber))
            {
                MessageBox.Show("Please select a customer", "Customer file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!String.IsNullOrEmpty(txtNewNote.Text))
            {
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                String status;
                sqlParms.Add("@customer", customer.accNumber);
                sqlParms.Add("@notes", txtNewNote.Text);
                String updateNotesQuery = "INSERT INTO tblCustomerNotes(customer, notes) VALUES(@customer, @notes)";
                dh.SetData(updateNotesQuery, sqlParms, out status);
                if (status != "")
                {
                    MessageBox.Show("Error: " + status);
                }
                else
                {
                    MessageBox.Show("Note saved");
                }
                txtNewNote.Text = "";
                LoadNotes();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                    bool actioned = (bool)dataGridView1.Rows[e.RowIndex].Cells[3].Value;
                    String updateRemQuery = "UPDATE tblReminders SET action = '" + actioned.ToString() + "', actionDate = getdate() WHERE id = " + id.ToString();
                    String status;
                    dh.SetData(updateRemQuery, null, out status);
                    LoadReminders();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private class Categories
        {
            public int categoryID { get; set; }
            public String CategoryName { get; set; }
        }

        private class UnitMaintenance
        {
            public int MaintenanceId { get; set; }
            public DateTime DateLogged { get; set; }
            public string DateLoggedDisplay { get { return DateLogged.ToString("yyyy-MM-dd"); } }

            public bool IsForBodyCorporate { get; set; }
            public string IsBodyCorporate { get { return IsForBodyCorporate ? "Yes" : "No"; } }

            public decimal TotalAmount { get; set; }
            public string WarrantyNotes { get; set; }
            public DateTime? WarrentyExpires { get; set; }
            public string WarrentyExpiresDisplay { get { return WarrentyExpires == null ? "" : WarrentyExpires.Value.ToString("yyyy/MM/dd"); } }
            public string Description { get; set; }

            public string SupplierName { get; set; }
            public string ButtonText { get { return "View"; } }

        }

        private void dgMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var selectedItem = senderGrid.Rows[e.RowIndex].DataBoundItem as UnitMaintenance;
                if(selectedItem != null)
                {
                    using (var dataContext = SqlDataHandler.GetDataContext())
                    {
                        var frmMaintenanceDetail = new Astrodon.Forms.frmMaintenanceDetail(dataContext, selectedItem.MaintenanceId,true);
                        var dialogResult = frmMaintenanceDetail.ShowDialog();
                    }
                }
            }
        }
    }
}
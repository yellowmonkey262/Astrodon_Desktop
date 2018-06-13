﻿using Astro.Library.Entities;
using Astrodon.Classes;
using Astrodon.Data;
using Astrodon.Data.DebitOrder;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
            LoadBanks();
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
            buildings = new Buildings(false).buildings;
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.Items.Clear();
            cmbBuilding.DataSource = buildings;
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.SelectedIndex = -1;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        private void ActivateTrustees()
        {
            int userid = Controller.user.id;
            if (userid == 1 || userid == 2 || userid == 27 || Controller.user.usertype == 2)
            {
                btnTrustees.Visible = true;
                btnTrustees.Enabled = true;
                btnTrustees.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                btnTrustees.Visible = false;
            }
        }

        private void LoadCustomers(int selectedIndex)
        {
            btnTrustees.BackColor = System.Drawing.Color.Red;
            cmbCustomer.SelectedIndexChanged -= cmbCustomer_SelectedIndexChanged;
            cmbCustomer.DataSource = null;
            cmbCustomer.Items.Clear();
            if (selectedIndex > -1)
            {
                customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath,true);
            }
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
            ActivateTrustees();
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DisplayPDFNew(string.Empty);

                this.axAcroPDF1.Visible = false;
                btnUpload.Visible = false;
                building = buildings[cmbBuilding.SelectedIndex];

                customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath,true);

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

                var trustee = myCats.Where(a => a.categoryID == 7).FirstOrDefault();
                if (trustee != null)
                   myCats.Remove(trustee);

                cmbCategory.DataSource = myCats;
                cmbCategory.ValueMember = "categoryID";
                cmbCategory.DisplayMember = "categoryName";
                cmbCategory.SelectedIndex = -1;
                UpdateSQLCustomers();
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

            this.axAcroPDF1.Visible = false;
            btnUpload.Visible = false;
            cbDebitOrderActive.Checked = false;
            cbBanks.SelectedIndex = -1;
            txtBranchCode.Text =string.Empty;
            txtAccountNumber.Text = string.Empty;
            tbMaxAmount.Text = string.Empty;
            cbAccountType.SelectedIndex = -1;
            cbProcessDate.SelectedIndex = -1;
            dtpDebitOrderCancelled.Visible = false;
            dtpDebitOrderCancelled.Value = DateTime.Today.AddMonths(1);
            dtpDebitOrderCancelled.Format = DateTimePickerFormat.Custom;
            dtpDebitOrderCancelled.CustomFormat = "yyyy/MM/dd";
            cbDebitOrderCancelled.Checked = false;
            cbTrustee.Checked = false;
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
                LoadWeb();
                LoadDebitOrder();
                LoadCustomerEntity();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                this.Cursor = Cursors.Default;
            }
        }

        private void LoadWeb()
        {
            try
            {
                //if (building.Name != "ASTRODON RENTALS")
                //{
                    bool loginFound = false;
                    MySqlConnector mySql = new MySqlConnector();
                    //mySql.ToggleConnection(true);
                    String[] emails = txtEmail.Text.Split(new String[] { ";" }, StringSplitOptions.None);
                    int i = 0;
                    String uid = "0";
                    List<String> linkedUnits = new List<string>();
                    while (!loginFound)
                    {
                        String password = mySql.GetLoginPassword(emails[i], out uid);
                        if (uid != "0")
                        {
                            txtWebLogin.Text = emails[i];
                            txtWebPassword.Text = password;
                            linkedUnits = mySql.GetLinkedUnits(uid);
                            loginFound = true;
                        }
                        i++;
                    }
                    if (!loginFound)
                    {
                        txtWebLogin.Text = "Not found";
                        txtWebPassword.Text = "Not found";
                    }
                    lstUnits.Items.Clear();
                    foreach (String linkedUnit in linkedUnits)
                    {
                        lstUnits.Items.Add(linkedUnit);
                    }
                //}
            }
            catch
            {
                //UpdateCustomer(false);
                //LoadWeb();
            }
            lstUnits.Refresh();
        }

        private List<UnitMaintenance> _UnitMaintenance;

        private void LoadMaintenance()
        {
            DateTime cutoff = DateTime.Today.AddYears(-2);
            using (var dataContext = SqlDataHandler.GetDataContext())
            {
                var q = from m in dataContext.MaintenanceSet
                        from md in m.DetailItems
                        where m.BuildingMaintenanceConfiguration.BuildingId == building.ID
                        && md.CustomerAccount == customer.accNumber
                        && (m.WarrentyExpires > DateTime.Today || m.DateLogged >= cutoff)
                        select new UnitMaintenance
                        {
                            MaintenanceId = m.id,
                            DateLogged = m.DateLogged,
                            IsForBodyCorporate = md.IsForBodyCorporate,
                            TotalAmount = md.Amount,
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
            DateTime start = DateTime.Today.AddMonths(6);

            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from r in context.tblReminders
                        where r.BuildingId == building.ID
                        && r.customer == customer.accNumber
                        && (r.action == false || r.actionDate == null || r.actionDate > start)
                        select new Reminder()
                        {
                            action = r.action,
                            User = r.User.name,
                            id = r.id,
                            note = r.remNote,
                            remDate = r.remDate
                        };
                bsReminders.DataSource = q.OrderBy(a => a.remDate).ToList();
                dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            }

            //    String remQuery = "SELECT r.id, u.name, r.remDate, r.remNote, r.action FROM tblReminders r  INNER JOIN tblUsers u ON r.userid = u.id WHERE r.customer = '" + customer.accNumber + "' AND action = 'False' ORDER BY remDate";

            //String status;
            //DataSet dsRem = dh.GetData(remQuery, null, out status);
            //if (dsRem != null && dsRem.Tables.Count > 0 && dsRem.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow drRem in dsRem.Tables[0].Rows)
            //    {
            //        Reminder r = new Reminder
            //        {
            //            action = bool.Parse(drRem["action"].ToString()),
            //            User = drRem["name"].ToString(),
            //            id = int.Parse(drRem["id"].ToString()),
            //            note = drRem["remNote"].ToString(),
            //            remDate = DateTime.Parse(drRem["remDate"].ToString())
            //        };
            //        bsReminders.Add(r);
            //        dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            //        dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            //    }
            //}
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
            UpdateCustomer(true);
        }

        private void UpdateCustomer(bool showMessage)
        {
            UpdateTrusteeTick();

            if (customer == null)
            {
                Controller.HandleError("Customer not selected");
                return;
            }
            String[] delAddress = customer.getDelAddress();
            String[] uDef = customer.userDefined;

            if(delAddress == null)
                delAddress = new string[] { "", "", "", "", "" };

            if (uDef == null)
                uDef = new string[] { "", "", "", "", "" };

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
            if (Controller.user.id == 1) { MessageBox.Show(building.DataPath + " === " + customerString); }
            if (result == "0")
            {
                MySqlConnector mySqlConn = new MySqlConnector();
                String status = String.Empty;
                mySqlConn.ToggleConnection(true);
                if (customer.Email.Length == 0)
                {
                    customer.Email = new string[] { customer.accNumber + "@astrodon.co.za" };
                }

                String[] emails = { txtEmail.Text };
                if (cbTrustee.Checked)
                {
                    if (emails == null || emails.Length == 0 || String.IsNullOrWhiteSpace(emails[0]))
                    {
                        Controller.HandleError("Trustee " + customer.description + " does not have an email address configured.\r" +
                            "The trustee will not be able to login to the website without an email\r" +
                            "Please configure an email address and try again.");
                    }
                }
                if (emails != null && emails.Length > 0 && !String.IsNullOrWhiteSpace(emails[0]))
                {
                    bool updatedWeb = mySqlConn.UpdateWebCustomer(building.Name, customer.accNumber, emails);
                    foreach (var email in emails)
                    {
                        String[] logins = mySqlConn.HasLogin(email);
                        if (logins != null)
                        {
                            foreach (var login in logins)
                            {
                                if (login != null)
                                {
                                    string usergroup;
                                    if (cbTrustee.Checked)
                                    {
                                        usergroup = "1,2,4";
                                    }
                                    else
                                    {
                                        usergroup = "1,2";
                                    }
                                    mySqlConn.UpdateGroup(login, usergroup);
                                }
                            }
                        }
                    }
                    if (showMessage)
                    {
                        MessageBox.Show(!updatedWeb ? "Pastel updated! Cannot save customer on web!" : "Customer updated!");
                    }
                }
                //mySqlConn.InsertCustomer(building, txtAccount.Text, new string[] { txtEmail.Text }, out status);
                mySqlConn.ToggleConnection(false);
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
                foreach (CustomerDocument doc in docs.OrderByDescending(a => a.tstamp))
                {
                    bsDocs.Add(doc);
                }
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
                    var cd = dgDocs.Rows[e.RowIndex].DataBoundItem as CustomerDocument;
                    if (download(cd.file))
                    {
                        if (colIdx == 0)
                        {
                            DisplayPDFNew(Path.Combine(Path.GetTempPath(), cd.file));
                           // System.Diagnostics.Process.Start(Path.Combine(Path.GetTempPath(), cd.file));
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


        private void DisplayPDFNew(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                this.axAcroPDFNew.Visible = false;
                return;
            }

            try
            {
                this.axAcroPDFNew.Visible = true;
                this.axAcroPDFNew.LoadFile(filePath);
                this.axAcroPDFNew.src = filePath;
                this.axAcroPDFNew.setShowToolbar(false);
                this.axAcroPDFNew.setView("FitH");
                this.axAcroPDFNew.setLayoutMode("SinglePage");
                this.axAcroPDFNew.setShowToolbar(false);

                this.axAcroPDFNew.Show();
                tbPreview.SelectedTab = tbPreviewPage;
            }
            catch (Exception ex)
            {
                throw ex;
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
            string outputFilePath = Path.Combine(Path.GetTempPath(), fileName);
            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            bool success = sftpClient.Download(outputFilePath, fileName, false, out status);
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
            DateTime reminderDate = new DateTime(dtRemDate.Value.Year, dtRemDate.Value.Month, dtRemDate.Value.Day, dtRemTime.Value.Hour, dtRemTime.Value.Minute, 0);
            String note = txtNote.Text;
            if (!String.IsNullOrEmpty(note))
            {

                using (var context = SqlDataHandler.GetDataContext())
                {
                    string email = string.Empty;
                    if (customer.Email != null && customer.Email.Length > 0)
                    {
                        foreach(var eml in customer.Email)
                        {
                            if (!string.IsNullOrWhiteSpace(email))
                                email = email + ";" + eml;
                            else
                                email = eml;
                        }
                        email = customer.Email[0];
                    }

                    var reminder = new tblReminder()
                    {
                        UserId = Controller.user.id,
                        customer = customer.accNumber,
                        BuildingId = building.ID,
                        remDate = reminderDate,
                        remNote = note,
                        Contacts = customer.Contact,
                        Phone = customer.CellPhone,
                        Fax = customer.Fax,
                        Email = email,
                    };
                    context.tblReminders.Add(reminder);
                    context.SaveChanges();
                }

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
                if (selectedItem != null)
                {
                    using (var dataContext = SqlDataHandler.GetDataContext())
                    {
                        var frmMaintenanceDetail = new Astrodon.Forms.frmMaintenanceDetail(dataContext, selectedItem.MaintenanceId, true);
                        var dialogResult = frmMaintenanceDetail.ShowDialog();
                    }
                }
            }
        }

        private void btnTrustees_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            MySqlConnector myConn = new MySqlConnector();
            myConn.ToggleConnection(true);
            String usergroup = "";

            using (var context = SqlDataHandler.GetDataContext())
            {

                foreach (var entity in context.CustomerSet.Where(a => a.BuildingId == building.ID).ToList())
                {
                    var c = customers.Where(a => a.accNumber == entity.AccountNumber).FirstOrDefault();
                    if (c != null)
                    {
                        if (entity.IsTrustee)
                        {
                            if (c.Email == null || c.Email.Length == 0)
                            {
                                Controller.HandleError("Trustee " + customer.description + " does not have an email address configured.\r" +
                                    "The trustee will not be able to login to the website without an email\r" +
                                    "Please configure an email address and try again.");
                            }
                        }
                        if (c.Email != null)
                        {
                            foreach (String email in c.Email)
                            {
                                if (email != "sheldon@astrodon.co.za")
                                {
                                    String[] logins = myConn.HasLogin(email);
                                    if (logins != null)
                                    {
                                        foreach (var login in logins)
                                        {
                                            if (login != null)
                                            {
                                                if (entity.IsTrustee)
                                                {
                                                    usergroup = "1,2,4";
                                                }
                                                else
                                                {
                                                    usergroup = "1,2";
                                                }
                                                myConn.UpdateGroup(login, usergroup);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            myConn.ToggleConnection(false);
            this.Cursor = Cursors.Arrow;
            MessageBox.Show("Complete");
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }
        #region Debit Order

        private List<Astrodon.Data.BankData.Bank> _Banks;

        private void LoadBanks()
        {
            //_Banks
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    _Banks = context.BankSet.Where(a => a.IsActive).ToList();
                    cbBanks.DataSource = _Banks;
                    cbBanks.ValueMember = "Id";
                    cbBanks.DisplayMember = "Name";
                    cbBanks.SelectedIndex = -1;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            cbAccountType.Items.Clear();
            foreach ( AccountTypeType c in Enum.GetValues(typeof(AccountTypeType)))
            {
                cbAccountType.Items.Add(c);
            }

            cbProcessDate.Items.Clear();
            foreach (DebitOrderDayType c in Enum.GetValues(typeof(DebitOrderDayType)))
            {
                cbProcessDate.Items.Add(c);
            }
        }

        private void LoadDebitOrder()
        {
            _promptForDefaults = false;
            try
            {
                btnUpload.Visible = false;
                cbDisableDebitOrderFee.Enabled = Controller.UserIsSheldon();
                cbDisableDebitOrderFee.Checked = false;
                cbDebitOrderActive.Checked = false;
                cbBanks.SelectedIndex = -1;
                txtBranchCode.Text = "";
                tbMaxAmount.Text = "";
                txtAccountNumber.Text = "";
                cbAccountType.SelectedIndex = -1;
                cbProcessDate.SelectedIndex = -1;
                dtpDebitOrderCancelled.Format = DateTimePickerFormat.Custom;
                dtpDebitOrderCancelled.CustomFormat = "yyyy/MM/dd";
                cbDebitOrderCancelled.Checked = false;
                dtpDebitOrderCancelled.Visible = cbDebitOrderCancelled.Checked;

                DisplayPDF(null);
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var customerEntity = context.CustomerSet.SingleOrDefault(a => a.BuildingId == building.ID && a.AccountNumber == customer.accNumber);
                    if(customerEntity == null)
                    {
                        customerEntity = new Data.CustomerData.Customer()
                        {
                            BuildingId = building.ID,
                            AccountNumber = customer.accNumber,
                            Description = customer.description,
                            IsTrustee = customer.IsTrustee,
                            Created = DateTime.Now
                        };
                        context.CustomerSet.Add(customerEntity);
                        context.SaveChanges();
                    }
                    cbTrustee.Checked = customerEntity.IsTrustee;

                    var debitOrder = context.CustomerDebitOrderSet.SingleOrDefault(a => a.BuildingId == building.ID && a.CustomerCode == customer.accNumber);
                    if (debitOrder != null)
                    {
                        cbDebitOrderActive.Checked = debitOrder.IsActive;
                        cbBanks.SelectedItem = _Banks.FirstOrDefault(a => a.id == debitOrder.BankId);
                        txtBranchCode.Text = debitOrder.BranceCode;
                        tbMaxAmount.Text = debitOrder.MaxDebitAmount.ToString("###,##0.00", CultureInfo.InvariantCulture);
                        txtAccountNumber.Text = debitOrder.AccountNumber;
                        cbAccountType.SelectedItem = debitOrder.AccountType;
                        cbProcessDate.SelectedItem = debitOrder.DebitOrderCollectionDay;
                        cbDisableDebitOrderFee.Checked = debitOrder.IsDebitOrderFeeDisabled;
                        btnUpload.Visible = true;
                        cbDebitOrderCancelled.Checked = debitOrder.DebitOrderCancelled;
                        if (cbDebitOrderCancelled.Checked)
                        {
                            dtpDebitOrderCancelled.Visible = true;
                            dtpDebitOrderCancelled.Value = debitOrder.DebitOrderCancelDate == null ? DateTime.Today : debitOrder.DebitOrderCancelDate.Value;
                        }

                        var signedForm = context.DebitOrderDocumentSet.SingleOrDefault(a => a.CustomerDebitOrderId == debitOrder.id
                                             && a.DocumentType == DebitOrderDocumentType.SignedDebitOrder);
                        if (signedForm != null)
                        {
                            DisplayPDF(signedForm.FileData);
                        }
                    }
                    LoadArchiveGrid(context);
                }
            }
            finally
            {
                _promptForDefaults = true;
            }
        }

        private void LoadArchiveGrid(Data.DataContext context)
        {
            var debitOrderArchive = context.CustomerDebitOrderArchiveSet.Where(a => a.BuildingId == building.ID && a.CustomerCode == customer.accNumber)
                .Select(a => new DebitOrderArchiveItem()
                {
                    DebitOrderCancelDate = a.DebitOrderCancelDate,
                    BankName = a.Bank.Name,
                    AccountNumber = a.AccountNumber,
                    UserUpdate = a.UserUpdate.name,
                    DateArchived = a.DateArchived,
                    MaxDebitAmount = a.MaxDebitAmount,
                    DebitOrderCancelled = a.DebitOrderCancelled ? "Yes" : "No"
                }).ToList();
            LoadDebitOrderArchiveGrid(debitOrderArchive);
        }

        class DebitOrderArchiveItem
        {
            public DateTime? DebitOrderCancelDate { get; set; }
            public string BankName { get; set; }
            public string AccountNumber { get; set; }
            public string UserUpdate { get; set; }
            public decimal MaxDebitAmount { get; set; }
            public DateTime? DateArchived { get; set; }
            public string DebitOrderCancelled { get; set; }

        }

        private void LoadDebitOrderArchiveGrid(List<DebitOrderArchiveItem> debitOrderArchive)
        {
            dgDebitOrderArchive.ClearSelection();
            dgDebitOrderArchive.MultiSelect = false;
            dgDebitOrderArchive.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = debitOrderArchive;

            dgDebitOrderArchive.Columns.Clear();

            dgDebitOrderArchive.DataSource = bs;

            dgDebitOrderArchive.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DateArchived",
                HeaderText = "Date",
                ReadOnly = true
            });
            dgDebitOrderArchive.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BankName",
                HeaderText = "Bank",
                ReadOnly = true
            });
            dgDebitOrderArchive.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountNumber",
                HeaderText = "Account Number",
                ReadOnly = true
            });
            dgDebitOrderArchive.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DebitOrderCancelled",
                HeaderText = "Cancelled",
                ReadOnly = true
            });
            dgDebitOrderArchive.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DebitOrderCancelDate",
                HeaderText = "Cancel Date",
                ReadOnly = true
            });
            dgDebitOrderArchive.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "MaxDebitAmount",
                HeaderText = "Max Amount",
                ReadOnly = true
            });
            dgDebitOrderArchive.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UserUpdate",
                HeaderText = "Updated by",
                ReadOnly = true
            });
        }

      

        private void btnSaveDebitOrder_Click(object sender, EventArgs e)
        {
            if (cbBanks.SelectedIndex < 0)
            {
                Controller.HandleError("Bank required", "Validation Error");
                return;
            }
            if (cbAccountType.SelectedIndex < 0)
            {
                Controller.HandleError("Account Type required", "Validation Error");
                return;
            }
            if (cbProcessDate.SelectedIndex < 0)
            {
                Controller.HandleError("Process Date required", "Validation Error");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtBranchCode.Text))
            {
                Controller.HandleError("Branch code required", "Validation Error");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                Controller.HandleError("Account number required", "Validation Error");
                return;
            }

            if (String.IsNullOrWhiteSpace(tbMaxAmount.Text))
                tbMaxAmount.Text = "0";

            decimal maxAmount = 0;
            try
            {
                maxAmount = Convert.ToDecimal(tbMaxAmount.Text);
            }
            catch
            {
                Controller.HandleError("Debit order max amount invalid", "Validation Error");
                return;
            }

            if(maxAmount < 0)
            {
                Controller.HandleError("Debit order max amount cannot be negative", "Validation Error");
                return;
            }

            using (var context = SqlDataHandler.GetDataContext())
            {
                var debitOrder = context.CustomerDebitOrderSet.SingleOrDefault(a => a.BuildingId == building.ID && a.CustomerCode == customer.accNumber);
                if (debitOrder == null)
                {
                    debitOrder = new CustomerDebitOrder()
                    {
                        BuildingId = building.ID,
                        CustomerCode = customer.accNumber
                    };
                    context.CustomerDebitOrderSet.Add(debitOrder);
                }
                else
                {
                    //create the arvice
                    var archive = new CustomerDebitOrderArchive()
                    {
                        BuildingId = debitOrder.BuildingId,
                        CustomerCode = debitOrder.CustomerCode,
                        DateArchived = DateTime.Now,
                        BankId = debitOrder.BankId,
                        BranceCode = debitOrder.BranceCode,
                        AccountNumber = debitOrder.AccountNumber,
                        AccountType = debitOrder.AccountType,
                        DebitOrderCollectionDay = debitOrder.DebitOrderCollectionDay,
                        IsActive = debitOrder.IsActive,
                        LastUpdatedByUserId = Controller.user.id,
                        LastUpdateDate = DateTime.Now,
                        IsDebitOrderFeeDisabled = debitOrder.IsDebitOrderFeeDisabled,
                        DebitOrderCancelDate = debitOrder.DebitOrderCancelDate,
                        MaxDebitAmount = debitOrder.MaxDebitAmount,
                        Documents = new List<DebitOrderDocumentArchive>()
                    };
                    foreach (var doc in debitOrder.Documents)
                    {
                        archive.Documents.Add(new DebitOrderDocumentArchive()
                        {
                            CustomerDebitOrderArchive = archive,
                            FileData = doc.FileData,
                            FileName = doc.FileName
                        });
                    }
                    context.CustomerDebitOrderArchiveSet.Add(archive);
                }
                debitOrder.IsActive = cbDebitOrderActive.Checked;
                debitOrder.BankId = (cbBanks.SelectedItem as Data.BankData.Bank).id;
                debitOrder.BranceCode = txtBranchCode.Text;
                debitOrder.AccountNumber = txtAccountNumber.Text;
                debitOrder.AccountType = (AccountTypeType)cbAccountType.SelectedItem;
                debitOrder.DebitOrderCollectionDay = (DebitOrderDayType)cbProcessDate.SelectedItem;
                debitOrder.LastUpdatedByUserId = Controller.user.id;
                debitOrder.LastUpdateDate = DateTime.Now;
                debitOrder.IsDebitOrderFeeDisabled = cbDisableDebitOrderFee.Checked;
                debitOrder.DebitOrderCancelled = cbDebitOrderCancelled.Checked;
                debitOrder.MaxDebitAmount = maxAmount;
                if (debitOrder.DebitOrderCancelled)
                    debitOrder.DebitOrderCancelDate = dtpDebitOrderCancelled.Value.Date;
                else
                    debitOrder.DebitOrderCancelDate = null;
                context.SaveChanges();
                btnUpload.Visible = true;
                LoadArchiveGrid(context);
                Controller.ShowMessage("Debit order detail saved. Please upload debit order form.");
            }
        }


        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (fdOpen.ShowDialog() == DialogResult.OK)
            {
                btnUpload.Enabled = false;
                try
                {
                    if (!IsValidPdf(fdOpen.FileName))
                    {
                        btnUpload.Enabled = true;
                        Controller.HandleError("Not a valid PDF");
                        return;
                    }

                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var debitOrder = context.CustomerDebitOrderSet.SingleOrDefault(a => a.BuildingId == building.ID && a.CustomerCode == customer.accNumber);
                        var archive = new CustomerDebitOrderArchive()
                        {
                            BuildingId = debitOrder.BuildingId,
                            CustomerCode = debitOrder.CustomerCode,
                            DateArchived = DateTime.Now,
                            BankId = debitOrder.BankId,
                            BranceCode = debitOrder.BranceCode,
                            AccountNumber = debitOrder.AccountNumber,
                            AccountType = debitOrder.AccountType,
                            DebitOrderCollectionDay = debitOrder.DebitOrderCollectionDay,
                            IsActive = debitOrder.IsActive,
                            LastUpdatedByUserId = Controller.user.id,
                            LastUpdateDate = DateTime.Now,
                            IsDebitOrderFeeDisabled = debitOrder.IsDebitOrderFeeDisabled,
                            DebitOrderCancelDate = debitOrder.DebitOrderCancelDate,
                            MaxDebitAmount = debitOrder.MaxDebitAmount,
                            Documents = new List<DebitOrderDocumentArchive>()
                        };
                        foreach (var doc in debitOrder.Documents)
                        {
                            archive.Documents.Add(new DebitOrderDocumentArchive()
                            {
                                CustomerDebitOrderArchive = archive,
                                FileData = doc.FileData,
                                FileName = doc.FileName
                            });
                        }
                        context.CustomerDebitOrderArchiveSet.Add(archive);

                        var signedForm = context.DebitOrderDocumentSet.SingleOrDefault(a => a.CustomerDebitOrderId == debitOrder.id && a.DocumentType == DebitOrderDocumentType.SignedDebitOrder);
                        if (signedForm == null)
                        {
                            signedForm = new DebitOrderDocument()
                            {
                                CustomerDebitOrderId = debitOrder.id,
                                DocumentType = DebitOrderDocumentType.SignedDebitOrder
                            };
                            context.DebitOrderDocumentSet.Add(signedForm);
                        }
                        signedForm.FileData = File.ReadAllBytes(fdOpen.FileName);
                        signedForm.FileName = Path.GetFileName(fdOpen.FileName);
                        context.SaveChanges();
                        DisplayPDF(signedForm.FileData);
                        LoadArchiveGrid(context);
                        MessageBox.Show("Successfully Uploaded Insurance Form");
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to upload Insurance Form");
                }
                finally
                {
                    btnUpload.Enabled = true;
                }
            }
        }

        bool _promptForDefaults = true;
        private void cbBanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customer == null)
                return;

            var bank = cbBanks.SelectedItem as Astrodon.Data.BankData.Bank;
            if (bank != null)
            {
                if (!String.IsNullOrWhiteSpace(bank.BranchCode) && txtBranchCode.Text != bank.BranchCode && _promptForDefaults)
                {
                    if (MessageBox.Show("Load default branch details?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        txtBranchCode.Text = bank.BranchCode;
                    }
                }
            }
        }

        #endregion


        private void label26_Click(object sender, EventArgs e)
        {

        }


        #region PDF Handler


        private string _TempPDFFile = string.Empty;
        private void DisplayPDF(byte[] pdfData)
        {
            if (pdfData == null)
            {
                this.axAcroPDF1.Visible = false;
                return;
            }
            if (!String.IsNullOrWhiteSpace(_TempPDFFile))
                File.Delete(_TempPDFFile);
            _TempPDFFile = Path.GetTempPath();
            if (!_TempPDFFile.EndsWith(@"\"))
                _TempPDFFile = _TempPDFFile + @"\";

            _TempPDFFile = _TempPDFFile + System.Guid.NewGuid().ToString("N") + ".pdf";
            File.WriteAllBytes(_TempPDFFile, pdfData);


            try
            {
                this.axAcroPDF1.Visible = true;
                this.axAcroPDF1.LoadFile(_TempPDFFile);
                this.axAcroPDF1.src = _TempPDFFile;
                this.axAcroPDF1.setShowToolbar(false);
                this.axAcroPDF1.setView("FitH");
                this.axAcroPDF1.setLayoutMode("SinglePage");
                this.axAcroPDF1.setShowToolbar(false);

                this.axAcroPDF1.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            File.Delete(_TempPDFFile);
        }

        private bool IsValidPdf(string filepath)
        {
            bool Ret = true;

            PdfReader reader = null;

            try
            {
                using (reader = new PdfReader(filepath))
                {
                    reader.Close();
                }
            }
            catch
            {
                Ret = false;
            }

            return Ret;
        }



        #endregion

        private void cbDebitOrderCancelled_CheckedChanged(object sender, EventArgs e)
        {
            dtpDebitOrderCancelled.Visible = cbDebitOrderCancelled.Checked;
         
        }

        private void UpdateTrusteeTick()
        {
            if (building == null)
                return;
            if (customer == null)
                return;

            using (var context = SqlDataHandler.GetDataContext())
            {
                var customerEntity = context.CustomerSet.SingleOrDefault(a => a.BuildingId == building.ID && a.AccountNumber == customer.accNumber);
                if (customerEntity == null)
                {
                    customerEntity = new Data.CustomerData.Customer()
                    {
                        BuildingId = building.ID,
                        AccountNumber = customer.accNumber,
                        Description = customer.description,
                        IsTrustee = customer.IsTrustee,
                        Created = DateTime.Now,
                        CellNumber = customer.CellPhone
                    };
                    context.CustomerSet.Add(customerEntity);
                }
                customerEntity.IsTrustee = cbTrustee.Checked;
                customerEntity.Description = customer.description;
                context.SaveChanges();
            }
        }

        private void UpdateSQLCustomers()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var customerEntities = context.CustomerSet.Where(a => a.BuildingId == building.ID).ToList();

                foreach(var cust in customers)
                {
                    var customerEntity = customerEntities.Where(a => a.AccountNumber == cust.accNumber).SingleOrDefault();
                    if(customerEntity == null)
                    {
                        customerEntity = new Data.CustomerData.Customer()
                        {
                            BuildingId = building.ID,
                            AccountNumber = cust.accNumber,
                            Description = cust.description,
                            IsTrustee = cust.IsTrustee,
                            Created = DateTime.Now,
                            CellNumber = cust.CellPhone
                        };
                        context.CustomerSet.Add(customerEntity);
                    }
                    if (customerEntity.Description != cust.description)
                        customerEntity.Description = cust.description;
                    if (customerEntity.CellNumber != cust.CellPhone)
                        customerEntity.CellNumber = cust.CellPhone;
                }
                context.SaveChanges();
            }
        }


        private void LoadCustomerEntity()
        {
            cbBirthDayNotificaiton.Checked = false;
            tbRSAIDNumber.Text = "";
            dtpDateOfBirth.Value = new DateTime(1900, 01, 01);
            dtpDateOfBirth.MaxDate = DateTime.Today;
            dtpDateOfBirth.MinDate = DateTime.Today.AddYears(-150);
            tbFullName.Text = "";
            using (var context = SqlDataHandler.GetDataContext())
            {
                var customerEntity = context.CustomerSet.SingleOrDefault(a => a.BuildingId == building.ID && a.AccountNumber == customer.accNumber);
                if (customerEntity == null)
                {
                    customerEntity = new Data.CustomerData.Customer()
                    {
                        BuildingId = building.ID,
                        AccountNumber = customer.accNumber,
                        Description = customer.description,
                        IsTrustee = customer.IsTrustee,
                        Created = DateTime.Now,
                        SendBirthdayNotification = false,
                        IDNumber = string.Empty,
                        DateOfBirth = null,
                        CellNumber = customer.CellPhone
                    };
                    context.CustomerSet.Add(customerEntity);
                    context.SaveChanges();
                }
                cbBirthDayNotificaiton.Checked = customerEntity.SendBirthdayNotification;

                if (!String.IsNullOrWhiteSpace(customerEntity.IDNumber))
                    tbRSAIDNumber.Text = customerEntity.IDNumber;

                if (customerEntity.DateOfBirth != null)
                    dtpDateOfBirth.Value = customerEntity.DateOfBirth.Value;

                if (!String.IsNullOrWhiteSpace(customerEntity.CustomerFullName))
                    tbFullName.Text = customerEntity.CustomerFullName;

            }
        }

        private void btnSaveBirthDay_Click(object sender, EventArgs e)
        {
            if (dtpDateOfBirth.Value < DateTime.Today.AddYears(-140))
                cbBirthDayNotificaiton.Checked = false;
            if(String.IsNullOrWhiteSpace(tbFullName.Text))
            {
                Controller.HandleError("Full Name is required for Birthday SMS", "Validation Error");
                return;
            }
            using (var context = SqlDataHandler.GetDataContext())
            {
                var customerEntity = context.CustomerSet.SingleOrDefault(a => a.BuildingId == building.ID && a.AccountNumber == customer.accNumber);
                if (customerEntity == null)
                {
                    customerEntity = new Data.CustomerData.Customer()
                    {
                        BuildingId = building.ID,
                        AccountNumber = customer.accNumber,
                        Description = customer.description,
                        IsTrustee = customer.IsTrustee,
                        Created = DateTime.Now,
                        CellNumber = customer.CellPhone

                    };
                    context.CustomerSet.Add(customerEntity);
                }

                customerEntity.DateOfBirth = dtpDateOfBirth.Value;
                customerEntity.IDNumber = tbRSAIDNumber.Text;
                customerEntity.SendBirthdayNotification = cbBirthDayNotificaiton.Checked;
                customerEntity.CellNumber = customer.CellPhone;
                customerEntity.CustomerFullName = tbFullName.Text;
                context.SaveChanges();

                Controller.ShowMessage("Birthday details updated");
            }
        }

        private void tbRSAIDNumber_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbRSAIDNumber.Text) && tbRSAIDNumber.Text.Length >= 13)
            {
                var idval = new IDValidator(tbRSAIDNumber.Text);
                if (idval.isValid())
                {
                    dtpDateOfBirth.Value = idval.GetDateOfBirthAsDateTime();
                    cbBirthDayNotificaiton.Checked = true;
                }
                else
                    Controller.HandleError(tbRSAIDNumber.Text + " is not a valid RSA ID Number");
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if ( e.RowIndex >= 0)
            {
                var selectedItem = senderGrid.Rows[e.RowIndex].DataBoundItem as Reminder;
                if (selectedItem != null)
                {
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var obj = context.tblReminders.Where(a => a.id == selectedItem.id).FirstOrDefault();

                        obj.action = selectedItem.action;
                        if (selectedItem.action)
                            obj.actionDate = DateTime.Now;
                        else
                            obj.actionDate = null;
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}

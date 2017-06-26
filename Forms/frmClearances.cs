using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class frmClearances : Form
    {
        private SqlDataHandler dh = new SqlDataHandler();
        private Building build;
        private Customer customer;
        private double os = 0;
        private double clrFee = 0;
        private double clrTotal = 0;
        private List<Customer> customers;
        private ClearanceValues values = new ClearanceValues();
        private List<Building> buildings;
        private String bcode, ccode, preparedBy, trfAttorneys, attReference, fax, complex, unitNo, seller, purchaser, purchaserAddress, purchaserTel, purchaserEmail, notes;
        private double clearanceFee, astrodonTotal;
        private bool registered;
        private DateTime certDate, validDate;
        private DateTime? regDate;
        private List<Trns> transactions = new List<Trns>();
        private BindingSource bs = new BindingSource();
        private int id;
        private List<ClearanceTransactions> clrTrans = new List<ClearanceTransactions>();

        public frmClearances(int clearanceID)
        {
            id = clearanceID;
            InitializeComponent();
            bcode = String.Empty;
            ccode = String.Empty;
            buildings = new Buildings(false).buildings;
        }

        private void frmClearances_Load(object sender, EventArgs e)
        {
            this.tblBuildingsTableAdapter.Fill(this.astrodonDataSet1.tblBuildings);
            LoadExtraBuilding();
            if (id != 0)
            {
                LoadClearance();
                cmbBuilding.SelectedValue = bcode;
                cmbCustomer.SelectedValue = ccode;
                txtPrepared.Text = preparedBy;
                txtTrfAttorneys.Text = trfAttorneys;
                txtYourRef.Text = attReference;
                txtFaxNumber.Text = fax;
                txtComplex.Text = complex;
                txtUnit.Text = unitNo;
                txtSeller.Text = seller;
                txtPurchaser.Text = purchaser;
                txtAddPurchaser.Text = purchaserAddress;
                txtTelPurchaser.Text = purchaserTel;
                txtEmailPurchaser.Text = purchaserEmail;
                txtNotes.Text = notes;
                dgClearance.DataSource = null;
                bs.DataSource = clrTrans;
                dgClearance.DataSource = bs;
                txtClearance.Text = clearanceFee.ToString("#,##0.00");
                //txtTotal.Text = astrodonTotal.ToString("#,##0.00");
                CalcTotals();
                dtPicker.Value = certDate;
                dtReg.Value = (regDate.GetValueOrDefault().Equals(null) ? DateTime.Now : regDate.GetValueOrDefault());
                chkRegDate.Checked = registered;
                dtValid.Value = validDate;
            }
            else
            {
                cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
                cmbBuilding.SelectedIndex = -1;
                cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
                clrFee = values.clearanceFee;
                txtClearance.Text = clrFee.ToString("#,##0.00");
                txtSplit.Text = values.splitFee.ToString("#,##0.00");
                CalcTotals();
            }
        }

        private void LoadExtraBuilding()
        {
            Astrodon.DataSets.AstrodonDataSet1.tblBuildingsRow dr = astrodonDataSet1.tblBuildings.NewtblBuildingsRow();
            dr.Code = "0";
            dr.Building = "Please select";
            this.astrodonDataSet1.tblBuildings.Rows.InsertAt(dr, 0);
            cmbBuilding.SelectedIndex = 0;
        }

        private void LoadClearance()
        {
            String status = String.Empty;
            String query = "SELECT id, buildingCode, customerCode, preparedBy, trfAttorneys, attReference, fax, certDate, complex, unitNo, seller, purchaser, purchaserAddress, purchaserTel, purchaserEmail, ";
            query += " regDate, registered, notes, clearanceFee, astrodonTotal, validDate, processed FROM tblClearances WHERE (id = " + id + ")";
            DataSet ds = dh.GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bcode = ds.Tables[0].Rows[0]["buildingCode"].ToString();
                ccode = ds.Tables[0].Rows[0]["customerCode"].ToString();
                preparedBy = ds.Tables[0].Rows[0]["preparedBy"].ToString();
                trfAttorneys = ds.Tables[0].Rows[0]["trfAttorneys"].ToString();
                attReference = ds.Tables[0].Rows[0]["attReference"].ToString();
                fax = ds.Tables[0].Rows[0]["fax"].ToString();
                complex = ds.Tables[0].Rows[0]["complex"].ToString();
                unitNo = ds.Tables[0].Rows[0]["unitNo"].ToString();
                seller = ds.Tables[0].Rows[0]["seller"].ToString();
                purchaser = ds.Tables[0].Rows[0]["purchaser"].ToString();
                purchaserAddress = ds.Tables[0].Rows[0]["purchaserAddress"].ToString();
                purchaserTel = ds.Tables[0].Rows[0]["purchaserTel"].ToString();
                purchaserEmail = ds.Tables[0].Rows[0]["purchaserEmail"].ToString();
                notes = ds.Tables[0].Rows[0]["notes"].ToString();
                clearanceFee = double.Parse(ds.Tables[0].Rows[0]["clearanceFee"].ToString());
                clrFee = clearanceFee;
                astrodonTotal = double.Parse(ds.Tables[0].Rows[0]["astrodonTotal"].ToString());
                certDate = DateTime.Parse(ds.Tables[0].Rows[0]["certDate"].ToString());
                regDate = DateTime.Parse(ds.Tables[0].Rows[0]["regDate"].ToString());
                registered = bool.Parse(ds.Tables[0].Rows[0]["registered"].ToString());
                validDate = DateTime.Parse(ds.Tables[0].Rows[0]["validDate"].ToString());
                dtValid.Value = validDate;
            }
            query = "SELECT description, qty, rate, markup, amount FROM tblClearanceTransactions WHERE clearanceID = " + id.ToString();
            DataSet ds2 = dh.GetData(query, null, out status);
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                clrTrans = new List<ClearanceTransactions>();
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    ClearanceTransactions clrT = new ClearanceTransactions
                    {
                        Description = dr["description"].ToString(),
                        Qty = double.Parse(dr["qty"].ToString()),
                        Rate = double.Parse(dr["rate"].ToString()),
                        Markup_Percentage = double.Parse(dr["markup"].ToString()),
                        Amount = double.Parse(dr["amount"].ToString())
                    };
                    if (clrT.Description == "Recon split Seller/Buyer date reconciliation")
                    {
                        txtSplit.Text = clrT.Amount.ToString("#,##0.00");
                    }
                    else
                    {
                        clrTrans.Add(clrT);
                    }
                }
            }
            else
            {
                MessageBox.Show(query);
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBuilding.SelectedItem != null && cmbBuilding.SelectedValue.ToString() != "0")
                {
                    String code = cmbBuilding.SelectedValue.ToString();
                    if (buildings == null) { buildings = new Buildings(false).buildings; }
                    foreach (Building b in buildings)
                    {
                        if (b.Abbr == code)
                        {
                            build = b;
                            break;
                        }
                    }
                    txtComplex.Text = build.Name;
                    customers = Controller.pastel.AddCustomers(build.Abbr, build.DataPath);
                    if (build != null && customers.Count > 0)
                    {
                        cmbCustomer.DataSource = customers;
                        cmbCustomer.DisplayMember = "accNumber";
                        cmbCustomer.ValueMember = "accNumber";
                        if (id == 0)
                        {
                            clrTrans = new List<ClearanceTransactions>();
                            dgClearance.DataSource = null;
                            bs.DataSource = clrTrans;
                            dgClearance.DataSource = bs;
                        }
                        dgTransactions.DataSource = null;
                        transactions = new List<Trns>();
                        dgTransactions.DataSource = transactions;
                    }
                }
            }
            catch { }
        }

        public int getPeriod(DateTime trnDate, int buildPeriod)
        {
            int myMonth = trnDate.Month;
            myMonth = myMonth - 2;
            myMonth = (myMonth < 1 ? myMonth + 12 : myMonth);
            buildPeriod = (myMonth - buildPeriod < 1 ? myMonth - buildPeriod + 12 : myMonth - buildPeriod);
            return buildPeriod;
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                customer = null;
                if (cmbCustomer.SelectedItem != null) { customer = customers[cmbCustomer.SelectedIndex]; }
                if (customer != null && cmbCustomer.SelectedIndex >= 0) { LoadCustomer(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCustomer()
        {
            if (customer != null)
            {
                transactions = new List<Trns>();
                String adjUnit = "Unit " + customer.accNumber.Replace(build.Abbr, "");
                txtUnit.Text = adjUnit;
                txtSeller.Text = customer.description;
                os = 0;

                DateTime trnDate = dtPicker.Value.AddMonths(-3);
                validDate = dtValid.Value;
                DateTime balDate = trnDate;
                //extract clearance fee from database

                if (customer != null)
                {
                    double totalDue = 0;
                    String trnMsg = "";
                    List<Transaction> transacts = (new Classes.LoadTrans()).LoadTransactions(build, customer, DateTime.Now, out totalDue, out trnMsg);
                    transactions = new List<Trns>();
                    foreach (Transaction t in transacts)
                    {
                        Trns tr = new Trns();
                        tr.Amount = t.TrnAmt.ToString();
                        tr.Date = t.TrnDate.ToString("yyyy/MM/dd");
                        tr.Description = t.Description;
                        tr.Reference = t.Reference;
                        transactions.Add(tr);
                    }
                    if (id == 0)
                    {
                        clrTrans = new List<ClearanceTransactions>();
                        dgClearance.DataSource = null;
                        ClearanceTransactions clrTrn = new ClearanceTransactions
                        {
                            Description = String.Format("OUTSTANDING BALANCE AT {0}", dtPicker.Value.ToString("yyyy/MM/dd")),
                            Qty = 1,
                            Rate = Math.Round(totalDue, 2),
                            Markup_Percentage = 0
                        };
                        clrTrans.Add(clrTrn);
                        bs.DataSource = clrTrans;
                        dgClearance.DataSource = bs;
                    }
                }
                dgTransactions.DataSource = null;
                dgTransactions.DataSource = transactions;
            }
        }

        private void dgTransactions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //get the current column details
            string strColumnName = dgTransactions.Columns[e.ColumnIndex].Name;
            SortOrder strSortOrder = getSortOrder(e.ColumnIndex);
            transactions.Sort(new TransComparer(strColumnName, strSortOrder));
            dgTransactions.DataSource = null;
            dgTransactions.DataSource = transactions;
            dgTransactions.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
        }

        private SortOrder getSortOrder(int columnIndex)
        {
            if (dgTransactions.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                dgTransactions.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending)
            {
                dgTransactions.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                return SortOrder.Ascending;
            }
            else
            {
                dgTransactions.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                return SortOrder.Descending;
            }
        }

        private void dgClearance_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CalcTotals();
        }

        private void dgClearance_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalcTotals();
        }

        private void dgClearance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //1 * 2 * (1 + (3 / 100)) = 4
            DataGridViewRow dvr = dgClearance.Rows[e.RowIndex];
            int colIdx = e.ColumnIndex;
            if (colIdx == 1 || colIdx == 2 || colIdx == 3)
            {
                double qty = double.Parse(dvr.Cells[1].Value.ToString());
                double rate = double.Parse(dvr.Cells[2].Value.ToString());
                double mu = double.Parse(dvr.Cells[3].Value.ToString());

                double lTotal = qty * rate * (1 + (mu / 100));
                dvr.Cells[4].Value = lTotal;
                CalcTotals();
            }
        }

        private void CalcTotals()
        {
            double total = 0;
            try
            {
                if (dgClearance.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dvr in dgClearance.Rows)
                    {
                        double value = (double.TryParse(dvr.Cells[4].Value.ToString(), out value) ? value : 0);
                        total += value;
                    }
                }
            }
            catch { }
            double splitFee = (double.TryParse(txtSplit.Text, out splitFee) ? splitFee : 0);
            clrTotal = total + clrFee + splitFee;
            txtTotal.Text = clrTotal.ToString("#,##0.00");
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            SaveClearance(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveClearance(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveClearance(bool process)
        {
            if (customer != null)
            {
                String query = "";

                String clearOldClearances = "";

                if (id == 0)
                {
                    clearOldClearances = "DELETE FROM tblClearances WHERE buildingCode = @buildingCode AND customerCode = @customerCode";
                    query = "INSERT INTO tblClearances(buildingCode, customerCode, preparedBy, trfAttorneys, attReference, fax, certDate, complex, unitNo, seller, purchaser, purchaserAddress, purchaserTel, ";
                    query += " purchaserEmail, regDate, notes, clearanceFee, astrodonTotal, validDate, processed, registered, extClearance)";
                    query += " VALUES(@buildingCode, @customerCode, @preparedBy, @trfAttorneys, @attReference, @fax, @certDate, @complex, @unitNo, @seller, @purchaser, @purchaserAddress, @purchaserTel, ";
                    query += " @purchaserEmail, @regDate, @notes, @clearanceFee, @astrodonTotal, @validDate, @processed, @registered, @extClearance)";
                }
                else
                {
                    query = "UPDATE tblClearances SET buildingCode = @buildingCode, customerCode = @customerCode, preparedBy = @preparedBy, trfAttorneys = @trfAttorneys, attReference = @attReference, fax = @fax, ";
                    query += " certDate = @certDate, complex = @complex, unitNo = @unitNo, seller = @seller, purchaser = @purchaser, purchaserAddress = @purchaserAddress, purchaserTel = @purchaserTel, ";
                    query += " purchaserEmail = @purchaserEmail, regDate = @regDate, notes = @notes, clearanceFee = @clearanceFee, astrodonTotal = @astrodonTotal, validDate = @validDate, ";
                    query += " processed = @processed, registered = @registered, extClearance = @extClearance";
                    query += " WHERE id = @id";
                }
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                sqlParms.Add("@buildingCode", build.Abbr);
                sqlParms.Add("@customerCode", customer.accNumber);
                sqlParms.Add("@preparedBy", txtPrepared.Text);
                sqlParms.Add("@trfAttorneys", txtTrfAttorneys.Text);
                sqlParms.Add("@attReference", txtYourRef.Text);
                sqlParms.Add("@fax", txtFaxNumber.Text);
                sqlParms.Add("@certDate", dtPicker.Value);
                sqlParms.Add("@complex", txtComplex.Text);
                sqlParms.Add("@unitNo", txtUnit.Text);
                sqlParms.Add("@seller", txtSeller.Text);
                sqlParms.Add("@purchaser", txtPurchaser.Text);
                sqlParms.Add("@purchaserAddress", txtAddPurchaser.Text);
                sqlParms.Add("@purchaserTel", txtTelPurchaser.Text);
                sqlParms.Add("@purchaserEmail", txtEmailPurchaser.Text);
                sqlParms.Add("@regDate", dtReg.Value);
                sqlParms.Add("@registered", chkRegDate.Checked);
                sqlParms.Add("@notes", txtNotes.Text);
                sqlParms.Add("@clearanceFee", double.Parse(txtClearance.Text));
                sqlParms.Add("@astrodonTotal", double.Parse(txtTotal.Text));
                sqlParms.Add("@validDate", dtValid.Value);
                sqlParms.Add("@processed", process);
                sqlParms.Add("@id", id);
                sqlParms.Add("@extClearance", chkExClearance.Checked);
                String status = String.Empty;
                if (!String.IsNullOrEmpty(clearOldClearances))
                {
                    dh.SetData(clearOldClearances, sqlParms, out status);
                }

                dh.SetData(query, sqlParms, out status);
                if (id == 0) { id = GetClearanceID(); }

                String transQuery = "DELETE FROM tblClearanceTransactions WHERE clearanceID NOT IN (SELECT id FROM tblClearances) OR id = " + id.ToString();
                dh.SetData(transQuery, null, out status);
                //MessageBox.Show("CLID = " + id.ToString());
                transQuery = "INSERT INTO tblClearanceTransactions(clearanceID, description, qty, rate, markup, amount) VALUES(@clearanceID, @description, @qty, @rate, @markup, @amount)";
                Dictionary<String, Object> sqlParms2 = new Dictionary<string, object>();
                sqlParms2.Add("@clearanceID", id);
                sqlParms2.Add("@description", "");
                sqlParms2.Add("@amount", 0);
                if (!String.IsNullOrEmpty(txtSplit.Text))
                {
                    double splitFee = (double.TryParse(txtSplit.Text, out splitFee) ? splitFee : 0);
                    if (splitFee > 0)
                    {
                        ClearanceTransactions trn = new ClearanceTransactions
                        {
                            Amount = splitFee,
                            Description = "Recon split Seller/Buyer date reconciliation",
                            Markup_Percentage = 0,
                            Qty = 1,
                            Rate = splitFee
                        };
                        clrTrans.Add(trn);
                    }
                }
                foreach (ClearanceTransactions clrT in clrTrans)
                {
                    //MessageBox.Show(clrT.Description);
                    if (clrT.Description != "")
                    {
                        sqlParms2["@description"] = clrT.Description;
                        sqlParms2["@qty"] = clrT.Qty;
                        sqlParms2["@rate"] = clrT.Rate;
                        sqlParms2["@markup"] = clrT.Markup_Percentage;
                        sqlParms2["@amount"] = clrT.Amount;
                        dh.SetData(transQuery, sqlParms2, out status);
                        if (status != "") { MessageBox.Show(status); }
                    }
                }
                if (id != 0 && process)
                {
                    PDF pdf = new PDF();
                    String fileName = String.Empty;
                    if (pdf.Create(id, out fileName) && !String.IsNullOrEmpty(Controller.user.email))
                    {
                        String fromAddress = Controller.user.email;
                        String toAddress = Controller.user.email;
                        if (!Mailer.SendMail(fromAddress, new String[] { toAddress }, "Clearance Certificate", "Please find attached clearance certificate", false, false, false, out status, new String[] { fileName }))
                        {
                            MessageBox.Show(status);
                        }
                        if (Environment.MachineName != "STEPHEN-PC") { ProcessJournals(id); }
                        MessageBox.Show("Clearance processed and certificate sent!");
                    }
                }
                else if (id != 0)
                {
                    MessageBox.Show("Clearance saved!");
                }
                this.Close();
            }
        }

        private void ProcessJournals(int clearanceID)
        {
            ClearanceObject clr = GetClearance(clearanceID);
            if (clr == null)
            {
                MessageBox.Show("clearanceID = " + clearanceID.ToString());
                return;
            }
            //Recon split Seller/Buyer date reconciliation
            bool hasSplit = false;
            String splitDesc = String.Empty;
            double splitFee = 0;
            try
            {
                foreach (ClearanceObjectTrans clrT in clr.Trans)
                {
                    if (clrT.description == "Recon split Seller/Buyer date reconciliation")
                    {
                        hasSplit = true;
                        splitDesc = "Recon split Seller/Buyer date reconciliation";
                        splitFee = clrT.amount;
                        break;
                    }
                }
            }
            catch
            {
                MessageBox.Show("clearanceID = " + clearanceID.ToString());
            }
            String docType = "Clearance " + clr.validTo.ToString("yyyy/MM/dd");
            Building building = null;
            foreach (Building b in buildings)
            {
                if (b.Abbr == clr.buildingCode)
                {
                    building = b;
                    break;
                }
            }
            if (building != null)
            {
                DateTime trnDate = clr.certDate;
                String pastelReturn, pastelString;
                try
                {
                    pastelReturn = Controller.pastel.PostBatch(trnDate, building.Period, values.centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, clr.customerCode, building.Centrec_Building, building.Centrec_Building, docType, docType, clr.clearanceFee.ToString(), "5500/000", "", out pastelString);
                    Controller.pastel.PostBusGBatch(trnDate, 5, "5500000", clr.customerCode, docType, docType, clr.clearanceFee.ToString("#0.00"));
                }
                catch
                {
                    MessageBox.Show("Invalid fee post (standard)");
                }
                if (hasSplit && !String.IsNullOrEmpty(splitDesc) && splitFee != 0)
                {
                    try
                    {
                        pastelReturn = Controller.pastel.PostBatch(trnDate, building.Period, values.centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, clr.customerCode, building.Centrec_Building, building.Centrec_Building, splitDesc, splitDesc, splitFee.ToString(), "5500/000", "", out pastelString);
                        Controller.pastel.PostBusGBatch(trnDate, 5, "5500000", clr.customerCode, splitDesc, splitDesc, splitFee.ToString("#0.00"));
                    }
                    catch
                    {
                        MessageBox.Show("Invalid fee post (split)");
                    }
                }
                else if (hasSplit && (String.IsNullOrEmpty(splitDesc) || splitFee != 0))
                {
                    MessageBox.Show("Invalid split description or value");
                }
            }
        }

        private ClearanceObject GetClearance(int id)
        {
            String query1 = "SELECT id, buildingCode, customerCode, preparedBy, trfAttorneys, attReference, fax, certDate, complex, unitNo, seller, purchaser, purchaserAddress, purchaserTel, purchaserEmail, ";
            query1 += " regDate, notes, clearanceFee, astrodonTotal, validDate, processed, registered, extClearance FROM tblClearances WHERE (id = " + id.ToString() + ")";
            String status = String.Empty;
            DataSet ds = dh.GetData(query1, null, out status);
            DataRow dr = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dr = ds.Tables[0].Rows[0];
            }
            String query2 = "SELECT description, amount FROM tblClearanceTransactions WHERE clearanceID = " + id.ToString();
            DataSet ds2 = dh.GetData(query2, null, out status);
            if (dr != null)
            {
                return new ClearanceObject(id, dr, ds2);
            }
            else
            {
                return null;
            }
        }

        private int GetClearanceID()
        {
            String query = "SELECT top(1) id FROM tblClearances WHERE customerCode = '" + (customer != null ? customer.accNumber : "ABC001") + "' ORDER BY id desc";
            //String query = "SELECT top(1) id FROM tblClearances WHERE customerCode = 'ABC001' ORDER BY id desc";
            String status = String.Empty;
            DataSet ds = dh.GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
            else
            {
                return 0;
            }
        }

        private void chkRegDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRegDate.Checked)
            {
                dtReg.Enabled = false;
            }
            else
            {
                dtReg.Enabled = true;
            }
        }

        private void chkExClearance_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExClearance.Checked)
            {
                clrFee = values.exClearanceFee;
            }
            else
            {
                clrFee = values.clearanceFee;
            }
            txtClearance.Text = clrFee.ToString("#,##0.00");
            CalcTotals();
        }

        private void txtClearance_TextChanged(object sender, EventArgs e)
        {
            CalcTotals();
        }

        private void txtSplit_TextChanged(object sender, EventArgs e)
        {
            CalcTotals();
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
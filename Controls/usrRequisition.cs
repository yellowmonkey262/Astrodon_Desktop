using Astrodon.Data;
using Astrodon.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Data.Entity;
using Astro.Library.Entities;
using Astrodon.Classes;

namespace Astrodon.Controls
{

    public partial class usrRequisition : UserControl
    {
        private List<Building> _AllBuildings;
        private List<Building> _MyBuildings = new List<Building>();
        private BindingList<RequisitionList> unProcessedRequisitions = new BindingList<RequisitionList>();
        private BindingList<RequisitionList> unPaidRequisitions = new BindingList<RequisitionList>();
        private BindingList<RequisitionList> paidRequisitions = new BindingList<RequisitionList>();
        private Data.SupplierData.Supplier _Supplier;

        private SqlDataHandler dh = new SqlDataHandler();
        private Dictionary<String, double> avAmts = new Dictionary<string, double>();
        private String status;

        public usrRequisition()
        {
            InitializeComponent();

            dtInvoiceDate.MaxDate = DateTime.Today.AddDays(7);
        }

        private void usrRequisition_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            dgUnprocessed.DataSource = unProcessedRequisitions;
            dgUnprocessed.AutoGenerateColumns = false;

            dgUnpaid.DataSource = unPaidRequisitions;
            dgUnpaid.AutoGenerateColumns = false;

            dgPaid.DataSource = paidRequisitions;
            dgUnpaid.AutoGenerateColumns = false;

            cmbRecur.SelectedIndex = 0;
        }

        private void LoadBuildings()
        {
            _AllBuildings = new Buildings(false).buildings;

            foreach (int bid in Controller.user.buildings)
            {
                foreach (Building b in _AllBuildings)
                {
                    if (bid == b.ID && b.Web_Building && !_MyBuildings.Contains(b))
                    {
                        _MyBuildings.Add(b);
                        break;
                    }
                }
            }

            _MyBuildings.Sort(new BuildingComparer("Name", SortOrder.Ascending));

            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.DataSource = _MyBuildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.SelectedItem = null;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        private void LoadRequisitions()
        {
            unProcessedRequisitions.Clear();
            unPaidRequisitions.Clear();
            paidRequisitions.Clear();
            String lineNumber = "0";

            try
            {
                if (cmbBuilding.SelectedIndex > -1)
                {
                    String buildingID = _MyBuildings[cmbBuilding.SelectedIndex].ID.ToString();
                    String unpaidQuery = "SELECT count(*) as unpaids FROM tblRequisition WHERE paid = 'False' AND building = " + buildingID;
                    DataSet unpaidDS = dh.GetData(unpaidQuery, null, out status);
                    lineNumber = "98";
                    bool hasUnpaids = false;
                    List<Trns> transactions = new List<Trns>();
                    if (unpaidDS != null && unpaidDS.Tables.Count > 0 && unpaidDS.Tables[0].Rows.Count > 0)
                    {
                        int unpaids = (int.TryParse(unpaidDS.Tables[0].Rows[0]["unpaids"].ToString(), out unpaids) ? unpaids : 0);
                        if (unpaids > 0)
                        {
                            hasUnpaids = true;
                            String path = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? GetTrustPath() : _MyBuildings[cmbBuilding.SelectedIndex].DataPath);
                            String acc = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? _MyBuildings[cmbBuilding.SelectedIndex].Trust.Replace("/", "") : _MyBuildings[cmbBuilding.SelectedIndex].OwnBank.Replace("/", ""));
                            transactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc);
                            transactions = transactions.OrderByDescending(a => a.Date).ToList();
                        }
                    }

                    lineNumber = "111";

                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var buildingId = _MyBuildings[cmbBuilding.SelectedIndex].ID;
                        var requisitions = (from r in context.tblRequisitions.Include(a => a.Supplier)
                                            where r.building == buildingId
                                            select new RequisitionList
                                            {
                                                ID = r.id.ToString(),
                                                trnDate = r.trnDate,
                                                building = r.building.ToString(),
                                                account = r.account,
                                                reference = r.reference,
                                                payreference = r.payreference,
                                                amount = (double)r.amount,
                                                ledger = r.ledger,
                                                paid = r.paid,
                                                processed = r.processed,

                                                supplierName = r.Supplier != null ? r.Supplier.CompanyName : string.Empty,
                                                supplierId = r.Supplier != null ? r.Supplier.id.ToString() : string.Empty,
                                                supplierContact = r.Supplier != null ? r.Supplier.ContactPerson : string.Empty,
                                                supplierVat = r.Supplier != null ? r.Supplier.VATNumber : string.Empty,
                                                supplierEmail = r.Supplier != null ? r.Supplier.EmailAddress : string.Empty,

                                                supplierBank = r.BankName,
                                                supplierBranchName = r.BranchName,
                                                supplierBranchCode = r.BranchCode,
                                                supplierAccountNumber = r.AccountNumber
                                            }).OrderBy(a => a.trnDate).ToList();

                        lineNumber = "117";
                        if (requisitions != null && requisitions.Count > 0)
                        {
                            foreach (RequisitionList r in requisitions)
                            {
                                //r.building = r.buildingId.ToString();
                               // r.amount = Convert.ToDouble(r.amountD);

                                bool matched = false;
                                bool paid = r.paid;
                                bool processed = r.processed;
                                String ledger = r.ledger.Split(new String[] { ":" }, StringSplitOptions.None)[0];
                                if (r.account.ToUpper() == "TRUST" && !paid)
                                {
                                    matched = GetTransactions(r.trnDate, r.amount, transactions);
                                }
                                else if (!paid)
                                {
                                    matched = GetTransactions(r.trnDate, r.amount * -1, transactions);
                                }
                                if (!processed)
                                {
                                    unProcessedRequisitions.Add(r);
                                }
                                else if (!matched && !paid)
                                {
                                    unPaidRequisitions.Add(r);
                                }
                                else
                                {
                                    String updateQuery = "UPDATE tblRequisition SET paid = 'True' WHERE id = " + r.ID;
                                    r.paid = true;
                                    paidRequisitions.Add(r);
                                    dh.SetData(updateQuery, null, out status);
                                }
                            }
                        }
                    }

                    #region Old Select Query

                    //String query = "SELECT r.id, r.trnDate, b.Building, r.account, r.processed, r.paid, r.reference, r.contractor, r.payreference, r.amount, r.ledger, b.acc, b.ownbank, b.datapath";
                    //query += " FROM tblRequisition AS r INNER JOIN tblBuildings AS b ON r.building = b.id";
                    //query += " WHERE b.id = " + buildingID + " ORDER BY trnDate";
                    //Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    //DataSet dsRequisitions = dh.GetData(query, null, out status);
                    //lineNumber = "117";
                    //if (dsRequisitions != null && dsRequisitions.Tables.Count > 0 && dsRequisitions.Tables[0].Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in dsRequisitions.Tables[0].Rows)
                    //    {
                    //        RequisitionList r = new RequisitionList();
                    //        r.ID = dr["id"].ToString();
                    //        r.trnDate = DateTime.Parse(dr["trnDate"].ToString());
                    //        r.building = dr["Building"].ToString();
                    //        r.account = dr["account"].ToString();
                    //        r.reference = dr["reference"].ToString();
                    //        r.payreference = dr["payreference"].ToString();
                    //        r.amount = double.Parse(dr["amount"].ToString());
                    //        r.ledger = dr["ledger"].ToString();
                    //        bool matched = false;
                    //        bool paid = bool.Parse(dr["paid"].ToString());
                    //        bool processed = bool.Parse(dr["processed"].ToString());
                    //        String ledger = r.ledger.Split(new String[] { ":" }, StringSplitOptions.None)[0];
                    //        if (r.account.ToUpper() == "TRUST" && !paid)
                    //        {
                    //            matched = GetTransactions(r.trnDate, r.amount, transactions);
                    //        }
                    //        else if (!paid)
                    //        {
                    //            matched = GetTransactions(r.trnDate, r.amount * -1, transactions);
                    //        }
                    //        if (!processed)
                    //        {
                    //            unProcessedRequisitions.Add(r);
                    //        }
                    //        else if (!matched && !paid)
                    //        {
                    //            unPaidRequisitions.Add(r);
                    //        }
                    //        else
                    //        {
                    //            String updateQuery = "UPDATE tblRequisition SET paid = 'True' WHERE id = " + r.ID;
                    //            paidRequisitions.Add(r);
                    //            dh.SetData(updateQuery, null, out status);
                    //        }
                    //    }
                    //}

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loading " + lineNumber + ":" + ex.Message);
            }
        }

        private bool GetTransactions(DateTime reqDate, double amt, List<Trns> transactions)
        {
            bool matched = false;
            reqDate = new DateTime(reqDate.Year, reqDate.Month, reqDate.Day, 0, 0, 0);
            //MessageBox.Show(transactions.Count.ToString());
            foreach (Trns transaction in transactions)
            {
                DateTime trnDate = DateTime.Parse(transaction.Date);
                //MessageBox.Show(amt.ToString() + " -- " + transaction.Amount);
                if (trnDate >= reqDate.AddDays(-5) && trnDate <= reqDate.AddDays(2) && double.Parse(transaction.Amount) == amt)
                {
                    matched = true;
                    break;
                }
            }
            return matched;
        }

        private double GetBalance(String datapath, String account)
        {
            String acc = Controller.pastel.GetAccount(datapath, account.Replace("/", ""));
            if (acc.StartsWith("99"))
            {
                return 0;
            }
            else
            {
                String[] accBits = acc.Split(new String[] { "|" }, StringSplitOptions.None);
                double bal = 0;
                try
                {
                    for (int i = 7; i <= 32; i++)
                    {
                        double lbal = (double.TryParse(accBits[i], out lbal) ? lbal : 0);
                        bal += lbal;
                    }
                }
                catch { }
                return bal;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            LoadRequisitions();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                double balance = GetBuildingBalance();
                foreach (DataGridViewRow dvr in dgUnprocessed.Rows)
                {
                    String message = "";
                    RequisitionList r = unProcessedRequisitions[dvr.Index];
                    String id = r.ID;
                    DateTime trnDate = r.trnDate;
                    double reqAmt = r.amount;
                    if (reqAmt <= balance && trnDate <= DateTime.Now)
                    {
                        message += "Date requested: " + r.trnDate.ToString("yyyy/MM/dd") + "." + Environment.NewLine;
                        message += "Building: " + r.building + "." + Environment.NewLine;
                        message += "From Account: " + r.account + "." + Environment.NewLine;
                        message += "ABBR / Trust Code: " + r.reference + "." + Environment.NewLine;
                        message += "Ledger Account: " + r.ledger + "." + Environment.NewLine;
                        message += "Reference: " + r.payreference + "." + Environment.NewLine;
                        message += "For amount : " + r.amount.ToString("#,##0.00") + "." + Environment.NewLine;

                        if (!String.IsNullOrWhiteSpace(r.supplierName))
                        {

                            message += Environment.NewLine;

                            message += "Supplier Details" + Environment.NewLine;
                            message += "Supplier ID: " + r.supplierId + Environment.NewLine;
                            message += "Name: " + r.supplierName + Environment.NewLine;
                            message += "VAT: " + r.supplierVat + Environment.NewLine;
                            message += "Contact Person: " + r.supplierContact + Environment.NewLine;
                            message += "Email :" + r.supplierEmail + Environment.NewLine;



                        }

                        if (!String.IsNullOrWhiteSpace(r.supplierBank))
                        {
                            message += Environment.NewLine;
                            message += "Banking Details" + Environment.NewLine;
                            message += "Bank: " + r.supplierBank + Environment.NewLine;
                            message += "Branch: " + r.supplierBranchName + Environment.NewLine;
                            message += "Branch Code: " + r.supplierBranchCode + Environment.NewLine;
                            message += "Account Number: " + r.supplierAccountNumber + Environment.NewLine;
                        }

                        message += "-----------------------" + "." + Environment.NewLine + Environment.NewLine;



                        String query = "UPDATE tblRequisition SET processed = 'True' WHERE id = " + id;
                        if (message != "")
                        {
                            String[] attachments = null;
                            String email = _MyBuildings[cmbBuilding.SelectedIndex].PM;
                            //email = "stephen@metathought.co.za";
                            Mailer.SendMail("noreply@astrodon.co.za", new string[] { email }, "Payment Requisitions", message, false, false, false, out status, attachments);
                        }
                        dh.SetData(query, null, out status);
                        balance -= reqAmt;
                    }
                    else if (reqAmt > balance)
                    {
                        MessageBox.Show("Insufficient funds for ID " + id);
                    }
                }
                ResetRequisitions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Cursor = Cursors.Default;
        }

        private String GetTrustPath()
        {
            String query = "SELECT trust FROM tblSettings";
            String status;
            DataSet ds = (new SqlDataHandler()).GetData(query, null, out status);
            String trustPath = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                trustPath = ds.Tables[0].Rows[0]["trust"].ToString();
            }
            else
            {
                trustPath = String.Empty;
            }
            return trustPath;
        }

        private double GetEFTFee()
        {
            DataSet dsEFT = dh.GetData("SELECT DebitOrder FROM tblBankCharges", null, out status);
            String amt = dsEFT.Tables[0].Rows[0]["DebitOrder"].ToString();
            return double.Parse(amt);
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSupplierLookup.Enabled = true;
            cmbAccount.Items.Clear();
            cmbLedger.Items.Clear();

            try
            {
                lblBank.Text = _MyBuildings[cmbBuilding.SelectedIndex].Bank.ToUpper();
                cmbAccount.Items.Add("TRUST");
                cmbAccount.Items.Add("OWN");
                cmbAccount.SelectedItem = lblBank.Text;
            }
            catch { }

            LoadRequisitions();

            try
            {
                Dictionary<String, String> accounts = Controller.pastel.GetAccountList(_MyBuildings[cmbBuilding.SelectedIndex].DataPath);
                foreach (KeyValuePair<String, String> reqAcc in accounts)
                {
                    cmbLedger.Items.Add(reqAcc.Key + ": " + reqAcc.Value);
                }
            }
            catch { }
        }

        private void cmbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateBalanceLabel();
            }
            catch { }
        }

        private void UpdateBalanceLabel()
        {
            double balance = GetBuildingBalance();
            balance -= getOutstandingAmt();
            lblBalance.Text = balance.ToString("#,##0.00");
            lblBalance.Refresh();
            double requestedAmt = double.TryParse(txtAmount.Text, out requestedAmt) ? requestedAmt : 0;
            lblAvAmt.Text = (double.Parse(lblBalance.Text) - requestedAmt - (requestedAmt > 0 ? GetEFTFee() : 0)).ToString("#,##0.00");
            lblAvAmt.Refresh();
        }

        private double GetBuildingBalance()
        {
            try
            {
                if (cmbAccount.SelectedItem != null)
                {
                    if (cmbAccount.SelectedItem.ToString() == "TRUST")
                    {
                        return (!String.IsNullOrEmpty(GetTrustPath()) ? GetBalance(GetTrustPath(), _MyBuildings[cmbBuilding.SelectedIndex].Trust) : 0) * -1;
                    }
                    else
                    {
                        return GetBalance(_MyBuildings[cmbBuilding.SelectedIndex].DataPath, _MyBuildings[cmbBuilding.SelectedIndex].OwnBank);
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("b count = " + _MyBuildings.Count.ToString() + " b idx = " + cmbBuilding.SelectedIndex);
                return 0;
            }
        }

        private double getOutstandingAmt()
        {
            double os = 0;
            foreach (RequisitionList r in unProcessedRequisitions)
            {
                os += r.amount;
            }
            foreach (RequisitionList r in unPaidRequisitions)
            {
                os += r.amount;
            }

            return os;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double amt;

            if (double.TryParse(txtAmount.Text, out amt) && cmbBuilding.SelectedItem != null 
                && cmbLedger.SelectedItem != null && cmbAccount.SelectedItem != null)
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var item = new tblRequisition()
                    {
                        trnDate = trnDatePicker.Value.Date,
                        account = cmbAccount.SelectedItem.ToString(),
                        reference = _MyBuildings[cmbBuilding.SelectedIndex].Abbr + (cmbAccount.SelectedItem.ToString() == "TRUST" ? " (" + _MyBuildings[cmbBuilding.SelectedIndex].Trust + ")" : ""),
                        ledger = cmbLedger.SelectedItem.ToString(),
                        amount = decimal.Parse(txtAmount.Text),
                        payreference = txtPaymentRef.Text,
                        userID = Controller.user.id,
                        building = _MyBuildings[cmbBuilding.SelectedIndex].ID,
                        SupplierId = _Supplier == null ? (int?)null : _Supplier.id,
                        InvoiceNumber = txtInvoiceNumber.Text,
                        InvoiceDate = dtInvoiceDate.Value.Date,
                        BankName = _Supplier == null ? (string)null : _Supplier.BankName,
                        BranchCode = _Supplier == null ? (string)null : _Supplier.BranceCode,
                        BranchName = _Supplier == null ? (string)null : _Supplier.BranchName,
                        AccountNumber = _Supplier == null ? (string)null : _Supplier.AccountNumber
                    };

                    context.tblRequisitions.Add(item);

                    string ledgerAccount = item.ledger;
                    
                    if(ledgerAccount.Contains(":"))
                        ledgerAccount = ledgerAccount.Split(":".ToCharArray())[0];

                    var config = (from c in context.BuildingMaintenanceConfigurationSet.Include(a => a.Building)
                                  where c.BuildingId == item.building
                                  && c.PastelAccountNumber == ledgerAccount
                                  select c).SingleOrDefault();

                    if (config != null)
                    {
                        if (item.SupplierId == null)
                        {
                            Controller.HandleError("Supplier required for Maintenance. Please select a supplier.", "Validation Error");
                            return;
                        }

                        //capture the maintenance as part of the same unit of work
                        var frmMaintenance = new frmMaintenanceDetail(context, item, config);
                        var dialogResult = frmMaintenance.ShowDialog();

                        if (dialogResult == DialogResult.OK)
                            context.SaveChanges();
                    }
                    else
                    {
                        context.SaveChanges();
                    }

                    //removed - insert handled by EF so that we can get to the PK of the record
                    //dh.SetData(query, sqlParms, out status);
                }

                if (cmbRecur.SelectedIndex > 0)
                {
                    DateTime startDate = trnDatePicker.Value;
                    List<DateTime> dates = new List<DateTime>();
                    switch (cmbRecur.SelectedIndex)
                    {
                        case 1: //weekly
                            while (startDate.AddDays(7) <= dtEndDate.Value)
                            {
                                dates.Add(startDate.AddDays(7));
                                startDate = startDate.AddDays(7);
                            }
                            break;

                        case 2: //month
                            while (startDate.AddMonths(1) <= dtEndDate.Value)
                            {
                                dates.Add(startDate.AddMonths(1));
                                startDate = startDate.AddMonths(1);
                            }
                            break;

                        case 3: //yearly
                            while (startDate.AddYears(1) <= dtEndDate.Value)
                            {
                                dates.Add(startDate.AddYears(1));
                                startDate = startDate.AddYears(1);
                            }
                            break;
                    }

                    #region Build Up SQL Query

                    String query = "INSERT INTO tblRequisition(trnDate, account, reference, payreference, ledger, amount, userID, building,SupplierId,InvoiceNumber,InvoiceDate,BankName,BranchCode,AccountNumber,BranchName)";
                    query += " VALUES(@trnDate, @account, @reference, @payment, @ledger, @amount, @userID, @building,@SupplierId,@InvoiceNumber,@InvoiceDate,@BankName,@BranchCode,@AccountNumber,@BranchName)";
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@trnDate", DateTime.Parse(trnDatePicker.Value.ToString("yyyy/MM/dd")));
                    sqlParms.Add("@account", cmbAccount.SelectedItem.ToString());
                    sqlParms.Add("@reference", _MyBuildings[cmbBuilding.SelectedIndex].Abbr + (cmbAccount.SelectedItem.ToString() == "TRUST" ? " (" + _MyBuildings[cmbBuilding.SelectedIndex].Trust + ")" : ""));
                    sqlParms.Add("@ledger", cmbLedger.SelectedItem.ToString());
                    sqlParms.Add("@amount", double.Parse(txtAmount.Text));
                    sqlParms.Add("@payment", txtPaymentRef.Text);
                    sqlParms.Add("@userID", Controller.user.id);
                    sqlParms.Add("@building", _MyBuildings[cmbBuilding.SelectedIndex].ID);
                    sqlParms.Add("@SupplierId", _Supplier == null ? (int?)null : _Supplier.id);
                    sqlParms.Add("@InvoiceNumber", txtInvoiceNumber.Text);
                    sqlParms.Add("@InvoiceDate", dtInvoiceDate.Value.ToString("yyyy/MM/dd"));
                    sqlParms.Add("@BankName", _Supplier == null ? (string)null : _Supplier.BankName);
                    sqlParms.Add("@BranchCode", _Supplier == null ? (string)null : _Supplier.BranceCode);
                    sqlParms.Add("@AccountNumber", _Supplier == null ? (string)null : _Supplier.AccountNumber);
                    sqlParms.Add("@BranchName", _Supplier == null ? (string)null : _Supplier.BranchName);

                    #endregion

                    foreach (DateTime dt in dates)
                    {
                        sqlParms["@trnDate"] = dt;
                        dh.SetData(query, sqlParms, out status);
                    }
                }

                LoadRequisitions();
                ClearRequisitions();
            }
            else
            {
                Controller.HandleError("Please enter all fields","Validation Error");
            }
        }

        private void ClearRequisitions()
        {
            trnDatePicker.Value = DateTime.Now;
            txtPaymentRef.Text = "";
            lblBalance.Text = "";
            lblAvAmt.Text = "";
            txtInvoiceNumber.Text = "";
            dtInvoiceDate.Value = DateTime.Today;

            ClearSupplier();

            txtAmount.TextChanged -= txtAmount_TextChanged;
            txtAmount.Text = "";
            txtAmount.TextChanged += txtAmount_TextChanged;
            this.Invalidate();
        }

        private void ClearSupplier()
        {
            _Supplier = null;
            lbSupplierName.Text = "";
            lbAccountNumber.Text = "";
            lbBankName.Text = "";
        }

        private void ResetRequisitions()
        {
            trnDatePicker.Value = DateTime.Now;
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.SelectedIndex = -1;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
            cmbAccount.SelectedIndexChanged -= cmbAccount_SelectedIndexChanged;
            cmbAccount.SelectedIndex = -1;
            cmbAccount.SelectedIndexChanged += cmbAccount_SelectedIndexChanged;
            txtPaymentRef.Text = "";
            lblBalance.Text = "";
            lblAvAmt.Text = "";
            txtAmount.TextChanged -= txtAmount_TextChanged;
            txtAmount.Text = "";
            txtAmount.TextChanged += txtAmount_TextChanged;
            unProcessedRequisitions.Clear();
            unPaidRequisitions.Clear();
            paidRequisitions.Clear();
            this.Invalidate();
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double requestedAmt = double.TryParse(txtAmount.Text, out requestedAmt) ? requestedAmt : 0;
                lblAvAmt.Text = (double.Parse(lblBalance.Text) - requestedAmt - (requestedAmt > 0 ? GetEFTFee() : 0)).ToString("#,##0.00");
            }
            catch
            {
                lblAvAmt.Text = string.Empty;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvr in dgUnprocessed.SelectedRows)
            {
                RequisitionList r = unProcessedRequisitions[dvr.Index];
                String query = "DELETE FROM tblRequisition WHERE id = " + r.ID;
                dh.SetData(query, null, out status);
            }
            LoadRequisitions();
            dgUnprocessed.Invalidate();
            UpdateBalanceLabel();
            foreach (DataGridViewRow dvr in dgUnprocessed.SelectedRows)
            {
                dvr.Selected = false;
            }
        }

        private void dgUnprocessed_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            double balance = GetBuildingBalance();
            try
            {
                for (int i = 0; i < unProcessedRequisitions.Count; i++)
                {
                    if (unProcessedRequisitions[i].amount > balance)
                    {
                        dgUnprocessed.Rows[i].Cells[7].Style.BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        dgUnprocessed.Rows[i].Cells[7].Style.BackColor = System.Drawing.Color.White;
                    }
                    balance -= unProcessedRequisitions[i].amount;
                    if (unProcessedRequisitions[i].trnDate > DateTime.Now)
                    {
                        dgUnprocessed.Rows[i].Cells[1].Style.BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        dgUnprocessed.Rows[i].Cells[1].Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }
            catch { }
        }

        private void cmbLedger_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String ledger = cmbLedger.SelectedItem.ToString();
                String[] ledgerBits = ledger.Split(new String[] { ":" }, StringSplitOptions.None);
            }
            catch { }
        }

        private void btnSupplierLookup_Click(object sender, EventArgs e)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var frmSupplierLookup = new frmSupplierLookup(context);

                var dialogResult = frmSupplierLookup.ShowDialog();
                var supplier = frmSupplierLookup.SelectedSupplier;

                if(dialogResult == DialogResult.OK && supplier != null)
                {
                    _Supplier = supplier;
                    lbSupplierName.Text = _Supplier.CompanyName;
                    lbBankName.Text = _Supplier.BankName + " (" + supplier.BranceCode + ")";
                    lbAccountNumber.Text = _Supplier.AccountNumber;
                    btnSave.Enabled = true;
                }
                else
                {
                    ClearSupplier();
                }
            }
        }

        private void btnViewTrans_Click(object sender, EventArgs e)
        {
            String path = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? GetTrustPath() : _MyBuildings[cmbBuilding.SelectedIndex].DataPath);
            String acc = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? _MyBuildings[cmbBuilding.SelectedIndex].Trust.Replace("/", "") : _MyBuildings[cmbBuilding.SelectedIndex].OwnBank.Replace("/", ""));
            List<Trns> transactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc).OrderByDescending(c => c.Date).ToList();
            Forms.frmReqTrans fTrans = new Forms.frmReqTrans(transactions);
            fTrans.Show();
        }

        private void dg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                RequisitionList req = (senderGrid.DataSource as BindingList<RequisitionList>)[e.RowIndex];
                String query = "DELETE FROM tblRequisition WHERE ID = " + req.ID;
                String status = "";
                dh.SetData(query, null, out status);
                LoadRequisitions();
            }
            else if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                UpdatePaidStatus(e.RowIndex);
            }
        }

        private void UpdatePaidStatus(int idx)
        {
            RequisitionList req = unPaidRequisitions[idx];
            bool paid = !req.paid;
            String query = "UPDATE tblRequisition SET paid = '" + paid.ToString() + "' WHERE ID = " + req.ID;
            MessageBox.Show(query);
            String status = "";
            dh.SetData(query, null, out status);
            LoadRequisitions();
            dgUnpaid.Invalidate();
        }

        private void dgUnpaid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
    }

    public class Requisition
    {
        public String ID { get; set; }

        public DateTime trnDate { get; set; }

        public String building { get; set; }

        public String reference { get; set; }

        public String payreference { get; set; }

        public String account { get; set; }

        public String ledger { get; set; }

        public double accBalance { get; set; }

        public double amount { get; set; }

    }

    public class RequisitionList
    {
        public string supplierName;

        public string supplierId;

        public string supplierContact;

        public string supplierVat;

        public string supplierEmail;

        public string supplierBank;

        public string supplierBranchName;

        public string supplierBranchCode;

        public string supplierAccountNumber;

        public bool paid;

        public bool processed;

        public String ID { get; set; }

        public DateTime trnDate { get; set; }

        public String building { get; set; }

        public String account { get; set; }

        public String reference { get; set; }

        public String ledger { get; set; }

        public String payreference { get; set; }

        public double amount { get; set; }
    }

    public class ReqAccount {
        public String accNumber { get; set; }

        private void dgUnpaid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
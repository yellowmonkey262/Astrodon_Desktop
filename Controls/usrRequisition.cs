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
using Astrodon.Data.MaintenanceData;
using System.IO;
using Astrodon.Data.RequisitionData;

namespace Astrodon.Controls
{

    public partial class usrRequisition : UserControl
    {
        private List<Building> _AllBuildings;
        private List<Building> myBuildings = new List<Building>();
        private BindingList<RequisitionList> unProcessedRequisitions = new BindingList<RequisitionList>();
        private BindingList<RequisitionList> unPaidRequisitions = new BindingList<RequisitionList>();
        private BindingList<RequisitionList> paidRequisitions = new BindingList<RequisitionList>();
        private Data.SupplierData.Supplier _Supplier;

        private SqlDataHandler dh = new SqlDataHandler();
        private Dictionary<String, double> avAmts = new Dictionary<string, double>();

        private List<Trns> buildTransactions = new List<Trns>();
        private String status;
        private DateTime _minDate = new DateTime(2000, 1, 1);


        public usrRequisition()
        {
            InitializeComponent();
            dtInvoiceDate.MinDate = _minDate;
            dtInvoiceDate.Value = _minDate;
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
                    if (bid == b.ID && b.Web_Building && !myBuildings.Contains(b))
                    {
                        myBuildings.Add(b);
                        break;
                    }
                }
            }

            myBuildings.Sort(new BuildingComparer("Name", SortOrder.Ascending));

            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.DataSource = myBuildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.SelectedItem = null;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        //private double LoadBuildingTransactions()
        //{
        //    String path = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? GetTrustPath() : myBuildings[cmbBuilding.SelectedIndex].DataPath);
        //    String acc = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? myBuildings[cmbBuilding.SelectedIndex].Trust.Replace("/", "") : myBuildings[cmbBuilding.SelectedIndex].OwnBank.Replace("/", ""));
        //    buildTransactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc).Where(c => Convert.ToDouble(c.Amount) < 0).OrderByDescending(c => c.Date).ToList();
        //}

        private void LoadRequisitions()
        {
            unProcessedRequisitions.Clear();
            unPaidRequisitions.Clear();
            paidRequisitions.Clear();
            _Documents.Clear();
            String lineNumber = "0";

            try
            {
                if (cmbBuilding.SelectedIndex > -1)
                {
                    String buildingID = myBuildings[cmbBuilding.SelectedIndex].ID.ToString();
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
                            String path = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? GetTrustPath() : myBuildings[cmbBuilding.SelectedIndex].DataPath);
                            String acc = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? myBuildings[cmbBuilding.SelectedIndex].Trust.Replace("/", "") : myBuildings[cmbBuilding.SelectedIndex].OwnBank.Replace("/", ""));
                            transactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc);
                            transactions = transactions.OrderByDescending(a => a.Date).ToList();
                        }
                    }

                    lineNumber = "111";

                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var buildingId = myBuildings[cmbBuilding.SelectedIndex].ID;
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
                                bool paid = r.paid;
                                bool processed = r.processed;

                                //handled by a batch processed

                                //String ledger = r.ledger.Split(new String[] { ":" }, StringSplitOptions.None)[0];
                                //if (r.account.ToUpper() == "TRUST" && !paid)
                                //{
                                //    matched = GetTransactions(r.trnDate, r.amount, transactions);
                                //}
                                //else if (!paid)
                                //{
                                //    matched = GetTransactions(r.trnDate, r.amount * -1, transactions);
                                //}
                                if (!processed)
                                {
                                    unProcessedRequisitions.Add(r);
                                }
                                else if (!paid)
                                {
                                    unPaidRequisitions.Add(r);
                                }
                              
                            }
                        }
                    }

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
                            String email = myBuildings[cmbBuilding.SelectedIndex].PM;
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
                lblBank.Text = myBuildings[cmbBuilding.SelectedIndex].Bank.ToUpper();
                cmbAccount.Items.Add("TRUST");
                cmbAccount.Items.Add("OWN");
                cmbAccount.SelectedItem = lblBank.Text;
            }
            catch { }

            LoadRequisitions();

            try
            {
                Dictionary<String, String> accounts = Controller.pastel.GetAccountList(myBuildings[cmbBuilding.SelectedIndex].DataPath);
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
                        return (!String.IsNullOrEmpty(GetTrustPath()) ? GetBalance(GetTrustPath(), myBuildings[cmbBuilding.SelectedIndex].Trust) : 0) * -1;
                    }
                    else
                    {
                        return GetBalance(myBuildings[cmbBuilding.SelectedIndex].DataPath, myBuildings[cmbBuilding.SelectedIndex].OwnBank);
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("b count = " + myBuildings.Count.ToString() + " b idx = " + cmbBuilding.SelectedIndex);
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

        private void CheckLimits(double requestedAmt, out bool d, out bool m)
        {
            double limitD = myBuildings[cmbBuilding.SelectedIndex].limitD;
            double limitM = myBuildings[cmbBuilding.SelectedIndex].limitM;
            String path = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? GetTrustPath() : myBuildings[cmbBuilding.SelectedIndex].DataPath);
            String acc = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? myBuildings[cmbBuilding.SelectedIndex].Trust.Replace("/", "") : myBuildings[cmbBuilding.SelectedIndex].OwnBank.Replace("/", ""));
            DateTime cDate = DateTime.Now;
            DateTime dsDate = new DateTime(cDate.Year, cDate.Month, cDate.Day, 0, 0, 0);
            DateTime deDate = new DateTime(cDate.Year, cDate.Month, cDate.Day, 23, 59, 59);
            DateTime sDate = new DateTime(cDate.Year, cDate.Month, 1, 0, 0, 0);
            DateTime eDate = new DateTime(cDate.Year, cDate.Month, DateTime.DaysInMonth(cDate.Year, cDate.Month), 23, 59, 59);
            List<Trns> mTransactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc).Where(c => DateTime.Parse(c.Date) >= sDate && DateTime.Parse(c.Date) <= eDate).ToList();
            double dTotal = paidRequisitions.Where(c => c.trnDate >= dsDate && c.trnDate <= deDate).ToList().Sum(c => c.amount);
            //.Sum(c=>c.amount) Controller.pastel.GetTransactions(path, "G", 101, 112, acc).Where(c => DateTime.Parse(c.Date) >= dsDate && DateTime.Parse(c.Date) <= deDate).ToList();
            double mTotal = 0;
            if (cmbAccount.SelectedItem.ToString() == "TRUST")
            {
                mTransactions = mTransactions.Where(c => double.Parse(c.Amount) > 0).ToList();
                mTotal = mTransactions.Sum(c => double.Parse(c.Amount));
            }
            else
            {
                mTransactions = mTransactions.Where(c => double.Parse(c.Amount) < 0).ToList();
                mTotal = mTransactions.Sum(c => double.Parse(c.Amount)) * -1;
            }
            double unp = unProcessedRequisitions.Sum(c => c.amount);
            m = (mTotal + unp + dTotal) + requestedAmt < limitM;
            d = dTotal + requestedAmt < limitD;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            bool dLimit, mLimit;
            CheckLimits(double.Parse(txtAmount.Text), out dLimit, out mLimit);
            bool showPassword = false;
            if (!dLimit)
            {
                if (MessageBox.Show("Daily limit exceeded. Enter password to continue?", "Requisitions", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    showPassword = true;
                }
                else
                {
                    return;
                }
            }
            else if (!mLimit)
            {
                if (MessageBox.Show("Monthly limit exceeded. Enter password to continue?", "Requisitions", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    showPassword = true;
                }
                else
                {
                    return;
                }
            }
            if (showPassword)
            {
                String password;
                using (Forms.frmPrompt prompt = new Forms.frmPrompt("Password", "Please enter password"))
                {
                    if (prompt.ShowDialog() != DialogResult.OK || prompt.fileName != "45828")
                    {
                        MessageBox.Show("Invalid password entered", "Requisitions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            decimal amt;
            if (decimal.TryParse(txtAmount.Text, out amt) && cmbBuilding.SelectedItem != null && cmbLedger.SelectedItem != null && cmbAccount.SelectedItem != null)
            {
                if (dtInvoiceDate.Value <= _minDate)
                {
                    Controller.HandleError("Invoice Date required for Maintenance. Please select a date.", "Validation Error");
                    return;
                }
                if (String.IsNullOrWhiteSpace(txtInvoiceNumber.Text))
                {
                    Controller.HandleError("Invoice Number required for Maintenance. Please supply an invoice number.", "Validation Error");
                    return;
                }

                if (_Documents.Count == 0)
                {
                    Controller.HandleError("Invoice attachment required, please upload Invoice PDF", "Validation Error");
                    return;
                }
                using (var context = SqlDataHandler.GetDataContext())
                {

                    var q = (from r in context.tblRequisitions
                             where r.SupplierId == _Supplier.id
                             && r.InvoiceNumber == txtInvoiceNumber.Text
                             && r.amount == amt
                             select r);
                    if (q.Count() > 0)
                    {
                        Controller.HandleError("Duplicate requisition detected.\n" +
                           txtInvoiceNumber.Text + " invoice has already been processed", "Validation Error");
                        return;
                    }

                    var buildingId = myBuildings[cmbBuilding.SelectedIndex].ID;

                    var bankDetails = context.SupplierBuildingSet
                                             .Include(a => a.Bank)
                                             .SingleOrDefault(a => a.BuildingId == buildingId && a.SupplierId == _Supplier.id);
                    if (bankDetails == null)
                    {
                        Controller.HandleError("Supplier banking details for this building is not configured.\n" +
                                            "Please capture bank details for this building on the suppier detail screen.", "Validation Error");
                        return;
                    }
                    var item = new tblRequisition()
                    {
                        trnDate = trnDatePicker.Value.Date,
                        account = cmbAccount.SelectedItem.ToString(),
                        reference = myBuildings[cmbBuilding.SelectedIndex].Abbr + (cmbAccount.SelectedItem.ToString() == "TRUST" ? " (" + myBuildings[cmbBuilding.SelectedIndex].Trust + ")" : ""),
                        ledger = cmbLedger.SelectedItem.ToString(),
                        amount = amt,
                        payreference = txtPaymentRef.Text,
                        userID = Controller.user.id,
                        building = buildingId,
                        SupplierId = _Supplier == null ? (int?)null : _Supplier.id,
                        InvoiceNumber = txtInvoiceNumber.Text,
                        InvoiceDate = dtInvoiceDate.Value,
                        BankName = _Supplier == null ? (string)null : bankDetails.Bank.Name,
                        BranchCode = _Supplier == null ? (string)null : bankDetails.BranceCode,
                        BranchName = _Supplier == null ? (string)null : bankDetails.BranchName,
                        AccountNumber = _Supplier == null ? (string)null : bankDetails.AccountNumber
                    };

                    context.tblRequisitions.Add(item);
                    foreach (var key in _Documents.Keys)
                    {
                        context.RequisitionDocumentSet.Add(new RequisitionDocument()
                        {
                            Requisition = item,
                            FileData = _Documents[key],
                            FileName = key,
                            IsInvoice = true
                        });
                    }

                    string ledgerAccount = item.ledger;

                    if (ledgerAccount.Contains(":"))
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

                #region Recurring
                //recurring is disabled

                //if (cmbRecur.SelectedIndex > 0)
                //{

                //    DateTime startDate = trnDatePicker.Value;
                //    List<DateTime> dates = new List<DateTime>();
                //    switch (cmbRecur.SelectedIndex)
                //    {
                //        case 1: //weekly
                //            while (startDate.AddDays(7) <= dtEndDate.Value)
                //            {
                //                dates.Add(startDate.AddDays(7));
                //                startDate = startDate.AddDays(7);
                //            }
                //            break;

                //        case 2: //month
                //            while (startDate.AddMonths(1) <= dtEndDate.Value)
                //            {
                //                dates.Add(startDate.AddMonths(1));
                //                startDate = startDate.AddMonths(1);
                //            }
                //            break;

                //        case 3: //yearly
                //            while (startDate.AddYears(1) <= dtEndDate.Value)
                //            {
                //                dates.Add(startDate.AddYears(1));
                //                startDate = startDate.AddYears(1);
                //            }
                //            break;
                //    }

                //    #region Build Up SQL Query

                //    String query = "INSERT INTO tblRequisition(trnDate, account, reference, payreference, ledger, amount, userID, building,SupplierId,InvoiceNumber,InvoiceDate,BankName,BranchCode,AccountNumber,BranchName)";
                //    query += " VALUES(@trnDate, @account, @reference, @payment, @ledger, @amount, @userID, @building,@SupplierId,@InvoiceNumber,@InvoiceDate,@BankName,@BranchCode,@AccountNumber,@BranchName)";
                //    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                //    sqlParms.Add("@trnDate", DateTime.Parse(trnDatePicker.Value.ToString("yyyy/MM/dd")));
                //    sqlParms.Add("@account", cmbAccount.SelectedItem.ToString());
                //    sqlParms.Add("@reference", _MyBuildings[cmbBuilding.SelectedIndex].Abbr + (cmbAccount.SelectedItem.ToString() == "TRUST" ? " (" + _MyBuildings[cmbBuilding.SelectedIndex].Trust + ")" : ""));
                //    sqlParms.Add("@ledger", cmbLedger.SelectedItem.ToString());
                //    sqlParms.Add("@amount", double.Parse(txtAmount.Text));
                //    sqlParms.Add("@payment", txtPaymentRef.Text);
                //    sqlParms.Add("@userID", Controller.user.id);
                //    sqlParms.Add("@building", _MyBuildings[cmbBuilding.SelectedIndex].ID);
                //    sqlParms.Add("@SupplierId", _Supplier == null ? (int?)null : _Supplier.id);
                //    sqlParms.Add("@InvoiceNumber", txtInvoiceNumber.Text);
                //    sqlParms.Add("@InvoiceDate", dtInvoiceDate.Value.ToString("yyyy/MM/dd"));

                //    sqlParms.Add("@BankName", _Supplier == null ? (string)null : _Supplier.BankName);
                //    sqlParms.Add("@BranchCode", _Supplier == null ? (string)null : _Supplier.BranceCode);
                //    sqlParms.Add("@AccountNumber", _Supplier == null ? (string)null : _Supplier.AccountNumber);
                //    sqlParms.Add("@BranchName", _Supplier == null ? (string)null : _Supplier.BranchName);

                //    #endregion

                //    foreach (DateTime dt in dates)
                //    {
                //        sqlParms["@trnDate"] = dt;
                //        dh.SetData(query, sqlParms, out status);
                //    }

                //}
                #endregion

                LoadRequisitions();
                ClearRequisitions();
                this.Cursor = Cursors.Arrow;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("Please enter all fields");
            }
        }

        private void ClearRequisitions()
        {
            trnDatePicker.Value = DateTime.Now;
            txtPaymentRef.Text = "";
            lblBalance.Text = "";
            lblAvAmt.Text = "";
            txtInvoiceNumber.Text = "";
            dtInvoiceDate.Value = _minDate;

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
            if (cmbBuilding.SelectedIndex < 0)
            {
                Controller.HandleError("Please select a building first.", "Validation Error");
                return;

            }
            var buildingId = myBuildings[cmbBuilding.SelectedIndex].ID;

            using (var context = SqlDataHandler.GetDataContext())
            {
                var frmSupplierLookup = new frmSupplierLookup(context, buildingId);

                var dialogResult = frmSupplierLookup.ShowDialog();
                var supplier = frmSupplierLookup.SelectedSupplier;

                if (dialogResult == DialogResult.OK && supplier != null)
                {
                    _Supplier = supplier;
                    lbSupplierName.Text = _Supplier.CompanyName;


                    var bankDetails = context.SupplierBuildingSet
                                             .Include(a => a.Bank)
                                             .SingleOrDefault(a => a.BuildingId == buildingId && a.SupplierId == _Supplier.id);
                    if (bankDetails == null)
                    {
                        Controller.HandleError("Supplier banking details for this building is not configured.\n" +
                                            "Please capture bank details for this building on the suppier detail screen.", "Validation Error");


                        var frmSupplierDetail = new frmSupplierDetail(context, _Supplier.id, buildingId);
                        frmSupplierDetail.ShowDialog();

                        bankDetails = context.SupplierBuildingSet
                                       .Include(a => a.Bank)
                                       .SingleOrDefault(a => a.BuildingId == buildingId && a.SupplierId == _Supplier.id);
                        if (bankDetails == null)
                        {
                            _Supplier = null;
                            return;
                        }

                    }

                    lbBankName.Text = bankDetails.Bank.Name + " (" + bankDetails.BranceCode + ")";
                    lbAccountNumber.Text = bankDetails.AccountNumber;
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
            if (cmbBuilding.SelectedIndex < 0)
            {
                Controller.HandleError("Please select a building first.", "Validation Error");
                return;

            }
            String path = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? GetTrustPath() : myBuildings[cmbBuilding.SelectedIndex].DataPath);
            String acc = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? myBuildings[cmbBuilding.SelectedIndex].Trust.Replace("/", "") : myBuildings[cmbBuilding.SelectedIndex].OwnBank.Replace("/", ""));
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

        private Dictionary<string, byte[]> _Documents = new Dictionary<string, byte[]>();

        private void btnUploadInvoice_Click(object sender, EventArgs e)
        {
            if (ofdAttachment.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < ofdAttachment.FileNames.Count(); i++)
                {
                    _Documents.Add(ofdAttachment.SafeFileNames[i], File.ReadAllBytes(ofdAttachment.FileNames[i]));
                }
            }
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

    public class ReqAccount
    {
        public String accNumber { get; set; }

        private void dgUnpaid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
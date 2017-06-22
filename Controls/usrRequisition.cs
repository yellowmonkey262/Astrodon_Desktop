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
using iTextSharp.text.pdf;

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
                        if (Controller.user.id == 15 && b.ID == 127)
                        {
                            myBuildings.Add(b);
                        }
                        else if (Controller.user.id != 15)
                        {
                            myBuildings.Add(b);
                        }
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
                        lineNumber = "99";
                        int unpaids = (int.TryParse(unpaidDS.Tables[0].Rows[0]["unpaids"].ToString(), out unpaids) ? unpaids : 0);
                        lineNumber = "100";
                        if (unpaids > 0)
                        {
                            lineNumber = "101";
                            hasUnpaids = true;
                            if (cmbAccount.SelectedItem != null)
                            {
                                String path = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? GetTrustPath() : myBuildings[cmbBuilding.SelectedIndex].DataPath);
                                lineNumber = "102";
                                String acc = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? myBuildings[cmbBuilding.SelectedIndex].Trust.Replace("/", "") : myBuildings[cmbBuilding.SelectedIndex].OwnBank.Replace("/", ""));
                                lineNumber = "103";
                                transactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc);
                                lineNumber = "104";
                                if (transactions != null)
                                    transactions = transactions.OrderByDescending(a => a.Date).ToList();
                                lineNumber = "105";
                            }
                        }
                    }

                    lineNumber = "111";

                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        DateTime startDate = DateTime.Today.AddDays(-60);
                        var buildingId = myBuildings[cmbBuilding.SelectedIndex].ID;
                        var requisitions = (from r in context.tblRequisitions.Include(a => a.Supplier)
                                            where r.building == buildingId
                                            && (r.processed == false || r.paid == false || (r.paid && r.trnDate > startDate))
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
                               
                                if (!processed)
                                    unProcessedRequisitions.Add(r);
                                else if (!paid)
                                    unPaidRequisitions.Add(r);
                                else
                                    paidRequisitions.Add(r);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loading " + lineNumber + ":" + ex.Message + "\n" + ex.StackTrace);
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
            bool dailyExceed, monthExceed;
            double monthLimit, dailyLimit = 0;
            //CheckLimits(requestedAmt, out dailyExceed, out monthExceed, out monthLimit, out dailyLimit);
            //lblDayLimit.Text = dailyLimit.ToString("#,##0.00");
            //lblMonthLimit.Text = monthLimit.ToString("#,##0.00");
            //lblDayLimit.Refresh();
            //lblMonthLimit.Refresh();
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

        private void CheckLimits(double requestedAmt, out bool d, out bool m, out double md, out double dd)
        {
            double limitD = myBuildings[cmbBuilding.SelectedIndex].limitD;
            double limitM = myBuildings[cmbBuilding.SelectedIndex].limitM;
            String path = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? GetTrustPath() : myBuildings[cmbBuilding.SelectedIndex].DataPath);
            String acc = (cmbAccount.SelectedItem.ToString().ToUpper() == "TRUST" ? myBuildings[cmbBuilding.SelectedIndex].Trust.Replace("/", "") : myBuildings[cmbBuilding.SelectedIndex].OwnBank.Replace("/", ""));
            DateTime cDate = DateTime.Now;

            DateTime sDate = new DateTime(cDate.Year, cDate.Month, 1, 0, 0, 0);
            DateTime eDate = new DateTime(cDate.Year, cDate.Month, DateTime.DaysInMonth(cDate.Year, cDate.Month), 23, 59, 59);
            List<Trns> mTransactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc).Where(c => DateTime.Parse(c.Date) >= sDate && DateTime.Parse(c.Date) <= eDate).ToList();

            DateTime dsDate = new DateTime(cDate.Year, cDate.Month, cDate.Day, 0, 0, 0);
            DateTime deDate = new DateTime(cDate.Year, cDate.Month, cDate.Day, 23, 59, 59);
            List<Trns> dTransactions = mTransactions.Where(c => DateTime.Parse(c.Date) >= dsDate && DateTime.Parse(c.Date) <= deDate).ToList();

            //double dTotal = paidRequisitions.Where(c => c.trnDate >= dsDate && c.trnDate <= deDate).ToList().Sum(c => c.amount);
            //.Sum(c=>c.amount) Controller.pastel.GetTransactions(path, "G", 101, 112, acc).Where(c => DateTime.Parse(c.Date) >= dsDate && DateTime.Parse(c.Date) <= deDate).ToList();

            double mPastelTotal = 0;
            double dPastelTotal = 0;
            if (cmbAccount.SelectedItem.ToString() == "TRUST")
            {
                mTransactions = mTransactions.Where(c => double.Parse(c.Amount) > 0).ToList();
                dTransactions = dTransactions.Where(c => double.Parse(c.Amount) > 0).ToList();
                mPastelTotal = mTransactions.Sum(c => double.Parse(c.Amount));
                dPastelTotal = dTransactions.Sum(c => double.Parse(c.Amount));
            }
            else
            {
                mTransactions = mTransactions.Where(c => double.Parse(c.Amount) < 0).ToList();
                dTransactions = dTransactions.Where(c => double.Parse(c.Amount) < 0).ToList();
                mPastelTotal = mTransactions.Sum(c => double.Parse(c.Amount)) * -1;
                dPastelTotal = dTransactions.Sum(c => double.Parse(c.Amount)) * -1;
            }

            double unprocDaily = unProcessedRequisitions.Where(c => c.trnDate >= dsDate && c.trnDate <= deDate).Sum(c => c.amount);
            double unprocMonth = unProcessedRequisitions.Where(c => c.trnDate >= sDate && c.trnDate <= eDate).Sum(c => c.amount);

            double unpaidDaily = unPaidRequisitions.Where(c => c.trnDate >= dsDate && c.trnDate <= deDate).Sum(c => c.amount);
            double unpaidMonth = unPaidRequisitions.Where(c => c.trnDate >= sDate && c.trnDate <= eDate).Sum(c => c.amount);

            if (Controller.user.id == 1)
            {
                MessageBox.Show("Monthly = " + (mPastelTotal + unprocMonth + unpaidMonth).ToString());
                MessageBox.Show("Daily = " + (dPastelTotal + unprocDaily + unpaidDaily).ToString());
            }

            m = (mPastelTotal + unprocMonth + unpaidMonth) + requestedAmt < limitM;
            d = (dPastelTotal + unprocDaily + unpaidDaily) + requestedAmt < limitD;

            md = limitM - (mPastelTotal + unprocMonth + unpaidMonth);
            dd = limitD - (dPastelTotal + unprocDaily + unpaidDaily);
        }

        private int? editRequisitonId = null;

        private void EditRequisition(RequisitionList req)
        {
            _Documents = new Dictionary<string, byte[]>();
            btnCancel.Visible = true;
            using (var context = SqlDataHandler.GetDataContext())
            {
                editRequisitonId = Convert.ToInt32(req.ID);

                var requisition = context.tblRequisitions.Single(a => a.id == editRequisitonId.Value);

                int buildingId = requisition.building;

                trnDatePicker.Value = requisition.trnDate;
                // cmbBuilding.SelectedItem = myBuildings.Where(a => a.ID == buildingId);

                // cmbBuilding_SelectedIndexChanged(this, EventArgs.Empty);

                cmbAccount.SelectedValue = requisition.account;
                cmbAccount_SelectedIndexChanged(this, EventArgs.Empty);
                if (requisition.InvoiceDate != null)
                {
                    dtInvoiceDate.Value = requisition.InvoiceDate.Value;
                }

                for (int x = 0; x < cmbLedger.Items.Count; x++)
                {
                    if (cmbLedger.Items[x].ToString() == requisition.ledger)
                    {
                        cmbLedger.SelectedIndex = x;
                        cmbLedger_SelectedIndexChanged(this, EventArgs.Empty);
                        break;
                    }
                }

                _Supplier = null;
                if (requisition.SupplierId != null)
                {
                    _Supplier = context.SupplierSet.Single(a => a.id == requisition.SupplierId);
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
                        else
                        {
                            lbBankName.Text = bankDetails.Bank.Name + " (" + bankDetails.BranceCode + ")";
                            lbAccountNumber.Text = bankDetails.AccountNumber;
                            btnSave.Enabled = true;
                        }
                    }
                }

                txtInvoiceNumber.Text = requisition.InvoiceNumber;
                txtPaymentRef.Text = requisition.payreference;
                txtAmount.Text = requisition.amount.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            bool dLimit, mLimit;
            try
            {
                double.Parse(txtAmount.Text);
            }
            catch
            {
                Controller.HandleError("Invalid invoice amount", "Validation Error");
                this.Cursor = Cursors.Arrow;
                return;
            }
            double md, dd;
            CheckLimits(double.Parse(txtAmount.Text), out dLimit, out mLimit, out md, out dd);
            bool showPassword = false;

            if (_Supplier == null)
            {
                Controller.HandleError("Supplier not selected, please select a supplier", "Validation Error");
                this.Cursor = Cursors.Arrow;
                return;
            }

            if (!dLimit && editRequisitonId == null)
            {
                if (MessageBox.Show("Daily limit exceeded. Enter password to continue?", "Requisitions", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    showPassword = true;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    return;
                }
            }
            else if (!mLimit && editRequisitonId == null)
            {
                if (MessageBox.Show("Monthly limit exceeded. Enter password to continue?", "Requisitions", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    showPassword = true;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
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
                        this.Cursor = Cursors.Arrow;
                        return;
                    }
                }
            }

            decimal amt;
            if (decimal.TryParse(txtAmount.Text, out amt) && cmbBuilding.SelectedItem != null && cmbLedger.SelectedItem != null && cmbAccount.SelectedItem != null)
            {
                if (dtInvoiceDate.Value <= _minDate)
                {
                    Controller.HandleError("Invoice Date required. Please select a date.", "Validation Error");
                    this.Cursor = Cursors.Arrow;
                    return;
                }
                if (String.IsNullOrWhiteSpace(txtInvoiceNumber.Text))
                {
                    Controller.HandleError("Invoice Number required. Please supply an invoice number.", "Validation Error");
                    this.Cursor = Cursors.Arrow;
                    return;
                }
                using (var context = SqlDataHandler.GetDataContext())
                {
                    if (_Documents.Count == 0 && editRequisitonId == null)
                    {
                        Controller.HandleError("Invoice attachment required, please upload Invoice PDF", "Validation Error");
                        this.Cursor = Cursors.Arrow;
                        return;
                    }
                    else if (editRequisitonId != null && _Documents.Count == 0)
                    {
                        var docCount = context.RequisitionDocumentSet.Count(a => a.RequisitionId == editRequisitonId.Value);

                        if (docCount < 0)
                        {
                            Controller.HandleError("Invoice attachment required, please upload Invoice PDF", "Validation Error");
                            this.Cursor = Cursors.Arrow;
                            return;
                        }
                    }

                    var buildingId = myBuildings[cmbBuilding.SelectedIndex].ID;
                    Data.SupplierData.SupplierBuilding bankDetails = null;
                    if (_Supplier != null)
                    {
                        var q = (from r in context.tblRequisitions
                                 where r.SupplierId == _Supplier.id
                                 && r.InvoiceNumber == txtInvoiceNumber.Text
                                 && r.amount == amt
                                 select r);
                        bool showDuplicate = false;
                        if (editRequisitonId != null)
                            showDuplicate = q.Where(a => a.id != editRequisitonId.Value).Count() > 0;
                        else
                            showDuplicate = q.Count() > 0;

                        if (showDuplicate)
                        {
                            Controller.HandleError("Duplicate requisition detected.\n" +
                               txtInvoiceNumber.Text + " invoice has already been processed", "Validation Error");
                            this.Cursor = Cursors.Arrow;
                            return;
                        }

                        bankDetails = context.SupplierBuildingSet
                                                .Include(a => a.Bank)
                                                .SingleOrDefault(a => a.BuildingId == buildingId && a.SupplierId == _Supplier.id);
                        if (bankDetails == null)
                        {
                            Controller.HandleError("Supplier banking details for this building is not configured.\n" +
                                                "Please capture bank details for this building on the suppier detail screen.", "Validation Error");
                            this.Cursor = Cursors.Arrow;
                            return;
                        }
                    }
                    tblRequisition item = null;
                    if (editRequisitonId == null)
                    {
                        item = new tblRequisition();
                        context.tblRequisitions.Add(item);
                    }
                    else
                    {
                        item = context.tblRequisitions.Single(a => a.id == editRequisitonId.Value);
                    }

                    item.trnDate = trnDatePicker.Value.Date;
                    item.account = cmbAccount.SelectedItem.ToString();
                    item.reference = myBuildings[cmbBuilding.SelectedIndex].Abbr + (cmbAccount.SelectedItem.ToString() == "TRUST" ? " (" + myBuildings[cmbBuilding.SelectedIndex].Trust + ")" : "");
                    item.ledger = cmbLedger.SelectedItem.ToString();
                    item.amount = amt;
                    item.payreference = txtPaymentRef.Text;
                    item.userID = Controller.user.id;
                    item.building = buildingId;
                    item.SupplierId = _Supplier == null ? (int?)null : _Supplier.id;
                    item.InvoiceNumber = txtInvoiceNumber.Text;
                    item.InvoiceDate = dtInvoiceDate.Value;
                    item.BankName = bankDetails == null ? (string)null : bankDetails.Bank.Name;
                    item.BranchCode = bankDetails == null ? (string)null : bankDetails.BranceCode;
                    item.BranchName = bankDetails == null ? (string)null : bankDetails.BranchName;
                    item.AccountNumber = bankDetails == null ? (string)null : bankDetails.AccountNumber;

                    if (editRequisitonId != null)
                    {
                        //clear all invoice attachments and load again
                        var docs = context.RequisitionDocumentSet.Where(a => a.RequisitionId == editRequisitonId && a.IsInvoice == true).ToList();
                        context.RequisitionDocumentSet.RemoveRange(docs);
                    }

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
                            this.Cursor = Cursors.Arrow;
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

                // case 2: //month while (startDate.AddMonths(1) <= dtEndDate.Value) {
                // dates.Add(startDate.AddMonths(1)); startDate = startDate.AddMonths(1); } break;

                // case 3: //yearly while (startDate.AddYears(1) <= dtEndDate.Value) {
                // dates.Add(startDate.AddYears(1)); startDate = startDate.AddYears(1); } break; }

                // #region Build Up SQL Query

                // String query = "INSERT INTO tblRequisition(trnDate, account, reference,
                // payreference, ledger, amount, userID,
                // building,SupplierId,InvoiceNumber,InvoiceDate,BankName,BranchCode,AccountNumber,BranchName)";
                // query += " VALUES(@trnDate, @account, @reference, @payment, @ledger, @amount,
                // @userID,
                // @building,@SupplierId,@InvoiceNumber,@InvoiceDate,@BankName,@BranchCode,@AccountNumber,@BranchName)";
                // Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                // sqlParms.Add("@trnDate",
                // DateTime.Parse(trnDatePicker.Value.ToString("yyyy/MM/dd")));
                // sqlParms.Add("@account", cmbAccount.SelectedItem.ToString());
                // sqlParms.Add("@reference", _MyBuildings[cmbBuilding.SelectedIndex].Abbr +
                // (cmbAccount.SelectedItem.ToString() == "TRUST" ? " (" +
                // _MyBuildings[cmbBuilding.SelectedIndex].Trust + ")" : ""));
                // sqlParms.Add("@ledger", cmbLedger.SelectedItem.ToString());
                // sqlParms.Add("@amount", double.Parse(txtAmount.Text)); sqlParms.Add("@payment",
                // txtPaymentRef.Text); sqlParms.Add("@userID", Controller.user.id);
                // sqlParms.Add("@building", _MyBuildings[cmbBuilding.SelectedIndex].ID);
                // sqlParms.Add("@SupplierId", _Supplier == null ? (int?)null : _Supplier.id);
                // sqlParms.Add("@InvoiceNumber", txtInvoiceNumber.Text);
                // sqlParms.Add("@InvoiceDate", dtInvoiceDate.Value.ToString("yyyy/MM/dd"));

                // sqlParms.Add("@BankName", _Supplier == null ? (string)null : _Supplier.BankName);
                // sqlParms.Add("@BranchCode", _Supplier == null ? (string)null :
                // _Supplier.BranceCode); sqlParms.Add("@AccountNumber", _Supplier == null ?
                // (string)null : _Supplier.AccountNumber); sqlParms.Add("@BranchName", _Supplier ==
                // null ? (string)null : _Supplier.BranchName);

                // #endregion

                // foreach (DateTime dt in dates) { sqlParms["@trnDate"] = dt; dh.SetData(query,
                // sqlParms, out status); }

                //}

                #endregion Recurring

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

            editRequisitonId = null;
            btnCancel.Visible = false;
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
            List<int> requisitionIds = new List<int>();
            foreach (DataGridViewRow dvr in dgUnprocessed.SelectedRows)
            {
                RequisitionList r = unProcessedRequisitions[dvr.Index];
                requisitionIds.Add(Convert.ToInt32(r.ID));
            }

            if (requisitionIds.Count > 0)
            {
                if (Controller.AskQuestion("Are you sure you want to delete " + requisitionIds.Count + " requisitions?" + Environment.NewLine
                                         + "Please note that this will also delete linked documents and maintenance records"))
                {
                    foreach (var requisitionId in requisitionIds)
                        DeleteRequisition(requisitionId);

                    LoadRequisitions();
                    dgUnprocessed.Invalidate();
                    UpdateBalanceLabel();
                }

                foreach (DataGridViewRow dvr in dgUnprocessed.SelectedRows)
                {
                    dvr.Selected = false;
                }
            }
        }

        private void DeleteRequisition(int id)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var requisition = context.tblRequisitions.Single(a => a.id == id);
                //delete all requisition documents
                var docs = context.RequisitionDocumentSet.Where(a => a.RequisitionId == id).ToList();
                if (docs.Count() > 0)
                    context.RequisitionDocumentSet.RemoveRange(docs);

                var maintenanceRecords = context.MaintenanceSet.Where(a => a.RequisitionId == id).ToList();
                if (maintenanceRecords.Count > 0)
                {
                    var maintIds = maintenanceRecords.Select(a => a.id).ToArray();
                    var maintenanceDocs = context.MaintenanceDocumentSet.Where(a => maintIds.Contains(a.MaintenanceId)).ToList();

                    if (maintenanceDocs.Count > 0)
                        context.MaintenanceDocumentSet.RemoveRange(maintenanceDocs);
                    context.MaintenanceSet.RemoveRange(maintenanceRecords);
                }

                context.tblRequisitions.Remove(requisition);
                context.SaveChanges();
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

                    if (_Supplier != null)
                    {
                        //find previous requisition for this building and this supplier
                        var q = from m in context.tblRequisitions
                                where m.building == buildingId
                                && m.SupplierId == _Supplier.id
                                orderby m.id descending
                                select m;
                        var prevRequisition = q.FirstOrDefault();
                        if (prevRequisition != null)
                        {
                            for (int x = 0; x < cmbLedger.Items.Count; x++)
                            {
                                if (cmbLedger.Items[x].ToString() == prevRequisition.ledger)
                                {
                                    cmbLedger.SelectedIndex = x;
                                    cmbLedger_SelectedIndexChanged(this, EventArgs.Empty);
                                    break;
                                }
                            }
                        }
                    }
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
                EditRequisition(req);
                /*
                String query = "DELETE FROM tblRequisition WHERE ID = " + req.ID;
                String status = "";
                dh.SetData(query, null, out status);
                LoadRequisitions();
                */
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
                    if (IsValidPdf(ofdAttachment.FileNames[i]))
                    {
                        if (_Documents.Keys.Contains(ofdAttachment.SafeFileNames[i]))
                            _Documents[ofdAttachment.SafeFileNames[i]] = File.ReadAllBytes(ofdAttachment.FileNames[i]);
                        else
                            _Documents.Add(ofdAttachment.SafeFileNames[i], File.ReadAllBytes(ofdAttachment.FileNames[i]));
                    }
                    else
                    {
                        Controller.HandleError("Invalid PDF\n" + ofdAttachment.FileNames[i] + "\n Please load a different pdf");
                    }
                }
            }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadRequisitions();
            ClearRequisitions();
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
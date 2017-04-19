using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Astro.Library.Entities;
using System.Linq;

namespace Astrodon.Controls
{
    public partial class usrRequisition : UserControl
    {
        private List<Building> allBuildings;
        private List<Building> myBuildings = new List<Building>();
        private BindingList<RequisitionList> unProcessedRequisitions = new BindingList<RequisitionList>();
        private BindingList<RequisitionList> unPaidRequisitions = new BindingList<RequisitionList>();
        private BindingList<RequisitionList> paidRequisitions = new BindingList<RequisitionList>();

        private SqlDataHandler dh = new SqlDataHandler();
        private Dictionary<String, double> avAmts = new Dictionary<string, double>();
        private String status;

        public usrRequisition()
        {
            InitializeComponent();
        }

        private void usrRequisition_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            dgUnprocessed.DataSource = unProcessedRequisitions;
            dgUnpaid.DataSource = unPaidRequisitions;
            dgPaid.DataSource = paidRequisitions;
            cmbRecur.SelectedIndex = 0;
        }

        private void LoadBuildings()
        {
            allBuildings = new Buildings(false).buildings;

            foreach (int bid in Controller.user.buildings)
            {
                foreach (Building b in allBuildings)
                {
                    if (bid == b.ID && b.Web_Building && !myBuildings.Contains(b))
                    {
                        myBuildings.Add(b);
                        break;
                    }
                }
            }
            myBuildings = myBuildings.OrderBy(c => c.Name).ToList();
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.DataSource = myBuildings;
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
                            transactions = Controller.pastel.GetTransactions(path, "G", 101, 112, acc).OrderByDescending(c => c.Date).ToList();
                        }
                    }
                    lineNumber = "111";
                    String query = "SELECT r.id, r.trnDate, b.Building, r.account, r.processed, r.paid, r.reference, r.contractor, r.payreference, r.amount, r.ledger, b.acc, b.ownbank, b.datapath";
                    query += " FROM tblRequisition AS r INNER JOIN tblBuildings AS b ON r.building = b.id";
                    query += " WHERE b.id = " + buildingID + " ORDER BY trnDate DESC";
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    DataSet dsRequisitions = dh.GetData(query, null, out status);
                    lineNumber = "117";
                    if (dsRequisitions != null && dsRequisitions.Tables.Count > 0 && dsRequisitions.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsRequisitions.Tables[0].Rows)
                        {
                            RequisitionList r = new RequisitionList
                            {
                                ID = dr["id"].ToString(),
                                trnDate = DateTime.Parse(dr["trnDate"].ToString()),
                                building = dr["Building"].ToString(),
                                account = dr["account"].ToString(),
                                reference = dr["reference"].ToString(),
                                payreference = dr["payreference"].ToString(),
                                amount = double.Parse(dr["amount"].ToString()),
                                ledger = dr["ledger"].ToString(),
                                paid = bool.Parse(dr["paid"].ToString())
                            };
                            bool matched = false;
                            bool paid = bool.Parse(dr["paid"].ToString());
                            bool processed = bool.Parse(dr["processed"].ToString());
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
                                paidRequisitions.Add(r);
                                dh.SetData(updateQuery, null, out status);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            double amt;
            if (double.TryParse(txtAmount.Text, out amt) && cmbBuilding.SelectedItem != null && cmbLedger.SelectedItem != null && cmbAccount.SelectedItem != null)
            {
                String query = "INSERT INTO tblRequisition(trnDate, account, reference, payreference, ledger, amount, userID, building)";
                query += " VALUES(@trnDate, @account, @reference, @payment, @ledger, @amount, @userID, @building)";
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                sqlParms.Add("@trnDate", DateTime.Parse(trnDatePicker.Value.ToString("yyyy/MM/dd")));
                sqlParms.Add("@account", cmbAccount.SelectedItem.ToString());
                sqlParms.Add("@reference", myBuildings[cmbBuilding.SelectedIndex].Abbr + (cmbAccount.SelectedItem.ToString() == "TRUST" ? " (" + myBuildings[cmbBuilding.SelectedIndex].Trust + ")" : ""));
                sqlParms.Add("@ledger", cmbLedger.SelectedItem.ToString());
                sqlParms.Add("@amount", double.Parse(txtAmount.Text));
                sqlParms.Add("@payment", txtPaymentRef.Text);
                sqlParms.Add("@userID", Controller.user.id);
                sqlParms.Add("@building", myBuildings[cmbBuilding.SelectedIndex].ID);
                dh.SetData(query, sqlParms, out status);
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
                MessageBox.Show("Please enter all fields");
            }
        }

        private void ClearRequisitions()
        {
            trnDatePicker.Value = DateTime.Now;
            txtPaymentRef.Text = "";
            lblBalance.Text = "";
            lblAvAmt.Text = "";
            txtAmount.TextChanged -= txtAmount_TextChanged;
            txtAmount.Text = "";
            txtAmount.TextChanged += txtAmount_TextChanged;
            this.Invalidate();
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
            double requestedAmt = double.TryParse(txtAmount.Text, out requestedAmt) ? requestedAmt : 0;
            lblAvAmt.Text = (double.Parse(lblBalance.Text) - requestedAmt - (requestedAmt > 0 ? GetEFTFee() : 0)).ToString("#,##0.00");
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

        private void btnViewTrans_Click(object sender, EventArgs e)
        {
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
    }
}
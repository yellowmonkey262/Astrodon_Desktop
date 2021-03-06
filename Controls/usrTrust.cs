using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrTrust : UserControl
    {
        private Buildings BuildingManager;
        private Building selectedBuilding;
        private BindingSource bs = new BindingSource();

        public usrTrust()
        {
            InitializeComponent();
        }

        private void usrTrust_Load(object sender, EventArgs e)
        {
            LoadCombo();
            dtFrom.Value = DateTime.Now.AddDays(-3);
            dtTo.Value = DateTime.Now;
            dgTrans.DataSource = bs;
        }

        private void LoadCombo()
        {
            BuildingManager = new Buildings(true, "All buildings");
            cmbBuilding.DataSource = BuildingManager.buildings;
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.ValueMember = "ID";
        }

        private List<Trns> LoadTransactions(int startPeriod, int endPeriod, String account, out double remBal, out double trnsBal)
        {
            List<Trns> trs = Controller.pastel.GetTransactions(GetTrustPath(), "G", startPeriod, endPeriod, account.Replace("/", ""));
            List<Trns> trans = new List<Trns>();
            remBal = 0;
            trnsBal = 0;
            foreach (Trns t in trs)
            {
                DateTime tdate = DateTime.Parse(t.Date);
                DateTime sDate = new DateTime(dtFrom.Value.Year, dtFrom.Value.Month, dtFrom.Value.Day, 0, 0, 0);
                if (tdate >= sDate && tdate <= dtTo.Value)
                {
                    trans.Add(t);
                    trnsBal += double.Parse(t.Amount);
                }
                else if (tdate < sDate)
                {
                    remBal += double.Parse(t.Amount);
                }
            }
            return trans;
        }

        private double GetBalance(String account, int startPeriod)
        {
            String acc = Controller.pastel.GetAccount(GetTrustPath(), account.Replace("/", ""));
            bool isthisyear = IsThisYear();
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
                    int lPeriod = 31;
                    int tPeriod = 19;
                    if (startPeriod <= 0)
                    {
                        if (isthisyear)
                        {
                            int additionalMonth = account.StartsWith("9391", StringComparison.Ordinal) ? 1 : 0;
                            tPeriod = 18 + startPeriod + additionalMonth;
                        }
                        else
                        {
                            lPeriod = 31 + startPeriod;
                        }
                    }
                    else
                    {
                        tPeriod = 6 + startPeriod;
                    }
                    double lbal;

                    for (int i = 20; i <= lPeriod; i++) { bal += (double.TryParse(accBits[i], out lbal) ? lbal : 0); }
                    //if (Controller.user.id == 1)
                    //{
                    //    MessageBox.Show(bal.ToString());
                    //}
                    if (isthisyear)//|| startPeriod > 0
                    {
                        for (int i = 7; i <= tPeriod; i++)
                        {
                            bal += (double.TryParse(accBits[i], out lbal) ? lbal : 0);
                            //if (Controller.user.id == 1)
                            //{
                            //    MessageBox.Show((i - 6).ToString() + "=" + bal.ToString());
                            //}
                        }
                    }
                }
                catch { }
                return bal;
            }
        }

        private String GetTrustPath()
        {
            String query = "SELECT trust FROM tblSettings";
            String status;
            DataSet ds = (new SqlDataHandler()).GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["trust"].ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedBuilding = BuildingManager.buildings[cmbBuilding.SelectedIndex];
            }
            catch { }
        }

        private bool IsThisYear()
        {
            String query = "SELECT trust FROM tblSettings";
            String status;
            DataSet ds = new SqlDataHandler().GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                String tYear = ds.Tables[0].Rows[0]["trust"].ToString().Replace("TRUST", "");
                int year = (int.TryParse(tYear, out year) ? year : 1);
                year += 2000;
                return year == DateTime.Now.Year;
            }
            else
            {
                return false;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            bs.Clear();
            int fromMonth = dtFrom.Value.Month - 2;
            int toMonth = dtTo.Value.Month - 2;
            bool thisYear = IsThisYear();
            int periodYear = (thisYear ? 112 : 12);
            if (fromMonth == 1 && thisYear) { fromMonth = 13; }
            if (toMonth == 1 && thisYear) { toMonth = 13; }
            int sPeriod = (fromMonth <= 0 ? periodYear + fromMonth : 100 + fromMonth);
            int ePeriod = (toMonth <= 0 ? periodYear + toMonth : 100 + toMonth);
            List<Trns> transactions = new List<Trns>();
            double trnBal = 0;
            double remBal = 0;
            List<string> btmsg = new List<string>();
            if (selectedBuilding.ID == 0)
            {
                Dictionary<String, List<Trns>> buildingTrans = new Dictionary<string, List<Trns>>();
                foreach (Building b in BuildingManager.buildings)
                {
                    if (b.ID != 0 && b.Web_Building)
                    {
                        transactions = LoadBuildingTransactions(sPeriod, ePeriod, fromMonth, b.Trust);
                        btmsg.Add(b.Name + " = " + transactions.Count.ToString());
                        if (transactions.Count > 2)
                        {
                            buildingTrans.Add(b.Name, transactions);
                        }
                    }
                }
                if (Controller.user.id == 1)
                {
                    // MessageBox.Show(String.Join(";", btmsg));
                }

                String pdfFile = new PDF().TrustMovement(buildingTrans);
                if (!String.IsNullOrEmpty(pdfFile))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(pdfFile);
                    }
                    catch
                    {
                        MessageBox.Show("File " + pdfFile + " is in use. Please close and try again.");
                    }
                }
                else
                {
                    MessageBox.Show("No file created. Please try again.");
                }
            }
            else
            {
                //List<Trns> bTrans = LoadTransactions(sPeriod, ePeriod, selectedBuilding.Trust, out remBal, out trnBal);
                //double openingBalance = GetBalance(selectedBuilding.Trust, fromMonth - 1) + remBal; //- selectedBuilding.Period
                //Trns openingTrns = new Trns
                //{
                //    Amount = openingBalance.ToString("#0.00"),
                //    Date = dtFrom.Value.AddDays(-1).ToString("yyyy/MM/dd"),
                //    Description = "Opening Balance",
                //    Reference = ""
                //};
                //transactions.Add(openingTrns);
                //transactions.AddRange(bTrans);
                //Trns closingTrns = new Trns
                //{
                //    Amount = (openingBalance + trnBal).ToString("#0.00"),
                //    Date = dtTo.Value.ToString("yyyy/MM/dd"),
                //    Description = "Closing Balance",
                //    Reference = ""
                //};
                //transactions.Add(closingTrns);
                transactions = LoadBuildingTransactions(sPeriod, ePeriod, fromMonth, selectedBuilding.Trust);
            }
            transactions = transactions.OrderBy(t => t.Date).ToList();
            foreach (Trns t in transactions) { bs.Add(t); }
        }

        //private void ProcessAllBuildings()
        //{
        //    int fromMonth = dtFrom.Value.Month - 2;
        //    int toMonth = dtTo.Value.Month - 2;
        //    int periodYear = (IsThisYear() ? 112 : 12);
        //    int sPeriod = (fromMonth <= 0 ? periodYear + fromMonth : 100 + fromMonth);
        //    int ePeriod = (toMonth <= 0 ? periodYear + toMonth : 100 + toMonth);
        //            //List<Trns> transactions = new List<Trns>();
        //            //double trnBal = 0;
        //            //double remBal = 0;
        //            //List<Trns> bTrans = LoadTransactions(sPeriod, ePeriod, b.Trust, out remBal, out trnBal);
        //            //double openingBalance = GetBalance(b.Trust, fromMonth - 1) + remBal;//- b.Period
        //            //Trns openingTrns = new Trns
        //            //{
        //            //    Amount = openingBalance.ToString("#0.00"),
        //            //    Date = dtFrom.Value.AddDays(-1).ToString("yyyy/MM/dd"),
        //            //    Description = "Opening Balance",
        //            //    Reference = ""
        //            //};
        //            //transactions.Add(openingTrns);
        //            //transactions.AddRange(bTrans);
        //            //Trns closingTrns = new Trns
        //            //{
        //            //    Amount = (openingBalance + trnBal).ToString("#0.00"),
        //            //    Date = dtTo.Value.ToString("yyyy/MM/dd"),
        //            //    Description = "Closing Balance",
        //            //    Reference = ""
        //            //};
        //            //transactions.Add(closingTrns);
        //            //transactions = transactions.OrderBy(t => t.Date).ToList();

        //        }
        //    }
        //}

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDGV.Print_DataGridView(dgTrans);
        }

        private void dgTrans_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn c in dgTrans.Columns)
            {
                if (!c.Name.StartsWith("col")) { c.Visible = false; }
            }
        }

        private List<Trns> LoadBuildingTransactions(int sPeriod, int ePeriod, int fromMonth, string Trust)
        {
            String wherearewe = "start";
            List<Trns> transactions = new List<Trns>();
            try
            {
                double trnBal = 0;
                double remBal = 0;
                List<Trns> bTrans = LoadTransactions(sPeriod, ePeriod, Trust, out remBal, out trnBal);
                wherearewe = "loaded trans";
                double openingBalance = GetBalance(Trust, fromMonth - 1) + remBal; //- selectedBuilding.Period
                wherearewe = "loaded balance";
                Trns openingTrns = new Trns
                {
                    Amount = openingBalance.ToString("#0.00"),
                    Date = dtFrom.Value.AddDays(-1).ToString("yyyy/MM/dd"),
                    Description = "Opening Balance",
                    Reference = ""
                };
                wherearewe = "openingtrans";
                transactions.Add(openingTrns);
                transactions.AddRange(bTrans);
                wherearewe = "buildtrans";
                Trns closingTrns = new Trns
                {
                    Amount = (openingBalance + trnBal).ToString("#0.00"),
                    Date = dtTo.Value.ToString("yyyy/MM/dd"),
                    Description = "Closing Balance",
                    Reference = ""
                };
                wherearewe = "closingtrans";
                transactions.Add(closingTrns);
            }
            catch (Exception ex)
            {
                MessageBox.Show(wherearewe + " - " + ex.Message);
            }
            return transactions;
        }
    }
}

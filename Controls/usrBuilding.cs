using Astro.Library;
using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Astrodon.Controls
{
    public partial class usrBuilding : UserControl
    {
        private List<PMBuilding> buildings;
        private List<Building> allBuildings;
        private List<Color> colors = new List<Color>();
        private Dictionary<String, List<Trns>> transactions0;
        private Dictionary<String, List<Trns>> transactions1;
        private Dictionary<String, List<Trns>> transactions2;
        private Dictionary<String, List<Trns>> transactions3;
        private Dictionary<String, List<Customer>> customers;
        private String trustPath;

        public usrBuilding()
        {
            InitializeComponent();
            SetBuildings();
            transactions0 = new Dictionary<string, List<Trns>>();
            transactions1 = new Dictionary<string, List<Trns>>();
            transactions2 = new Dictionary<string, List<Trns>>();
            transactions3 = new Dictionary<string, List<Trns>>();
            customers = new Dictionary<string, List<Customer>>();
            GetTrustPath();
        }

        private void SetBuildings()
        {
            String[] excludeMe = new string[] { "AFM", "MV" };
            String[] includeMe = new string[] { "EZ", "HAM", "LTG", "WW" };
            allBuildings = new Buildings(false).buildings;
            buildings = new List<PMBuilding>();
            foreach (int bid in Controller.user.buildings)
            {
                foreach (Building b in allBuildings)
                {
                    PMBuilding pb = new PMBuilding();
                    pb.Code = b.Abbr;
                    pb.Name = b.Name;
                    if (bid == b.ID && b.Web_Building && !buildings.Contains(pb) && !excludeMe.Contains(pb.Code))
                    {
                        buildings.Add(pb);
                        break;
                    }
                }
            }
            buildings = buildings.OrderBy(c => c.Name).ToList();
        }

        private void usrBuilding_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            LoadBuildings();
            this.Cursor = Cursors.Default;
        }

        private double GetBalance(String datapath, String account)
        {
            String acc = Controller.pastel.GetAccount(datapath, account.Replace("/", ""));
            if (!acc.StartsWith("99"))
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
            else
            {
                return 0;
            }
        }

        private DateTime GetLastDate(String code, String datapath, int startperiod, int endperiod, String account, int column, double currBalance)
        {
            List<Trns> trns = Controller.pastel.GetTransactions(datapath, "G", startperiod, endperiod, account.Replace("/", "")).OrderByDescending(c => c.Date).ToList();
            foreach (Trns t in trns)
            {
                t.Cumulative = currBalance.ToString("#,##0.00");
                currBalance -= double.Parse(t.Amount);
            }
            DateTime iniDate = DateTime.Now.AddYears(-2);
            DateTime lastDate = iniDate;
            try
            {
                switch (column)
                {
                    case 0:
                        if (!transactions0.ContainsKey(code)) { transactions0.Add(code, new List<Trns>()); }
                        break;

                    case 1:
                        if (!transactions1.ContainsKey(code)) { transactions1.Add(code, new List<Trns>()); }
                        break;

                    case 2:
                        if (!transactions2.ContainsKey(code)) { transactions2.Add(code, new List<Trns>()); }
                        break;

                    case 3:
                        if (!transactions3.ContainsKey(code)) { transactions3.Add(code, new List<Trns>()); }
                        break;
                }
            }
            catch
            {
                MessageBox.Show(column.ToString() + " - " + code);
            }
            foreach (Trns trn in trns)
            {
                DateTime trnDate = DateTime.Parse(trn.Date);
                if (trnDate >= DateTime.Now.AddMonths(-3))
                {
                    switch (column)
                    {
                        case 0:
                            transactions0[code].Add(trn);
                            break;

                        case 1:
                            transactions1[code].Add(trn);
                            break;

                        case 2:
                            transactions2[code].Add(trn);
                            break;

                        case 3:
                            transactions3[code].Add(trn);
                            break;
                    }
                    if (trnDate > lastDate)
                    {
                        lastDate = trnDate;
                    }
                }
            }
            return lastDate;
        }

        private void GetTrustPath()
        {
            String query = "SELECT trust FROM tblSettings";
            String status;
            DataSet ds = (new SqlDataHandler()).GetData(query, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                trustPath = ds.Tables[0].Rows[0]["trust"].ToString();
            }
            else
            {
                trustPath = String.Empty;
            }
        }

        private void LoadBuildings()
        {
            int tYear = (!String.IsNullOrEmpty(trustPath) ? int.Parse(trustPath.Substring(trustPath.Length - 2, 2)) : 0);
            int dPeriod;
            int tPeriod = Methods.getPeriod(DateTime.Now, 0, out dPeriod);
            int thisYear = DateTime.Now.Year - 2000;
            int tStart = 0;
            int tEnd = 0;

            #region Trust Period

            if (tPeriod - 2 == 0)
            {
                if (thisYear == tYear)
                {
                    tStart = 12;
                    tEnd = 102;
                }
                else
                {
                    tStart = 12;
                    tEnd = 102;
                }
            }
            else if (tPeriod - 2 < 0)
            {
                if (thisYear == tYear)
                {
                    tStart = 111;
                    tEnd = 113;
                }
                else
                {
                    tStart = 11;
                    tEnd = 101;
                }
            }
            else if (tPeriod - 2 > 0)
            {
                tStart = 100 + tPeriod - 2;
                tEnd = 100 + tPeriod;
            }

            #endregion Trust Period

            foreach (PMBuilding pmb in buildings)
            {
                foreach (Building b in allBuildings)
                {
                    if (pmb.Code == b.Abbr)
                    {
                        try
                        {
                            lblStatus.Text = "Processing: " + b.Name;
                            Application.DoEvents();
                            pmb.Bank_Balance = (!String.IsNullOrEmpty(b.Cash_Book) ? GetBalance(b.DataPath, b.Cash_Book).ToString("#,##0.00") : "");
                            pmb.Own_Bank_Balance = (!String.IsNullOrEmpty(b.OwnBank) ? GetBalance(b.DataPath, b.OwnBank).ToString("#,##0.00") : "");
                            pmb.Invest_Balance = (!String.IsNullOrEmpty(b.Cashbook3) ? GetBalance(b.DataPath, b.Cashbook3).ToString("#,##0.00") : "");
                            pmb.Trust_Balance = (!String.IsNullOrEmpty(trustPath) ? GetBalance(trustPath, b.Trust).ToString("#,##0.00") : "");
                            double bb = double.Parse(pmb.Bank_Balance);
                            double tb = double.Parse(pmb.Trust_Balance);
                            int bPeriod;
                            tPeriod = Methods.getPeriod(DateTime.Now, b.Period, out bPeriod);
                            int startperiod = 0;
                            int endperiod = 0;
                            int dataYear = (int.TryParse(b.DataPath.Substring(b.DataPath.Length - 2, 2), out dataYear) ? dataYear : 15);

                            #region Building Period

                            if (bPeriod - 2 == 0)
                            {
                                if (thisYear == dataYear)
                                {
                                    startperiod = 12;
                                    endperiod = 102;
                                }
                                else
                                {
                                    startperiod = 12;
                                    endperiod = 102;
                                }
                            }
                            else if (bPeriod - 2 < 0)
                            {
                                if (thisYear == dataYear && pmb.Code != "EZ")
                                {
                                    startperiod = 111;
                                    endperiod = 113;
                                }
                                else
                                {
                                    startperiod = 11;
                                    endperiod = 101;
                                }
                            }
                            else if (bPeriod - 2 > 0)
                            {
                                startperiod = 100 + bPeriod - 2;
                                endperiod = 100 + bPeriod;
                            }
                            //MessageBox.Show(pmb.Code + ": " + startperiod.ToString() + " - " + endperiod.ToString());

                            #endregion Building Period

                            double outstanding = 0;
                            List<Customer> myCustomers = getCustomers(b, out outstanding);
                            if (!customers.ContainsKey(pmb.Code)) { customers.Add(pmb.Code, myCustomers); }
                            pmb.Outstanding = outstanding.ToString("#,##0.00");
                            pmb.Trust_Last_Transaction_Date = (!String.IsNullOrEmpty(trustPath) ? GetLastDate(pmb.Code, trustPath, tStart, tEnd, b.Trust, 0, double.Parse(pmb.Trust_Balance)).ToString("yyyy/MM/dd") : "");
                            pmb.Bank_Last_Transaction_Date = (!String.IsNullOrEmpty(b.Cash_Book) ? GetLastDate(pmb.Code, b.DataPath, startperiod, endperiod, b.Cash_Book, 1, double.Parse(pmb.Bank_Balance)).ToString("yyyy/MM/dd") : "");
                            pmb.Own_Bank_Last_Transaction_Date = (!String.IsNullOrEmpty(b.OwnBank) ? GetLastDate(pmb.Code, b.DataPath, startperiod, endperiod, b.OwnBank, 2, double.Parse(pmb.Own_Bank_Balance)).ToString("yyyy/MM/dd") : "");
                            pmb.Invest_Last_Transaction_Date = (!String.IsNullOrEmpty(b.Cashbook3) ? GetLastDate(pmb.Code, b.DataPath, startperiod, endperiod, b.Cashbook3, 3, double.Parse(pmb.Invest_Balance)).ToString("yyyy/MM/dd") : "");

                            if (pmb.Trust_Last_Transaction_Date == DateTime.Now.AddYears(-2).ToString("yyyy/MM/dd")) { pmb.Trust_Last_Transaction_Date = "< " + DateTime.Now.AddMonths(-3).ToString(); }
                            if (pmb.Bank_Last_Transaction_Date == DateTime.Now.AddYears(-2).ToString("yyyy/MM/dd")) { pmb.Bank_Last_Transaction_Date = "< " + DateTime.Now.AddMonths(-3).ToString(); }
                            if (pmb.Own_Bank_Last_Transaction_Date == DateTime.Now.AddYears(-2).ToString("yyyy/MM/dd")) { pmb.Own_Bank_Last_Transaction_Date = "< " + DateTime.Now.AddMonths(-3).ToString(); }
                            if (pmb.Invest_Last_Transaction_Date == DateTime.Now.AddYears(-2).ToString("yyyy/MM/dd")) { pmb.Invest_Last_Transaction_Date = "< " + DateTime.Now.AddMonths(-3).ToString(); }

                            if (pmb.Trust_Last_Transaction_Date == "" && !transactions0.ContainsKey(pmb.Code)) { transactions0.Add(pmb.Code, new List<Trns>()); }
                            if (pmb.Bank_Last_Transaction_Date == "" && !transactions1.ContainsKey(pmb.Code)) { transactions1.Add(pmb.Code, new List<Trns>()); }
                            if (pmb.Own_Bank_Last_Transaction_Date == "" && !transactions2.ContainsKey(pmb.Code)) { transactions2.Add(pmb.Code, new List<Trns>()); }
                            if (pmb.Invest_Last_Transaction_Date == "" && !transactions3.ContainsKey(pmb.Code)) { transactions3.Add(pmb.Code, new List<Trns>()); }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    }
                }
            }
            dgBuildings.DataSource = buildings;
            lblStatus.Text = "Buildings";
            ValidateLines();
            Application.DoEvents();
        }

        public List<Customer> getCustomers(Building b, out double totalOS)
        {
            totalOS = 0;
            int buildPeriod;
            int trustPeriod = Methods.getPeriod(DateTime.Now, b.Period, out buildPeriod);
            List<Customer> catCustomers = Controller.pastel.AddCustomers(b.Name, b.DataPath);
            List<Customer> _Customers = new List<Customer>();
            foreach (Customer _customer in catCustomers)
            {
                double totBal = 0;
                for (int li = 0; li < _customer.lastBal.Length; li++) { totBal += _customer.lastBal[li]; }
                for (int i = 0; i < buildPeriod; i++) { totBal += _customer.balance[i]; }
                _customer.setAgeing(Math.Round(totBal, 2), 0);
                totalOS += totBal;
                _Customers.Add(_customer);
            }
            return _Customers;
        }

        private void ValidateLines()
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                try
                {
                    double bb = (double.TryParse(buildings[i].Bank_Balance, out bb) ? bb : 0);
                    double tb = (double.TryParse(buildings[i].Trust_Balance, out tb) ? tb : 0);
                    if (bb + tb != 0)
                    {
                        dgBuildings.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dgBuildings_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                try
                {
                    String bCode = dgBuildings.Rows[e.RowIndex].Cells[0].Value.ToString();
                    int column = e.ColumnIndex;
                    Dictionary<String, List<Trns>> transactions = new Dictionary<string, List<Trns>>();

                    switch (column)
                    {
                        case 2:
                            List<Trns> customerTrns = new List<Trns>();
                            List<Customer> tempCustomers = customers[bCode];
                            Dictionary<String, List<Trns>> cTrans = new Dictionary<string, List<Trns>>();
                            foreach (Customer customer in tempCustomers)
                            {
                                Trns t = new Trns();
                                t.Amount = customer.ageing[0].ToString("#,##0.00");
                                t.Date = DateTime.Now.ToString("yyyy/MM/dd");
                                t.Description = customer.description;
                                t.Reference = customer.accNumber;
                                customerTrns.Add(t);
                            }
                            cTrans.Add(bCode, customerTrns);
                            transactions = cTrans;
                            break;

                        case 3:
                        case 4:
                            transactions = transactions1;
                            break;

                        case 5:
                        case 6:
                            transactions = transactions0;
                            break;

                        case 7:
                        case 8:
                            transactions = transactions2;
                            break;

                        case 9:
                        case 10:
                            transactions = transactions3;
                            break;

                        default:
                            transactions = null;
                            break;
                    }
                    if (transactions != null)
                    {
                        Forms.frmBuildingTrans fTrans = new Forms.frmBuildingTrans(dgBuildings.Rows[e.RowIndex].Cells[1].Value.ToString(), transactions[bCode]);
                        fTrans.ShowDialog();
                    }
                }
                catch { }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }

        private void CreateExcel()
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();

                if (xlApp == null)
                {
                    MessageBox.Show("EXCEL could not be started. Check that your office installation and project references are correct.");
                    return;
                }
                xlApp.Visible = true;

                Excel.Workbook wb = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

                if (ws == null)
                {
                    MessageBox.Show("Worksheet could not be created. Check that your office installation and project references are correct.");
                    return;
                }
                ws.Name = "Building Report";
                ws.Cells[1, "A"].Value2 = "Code";
                ws.Cells[1, "B"].Value2 = "Building";
                ws.Cells[1, "C"].Value2 = "Outstanding Debtors";
                ws.Cells[1, "D"].Value2 = "Bank Balance";
                ws.Cells[1, "E"].Value2 = "Bank Last Trn Date";
                ws.Cells[1, "F"].Value2 = "Trust Balance";
                ws.Cells[1, "G"].Value2 = "Trust Last Trn Date";
                ws.Cells[1, "H"].Value2 = "Own Bank Balance";
                ws.Cells[1, "I"].Value2 = "Own Bank Last Trn Date";
                ws.Cells[1, "J"].Value2 = "Investment Balance";
                ws.Cells[1, "K"].Value2 = "Investment Last Trn Date";

                int rowIdx = 2;
                foreach (PMBuilding pmb in buildings)
                {
                    try
                    {
                        ws.Cells[rowIdx, "A"].Value2 = pmb.Code;
                        ws.Cells[rowIdx, "B"].Value2 = pmb.Name;
                        ws.Cells[rowIdx, "C"].Value2 = pmb.Outstanding;
                        ws.Cells[rowIdx, "C"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.Cells[rowIdx, "D"].Value2 = pmb.Bank_Balance;
                        ws.Cells[rowIdx, "D"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.Cells[rowIdx, "E"].Value2 = pmb.Bank_Last_Transaction_Date;
                        ws.Cells[rowIdx, "F"].Value2 = pmb.Trust_Balance;
                        ws.Cells[rowIdx, "F"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.Cells[rowIdx, "G"].Value2 = pmb.Trust_Last_Transaction_Date;
                        ws.Cells[rowIdx, "H"].Value2 = pmb.Own_Bank_Balance;
                        ws.Cells[rowIdx, "H"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.Cells[rowIdx, "I"].Value2 = pmb.Own_Bank_Last_Transaction_Date;
                        ws.Cells[rowIdx, "J"].Value2 = pmb.Invest_Balance;
                        ws.Cells[rowIdx, "J"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.Cells[rowIdx, "K"].Value2 = pmb.Invest_Last_Transaction_Date;
                        rowIdx++;
                    }
                    catch { }
                }

                ws.Columns.AutoFit();
                ws.Application.ActiveWindow.SplitRow = 1;
                ws.Application.ActiveWindow.SplitColumn = 1;
                ws.Application.ActiveWindow.FreezePanes = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void dgBuildings_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgBuildings.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgBuildings.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgBuildings.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgBuildings.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgBuildings.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                for (int i = 0; i < buildings.Count; i++)
                {
                    try
                    {
                        double bb = (double.TryParse(buildings[i].Bank_Balance, out bb) ? bb : 0);
                        double tb = (double.TryParse(buildings[i].Trust_Balance, out tb) ? tb : 0);
                        if (bb + tb != 0)
                        {
                            dgBuildings.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                        }
                        else
                        {
                            dgBuildings.Rows[i].DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch { }
        }
    }
}
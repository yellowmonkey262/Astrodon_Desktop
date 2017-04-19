using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Astrodon.Controls
{
    public partial class usrDebtorReport : UserControl
    {
        private BindingList<ListReport> reportList = new BindingList<ListReport>();
        private SqlDataHandler dh = new SqlDataHandler();
        private List<Building> buildings;

        public usrDebtorReport()
        {
            InitializeComponent();
        }

        private void usrDebtorReport_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            dgReport.DataSource = reportList;
            dgReport.Columns[0].Frozen = true;
            LoadDebtors();
            LoadBuildings(0);
            dgReport.Width = this.Width - 10;
            dgReport.Left = 5;
            this.Cursor = Cursors.Default;
        }

        private void LoadDebtors()
        {
            cmbDebtor.SelectedIndexChanged -= cmbDebtor_SelectedIndexChanged;
            String query = "SELECT id, name FROM tblUsers WHERE (usertype = 3) ORDER BY name";
            String status;
            DataSet ds = dh.GetData(query, null, out status);
            List<DebtorSelector> debtors = new List<DebtorSelector>();
            DebtorSelector debtor = new DebtorSelector
            {
                ID = 0,
                Name = "All debtors"
            };
            debtors.Add(debtor);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DebtorSelector drDebtor = new DebtorSelector
                    {
                        ID = int.Parse(dr["id"].ToString()),
                        Name = dr["name"].ToString()
                    };
                    debtors.Add(drDebtor);
                }
            }
            cmbDebtor.DataSource = debtors;
            cmbDebtor.DisplayMember = "Name";
            cmbDebtor.ValueMember = "ID";
            cmbDebtor.SelectedIndex = 0;
            cmbDebtor.SelectedIndexChanged += cmbDebtor_SelectedIndexChanged;
        }

        private void cmbDebtor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDebtor.SelectedItem != null) { LoadBuildings(int.Parse(cmbDebtor.SelectedValue.ToString())); }
        }

        private class DebtorSelector
        {
            public int ID { get; set; }

            public String Name { get; set; }
        }

        private void LoadBuildings(int userid)
        {
            this.Cursor = Cursors.WaitCursor;
            reportList.Clear();
            Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
            buildings = bManager.buildings.OrderBy(c => c.Name).ToList();
            foreach (Building b in buildings)
            {
                if (b.Web_Building)
                {
                    String buildingCode = GetBuildingID(b.Abbr);
                    ListReport lr = new ListReport
                    {
                        Name = b.Name,
                        Code = b.Abbr,
                        Debtor = bManager.getDebtorName(b.ID),
                        Letters_updated = GetDate(1, buildingCode),
                        Letters_ageanalysis = GetDate(2, buildingCode),
                        Letters_printed = GetDate(3, buildingCode),
                        Letters_filed = GetDate(4, buildingCode),
                        Statements_updated = GetDate(5, buildingCode),
                        Statements_interest = GetDate(6, buildingCode),
                        Statements_printed = GetDate(7, buildingCode),
                        Statements_filed = GetDate(8, buildingCode),
                        Month_end_updated = GetDate(9, buildingCode),
                        Month_end_invest_account = GetDate(10, buildingCode),
                        Month_end_9990 = GetDate(11, buildingCode),
                        Month_end_4000 = GetDate(12, buildingCode),
                        Month_end_petty_cash = GetDate(13, buildingCode),
                        Daily_trust = GetDate(14, buildingCode),
                        Daily_own = GetDate(15, buildingCode),
                        Daily_file = GetDate(16, buildingCode)
                    };
                    reportList.Add(lr);
                }
            }
            this.Cursor = Cursors.Default;
        }

        private String GetBuildingID(String buildingCode)
        {
            String bID = "";
            foreach (Building b in buildings)
            {
                if (buildingCode == b.Abbr)
                {
                    bID = b.ID.ToString();
                    break;
                }
            }
            return bID;
        }

        private String GetDate(int column, String building)
        {
            String date = "";
            String columnName = "";

            #region Switch

            switch (column)
            {
                case 1:
                    columnName = "lettersupdated";
                    break;

                case 2:
                    columnName = "lettersageanalysis";
                    break;

                case 3:
                    columnName = "lettersprintemail";
                    break;

                case 4:
                    columnName = "lettersfiled";
                    break;

                case 5:
                    columnName = "stmtupdated";
                    break;

                case 6:
                    columnName = "stmtinterest";
                    break;

                case 7:
                    columnName = "stmtprintemail";
                    break;

                case 8:
                    columnName = "stmtfiled";
                    break;

                case 9:
                    columnName = "meupdate";
                    break;

                case 10:
                    columnName = "meinvest";
                    break;

                case 11:
                    columnName = "me9990";
                    break;

                case 12:
                    columnName = "me4000";
                    break;

                case 13:
                    columnName = "mepettycash";
                    break;

                case 14:
                    columnName = "dailytrust";
                    break;

                case 15:
                    columnName = "dailyown";
                    break;

                case 16:
                    columnName = "dailyfile";
                    break;
            }

            #endregion Switch

            String query = "SELECT max(completeDate) as myDate FROM tblDebtors WHERE " + columnName + " = 'True' AND buildingID = " + building;
            String status = "";
            DataSet dsMax = dh.GetData(query, null, out status);
            if (dsMax != null && dsMax.Tables.Count > 0 && dsMax.Tables[0].Rows.Count > 0)
            {
                date = (!String.IsNullOrEmpty(dsMax.Tables[0].Rows[0]["myDate"].ToString()) ? DateTime.Parse(dsMax.Tables[0].Rows[0]["myDate"].ToString()).ToString("yyyy/MM/dd HH:mm") : "");
            }
            return date;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CreateExcel(reportList);
        }

        private void CreateExcel(BindingList<ListReport> reportDS)
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
                ws.Name = "Debtor Report";
                ws.Cells[1, "A"].Value2 = "Building";
                ws.Cells[1, "B"].Value2 = "Code";
                ws.Cells[1, "C"].Value2 = "Debtor";
                ws.Cells[1, "D"].Value2 = "Daily Trust";
                ws.Cells[1, "E"].Value2 = "Daily Own Account";
                ws.Cells[1, "F"].Value2 = "Daily Filing";
                ws.Cells[1, "G"].Value2 = "Letters Updated";
                ws.Cells[1, "H"].Value2 = "Letters Age Analysis";
                ws.Cells[1, "I"].Value2 = "Letter Printed";
                ws.Cells[1, "J"].Value2 = "Letters Filed";
                ws.Cells[1, "K"].Value2 = "Statements Updated";
                ws.Cells[1, "L"].Value2 = "Statements Interest";
                ws.Cells[1, "M"].Value2 = "Statements Printed";
                ws.Cells[1, "N"].Value2 = "Statements Filed";
                ws.Cells[1, "O"].Value2 = "Month End Updated";
                ws.Cells[1, "P"].Value2 = "Month End Investment Account";
                ws.Cells[1, "Q"].Value2 = "Month End 9990";
                ws.Cells[1, "R"].Value2 = "Month End 4000";
                ws.Cells[1, "S"].Value2 = "Month End Petty Cash";

                int rowIdx = 2;
                foreach (ListReport lr in reportDS)
                {
                    try
                    {
                        ws.Cells[rowIdx, "A"].Value2 = lr.Name;
                        ws.Cells[rowIdx, "B"].Value2 = lr.Code;
                        ws.Cells[rowIdx, "C"].Value2 = lr.Debtor;
                        ws.Cells[rowIdx, "D"].Value2 = lr.Daily_trust;
                        ws.Cells[rowIdx, "E"].Value2 = lr.Daily_own;
                        ws.Cells[rowIdx, "F"].Value2 = lr.Daily_file;
                        ws.Cells[rowIdx, "G"].Value2 = lr.Letters_updated;
                        ws.Cells[rowIdx, "H"].Value2 = lr.Letters_ageanalysis;
                        ws.Cells[rowIdx, "I"].Value2 = lr.Letters_printed;
                        ws.Cells[rowIdx, "J"].Value2 = lr.Letters_filed;
                        ws.Cells[rowIdx, "K"].Value2 = lr.Statements_updated;
                        ws.Cells[rowIdx, "L"].Value2 = lr.Statements_interest;
                        ws.Cells[rowIdx, "M"].Value2 = lr.Statements_printed;
                        ws.Cells[rowIdx, "N"].Value2 = lr.Statements_filed;
                        ws.Cells[rowIdx, "O"].Value2 = lr.Month_end_updated;
                        ws.Cells[rowIdx, "P"].Value2 = lr.Month_end_invest_account;
                        ws.Cells[rowIdx, "Q"].Value2 = lr.Month_end_9990;
                        ws.Cells[rowIdx, "R"].Value2 = lr.Month_end_4000;
                        ws.Cells[rowIdx, "S"].Value2 = lr.Month_end_petty_cash;
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

        private void dgReport_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            String error = e.Exception.Message;
        }
    }
}
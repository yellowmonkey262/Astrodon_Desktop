using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Astrodon.Controls
{
    public partial class usrMonthReport : UserControl
    {
        private List<Building> buildings;
        private BindingList<MonthReport> results;
        private DateTime today;

        public usrMonthReport()
        {
            InitializeComponent();
            results = new BindingList<MonthReport>();
        }

        private void usrMonthReport_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            today = DateTime.Now;
            dtStart.Value = new DateTime(today.Year, today.Month, 1, 0, 0, 0);
            dgMonthly.DataSource = results;
            LoadReport(0);
        }

        private void LoadBuildings()
        {
            buildings = new Buildings(false).buildings;
        }

        private String completedQuery()
        {
            var dt= new DateTime(dtStart.Value.Year, dtStart.Value.Month, 1);
            
            String query = "SELECT b.Building, b.Code, f.findate, f.completeDate, u.name FROM tblMonthFin AS f INNER JOIN tblBuildings AS b ON f.buildingID = b.Code LEFT OUTER JOIN tblUsers AS u ON f.userID = u.id";
            query += " WHERE finDate >= '" + dt.ToString("yyyy/MM/dd") + "' AND finDate <= '" + dt.AddMonths(1).AddMinutes(-1).ToString("yyyy/MM/dd HH:mm") + "'";
            return query;
        }

        private String incompletedQuery()
        {
            var dt = new DateTime(dtStart.Value.Year, dtStart.Value.Month, 1);

            String query = "SELECT b.Code FROM tblMonthFin AS f INNER JOIN tblBuildings AS b ON f.buildingID = b.Code";
            query += " WHERE finDate >= '" + dt.ToString("yyyy/MM/dd") + "' AND finDate <= '" + dt.AddMonths(1).AddMinutes(-1).ToString("yyyy/MM/dd HH:mm") + "'";
            return query;
        }

        private List<MonthReport> GetAllResults(DataSet ds)
        {
            List<MonthReport> myResults = new List<MonthReport>();
            foreach (Building b in buildings)
            {
                MonthReport mr = new MonthReport
                {
                    Building = b.Name,
                    Code = b.Abbr
                };
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == mr.Code)
                    {
                        mr.User = dr["name"].ToString();
                        mr.finPeriod = (String.IsNullOrEmpty(dr["finDate"].ToString()) ? "" : DateTime.Parse(dr["finDate"].ToString()).ToString("MM yyyy"));
                        mr.prcDate = (String.IsNullOrEmpty(dr["completeDate"].ToString()) ? "" : DateTime.Parse(dr["completeDate"].ToString()).ToString("yyyy/MM/dd HH:mm"));
                    }
                }
                myResults.Add(mr);
            }
            return myResults;
        }

        private List<MonthReport> GetCompletedResults(DataSet ds)
        {
            List<MonthReport> myResults = new List<MonthReport>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MonthReport mr = new MonthReport
                {
                    Building = dr["Building"].ToString(),
                    Code = dr["Code"].ToString(),
                    User = dr["name"].ToString(),
                    finPeriod = (String.IsNullOrEmpty(dr["finDate"].ToString()) ? "" : DateTime.Parse(dr["finDate"].ToString()).ToString("MM yyyy")),
                    prcDate = (String.IsNullOrEmpty(dr["completeDate"].ToString()) ? "" : DateTime.Parse(dr["completeDate"].ToString()).ToString("yyyy/MM/dd HH:mm"))
                };
                myResults.Add(mr);
            }
            return myResults;
        }

        private List<MonthReport> GetIncompleteResults(DataSet ds)
        {
            List<MonthReport> myResults = new List<MonthReport>();
            foreach (Building b in buildings)
            {
                MonthReport mr = new MonthReport
                {
                    Building = b.Name,
                    Code = b.Abbr
                };
                bool hasEntry = false;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == b.Abbr)
                    {
                        hasEntry = true;
                        break;
                    }
                }
                mr.User = "";
                mr.finPeriod = "";
                mr.prcDate = "";
                if (!hasEntry) { myResults.Add(mr); }
            }
            return myResults;
        }

        private void LoadReport(int selection)
        {
            this.Cursor = Cursors.WaitCursor;
            results.Clear();
            SqlDataHandler dh = new SqlDataHandler();
            String status;
            List<MonthReport> myResults = null;
            if (selection == 0)
            {
                myResults = GetAllResults(dh.GetData(completedQuery(), null, out status));
            }
            else if (selection == 1)
            {
                myResults = GetCompletedResults(dh.GetData(completedQuery(), null, out status));
            }
            else
            {
                myResults = GetIncompleteResults(dh.GetData(incompletedQuery(), null, out status));
            }
            foreach (MonthReport mr in myResults) { results.Add(mr); }
            dgMonthly.Invalidate();
            this.Cursor = Cursors.Default;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            CreateExcel();
            this.Cursor = Cursors.Default;
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
                ws.Name = "Financial Checklist Report";
                ws.Cells[1, "A"].Value2 = "Building";
                ws.Cells[1, "B"].Value2 = "Code";
                ws.Cells[1, "C"].Value2 = "Financial Period";
                ws.Cells[1, "D"].Value2 = "Processed Date";
                ws.Cells[1, "E"].Value2 = "User";

                int rowIdx = 2;
                foreach (DataGridViewRow dvr in dgMonthly.Rows)
                {
                    try
                    {
                        ws.Cells[rowIdx, "A"].Value2 = (dvr.Cells[0].Value != null ? dvr.Cells[0].Value.ToString() : "");
                        ws.Cells[rowIdx, "B"].Value2 = (dvr.Cells[1].Value != null ? dvr.Cells[1].Value.ToString() : "");
                        ws.Cells[rowIdx, "C"].Value2 = (dvr.Cells[2].Value != null ? dvr.Cells[2].Value.ToString() : "");
                        ws.Cells[rowIdx, "D"].Value2 = (dvr.Cells[3].Value != null ? dvr.Cells[3].Value.ToString() : "");
                        ws.Cells[rowIdx, "E"].Value2 = (dvr.Cells[4].Value != null ? dvr.Cells[4].Value.ToString() : "");
                        rowIdx++;
                    }
                    catch { }
                }

                ws.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {
            int selection = 0;
            if (rdCompleted.Checked) { selection = 1; } else if (rdIncomplete.Checked) { selection = 2; }
            LoadReport(selection);
        }

        private void rdCompleted_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rd = sender as RadioButton;
            int selection = 0;
            if (rd.Checked)
            {
                if (rd == rdCompleted) { selection = 1; } else { selection = 2; }
            }
            LoadReport(selection);
        }
    }
}
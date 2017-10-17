using Astro.Library.Entities;
using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using Astrodon.ReportService;
using System.IO;
using System.Diagnostics;

namespace Astrodon.Controls
{
    public partial class usrMonthReport : UserControl
    {
        private List<Building> buildings;
        private BindingList<MonthReport> results;
        private DateTime today;
        private List<IdValue> _Years;
        private List<IdValue> _Months;
        private List<IdValue> _Users;

        public usrMonthReport()
        {
            InitializeComponent();
            results = new BindingList<MonthReport>();
            LoadYears();
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from u in context.tblUsers
                        select new IdValue()
                        {
                            Id = u.id,
                            Value = u.name
                        };
                _Users = q.OrderBy(a => a.Value).ToList();
                _Users.Insert(0,new IdValue()
                {
                    Id = 0,
                    Value = "All Users"
                });

                cbUserList.DataSource = _Users;
                cbUserList.ValueMember = "Id";
                cbUserList.DisplayMember = "Value";
                cbUserList.SelectedValue = 0;
            }
        }

        private void LoadYears()
        {
            _Years = new List<IdValue>();
            _Years.Add(new IdValue() { Id = DateTime.Now.Year - 1, Value = (DateTime.Now.Year - 1).ToString() });
            _Years.Add(new IdValue() { Id = DateTime.Now.Year, Value = (DateTime.Now.Year).ToString() });
            _Years.Add(new IdValue() { Id = DateTime.Now.Year + 1, Value = (DateTime.Now.Year + 1).ToString() });

            _Months = new List<IdValue>();
            for (int x = 1; x <= 12; x++)
            {
                _Months.Add(new IdValue()
                {
                    Id = x,
                    Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x)
                });
            }

            cmbYear.DataSource = _Years;
            cmbYear.ValueMember = "Id";
            cmbYear.DisplayMember = "Value";
            cmbYear.SelectedValue = DateTime.Now.AddMonths(-1).Year;

            cmbMonth.DataSource = _Months;
            cmbMonth.ValueMember = "Id";
            cmbMonth.DisplayMember = "Value";
            cmbMonth.SelectedValue = DateTime.Now.AddMonths(-1).Month;

            
        }

        private void usrMonthReport_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            today = DateTime.Now;
            dgMonthly.DataSource = results;
            LoadReport();
        }

        private void LoadBuildings()
        {
            buildings = new Buildings(false).buildings;
        }

        private String completedQuery()
        {
            var selectedYear = cmbYear.SelectedItem as IdValue;
            if (selectedYear == null)
                return string.Empty;

            var selectedMonth = cmbMonth.SelectedItem as IdValue;
            if (selectedMonth == null)
                return string.Empty;

            var dt = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);

            String query = "SELECT b.Building, b.Code, f.findate, f.completeDate, u.name FROM tblMonthFin AS f INNER JOIN tblBuildings AS b ON f.buildingID = b.Code LEFT OUTER JOIN tblUsers AS u ON f.userID = u.id";
            query += " WHERE finDate >= '" + dt.ToString("yyyy/MM/dd") + "' AND finDate <= '" + dt.AddMonths(1).AddMinutes(-1).ToString("yyyy/MM/dd HH:mm") + "'";
            return query;
        }

        private String incompletedQuery()
        {
            var selectedYear = cmbYear.SelectedItem as IdValue;
            if (selectedYear == null)
                return string.Empty;

            var selectedMonth = cmbMonth.SelectedItem as IdValue;
            if (selectedMonth == null)
                return string.Empty;

            var dt = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);

            String query = "SELECT b.Code FROM tblMonthFin AS f INNER JOIN tblBuildings AS b ON f.buildingID = b.Code";
            query += " WHERE finDate >= '" + dt.ToString("yyyy/MM/dd") + "' AND finDate <= '" + dt.AddMonths(1).AddMinutes(-1).ToString("yyyy/MM/dd HH:mm") + "'";
            return query;
        }

        private List<MonthReport> GetAllResults(DataSet ds)
        {
            List<MonthReport> myResults = new List<MonthReport>();
            if (ds == null || buildings == null)
                return myResults;
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
            if (ds == null)
                return new List<MonthReport>();

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
            if (ds == null)
                return new List<MonthReport>();

            List<MonthReport> myResults = new List<MonthReport>();
            if (buildings == null)
                return myResults;

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

        private void LoadReport()
        {
            int selection = 0;
            if (rdCompleted.Checked) { selection = 1; } else if (rdIncomplete.Checked) { selection = 2; }
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
            var idVal = (cbUserList.SelectedItem as IdValue);
            if (idVal != null)
            {
                int selectedUserId = (cbUserList.SelectedItem as IdValue).Id;
                if (selectedUserId > 0)
                    myResults = myResults.Where(a => a.User == (cbUserList.SelectedItem as IdValue).Value).ToList();
            }
            foreach (MonthReport mr in myResults)
            {
                results.Add(mr);
            }
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
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                btnPrint.Enabled = false;
                try
                {
                    using (var reportService = ReportServiceClient.CreateInstance())
                    {
                        try
                        {
                            DateTime dDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
                            var idVal = (cbUserList.SelectedItem as IdValue);
                           var reportData =  reportService.MonthlyReport(SqlDataHandler.GetConnectionString(), dDate, rdCompleted.Checked, idVal != null ? idVal.Id : (int?)null);

                            File.WriteAllBytes(dlgSave.FileName, reportData);
                            Process.Start(dlgSave.FileName);
                        }
                        catch (Exception ex)
                        {
                            Controller.HandleError(ex);

                            Controller.ShowMessage(ex.GetType().ToString());
                        }
                    }
                }
                finally
                {
                    btnPrint.Enabled = true;
                }
            }

            /*
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
            */
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {
           
            LoadReport();
        }

        private void rdCompleted_CheckedChanged(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void cmbYear_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void cmbMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void cbUserList_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadReport();
        }
    }
}
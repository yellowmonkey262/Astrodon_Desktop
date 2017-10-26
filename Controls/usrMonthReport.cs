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
            dgMonthly.AutoGenerateColumns = false;
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
                _Users.Insert(0, new IdValue()
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


        private void LoadReport()
        {

            var selectedYear = cmbYear.SelectedItem as IdValue;
            if (selectedYear == null)
                return;

            var selectedMonth = cmbMonth.SelectedItem as IdValue;
            if (selectedMonth == null)
                return;

            var dt = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);


            var idVal = (cbUserList.SelectedItem as IdValue);
            int selectedUserId = 0;
            if (idVal != null)
                selectedUserId = (cbUserList.SelectedItem as IdValue).Id;

            Cursor = Cursors.WaitCursor;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    IQueryable<MonthReport> query;
                    if (rdCompleted.Checked)
                    {
                        query = from m in context.tblMonthFins
                                join u in context.tblUsers on m.userID equals u.id into usr
                                from us in usr.DefaultIfEmpty()
                                join b in context.tblBuildings on m.buildingID equals b.Code
                                where m.completeDate != null
                                && m.findate == dt
                                && (selectedUserId == 0 || (us != null && us.id == selectedUserId))
                                select new MonthReport
                                {
                                    Building = b.Building,
                                    Code = b.Code,
                                    User = us == null ? string.Empty : us.name,
                                    FinDate = m.findate,
                                    CompletedDate = m.completeDate
                                };
                    }
                    else
                    {
                        query = from m in context.tblMonthFins
                                join u in context.tblUsers on m.userID equals u.id into usr
                                from us in usr.DefaultIfEmpty()
                                join b in context.tblBuildings on m.buildingID equals b.Code
                                where m.completeDate == null
                                && m.findate == dt
                                && (selectedUserId == 0 || (us != null && us.id == selectedUserId))
                                select new MonthReport
                                {
                                    Building = b.Building,
                                    Code = b.Code,
                                    User = us == null ? string.Empty : us.name,
                                    FinDate = m.findate,
                                    CompletedDate = m.completeDate
                                };
                    }

                    results.Clear();
                    foreach (MonthReport mr in query.OrderBy(a => a.Code).ToList())
                    {
                        results.Add(mr);
                    }
                    dgMonthly.Invalidate();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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
                            var reportData = reportService.MonthlyReport(SqlDataHandler.GetConnectionString(), dDate, rdCompleted.Checked, idVal != null ? idVal.Id : (int?)null);

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

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedYear = cmbYear.SelectedItem as IdValue;
            if (selectedYear == null)
                return;

            var selectedMonth = cmbMonth.SelectedItem as IdValue;
            if (selectedMonth == null)
                return;

            var dt = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);


            if (!Controller.AskQuestion("Are you sure you want top create the allocation for " + dt.ToString("MMM yyyy") + "?"))
                return;
            
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    //create a list of building id and user id
                    var userIdList = context.tblUsers
                                            .Where(a => a.ProcessCheckLists == true)
                                            .Select(a => a.id).ToList();

                    var buildingCodeList = context.tblBuildings.Select(a => a.Code).ToList();

                    Dictionary<string, int> randomAllocations = new Dictionary<string, int>();

                    //load the current allocations
                    foreach (var itm in context.tblMonthFins.Where(a => a.findate == dt)
                                                           .Select(a => new { a.buildingID, a.userID }).ToList())
                    {
                        randomAllocations.Add(itm.buildingID, itm.userID);
                        if (buildingCodeList.Contains(itm.buildingID))
                            buildingCodeList.Remove(itm.buildingID);
                    }

                    //get last months allocations
                    Dictionary<string, int> lastMonthsAllocations = new Dictionary<string, int>();
                    var lastMonth = dt.AddMonths(-1);
                    foreach (var itm in context.tblMonthFins.Where(a => a.findate == lastMonth)
                                                           .Select(a => new { a.buildingID, a.userID }).ToList())
                    {
                        lastMonthsAllocations.Add(itm.buildingID, itm.userID);
                    }

                    var userProcesslist = userIdList.ToList();
                    var random = new Random();
                    //bublle through buildings to build random dictionary
                    #region Random Allocations
                    while (buildingCodeList.Count > 0)
                    {
                        int userId = 0;
                        var buildingCode = buildingCodeList[0];

                        if (userProcesslist.Count > 1)
                        {
                            var idx = random.Next(0, userProcesslist.Count);

                            userId = userProcesslist[idx];
                            userProcesslist.Remove(userId);
                        }
                        else
                        {
                            if (userProcesslist.Count == 1)
                            {
                                userId = userProcesslist[0];
                                userProcesslist.Remove(userId);
                            }
                        }

                        if (userProcesslist.Count < 1)
                            userProcesslist = userIdList.ToList();

                        if (userId > 0)
                        {

                            if (lastMonthsAllocations.ContainsKey(buildingCode) && lastMonthsAllocations[buildingCode] == userId)
                            {
                                if (userProcesslist.Count > 1)
                                {
                                    var idx = random.Next(0, userProcesslist.Count);
                                    int newUserId = userProcesslist[idx];
                                    userProcesslist.Remove(newUserId);
                                    userProcesslist.Add(userId);
                                    if (newUserId > 0)
                                    {
                                        randomAllocations.Add(buildingCode, newUserId);
                                        buildingCodeList.Remove(buildingCode);
                                    }
                                }
                            }
                            else
                            {
                                if (userId > 0)
                                {
                                    randomAllocations.Add(buildingCode, userId);
                                    buildingCodeList.Remove(buildingCode);
                                }
                            }
                        }
                    }


                    #endregion

                    //Process dictionary to tblMonthFin

                    foreach (var key in randomAllocations.Keys)
                    {
                        var curr = context.tblMonthFins.Where(a => a.buildingID == key && a.findate == dt).SingleOrDefault();
                        if (curr == null)
                        {
                            curr = new Data.tblMonthFin()
                            {
                                buildingID = key,
                                findate = dt,
                                finPeriod = dt.Month,
                                year = dt.Year
                            };
                            context.tblMonthFins.Add(curr);
                        }
                        curr.userID = randomAllocations[key];
                    }

                    context.SaveChanges();

                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            LoadReport();

            Controller.ShowMessage("Random allocations completed");


        }
    }
}
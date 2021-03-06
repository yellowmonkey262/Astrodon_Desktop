﻿using Astro.Library.Entities;
using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;
using Astrodon.ReportService;
using System.IO;
using System.Diagnostics;
using Astrodon.Data;

namespace Astrodon.Controls
{
    public partial class usrMonthReport : UserControl
    {
        private List<tblBuilding> buildings;
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
                        where u.Active == true
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
            using (var context = SqlDataHandler.GetDataContext())
            {


                buildings = context.tblBuildings.Where(a => a.BuildingFinancialsEnabled
                                                         && a.BuildingDisabled == false
                                                         && (a.FinancialStartDate == null || a.FinancialStartDate <= DateTime.Today)
                        && (a.FinancialEndDate == null || a.FinancialEndDate >= DateTime.Today)
                                                         ).ToList();
            }
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
                                && b.BuildingDisabled == false
                                select new MonthReport
                                {
                                    Id = m.id,
                                    AdditionalComments = m.AdditionalComments,
                                    Building = b.Building,
                                    Code = b.Code,
                                    User = us == null ? string.Empty : us.name,
                                    FinDate = m.findate,
                                    CompletedDate = m.completeDate,
                                    Period = b.Period
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
                                && b.BuildingDisabled == false
                                && (selectedUserId == 0 || (us != null && us.id == selectedUserId))
                                select new MonthReport
                                {
                                    Id = m.id,
                                    AdditionalComments = m.AdditionalComments,
                                    Building = b.Building,
                                    Code = b.Code,
                                    User = us == null ? string.Empty : us.name,
                                    FinDate = m.findate,
                                    CompletedDate = m.completeDate,
                                    Period = b.Period
                                };
                    }

                    results.Clear();
                    foreach (MonthReport mr in query.OrderBy(a => a.Code).ToList())
                    {
                        mr.CalculatePeriod();
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


            if (!Controller.AskQuestion("Are you sure you want to create the allocation for " + dt.ToString("MMM yyyy") + "?"))
                return;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    //clear current allocations
                    var currentAllocations = context.tblMonthFins.Where(a => a.completeDate == null && a.findate == dt).ToList();
                    context.tblMonthFins.RemoveRange(currentAllocations);
                    context.SaveChanges();

                    //create a list of building id and user id
                    var userIdList = context.tblUsers
                                            .Where(a => a.ProcessCheckLists == true && a.Active == true)
                                            .Select(a => a.id).ToList();

                    var buildingCodeList = context.tblBuildings.Where(a => a.BuildingFinancialsEnabled == true && a.BuildingDisabled == false).Select(a => a.Code).ToList();

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
                        var old = currentAllocations.Where(a => a.buildingID == key && a.findate == dt).FirstOrDefault();

                        var prev = context.tblMonthFins.Where(a => a.buildingID == key && a.findate < dt).OrderByDescending(a => a.findate).FirstOrDefault();
                         
                        if (curr == null)
                        {
                            curr = new Data.tblMonthFin()
                            {
                                buildingID = key,
                                findate = dt,
                                finPeriod = dt.Month,
                                year = dt.Year,
                                AdditionalComments = prev != null ? prev.AdditionalComments : null
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

        private void dgMonthly_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var selectedItem = senderGrid.Rows[e.RowIndex].DataBoundItem as MonthReport;
                if(selectedItem != null)
                {
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var curr = context.tblMonthFins.Where(a => a.id == selectedItem.Id).SingleOrDefault();
                        if (curr != null)
                        {
                            curr.AdditionalComments = selectedItem.AdditionalComments;

                            //future
                            var future = context.tblMonthFins.Where(a => a.buildingID == curr.buildingID && a.findate > curr.findate).ToList();
                            foreach(var item in future)
                            {
                                item.AdditionalComments = selectedItem.AdditionalComments;
                            }

                            context.SaveChanges();
                            Controller.ShowMessage("Comment updated");
                        }


                    }
                }
            }

        }
    }
}
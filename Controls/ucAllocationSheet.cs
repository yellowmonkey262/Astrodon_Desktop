using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using System.IO;
using System.Diagnostics;
using OfficeOpenXml;

namespace Astrodon.Controls
{
    public partial class ucAllocationSheet : UserControl
    {
        public ucAllocationSheet()
        {
            InitializeComponent();

            LoadAllocations();
        }

        List<AllocationItem> _Data;
        private void LoadAllocations()
        {
            _Data = new List<AllocationItem>();
            using (var context = SqlDataHandler.GetDataContext())
            {
                var userList = context.tblUsers.Where(a => a.ProcessCheckLists).ToList();
                foreach (var user in userList.OrderBy(a => a.name))
                {
                    _Data.AddRange(ProcessAllocation(context, user, 6)); 
                    Application.DoEvents();
                }
            }
            BindDataGrid();
        }

        private List<AllocationItem> ProcessAllocation(DataContext context, tblUser user, int buildingsToAllocate)
        {
            List<AllocationItem> result = new List<AllocationItem>();
            //find the buildings allocated to this user to process check lists for

            var query = from m in context.tblMonthFins
                        join u in context.tblUsers on m.userID equals u.id
                        join b in context.tblBuildings on m.buildingID equals b.Code
                        where m.completeDate == null
                        && b.BuildingDisabled == false
                        && u.id == user.id
                        select new
                        {
                            Building = b,
                            Financial = m
                        };

            var myBuildingsToProcess = query.OrderBy(a => a.Financial.findate).ToList();

            var buildingIds = myBuildingsToProcess.Select(a => a.Building.id).Distinct().ToArray();

            var dtStart = DateTime.Now;
            var dtEnd = dtStart.AddHours(72);

            //find all calendar entries for these buildings > today and < today + 72 hours
            var calendarEntries = from c in context.BuildingCalendarEntrySet
                                  where buildingIds.Contains(c.BuildingId)
                                  && c.EntryDate >= dtStart
                                  && c.EntryDate <= dtEnd
                                  select new
                                  {
                                      c.BuildingId,
                                      c.Building.Building,
                                      c.Building.Code,
                                      c.EntryDate,
                                      c.Event
                                  };



            foreach (var itm in calendarEntries.OrderBy(a => a.EntryDate).ToList())
            {
                var existing = result.FirstOrDefault(a => a.BuildingId == itm.BuildingId);
                if (existing == null)
                {
                    int priortiy = result.Count + 1;
                    result.Add(new AllocationItem()
                    {
                        BuildingId = itm.BuildingId,
                        BuildingName = itm.Building,
                        BuildingCode = itm.Code,
                        Priority = priortiy,
                        UserId = user.id,
                        UserName = user.name,
                        DayOfMonth = itm.EntryDate.AddHours(-72).Date
                    });
                }
            }

            //next allocate all other buildings not yet in the list
            buildingIds = result.Select(a => a.BuildingId).Distinct().ToArray();
            List<AllocationItem> randomBuildings = new List<AllocationItem>();

            foreach (var itm in myBuildingsToProcess
                                        .Where(a => !buildingIds.Contains(a.Building.id))
                                        .OrderBy(a => a.Financial.findate)
                                        .Select(a => a))
            {
                var existing = result.FirstOrDefault(a => a.BuildingId == itm.Building.id);
                if (existing == null)
                {

                    var finDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, itm.Building.FinancialDayOfMonth);
                    int priority = Math.Abs( (DateTime.Today - finDate).Days);


                    randomBuildings.Add(new AllocationItem()
                    {
                        BuildingId = itm.Building.id,
                        BuildingName = itm.Building.Building,
                        BuildingCode = itm.Building.Code,
                        Priority = priority,
                        UserId = user.id,
                        UserName = user.name,
                        DayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month,itm.Building.FinancialDayOfMonth)
                    });
                }
            }

            foreach(var itm in randomBuildings.OrderBy(a => a.Priority))
            {
                itm.Priority = result.Count() + 1;
                result.Add(itm);
            }


            return result.OrderBy(a => a.Priority).Take(buildingsToAllocate).ToList();

        }

        private void BindDataGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            var dateTimeColumnStyle = new DataGridViewCellStyle();
            dateTimeColumnStyle.Format = "yyyy/MM/dd HH:mm";

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";


            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _Data;

            dgItems.Columns.Clear();
            dgItems.ReadOnly = false;
            dgItems.EditMode = DataGridViewEditMode.EditOnEnter;


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UserName",
                HeaderText = "Name",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Priority",
                HeaderText = "Priority",
                ReadOnly = true,
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Building",
                DataPropertyName = "BuildingCode",
                ReadOnly = true,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingName",
                HeaderText = "Building Name",
                ReadOnly = false
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DayOfMonth",
                HeaderText = "Deadline",
                ReadOnly = false,
                DefaultCellStyle = dateColumnStyle
            });


            dgItems.DataSource = bs;

            dgItems.AutoResizeColumns();
        }


        class AllocationItem
        {
            public int UserId { get; set; }
            public string UserName { get; set; }

            public int Priority { get; set; }

            public string BuildingCode { get; set; }
            public int BuildingId { get; set; }
            public string BuildingName { get; set; }

            public DateTime DayOfMonth { get; set; }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                btnPrint.Enabled = false;
                try
                {
                    try
                    {

                        var excelReport = CreateExcelFile();
                        File.WriteAllBytes(dlgSave.FileName, excelReport);
                        Process.Start(dlgSave.FileName);
                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);

                        Controller.ShowMessage(ex.GetType().ToString());
                    }
                }
                finally
                {
                    btnPrint.Enabled = true;
                }
            }

        }

        private byte[] CreateExcelFile()
        {
            byte[] result = null;
            using (var memStream = new MemoryStream())
            {
                using (ExcelPackage excelPkg = new ExcelPackage())
                {

                    using (ExcelWorksheet wsSheet1 = excelPkg.Workbook.Worksheets.Add("Debtors"))
                    {

                        wsSheet1.Cells["A1"].Value = "Name";
                        wsSheet1.Cells["A1"].Style.Font.Bold = true;

                        wsSheet1.Cells["B1"].Value = "Priority";
                        wsSheet1.Cells["B1"].Style.Font.Bold = true;

                        wsSheet1.Cells["C1"].Value = "Building";
                        wsSheet1.Cells["C1"].Style.Font.Bold = true;

                        wsSheet1.Cells["D1"].Value = "Building Name";
                        wsSheet1.Cells["D1"].Style.Font.Bold = true;

                        //wsSheet1.Cells["E1"].Value = "Period";
                        //wsSheet1.Cells["E1"].Style.Font.Bold = true;

                        //wsSheet1.Cells["F1"].Value = "Meeting";
                        //wsSheet1.Cells["F1"].Style.Font.Bold = true;

                        //wsSheet1.Cells["G1"].Value = "Event";
                        //wsSheet1.Cells["G1"].Style.Font.Bold = true;

                        int rowNum = 1;
                        foreach (var row in _Data.OrderBy(a => a.UserName).ThenBy(a => a.Priority).ToList())
                        {
                            rowNum++;
                            wsSheet1.Cells["A" + rowNum.ToString()].Value = row.UserName;
                            wsSheet1.Cells["B" + rowNum.ToString()].Value = row.Priority;
                            wsSheet1.Cells["C" + rowNum.ToString()].Value = row.BuildingCode;
                            wsSheet1.Cells["D" + rowNum.ToString()].Value = row.BuildingName;

                            //wsSheet1.Cells["E" + rowNum.ToString()].Style.Numberformat.Format = "yyyy/MM/dd";
                            //wsSheet1.Cells["E" + rowNum.ToString()].Value = row.FinancialPeriod;

                            //wsSheet1.Cells["F" + rowNum.ToString()].Style.Numberformat.Format = "yyyy/MM/dd HH:mm";
                            //wsSheet1.Cells["F" + rowNum.ToString()].Value = row.MeetingDate;

                            //wsSheet1.Cells["G" + rowNum.ToString()].Value = row.MeetingType;
                        }


                        wsSheet1.Protection.IsProtected = false;
                        wsSheet1.Protection.AllowSelectLockedCells = false;
                        wsSheet1.Cells.AutoFitColumns();

                        excelPkg.SaveAs(memStream);
                        memStream.Flush();
                        result = memStream.ToArray();
                    }
                }
            }
            return result;
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using Astrodon.Data.Base;
using System.Globalization;
using Astrodon.Data;
using Astro.Library.Entities;
using Astrodon.Data.Calendar;

namespace Astrodon.Reports.Calendar
{
    public partial class ucPrintCalendar : UserControl
    {
        private List<IdValue> _Years;
        private List<IdValue> _Months;
        private List<IdValue> _PMUsers;
        private List<Building> _Buildings;
        private DateTime _ReportDate = DateTime.Today;

        private BuildingCalendarEntry _Item = null;
        private List<CalendarPrintItem> _Data;

        public ucPrintCalendar()
        {
            InitializeComponent();
            LoadYears();
            LoadPMUsers();
            LoadBuildings();

            cbFilterPM_CheckedChanged(this, EventArgs.Empty);

            dtpEventDate.MinDate = DateTime.Today.AddYears(-2);
            dtpEventDate.MaxDate = DateTime.Today.AddYears(2);

            dtpEventDate.Format = DateTimePickerFormat.Custom;
            dtpEventDate.CustomFormat = "yyyy/MM/dd";

            dtpEventTime.Format = DateTimePickerFormat.Time;
            dtpEventTime.ShowUpDown = true;

            LoadGrid();

        }

        private void LoadBuildings()
        {
            var userid = Controller.user.id;
            Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
            _Buildings = bManager.buildings;
            cbBuilding.DataSource = _Buildings;
            cbBuilding.ValueMember = "ID";
            cbBuilding.DisplayMember = "Name";
        }

        private void LoadPMUsers()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var pmsQ = from b in context.tblBuildings
                           join u in context.tblUsers on b.pm equals u.email
                           where b.BuildingDisabled == false
                           select new IdValue()
                           {
                               Id = u.id,
                               Value = u.name
                           };

                _PMUsers = pmsQ.Distinct().OrderBy(a => a.Value).ToList();
            }

            cbPM.DataSource = _PMUsers;
            cbPM.ValueMember = "Id";
            cbPM.DisplayMember = "Value";
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

        Dictionary<int, string> _CalendarData;
        private void pdocCalendar_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var calendarReport = new CalendarReport();
            calendarReport.DrawCalendar(e.Graphics, e.MarginBounds, _ReportDate, _CalendarData);
        }

        private void pdocCalendar_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.Landscape = true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            _ReportDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
            _CalendarData = new Dictionary<int, string>();

            var dtFrom = new DateTime(_ReportDate.Year, _ReportDate.Month, 1);
            var dtTo = dtFrom.AddMonths(1).AddSeconds(-1);

            int? pmId = null;
            if (cbFilterPM.Checked)
            {
                pmId = (cbPM.SelectedItem as IdValue).Id;
            }

            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from c in context.BuildingCalendarEntrySet
                        where c.EntryDate >= dtFrom
                           && c.EntryDate <= dtTo
                           && (pmId == null || c.UserId == pmId)
                        select new CalendarPrintItem
                        {
                            Id = c.id,
                            EventDate = c.EntryDate,
                            BuildingId = c.BuildingId,
                            BuildingName = c.Building.Building,
                            Event = c.Event,
                            Venue = c.Venue,
                            PM = c.User.name
                        };

                var entries = q.ToList();

                for (int x = 1; x <= 31; x++)
                {
                    var events = entries.Where(a => a.EventDate.Day == x).ToList();
                    if (events.Count() == 1)
                    {
                        _CalendarData.Add(x, events[0].ToString());

                    }
                    else if (events.Count() > 1)
                    {
                        string entryText = string.Empty;
                        foreach (var entry in events.OrderBy(a => a.EventDate).ThenBy(a => a.BuildingName))
                        {
                            if (entryText.Length > 0)
                                entryText = entryText + "\n" + entry.ToString();
                            else
                                entryText = entry.ToString();
                        }
                        _CalendarData.Add(x, entryText);
                    }
                }
            }

            if (_CalendarData.Keys.Count > 0)
            {
                printDialog1.Document = pdocCalendar;
                DialogResult result = printDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pdocCalendar.Print();
                }
            }
            else
            {
                Controller.ShowMessage("No data to print");
            }
        }

        private void cbFilterPM_CheckedChanged(object sender, EventArgs e)
        {
            cbPM.Enabled = cbFilterPM.Checked;
        }

       
        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            _Data = null;
            _Item = null;
             LoadCalendarData();
            GotoReadOnly();
        }

        private void LoadCalendarData()
        {
            if (cmbMonth.SelectedItem == null || cmbYear.SelectedItem == null)
                return;

            var dtFrom = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
            var dtTo = dtFrom.AddMonths(1).AddSeconds(-1);

            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from c in context.BuildingCalendarEntrySet
                        where c.EntryDate >= dtFrom
                           && c.EntryDate <= dtTo
                        select new CalendarPrintItem
                        {
                            Id = c.id,
                            EventDate = c.EntryDate,
                            BuildingId = c.BuildingId,
                            BuildingName = c.Building.Building,
                            Event = c.Event,
                            Venue = c.Venue,
                            PM = c.User.name
                        };

                _Data = q.OrderBy(a => a.EventDate).ThenBy(a => a.BuildingName).ToList();
            }

            BindDataGrid();
        }

        private void BindDataGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = _Data;

            dgItems.Columns.Clear();

            dgItems.DataSource = bs;

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true
            });
            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DisplayDate",
                HeaderText = "Date",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PM",
                HeaderText = "PM",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingName",
                HeaderText = "Building",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Event",
                HeaderText = "Event",
                ReadOnly = true
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Venue",
                HeaderText = "Venue",
                ReadOnly = true
            });

            dgItems.AutoResizeColumns();
        }

        private void GotoReadOnly()
        {
            if (cmbYear.SelectedItem != null && cmbMonth.SelectedItem != null)
                dtpEventDate.Value = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
            else
                dtpEventDate.Value = DateTime.Today;

            dtpEventTime.Value = dtpEventDate.Value + new TimeSpan(08, 00, 00);
            cbEvent.SelectedIndex = -1;
            tbVenue.Text = string.Empty;

            cbBuilding.Enabled = false;
            dtpEventDate.Enabled = false;
            dtpEventTime.Enabled = false;
            cbEvent.Enabled = false;
            tbVenue.Enabled = false;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnNew.Visible = true;
            dgItems.Enabled = true;

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _Item = null;
            GotoReadOnly();

            cbBuilding.Enabled = true;
            dtpEventDate.Enabled = true;
            dtpEventTime.Enabled = true;
            cbEvent.Enabled = true;
            tbVenue.Enabled = true;

            btnSave.Visible = true;
            btnCancel.Visible = true;

            btnNew.Visible = false;
            dgItems.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var selectedBuilding = (cbBuilding.SelectedItem as Building);

            if (String.IsNullOrWhiteSpace(cbEvent.Text)
               || String.IsNullOrWhiteSpace(tbVenue.Text)
               || selectedBuilding == null)
            {
                Controller.HandleError("Please complete all fields", "Validation Error");
                return;
            }
            var minTime = new TimeSpan(05, 00, 00);
            var maxTime = new TimeSpan(23, 59, 00);

            if(dtpEventTime.Value.TimeOfDay < minTime || dtpEventTime.Value.TimeOfDay > maxTime)
            {
                Controller.HandleError("Event time must be > 05:00 and < 23:59", "Validation Error");
                return;
            }

            using (var context = SqlDataHandler.GetDataContext())
            {
                BuildingCalendarEntry editItem = null;
                if (_Item == null)
                {
                    editItem = new BuildingCalendarEntry();
                    context.BuildingCalendarEntrySet.Add(editItem);
                }
                else
                {
                    editItem = context.BuildingCalendarEntrySet.Single(a => a.id == _Item.id);
                }

                editItem.BuildingId = selectedBuilding.ID;

                var pm = (from b in context.tblBuildings
                          join u in context.tblUsers on b.pm equals u.email
                          where b.id == editItem.BuildingId
                          select u).SingleOrDefault();

                if (pm != null)
                    editItem.UserId = pm.id;
                else if (editItem.id == 0)
                    editItem.UserId = Controller.user.id;

                editItem.EntryDate = dtpEventDate.Value.Date + dtpEventTime.Value.TimeOfDay;
                editItem.Event = cbEvent.Text;
                editItem.Venue = tbVenue.Text;

                context.SaveChanges();
            }

            LoadGrid();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var i = senderGrid.Rows[e.RowIndex].DataBoundItem as CalendarPrintItem;
                if (i != null)
                {

                    if (e.ColumnIndex == 0)
                    {
                        EditItem(i.Id);
                    }
                    else if(e.ColumnIndex == 1)
                    {
                        DeleteItem(i.Id);
                    }

                }
            }
        }

        private void DeleteItem(int id)
        {
            if (Controller.AskQuestion("Are you sure you want to delete this entry?"))
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    var itm = context.BuildingCalendarEntrySet.Single(a => a.id == id);
                    context.BuildingCalendarEntrySet.Remove(itm);
                    context.SaveChanges();
                }
                LoadGrid();
                GotoReadOnly();
            }
        }

        private void EditItem(int id)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                _Item = context.BuildingCalendarEntrySet.Single(a => a.id == id);
                cbBuilding.SelectedItem = _Buildings.Where(a => a.ID == _Item.BuildingId);
                dtpEventDate.Value = _Item.EntryDate;
                dtpEventTime.Value = _Item.EntryDate;
                cbEvent.Text = _Item.Event;
                tbVenue.Text = _Item.Venue;


                cbBuilding.Enabled = true;
                dtpEventDate.Enabled = true;
                dtpEventTime.Enabled = true;
                cbEvent.Enabled = true;
                tbVenue.Enabled = true;

                btnSave.Visible = true;
                btnCancel.Visible = true;

                btnNew.Visible = false;
                dgItems.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GotoReadOnly();
        }

        class CalendarPrintItem
        {
            public int BuildingId { get; set; }
            public string BuildingName { get; set; }
            public string Event { get; set; }
            public DateTime EventDate { get; set; }
            public int Id { get; internal set; }
            public string PM { get; internal set; }
            public string Venue { get; set; }

            public override string ToString()
            {
                return EventDate.ToString("HH:mm", CultureInfo.InvariantCulture) + " " + BuildingName + "-" + Event + "-" + PM + "-" + Venue;
            }

            public string DisplayDate
            {
                get
                {
                    return EventDate.ToString("yyyy/MM/dd HH:mm");
                }
            }
        }

    }
}

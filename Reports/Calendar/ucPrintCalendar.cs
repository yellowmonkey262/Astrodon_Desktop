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

            dtpEventToDate.MinDate = DateTime.Today.AddYears(-2);
            dtpEventToDate.MaxDate = DateTime.Today.AddYears(2);

            dtpEventToDate.Format = DateTimePickerFormat.Custom;
            dtpEventDate.CustomFormat = "yyyy/MM/dd";

            dtpEventToTime.Format = DateTimePickerFormat.Time;
            dtpEventToTime.ShowUpDown = true;


            dtpEventDate.Value = DateTime.Today;
            dtpEventTime.Value = DateTime.Now;

            dtpEventToDate.Value = DateTime.Today;
            dtpEventToTime.Value = DateTime.Now;

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
            cmbYear.SelectedValue = DateTime.Now.Year;

            cmbMonth.DataSource = _Months;
            cmbMonth.ValueMember = "Id";
            cmbMonth.DisplayMember = "Value";
            cmbMonth.SelectedValue = DateTime.Now.Month;

            

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
            if (cbFilterPM.Checked && cbPM.SelectedItem != null)
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
                            BuildingAbreviation = c.Building.Code,
                            BuildingDataPath = c.Building.DataPath,
                            Event = c.Event,
                            Venue = c.Venue,
                            PM = c.User.name,
                            NotifyTrustees = c.NotifyTrustees,
                            InviteSubject = c.InviteSubject,
                            InviteBody = c.InviteBody,
                            BCCEmailAddress = c.BCCEmailAddress,
                            TrusteesNotified = c.TrusteesNotified
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
            LoadGrid();
        }


        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void cbPM_SelectedIndexChanged(object sender, EventArgs e)
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

            int pmId = 0;
            if (cbFilterPM.Checked && cbPM.SelectedItem != null)
            {
                pmId = (cbPM.SelectedItem as IdValue).Id;
            }


            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from c in context.BuildingCalendarEntrySet
                        where c.EntryDate >= dtFrom
                           && c.EntryDate <= dtTo
                           && (pmId == 0 || c.UserId == pmId)
                        select new CalendarPrintItem
                        {
                            Id = c.id,
                            EventDate = c.EntryDate,
                            EventToDate = c.EventToDate,
                            BuildingId = c.BuildingId,
                            BuildingAbreviation = c.Building.Code,
                            BuildingDataPath = c.Building.DataPath,
                            BuildingName = c.Building.Building,
                            Event = c.Event,
                            Venue = c.Venue,
                            PM = c.User.name,
                            PMEmail = c.User.email,
                            NotifyTrustees = c.NotifyTrustees,
                            InviteSubject = c.InviteSubject,
                            InviteBody = c.InviteBody,
                            BCCEmailAddress = c.BCCEmailAddress,
                            TrusteesNotified = c.TrusteesNotified

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
            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Invite",
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
                DataPropertyName = "NotifyTrusteesStr",
                HeaderText = "Notify Trustees",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TrusteesNotifiedStr",
                HeaderText = "Trustees Notified",
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

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "InviteSubject",
                HeaderText = "Subject",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "InviteBody",
                HeaderText = "Body",
                ReadOnly = true
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BCCEmailAddress",
                HeaderText = "BCC",
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


            dtpEventToDate.Enabled = false;
            dtpEventToTime.Enabled = false;

            cbNotifyTrustees.Enabled = false;
            tbBCC.Enabled = false;
            tbSubject.Enabled = false;
            tbBodyContent.Enabled = false;

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

            dtpEventDate.Value = DateTime.Today;
            dtpEventTime.Value = DateTime.Now;

            dtpEventToDate.Value = DateTime.Today;
            dtpEventToTime.Value = DateTime.Now;

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

            if (dtpEventTime.Value.TimeOfDay < minTime || dtpEventTime.Value.TimeOfDay > maxTime)
            {
                Controller.HandleError("Event time must be > 05:00 and < 23:59", "Validation Error");
                return;
            }


            if (dtpEventToTime.Value.TimeOfDay < minTime || dtpEventToTime.Value.TimeOfDay > maxTime)
            {
                Controller.HandleError("Event time must be > 05:00 and < 23:59", "Validation Error");
                return;
            }

            if (dtpEventDate.Value.Date < DateTime.Today)
            {
                Controller.HandleError("Cannot schedule an event in the past.", "Validation Error");
                return;
            }

            if(!string.IsNullOrWhiteSpace(tbBCC.Text))
            {
                if(!tbBCC.Text.Contains("@") || !tbBCC.Text.Contains("."))
                {
                    Controller.HandleError("BCC not a valid email address.", "Validation Error");
                    return;
                }
            }

            DateTime fromDate = dtpEventDate.Value.Date + dtpEventTime.Value.TimeOfDay;
            DateTime toDate = dtpEventToDate.Value.Date + dtpEventToTime.Value.TimeOfDay;

            if (fromDate >= toDate)
            {
                Controller.HandleError("To Date/Time cannot be more than the From Date/Time.", "Validation Error");
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

                editItem.EntryDate = fromDate;
                editItem.EventToDate = toDate;
                editItem.Event = cbEvent.Text;
                editItem.Venue = tbVenue.Text;
                editItem.NotifyTrustees = cbNotifyTrustees.Checked;
                editItem.InviteSubject = tbSubject.Text;
                editItem.BCCEmailAddress = tbBCC.Text;
                editItem.InviteBody = tbBodyContent.Text;

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
                    else if (e.ColumnIndex == 1)
                    {
                        DeleteItem(i.Id);
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        SendCalenderInvite(i);
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
                if (_Item != null)
                {
                    dtpEventToDate.Value = _Item.EventToDate;
                    dtpEventToTime.Value = _Item.EventToDate;
                }

                cbEvent.Text = _Item.Event;
                tbVenue.Text = _Item.Venue;
                cbNotifyTrustees.Checked = _Item.NotifyTrustees;
                tbBCC.Text = _Item.BCCEmailAddress;
                tbSubject.Text = _Item.InviteSubject;
                tbBodyContent.Text = _Item.InviteBody;


                cbBuilding.Enabled = true;
                dtpEventDate.Enabled = true;
                dtpEventTime.Enabled = true;
                cbEvent.Enabled = true;
                tbVenue.Enabled = true;

                dtpEventToDate.Enabled = true;
                dtpEventToTime.Enabled = true;

                cbNotifyTrustees.Enabled = true;
                tbBCC.Enabled = true;
                tbSubject.Enabled = true;
                tbBodyContent.Enabled = true;


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


        private byte[] CreateCalendarInvite(string subject, string description,
            string location, DateTime startDate, DateTime endDate)
        {
            string contents =
                "BEGIN:VCALENDAR" + "\n" +
                "PRODID:-//" + "HCC" + "//" + "HealthCloud" + "//EN" + "\n" +
                "BEGIN:VEVENT" + "\n" +
                "DTSTART:" + startDate.ToString("yyyyMMdd\\THHmmss") + "\n" +
                "DTEND:" + endDate.ToString("yyyyMMdd\\THHmmss") + "\n" +
                "LOCATION:" + location + "\n" +
                "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + description + "\n" +
                "SUMMARY:" + subject + "\n" +
                "PRIORITY:3" + "\n" +
                "END:VEVENT" + "\n" +
                "END:VCALENDAR";
            return System.Text.Encoding.UTF8.GetBytes(contents.ToString());
        }

        private void SendCalenderInvite(CalendarPrintItem entry)
        {
            string status;
            if (string.IsNullOrWhiteSpace(entry.PMEmail))
            {
                Controller.HandleError("Email address not found for " + entry.PM);
                return;
            }

            var calendarInvite = CreateCalendarInvite(entry.BuildingName + " - " + entry.Event, "", entry.Venue, entry.EventDate, entry.EventToDate != null ? entry.EventToDate.Value: entry.EventDate.AddHours(2));
            Dictionary<string, byte[]> attachments = new Dictionary<string, byte[]>();
            attachments.Add("Appointment.ics", calendarInvite);
            string subject = entry.InviteSubject;
            if (String.IsNullOrWhiteSpace(subject))
                subject = entry.BuildingName + " - " + entry.Event;

            string bodyContent = entry.InviteBody;
            if (String.IsNullOrWhiteSpace(bodyContent))
                bodyContent = "Invite generated by " + Controller.user.name + " using the Astrodon System";

            string bccEmail = entry.BCCEmailAddress;
            if (bccEmail == string.Empty)
                bccEmail = null;


            if (!Mailer.SendMailWithAttachments("noreply@astrodon.co.za", new string[] { entry.PMEmail },
                subject, bodyContent,
                false, false, false, out status, attachments, bccEmail))
            {
                Controller.HandleError("Error seding email " + status, "Email error");
            }

            if (entry.NotifyTrustees)
            {
                var customers = Controller.pastel.AddCustomers(entry.BuildingAbreviation, entry.BuildingDataPath);
                var trustees = customers.Where(a => a.IsTrustee).ToList();
                if (trustees.Count() > 0 && Controller.AskQuestion("Are you sure you want to send the invite to " + trustees.Count().ToString() + " trustees?"))
                {
                    foreach (var trustee in trustees)
                    {
                        if (trustee.Email != null && trustee.Email.Length > 0)
                        {
                            if (!Mailer.SendMailWithAttachments("noreply@astrodon.co.za", trustee.Email,
                                 subject, bodyContent, false, false, false, out status, attachments, bccEmail))
                            {
                                Controller.HandleError("Error seding email " + status, "Email error");
                            }
                        }
                    }
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var itm = context.BuildingCalendarEntrySet.Single(a => a.id == entry.Id);
                        itm.TrusteesNotified = true;
                        entry.TrusteesNotified = true;
                        context.SaveChanges();
                        BindDataGrid();
                    }
                }

            }
        }

  
        class CalendarPrintItem
        {

            public int BuildingId { get; set; }
            public string BuildingName { get; set; }
            public string BuildingAbreviation { get; set; }
            public string BuildingDataPath { get; set; }
            public string Event { get; set; }
            public DateTime EventDate { get; set; }
            public DateTime? EventToDate { get; set; }

            public int Id { get; internal set; }
            public string PM { get; internal set; }
            public string Venue { get; set; }

            public override string ToString()
            {
                return EventDate.ToString("HH:mm", CultureInfo.InvariantCulture) + "-" 
                     + EventToDate.Value.ToString("HH:mm", CultureInfo.InvariantCulture) 
                     + " " + BuildingName 
                     + "-" + Event 
                     + "-" 
                     + PM + "-" 
                     + Venue;
            }

            public string DisplayDate
            {
                get
                {
                    return EventDate.ToString("yyyy/MM/dd HH:mm");
                }
            }

            public string PMEmail { get; set; }
            public bool NotifyTrustees { get;  set; }
            public string NotifyTrusteesStr { get { return NotifyTrustees ? "Yes" : "No"; } }

            public bool TrusteesNotified { get; set; }
            public string TrusteesNotifiedStr { get { return TrusteesNotified ? "Yes" : "No"; } }

            public string InviteSubject { get;  set; }
            public string InviteBody { get; internal set; }
            public string BCCEmailAddress { get; internal set; }
        }

    }
}

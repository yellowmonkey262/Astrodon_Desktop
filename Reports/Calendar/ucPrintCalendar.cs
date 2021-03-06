﻿using System;
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
using System.Data.Entity;
using iTextSharp.text.pdf;
using System.IO;
using Astrodon.ClientPortal;

namespace Astrodon.Reports.Calendar
{
    public partial class ucPrintCalendar : UserControl
    {
        private List<IdValue> _Years;
        private List<IdValue> _Months;
        private List<IdValue> _PMUsers;
        private List<tblUser> _Users;
        private List<Building> _Buildings;
        private List<IdValue> _MeetingRooms;

        private DateTime _ReportDate = DateTime.Today;

        private BuildingCalendarEntry _Item = null;
        byte[] _FileToLoad = null;

        private List<CalendarPrintItem> _Data;

        public ucPrintCalendar()
        {
            InitializeComponent();
            LoadYears();
            LoadPMUsers();
            LoadBuildings();
            LoadAllUsers();
            LoadMeetingRooms();

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

        private void LoadMeetingRooms()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var rooms = context.MeetingRoomSet.Where(a => a.Active).ToList();
                _MeetingRooms = rooms.Select(a => new IdValue()
                {
                    Id = a.id,
                    Value = a.ToString()
                }).OrderBy(a => a.Value).ToList();
            }

            cbFilterRoom.DataSource = _MeetingRooms;
            cbFilterRoom.ValueMember = "Id";
            cbFilterRoom.DisplayMember = "Value";

            cbRoom.DataSource = _MeetingRooms;
            cbRoom.ValueMember = "Id";
            cbRoom.DisplayMember = "Value";

            
        }

        private void LoadAllUsers()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var users = from u in context.tblUsers
                            where u.Active
                           select u;

                _Users = users.OrderBy(a => a.name).ToList();
            }

            cbUserInvites.DataSource = _Users;
            cbUserInvites.DisplayMember = "name";
            cbUserInvites.ValueMember = "id";
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

            int? roomId = null;
            if(cbRoomFilter.Checked && cbFilterRoom.SelectedItem != null)
            {
                roomId = (cbFilterRoom.SelectedItem as IdValue).Id;
            }


            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from c in context.BuildingCalendarEntrySet
                        where c.EntryDate >= dtFrom
                           && c.EntryDate <= dtTo
                           && (pmId == null || c.UserId == pmId)
                           && (roomId == null || c.MeetingRoomId == roomId)
                        select new CalendarPrintItem
                        {
                            Id = c.id,
                            EventDate = c.EntryDate,
                            BuildingId = c.BuildingId,
                            BuildingName = c.BuildingId != null ? c.Building.Building : string.Empty,
                            BuildingAbreviation = c.BuildingId != null ? c.Building.Code : string.Empty,
                            BuildingDataPath = c.BuildingId != null ? c.Building.DataPath : string.Empty,
                            Event = c.Event,
                            Venue = c.Venue,
                            PM = c.User.name,
                            NotifyTrustees = c.NotifyTrustees,
                            InviteSubject = c.InviteSubject,
                            InviteBody = c.InviteBody,
                            BCCEmailAddress = c.BCCEmailAddress,
                            TrusteesNotified = c.TrusteesNotified,
                            EventyType = c.CalendarEntryType
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


        private void rbFinancial_CheckedChanged(object sender, EventArgs e)
        {
            LoadGrid();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LoadGrid();

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
            if (_DisableGridLoad)
                return;

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

            CalendarEntryType entryType = CalendarEntryType.Financial;
            if (rbStaff.Checked)
                entryType = CalendarEntryType.Staff;


            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from c in context.BuildingCalendarEntrySet
                        join i in context.CalendarEntryAttachmentSet on c.id equals i.BuildingCalendarEntryId into attach
                        from a in attach.DefaultIfEmpty()
                        where c.EntryDate >= dtFrom
                           && c.EntryDate <= dtTo
                           && (pmId == 0 || c.UserId == pmId)
           //                && c.CalendarEntryType == entryType
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
                            TrusteesNotified = c.TrusteesNotified,
                            FileName = a != null ? a.FileName : string.Empty,
                            EventyType = c.CalendarEntryType,
                            Room = c.MeetingRoomId != null ? c.MeetingRoom.Name : string.Empty

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
                DataPropertyName = "Room",
                HeaderText = "Room",
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


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FileName",
                HeaderText = "Attachment",
                ReadOnly = true
            });

           // dgItems.AutoResizeColumns();
        }

        
        private void GotoReadOnly()
        {
            if (cmbYear.SelectedItem != null && cmbMonth.SelectedItem != null)
                dtpEventDate.Value = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
            else
                dtpEventDate.Value = DateTime.Today;

            dtpEventTime.Value = dtpEventDate.Value + new TimeSpan(08, 00, 00);
            cbEvent.SelectedIndex = -1;
            cbRoom.SelectedIndex = -1;
            tbVenue.Text = string.Empty;

            cbBuilding.Enabled = false;
            dtpEventDate.Enabled = false;
            dtpEventTime.Enabled = false;
            cbEvent.Enabled = false;
            cbRoom.Enabled = false;
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
            btnUpload.Enabled = false;

            cbBuilding.Enabled = false;
            cbUserInvites.Enabled = false;

            for (int x = 0; x < cbUserInvites.Items.Count; x++)
                    cbUserInvites.SetItemChecked(x, false);

            if (rbStaff.Checked)
            {
                cbBuilding.Visible = false;
                cbPM.Visible = false;
                cbFilterPM.Visible = false;
                label3.Visible = false;
                cbUserInvites.Visible = true;
                label4.Visible = false;
                label8.Visible = false;
                cbNotifyTrustees.Visible = false;
                cbNotifyTrustees.Checked = false;
            }
            else
            {
                cbBuilding.Visible = true;
                cbPM.Visible = true;
                cbFilterPM.Visible = true;
                label3.Visible = true;
                cbUserInvites.Visible = false;

                label4.Visible = true;
                label8.Visible = true;
                cbNotifyTrustees.Visible = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _Item = null;
            _FileToLoad = null;
            GotoReadOnly();

            btnUpload.Enabled = true;

            cbBuilding.Enabled = true;
            dtpEventDate.Enabled = true;
            dtpEventTime.Enabled = true;
            cbEvent.Enabled = true;
            cbRoom.Enabled = true;
            tbVenue.Enabled = true;

            btnSave.Visible = true;
            btnCancel.Visible = true;

            btnNew.Visible = false;
            dgItems.Enabled = false;

            dtpEventDate.Value = DateTime.Today;
            dtpEventTime.Value = DateTime.Now;

            dtpEventToDate.Value = DateTime.Today;
            dtpEventToTime.Value = DateTime.Now;

            dtpEventToDate.Enabled = true;
            dtpEventToTime.Enabled = true;

            cbNotifyTrustees.Enabled = true;
            tbBCC.Enabled = true;
            tbSubject.Enabled = true;
            tbBodyContent.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Building selectedBuilding = null;

            if (String.IsNullOrWhiteSpace(cbEvent.Text)
               || String.IsNullOrWhiteSpace(tbVenue.Text)
               )
            {
                Controller.HandleError("Please complete all fields", "Validation Error");
                return;
            }

            if(rbFinancial.Checked)
            {
                selectedBuilding = (cbBuilding.SelectedItem as Building);
                if(selectedBuilding == null)
                {
                    Controller.HandleError("Building required", "Validation Error");
                    return;
                }
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
                    editItem = context.BuildingCalendarEntrySet.Include(a => a.UserInvites).Single(a => a.id == _Item.id);
                    foreach (var itm in editItem.UserInvites.ToList())
                        context.CalendarUserInviteSet.Remove(itm);
                }

                if (selectedBuilding != null)
                    editItem.BuildingId = selectedBuilding.ID;
                else
                    editItem.BuildingId = null;
                if (rbStaff.Checked)
                    editItem.CalendarEntryType = CalendarEntryType.Staff;
                else
                    editItem.CalendarEntryType = CalendarEntryType.Financial;

                if (rbFinancial.Checked)
                {
                    var pm = (from b in context.tblBuildings
                              join u in context.tblUsers on b.pm equals u.email
                              where b.id == editItem.BuildingId
                              select u).SingleOrDefault();

                    if (pm != null)
                        editItem.UserId = pm.id;
                    else if (editItem.id == 0)
                        editItem.UserId = Controller.user.id;
                    editItem.UserInvites = new List<CalendarUserInvite>();
                }
                else
                {
                    editItem.UserId = Controller.user.id;
                    editItem.UserInvites = new List<CalendarUserInvite>();
                    foreach(var checkedItem in cbUserInvites.CheckedItems)
                    {
                        var usr = (tblUser)checkedItem;
                        editItem.UserInvites.Add(new CalendarUserInvite()
                        {
                            CalendarEntry = editItem,
                            UserId = usr.id
                        });
                    }
                }

                editItem.EntryDate = fromDate;
                editItem.EventToDate = toDate;
                editItem.Event = cbEvent.Text;
                editItem.Venue = tbVenue.Text;
                editItem.NotifyTrustees = cbNotifyTrustees.Checked;
                editItem.InviteSubject = tbSubject.Text;
                editItem.BCCEmailAddress = tbBCC.Text;
                editItem.InviteBody = tbBodyContent.Text;

                if (cbRoom.SelectedItem != null)
                    editItem.MeetingRoomId = (cbRoom.SelectedItem as IdValue).Id;
                else
                    editItem.MeetingRoomId = null;

                if(CheckDoubleBooking(context,editItem))
                {
                    return;
                }

                //check for attachments
                if(_FileToLoad != null)
                {
                    CalendarEntryAttachment fileItem = null;

                    if (editItem.id > 0)
                        fileItem = context.CalendarEntryAttachmentSet.Where(a => a.BuildingCalendarEntryId == editItem.id).FirstOrDefault();

                    if(fileItem == null)
                    {
                        fileItem = new CalendarEntryAttachment()
                        {
                            CalendarEntry = editItem
                        };
                        context.CalendarEntryAttachmentSet.Add(fileItem);
                    }

                    fileItem.FileData = _FileToLoad;
                    fileItem.FileName = tbAttachment.Text;
                }

                context.SaveChanges();
            }

            LoadGrid();
        }

        private bool CheckDoubleBooking(DataContext context, BuildingCalendarEntry editItem)
        {
            //first find out which rooms to load.
            if (editItem.MeetingRoomId == null)
                return false;

            var roomList = context.MeetingRoomSet.ToList();

            var selectedRoom = roomList.Where(a => a.id == editItem.MeetingRoomId).FirstOrDefault();
            if (selectedRoom == null)
                return false;

            if (selectedRoom.Name.Contains("A-B-C")) //we selected room A B and C
            {
                foreach (var room in roomList)
                {
                    if (HasMeetingOverlap(context, room, editItem.EntryDate, editItem.EventToDate))
                    {
                        Controller.HandleError("Meeting double booked for room " + room.ToString(), "Validation Error");
                        return true;
                    }
                }
            }
            else if(selectedRoom.Name.Contains("B-C"))
            {
                foreach (var room in roomList.Where(a => a.Name.Contains("B") || a.Name.Contains("C")))
                {
                    if (HasMeetingOverlap(context, room, editItem.EntryDate, editItem.EventToDate))
                    {
                        Controller.HandleError("Meeting double booked for room " + selectedRoom.ToString(), "Validation Error");
                        return true;
                    }
                }
            }
            else
            {
                if (HasMeetingOverlap(context, selectedRoom, editItem.EntryDate, editItem.EventToDate))
                {
                    Controller.HandleError("Meeting double booked for room " + selectedRoom.ToString(), "Validation Error");
                    return true;
                }
            }
            return false;
        }

        private bool HasMeetingOverlap(DataContext context, MeetingRoom selectedRoom, 
            DateTime fromDate, DateTime toDate)
        {
            var q = from c in context.BuildingCalendarEntrySet
                    where c.MeetingRoomId == selectedRoom.id
                    && ((c.EntryDate <= fromDate && c.EventToDate >= fromDate) //starts in an exsting
                    || (c.EntryDate <= toDate && c.EventToDate >= toDate) // ends in an existing
                    || (fromDate <= c.EntryDate  && toDate >= c.EventToDate) //overlaps
                    )
                    select c;

            var cnt = q.Count();
            return (cnt > 0);
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
                    foreach (var invite in itm.UserInvites)
                        context.CalendarUserInviteSet.Remove(invite);
                    context.BuildingCalendarEntrySet.Remove(itm);
                    context.SaveChanges();
                }
                LoadGrid();
                GotoReadOnly();
            }
        }
        bool _DisableGridLoad = false;
        private void EditItem(int id)
        {
            _DisableGridLoad = true;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    _Item = context.BuildingCalendarEntrySet.Single(a => a.id == id);
                    if (_Item.CalendarEntryType == CalendarEntryType.Financial)
                        rbFinancial.Checked = true;
                    else
                        rbStaff.Checked = true;
                    _FileToLoad = null;

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

                    if(_Item.MeetingRoomId != null)
                      cbRoom.SelectedValue = _MeetingRooms.Where(a => a.Id == _Item.MeetingRoomId).FirstOrDefault();
                    else
                      cbRoom.SelectedIndex = -1;


                    var attachmentName = context.CalendarEntryAttachmentSet.Where(a => a.BuildingCalendarEntryId == id).Select(a => a.FileName).FirstOrDefault();

                    if (!String.IsNullOrWhiteSpace(attachmentName))
                        tbAttachment.Text = attachmentName;
                    else
                        tbAttachment.Text = string.Empty;

                    var usrInvates = context.CalendarUserInviteSet.Where(a => a.CalendarEntryId == id).ToList();
                    for (int x = 0; x < cbUserInvites.Items.Count; x++)
                    {
                        var usr = cbUserInvites.Items[x] as tblUser;
                        var c = usrInvates.Where(a => a.UserId == usr.id).FirstOrDefault();
                        if (c != null)
                            cbUserInvites.SetItemChecked(x, true);
                    }


                    btnUpload.Enabled = true;

                    cbBuilding.Enabled = true;
                    dtpEventDate.Enabled = true;
                    dtpEventTime.Enabled = true;
                    cbEvent.Enabled = true;
                    tbVenue.Enabled = true;
                    cbRoom.Enabled = true;

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
                    cbUserInvites.Enabled = true;


                    if (rbStaff.Checked)
                    {
                        cbBuilding.Visible = false;
                        cbPM.Visible = false;
                        cbFilterPM.Visible = false;
                        label3.Visible = false;
                        cbUserInvites.Visible = true;
                        label4.Visible = false;
                        label8.Visible = false;
                        cbNotifyTrustees.Visible = false;
                        cbNotifyTrustees.Checked = false;
                    }
                    else
                    {
                        cbBuilding.Visible = true;
                        cbPM.Visible = true;
                        cbFilterPM.Visible = true;
                        label3.Visible = true;
                        cbUserInvites.Visible = false;

                        label4.Visible = true;
                        label8.Visible = true;
                        cbNotifyTrustees.Visible = true;
                    }

                }
            }
            finally
            {
                _DisableGridLoad = false;
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
            if (string.IsNullOrWhiteSpace(entry.PMEmail))
            {
                Controller.HandleError("Email address not found for " + entry.PM);
                return;
            }

            var calendarInvite = CreateCalendarInvite(entry.BuildingName + " - " + entry.Event, "", entry.Venue, entry.EventDate, entry.EventToDate != null ? entry.EventToDate.Value : entry.EventDate.AddHours(2));
            Dictionary<string, byte[]> attachments = new Dictionary<string, byte[]>();

            attachments.Add("Appointment.ics", calendarInvite);
            string subject = entry.InviteSubject;
            if (String.IsNullOrWhiteSpace(subject))
                subject = entry.BuildingName + " - " + entry.Event;

            string bodyContent = entry.InviteBody;
            if (String.IsNullOrWhiteSpace(bodyContent))
                bodyContent = "";

            string bccEmail = entry.BCCEmailAddress;
            if (bccEmail == string.Empty)
                bccEmail = null;

            //add aditional attachments
            using (var context = SqlDataHandler.GetDataContext())
            {
                var attach = context.CalendarEntryAttachmentSet.Where(a => a.BuildingCalendarEntryId == entry.Id).ToList();
                foreach(var a in attach)
                {
                    attachments.Add(a.FileName, a.FileData);
                }

                List<string> toAddress = new List<string>();
                toAddress.Add(entry.PMEmail);
                if (entry.EventyType == CalendarEntryType.Staff)
                {
                    var qInvite = from i in context.CalendarUserInviteSet
                                  where i.CalendarEntryId == entry.Id
                                  select i.User.email;

                    toAddress.AddRange(qInvite.ToList());

                }

                if (!Email.EmailProvider.SendCalendarInvite(entry.PMEmail, toAddress.Distinct().ToArray(),subject, bodyContent, attachments, bccEmail))
                {
                    Controller.HandleError("Error seding email ", "Email error");
                }

                if (entry.NotifyTrustees && entry.EventyType == CalendarEntryType.Financial)
                {
                    var customers = Controller.pastel.AddCustomers(entry.BuildingAbreviation, entry.BuildingDataPath, true);
                    var dbCustomers = context.CustomerSet.Where(a => a.BuildingId == entry.BuildingId && a.IsTrustee == true).ToList();
                    var clientPortal = new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString());

                    if (dbCustomers.Count() > 0 && Controller.AskQuestion("Are you sure you want to send the invite to " + dbCustomers.Count().ToString() + " trustees?"))
                    {

                        //upload building documents

                        Dictionary<string, string> trusteeAttachmentLinks = new Dictionary<string, string>();
                        foreach(var fileName in attachments.Keys.ToList())
                        {
                            if (fileName != "Appointment.ics")
                            {
                                var url = clientPortal.UploadBuildingDocument(DocumentCategoryType.PrivilegedCorrespondence, DateTime.Today, entry.BuildingId.Value, Path.GetFileNameWithoutExtension(fileName),fileName, attachments[fileName],"From Email");
                                trusteeAttachmentLinks.Add(fileName, url);
                            }
                        }

                        foreach (var dbCustomer in dbCustomers)
                        {
                            var trustee = customers.Where(a => a.accNumber == dbCustomer.AccountNumber).FirstOrDefault();
                            if (trustee != null)
                            {
                                if (trustee.Email != null && trustee.Email.Length > 0)
                                {
                                    if (!Email.EmailProvider.SendTrusteeCalendarInvite(entry.PMEmail, trustee.Email,subject,bodyContent,attachments,bccEmail,trusteeAttachmentLinks))
                                    {
                                        Controller.HandleError("Error seding email", "Email error");
                                    }
                                }
                            }
                        }

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

            public int? BuildingId { get; set; }
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
                     //    + EventToDate != null ? EventToDate.Value.ToString("HH:mm", CultureInfo.InvariantCulture)  : string.Empty
                     + " " + BuildingName
                     + "-" + Event
                     + "-" + PM
                     + "-" + Venue
                     + Room != string.Empty ? "-" + Room : "";
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
            public string FileName { get; internal set; }
            public CalendarEntryType EventyType { get; internal set; }
            public string Room { get; internal set; }
        }


        private bool IsValidPdf(string filepath)
        {
            bool Ret = true;

            PdfReader reader = null;

            try
            {
                using (reader = new PdfReader(filepath))
                {
                    reader.Close();
                }
            }
            catch
            {
                Ret = false;
            }

            return Ret;
        }


        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (fdOpen.ShowDialog() == DialogResult.OK)
            {
                btnUpload.Enabled = false;
                try
                {
                    if (!IsValidPdf(fdOpen.FileName))
                    {
                        btnUpload.Enabled = true;
                        Controller.HandleError("Not a valid PDF");
                        return;
                    }

                    _FileToLoad = File.ReadAllBytes(fdOpen.FileName);
                    tbAttachment.Text = Path.GetFileName(fdOpen.FileName);
                }
                catch
                {
                    MessageBox.Show("Failed to upload attachment");
                }
                finally
                {
                    btnUpload.Enabled = true;
                }
            }
        }

        private void cbRoomFilter_CheckedChanged(object sender, EventArgs e)
        {
            cbFilterRoom.Enabled = cbRoomFilter.Checked;
            LoadGrid();
        }

        private void cbRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbRoom.SelectedItem != null)
            {
                tbVenue.Text = cbRoom.Text;
            }
            else
            {
                tbVenue.Text = "";
            }
        }
    }
}

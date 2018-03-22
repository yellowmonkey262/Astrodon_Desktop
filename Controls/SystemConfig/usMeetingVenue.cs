using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Data.Calendar;
using System.Data.Entity.Infrastructure;

namespace Astrodon.Controls.SystemConfig
{
    public partial class usMeetingVenue : UserControl
    {
        private DataContext _Context;
        private MeetingRoom _Item;
        private List<MeetingRoom> _Data;

        public usMeetingVenue(DataContext context)
        {
            _Context = context;
            InitializeComponent();

            LoadMeetingVenues();
            GotoReadOnly();
        }

        private void GotoReadOnly()
        {
            tbName.Text = "";
            tbName.ReadOnly = true;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnNew.Visible = true;
            dgItems.Enabled = true;
            tbSeats.Enabled = false;
            cbActive.Enabled = false;
            cbActive.Checked = false;
            tbSeats.Value = 1;
        }


        private void GotoEditable()
        {
            tbName.ReadOnly = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnNew.Visible = false;
            dgItems.Enabled = false;
            tbSeats.Enabled = true;
            cbActive.Enabled = true;

        }

        private void LoadMeetingVenues()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                _Data = _Context.MeetingRoomSet.OrderBy(a => a.Name).ToList();

                BindDataGrid();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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
                Text = "Select",
                UseColumnTextForButtonValue = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "NumberOfSeats",
                HeaderText = "Seats",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Active",
                HeaderText = "Active",
                ReadOnly = true
            });

            dgItems.AutoResizeColumns();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbName.Text))
            {
                Controller.HandleError("Name is required", "Validation Error");
                return;
            }

            if (_Item == null)
            {
                _Item = new MeetingRoom();
                _Context.MeetingRoomSet.Add(_Item);
            }
            _Item.Name = tbName.Text;
            _Item.NumberOfSeats = Convert.ToInt32( tbSeats.Value);
            _Item.Active = cbActive.Checked;

            try
            {
                bool isNew = _Item.id == 0;
                _Context.SaveChanges();

                if (isNew)
                    _Data.Insert(0, _Item);
                BindDataGrid();
                GotoReadOnly();
            }
            catch (DbUpdateException)
            {
                Controller.HandleError("Possible duplicate record detected", "Database Error");
            }
            catch (Exception ex2)
            {
                Controller.HandleError(ex2.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _Item = null;
            GotoEditable();
            cbActive.Checked = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _Context.ClearChanges();
            GotoReadOnly();
            LoadMeetingVenues();
        }

        private void EditItem()
        {
            tbName.Text = _Item.Name;
            cbActive.Checked = _Item.Active;
            tbSeats.Value = _Item.NumberOfSeats;
            GotoEditable();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as MeetingRoom;

                if (_Item != null)
                {
                    EditItem();
                }
            }
        }

       
    }
}

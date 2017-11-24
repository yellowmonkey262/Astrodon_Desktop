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
    public partial class ucPublicHoliday : UserControl
    {
        private DataContext _Context;
        private PublicHoliday _Item;
        private List<PublicHoliday> _Data;
        private int userid;

        public ucPublicHoliday(DataContext context)
        {
            _Context = context;
            InitializeComponent();

            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "yyyy/MM/dd";

            LoadPublicHolidays();
            GotoReadOnly();
            
        }

        private void GotoReadOnly()
        {
            tbName.Text = "";
            tbName.ReadOnly = true;

            dtpDate.Value = DateTime.Today;
            dtpDate.Enabled = false;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnNew.Visible = true;
            dgItems.Enabled = true;
        }

        private void GotoEditable()
        {
            tbName.ReadOnly = false;
            dtpDate.Enabled = true;

            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnNew.Visible = false;
            dgItems.Enabled = false;

        }

        private void LoadPublicHolidays()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DateTime startDate = DateTime.Today.AddMonths(-3);

                _Data = _Context.PublicHolidaySet.Where(a => a.Date > startDate).OrderBy(a => a.Date).ToList();

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
                DataPropertyName = "HolidayName",
                HeaderText = "Name",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Date",
                HeaderText = "Date",
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
                _Item = new PublicHoliday();
                _Context.PublicHolidaySet.Add(_Item);
            }
            _Item.HolidayName = tbName.Text;
            _Item.Date = dtpDate.Value.Date;

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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _Context.ClearChanges();
            GotoReadOnly();
            LoadPublicHolidays();
        }

        private void EditItem()
        {
            tbName.Text = _Item.HolidayName;
            dtpDate.Value = _Item.Date;
            GotoEditable();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as PublicHoliday;

                if (_Item != null)
                {
                    EditItem();
                }
            }
        }
    }
}

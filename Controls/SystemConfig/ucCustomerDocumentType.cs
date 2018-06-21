using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Data.CustomerData;
using System.Data.Entity.Infrastructure;

namespace Astrodon.Controls.SystemConfig
{
    public partial class ucCustomerDocumentType : UserControl
    {
        private DataContext _Context;
        private CustomerDocumentType _Item;
        private List<CustomerDocumentType> _Data;

        public ucCustomerDocumentType(DataContext context)
        {
            InitializeComponent();

            _Context = context;

            LoadData();
            GotoReadOnly();
        }

        private void GotoReadOnly()
        {
            txDocumentType.Text = "";
            txDocumentType.ReadOnly = true;
            cbActive.Enabled = false;
            cbActive.Checked = true;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnNew.Visible = true;
            dgItems.Enabled = true;
        }

        private void GotoEditable()
        {
            txDocumentType.ReadOnly = false;
            cbActive.Enabled = true;

            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnNew.Visible = false;
            dgItems.Enabled = false;

        }

        private void LoadData()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DateTime startDate = DateTime.Today.AddMonths(-3);

                _Data = _Context.CustomerDocumentTypeSet.OrderBy(a => a.Name).ToList();

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
                UseColumnTextForButtonValue = true,
                Width = 50
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "IsActive",
                HeaderText = "Active",
                ReadOnly = true,
                Width = 100
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                ReadOnly = true,
                Width = 200
            });
            //   dgItems.AutoResizeColumns();

        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            _Item = null;
            cbActive.Checked = true;
            GotoEditable();
        }

        private void EditItem()
        {
            txDocumentType.Text = _Item.Name;
            cbActive.Checked = _Item.IsActive;
            GotoEditable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txDocumentType.Text))
            {
                Controller.HandleError("Name is required", "Validation Error");
                return;
            }

            if (_Item == null)
            {
                _Item = new CustomerDocumentType();
                _Context.CustomerDocumentTypeSet.Add(_Item);
            }
            _Item.Name = txDocumentType.Text;
            _Item.IsActive = cbActive.Checked;

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _Context.ClearChanges();
            GotoReadOnly();
            LoadData();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as CustomerDocumentType;

                if (_Item != null)
                {
                    EditItem();
                }
            }
        }
    }
}

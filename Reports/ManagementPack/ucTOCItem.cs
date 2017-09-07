using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data.ManagementPackData;
using Astrodon.Data;
using System.Data.Entity.Infrastructure;

namespace Astrodon.Reports.ManagementPack
{
    public partial class ucTOCItem : UserControl
    {
        private List<ManagementPackTOCItem> _Data = null;
        private ManagementPackTOCItem _Item = null;
        private DataContext dataContext;
        public ucTOCItem(DataContext context)
        {
            InitializeComponent();
            dataContext = context;
            LoadTOCRecords();
        }

        private void LoadTOCRecords()
        {
            _Data = dataContext.ManagementPackTOCItemSet.OrderBy(a => a.Description).ToList();
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
                HeaderText = "Edit",
                Text = "Edit",
                UseColumnTextForButtonValue = true
            });

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                ReadOnly = true,
                MinimumWidth = 200
            });

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _Item = null;
            tbName.Text = "";

            tbName.ReadOnly = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            btnNew.Visible = false;
            dgItems.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dataContext.ClearChanges();

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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbName.Text) )
            {
                Controller.HandleError("Name  is required", "Validation Error");
                return;
            }

            if (_Item == null)
            {
                _Item = new ManagementPackTOCItem();
                dataContext.ManagementPackTOCItemSet.Add(_Item);
            }

            _Item.Description = tbName.Text;
            try
            {
                bool isNew = _Item.id == 0;
                dataContext.SaveChanges();

                if (isNew)
                    _Data.Insert(0, _Item);
                BindDataGrid();
                GotoReadOnly();
            }
            catch (DbUpdateException ex)
            {
                Controller.HandleError("Possible duplicate record detected", "Database Error");
            }
            catch (Exception ex2)
            {
                Controller.HandleError(ex2.Message);
            }
        }

        private void EditItem()
        {
            tbName.Text = _Item.Description;
            tbName.ReadOnly = false;
            btnNew.Visible = false;
            btnCancel.Visible = true;
            btnSave.Visible = true;
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as ManagementPackTOCItem;

                if (_Item != null )
                {
                    if (e.ColumnIndex == 0)
                        EditItem();
                    else
                        DeleteItem();
                }
            }
        }

        private void DeleteItem()
        {
            if (_Item == null)
                return;

            if(Controller.AskQuestion("Are you sure you want to delete \n" + _Item.Description))
            {
                var i = dataContext.ManagementPackTOCItemSet.Single(a => a.id == _Item.id);
                dataContext.ManagementPackTOCItemSet.Remove(i);
                dataContext.SaveChanges();
                _Data.Remove(_Item);
                BindDataGrid();
                GotoReadOnly();
            }
        }
    }
}

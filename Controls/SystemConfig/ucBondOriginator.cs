using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Data.BankData;
using System.Data.Entity.Infrastructure;

namespace Astrodon.Controls.SystemConfig
{
    public partial class ucBondOriginator : UserControl
    {
        private DataContext _Context;
        private BondOriginator _Item;
        private List<BondOriginator> _Data;

        public ucBondOriginator(DataContext context)
        {
            InitializeComponent();

            _Context = context;

            LoadData();
            GotoReadOnly();

        }

        private void GotoReadOnly()
        {
            txCompanyName.Text = "";
            txCompanyName.ReadOnly = true;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnNew.Visible = true;
            dgItems.Enabled = true;
        }

        private void GotoEditable()
        {
            txCompanyName.ReadOnly = false;

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

                _Data = _Context.BondOriginatorSet.OrderBy(a => a.CompanyName).ToList();

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
                DataPropertyName = "CompanyName",
                HeaderText = "Company Name",
                ReadOnly = true,
                Width = 200
            });
         //   dgItems.AutoResizeColumns();

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txCompanyName.Text))
            {
                Controller.HandleError("Name is required", "Validation Error");
                return;
            }

            if (_Item == null)
            {
                _Item = new BondOriginator();
                _Context.BondOriginatorSet.Add(_Item);
            }
            _Item.CompanyName = txCompanyName.Text;

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
            LoadData();
        }

        private void EditItem()
        {
            txCompanyName.Text = _Item.CompanyName;
            GotoEditable();
        }


        private void dgItems_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as BondOriginator;

                if (_Item != null)
                {
                    EditItem();
                }
            }
        }

        
    }
}

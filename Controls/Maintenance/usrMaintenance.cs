using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Data.Base;
using Astrodon.Forms;
using Astrodon.Data.MaintenanceData;
using Astradon.Data.Utility;

namespace Astrodon.Controls.Maintenance
{
    public partial class usrMaintenance : UserControl
    {
        private DataContext _DataContext;
        private List<MaintenanceResult> _MaintenanceRecords;

        public usrMaintenance(DataContext context)
        {
            _DataContext = context;
            InitializeComponent();
            LoadBuildings();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            int buildingId = (cmbBuilding.SelectedItem as IdValue).Id;
            var fromDate = dtpFromDate.Value.Date;
            var toDate = dtpToDate.Value.Date.AddDays(1).AddMinutes(-1);

            _MaintenanceRecords = (from m in _DataContext.MaintenanceSet
                                   where m.BuildingMaintenanceConfiguration.BuildingId == buildingId
                                   && m.DateLogged >= fromDate
                                   && m.DateLogged <= toDate
                                   select new MaintenanceResult()
                                   {
                                       Id = m.id,
                                       DateLogged = m.DateLogged,
                                       ConfigClassification = m.BuildingMaintenanceConfiguration.MaintenanceClassificationType,
                                       ConfigName = m.BuildingMaintenanceConfiguration.Name,
                                       CustomerAccount = m.CustomerAccount,
                                       SupplierName = m.Supplier.CompanyName,
                                       SupplierContactPerson = m.Supplier.ContactPerson,
                                       TotalAmount = m.TotalAmount
                                   }).OrderBy(a => a.DateLogged).ToList();

            BindMaintenanceDataGrid();

            this.Cursor = Cursors.Default;
        }

        #region Helper Functions

        private void LoadBuildings()
        {
            Buildings bManager = (Controller.user.id == 0 ? new Buildings(false) : new Buildings(Controller.user.id));

            var buildings = (from building in bManager.buildings
                            select new IdValue
                            {
                                Id = building.ID,
                                Value = building.Name
                            }).OrderBy(a => a.Value).ToList();

            cmbBuilding.DataSource = buildings;
            cmbBuilding.ValueMember = "Id";
            cmbBuilding.DisplayMember = "Value";
            cmbBuilding.SelectedIndex = 0;
        }

        private void BindMaintenanceDataGrid()
        {
            dgMaintenance.ClearSelection();
            dgMaintenance.MultiSelect = false;
            dgMaintenance.AutoGenerateColumns = false;

            dgMaintenance.Columns.Clear();
            dgMaintenance.DataSource = null;

            if (_MaintenanceRecords.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = _MaintenanceRecords;
                dgMaintenance.DataSource = bs;

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "DateLoggedDisplay",
                    HeaderText = "Date",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "ConfigClassificationDisplay",
                    HeaderText = "Classification",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "ConfigName",
                    HeaderText = "Type",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "CustomerAccount",
                    HeaderText = "Unit",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "SupplierName",
                    HeaderText = "Supplier",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "SupplierContactPerson",
                    HeaderText = "Contact",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "TotalAmount",
                    HeaderText = "Amount",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewButtonColumn()
                {
                    HeaderText = "Action",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                });

                dgMaintenance.AutoResizeColumns();
            }
        }

        #endregion

        private void dgMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            var editColumnIndex = senderGrid.Columns.Count - 1;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var selectedItem = senderGrid.Rows[e.RowIndex].DataBoundItem as MaintenanceResult;

                if (selectedItem != null)
                {
                    if (e.ColumnIndex == editColumnIndex)
                    {
                        var frmMaintenanceDetail = new frmMaintenanceDetail(_DataContext, selectedItem.Id);
                        var dialogResult = frmMaintenanceDetail.ShowDialog();

                        if (dialogResult == DialogResult.OK)
                            btnSearch.PerformClick();
                    }
                }
            }
        }
    }

    public class MaintenanceResult
    {
        public int Id { get; set; }

        public DateTime DateLogged { get; set; }

        public string DateLoggedDisplay { get { return DateLogged.ToString("yyyy-MM-dd"); } }

        public MaintenanceClassificationType ConfigClassification { get; set; }

        public string ConfigClassificationDisplay { get { return NameSplitting.SplitCamelCase(ConfigClassification); } }

        public string ConfigName { get; set; }

        public string CustomerAccount { get; set; }

        public string SupplierName { get; set; }

        public string SupplierContactPerson { get; set; }

        public decimal TotalAmount { get; set; }

        public string TotalAmountDisplay { get { return TotalAmount.ToString("#,###.00"); } }
    }
}

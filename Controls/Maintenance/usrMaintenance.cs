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
using System.Data.Entity;
using Astro.Library.Entities;

namespace Astrodon.Controls.Maintenance
{
    public partial class usrMaintenance : UserControl
    {
        private DataContext _DataContext;
        private List<MaintenanceResult> _MaintenanceRecords;
        private List<Building> allBuildings;

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

            var config = _DataContext.BuildingMaintenanceConfigurationSet.Where(a => a.BuildingId == buildingId).ToList();
            var ledgerAccounts = config.Select(a => a.PastelAccountNumber).ToArray();


            var tmp = (from r in _DataContext.tblRequisitions
                                   join maint in _DataContext.MaintenanceSet.Include(a =>a.DetailItems) on r.id equals maint.RequisitionId into mx
                                   where r.building == buildingId
                                   && r.trnDate >= fromDate
                                   && r.trnDate <= toDate
                                   from m in mx.DefaultIfEmpty()
                                   select new MaintenanceResult()
                                   {
                                       RequisitionId = r.id,
                                       RequsitionDate = r.trnDate,
                                       Reference = r.reference,
                                       PaymentReference = r.payreference,
                                       Account = r.account,
                                       Ledger = r.ledger,
                                       Paid = r.paid,
                                       MaintenanceId = m == null ? (int?)null : m.id,
                                       DateLogged = m == null ? (DateTime?)null : m.DateLogged,
                                       ConfigClassification = m == null ? (MaintenanceClassificationType?)null : m.BuildingMaintenanceConfiguration.MaintenanceClassificationType,
                                       ConfigName = m == null ? null : m.BuildingMaintenanceConfiguration.Name,
                                       SupplierName = m == null ? null : m.Supplier.CompanyName,
                                       SupplierContactPerson = m == null ? null : m.Supplier.ContactPerson,
                                       TotalAmount = m == null ? (decimal?)null : m.TotalAmount,
                                       ConfigItemId = m == null ? (int?)null : m.BuildingMaintenanceConfiguration.id,
                                   }).OrderBy(a => a.RequsitionDate).ToList();

            _MaintenanceRecords = (from s in tmp
                                   join c in config on s.LedgerAccountNumber equals c.PastelAccountNumber into mx
                                   from d in mx.DefaultIfEmpty()
                                   select new MaintenanceResult()
                                   {
                                       RequisitionId = s.RequisitionId,
                                       RequsitionDate = s.RequsitionDate,
                                       Reference = s.Reference,
                                       PaymentReference = s.PaymentReference,
                                       Account = s.Account,
                                       Ledger = s.Ledger,
                                       Paid = s.Paid,
                                       MaintenanceId = s.MaintenanceId,
                                       DateLogged = s.DateLogged,
                                       ConfigClassification = s.ConfigClassification != null ? s.ConfigClassification : d == null ? (MaintenanceClassificationType?)null : d.MaintenanceClassificationType,
                                       ConfigName = s.ConfigName != null ? s.ConfigName : d == null ? null : d.Name,
                                       SupplierName = s.SupplierName,
                                       SupplierContactPerson = s.SupplierContactPerson,
                                       TotalAmount = s.TotalAmount,
                                       ConfigItemId = s.ConfigItemId != null ? s.ConfigItemId : d == null ? (int?)null : d.id,
                                   }).Where(a => a.ConfigItemId != null &&
                                   
                                   (cbUnlinked.Checked == false || a.HasMaintenance == false)
                                   ).OrderBy(a => a.RequsitionDate).ToList();

            BindMaintenanceDataGrid();

            if(_MaintenanceRecords.Count == 0)
            {
                Controller.HandleError("Your search did not return any result", "Empty Search");
            }

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

            buildings.Insert(0, new IdValue() { Id = 0, Value = "" });
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

            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            if (_MaintenanceRecords.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = _MaintenanceRecords;
                dgMaintenance.DataSource = bs;

                dgMaintenance.Columns.Add(new DataGridViewButtonColumn()
                {
                    HeaderText = "Action",
                    DataPropertyName = "ButtonText",
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "DateLoggedDisplay",
                    HeaderText = "Date",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "TotalAmount",
                    HeaderText = "Amount",
                    ReadOnly = true,
                    DefaultCellStyle = currencyColumnStyle
                });


                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Reference",
                    HeaderText = "Reference",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "PaymentReference",
                    HeaderText = "Payment Reference",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Account",
                    HeaderText = "Account",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Ledger",
                    HeaderText = "Ledger",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "PaidString",
                    HeaderText = "Paid",
                    ReadOnly = true
                });

                dgMaintenance.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "MaintenanceLinked",
                    HeaderText = "Has Maintenance",
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

              
              

                dgMaintenance.AutoResizeColumns();
            }
        }

        #endregion

        private void dgMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            var editColumnIndex = 0;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var selectedItem = senderGrid.Rows[e.RowIndex].DataBoundItem as MaintenanceResult;

                if (selectedItem != null)
                {
                    if (e.ColumnIndex == editColumnIndex)
                    {
                        if (selectedItem.MaintenanceId != null)
                        {
                            var frmMaintenanceDetail = new frmMaintenanceDetail(_DataContext, selectedItem.MaintenanceId.Value,false);
                            var dialogResult = frmMaintenanceDetail.ShowDialog();

                            if (dialogResult == DialogResult.OK)
                                btnSearch.PerformClick();
                        }
                        else
                        {
                            var reqItem = _DataContext.tblRequisitions.Where(a => a.id == selectedItem.RequisitionId).Single();
                            var configItem = _DataContext.BuildingMaintenanceConfigurationSet.Single(a => a.id == selectedItem.ConfigItemId.Value);

                            if(reqItem.SupplierId == null)
                            {
                                //lets find the supplier id


                                var frmSupplierLookup = new frmSupplierLookup(_DataContext, configItem.BuildingId);

                                var supplierResult = frmSupplierLookup.ShowDialog();
                                var supplier = frmSupplierLookup.SelectedSupplier;

                                if (supplierResult == DialogResult.OK && supplier != null)
                                {
                                    var bankDetails = _DataContext.SupplierBuildingSet.Include(a => a.Bank).SingleOrDefault(a => a.BuildingId == configItem.BuildingId && a.SupplierId == supplier.id);
                                    if (bankDetails != null)
                                    {
                                        reqItem.SupplierId = supplier.id;
                                        reqItem.Supplier = supplier;
                                        reqItem.BankName = bankDetails.Bank.Name;
                                        reqItem.BranchCode = bankDetails.BranceCode;
                                        reqItem.BranchName = bankDetails.BranchName;
                                        reqItem.AccountNumber = bankDetails.AccountNumber;
                                    }
                                    else
                                    {
                                        Controller.HandleError("Supplier banking details for this building is not configured.\n"+
                                            "Please capture bank details for this building on the suppier detail screen.", "Validation Error");
                                        return;
                                    }
                                }
                                else
                                {
                                    Controller.HandleError("Supplier required for Maintenance. Please select a supplier.", "Validation Error");
                                    return;
                                }
                            }

                            var frmMaintenanceDetail = new frmMaintenanceDetail(_DataContext, reqItem, configItem,true);
                            var dialogResult = frmMaintenanceDetail.ShowDialog();
                            if(dialogResult == DialogResult.OK)
                            {
                                _DataContext.SaveChanges();
                                btnSearch.PerformClick();
                            }
                            else
                            {
                                //reject changes
                                foreach (var entry in _DataContext.ChangeTracker.Entries())
                                {
                                    switch (entry.State)
                                    {
                                        case EntityState.Modified:
                                        case EntityState.Deleted:
                                            entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                                            entry.State = EntityState.Unchanged;
                                            break;
                                        case EntityState.Added:
                                            entry.State = EntityState.Detached;
                                            break;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            IdValue selectedBuilding = cmbBuilding.SelectedItem as IdValue;

            if (cmbBuilding.SelectedItem != null)
            {
                if (!Controller.VerifyBuildingDetailsEntered(selectedBuilding.Id))
                {
                    cmbBuilding.SelectedIndex = -1;
                    dgMaintenance.DataSource = null;
                    return;
                }
            }
        }
    }

    public class MaintenanceResult
    {
        #region Requisition
        public int RequisitionId { get; set; }
        public DateTime RequsitionDate { get; set; }
        public string Reference { get; set; }
        public string PaymentReference { get; set; }
        public string Account { get; set; }
        public string Ledger { get; set; }
        public string LedgerAccountNumber
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Ledger))
                    return Ledger;
                if (!Ledger.Contains(":"))
                    return Ledger;

                return Ledger.Substring(0, Ledger.IndexOf(":"));
            }
        }
        public bool Paid { get; set; }
        public string PaidString { get { return Paid ? "Yes" : "No"; } }
        public string MaintenanceLinked { get { return MaintenanceId == null ? "No" : "Yes"; } }
        public string ButtonText { get { return MaintenanceId == null ? "Edit" : "Edit"; } }
        public int? ConfigItemId { get;  set; }

        #endregion

        #region Maintenance
        public int? MaintenanceId { get; set; }

        public DateTime? DateLogged { get; set; }
        public string DateLoggedDisplay { get { return RequsitionDate.ToString("yyyy-MM-dd"); } }

        public MaintenanceClassificationType? ConfigClassification { get; set; }
        public string ConfigClassificationDisplay { get { return ConfigClassification == null ? string.Empty : NameSplitting.SplitCamelCase(ConfigClassification); } }

        public string ConfigName { get; set; }

        public string SupplierName { get; set; }

        public string SupplierContactPerson { get; set; }

        public decimal? TotalAmount { get; set; }
        public string TotalAmountDisplay { get { return TotalAmount == null ? string.Empty : TotalAmount.Value.ToString("#,###.00"); } }

        public bool HasMaintenance
        {
            get { return MaintenanceId != null; }
        }

        public List<MaintenanceDetailItem> Units { get;  set; }

        #endregion

    }
}

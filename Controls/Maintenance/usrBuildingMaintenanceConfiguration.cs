using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data.Base;
using Astrodon.Data.MaintenanceData;
using Astrodon.Data;
using System.Data.Entity.Infrastructure;

namespace Astrodon.Controls.Maintenance
{
    public partial class usrBuildingMaintenanceConfiguration : UserControl
    {
        private List<Building> _Buildings;
        private List<StringKeyValue> _Accounts;
        private List<StringKeyValue> _FilteredList;
        private List<BuildingMaintenanceConfiguration> _Data;
        private BuildingMaintenanceConfiguration _Item = null;
        private Building _SelectedBuilding;
        private DataContext dataContext;



        private int userid;

        public usrBuildingMaintenanceConfiguration(DataContext context)
        {
            dataContext = context;
            userid = Controller.user.id;
            InitializeComponent();
            LoadBuildings();
            GotoReadOnly();
        }

      

        private void LoadAccounts()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                try
                {
                    if (_SelectedBuilding == null)
                        return;
                    Dictionary<String, String> ledgerAccounts = Controller.pastel.GetAccountList(_SelectedBuilding.DataPath);

                    _Accounts = ledgerAccounts.Where(a => a.Key.StartsWith("435") || a.Key == "9200999").Select(b => new StringKeyValue() { Id = b.Key, Value = b.Value }).ToList();
                    cmbAccount.DataSource = _Accounts;
                    cmbAccount.ValueMember = "Id";
                    cmbAccount.DisplayMember = "Display";
                }
                catch (Exception e)
                {
                    Controller.HandleError(e);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
                _Buildings = bManager.buildings;
                cmbBuilding.DataSource = _Buildings;
                cmbBuilding.ValueMember = "ID";
                cmbBuilding.DisplayMember = "Name";
                if (_Buildings.Count > 0)
                    cmbBuilding.SelectedIndex = 0;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SelectedBuilding = (cmbBuilding.SelectedItem as Building);
            _Data = null;
            _Item = null;
            LoadAccounts();
            LoadMaintenanceRecords();
            GotoReadOnly();

        }

        private void LoadMaintenanceRecords()
        {
            if (_SelectedBuilding == null)
                return;

            _Data = dataContext.BuildingMaintenanceConfigurationSet.Where(a => a.BuildingId == _SelectedBuilding.ID).OrderBy(a => a.Name).ToList();
           
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

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PastelAccount",
                HeaderText = "Account",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Classification",
                HeaderText = "Classification",
                ReadOnly = true
            });

            dgItems.AutoResizeColumns();

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _Item = null;
            var linked = _Data.Select(a => a.PastelAccountNumber).ToArray() ;

            _FilteredList = _Accounts.Where(a => !linked.Contains(a.Id)).ToList();
            cmbAccount.DataSource = _FilteredList;
            cmbAccount.SelectedItem = null;

            cmbBuilding.Enabled = false;
            rbInsurance.Checked = false;
            rbMaintenancePlan.Checked = false;
            rbProject.Checked = false;
            rbRemedialMaintenance.Checked = true;

            tbName.Text = "";

            gbClassification.Enabled = true;
            tbName.ReadOnly = false;
            cmbAccount.Enabled = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            btnNew.Visible = false;
            dgItems.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbName.Text)
                || cmbAccount.SelectedItem == null
                || _SelectedBuilding == null)
            {
                Controller.HandleError("Name and Account is required", "Validation Error");
                return;
            }

            if (_Item == null)
            {
                _Item = new BuildingMaintenanceConfiguration();
                dataContext.BuildingMaintenanceConfigurationSet.Add(_Item);
            }

            _Item.BuildingId = _SelectedBuilding.ID;
            _Item.MaintenanceClassificationType = GetClassificationType();
            _Item.Name = tbName.Text;
            _Item.PastelAccountNumber = (cmbAccount.SelectedItem as StringKeyValue).Id;
            _Item.PastelAccountName = (cmbAccount.SelectedItem as StringKeyValue).Value;

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
            rbRemedialMaintenance.Checked = false;
            rbMaintenancePlan.Checked = false;
            rbProject.Checked = false;
            rbInsurance.Checked = false;
            switch (_Item.MaintenanceClassificationType)
            {
                case MaintenanceClassificationType.RemedialMaintenance:
                    rbRemedialMaintenance.Checked = true;
                    break;
                case MaintenanceClassificationType.MaintenancePlan:
                    rbMaintenancePlan.Checked = true;
                    break;
                case MaintenanceClassificationType.Project:
                    rbProject.Checked = true;
                    break;
                case MaintenanceClassificationType.Insurance:
                    rbInsurance.Checked = true;
                    break;
                default:
                    break;
            }
            gbClassification.Enabled = true;

            tbName.Text = _Item.Name;
            tbName.ReadOnly = false;


            var linked = _Data.Where(a => a.PastelAccountNumber != _Item.PastelAccountNumber).Select(a => a.PastelAccountNumber).ToArray();

            _FilteredList = _Accounts.Where(a => !linked.Contains(a.Id)).ToList();
            cmbAccount.DataSource = _Accounts;
            cmbAccount.SelectedItem = _Accounts.Where(a => a.Id == _Item.PastelAccountNumber).FirstOrDefault();
            cmbAccount.Enabled = false;
            btnNew.Visible = false;
            btnCancel.Visible = true;
            btnSave.Visible = true;
        }

        private void GotoReadOnly()
        {
            rbInsurance.Checked = false;
            rbMaintenancePlan.Checked = false;
            rbProject.Checked = false;
            rbRemedialMaintenance.Checked = false;

            tbName.Text = "";
            gbClassification.Enabled = false;
            tbName.ReadOnly = true;
            cmbAccount.Enabled = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnNew.Visible = true;
            dgItems.Enabled = true;

            if (_Data != null)
            {
                var linked = _Data.Select(a => a.PastelAccountNumber).ToArray();
                _FilteredList = _Accounts.Where(a => !linked.Contains(a.Id)).ToList();
                cmbAccount.DataSource = _FilteredList;

                btnNew.Visible = _FilteredList.Count > 0;

            }

        }

        private MaintenanceClassificationType GetClassificationType()
        {
            if (rbInsurance.Checked)
                return Data.MaintenanceData.MaintenanceClassificationType.Insurance;

            if (rbMaintenancePlan.Checked)
                return Data.MaintenanceData.MaintenanceClassificationType.MaintenancePlan;

            if (rbProject.Checked)
                return Data.MaintenanceData.MaintenanceClassificationType.Project;

            return Data.MaintenanceData.MaintenanceClassificationType.RemedialMaintenance;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GotoReadOnly();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&  e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as BuildingMaintenanceConfiguration;

                if(_Item != null)
                {
                    EditItem();
                }
            }

        }

       
    }
}

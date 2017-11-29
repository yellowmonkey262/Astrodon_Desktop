using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using System.Data.Entity;
using Astrodon.Controls.Events;
using Astro.Library.Entities;
using Astrodon.Data.SupplierData;

namespace Astrodon.Controls.Supplier
{
    public partial class usrSupplierDetail : UserControl
    {
        private List<Building> _Buildings;
        private List<Astrodon.Data.BankData.Bank> _Banks;
        private DataContext _DataContext;
        private List<BuildingResult> _BuildingData;
        private List<SupplierAuditTrailResult> _AuditTrailData;
        private List<SupplierAuditTrailResult> _BuildingAuditTrailData;
        private Astrodon.Data.SupplierData.Supplier _Supplier;
        private int _SupplierId;
        private int _userid;
        private int? _buildingId;
        private bool promptBankEnabled = false;
        private bool _closeOnSave = false;

        public usrSupplierDetail(DataContext context, int supplierId, int? buildingId, bool closeOnSave)
        {
            InitializeComponent();
            _closeOnSave = closeOnSave;
            _DataContext = context;
            _userid = Controller.user.id;
            _buildingId = buildingId;
            _SupplierId = supplierId;
            btnNewBuilding.Visible = closeOnSave == false;
            dgBuildings.Enabled = closeOnSave == false;
       
            LoadBanks();
            LoadBuildings();

            PopulateForm();
            if (buildingId != null)
                LoadSupplierBuilding(supplierId, buildingId.Value);

            promptBankEnabled = true;

        }

        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var userid = Controller.user.id;
                Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
                _Buildings = bManager.buildings;
                if (_buildingId != null)
                    _Buildings = _Buildings.Where(a => a.ID == _buildingId.Value).ToList();
                cmbBuilding.DataSource = _Buildings;
                cmbBuilding.ValueMember = "ID";
                cmbBuilding.DisplayMember = "Name";
                if (_Buildings.Count == 1)
                {
                    cmbBuilding.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbBuilding.SelectedIndex = 0;
                }
                else
                {
                    cmbBuilding.DropDownStyle = ComboBoxStyle.DropDown;
                    cmbBuilding.SelectedIndex = -1;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void LoadBanks()
        {
            //_Banks
            this.Cursor = Cursors.WaitCursor;

            try
            {
                _Banks = _DataContext.BankSet.Where(a => a.IsActive).ToList();
                cbBanks.DataSource = _Banks;
                cbBanks.ValueMember = "Id";
                cbBanks.DisplayMember = "Name";
                cbBanks.SelectedIndex = -1;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }



        #region Events

        public event SaveResultEventHandler SaveResultEvent;

        private void RaiseSaveSuccess()
        {
            if (SaveResultEvent != null)
                SaveResultEvent(this, new SaveResultEventArgs(true));
        }

        private void RaiseCancel()
        {
            if (SaveResultEvent != null)
                SaveResultEvent(this, new SaveResultEventArgs());
        }

        #endregion

        private void PopulateForm()
        {
            btnChangeAll.Enabled = Controller.UserIsSheldon() && _SupplierId > 0; //Sheldon and Tertia
            btnUpdateSupplier.Visible = false;
            if (_SupplierId > 0)
            {
                btnUpdateSupplier.Visible = true;
                _Supplier = _DataContext.SupplierSet.Single(a => a.id == _SupplierId);

                _AuditTrailData = _DataContext.SupplierAuditSet
                                  .Where(a => a.SupplierId == _SupplierId)
                                  .Select(a => new SupplierAuditTrailResult
                                  {
                                      AuditDate = a.AuditTimeStamp,
                                      UserName = a.User.username,
                                      FieldName = a.FieldName,
                                      OldValue = a.OldValue,
                                      NewValue = a.NewValue
                                  })
                                  .OrderByDescending(a => a.AuditDate)
                                  .ToList();



                _BuildingData = (from m in _DataContext.SupplierBuildingSet.Where(a => a.SupplierId == _SupplierId)
                                 select new BuildingResult
                                 {
                                     SupplierBuildingId = m.id,
                                     BuildingName = m.Building.Building,
                                     Bank = m.Bank.Name,
                                     BankId = m.BankId,
                                     BranchCode = m.BranceCode,
                                     BranchName = m.BranchName,
                                     AccountNumber = m.AccountNumber,
                                     IsLinked = true,
                                     SupplierId = m.SupplierId,
                                     BuildingId = m.BuildingId
                                 })
                                .ToList();
            }
            else
            {
                _Supplier = new Data.SupplierData.Supplier();
                _AuditTrailData = new List<SupplierAuditTrailResult>();
                _BuildingData = new List<BuildingResult>();
            }

            BindInputs();
            BindAuditTrailDataGrid();
            BindBuildingsDataGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSupplier(false); //include banking details
        }
        private void SaveSupplier(bool supplierOnly)
        {
            var validationResult = ValidateForm(supplierOnly);

            if (!String.IsNullOrEmpty(validationResult))
            {
                Controller.HandleError(validationResult, "Validation Error");
                return;
            }
            else
            {
                if (_SupplierId > 0)
                {
                    var supplier = _DataContext.SupplierSet.Single(a => a.id == _SupplierId);
                    supplier.CompanyName = txtCompanyName.Text.Trim();
                    supplier.CompanyRegistration = txtCompanyReg.Text.Trim();
                    supplier.VATNumber = txtVatNumber.Text.Trim();
                    supplier.ContactPerson = txtContactPerson.Text.Trim();
                    supplier.EmailAddress = txtEmailAddress.Text.Trim();
                    supplier.ContactNumber = txtContactNumber.Text.Trim();
                    if (supplier.BlackListed != chkIsBlackListed.Checked)
                        supplier.BlacklistedUserId = Controller.user.id;
                    supplier.BlackListed = chkIsBlackListed.Checked;
                    supplier.BlackListReason = txtBlackListReason.Text.Trim();


                    if (cmbBuilding.SelectedItem != null && cbBanks.SelectedItem != null && !supplierOnly)
                    {
                        var bank = cbBanks.SelectedItem as Astrodon.Data.BankData.Bank;
                        var buildingId = (cmbBuilding.SelectedItem as Building).ID;
                        var buildingItem = _DataContext.SupplierBuildingSet.Include(a => a.Bank).SingleOrDefault(a => a.BuildingId == buildingId && a.SupplierId == supplier.id);
                        if (buildingItem == null)
                        {
                            buildingItem = new Data.SupplierData.SupplierBuilding();
                            buildingItem.BuildingId = buildingId;
                            buildingItem.SupplierId = supplier.id;

                            _DataContext.SupplierBuildingSet.Add(buildingItem);
                        }

                        LoadBuildingAudit(buildingItem);


                        buildingItem.BankId = bank.id;
                        buildingItem.Bank = bank;
                        buildingItem.AccountNumber = txtAccountNumber.Text;
                        buildingItem.BranceCode = txtBranchCode.Text;
                        buildingItem.BranchName = txtBranch.Text;

                    }

                    TrackSupplierChanges(_Supplier, supplier);

                    _DataContext.SaveChanges();

                }
                else
                {
                    var supplier = _DataContext.SupplierSet.FirstOrDefault(a => a.CompanyName == txtCompanyName.Text.Trim());

                    if (supplier != null)
                    {
                        Controller.HandleError("Supplier with the same name already exists.", "Save Error");
                        return;
                    }
                    else
                    {
                        var supplierItem = new Data.SupplierData.Supplier()
                        {
                            CompanyName = txtCompanyName.Text.Trim(),
                            CompanyRegistration = txtCompanyReg.Text.Trim(),
                            VATNumber = txtVatNumber.Text.Trim(),
                            ContactPerson = txtContactPerson.Text.Trim(),
                            EmailAddress = txtEmailAddress.Text.Trim(),
                            ContactNumber = txtContactNumber.Text.Trim(),
                            BlackListed = chkIsBlackListed.Checked,
                            BlackListReason = txtBlackListReason.Text.Trim(),
                        };

                        _DataContext.SupplierSet.Add(supplierItem);

                        if (cmbBuilding.SelectedItem != null && cbBanks.SelectedItem != null)
                        {
                            var bank = cbBanks.SelectedItem as Astrodon.Data.BankData.Bank;
                            var buildingId = (cmbBuilding.SelectedItem as Building).ID;

                            _DataContext.SupplierBuildingSet.Add(new Data.SupplierData.SupplierBuilding()
                            {
                                Supplier = supplierItem,
                                BankId = bank.id,
                                BuildingId = buildingId,
                                AccountNumber = txtAccountNumber.Text,
                                BranceCode = txtBranchCode.Text,
                                BranchName = txtBranch.Text,
                            });
                        }

                        _DataContext.SaveChanges();
                        _SupplierId = supplierItem.id;

                    }
                }
                if (_closeOnSave)
                    RaiseSaveSuccess();
                else
                {
                    _buildingId = null;
                    PopulateForm();
                }

            }
        }

        private void LoadBuildingAudit(SupplierBuilding updatedBuildingItem)
        {
            var bank = cbBanks.SelectedItem as Astrodon.Data.BankData.Bank;

            if (updatedBuildingItem.BankId != bank.id)
            {
                _DataContext.SupplierBuildingAuditSet.Add(new SupplierBuildingAudit()
                {
                    SupplierBuilding = updatedBuildingItem,
                    UserId = Controller.user.id,
                    AuditTimeStamp = DateTime.Now,
                    FieldName = "Bank",
                    OldValue = updatedBuildingItem.Bank != null ? updatedBuildingItem.Bank.Name : string.Empty,
                    NewValue = bank.Name
                });
            }
            if (updatedBuildingItem.BranchName != txtBranch.Text)
            {
                _DataContext.SupplierBuildingAuditSet.Add(new SupplierBuildingAudit()
                {
                    SupplierBuilding = updatedBuildingItem,
                    UserId = Controller.user.id,
                    AuditTimeStamp = DateTime.Now,
                    FieldName = "BranchName",
                    OldValue = updatedBuildingItem.BranchName,
                    NewValue = txtBranch.Text
                });
            }
            if (updatedBuildingItem.BranceCode != txtBranchCode.Text)
            {
                _DataContext.SupplierBuildingAuditSet.Add(new SupplierBuildingAudit()
                {
                    SupplierBuilding = updatedBuildingItem,
                    UserId = Controller.user.id,
                    AuditTimeStamp = DateTime.Now,
                    FieldName = "BranceCode",
                    OldValue = updatedBuildingItem.BranceCode,
                    NewValue = txtBranchCode.Text
                });
            }
            if (updatedBuildingItem.AccountNumber != txtAccountNumber.Text)
            {
                _DataContext.SupplierBuildingAuditSet.Add(new SupplierBuildingAudit()
                {
                    SupplierBuilding = updatedBuildingItem,
                    UserId = Controller.user.id,
                    AuditTimeStamp = DateTime.Now,
                    FieldName = "AccountNumber",
                    OldValue = updatedBuildingItem.AccountNumber,
                    NewValue = txtAccountNumber.Text
                });
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RaiseCancel();
        }

        #region Helper Functions

        private void BindInputs()
        {
            txtCompanyName.Text = _Supplier.CompanyName;
            txtCompanyReg.Text = _Supplier.CompanyRegistration;
            txtVatNumber.Text = _Supplier.VATNumber;
            txtContactPerson.Text = _Supplier.ContactPerson;
            txtEmailAddress.Text = _Supplier.EmailAddress;
            txtContactNumber.Text = _Supplier.ContactNumber;
            chkIsBlackListed.Checked = _Supplier.BlackListed;
            txtBlackListReason.Text = _Supplier.BlackListReason;

            cbBanks.SelectedIndex = -1;
            if (_Buildings.Count > 0)
                cmbBuilding.SelectedIndex = -1;
            txtBranch.Text = "";
            txtBranchCode.Text = "";
            txtAccountNumber.Text = "";

        }

        private void LoadSupplierBuilding(int supplierId, int buildingId)
        {
            bool storePromptBankEnabled = promptBankEnabled;

            var supplierBuilding = _DataContext.SupplierBuildingSet.Include(a => a.AuditRecords).SingleOrDefault(a => a.SupplierId == supplierId && a.BuildingId == buildingId);
            lbBuildingName.Text = _DataContext.tblBuildings.Where(a => a.id == buildingId).Select(a => a.Building).Single();
            lbBuildingAuditTrail.Text = lbBuildingName.Text + " bank details audit trail";
            _buildingId = buildingId;

            LoadBuildings();


            _BuildingAuditTrailData = _DataContext.SupplierBuildingAuditSet
                             .Where(a => a.SupplierBuilding.SupplierId == supplierId && a.SupplierBuilding.BuildingId == buildingId)
                             .Select(a => new SupplierAuditTrailResult
                             {
                                 AuditDate = a.AuditTimeStamp,
                                 UserName = a.User.username,
                                 FieldName = a.FieldName,
                                 OldValue = a.OldValue,
                                 NewValue = a.NewValue
                             })
                             .OrderByDescending(a => a.AuditDate)
                             .ToList();
            BindBuildingAuditTrailDataGrid();

            if (supplierBuilding != null)
            {
                promptBankEnabled = false;

                cbBanks.SelectedItem = _Banks.SingleOrDefault(a => a.id == supplierBuilding.BankId);
                txtBranch.Text = supplierBuilding.BranchName;
                txtBranchCode.Text = supplierBuilding.BranceCode;
                txtAccountNumber.Text = supplierBuilding.AccountNumber;
                promptBankEnabled = storePromptBankEnabled;

            }

        }

        private void BindBuildingAuditTrailDataGrid()
        {
            dgBuildingAuditTrail.ClearSelection();
            dgBuildingAuditTrail.MultiSelect = false;
            dgBuildingAuditTrail.AutoGenerateColumns = false;

            dgBuildingAuditTrail.Columns.Clear();

            if (_BuildingAuditTrailData.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = _BuildingAuditTrailData;

                dgBuildingAuditTrail.DataSource = bs;

                dgBuildingAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "AuditDateString",
                    HeaderText = "Date",
                    ReadOnly = true
                });

                dgBuildingAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "UserName",
                    HeaderText = "User",
                    ReadOnly = true
                });

                dgBuildingAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "FieldName",
                    HeaderText = "Field",
                    ReadOnly = true
                });

                dgBuildingAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "OldValue",
                    HeaderText = "Old Value",
                    ReadOnly = true
                });

                dgBuildingAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "NewValue",
                    HeaderText = "New Value",
                    ReadOnly = true
                });

                dgBuildingAuditTrail.AutoResizeColumns();
            }
        }


        private void BindAuditTrailDataGrid()
        {
            dgAuditTrail.ClearSelection();
            dgAuditTrail.MultiSelect = false;
            dgAuditTrail.AutoGenerateColumns = false;

            dgAuditTrail.Columns.Clear();

            if (_AuditTrailData.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = _AuditTrailData;

                dgAuditTrail.DataSource = bs;

                dgAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "AuditDateString",
                    HeaderText = "Date",
                    ReadOnly = true
                });

                dgAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "UserName",
                    HeaderText = "User",
                    ReadOnly = true
                });

                dgAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "FieldName",
                    HeaderText = "Field",
                    ReadOnly = true
                });

                dgAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "OldValue",
                    HeaderText = "Old Value",
                    ReadOnly = true
                });

                dgAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "NewValue",
                    HeaderText = "New Value",
                    ReadOnly = true
                });

                dgAuditTrail.AutoResizeColumns();
            }
        }

        private void BindBuildingsDataGrid()
        {
            dgBuildings.ClearSelection();
            dgBuildings.MultiSelect = false;
            dgBuildings.AutoGenerateColumns = false;

            dgBuildings.Columns.Clear();

            if (_BuildingData.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = _BuildingData;
                dgBuildings.DataSource = bs;

                dgBuildings.Columns.Add(new DataGridViewButtonColumn()
                {
                    HeaderText = "Action",
                    Text = "Select",
                    UseColumnTextForButtonValue = true,
                });


                dgBuildings.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "BuildingName",
                    HeaderText = "Building",
                    ReadOnly = true
                });
                dgBuildings.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Bank",
                    HeaderText = "Bank",
                    ReadOnly = true
                });
                dgBuildings.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "AccountNumber",
                    HeaderText = "AccountNumber",
                    ReadOnly = true
                });

                dgBuildings.AutoResizeColumns();
            }

            
        }

        private string ValidateForm(bool supplierOnly = false)
        {
            var errors = new List<string>();
            string result = "";

            if (String.IsNullOrWhiteSpace(txtCompanyName.Text))
                errors.Add("Company Name is Required.");

            if (String.IsNullOrEmpty(txtContactPerson.Text))
                errors.Add("Contact Person is Required.");

            if (!supplierOnly)
            {
                if (cmbBuilding.SelectedIndex < 0)
                    errors.Add("Building is Required.");

                if (cbBanks.SelectedIndex < 0)
                    errors.Add("Bank is Required.");

                if (String.IsNullOrEmpty(txtBranch.Text))
                    errors.Add("Branch is Required.");

                if (String.IsNullOrEmpty(txtBranchCode.Text))
                    errors.Add("Branch Code is Required.");

                if (String.IsNullOrEmpty(txtAccountNumber.Text))
                    errors.Add("Account Number is Required.");
            }
            if (string.IsNullOrEmpty(txtEmailAddress.Text) || !(txtEmailAddress.Text.Contains("@") && txtEmailAddress.Text.Contains(".")))
                errors.Add("Invalid Email Address");

            if (errors.Count > 0)
            {
                result += "Please correct the following errors: " + Environment.NewLine + Environment.NewLine;

                foreach (var error in errors)
                {
                    result += error + Environment.NewLine;
                }
            }

            return result;
        }

        private void TrackSupplierChanges(Astrodon.Data.SupplierData.Supplier source, Astrodon.Data.SupplierData.Supplier dest)
        {
            var modified = _DataContext.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified && p.Entity is Astrodon.Data.SupplierData.Supplier);

            foreach (var supplier in modified)
            {
                var audit = (from propertyName in supplier.OriginalValues.PropertyNames
                             where !Equals(supplier.OriginalValues.GetValue<object>(propertyName), supplier.CurrentValues.GetValue<object>(propertyName))
                             select new Data.SupplierData.SupplierAudit()
                             {
                                 AuditTimeStamp = DateTime.Now,
                                 SupplierId = _SupplierId,
                                 UserId = Controller.user.id,
                                 FieldName = propertyName,
                                 OldValue = supplier.OriginalValues.GetValue<object>(propertyName) == null ? null : supplier.OriginalValues.GetValue<object>(propertyName).ToString(),
                                 NewValue = supplier.CurrentValues.GetValue<object>(propertyName) == null ? null : supplier.CurrentValues.GetValue<object>(propertyName).ToString()
                             }).ToList();

                _DataContext.SupplierAuditSet.AddRange(audit);
            }
        }

        #endregion


        private void dgBuildings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectColumnIndex = 0;
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var building = senderGrid.Rows[e.RowIndex].DataBoundItem as BuildingResult;

                if (building != null)
                {
                    if (e.ColumnIndex == selectColumnIndex)
                    {
                        LoadSupplierBuilding(building.SupplierId, building.BuildingId);
                    }
                }
            }
        }

        private void cbBanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (promptBankEnabled == false)
                return;

            var bank = cbBanks.SelectedItem as Astrodon.Data.BankData.Bank;
            if (bank != null)
            {
                if (!String.IsNullOrWhiteSpace(bank.BranchCode) && txtBranchCode.Text != bank.BranchCode)
                {
                    if (MessageBox.Show("Load default branch details?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        txtBranch.Text = bank.BranchName;
                        txtBranchCode.Text = bank.BranchCode;
                    }
                }
                //else
                //{
                //    if (String.IsNullOrWhiteSpace(txtBranchCode.Text) && !String.IsNullOrWhiteSpace(bank.BranchCode))
                //    {
                //        txtBranch.Text = bank.BranchName;
                //        txtBranchCode.Text = bank.BranchCode;
                //    }
                //}
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            RaiseSaveSuccess();
        }

        private void btnNewBuilding_Click(object sender, EventArgs e)
        {
            _buildingId = null;
            LoadBuildings();
            PopulateForm();
        }

        private void btnChangeAll_Click(object sender, EventArgs e)
        {

            if (_SupplierId <= 0)
                return;
            if (cbBanks.SelectedItem == null)
                return;

            if (!Controller.AskQuestion("Are you sure you want to update the banking details for ALL buildings?"))
                return;

            var buildingBanks = _DataContext.SupplierBuildingSet.Where(a => a.SupplierId == _SupplierId).ToList();
            var bank = cbBanks.SelectedItem as Astrodon.Data.BankData.Bank;

            foreach (var building in buildingBanks)
            {
                LoadBuildingAudit(building);
                building.BankId = bank.id;
                building.AccountNumber = txtAccountNumber.Text;
                building.BranceCode = txtBranchCode.Text;
                building.BranchName = txtBranch.Text;
            }
            _DataContext.SaveChanges();
            PopulateForm();
            Controller.ShowMessage("Banking Details Changed");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSupplier(true);
        }
    }


    public class BuildingResult
    {
        public bool IsLinked { get; set; }

        public string BuildingName { get; set; }
        public int SupplierBuildingId { get; set; }
        public string Bank { get; set; }
        public int BankId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public int SupplierId { get;  set; }
        public int BuildingId { get;  set; }
    }

    public class SupplierAuditTrailResult
    {

        public DateTime AuditDate { get; set; }

        public string AuditDateString { get { return AuditDate.ToString("yyy-MM-dd"); } }

        public string UserName { get; set; }

        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}

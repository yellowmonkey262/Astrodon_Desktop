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

namespace Astrodon.Controls.Supplier
{
    public partial class usrSupplierDetail : UserControl
    {
        private DataContext _DataContext;
        private List<BuildingResult> _BuildingData;
        private List<SupplierAuditTrailResult> _AuditTrailData;
        private Astrodon.Data.SupplierData.Supplier _Supplier;
        private int _SupplierId;

        public usrSupplierDetail(DataContext context, int supplierId)
        {
            _DataContext = context;
            _SupplierId = supplierId;
            InitializeComponent();
            PopulateForm();
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
            if (_SupplierId > 0)
            {
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

                _BuildingData = (from m in _DataContext.MaintenanceSet.Where(a => a.SupplierId == _SupplierId)
                                 join bc in _DataContext.BuildingMaintenanceConfigurationSet.Include(a => a.Building) on m.BuildingMaintenanceConfigurationId equals bc.id
                                 select new BuildingResult
                                 {
                                     BuildingName = bc.Building.Building,
                                     IsLinked = true
                                 })
                                .Distinct()
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
            var validationResult = ValidateForm();

            if (!String.IsNullOrEmpty(validationResult))
            {
                Controller.HandleError(validationResult, "Validation Error");
                return;
            }
            else
            {
                if(_SupplierId > 0)
                {
                    var supplier = _DataContext.SupplierSet.Single(a => a.id == _SupplierId);
                    supplier.CompanyName = txtCompanyName.Text.Trim();
                    supplier.CompanyRegistration = txtCompanyReg.Text.Trim();
                    supplier.VATNumber = txtVatNumber.Text.Trim();
                    supplier.ContactPerson = txtContactPerson.Text.Trim();
                    supplier.EmailAddress = txtEmailAddress.Text.Trim();
                    supplier.ContactNumber = txtContactNumber.Text.Trim();
                    supplier.BlackListed = chkIsBlackListed.Checked;
                    supplier.BlackListReason = txtBlackListReason.Text.Trim();
                    //supplier.BankName = txtBank.Text.Trim();
                    //supplier.BranchName = txtBranch.Text.Trim();
                    //supplier.BranceCode = txtBranchCode.Text.Trim();
                    //supplier.AccountNumber = txtAccountNumber.Text.Trim();

                    TrackSupplierChanges(_Supplier, supplier);

                    _DataContext.SaveChanges();

                    RaiseSaveSuccess();
                }
                else
                {
                    var supplier = _DataContext.SupplierSet.FirstOrDefault(a => a.CompanyName == txtCompanyName.Text.Trim());

                    if(supplier != null)
                    {
                        Controller.HandleError("Supplier with the same name already exists.", "Save Error");
                        return;
                    }
                    else
                    {
                        _DataContext.SupplierSet.Add(new Data.SupplierData.Supplier()
                        {
                            CompanyName = txtCompanyName.Text.Trim(),
                            CompanyRegistration = txtCompanyReg.Text.Trim(),
                            VATNumber = txtVatNumber.Text.Trim(),
                            ContactPerson = txtContactPerson.Text.Trim(),
                            EmailAddress = txtEmailAddress.Text.Trim(),
                            ContactNumber = txtContactNumber.Text.Trim(),
                            BlackListed = chkIsBlackListed.Checked,
                            BlackListReason = txtBlackListReason.Text.Trim(),
                            //BankName = txtBank.Text.Trim(),
                            //BranchName = txtBranch.Text.Trim(),
                            //BranceCode = txtBranchCode.Text.Trim(),
                            //AccountNumber = txtAccountNumber.Text.Trim()
                        });

                        _DataContext.SaveChanges();

                        RaiseSaveSuccess();
                    }
                }
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
            //txtBank.Text = _Supplier.BankName;
            //txtBranch.Text = _Supplier.BranchName;
            //txtBranchCode.Text = _Supplier.BranceCode;
            //txtAccountNumber.Text = _Supplier.AccountNumber;
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


                dgBuildings.Columns.Add(new DataGridViewCheckBoxColumn()
                {
                    DataPropertyName = "IsLinked",
                    ReadOnly = true
                });

                dgBuildings.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "BuildingName",
                    HeaderText = "Building",
                    ReadOnly = true
                });

                dgBuildings.AutoResizeColumns();
            }
        }

        private string ValidateForm()
        {
            var errors = new List<string>();
            string result = "";

            if (String.IsNullOrWhiteSpace(txtCompanyName.Text))
                errors.Add("Company Name is Required.");

            if(String.IsNullOrEmpty(txtContactPerson.Text))
                errors.Add("Contact Person is Required.");

            if (String.IsNullOrEmpty(txtBank.Text))
                errors.Add("Bank is Required.");

            if (String.IsNullOrEmpty(txtBranch.Text))
                errors.Add("Branch is Required.");

            if (String.IsNullOrEmpty(txtBranchCode.Text))
                errors.Add("Branch Code is Required.");

            if (String.IsNullOrEmpty(txtAccountNumber.Text))
                errors.Add("Account Number is Required.");

            if (string.IsNullOrEmpty(txtEmailAddress.Text) && !(txtEmailAddress.Text.Contains("@") && txtEmailAddress.Text.Contains(".")))
                errors.Add("Invalid Email Address");

            if(errors.Count > 0)
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

        public class BuildingResult
        {
            public bool IsLinked { get; set; }

            public string BuildingName { get; set; }
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
}

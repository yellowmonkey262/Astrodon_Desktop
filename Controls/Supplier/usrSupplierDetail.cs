using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;

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

        private void PopulateForm()
        {
            if (_SupplierId > 0)
            {
                _Supplier = _DataContext.SupplierSet.Single(a => a.id == _SupplierId);

                _AuditTrailData = _DataContext.SupplierAuditSet
                                  .Where(a => a.SupplierId == _SupplierId)
                                  .Select(a => new SupplierAuditTrailResult
                                  {
                                      AuditTimeStamp = a.AuditTimeStamp,
                                      UserName = a.User.username,
                                      FieldName = a.FieldName,
                                      OldValue = a.OldValue,
                                      NewValue = a.NewValue
                                  }).ToList();

                _BuildingData = (from m in _DataContext.MaintenanceSet.Where(a => a.SupplierId == _SupplierId)
                                 join bc in _DataContext.BuildingMaintenanceConfigurationSet on m.BuildingMaintenanceConfigurationId equals bc.id
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

        public class BuildingResult
        {
            public bool IsLinked { get; set; }

            public string BuildingName { get; set; }
        }

        public class SupplierAuditTrailResult
        {
            public DateTime AuditTimeStamp { get; set; }

            public string UserName { get; set; }

            public string FieldName { get; set; }

            public string OldValue { get; set; }

            public string NewValue { get; set; }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var validationResult = ValidateForm();

            if (!String.IsNullOrEmpty(validationResult))
            {
                Controller.HandleError(validationResult, "Validation Error");
                return;
            }
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
        }

        private void BindAuditTrailDataGrid()
        {
            dgAuditTrail.ClearSelection();
            dgAuditTrail.MultiSelect = false;
            dgAuditTrail.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = _AuditTrailData;

            dgAuditTrail.Columns.Clear();

            dgAuditTrail.DataSource = bs;

            dgAuditTrail.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AuditTimeStamp",
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

        private void BindBuildingsDataGrid()
        {
            dgBuildings.ClearSelection();
            dgBuildings.MultiSelect = false;
            dgBuildings.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = _AuditTrailData;

            dgBuildings.Columns.Clear();

            dgBuildings.DataSource = bs;

            dgBuildings.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                DataPropertyName = "IsLinked",
                HeaderText = "Date",
                ReadOnly = true,
            });

            dgBuildings.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingName",
                HeaderText = "Buildings",
                ReadOnly = true
            });

            dgBuildings.AutoResizeColumns();
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

            if (!(txtEmailAddress.Text.Contains("@") && txtEmailAddress.Text.Contains(".")))
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

        #endregion
    }
}

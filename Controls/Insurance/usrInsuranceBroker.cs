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
using Astrodon.Data.InsuranceData;

namespace Astrodon.Controls.Insurance
{
    public partial class usrInsuranceBrokerDetail : UserControl
    {
        private DataContext _DataContext;
        private Astrodon.Data.InsuranceData.InsuranceBroker _InsuranceBroker;
        private int _InsuranceBrokerId;
        private int _userid;
        private bool _closeOnSave = false;

        public usrInsuranceBrokerDetail(DataContext context, int insuranceBrokerId, bool closeOnSave)
        {
            InitializeComponent();
            _closeOnSave = closeOnSave;
            _DataContext = context;
            _userid = Controller.user.id;
            _InsuranceBrokerId = insuranceBrokerId;
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

            if (_InsuranceBrokerId > 0)
            {
                _InsuranceBroker = _DataContext.InsuranceBrokerSet.Single(a => a.id == _InsuranceBrokerId);
              
            }
            else
            {
                _InsuranceBroker = new Data.InsuranceData.InsuranceBroker();
             
            }

            BindInputs();
         
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
                if (_InsuranceBrokerId > 0)
                {
                    var InsuranceBroker = _DataContext.InsuranceBrokerSet.Single(a => a.id == _InsuranceBrokerId);
                    InsuranceBroker.CompanyName = txtCompanyName.Text.Trim();
                    InsuranceBroker.CompanyRegistration = txtCompanyReg.Text.Trim();
                    InsuranceBroker.VATNumber = txtVatNumber.Text.Trim();
                    InsuranceBroker.ContactPerson = txtContactPerson.Text.Trim();
                    InsuranceBroker.EmailAddress = txtEmailAddress.Text.Trim();
                    InsuranceBroker.ContactNumber = txtContactNumber.Text.Trim();
                    _DataContext.SaveChanges();

                }
                else
                {
                    var InsuranceBroker = _DataContext.InsuranceBrokerSet.FirstOrDefault(a => a.CompanyName == txtCompanyName.Text.Trim());

                    if (InsuranceBroker != null)
                    {
                        Controller.HandleError("InsuranceBroker with the same name already exists.", "Save Error");
                        return;
                    }
                    else
                    {
                        var InsuranceBrokerItem = new Data.InsuranceData.InsuranceBroker()
                        {
                            CompanyName = txtCompanyName.Text.Trim(),
                            CompanyRegistration = txtCompanyReg.Text.Trim(),
                            VATNumber = txtVatNumber.Text.Trim(),
                            ContactPerson = txtContactPerson.Text.Trim(),
                            EmailAddress = txtEmailAddress.Text.Trim(),
                            ContactNumber = txtContactNumber.Text.Trim(),
                        };

                        _DataContext.InsuranceBrokerSet.Add(InsuranceBrokerItem);
                        _DataContext.SaveChanges();
                        _InsuranceBrokerId = InsuranceBrokerItem.id;
                    }
                }
                if (_closeOnSave)
                    RaiseSaveSuccess();
                else
                {
                    PopulateForm();
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
            txtCompanyName.Text = _InsuranceBroker.CompanyName;
            txtCompanyReg.Text = _InsuranceBroker.CompanyRegistration;
            txtVatNumber.Text = _InsuranceBroker.VATNumber;
            txtContactPerson.Text = _InsuranceBroker.ContactPerson;
            txtEmailAddress.Text = _InsuranceBroker.EmailAddress;
            txtContactNumber.Text = _InsuranceBroker.ContactNumber;
        }
    
        private string ValidateForm()
        {
            var errors = new List<string>();
            string result = "";

            if (String.IsNullOrWhiteSpace(txtCompanyName.Text))
                errors.Add("Company Name is Required.");

            if (String.IsNullOrEmpty(txtContactPerson.Text))
                errors.Add("Contact Person is Required.");

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

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            RaiseSaveSuccess();
        }

        private void btnNewBuilding_Click(object sender, EventArgs e)
        {
            PopulateForm();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Data.MaintenanceData;
using Astrodon.Data.SupplierData;
using Astradon.Data.Utility;
using Astrodon.Data.Base;

namespace Astrodon.Controls.Maintenance
{
    public partial class usrCaptureMaintenance : UserControl
    {
        private BuildingMaintenanceConfiguration _Config;
        private tblRequisition _Requisition;

        public usrCaptureMaintenance(DataContext context, int maintenanceId)
        {

        }

        public usrCaptureMaintenance(DataContext context, tblRequisition requisition, BuildingMaintenanceConfiguration config)
        {
            if (requisition.SupplierId == null)
                throw new Exception("No Supplier found.");
            else if (requisition.Supplier == null)
                requisition.Supplier = context.SupplierSet.Single(a => a.id == requisition.SupplierId);

            InitializeComponent();
            _Requisition = requisition;
            _Config = config;

            BindForm();
            BindCustomers();
            BindWarrantyDurationType();
        }

        private void BindForm()
        {
            lblBuildingName.Text = _Config.Building.Building;
            lblMaintenanceType.Text = _Config.Name;
            lblLedgerAccount.Text = _Config.PastelAccountName;
            lblClassification.Text = NameSplitting.SplitCamelCase(_Config.Classification.ToString());
            lblTotalAmount.Text = "R" + _Requisition.amount.ToString("#,###.00");
            lblSupplier.Text = _Requisition.Supplier.CompanyName;
            lblCompanyReg.Text = _Requisition.Supplier.CompanyRegistration;
            lblVat.Text = _Requisition.Supplier.VATNumber;
            lblContactPerson.Text = _Requisition.Supplier.ContactPerson;
            lblContactNumber.Text = _Requisition.Supplier.ContactNumber;
            lblEmail.Text = _Requisition.Supplier.EmailAddress;
            lblInvoiceNumber.Text = _Requisition.InvoiceNumber;
            lblInvoiceDate.Text = _Requisition.InvoiceDate.HasValue ? _Requisition.InvoiceDate.Value.ToString("yyyy/MM/dd") : "";
        }

        private void BindCustomers()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                try
                {
                    var customers = new List<StringKeyValue>();
                    customers.Add(new StringKeyValue() { Id = string.Empty, Value = "Body Corporate" });

                    var loadedCustomers = Controller.pastel.GetCustomers(_Config.Building.DataPath);

                    customers.AddRange(loadedCustomers.Select(a => new StringKeyValue()
                    {
                        Id = a.Split('|')[2],
                        Value = a.Split('|')[3]
                    }));

                    cbUnit.DataSource = customers;
                    cbUnit.ValueMember = "Id";
                    cbUnit.DisplayMember = "Display";
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

        private void BindWarrantyDurationType()
        {
            var durationTypes = new List<KeyValuePair<DurationType, string>>();
            
            foreach (DurationType item in Enum.GetValues(typeof(DurationType)))
            {
                durationTypes.Add(new KeyValuePair<DurationType, string>(item, item.ToString()));
            }

            cbWarrantyDurationType.DataSource = durationTypes;
            cbWarrantyDurationType.ValueMember = "Key";
            cbWarrantyDurationType.DisplayMember = "Value";
        }
    }
}

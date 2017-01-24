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
    public partial class usrSupplierLookup : UserControl
    {
        private DataContext _DataContext;
        private List<SupplierResult> _SupplierData;

        public usrSupplierLookup(DataContext context)
        {
            _DataContext = context;
            InitializeComponent();
        }

        #region Supplier Lookup Events

        public event SupplierSelectedEventHandler SupplierSelectedEvent;

        private void SupplierSelected(Astrodon.Data.SupplierData.Supplier selectedItem)
        {
            if (SupplierSelectedEvent != null)
                SupplierSelectedEvent(this, new SupplierEventArgs(selectedItem));
        }

        private void SupplierCancelled()
        {
            if (SupplierSelectedEvent != null)
                SupplierSelectedEvent(this, new SupplierEventArgs());
        }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string companyName = txtCompanyName.Text.Trim();
            string companyReg = txtCompanyReg.Text.Trim();
            string contactPerson = txtContactPerson.Text.Trim();
            string contactNumber = txtContactNumber.Text.Trim();

            _SupplierData = _DataContext.SupplierSet
                            .Where(a => (a.CompanyName.StartsWith(companyName) || companyName == "")
                              && (a.CompanyRegistration.StartsWith(companyReg) || companyReg == "")
                              && (a.ContactPerson.StartsWith(contactPerson) || contactPerson == "")
                              && (a.ContactNumber.StartsWith(contactNumber) || contactNumber == ""))
                            .Select(a => new SupplierResult()
                            {
                                IsBlackListed = a.BlackListed,
                                CompanyName = a.CompanyName,
                                CompanyRegistration = a.CompanyRegistration,
                                ContactPerson = a.ContactPerson,
                                ContactNumber = a.ContactNumber,
                                IsLinkedToBuilding = a.Maintenance.Any()
                            })
                            .OrderBy(a => a.CompanyName).ToList();

            BindSupplierDataGrid();

            btnNewSupplier.Visible = true;

            this.Cursor = Cursors.Default;
        }

        private void BindSupplierDataGrid()
        {
            dgSuppliers.ClearSelection();
            dgSuppliers.MultiSelect = false;
            dgSuppliers.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = _SupplierData;

            dgSuppliers.Columns.Clear();

            dgSuppliers.DataSource = bs;

            dgSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BlackListed",
                HeaderText = "Black Listed",
                ReadOnly = true
            });

            dgSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CompanyName",
                HeaderText = "Name",
                ReadOnly = true
            });

            dgSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CompanyRegistration",
                HeaderText = "Registration Number",
                ReadOnly = true
            });

            dgSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ContactPerson",
                HeaderText = "Contact Person",
                ReadOnly = true
            });

            dgSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ContactNumber",
                HeaderText = "Contact Number",
                ReadOnly = true
            });

            dgSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ContactNumber",
                HeaderText = "Contact Number",
                ReadOnly = true
            });


            dgSuppliers.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true
            });

            dgSuppliers.AutoResizeColumns();
        }

        public class SupplierResult
        {
            public bool IsBlackListed { get; set; }

            public string CompanyName { get; set; }

            public string CompanyRegistration { get; set; }

            public string ContactPerson { get; set; }

            public string ContactNumber { get; set; }

            public bool IsLinkedToBuilding { get; set; }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Forms;

namespace Astrodon.Controls.Supplier
{
    public partial class usrSupplierLookup : UserControl
    {
        private DataContext _DataContext;
        private List<SupplierResult> _SupplierData;
        private SupplierResult _SelectedSupplier;
        private bool _IsSelectDialog;

        public usrSupplierLookup(DataContext context, bool isSelectDialog = false)
        {
            _DataContext = context;
            _IsSelectDialog = isSelectDialog;

            InitializeComponent();

            if (!_IsSelectDialog)
                lblTitle.Text = "Supplier Maintenance";
        }

        #region Supplier Lookup Events

        public event SupplierSelectedEventHandler SupplierSelectedEvent;

        private void SupplierSelected(Astrodon.Data.SupplierData.Supplier selectedItem)
        {
            if (selectedItem.BlackListed == false)
            {

                if (SupplierSelectedEvent != null)
                    SupplierSelectedEvent(this, new SupplierSelectEventArgs(selectedItem));
            }
            else
            {
                string username = "Unknown";
                if (selectedItem.BlacklistedUserId != null)
                    username = _DataContext.tblUsers.Where(a => a.id == selectedItem.BlacklistedUserId.Value).Select(a => a.name).SingleOrDefault();

                if (String.IsNullOrWhiteSpace(username))
                    username = "Unknown";

                Controller.HandleError("The supplier was blacklisted by "+username + " for " + selectedItem.BlackListReason + "\n"+
                    "Please cleare the blacklist to proceeding.", "Validation Error");
            }
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
                                SupplierId = a.id,
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

        private void btnNewSupplier_Click(object sender, EventArgs e)
        {
            var frmSupplierDetail = new frmSupplierDetail(_DataContext, 0);
            var dialogResult = frmSupplierDetail.ShowDialog();

            if (dialogResult == DialogResult.OK)
                btnSearch.PerformClick();
        }

        private void dgSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            int editColumnIndex = -1;
            int selectColumnIndex = -1;

            if (_IsSelectDialog)
            {
                editColumnIndex = senderGrid.Columns.Count - 1;
                selectColumnIndex = senderGrid.Columns.Count - 2;
            }
            else
            {
                editColumnIndex = senderGrid.Columns.Count - 1;
            }

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _SelectedSupplier = senderGrid.Rows[e.RowIndex].DataBoundItem as SupplierResult;

                if (_SelectedSupplier != null)
                {
                    if (e.ColumnIndex == selectColumnIndex)
                    {
                        var supplier = _DataContext.SupplierSet.Single(a => a.id == _SelectedSupplier.SupplierId);
                        SupplierSelected(supplier);
                    }
                    else if (e.ColumnIndex == editColumnIndex)
                    {
                        var frmSupplierDetail = new frmSupplierDetail(_DataContext, _SelectedSupplier.SupplierId);
                        var dialogResult = frmSupplierDetail.ShowDialog();

                        if (dialogResult == DialogResult.OK)
                            btnSearch.PerformClick();
                    }
                }
            }
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
                DataPropertyName = "IsBlackListedString",
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
                DataPropertyName = "LinkedToBuildingString",
                HeaderText = "Linked To Building",
                ReadOnly = true
            });

            if (_IsSelectDialog)
            {
                dgSuppliers.Columns.Add(new DataGridViewButtonColumn()
                {
                    HeaderText = "Action",
                    Text = "Select",
                    UseColumnTextForButtonValue = true,
                });
            }
            
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
            public int SupplierId { get; set; }

            public bool IsBlackListed { get; set; }

            public string IsBlackListedString { get { return IsBlackListed == true ? "Yes" : "No"; } }

            public string CompanyName { get; set; }

            public string CompanyRegistration { get; set; }

            public string ContactPerson { get; set; }

            public string ContactNumber { get; set; }

            public bool IsLinkedToBuilding { get; set; }

            public string LinkedToBuildingString { get { return IsLinkedToBuilding == true ? "Yes" : "No"; } }
        }
    }
}

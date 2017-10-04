using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Forms;

namespace Astrodon.Controls.Insurance
{
    public partial class usrInsuranceBrokerLookup : UserControl
    {
        private DataContext _DataContext;
        private List<InsuranceBrokerResult> _InsuranceBrokerData;
        private InsuranceBrokerResult _SelectedInsuranceBroker;
        private bool _IsSelectDialog;

        public usrInsuranceBrokerLookup(DataContext context, bool isSelectDialog = false)
        {
            _DataContext = context;
            _IsSelectDialog = isSelectDialog;
            InitializeComponent();

            if (!_IsSelectDialog)
                lblTitle.Text = "Insurance Broker Maintenance";

            LoadInsuranceBrokersLinkedTo();
        }


        #region InsuranceBroker Lookup Events

        public event InsuranceBrokerSelectedEventHandler InsuranceBrokerSelectedEvent;

        private void InsuranceBrokerSelected(Astrodon.Data.InsuranceData.InsuranceBroker selectedItem)
        {
            if (InsuranceBrokerSelectedEvent != null)
                InsuranceBrokerSelectedEvent(this, new InsuranceBrokerSelectEventArgs(selectedItem));
        }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string companyName = txtCompanyName.Text.Trim();
            string companyReg = txtCompanyReg.Text.Trim();
            string contactPerson = txtContactPerson.Text.Trim();
            string contactNumber = txtContactNumber.Text.Trim();

            _InsuranceBrokerData = _DataContext.InsuranceBrokerSet
                            .Where(a => (a.CompanyName.StartsWith(companyName) || companyName == "")
                              && (a.CompanyRegistration.StartsWith(companyReg) || companyReg == "")
                              && (a.ContactPerson.StartsWith(contactPerson) || contactPerson == "")
                              && (a.ContactNumber.StartsWith(contactNumber) || contactNumber == ""))
                            .Select(a => new InsuranceBrokerResult()
                            {
                                InsuranceBrokerId = a.id,
                                CompanyName = a.CompanyName,
                                CompanyRegistration = a.CompanyRegistration,
                                ContactPerson = a.ContactPerson,
                                ContactNumber = a.ContactNumber,
                            })
                            .OrderBy(a => a.CompanyName).ToList();

            BindInsuranceBrokerDataGrid();

            btnNewInsuranceBroker.Visible = true;

            this.Cursor = Cursors.Default;
        }


        private void LoadInsuranceBrokersLinkedTo()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                _InsuranceBrokerData = _DataContext.InsuranceBrokerSet
                                .Select(a => new InsuranceBrokerResult()
                                {
                                    InsuranceBrokerId = a.id,
                                    CompanyName = a.CompanyName,
                                    CompanyRegistration = a.CompanyRegistration,
                                    ContactPerson = a.ContactPerson,
                                    ContactNumber = a.ContactNumber,
                                })
                                .OrderBy(a => a.CompanyName).ToList();

                BindInsuranceBrokerDataGrid();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnNewInsuranceBroker_Click(object sender, EventArgs e)
        {
            var frmInsuranceBrokerDetail = new frmInsuranceBrokerDetail(_DataContext, 0);
            var dialogResult = frmInsuranceBrokerDetail.ShowDialog();

            if (dialogResult == DialogResult.OK)
                btnSearch.PerformClick();
        }

        private void dgInsuranceBrokers_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                _SelectedInsuranceBroker = senderGrid.Rows[e.RowIndex].DataBoundItem as InsuranceBrokerResult;

                if (_SelectedInsuranceBroker != null)
                {
                    if (e.ColumnIndex == selectColumnIndex)
                    {
                        var InsuranceBroker = _DataContext.InsuranceBrokerSet.Single(a => a.id == _SelectedInsuranceBroker.InsuranceBrokerId);
                        InsuranceBrokerSelected(InsuranceBroker);
                    }
                    else if (e.ColumnIndex == editColumnIndex)
                    {
                        var frmInsuranceBrokerDetail = new frmInsuranceBrokerDetail(_DataContext, _SelectedInsuranceBroker.InsuranceBrokerId);
                        var dialogResult = frmInsuranceBrokerDetail.ShowDialog();

                        if (dialogResult == DialogResult.OK)
                            btnSearch.PerformClick();
                    }
                }
            }
        }

        private void BindInsuranceBrokerDataGrid()
        {
            dgInsuranceBrokers.ClearSelection();
            dgInsuranceBrokers.MultiSelect = false;
            dgInsuranceBrokers.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = _InsuranceBrokerData;

            dgInsuranceBrokers.Columns.Clear();

            dgInsuranceBrokers.DataSource = bs;

            dgInsuranceBrokers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CompanyName",
                HeaderText = "Name",
                ReadOnly = true
            });

            dgInsuranceBrokers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CompanyRegistration",
                HeaderText = "Registration Number",
                ReadOnly = true
            });

            dgInsuranceBrokers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ContactPerson",
                HeaderText = "Contact Person",
                ReadOnly = true
            });

            dgInsuranceBrokers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ContactNumber",
                HeaderText = "Contact Number",
                ReadOnly = true
            });

            if (_IsSelectDialog)
            {
                dgInsuranceBrokers.Columns.Add(new DataGridViewButtonColumn()
                {
                    HeaderText = "Action",
                    Text = "Select",
                    UseColumnTextForButtonValue = true,
                    Width = 100,
                    MinimumWidth = 100
                });
            }
            
            dgInsuranceBrokers.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                Width = 100,
                MinimumWidth = 100
            });

            dgInsuranceBrokers.AutoResizeColumns();
        }

        public class InsuranceBrokerResult
        {
            public int InsuranceBrokerId { get; set; }

            public string CompanyName { get; set; }

            public string CompanyRegistration { get; set; }

            public string ContactPerson { get; set; }

            public string ContactNumber { get; set; }
        }
    }
}

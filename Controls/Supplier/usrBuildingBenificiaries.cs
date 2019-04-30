using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astro.Library.Entities;
using Astrodon.Data.Migrations;

namespace Astrodon.Controls.Supplier
{
    public partial class usrBuildingBenificiaries : UserControl
    {
        private List<Building> _Buildings;
        private List<SupplierBuildingItem> _SupplierList;

        public usrBuildingBenificiaries()
        {
            InitializeComponent();
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var userid = Controller.user.id;
                Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
                _Buildings = bManager.buildings.Where(a => a.Bank_Name == "Nedbank").ToList();
                _Buildings.Insert(0, new Building() { Name = "", ID = 0 });
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSupplierList();
        }

        private void LoadSupplierList()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var building = cmbBuilding.SelectedItem as Building;
                if (building == null)
                    return;

                using (var context = SqlDataHandler.GetDataContext())
                {
                    var q = from s in context.SupplierBuildingSet
                            where s.BuildingId == building.ID
                            select new SupplierBuildingItem
                            {
                                Id = s.id,
                                BuildingAccountNumber = s.Building.bankAccNumber,
                                BuldingAccountName = s.Building.accName,
                                Supplier = s.Supplier.CompanyName,
                                Bank = s.Bank.Name,
                                BranchCode = s.BranceCode,
                                BranchName = s.BranchName,
                                AccountNumber = s.AccountNumber,
                                BeneficiaryReferenceNumber = s.BeneficiaryReferenceNumber,
                                OldBeneficiaryReferenceNumber = s.BeneficiaryReferenceNumber
                            };

                    if (cbAllSuppliers.Checked)
                        _SupplierList = q.OrderBy(a => a.Supplier).ToList();
                    else
                        _SupplierList = q.Where(a => a.BeneficiaryReferenceNumber == null).OrderBy(a => a.Supplier).ToList();
                    LoadSupplierGrid();

                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void LoadSupplierGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";


            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _SupplierList;

            dgItems.Columns.Clear();
            dgItems.ReadOnly = false;
            dgItems.EditMode = DataGridViewEditMode.EditOnEnter;
          
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuldingAccountName",
                HeaderText = "Building Account",
                ReadOnly = true,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingAccountNumber",
                HeaderText = "Building Account Number",
                ReadOnly = true,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Supplier",
                HeaderText = "Supplier",
                ReadOnly = true,
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Bank",
                HeaderText = "Bank",
                ReadOnly = true,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BranchCode",
                HeaderText = "Branch Code",
                ReadOnly = true
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BranchName",
                HeaderText = "BranchName",
                ReadOnly = false,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountNumber",
                HeaderText = "Account Number",
                ReadOnly = false,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BeneficiaryReferenceNumber",
                HeaderText = "Beneficiary Reference Number",
                ReadOnly = false,
                MaxInputLength = 10
            });

            dgItems.DataSource = bs;

            dgItems.AutoResizeColumns();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_SupplierList == null)
                return;

            var itemsToSave = _SupplierList.Where(a => a.BeneficiaryReferenceNumber != a.OldBeneficiaryReferenceNumber).ToList();
            if(itemsToSave.Count > 0 &&  Controller.AskQuestion("Are you sure you want to update " + itemsToSave.Count() + " benificiaries?"))
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        foreach (var src in itemsToSave)
                        {
                            var bankEntry = context.SupplierBuildingSet.Single(a => a.id == src.Id);
                            var audit = new Astrodon.Data.SupplierData.SupplierBuildingAudit()
                            {
                                SupplierBuildingId = bankEntry.id,
                                FieldName = "BeneficiaryReferenceNumber",
                                OldValue = bankEntry.BeneficiaryReferenceNumber,
                                NewValue = src.BeneficiaryReferenceNumber,
                                AuditTimeStamp = DateTime.Now,
                                UserId = Controller.user.id
                            };
                            context.SupplierBuildingAuditSet.Add(audit);
                            bankEntry.BeneficiaryReferenceNumber = src.BeneficiaryReferenceNumber;
                        }
                        context.SaveChanges();
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
                Application.DoEvents();
                LoadSupplierList();
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            Building selectedBuilding = cmbBuilding.SelectedItem as Building;

            if (cmbBuilding.SelectedItem != null)
            {
                if (!Controller.VerifyBuildingDetailsEntered(selectedBuilding.ID))
                {
                    cmbBuilding.SelectedIndex = 0;
                    dgItems.DataSource = null;
                    cbAllSuppliers.Checked = false;
                    return;
                }
            }
        }
    }

    class SupplierBuildingItem
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string BeneficiaryReferenceNumber { get; set; }
        public string OldBeneficiaryReferenceNumber { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BuildingAccountNumber { get; set; }
        public string BuldingAccountName { get; set; }
        public string Supplier { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astro.Library.Entities;
using Astrodon.Data;
using Astrodon.Data.SupplierData;

namespace Astrodon.Controls.Supplier
{
    public partial class usrPreferredSuppliers : UserControl
    {
        List<Building> _Buildings { get; set; }
        List<PreferredSupplierResult> _PreferredSupplierResults { get; set; }
        private PreferredSupplierResult _SelectedSupplier;
        private int _BuildingId;

        public usrPreferredSuppliers()
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
                _Buildings = bManager.buildings.ToList();
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

        private void LoadSuppliers(int buildingId)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                _PreferredSupplierResults = (from s in context.SupplierSet
                                             join sbTemp in context.SupplierBuildingSet on s.id equals sbTemp.SupplierId into sbJoin
                                             from sb in sbJoin.DefaultIfEmpty()

                                             where sb.BuildingId == buildingId

                                             select new PreferredSupplierResult
                                             {
                                                 CompanyName = s.CompanyName,
                                                 ContactPerson = s.ContactPerson,
                                                 ContactNumber = s.ContactNumber,
                                                 SpecialInstructions = sb.SpecialInstructions,
                                                 BuildingId = sb.BuildingId,
                                                 SupplierId = sb.SupplierId
                                             }).ToList();
            }

            BindDataGrid();
        }

        private void BindDataGrid()
        {
            dgvPreferredSuppliers.ClearSelection();
            dgvPreferredSuppliers.MultiSelect = false;
            dgvPreferredSuppliers.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();

            bs.DataSource = _PreferredSupplierResults;

            dgvPreferredSuppliers.Columns.Clear();

            dgvPreferredSuppliers.DataSource = bs;

            dgvPreferredSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CompanyName",
                HeaderText = "Supplier",
                ReadOnly = true,
            });

            dgvPreferredSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ContactPerson",
                HeaderText = "Contact Person",
                ReadOnly = true,
            });

            dgvPreferredSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ContactNumber",
                HeaderText = "Contact Number",
                ReadOnly = true
            });
            dgvPreferredSuppliers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SpecialInstructions",
                HeaderText = "Special Instructions",
                ReadOnly = true
            });
            dgvPreferredSuppliers.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                Width = 30
            });
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            Building selectedBuilding = cmbBuilding.SelectedItem as Building;

            if (cmbBuilding.SelectedItem != null)
            {
                if (!Controller.VerifyBuildingDetailsEntered(selectedBuilding.ID))
                {
                    cmbBuilding.SelectedIndex = 0;
                    return;
                }
            }

            if (cmbBuilding.SelectedIndex >= 0)
            {
                _BuildingId = selectedBuilding.ID;

                LoadSuppliers(_BuildingId);
                lblSupplierNameEdit.Text = "";
                txtSpecialInstructions.Text = "";
            }
        }

        private void dgvPreferredSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _SelectedSupplier = senderGrid.Rows[e.RowIndex].DataBoundItem as PreferredSupplierResult;

                if (_SelectedSupplier != null)
                {
                    txtSpecialInstructions.Text = _SelectedSupplier.SpecialInstructions;
                    txtSpecialInstructions.Focus();
                    lblSupplierNameEdit.Text = _SelectedSupplier.CompanyName;
                }
            }
        }

        private void btnSaveInstructions_Click(object sender, EventArgs e)
        {
            var specialInstructions = txtSpecialInstructions.Text;
            var supplierName = lblSupplierNameEdit.Text;
            if (String.IsNullOrWhiteSpace(specialInstructions) && String.IsNullOrWhiteSpace(supplierName))
            {
                Controller.HandleError("Please select a supplier to edit", "Validation Error");
            }
            else
            {
                using (var context = SqlDataHandler.GetDataContext())
                {

                    var supplier = context.SupplierBuildingSet.Single(a => a.SupplierId == _SelectedSupplier.SupplierId && _BuildingId == a.BuildingId);
                    supplier.SpecialInstructions = specialInstructions;
                    context.SaveChanges();

                }
                LoadSuppliers((cmbBuilding.SelectedItem as Building).ID);
            }
        }

        #region Preferred Suuplier
        public class PreferredSupplierResult
        {
            public string CompanyName { get; set; }

            public string ContactPerson { get; set; }

            public string ContactNumber { get; set; }

            public string SpecialInstructions { get; set; }

            public int BuildingId { get; set; }

            public int SupplierId { get; set; }
        }
        #endregion
    }
}

using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrJournal : UserControl
    {
        private List<Building> buildings;
        private List<Customer> customers;
        private Building building;
        private Customer customer;
        private String centrec;
        private BindingSource bs;

        public usrJournal()
        {
            InitializeComponent();
            buildings = new Buildings(false).buildings;
            LoadDefaultValues();
            bs = new BindingSource();
        }

        private void usrJournal_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            dgJournals.DataSource = bs;
        }

        private void LoadBuildings()
        {
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.Items.Clear();
            cmbBuilding.DataSource = buildings;
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.SelectedIndex = -1;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        private void LoadCustomers()
        {
            cmbCustomer.SelectedIndexChanged -= cmbCustomer_SelectedIndexChanged;
            cmbCustomer.DataSource = null;
            cmbCustomer.Items.Clear();
            cmbCustomer.DataSource = customers;
            cmbCustomer.DisplayMember = "accNumber";
            cmbCustomer.ValueMember = "accNumber";
            cmbCustomer.SelectedIndex = -1;
            cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                building = buildings[cmbBuilding.SelectedIndex];
                customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath);
                customer = null;
                LoadCustomers();
            }
            catch
            {
                customer = null;
                customers = null;
                LoadCustomers();
            }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            customer = customers[cmbCustomer.SelectedIndex];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool hasBuilding = (cmbBuilding.SelectedItem != null);
            bool hasCustomer = (cmbCustomer.SelectedItem != null);
            if (hasBuilding && hasCustomer && !String.IsNullOrEmpty(txtReference.Text) && !String.IsNullOrEmpty(txtDescription.Text) && !String.IsNullOrEmpty(txtCentrec.Text) && !String.IsNullOrEmpty(txtAmount.Text))
            {
                String centrecContra = txtCentrec.Text.Replace("\\", "").Replace("/", "").Replace(" ", "");
                if (centrecContra.Length != 7)
                {
                    MessageBox.Show("Please enter a valid centrec contra", "Journals", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCentrec.Focus();
                    return;
                }
                double amt = 0;
                if (!double.TryParse(txtAmount.Text, out amt))
                {
                    MessageBox.Show("Please enter a valid amount", "Journals", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtAmount.Focus();
                    return;
                }
                Journal journal = new Journal
                {
                    amt = amt.ToString(),
                    bc = building.Centrec_Account,
                    buildAcc = customer.accNumber,
                    buildContra = building.Centrec_Building,
                    buildPath = building.DataPath,
                    buildPeriod = building.Period,
                    buildType = building.Journal,
                    description = txtDescription.Text,
                    rAcc = "",
                    reference = txtReference.Text,
                    trnDate = trnDatePicker.Value,
                    trustAcc = txtCentrec.Text,
                    trustContra = building.Centrec_Building,
                    trustPath = centrec,
                    trustType = 5
                };
                bs.Add(journal);
                ClearForm();
            }
            else
            {
                MessageBox.Show("Please enter all fields", "Journals", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadDefaultValues()
        {
            SqlDataHandler dh = new SqlDataHandler();
            String settingsQuery = "SELECT centrec FROM tblSettings";
            String status;
            DataSet dsDefault = dh.GetData(settingsQuery, null, out status);
            if (dsDefault != null && dsDefault.Tables.Count > 0 && dsDefault.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsDefault.Tables[0].Rows[0];
                centrec = dr["centrec"].ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            building = null;
            customer = null;
            cmbBuilding.SelectedIndex = -1;
            trnDatePicker.Value = DateTime.Now;
            txtReference.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtCentrec.Text = String.Empty;
            txtAmount.Text = String.Empty;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            LoadPrintGrid();
        }

        private void LoadPrintGrid()
        {
            //2-9
            BindingSource bsPrint = new BindingSource();
            DataGridView dgPrint = new DataGridView();
            foreach (DataGridViewRow dr in dgJournals.Rows)
            {
                Journal j = dr.DataBoundItem as Journal;
                bsPrint.Add(j);
            }
            dgPrint.DataSource = bsPrint;
            this.Controls.Add(dgPrint);
            dgPrint.Size = dgJournals.Size;
            dgPrint.Visible = true;
            PrintDGV.Print_DataGridView(dgPrint);
            dgPrint.Visible = false;
            this.Controls.Remove(dgPrint);
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgJournals.Rows.Count; i++)
            {
                Journal journal = (Journal)bs[i];
                String pString;
                String pastelReturn = Controller.pastel.PostBatch(journal.trnDate, journal.buildPeriod, journal.trustPath, journal.buildPath, journal.trustType, journal.buildType, journal.bc, journal.buildAcc, journal.trustContra, journal.buildContra, journal.reference, journal.description, journal.amt, journal.trustAcc, journal.rAcc, out pString);
            }
            ClearForm();
            bs.Clear();
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            ClearForm();
            bs.Clear();
        }
    }
}
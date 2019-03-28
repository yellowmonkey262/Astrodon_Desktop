using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class usrCredits : UserControl
    {
        private List<Building> buildings;
        private List<Customer> customers;
        private bool rememberedPassword = false;
        private Building building;
        private String centrec, business;

        public usrCredits()
        {
            InitializeComponent();
            buildings = new Buildings(false).buildings;
        }

        private void usrCredits_Load(object sender, EventArgs e)
        {
            List<Building> allBuildings = new Buildings(false).buildings;
            LoadDefaultValues();
            buildings = new List<Building>();
            foreach (int bid in Controller.user.buildings)
            {
                foreach (Building b in allBuildings)
                {
                    if (bid == b.ID && !buildings.Contains(b))
                    {
                        buildings.Add(b);
                        break;
                    }
                }
            }
            buildings = buildings.OrderBy(c => c.Name).ToList();
            LoadBuildings();
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
            BindingSource bindingSource2 = new BindingSource();
            if (customers.Count > 0) { bindingSource2.Add(new CustomerRefundList("", "", 0, "")); }
            Dictionary<string, Customer> customerDic = new Dictionary<string, Customer>();
            foreach (Customer customer in customers)
            {
                double os = 0;
                for (int li = 0; li < customer.lastBal.Length; li++) { os += customer.lastBal[li]; }
                for (int i = 0; i < (building.Period - 1 == 0 ? 12 : building.Period - 1); i++) { os += customer.balance[i]; }
                String oss = os.ToString("#0.00");
                os = double.Parse(oss);
                for (int i = 0; i < 5; i++) { bindingSource2.Add(new CustomerRefundList(customer.accNumber, customer.description, os, "")); }
                customerDic.Add(customer.accNumber, customer);
            }
            dgCredits.DataSource = bindingSource2;
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                building = buildings[cmbBuilding.SelectedIndex];
                customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath);
                LoadCustomers();
            }
            catch { }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            LoadPrintingGrid();
        }

        private void LoadPrintingGrid()
        {
            DataGridView dgPrint = new DataGridView();
            BindingSource bsPrint = new BindingSource();
            //amtcol = 3;
            String msg = "";
            foreach (DataGridViewRow dr in dgCredits.Rows)
            {
                try
                {
                    double amt;
                    String accNumber = dr.Cells[0].Value.ToString();
                    String name = dr.Cells[1].Value.ToString();
                    double bal = double.Parse(dr.Cells[2].Value.ToString());
                    String amount = dr.Cells[3].Value.ToString();
                    String note = dr.Cells[4].Value.ToString();
                    String acc = dr.Cells[5].Value.ToString();

                    if (double.TryParse(amount, out amt) && amt != 0)
                    {
                        bsPrint.Add(new CustomerRefundList(accNumber, name, bal, amt, note, acc));
                    }
                }
                catch
                {
                    MessageBox.Show(msg);
                }
            }
            dgPrint.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgPrint_DataBindingComplete);
            dgPrint.DataSource = bsPrint;
            this.Controls.Add(dgPrint);
            dgPrint.Size = dgCredits.Size;
            dgPrint.Visible = true;
            PrintDGV.Print_DataGridView(dgPrint);
            dgPrint.Visible = false;
            this.Controls.Remove(dgPrint);
        }

        private void dgPrint_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                DataGridView dgPrint = sender as DataGridView;
                dgPrint.Columns[4].HeaderText = "Description";
                dgPrint.Columns[5].HeaderText = "Centrec General Code";
            }
            catch { }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (Prompt.ShowDialog("Password", "Credits") == "19780411" || rememberedPassword)
            {
                rememberedPassword = true;
                int colCount = dgCredits.ColumnCount;
                int amtCol = 3;// colCount - 2;
                int noteCol = 4;// colCount - 1;
                int accCol = 5;
                for (int i = 1; i < dgCredits.Rows.Count; i++)
                {
                    DataGridViewRow gvr = dgCredits.Rows[i];
                    String accNumber = gvr.Cells[0].Value.ToString();
                    double amt = 0;
                    String customerAcc = gvr.Cells[0].Value.ToString();
                    String amount = gvr.Cells[amtCol].Value.ToString();
                    String note = gvr.Cells[noteCol].Value.ToString();
                    String acc = gvr.Cells[accCol].Value.ToString();
                    if (double.TryParse(amount, out amt) && note != "" && amt != 0)
                    {
                        acc = acc.Replace("/", "");
                        if (acc.Length != 7)
                        {
                            MessageBox.Show("Please enter a valid account number. " + acc + " is invalid", "Credits", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }
                        if (amt > 0) { amt = amt * -1; }
                        String pastelString = "";

                        String returner = Controller.pastel.PostCredit2(DateTime.Now, building.Period, centrec, building.DataPath, 5, building.Journal, building.Centrec_Account, customerAcc, building.Cash_Book, building.Centrec_Building, note, note, amt.ToString("#0.00"), acc, customerAcc, out pastelString);
                    }
                    else
                    {
                        continue;
                    }
                    gvr.Cells[amtCol].Value = 0;
                    gvr.Cells[noteCol].Value = "";
                }
            }
            else
            {
                MessageBox.Show("Please enter the correct password to process", "Process", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadDefaultValues()
        {
            String settingsQuery = "SELECT minbal, reminder_fee, final_fee, summons_fee, discon_notice_fee, discon_fee, handover_fee, centrec, business FROM tblSettings";
            String status;
            SqlDataHandler dh = new SqlDataHandler();
            DataSet dsDefault = dh.GetData(settingsQuery, null, out status);
            if (dsDefault != null && dsDefault.Tables.Count > 0 && dsDefault.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsDefault.Tables[0].Rows[0];
                centrec = dr["centrec"].ToString();
                business = dr["business"].ToString();
            }
        }

        private void usrCredits_Leave(object sender, EventArgs e)
        {
            rememberedPassword = false;
        }
    }
}
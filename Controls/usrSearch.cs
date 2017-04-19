using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrSearch : UserControl
    {
        private List<Building> buildings;
        private BindingList<SearchResult> searchResults = new BindingList<SearchResult>();
        private Thread myNewThread;
        private bool runSearch = false;

        private delegate void UpdateGridDelegate(Customer c, String buildingName);

        private delegate void ChangeCursorDelegate(Cursor c);

        private delegate void ShowMessageBoxDelegate(String message);

        public usrSearch()
        {
            InitializeComponent();
            Controller.pastel.Message += pastel_Message;
            Controller.pastel.CustomerFound += pastel_CustomerFound;
        }

        private void usrSearch_Load(object sender, EventArgs e)
        {
            buildings = new Buildings(true, "All buildings").buildings;
            cmbBuilding.Items.Clear();
            cmbBuilding.DataSource = buildings;
            cmbBuilding.DisplayMember = "Name";
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.SelectedIndex = -1;
            dgResults.DataSource = searchResults;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbBuilding.SelectedIndex = -1;
            txtCustomer.Text = "";
            txtEmail.Text = "";
            searchResults.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            String customer = txtCustomer.Text;
            String email = txtEmail.Text;
            if (btnSearch.Text == "Search")
            {
                if ((!String.IsNullOrEmpty(customer) || !String.IsNullOrEmpty(email)) && cmbBuilding.SelectedIndex >= 0)
                {
                    Dictionary<String, String> bCriteria = new Dictionary<string, string>();
                    if (cmbBuilding.SelectedValue.ToString() == "0")
                    {
                        foreach (Building b in buildings) { bCriteria.Add(b.Name, b.DataPath); }
                    }
                    else
                    {
                        bCriteria.Add(buildings[cmbBuilding.SelectedIndex].Name, buildings[cmbBuilding.SelectedIndex].DataPath);
                    }
                    List<String> searchCriteria = new List<string>();
                    if (!String.IsNullOrEmpty(customer)) { searchCriteria.Add(customer.Trim()); }
                    if (!String.IsNullOrEmpty(email)) { searchCriteria.Add(email); }
                    searchResults.Clear();
                    btnSearch.Text = "Stop";
                    Application.DoEvents();
                    this.Cursor = Cursors.WaitCursor;
                    myNewThread = new Thread(() => Controller.pastel.SearchCustomers(searchCriteria, bCriteria));
                    myNewThread.Start();
                }
                else
                {
                    MessageBox.Show("Please enter search criteria");
                }
            }
            else
            {
                try
                {
                    Controller.pastel.runSearch = false;
                    myNewThread.Join();
                    this.Cursor = Cursors.Default;
                    btnSearch.Text = "Search";
                }
                catch { }
            }
        }

        private void pastel_CustomerFound(object sender, CustomerArgs e)
        {
            UpdateGrid(e.customer, e.building);
        }

        private void UpdateGrid(Customer c, String buildingName)
        {
            if (InvokeRequired)
            {
                this.Invoke(new UpdateGridDelegate(UpdateGrid), c, buildingName);
            }
            else
            {
                SearchResult sr = new SearchResult
                {
                    building = buildingName,
                    acc = c.accNumber,
                    description = c.description,
                    email = String.Join("; ", c.Email)
                };
                searchResults.Add(sr);
                dgResults.Invalidate();
            }
        }

        private void pastel_Message(object sender, MessageArgs e)
        {
            ShowMessageBox(e.message);
            ChangeCursor(Cursors.Default);
        }

        private void ChangeCursor(Cursor c)
        {
            if (InvokeRequired)
            {
                this.Invoke(new ChangeCursorDelegate(ChangeCursor), c);
            }
            else
            {
                this.Cursor = c;
                btnSearch.Text = "Search";
            }
        }

        private void ShowMessageBox(String message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new ShowMessageBoxDelegate(ShowMessageBox), message);
            }
            else
            {
                MessageBox.Show(this, message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Astrodon.Controls {

    public partial class usrEnvelopes : UserControl {
        private Buildings BuildingManager = new Buildings(true);

        public usrEnvelopes() {
            InitializeComponent();
        }

        private void usrEnvelopes_Load(object sender, EventArgs e) {
            LoadBuildings();
        }

        private void LoadBuildings() {
            for (int i = 0; i < BuildingManager.buildings.Count; i++) {
                chkBuildings.Items.Add(BuildingManager.buildings[i].Name, false);
            }
        }

        private void btnGo_Click(object sender, EventArgs e) {
            //Customer c = new Customer();
            //c.accNumber = "007";
            //c.description = "My customer";
            //c.address = new string[] { "Line 1", "Line 2", "Line 3", "Line 4", "Line 5" };
            //PDF generator = new PDF();
            //List<Customer> customers = new List<Customer>();
            //customers.Add(c);
            //List<String> files = generator.CreateEnvelope(customers, cmbPaper.SelectedIndex);
            //MessageBox.Show("Done");

            if (cmbPaper.SelectedItem != null && cmbPaper.SelectedItem.ToString() != "Please select" && chkBuildings.CheckedItems.Count > 0) {
                //get customers
                List<Customer> allCustomers = new List<Customer>();
                for (int i = 0; i < chkBuildings.Items.Count; i++) {
                    if (chkBuildings.GetItemChecked(i)) {
                        List<Customer> customers = Controller.pastel.AddCustomers("", BuildingManager.buildings[i].DataPath);
                        foreach (Customer c in customers) {
                            String fullAddy = String.Join("", c.address).Trim();
                            bool hasAddress = fullAddy != "";
                            if (!c.accNumber.ToUpper().EndsWith("Z") && hasAddress) { allCustomers.Add(c); }
                        }
                    }
                }
                if (MessageBox.Show("All customer info has been retrieved...Proceed with printing?", "Envelopes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    int envelopeSize = cmbPaper.SelectedIndex;
                    PDF pdfGenerator = new PDF();
                    String outFile = pdfGenerator.CreateEnvelope(allCustomers, envelopeSize);
                    Process.Start(outFile);
                    //foreach (String file in files) { SendToPrinter(file); }
                    MessageBox.Show("Printing complete");
                }
            }
        }

        private void SendToPrinter(String fileName) { //C:\statement.pdf
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = fileName;
            //info.CreateNoWindow = true;
            //info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            //p.WaitForInputIdle();

            //if (false == p.CloseMainWindow()) {
            //p.Kill();
            //}
            System.Threading.Thread.Sleep(5000);
        }
    }
}
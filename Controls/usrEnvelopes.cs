using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrEnvelopes : UserControl
    {
        private Buildings BuildingManager = new Buildings(true);

        public usrEnvelopes()
        {
            InitializeComponent();
        }

        private void usrEnvelopes_Load(object sender, EventArgs e)
        {
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            for (int i = 0; i < BuildingManager.buildings.Count; i++)
            {
                chkBuildings.Items.Add(BuildingManager.buildings[i].Name, false);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (cmbPaper.SelectedItem != null && cmbPaper.SelectedItem.ToString() != "Please select" && chkBuildings.CheckedItems.Count > 0)
            {
                //get customers
                List<Customer> allCustomers = new List<Customer>();
                for (int i = 0; i < chkBuildings.Items.Count; i++)
                {
                    if (chkBuildings.GetItemChecked(i))
                    {
                        List<Customer> customers = Controller.pastel.AddCustomers("", BuildingManager.buildings[i].DataPath, true);
                        foreach (Customer c in customers)
                        {
                            String fullAddy = String.Join("", c.address).Trim();
                            bool hasAddress = fullAddy != "";
                            if (!c.accNumber.ToUpper().EndsWith("Z") && hasAddress) { allCustomers.Add(c); }
                        }
                    }
                }
                if (MessageBox.Show("All customer info has been retrieved...Proceed with printing?", "Envelopes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int envelopeSize = cmbPaper.SelectedIndex;
                    PDF pdfGenerator = new PDF();
                    String outFile = pdfGenerator.CreateEnvelope(allCustomers, envelopeSize);
                    Process.Start(outFile);
                    //foreach (String file in files) { SendToPrinter(file); }
                    MessageBox.Show("Printing complete");
                }
            }
        }
        private void PrintOrViewFile(string outputFileName,string prinername)
        {
            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo
                    {
                        Verb = "print",
                        FileName = outputFileName,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = prinername
                    };
                    p.Start();
                    Thread.Sleep(5000);
                }
            }
            catch (Exception e)
            {
                Controller.HandleError("Unable to print file - the file will now open for manual printing.");
                Process.Start(outputFileName);

            }
        }


        
    }
}
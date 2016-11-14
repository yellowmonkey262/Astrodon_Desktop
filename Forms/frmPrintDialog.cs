using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Astrodon {

    public partial class frmPrintDialog : Form {
        public String selectedPrinter;

        public frmPrintDialog() {
            InitializeComponent();
        }

        private void frmPrintDialog_Load(object sender, EventArgs e) {
            GetDefaultPrinter();
        }

        private String GetDefaultPrinter() {
            String defaultPrinter = String.Empty;
            foreach (string printer in PrinterSettings.InstalledPrinters) {
                comboBox1.Items.Add(printer);
                if (printer == Properties.Settings.Default.defaultPrinter) { defaultPrinter = printer; }
            }
            if (comboBox1.Items.Contains(defaultPrinter)) { comboBox1.SelectedItem = defaultPrinter; }
            return defaultPrinter;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            selectedPrinter = comboBox1.SelectedItem.ToString();
        }
    }
}
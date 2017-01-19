using System;
using System.Windows.Forms;

namespace Astrodon.Forms {

    public partial class frmPrompt : Form {
        public String fileName;
        public String windowText = String.Empty;
        public String labelText = String.Empty;

        public frmPrompt() {
            InitializeComponent();
        }

        public frmPrompt(String winText, String lblText) {
            windowText = winText;
            labelText = lblText;
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(txtFile.Text)) {
                fileName = txtFile.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            } else {
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void frmPrompt_Load(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(windowText)) { this.Text = windowText; }
            if (!String.IsNullOrEmpty(labelText)) { lblPrompt.Text = labelText; }
        }
    }
}
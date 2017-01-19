using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Astrodon.Controls {

    public partial class usrAccounts : UserControl {

        public usrAccounts() {
            InitializeComponent();
        }

        private void usrAccounts_Load(object sender, EventArgs e) {
            List<String> accounts = Controller.pastel.GetAccounts("META");
        }
    }
}
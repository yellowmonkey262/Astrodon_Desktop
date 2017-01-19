using System;
using System.Windows.Forms;

namespace Astrodon {

    public partial class frmLogin : Form {
        public String username;
        public String password;
        public bool failed = false;

        public frmLogin(bool loggedIn) {
            failed = !loggedIn;
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e) {
            if (failed) {
                lblError.Text = "Invalid user name or password provided";
            } else {
                lblError.Text = "";
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            username = txtUsername.Text;
            password = txtPassword.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
        }

        public void UpdateMessage() {
            lblError.Text = "Invalid user name or password provided";
        }
    }
}
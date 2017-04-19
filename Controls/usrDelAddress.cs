using Astro.Library.Entities;
using System;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrDelAddress : UserControl
    {
        private AdditionalAddress add;

        public usrDelAddress(AdditionalAddress aa)
        {
            InitializeComponent();
            add = aa;
        }

        private void usrDelAddress_Load(object sender, EventArgs e)
        {
            txtAddy1.Text = add.Address[0];
            txtAddy2.Text = add.Address[1];
            txtAddy3.Text = add.Address[2];
            txtAddy4.Text = add.Address[3];
            txtAddy5.Text = add.Address[4];
            txtCell.Text = add.Cell;
            txtContact.Text = add.Contact;
            txtEmail.Text = add.Email;
            txtFax.Text = add.Fax;
            txtTel.Text = add.Telephone;
        }
    }
}
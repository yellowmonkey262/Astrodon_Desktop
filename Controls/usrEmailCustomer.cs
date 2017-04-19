using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrEmailCustomer : UserControl
    {
        private List<Building> buildings;
        private Building building;
        private List<Customer> customers;

        public usrEmailCustomer()
        {
            InitializeComponent();
            buildings = new Buildings(false).buildings;
            building = new Building();
            customers = new List<Customer>();
        }

        private void usrEmailCustomer_Load(object sender, EventArgs e)
        {
            LoadBuildings();
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbCustomer.SelectedIndexChanged -= cmbCustomer_SelectedIndexChanged;
                building = buildings[cmbBuilding.SelectedIndex];
                txtBCC.Text = building.PM;
                if (!txtBCC.Text.Contains(Controller.user.email)) { txtBCC.Text += "; " + Controller.user.email; }
                customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath);
                cmbCustomer.Items.Clear();
                foreach (Customer c in customers)
                {
                    cmbCustomer.Items.Add(c.accNumber);
                }
                cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
            }
            catch { }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblCustomer.Text = customers[cmbCustomer.SelectedIndex].description;
                var builder = new System.Text.StringBuilder();
                builder.Append(txtEmail.Text);
                foreach (String email in customers[cmbCustomer.SelectedIndex].Email)
                {
                    builder.Append(email + ";");
                }
                txtEmail.Text = builder.ToString();
                txtMessage.Text = "";
                String mailBody = "Account #: " + customers[cmbCustomer.SelectedIndex].accNumber + ". For any queries on your account, please contact " + building.Debtor + Environment.NewLine + Environment.NewLine;
                mailBody += "For any maintenance queries, please contact " + building.PM + Environment.NewLine + Environment.NewLine;
                mailBody += "Regards" + Environment.NewLine;
                mailBody += "Astrodon (Pty) Ltd" + Environment.NewLine;
                mailBody += "You're in good hands";
                txtMessage.Text += Environment.NewLine + Environment.NewLine + mailBody;
            }
            catch { }
        }

        private void btnAttachment_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                {
                    if (File.Exists(ofd.FileName) && !lstAttachments.Items.Contains(ofd.FileName))
                    {
                        lstAttachments.Items.Add(ofd.FileName);
                    }
                }
            }
        }

        private void btnSendNow_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text != "" && txtSubject.Text != "" && txtMessage.Text != "")
            {
                String[] emails = txtEmail.Text.Split(new String[] { ";" }, StringSplitOptions.None);
                List<String> att = new List<string>();
                foreach (Object obj in lstAttachments.Items) { att.Add(obj.ToString()); }
                String[] attachments = att.ToArray();
                String status;
                if (Mailer.SendDirectMail(Controller.user.email, emails, txtCC.Text, txtBCC.Text, txtSubject.Text, txtMessage.Text, false, true, out status, attachments))
                {
                    MessageBox.Show("Message sent");
                }
                else
                {
                    MessageBox.Show("Message not sent: " + status);
                }
            }
            else
            {
                if (txtEmail.Text == "")
                {
                    MessageBox.Show("Please enter email address");
                }
                else if (txtSubject.Text == "")
                {
                    MessageBox.Show("Please enter subject");
                }
                else if (txtMessage.Text == "")
                {
                    MessageBox.Show("Please enter message");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmbBuilding.SelectedIndex = -1;
            cmbCustomer.SelectedIndex = -1;
            lblCustomer.Text = "";
            txtEmail.Text = "";
            txtCC.Text = "";
            txtBCC.Text = "";
            txtSubject.Text = "";
            txtMessage.Text = "";
            lstAttachments.Items.Clear();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstAttachments.SelectedItems.Count > 0)
            {
                foreach (int idx in lstAttachments.SelectedIndices)
                {
                    lstAttachments.Items.Remove(lstAttachments.Items[idx]);
                }
            }
            else
            {
                MessageBox.Show("Please select attachments to be deleted");
            }
        }
    }
}
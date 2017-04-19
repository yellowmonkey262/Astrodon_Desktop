using Astro.Library.Entities;
using System;
using System.Windows.Forms;

namespace Astrodon.Forms
{
    public partial class frmNewBuilding : Form
    {
        public Building building = null;
        private String oldName = "";
        private String oldAbbr = "";

        public frmNewBuilding(Building b)
        {
            InitializeComponent();
            if (b != null)
            {
                building = b;
                oldName = building.Name;
                oldAbbr = building.Abbr;
            }
            else
            {
                building = new Building
                {
                    ID = 0,
                    pid = "0"
                };
            }
        }

        private void frmNewBuilding_Load(object sender, EventArgs e)
        {
            if (building != null) { LoadBuilding(); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveBuilding();
        }

        private void LoadBuilding()
        {
            txtName.Text = building.Name;
            txtAbbr.Text = building.Abbr;
            txtTrust.Text = building.Trust;
            txtPath.Text = building.DataPath;
            txtPeriod.Text = building.Period.ToString();
            txtCash.Text = building.Cash_Book;
            txtPayment.Text = building.Payments.ToString();
            txtReceipt.Text = building.Receipts.ToString();
            txtJournal.Text = building.Journal.ToString();
            txtCentrec1.Text = building.Centrec_Account;
            txtCentrec2.Text = building.Centrec_Building;
            txtBus.Text = building.Business_Account;
            txtBank.Text = building.Bank;
            txtPM.Text = building.PM;
            txtBankName.Text = building.Bank_Name;
            txtAccNumber.Text = building.Bank_Acc_Number;
            txtAccName.Text = building.Acc_Name;
            txtBranch.Text = building.Branch_Code;
            chkWeb.Checked = building.Web_Building;
            txtLetter.Text = building.letterName;
            txtAddress1.Text = building.addy1;
            txtAddress2.Text = building.addy2;
            txtAddress3.Text = building.addy3;
            txtAddress4.Text = building.addy4;
            txtAddress5.Text = building.addy5;
        }

        private void SaveBuilding()
        {
            building.Name = txtName.Text;
            building.Abbr = txtAbbr.Text;
            building.Trust = txtTrust.Text;
            building.DataPath = txtPath.Text;
            building.Period = int.Parse(txtPeriod.Text);
            building.Cash_Book = txtCash.Text;
            building.Payments = int.Parse(txtPayment.Text);
            building.Receipts = int.Parse(txtReceipt.Text);
            building.Journal = int.Parse(txtJournal.Text);
            building.Centrec_Account = txtCentrec1.Text;
            building.Centrec_Building = txtCentrec2.Text;
            building.Business_Account = txtBus.Text;
            building.Bank = txtBank.Text;
            building.PM = txtPM.Text;
            building.Bank_Name = txtBankName.Text;
            building.Bank_Acc_Number = txtAccNumber.Text;
            building.Acc_Name = txtAccName.Text;
            building.Branch_Code = txtBranch.Text;
            building.Web_Building = chkWeb.Checked;
            building.letterName = txtLetter.Text;
            building.addy1 = txtAddress1.Text;
            building.addy2 = txtAddress2.Text;
            building.addy3 = txtAddress3.Text;
            building.addy4 = txtAddress4.Text;
            building.addy5 = txtAddress5.Text;

            Buildings BuildingManager = new Buildings(true);
            MySqlConnector mysql = new MySqlConnector();
            Classes.Sftp ftpClient = new Classes.Sftp(String.Empty, false);
            String websafeName = building.Name.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
            building.webFolder = websafeName;
            try
            {
                String status = String.Empty;
                if (BuildingManager.Update(building, out status))
                {
                    String newID = "";
                    mysql.InsertBuilding(building, oldName, oldAbbr, out newID, out status);
                    building.pid = newID;
                    BuildingManager.Update(building, out status);
                    if (ftpClient.CreateDirectory(websafeName, false))
                    {
                        ftpClient.WorkingDirectory += "/" + websafeName;
                        ftpClient.ChangeDirectory(false);
                        ftpClient.CreateDirectory("Annual Financial Statements", false);
                        ftpClient.CreateDirectory("Conduct Rules", false);
                        ftpClient.CreateDirectory("Insurance Information", false);
                        ftpClient.CreateDirectory("Meeting Minutes", false);
                        ftpClient.CreateDirectory("Meeting Notices", false);
                        ftpClient.CreateDirectory("Sectional Title Plans", false);
                    }

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Building update failed: " + status, "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Building update failed: " + ex.Message, "Buildings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
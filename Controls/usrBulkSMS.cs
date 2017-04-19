using Astro.Library;
using Astro.Library.Entities;
using Astrodon.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class usrBulkSMS : UserControl
    {
        private Building selectedBuilding;
        private List<Building> buildings;
        private SMSCustomers smsCustomers;
        private SqlDataHandler dh;
        private ClearanceValues values = new ClearanceValues();
        private SMS sms = new SMS();
        private int messageCount = 0;

        private BindingList<SMSCustomer> bs;
        private bool validSMS = false;

        public usrBulkSMS()
        {
            InitializeComponent();
            buildings = new Buildings(false).buildings;
            dh = new SqlDataHandler();
            smsCustomers = new SMSCustomers { customers = new List<SMSCustomer>() };
            bs = new BindingList<SMSCustomer>();
        }

        private void usrBulkSMS_Load(object sender, EventArgs e)
        {
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.DataSource = buildings;
            cmbBuilding.SelectedIndex = -1;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedBuilding = buildings[cmbBuilding.SelectedIndex];
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCustomers()
        {
            List<Customer> customers = Controller.pastel.AddCustomers(selectedBuilding.Name, selectedBuilding.DataPath);
            foreach (Customer c in customers)
            {
                SMSCustomer smsc = new SMSCustomer
                {
                    customerName = c.description,
                    customerAccount = c.accNumber,
                    customerNumber = c.CellPhone
                };
                if (!String.IsNullOrEmpty(smsc.customerNumber))
                {
                    smsCustomers.customers.Add(smsc);
                    bs.Add(smsc);
                }
            }
            dgCustomers.DataSource = bs;
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvr in dgCustomers.Rows) { dvr.Cells[0].Value = chkAll.Checked; }
            foreach (SMSCustomer smsc in smsCustomers.customers) { smsc.include = chkAll.Checked; }
        }

        private void chkBillBuilding_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBillBuilding.Checked)
            {
                chkBillCustomer.Checked = false;
                chkMarketing.Checked = false;
                validSMS = true;
            }
        }

        private void chkBillCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBillCustomer.Checked)
            {
                validSMS = true;
                chkBillBuilding.Checked = false;
                chkMarketing.Checked = false;
            }
        }

        private void chkMarketing_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMarketing.Checked)
            {
                if (Prompt.ShowDialog("Password", "SMS") == "sheldonpwd")
                {
                    validSMS = true;
                    chkBillCustomer.Checked = false;
                    chkBillBuilding.Checked = false;
                }
                else
                {
                    validSMS = false;
                    chkBillCustomer.Checked = false;
                    chkBillBuilding.Checked = false;
                    chkMarketing.Checked = false;
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (validSMS)
            {
                String sendTo = String.Empty;
                String sendName = String.Empty;
                String status;
                bool success = false;
                int entries = 0;
                List<SMSMessage> messages = new List<SMSMessage>();

                if (!String.IsNullOrEmpty(txtMessage.Text) && txtMessage.Text.Length <= 160)
                {
                    var builder = new System.Text.StringBuilder();
                    builder.Append(sendTo);
                    var builder1 = new System.Text.StringBuilder();
                    builder1.Append(sendName);
                    foreach (SMSCustomer smsc in smsCustomers.customers)
                    {
                        if (smsc.include && !chkBillCustomer.Checked)
                        {
                            builder.Append(smsc.customerNumber + ";");
                            builder1.Append(smsc.customerName + ";");
                            entries++;
                        }
                        else if (smsc.include)
                        {
                            SMSMessage m = AddSMS(smsc.customerAccount, smsc.customerNumber);
                            messages.Add(m);
                        }
                    }
                    sendName = builder1.ToString();
                    sendTo = builder.ToString();
                }
                else
                {
                    MessageBox.Show("Please enter a valid message", "SMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtMessage.Focus();
                    return;
                }
                if (!String.IsNullOrEmpty(sendName) && !String.IsNullOrEmpty(sendTo))
                {
                    SMSMessage m = AddSMS(sendName, sendTo);
                    String msg = "";

                    if (!String.IsNullOrEmpty(m.message))
                    {
                        success = sms.SendBulkMessage(m, chkImmediate.Checked, out status);
                        if (success)
                        {
                            msg = "Message sent";
                            if (m.bulkbillable) { PostPastel(m.message, entries, out status); }
                        }
                        else { msg = status; }
                    }
                    else
                    {
                        msg = "Cannot send SMS";
                    }
                    MessageBox.Show(msg, "SMS", MessageBoxButtons.OK, (success ? MessageBoxIcon.Information : MessageBoxIcon.Error));
                }
                else
                {
                    int mCount = 0;
                    foreach (SMSMessage m in messages)
                    {
                        if (sms.SendMessage(m, chkImmediate.Checked, out status)) { mCount++; }
                    }
                    MessageBox.Show(mCount.ToString() + " SMS Sent", "SMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (Prompt.ShowDialog("Password", "SMS") == "sheldonpwd")
                {
                    validSMS = true;
                    btnSend_Click(sender, e);
                }
            }
        }

        private void PostPastel(String reference, int entries, out String pastelString)
        {
            double ramt = entries * 2;
            String amt = ramt.ToString("#0.00");
            int buildPeriod;
            int trustPeriod = Methods.getPeriod(DateTime.Now, selectedBuilding.Period, out buildPeriod);
            String pastelReturn1 = Controller.pastel.PostBatch(DateTime.Now, buildPeriod, "CENTRE18", selectedBuilding.DataPath, 5, selectedBuilding.Journal, selectedBuilding.Centrec_Account, "9250000", "1115000", "1115000", "SMS", reference, amt, selectedBuilding.Centrec_Building, "", out pastelString);
        }

        private SMSMessage AddSMS(String customer, String number)
        {
            SMSMessage m = new SMSMessage();
            try
            {
                m.id = 0;
                m.building = selectedBuilding.Abbr;
                m.customer = customer;
                m.number = number;
                m.reference = "";
                m.message = txtMessage.Text;
                m.sent = DateTime.Now;
                m.sender = Controller.user.id.ToString();
                m.billable = chkBillCustomer.Checked;
                m.bulkbillable = chkBillBuilding.Checked;
                m.astStatus = "1";
                m.batchID = "";
                m.status = "-1";
                m.smsType = "SMS: " + txtMessage.Text;
                m.nextPolled = DateTime.Now.AddMinutes(5);
                m.pollCount = 0;
            }
            catch
            {
                m.message = string.Empty;
            }
            return m;
        }

        private void dgCustomers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            messageCount = 0;
            foreach (DataGridViewRow dvr in dgCustomers.Rows)
            {
                if ((bool)dvr.Cells[0].Value) { messageCount++; }
            }
        }
    }
}
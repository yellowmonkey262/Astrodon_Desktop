using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Astrodon
{
    public partial class usrBulkEmail : UserControl
    {
        private List<Building> buildings;
        private List<EmailList> emailList;

        private Building building;

        public usrBulkEmail()
        {
            InitializeComponent();
            buildings = new Buildings(false).buildings;
        }

        private void usrBulkEmail_Load(object sender, EventArgs e)
        {
            LoadBuildings();
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

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                building = buildings[cmbBuilding.SelectedIndex];
                txtBCC.Text = building.PM;
                if (Controller.user.email != building.PM) { txtBCC.Text += "; " + Controller.user.email; }
                String mailBody = "For any queries on your account, please contact " + building.Debtor + Environment.NewLine + Environment.NewLine;
                mailBody += "For any maintenance queries, please contact " + building.PM + Environment.NewLine + Environment.NewLine;
                mailBody += "Regards" + Environment.NewLine;
                mailBody += "Astrodon (Pty) Ltd" + Environment.NewLine;
                mailBody += "You're in good hands";
                txtMessage.Text = Environment.NewLine + Environment.NewLine + mailBody;
                LoadFolders();
                LoadCustomers(String.Empty);
                LoadCategories();
            }
            catch { }
        }

        private void LoadCategories()
        {
            Dictionary<String, String> categoryDictionary = Controller.pastel.GetCategories(building.DataPath);
            cmbCategory.Items.Clear();
            List<Category> categoryList = new List<Category>();
            Category c1 = new Category
            {
                categoryID = String.Empty,
                categoryName = "All customers"
            };
            categoryList.Add(c1);
            foreach (KeyValuePair<String, String> cat in categoryDictionary)
            {
                Category c = new Category
                {
                    categoryID = cat.Key,
                    categoryName = cat.Value
                };
                categoryList.Add(c);
            }
            cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            cmbCategory.ValueMember = "categoryID";
            cmbCategory.DisplayMember = "categoryName";
            cmbCategory.DataSource = null;
            cmbCategory.DataSource = categoryList;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
        }

        private void LoadCustomers(String category)
        {
            List<Customer> customers = Controller.pastel.AddCustomers(building.Abbr, building.DataPath);
            emailList = new List<EmailList>();
            foreach (Customer c in customers)
            {
                if (c.statPrintorEmail != 4 && (c.category == category || String.IsNullOrEmpty(category)))
                {
                    EmailList el = new EmailList();
                    el.AccNumber = c.accNumber;
                    el.Name = c.description;
                    el.EmailAddress = String.Empty;
                    var builder = new System.Text.StringBuilder();
                    builder.Append(el.EmailAddress);
                    foreach (String email in c.Email)
                    {
                        if (!email.Contains("imp.ad-one"))
                        {
                            builder.Append(email.Replace("\"", "").Replace("\'", "").Replace(";", "").Replace(",", "") + ";");
                        }
                    }
                    el.EmailAddress = builder.ToString();

                    el.Include = false;
                    if (!emailList.Contains(el)) { emailList.Add(el); }
                }
            }
            dgCustomers.DataSource = null;
            dgCustomers.DataSource = emailList;
            lstAttachments.Items.Clear();
        }

        private void LoadFolders()
        {
            try
            {
                Classes.Sftp ftpClient = new Classes.Sftp(building.webFolder, false);
                String workingDirectory = ftpClient.WorkingDirectory;
                List<String> folders = ftpClient.RemoteFolders(false);
                folders.Sort();
                cmbFolder.Items.Clear();
                foreach (String folder in folders) { cmbFolder.Items.Add(folder); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Folder error:" + ex.Message);
            }
        }

        private void chkIncludeAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dgCustomers.Rows.Count > 0)
            {
                foreach (DataGridViewRow dvr in dgCustomers.Rows) { dvr.Cells[3].Value = chkIncludeAll.Checked; }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.SelectedIndex = -1;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
            dgCustomers.DataSource = null;
            txtMessage.Text = String.Empty;
            lstAttachments.Items.Clear();
        }

        private void btnAttachment_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
            {
                if (File.Exists(ofd.FileName) && !lstAttachments.Items.Contains(ofd.FileName)) { lstAttachments.Items.Add(ofd.FileName); }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            String status = String.Empty;
            int msgID = 0;
            bool success = CreateMail(true, out status, out msgID);
            MessageBox.Show(!success ? "Error sending mail: " + status : "All mails queued");
            this.Cursor = Cursors.Default;
        }

        private void btnSendNow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            String status = String.Empty;
            int msgID = 0;
            bool success = CreateMail(false, out status, out msgID);
            if (success) { SendMail(msgID); }
            MessageBox.Show(!success ? "Error sending mail:" + status : "All mails sent");
            this.Cursor = Cursors.Default;
        }

        private bool CreateMail(bool queue, out String status, out int msgID)
        {
            SqlDataHandler dh = new SqlDataHandler();
            bool success = false;
            int incCount = 0;
            msgID = 0;
            List<EmailList> sentToList = new List<EmailList>();
            foreach (EmailList el in emailList)
            {
                if (el.Include && !sentToList.Contains(el))
                {
                    incCount++;
                    sentToList.Add(el);
                }
            }
            if (building == null)
            {
                status = "No building selected";
                return false;
            }
            if (incCount == 0)
            {
                status = "No customers selected";
                return false;
            }
            if (lstAttachments.Items.Count == 0)
            {
                if (MessageBox.Show("No attachments included. Continue sending?", "Bulk Mail", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    status = "User cancelled";
                    return false;
                }
            }
            if (String.IsNullOrEmpty(txtMessage.Text))
            {
                status = "No message or attachments";
                return false;
            }
            if (String.IsNullOrEmpty(txtSubject.Text))
            {
                status = "No subject";
                return false;
            }

            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@buildingID", building.ID);
            sqlParms.Add("@incBCC", chkBCC.Checked);
            sqlParms.Add("@bccAddy", txtBCC.Text);
            sqlParms.Add("@subject", txtSubject.Text);
            String message = String.IsNullOrEmpty(txtMessage.Text) ? "Please find attached document(s) for your attention" : txtMessage.Text;

            User pmUser;
            new Users().GetUser(building.PM, out pmUser, out status);
            if (pmUser == null) { pmUser = Controller.user; }
            message += Environment.NewLine + Environment.NewLine;
            message += "Kind Regards" + Environment.NewLine;
            message += pmUser.name + Environment.NewLine;
            message += "Portfolio Manager" + Environment.NewLine;
            message += "Tel: " + pmUser.phone + Environment.NewLine;
            message += "Fax: 011 867 3163" + Environment.NewLine;
            message += "Direct Fax: " + pmUser.fax + Environment.NewLine;
            message += "For and on behalf of Astrodon Pty Ltd";

            sqlParms.Add("@message", message);
            sqlParms.Add("@billBuilding", cmbBill.SelectedItem != null && cmbBill.SelectedItem.ToString() == "Building" ? true : false);
            sqlParms.Add("@billAmount", txtBill.Text);
            sqlParms.Add("@queue", queue);
            sqlParms.Add("@fromAddress", pmUser.email);
            String msgQuery = "INSERT INTO tblMsg(buildingID, fromAddress, incBCC, bccAddy, subject, message, billBuilding, billAmount, queue)";
            msgQuery += " VALUES (@buildingID, @fromAddress, @incBCC, @bccAddy, @subject, @message, @billBuilding, @billAmount, @queue)";
            int rs = dh.SetData(msgQuery, sqlParms, out status);
            if (rs > 0)
            {
                msgQuery = "SELECT max(id) as id FROM tblMsg";
                DataSet dsMsg = dh.GetData(msgQuery, null, out status);
                if (dsMsg != null && dsMsg.Tables.Count > 0 && dsMsg.Tables[0].Rows.Count > 0)
                {
                    msgID = int.Parse(dsMsg.Tables[0].Rows[0]["id"].ToString());
                    sqlParms.Clear();
                    sqlParms.Add("@msgID", msgID);
                    sqlParms.Add("@billCustomer", cmbBill.SelectedItem != null && cmbBill.SelectedItem.ToString() == "Customer" ? true : false);
                    sqlParms.Add("@fromAddress", !string.IsNullOrEmpty(building.PM) ? building.PM : Controller.user.email);
                    sqlParms.Add("@recipient", "");
                    sqlParms.Add("@accNo", "");
                    String msgReceipientQuery = "INSERT INTO tblMsgRecipients(msgID, recipient, accNo, billCustomer) VALUES(@msgID, @recipient, @accNo, @billCustomer)";
                    foreach (EmailList el in sentToList)
                    {
                        String[] toAddys = el.EmailAddress.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        sqlParms["@accNo"] = el.AccNumber;
                        foreach (String toAddy in toAddys)
                        {
                            sqlParms["@recipient"] = toAddy;
                            dh.SetData(msgReceipientQuery, sqlParms, out status);
                            success = true;
                        }
                    }

                    //lstAttachments.Items.Add("Y:\\Users\\Buildings Managed\\Dolphin Cove BC\\Meetings\\2015\\Proxy & Nomination forms 16.04.2015.pdf");
                    if (lstAttachments.Items != null && lstAttachments.Items.Count > 0)
                    {
                        foreach (Object obj in lstAttachments.Items)
                        {
                            String fileName = obj.ToString();
                            string filename = Path.GetFileName(fileName);
                            string ext = Path.GetExtension(filename);
                            string contenttype = String.Empty;

                            //Set the contenttype based on File Extension
                            switch (ext)
                            {
                                case ".doc":
                                case ".docx":
                                    contenttype = "Word";
                                    break;

                                case ".xls":
                                    contenttype = "Excel";
                                    break;

                                case ".jpg":
                                case ".png":
                                case ".gif":
                                    contenttype = "Image";
                                    break;

                                case ".pdf":
                                    contenttype = "PDF";
                                    break;

                                default:
                                    contenttype = ext.Replace(".", "");
                                    break;
                            }
                            if (contenttype != String.Empty)
                            {
                                Byte[] bytes;
                                //if (File.Exists(fileName)) {
                                Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                                BinaryReader br = new BinaryReader(fs);
                                bytes = br.ReadBytes((Int32)fs.Length);
                                //} else {
                                //bytes = new byte[0];
                                //fileName = "Y:\\Users\\Buildings Managed\\Dolphin Cove BC\\Meetings\\2015\\Proxy & Nomination forms 16.04.2015.pdf";
                                //}
                                String fileQuery = "INSERT INTO tblMsgData(msgID, Name, ContentType, Data) VALUES (@msgID, @Name, @ContentType, @Data)";
                                sqlParms.Clear();
                                sqlParms.Add("@msgID", msgID);
                                sqlParms.Add("@Name", filename);
                                sqlParms.Add("@ContentType", contenttype);
                                sqlParms.Add("@Data", bytes);
                                dh.SetData(fileQuery, sqlParms, out status);
                                success = true;
                                if (status != "")
                                {
                                    MessageBox.Show(status, "Attachments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return false;
                                }
                                else
                                {
                                    UploadToWeb(bytes, filename, sentToList);
                                    success = true;
                                }
                            }
                            else
                            {
                                status = "File format not recognised." + " Upload Image/Word/PDF/Excel formats";
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                //status = "Failed to save email";
                return false;
            }

            return success;
        }

        private void UploadToWeb(byte[] data, string filename, List<EmailList> sendToList)
        {
            if (data != null)
            {
                try
                {
                    String tempFolder = System.IO.Path.GetTempPath();
                    String tempFileName = filename;
                    Classes.Sftp ftpClient = new Classes.Sftp("", true);
                    filename = System.IO.Path.Combine(tempFolder, filename);
                    File.WriteAllBytes(filename, data);
                    if (chkIncludeAll.Checked)
                    {
                        ftpClient = new Classes.Sftp((cmbFolder.SelectedItem == null ? building.webFolder : cmbFolder.SelectedItem.ToString()), false);
                        ftpClient.Upload(filename, tempFileName, false);
                    }
                    else
                    {
                        MySqlConnector mySqlConn = new MySqlConnector();
                        foreach (EmailList el in sendToList)
                        {
                            mySqlConn.InsertStatement(tempFileName, "Customer Statements", filename, el.AccNumber, new String[] { el.EmailAddress });
                            ftpClient.Upload(filename, tempFileName, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Upload error: " + ex.Message);
                }
            }
        }

        private void SendMail(int msgID)
        {
            String mailQuery = "SELECT msg.id, msg.fromAddress, b.Code, b.DataPath, msg.incBCC, msg.bccAddy, msg.subject, msg.message, msg.billBuilding, msg.billAmount FROM tblMsg AS msg ";
            mailQuery += " INNER JOIN tblBuildings AS b ON msg.buildingID = b.id WHERE (msg.queue = 'False') AND msg.id = " + msgID.ToString();
            SqlDataHandler dh = new SqlDataHandler();
            String status = String.Empty;
            DataSet ds = dh.GetData(mailQuery, null, out status);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    String bCode = dr["Code"].ToString();
                    String dataPath = dr["DataPath"].ToString();
                    String fromAddress = dr["fromAddress"].ToString();
                    bool incBCC = bool.Parse(dr["incBCC"].ToString());
                    String bccAddy = dr["bccAddy"].ToString();
                    String subject = dr["subject"].ToString();
                    String message = dr["message"].ToString();
                    bool billBuilding = bool.Parse(dr["billBuilding"].ToString());
                    double billAmount = double.Parse(dr["billAmount"].ToString());
                    String attachmentQuery = "SELECT Name, Data FROM tblMsgData WHERE msgID = " + msgID.ToString();
                    DataSet dsAttachment = dh.GetData(attachmentQuery, null, out status);
                    Dictionary<String, byte[]> attachments = new Dictionary<string, byte[]>();
                    if (dsAttachment != null && dsAttachment.Tables.Count > 0 && dsAttachment.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drA in dsAttachment.Tables[0].Rows)
                        {
                            if (!attachments.ContainsKey(drA["Name"].ToString())) { attachments.Add(drA["Name"].ToString(), (byte[])drA["Data"]); }
                        }
                    }
                    String billableCustomersQuery = "SELECT distinct accNo FROM tblMsgRecipients WHERE billCustomer = 'True' and msgID = " + msgID.ToString();
                    String allRecipientsQuery = "SELECT id, accNo, recipient FROM tblMsgRecipients WHERE msgID = " + msgID.ToString();
                    DataSet billableCustomers = dh.GetData(billableCustomersQuery, null, out status);
                    DataSet receivers = dh.GetData(allRecipientsQuery, null, out status);
                    Dictionary<String, bool> emails = new Dictionary<string, bool>();
                    if (receivers != null && receivers.Tables.Count > 0 && receivers.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow rrece in receivers.Tables[0].Rows)
                        {
                            String[] emailAddys = rrece["recipient"].ToString().Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            bool success = Mailer.SendMail(fromAddress, emailAddys, subject, message, false, false, false, out status, attachments);
                            if (!emails.ContainsKey(rrece["accNo"].ToString())) { emails.Add(rrece["accNo"].ToString(), success); }
                        }
                    }
                    message += Environment.NewLine + Environment.NewLine;
                    message += "Send status:" + Environment.NewLine + Environment.NewLine;
                    var builder = new System.Text.StringBuilder();
                    builder.Append(message);
                    foreach (KeyValuePair<String, bool> statuses in emails)
                    {
                        builder.Append(statuses.Key + " = " + statuses.Value.ToString() + Environment.NewLine);
                    }
                    message = builder.ToString();
                    if (incBCC)
                    {
                        String[] bccs = bccAddy.Split(new String[] { ";" }, StringSplitOptions.None);
                        Mailer.SendMail(fromAddress, bccs, subject, message, false, false, false, out status, attachments);
                    }
                }
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem != null) { LoadCustomers(cmbCategory.SelectedValue.ToString()); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstAttachments.SelectedItems.Count > 0)
            {
                foreach (int idx in lstAttachments.SelectedIndices) { lstAttachments.Items.Remove(lstAttachments.Items[idx]); }
            }
            else
            {
                MessageBox.Show("Please select attachments to be deleted");
            }
        }

        private class Category
        {
            public String categoryID { get; set; }

            public String categoryName { get; set; }
        }
    }
}
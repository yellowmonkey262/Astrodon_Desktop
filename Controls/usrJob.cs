using Astro.Library.Entities;
using Astrodon.Classes;
using Itenso.Rtf;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Support;
using NetSpell.SpellChecker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrJob : UserControl
    {
        #region Variables

        private List<Building> allBuildings;
        private List<Building> pmBuildings;
        private Building selectedBuilding = null;
        private List<Customer> customers;
        private BindingList<LetterTemplates> templates = new BindingList<LetterTemplates>();
        private int jobID = 0;
        private Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
        private SqlDataHandler dataHandler = new SqlDataHandler();
        private String status = String.Empty;
        private String buildingFolder, sms, jobstatus;
        private BindingList<jobCustomers> JobCustomers = new BindingList<jobCustomers>();
        private Classes.Sftp ftpClient;
        private List<Attachments> supportDocs = new List<Attachments>();
        private List<Attachments> EmailDocs = new List<Attachments>();
        private String letterContent;
        private int creator, processor;
        private NetSpell.SpellChecker.Spelling spellCheck;
        private NetSpell.SpellChecker.Dictionary.WordDictionary dictionary;
        private bool isEmailBox = false;
        private Dictionary<String, System.Drawing.Image> htmlImages = null;
        private bool printerSet = false;
        private String uploadDirectory = String.Empty;
        private bool justify = false;

        #endregion Variables

        private class jobCustomers
        {
            public bool Include { get; set; }

            public String Account { get; set; }

            public String Description { get; set; }

            public String email1 { get; set; }

            public String email2 { get; set; }

            public String email3 { get; set; }

            public String email4 { get; set; }

            public String cell { get; set; }

            public bool sEmail1 { get; set; }

            public bool sEmail2 { get; set; }

            public bool sEmail3 { get; set; }

            public bool sEmail4 { get; set; }

            public bool sCell { get; set; }
        }

        #region Events

        private void spellCheck_ReplacedWord(object sender, ReplaceWordEventArgs e)
        {
            if (isEmailBox)
            {
                int start = txtBody.SelectionStart;
                int length = txtBody.SelectionLength;

                this.txtBody.Select(e.TextIndex, e.Word.Length);
                this.txtBody.SelectedText = e.ReplacementWord;

                if (start > this.txtBody.Text.Length) { start = this.txtBody.Text.Length; }

                if ((start + length) > this.txtBody.Text.Length) { length = 0; }

                this.txtBody.Select(start, length);
            }
            else
            {
                int start = rtfEditor.SelectionStart;
                int length = rtfEditor.SelectionLength;

                this.rtfEditor.Select(e.TextIndex, e.Word.Length);
                this.rtfEditor.SelectedText = e.ReplacementWord;

                if (start > this.rtfEditor.Text.Length) { start = this.rtfEditor.Text.Length; }

                if ((start + length) > this.rtfEditor.Text.Length) { length = 0; }

                this.rtfEditor.Select(start, length);
            }
        }

        private void spellCheck_DeletedWord(object sender, SpellingEventArgs e)
        {
            if (isEmailBox)
            {
                int start = this.txtBody.SelectionStart;
                int length = this.txtBody.SelectionLength;

                this.txtBody.Select(e.TextIndex, e.Word.Length);
                this.txtBody.SelectedText = "";

                if (start > this.txtBody.Text.Length) { start = this.txtBody.Text.Length; }
                if ((start + length) > this.txtBody.Text.Length) { length = 0; }

                this.txtBody.Select(start, length);
            }
            else
            {
                int start = this.rtfEditor.SelectionStart;
                int length = this.rtfEditor.SelectionLength;

                this.rtfEditor.Select(e.TextIndex, e.Word.Length);
                this.rtfEditor.SelectedText = "";

                if (start > this.rtfEditor.Text.Length) { start = this.rtfEditor.Text.Length; }
                if ((start + length) > this.rtfEditor.Text.Length) { length = 0; }

                this.rtfEditor.Select(start, length);
            }
        }

        private void spellCheck_EndOfText(object sender, EventArgs e)
        {
            MessageBox.Show("End of text");
        }

        #endregion Events

        public usrJob(int JobID)
        {
            jobID = JobID;
            InitializeComponent();
            allBuildings = new Buildings(false).buildings;
            pmBuildings = new List<Building>();
            spellCheck = new Spelling(this.components);
            dictionary = new NetSpell.SpellChecker.Dictionary.WordDictionary(this.components) { DictionaryFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dic") };
            spellCheck.Dictionary = dictionary;
            spellCheck.EndOfText += spellCheck_EndOfText;
            spellCheck.DeletedWord += spellCheck_DeletedWord;
            spellCheck.ReplacedWord += spellCheck_ReplacedWord;
            if (JobID == 0)
            {
                foreach (int bid in Controller.user.buildings)
                {
                    foreach (Building b in allBuildings)
                    {
                        if (bid == b.ID && !pmBuildings.Contains(b))
                        {
                            pmBuildings.Add(b);
                            break;
                        }
                    }
                }
                pmBuildings = pmBuildings.OrderBy(c => c.Name).ToList();
                buildingFolder = String.Empty;
            }
        }

        private void usrJob_Load(object sender, EventArgs e)
        {
            if (Controller.user.usertype != 2) { chkDisablePrint.Enabled = false; }
            if (jobID == 0)
            {
                cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
                cmbBuilding.DataSource = pmBuildings;
                cmbBuilding.DisplayMember = "Name";
                cmbBuilding.ValueMember = "ID";
                cmbBuilding.SelectedIndex = -1;
                cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
                DisableControls(tbSupporting, true);
                DisableControls(tbEmail, true);
                DisableControls(tbLetter, true);
                DisableControls(tbProcess, true);
                lblBuildSelect.ForeColor = Color.Red;
                lblTopic.ForeColor = Color.Red;
                btnSave1.ForeColor = Color.Red;
                lblSavePlease.ForeColor = Color.Red;
                lblSavePlease.Visible = true;
                tbSupporting.Enabled = false;
                tbEmail.Enabled = false;
                tbLetter.Enabled = false;
                tbProcess.Enabled = false;
            }
            else
            {
                lblBuildSelect.ForeColor = Color.Black;
                lblTopic.ForeColor = Color.Black;
                btnSave1.ForeColor = Color.Black;
                lblSavePlease.Visible = false;
                tbSupporting.Enabled = true;
                tbEmail.Enabled = true;
                tbLetter.Enabled = true;
                tbProcess.Enabled = true;
                LoadJob();
            }
        }

        private void DefaultLetter()
        {
            String headerLine = "We write in our capacity as the Managing Agent and on behalf of the " + (selectedBuilding.Name.ToLower().Contains("hoa") ? "Directors" : "Trustees") + " of " + selectedBuilding.letterName;
            String footerLine = "Should you have any queries, please do not hesitate to contact writer hereof.";
            if (rtfEditor.Text.Trim() == "") { rtfEditor.Text = headerLine + Environment.NewLine + Environment.NewLine + footerLine; }
        }

        private String DefaultLetterContent()
        {
            String headerLine = "We write in our capacity as the Managing Agent and on behalf of the " + (selectedBuilding.Name.ToLower().Contains("hoa") ? "Directors" : "Trustees") + " of " + selectedBuilding.letterName;
            String footerLine = "Should you have any queries, please do not hesitate to contact writer hereof.";
            return headerLine + Environment.NewLine + Environment.NewLine + footerLine;
        }

        private void DisableControls(Control c, bool disable)
        {
            if (c.HasChildren)
            {
                foreach (Control cc in c.Controls) { DisableControls(cc, disable); }
            }
            else
            {
                try
                {
                    if (c.GetType() == typeof(TextBox)) { (c as TextBox).ReadOnly = disable; } else { c.Enabled = !disable; }
                }
                catch { }
            }
        }

        private void ShowErrorMessage(String error)
        {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LoadJob()
        {
            String jobQuery = "SELECT * FROM tblPMJob WHERE id = @jid";
            sqlParms.Clear();
            sqlParms.Add("@jid", jobID);
            DataSet dsJob = dataHandler.GetData(jobQuery, sqlParms, out status);
            if (dsJob != null && dsJob.Tables.Count > 0 && dsJob.Tables[0].Rows.Count > 0)
            {
                DataRow drJob = dsJob.Tables[0].Rows[0];
                foreach (Building b in allBuildings)
                {
                    if (b.Abbr == drJob["buildingCode"].ToString())
                    {
                        selectedBuilding = b;
                        break;
                    }
                }
                if (selectedBuilding != null)
                {
                    cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
                    cmbBuilding.DataSource = null;
                    cmbBuilding.Items.Add(selectedBuilding.Name);
                    cmbBuilding.SelectedIndex = 0;
                    cmbBuilding.Enabled = false;

                    LoadCustomers();
                    LoadDocuments();
                    LoadTemplates();
                    creator = int.Parse(drJob["creator"].ToString());
                    processor = (int.TryParse(drJob["processedBy"].ToString(), out processor) ? processor : 0);
                    buildingFolder = drJob["buildingFolder"].ToString();

                    chkBuilding.Checked = bool.Parse(drJob["buildingUpload"].ToString());
                    chkInbox.Checked = bool.Parse(drJob["inboxUpload"].ToString());

                    txtTopic.Text = drJob["topic"].ToString();
                    txtInstructions.Text = drJob["instructions"].ToString();
                    txtNotes.Text = drJob["notes"].ToString();
                    txtCC.Text = drJob["cc"].ToString();
                    txtBCC.Text = drJob["bcc"].ToString();
                    if (!txtBCC.Text.Contains(Controller.user.email)) { txtBCC.Text += "; " + Controller.user.email; }
                    txtSubject.Text = drJob["subject"].ToString();
                    txtLetter.Text = drJob["subject"].ToString();
                    if (String.IsNullOrEmpty(txtSubject.Text))
                    {
                        DisableControls(tbLetter, true);
                        txtLetter.Enabled = true;
                        txtLetter.ReadOnly = false;
                        cmbLetter.Enabled = true;
                    }
                    txtBody.Text = drJob["body"].ToString();
                    txtSMS.Text = drJob["sms"].ToString();
                    chkBill.Checked = bool.Parse(drJob["billcustomer"].ToString());
                    txtStatus.Text = drJob["status"].ToString();
                    jobstatus = txtStatus.Text;
                    if (Controller.user.usertype == 2)
                    {
                        if (jobstatus == "REVIEW")
                        {
                            btnSubmit.Enabled = false;
                            btnApprove.Enabled = true;
                            btnProcess.Enabled = false;
                            btnRework.Enabled = true;
                        }
                        else if (jobstatus == "NEW")
                        {
                            btnSubmit.Enabled = true;
                            btnApprove.Enabled = false;
                            btnProcess.Enabled = true;
                            btnRework.Enabled = false;
                        }
                        else
                        {
                            btnSubmit.Enabled = false;
                            btnApprove.Enabled = false;
                            btnProcess.Enabled = false;
                            btnRework.Enabled = false;
                        }
                    }
                    else if (Controller.user.usertype == 4)
                    {
                        btnSubmit.Enabled = true;
                        btnApprove.Enabled = false;
                        btnProcess.Enabled = false;
                        btnRework.Enabled = false;
                    }
                }
                else
                {
                    ShowErrorMessage("No such building on file...");
                }
            }
            else
            {
                ShowErrorMessage(status);
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedItem != null)
            {
                String bCode = cmbBuilding.SelectedValue.ToString();
                foreach (Building b in allBuildings)
                {
                    if (b.ID == int.Parse(bCode))
                    {
                        selectedBuilding = b;
                        break;
                    }
                }
            }
            else
            {
                selectedBuilding = null;
            }
            if (selectedBuilding != null)
            {
                LoadCustomers();
                LoadTemplates();
                txtBCC.Text = selectedBuilding.PM + ";";
            }
        }

        private void LoadCustomers()
        {
            if (customers == null) { customers = new List<Customer>(); }
            customers.Clear();
            customers = Controller.pastel.AddCustomers(String.Empty, selectedBuilding.DataPath);
            JobCustomers = new BindingList<jobCustomers>();
            sqlParms.Clear();
            String customerQuery = "SELECT * FROM tblPMCustomers WHERE jobID = " + jobID.ToString() + " AND account = @account";
            int totalCustomers = customers.Count;
            int selectedCustomers = 0;
            foreach (Customer c in customers)
            {
                jobCustomers jc = new jobCustomers
                {
                    Account = c.accNumber,
                    Description = c.description,
                    Include = false
                };
                if (c.Email.Length > 0)
                {
                    jc.email1 = c.Email[0];
                    if (c.Email.Length > 1)
                    {
                        jc.email2 = c.Email[1];
                        if (c.Email.Length > 2)
                        {
                            jc.email3 = c.Email[2];
                            if (c.Email.Length > 3)
                            {
                                jc.email4 = c.Email[3];
                            }
                        }
                    }
                }
                jc.cell = c.CellPhone;
                sqlParms.Clear();
                sqlParms.Add("@account", jc.Account);
                DataSet jobCustomer = dataHandler.GetData(customerQuery, sqlParms, out status);
                if (jobCustomer != null && jobCustomer.Tables.Count > 0 && jobCustomer.Tables[0].Rows.Count > 0)
                {
                    DataRow drJobCustomer = jobCustomer.Tables[0].Rows[0];
                    if (jc.Account == drJobCustomer["account"].ToString())
                    {
                        selectedCustomers++;
                        jc.Include = true;
                        jc.sEmail1 = bool.Parse(drJobCustomer["sendMail1"].ToString());
                        jc.sEmail2 = bool.Parse(drJobCustomer["sendMail2"].ToString());
                        jc.sEmail3 = bool.Parse(drJobCustomer["sendMail3"].ToString());
                        jc.sEmail4 = bool.Parse(drJobCustomer["sendMail4"].ToString());
                        jc.sCell = bool.Parse(drJobCustomer["sendSMS"].ToString());
                    }
                }
                JobCustomers.Add(jc);
            }
            dgCustomers.DataSource = JobCustomers;
            if (totalCustomers == selectedCustomers)
            {
                rdAllCustomers.Checked = true;
            }
            else
            {
                rdCustomers.Checked = true;
            }
        }

        private void LoadTemplates()
        {
            cmbLetter.SelectedIndexChanged -= cmbLetter_SelectedIndexChanged;
            cmbLetter.DataSource = null;
            String templateQuery = "SELECT id, templateName, templateContent, buildingID FROM tblTemplates WHERE buildingID = " + selectedBuilding.ID.ToString();
            DataSet dsTemplates = dataHandler.GetData(templateQuery, null, out status);
            templates = new BindingList<LetterTemplates>();
            LetterTemplates newLetter = new LetterTemplates
            {
                id = "0",
                title = "New Letter",
                content = "",
                buildingID = 0
            };
            templates.Add(newLetter);
            if (dsTemplates != null && dsTemplates.Tables.Count > 0 && dsTemplates.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drTemplate in dsTemplates.Tables[0].Rows)
                {
                    LetterTemplates template = new LetterTemplates
                    {
                        id = drTemplate["id"].ToString(),
                        title = drTemplate["templateName"].ToString(),
                        content = drTemplate["templateContent"].ToString(),
                        buildingID = Convert.ToInt16(drTemplate["buildingID"])
                    };
                    templates.Add(template);
                }
            }
            cmbLetter.DataSource = templates;
            cmbLetter.ValueMember = "id";
            cmbLetter.DisplayMember = "title";
            cmbLetter.SelectedIndex = -1;
            cmbLetter.SelectedIndexChanged += cmbLetter_SelectedIndexChanged;
        }

        private class LetterTemplates
        {
            public String id { get; set; }

            public String title { get; set; }

            public String content { get; set; }

            public int buildingID { get; set; }
        }

        private void LoadDocuments()
        {
            String docQuery = "SELECT * FROM tblAttachments WHERE jobID = " + jobID.ToString();
            DataSet dsDocs = dataHandler.GetData(docQuery, null, out status);
            if (dsDocs != null && dsDocs.Tables.Count > 0 && dsDocs.Tables[0].Rows.Count > 0)
            {
                supportDocs = new List<Attachments>();
                EmailDocs = new List<Attachments>();
                letterContent = String.Empty;
                foreach (DataRow drDoc in dsDocs.Tables[0].Rows)
                {
                    int attachmentType = int.Parse(drDoc["attachmentType"].ToString());
                    if (attachmentType == 1)
                    {//support
                        try
                        {
                            Attachments a = new Attachments { FileName = drDoc["fileName"].ToString() };
                            supportDocs.Add(a);
                        }
                        catch { }
                    }
                    else if (attachmentType == 2)
                    { //email
                        try
                        {
                            Attachments a = new Attachments { FileName = drDoc["fileName"].ToString() };
                            EmailDocs.Add(a);
                        }
                        catch { }
                    }
                    else if (attachmentType == 3)
                    { //document
                        try
                        {
                            letterContent = drDoc["fileRTF"].ToString();
                            justify = bool.Parse(drDoc["justify"].ToString());
                            chkUseLetterhead.Checked = bool.Parse(drDoc["letterhead"].ToString());
                            chkCustomer.Checked = bool.Parse(drDoc["customer"].ToString());
                            btnJustify.BackColor = (!justify ? Color.Red : Color.Green);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                dgSupportDocs.DataSource = supportDocs;
                dgAttachments.DataSource = EmailDocs;
            }
            if (!String.IsNullOrEmpty(letterContent))
            {
                rtfEditor.Rtf = letterContent;
                rtfEditor.Invalidate();
            }
            else
            {
                DefaultLetter();
            }
        }

        private void chkBuilding_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBuilding.Checked)
            {
                this.Cursor = Cursors.WaitCursor;
                chkBuilding.Enabled = false;
                LoadFolders();
                if (!String.IsNullOrEmpty(buildingFolder)) { cmbFolder.SelectedItem = buildingFolder; }
                this.Cursor = Cursors.Arrow;
                chkBuilding.Enabled = true;
            }
        }

        private void LoadFolders()
        {
            try
            {
                ftpClient = new Classes.Sftp(selectedBuilding.webFolder, false);
                List<String> folders = ftpClient.RemoteFolders(false);
                folders.Sort();
                folders.Insert(0, "Root");
                cmbFolder.Items.Clear();
                foreach (String folder in folders) { cmbFolder.Items.Add(folder); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rdAllCustomers_CheckedChanged(object sender, EventArgs e)
        {
            if (rdAllCustomers.Checked)
            {
                foreach (jobCustomers jc in JobCustomers) { jc.Include = true; }
                rdCustomers.Checked = false;
                rdTrustees.Checked = false;
            }
            chkBuilding.Checked = rdAllCustomers.Checked;
            dgCustomers.DataSource = JobCustomers;
            dgCustomers.Refresh();
        }

        private void rdCustomers_CheckedChanged(object sender, EventArgs e)
        {
            if (rdCustomers.Checked)
            {
                rdAllCustomers.Checked = false;
                rdTrustees.Checked = false;
            }
            chkInbox.Checked = rdCustomers.Checked;
            dgCustomers.DataSource = JobCustomers;
            dgCustomers.Refresh();
        }

        private void rdTrustees_CheckedChanged(object sender, EventArgs e)
        {
            if (rdTrustees.Checked)
            {
                foreach (jobCustomers jc in JobCustomers)
                {
                    Customer customer = customers.SingleOrDefault(c => c.accNumber == jc.Account);
                    if (customer != null)
                    {
                        if (Controller.user.id == 1) { MessageBox.Show(customer.category); }
                        int iCat = Convert.ToInt32(customer.category);
                        jc.Include = iCat == 7;
                    }
                    else
                    {
                        jc.Include = false;
                    }
                }
                rdAllCustomers.Checked = false;
                rdCustomers.Checked = false;
            }
            chkInbox.Checked = rdTrustees.Checked;
            dgCustomers.DataSource = JobCustomers;
            dgCustomers.Refresh();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            ///validation
            if (selectedBuilding != null)
            {
                String buildingCode = selectedBuilding.Abbr;
                String buildingFolder = "";
                if (chkBuilding.Checked && cmbFolder.SelectedItem != null && !String.IsNullOrEmpty(cmbFolder.SelectedItem.ToString()))
                {
                    buildingFolder = cmbFolder.SelectedItem.ToString();
                }
                else if (chkBuilding.Checked)
                {
                    MessageBox.Show("Please select a folder", "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cmbFolder.Focus();
                    return;
                }
                int selectedCustomers = 0;
                foreach (jobCustomers jc in JobCustomers) { if (jc.Include) { selectedCustomers++; } }
                if (selectedCustomers == 0)
                {
                    MessageBox.Show("Please select customers", "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (String.IsNullOrEmpty(txtTopic.Text))
                {
                    MessageBox.Show("Please enter topic / subject", "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtTopic.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtInstructions.Text))
                {
                    MessageBox.Show("Please enter instructions", "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtInstructions.Focus();
                    return;
                }
                String notes = String.IsNullOrEmpty(txtNotes.Text) ? "" : txtNotes.Text;
                String insertQuery = "INSERT INTO tblPMJob(creator, buildingCode, buildingUpload, inboxUpload, buildingFolder, topic, instructions,notes)";
                insertQuery += " VALUES(@creator, @buildingCode, @buildingUpload, @inboxUpload, @buildingFolder, @topic, @instructions, @notes)";
                String updateQuery = "UPDATE tblPMJob SET buildingUpload = @buildingUpload, inboxUpload = @inboxUpload, buildingFolder = @buildingFolder, topic = @topic, ";
                updateQuery += " instructions = @instructions, notes = @notes WHERE id = @jobID";
                sqlParms.Clear();
                sqlParms.Add("@creator", Controller.user.id);
                sqlParms.Add("@buildingCode", buildingCode);
                sqlParms.Add("@buildingUpload", chkBuilding.Checked);
                sqlParms.Add("@inboxUpload", chkInbox.Checked);
                sqlParms.Add("@buildingFolder", buildingFolder);
                sqlParms.Add("@topic", txtTopic.Text);
                sqlParms.Add("@instructions", txtInstructions.Text);
                sqlParms.Add("@notes", txtNotes.Text);
                sqlParms.Add("@jobID", jobID);
                String query = jobID == 0 ? insertQuery : updateQuery;
                int success = dataHandler.SetData(query, sqlParms, out status);
                if (success > 0 && jobID == 0)
                {
                    String newQuery = "SELECT max(id) as jobID FROM tblPMJob";
                    DataSet dsNewJob = dataHandler.GetData(newQuery, null, out status);
                    jobID = int.Parse(dsNewJob.Tables[0].Rows[0]["jobID"].ToString());
                    DisableControls(tbSupporting, false);
                    DisableControls(tbEmail, false);
                    txtLetter.Enabled = true;
                    txtLetter.ReadOnly = false;
                    cmbLetter.Enabled = true;
                    DisableControls(tbProcess, false);
                    btnSubmit.Enabled = true;
                    btnApprove.Enabled = false;
                    btnProcess.Enabled = true;
                    btnRework.Enabled = false;
                    ProcessMessage("added new job");
                }
                else if (success > 0)
                {
                    ProcessMessage("updated job");
                    if (Controller.user.usertype == 2)
                    {
                        if (jobstatus == "REVIEW")
                        {
                            btnSubmit.Enabled = false;
                            btnApprove.Enabled = true;
                            btnProcess.Enabled = false;
                            btnRework.Enabled = true;
                        }
                        else if (jobstatus == "NEW")
                        {
                            btnSubmit.Enabled = true;
                            btnApprove.Enabled = false;
                            btnProcess.Enabled = true;
                            btnRework.Enabled = false;
                        }
                        else
                        {
                            btnSubmit.Enabled = false;
                            btnApprove.Enabled = false;
                            btnProcess.Enabled = false;
                            btnRework.Enabled = false;
                        }
                    }
                    else if (Controller.user.usertype == 4)
                    {
                        btnSubmit.Enabled = true;
                        btnApprove.Enabled = false;
                        btnProcess.Enabled = false;
                        btnRework.Enabled = false;
                    }
                }
                if (success > 0)
                {
                    SaveCustomers();
                    MessageBox.Show("Job ID " + jobID.ToString() + " saved!", "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    usrJob_Load(null, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Please select a building", "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbBuilding.Focus();
            }
        }

        private void SaveCustomers()
        {
            String clearQuery = "DELETE FROM tblPMCustomers WHERE jobID = " + jobID.ToString();
            dataHandler.SetData(clearQuery, null, out status);
            String addQuery = "INSERT INTO tblPMCustomers(jobID, account, sendMail1, sendMail2, sendMail3, sendMail4, sendSMS)";
            addQuery += " VALUES(" + jobID.ToString() + ", @account, @sendMail1, @sendMail2, @sendMail3, @sendMail4, @sendSMS)";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            foreach (jobCustomers jc in JobCustomers)
            {
                if (jc.Include)
                {
                    sqlParms.Clear();
                    sqlParms.Add("@account", jc.Account);
                    sqlParms.Add("@sendMail1", jc.sEmail1);
                    sqlParms.Add("@sendMail2", jc.sEmail2);
                    sqlParms.Add("@sendMail3", jc.sEmail3);
                    sqlParms.Add("@sendMail4", jc.sEmail4);
                    sqlParms.Add("@sendSMS", jc.sCell);
                    dataHandler.SetData(addQuery, sqlParms, out status);
                }
            }
        }

        private void ResetForm(TabPage tb)
        {
            foreach (Control control in tb.Controls)
            {
                if (control is TextBox) { ((TextBox)control).Text = null; }
                if (control is ComboBox)
                {
                    ComboBox comboBox = (ComboBox)control;
                    if (comboBox.Items.Count > 0) { comboBox.SelectedIndex = 0; }
                }
                if (control is CheckBox) { ((CheckBox)control).Checked = false; }
                if (control is RadioButton) { ((RadioButton)control).Checked = false; }
                if (control is RichTextBox) { ((RichTextBox)control).Clear(); }
                if (tb == tbInstructions)
                {
                    foreach (jobCustomers jc in JobCustomers) { jc.Include = false; }
                }
            }
        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            if (jobID == 0) { ResetForm(tbInstructions); } else { LoadJob(); }
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            String letterQuery = "IF EXISTS(SELECT id FROM tblAttachments WHERE jobID = @jobID AND attachmentType = 3)";
            letterQuery += " UPDATE tblAttachments SET fileRTF = @fileRTF, justify = @justify, letterhead = @letterhead, customer = @customer WHERE jobID = @jobID AND attachmentType = 3";
            letterQuery += " ELSE ";
            letterQuery += " INSERT INTO tblAttachments(jobID, fileName, fileRTF, attachmentType, justify, letterhead, customer) VALUES(@jobID, @fileName, @fileRTF, 3, @justify, @letterhead, @customer)";
            sqlParms.Clear();
            sqlParms.Add("@jobID", jobID);
            int selectedCustomers = 0;
            String selectedCustomer = "";
            foreach (jobCustomers jc in JobCustomers)
            {
                if (jc.Include)
                {
                    selectedCustomers++;
                    selectedCustomer = jc.Account;
                }
            }
            String fileName = (selectedCustomers > 1 ? selectedBuilding.Abbr : selectedCustomer) + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + txtSubject.Text;
            if (fileName.Length > 32) { fileName = fileName.Substring(0, 32); }
            sqlParms.Add("@fileName", fileName);
            //sqlParms.Add("@jobID", jobID);
            sqlParms.Add("@fileRTF", rtfEditor.Rtf);
            sqlParms.Add("@justify", justify);
            sqlParms.Add("@letterhead", chkUseLetterhead.Checked);
            sqlParms.Add("@customer", chkCustomer.Checked);
            dataHandler.SetData(letterQuery, sqlParms, out status);
            if (status != "") { MessageBox.Show(status); }
            SaveLetter();
            ProcessMessage("updated job");
        }

        private void SaveLetter()
        {
            String emailQuery = "UPDATE tblPMJob SET subject = @subject WHERE id = @jobID";
            sqlParms.Clear();
            sqlParms.Add("@subject", txtSubject.Text);
            sqlParms.Add("@jobID", jobID);
            dataHandler.SetData(emailQuery, sqlParms, out status);
        }

        private void btnCancel3_Click(object sender, EventArgs e)
        {
            ResetForm(tbLetter);
        }

        private void btnAddSupport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                {
                    String fileName = ofd.FileName;
                    String uploadFileName = Path.GetFileName(fileName);
                    byte[] file;
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) { using (var reader = new BinaryReader(stream)) { file = reader.ReadBytes((int)stream.Length); } }
                    if (MessageBox.Show("Confirm addition of " + uploadFileName + "?", "File Upload", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Attachments a = new Attachments { FileName = uploadFileName };
                        supportDocs.Add(a);
                        String uploadQuery = "INSERT INTO tblAttachments(jobID, fileName, fileContent, attachmentType) VALUES(@jobID, @fileName, @fileContent, 1)";
                        sqlParms.Clear();
                        sqlParms.Add("@jobID", jobID);
                        sqlParms.Add("@fileName", uploadFileName);
                        sqlParms.Add("@fileContent", file);
                        dataHandler.SetData(uploadQuery, sqlParms, out status);
                        if (status != "") { MessageBox.Show(status); }
                        LoadDocuments();
                    }
                }
            }
        }

        private void btnEmailAttach_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                {
                    String fileName = ofd.FileName;
                    String uploadFileName = Path.GetFileName(fileName);
                    byte[] file;
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) { using (var reader = new BinaryReader(stream)) { file = reader.ReadBytes((int)stream.Length); } }
                    if (MessageBox.Show("Confirm addition of " + uploadFileName + "?", "File Upload", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Attachments a = new Attachments { FileName = uploadFileName };
                        supportDocs.Add(a);
                        String uploadQuery = "INSERT INTO tblAttachments(jobID, fileName, fileContent, attachmentType) VALUES(@jobID, @fileName, @fileContent, 2)";
                        sqlParms.Clear();
                        sqlParms.Add("@jobID", jobID);
                        sqlParms.Add("@fileName", uploadFileName);
                        sqlParms.Add("@fileContent", file);
                        dataHandler.SetData(uploadQuery, sqlParms, out status);
                        if (status != "") { MessageBox.Show(status); }
                        LoadDocuments();
                    }
                }
            }
        }

        private String ConvertRTFToHtml()
        {
            try
            {
                String convertRTF = rtfEditor.Rtf.Replace("\\tab", "        ");
                IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc(convertRTF);
                RtfHtmlConverter htmlConvertor = new RtfHtmlConverter(rtfDocument);
                String html = htmlConvertor.Convert();
                Itenso.Rtf.Converter.Image.RtfConvertedImageInfoCollection images = htmlConvertor.DocumentImages;
                htmlImages = new Dictionary<string, Image>();
                foreach (Itenso.Rtf.Converter.Image.RtfConvertedImageInfo htmlImage in images)
                {
                    try
                    {
                        String savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, htmlImage.FileName);
                        htmlImage.MyImage.Save(savePath);
                        Image i = Image.FromFile(htmlImage.FileName);
                        String newName = Path.GetFileNameWithoutExtension(htmlImage.FileName) + ".jpg";
                        html = html.Replace(htmlImage.FileName, newName);
                        savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, newName);
                        i.Save(savePath, ImageFormat.Jpeg);
                        htmlImages.Add(newName, htmlImage.MyImage);
                    }
                    catch (Exception ex1)
                    {
                        //MessageBox.Show(ex1.Message);
                    }
                }
                return html;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return String.Empty;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ProcessDocuments(false);
        }

        private bool HasLetter()
        {
            String letterQuery = "SELECT id FROM tblAttachments WHERE jobID = @jobID AND attachmentType = 3";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@jobID", jobID);
            DataSet dsHasLetter = dataHandler.GetData(letterQuery, sqlParms, out status);
            return (dsHasLetter != null && dsHasLetter.Tables.Count > 0 && dsHasLetter.Tables[0].Rows.Count > 0);
        }

        private bool ValidateProcess()
        {
            bool atleastone = false;
            try
            {
                for (int i = 0; i < dgCustomers.Rows.Count; i++)
                {
                    DataGridViewRow dvr = dgCustomers.Rows[i];
                    bool included = (bool)dvr.Cells[0].Value;
                    jobCustomers jAcc = null;
                    if (included)
                    {
                        jAcc = JobCustomers[i];
                        foreach (Customer c in customers)
                        {
                            if (c.accNumber == jAcc.Account)
                            {
                                try { c.Email[0] = jAcc.sEmail1 ? jAcc.email1 : String.Empty; } catch { }
                                try { c.Email[1] = jAcc.sEmail2 ? jAcc.email2 : String.Empty; } catch { }
                                try { c.Email[2] = jAcc.sEmail3 ? jAcc.email3 : String.Empty; } catch { }
                                try { c.Email[3] = jAcc.sEmail4 ? jAcc.email4 : String.Empty; } catch { }
                                try { c.CellPhone = jAcc.sCell ? jAcc.cell : String.Empty; } catch { }
                                if (!String.IsNullOrEmpty(c.Email[0]) || !String.IsNullOrEmpty(c.Email[1]) || !String.IsNullOrEmpty(c.Email[2]) || !String.IsNullOrEmpty(c.Email[3]))
                                {
                                    atleastone = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            if (!atleastone)
            {
                MessageBox.Show("No customers / email addresses selected");
            }
            return atleastone;
        }

        private void ProcessDocuments(bool sendNow)
        {
            this.Cursor = Cursors.WaitCursor;
            if (sendNow) { txtStatus.Text += Environment.NewLine + "Starting processing: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine; }
            String ftpUploadFolder = "";
            if (chkBuilding.Checked && cmbFolder.SelectedItem != null)
            {
                if (buildingFolder.ToLower() != "root" && ftpUploadFolder != ftpClient.WorkingDirectory)
                {
                    ftpUploadFolder = ftpClient.WorkingDirectory + "/" + buildingFolder;
                }
            }

            String html = ConvertRTFToHtml();
            //MessageBox.Show(html);
            PDF pdf = new PDF();
            int selectedCustomers = 0;
            List<Customer> includedCustomers = new List<Customer>();

            int rowIncludes = 0;
            for (int i = 0; i < dgCustomers.Rows.Count; i++)
            {
                DataGridViewRow dvr = dgCustomers.Rows[i];
                bool included = (bool)dvr.Cells[0].Value;
                jobCustomers jAcc = null;
                if (included)
                {
                    jAcc = JobCustomers[i];
                    foreach (Customer c in customers)
                    {
                        if (c.accNumber == jAcc.Account)
                        {
                            try { c.Email[0] = jAcc.sEmail1 ? jAcc.email1 : String.Empty; } catch { }
                            try { c.Email[1] = jAcc.sEmail2 ? jAcc.email2 : String.Empty; } catch { }
                            try { c.Email[2] = jAcc.sEmail3 ? jAcc.email3 : String.Empty; } catch { }
                            try { c.Email[3] = jAcc.sEmail4 ? jAcc.email4 : String.Empty; } catch { }
                            try { c.CellPhone = jAcc.sCell ? jAcc.cell : String.Empty; } catch { }
                            if (!includedCustomers.Contains(c)) { includedCustomers.Add(c); }
                            selectedCustomers++;
                            break;
                        }
                    }
                }
            }
            bool cansend = false;

            if (includedCustomers.Count == 0)
            {
                Customer c = new Customer
                {
                    accNumber = "<Account Number>",
                    description = "<Description>",
                    address = new string[] { "<Address 1>", "<Address 2>", "<Address 3>", "<Address 4>", "<Address 5>" },
                    Email = new string[] { "<Email Address>" }
                };
                includedCustomers.Clear();
                includedCustomers.Add(c);
            }
            else
            {
                int i = 0;
                foreach (Customer sendCustomer in includedCustomers)
                {
                    int j = sendCustomer.Email.ToList().Count(c => !String.IsNullOrEmpty(c));
                    i += j;
                }
                cansend = i > 0;
                //////////
                //MessageBox.Show(cansend.ToString());
                //cansend = false;
                //////////
                if (!cansend)
                {
                    MessageBox.Show("No customers / email addresses selected");
                    return;
                }
            }

            String defaultLocation = "K:\\Debtors System";
            if (!Directory.Exists(defaultLocation)) { defaultLocation = "C:\\Pastel11"; }
            String attachmentLocation = defaultLocation + "\\PA Attachments";
            if (!Directory.Exists(attachmentLocation)) { Directory.CreateDirectory(attachmentLocation); }
            SMS smsSender = new SMS();
            if (!sendNow)
            {
                if (EmailDocs.Count > 0 && MessageBox.Show("This job has attachments. Display attachments now?", "View Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (Attachments a in EmailDocs)
                    {
                        int fileID;
                        byte[] aByte = GetAttachment(a.FileName, 2, out fileID);
                        OpenInAnotherApp(aByte, a.FileName);
                    }
                }
            }
            int cCount = 1;
            byte[] fileStream = null;
            String fileName = String.Empty;
            if (!chkCustomer.Checked && rtfEditor.Enabled == true)
            {
                pdf.CreatePALetter(null, selectedBuilding, GetPM(), txtSubject.Text, DateTime.Now, html, null, justify, chkUseLetterhead.Checked, out fileStream); //.Replace("/", "\\")
                String tempFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + selectedBuilding.Abbr + "_" + txtSubject.Text + "_" + cCount.ToString();
                if (tempFileName.Length > 32) { tempFileName = tempFileName.Substring(0, 32); }
                fileName = tempFileName + ".pdf";
                fileName = fileName.Replace("\\", "_").Replace("/", "_");
                string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                fileName = r.Replace(fileName, "");
            }
            bool onetimebuildingattachments = false;
            foreach (Customer sendCustomer in includedCustomers)
            {
                if (rtfEditor.Enabled == true && chkCustomer.Checked)
                {
                    fileStream = null;
                    pdf.CreatePALetter(sendCustomer, selectedBuilding, GetPM(), txtSubject.Text, DateTime.Now, html, null, justify, chkUseLetterhead.Checked, out fileStream); //.Replace("/", "\\")
                    String tempFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + sendCustomer.accNumber + "_" + txtSubject.Text + "_" + cCount.ToString();
                    if (tempFileName.Length > 32) { tempFileName = tempFileName.Substring(0, 32); }
                    fileName = tempFileName + ".pdf";
                    fileName = fileName.Replace("\\", "_").Replace("/", "_");
                    string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                    Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                    fileName = r.Replace(fileName, "");
                }
                cCount++;
                if (!sendNow && rtfEditor.Enabled == true)
                {
                    bool onlyone = false;
                    if (includedCustomers.Count > 0)
                    {
                        DialogResult drMsg = MessageBox.Show("Multiple customers have been selected. Click Yes to open all documents or No to open first document", "Documents", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (drMsg == DialogResult.No) { onlyone = true; }
                    }
                    OpenInAnotherApp(fileStream, fileName);
                    if (onlyone) { break; }
                }
                else
                {
                    bool success = true;
                    bool delay = includedCustomers.Count > 1;
                    String mailBody = "";
                    if (!String.IsNullOrEmpty(txtBody.Text))
                    {
                        mailBody = txtBody.Text;
                    }
                    else
                    {
                        mailBody = "Dear Owner" + Environment.NewLine + Environment.NewLine;
                        mailBody += "Attached please find correspondence for your attention." + Environment.NewLine + Environment.NewLine;
                    }
                    mailBody += "Account #: " + sendCustomer.accNumber + ". For any queries on your account, please contact " + Controller.user.name + " on email: " + Controller.user.email + ", tel: " + Controller.user.phone + "." + Environment.NewLine + Environment.NewLine;
                    mailBody += "Regards" + Environment.NewLine;
                    mailBody += "Astrodon (Pty) Ltd" + Environment.NewLine;
                    mailBody += "You're in good hands";
                    txtStatus.Text += "Creating documents: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;

                    List<String> attachments = new List<string>();
                    Dictionary<String, byte[]> liveAttachments = new Dictionary<string, byte[]>();
                    String filePath = "";
                    if (HasLetter() && !String.IsNullOrEmpty(fileName) && fileStream != null)
                    {
                        if (CreateDocument(attachmentLocation, fileName, fileStream, out status))
                        {
                            filePath = Path.Combine(attachmentLocation, fileName);
                            attachments.Add(filePath);
                            try { liveAttachments.Add(fileName, fileStream); }
                            catch { }
                        }
                    }

                    foreach (Attachments a in EmailDocs)
                    {
                        int fileID;
                        byte[] aByte = GetAttachment(a.FileName, 2, out fileID);
                        if (CreateDocument(attachmentLocation, a.FileName, aByte, out status))
                        {
                            attachments.Add(Path.Combine(attachmentLocation, a.FileName));
                            try { liveAttachments.Add(a.FileName, aByte); }
                            catch { }
                        }
                    }

                    SMSMessage m = new SMSMessage();
                    if (!String.IsNullOrEmpty(txtSMS.Text.Trim()) && !String.IsNullOrEmpty(sendCustomer.CellPhone))
                    {
                        m.billable = chkBill.Checked;
                        m.building = selectedBuilding.Abbr;
                        m.customer = sendCustomer.accNumber;
                        m.message = txtSMS.Text;
                        m.number = sendCustomer.CellPhone;
                        m.sender = Controller.user.id.ToString();
                        smsSender.SendMessage(m, !delay, out status);
                    }

                    if (attachments.Count > 0)
                    {
                        String insertStatus;
                        bool print = false;
                        int cEmailCount = sendCustomer.Email.Length;
                        foreach (String cEmail in sendCustomer.Email) { if (String.IsNullOrEmpty(cEmail)) { cEmailCount--; } }
                        print = cEmailCount == 0;

                        if (chkBuilding.Checked && cmbFolder.SelectedItem != null && onetimebuildingattachments == false)
                        {
                            onetimebuildingattachments = true;
                            if (buildingFolder.ToLower() != "root" && ftpUploadFolder != ftpClient.WorkingDirectory)
                            {
                                ftpClient.WorkingDirectory = ftpUploadFolder;
                                //MessageBox.Show(ftpClient.WorkingDirectory);
                                ftpClient.ChangeDirectory(false);
                            }

                            pdf.CreatePALetter(null, selectedBuilding, GetPM(), txtSubject.Text, DateTime.Now, html, null, justify, chkUseLetterhead.Checked, out fileStream); //.Replace("/", "\\")
                            String tempFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + selectedBuilding.Abbr + "_" + txtSubject.Text + "_" + cCount.ToString();
                            if (tempFileName.Length > 32) { tempFileName = tempFileName.Substring(0, 32); }
                            String bfileName = tempFileName + ".pdf";
                            bfileName = bfileName.Replace("\\", "_").Replace("/", "_");
                            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                            bfileName = r.Replace(bfileName, "");

                            String bfilePath = Path.Combine(attachmentLocation, bfileName);
                            attachments.Add(bfilePath);

                            try
                            {
                                txtStatus.Text += "Uploading documents: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                                foreach (String attach in attachments)
                                {
                                    if (attach != filePath)
                                    {
                                        if (ftpClient.Upload(attach, Path.GetFileName(attach), false))
                                        {
                                            txtStatus.Text += Path.GetFileName(attach) + " uploaded: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                                        }
                                        else
                                        {
                                            txtStatus.Text += "Error uploading " + Path.GetFileName(attach) + ": " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                                            success = false;
                                            MessageBox.Show("Error processing...");
                                            return;
                                        }
                                    }
                                }
                                txtStatus.Text += "Completed uploading documents: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "Error uploading documents: " + ex.Message + Environment.NewLine;
                            }
                            attachments.Remove(bfilePath);
                        }

                        if (!print)
                        {
                            txtStatus.Text += "Setting up email: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;

                            String cc = String.IsNullOrEmpty(txtCC.Text) ? " " : txtCC.Text;
                            String bcc = String.IsNullOrEmpty(txtBCC.Text) ? " " : txtBCC.Text;
                            if (delay)
                            {
                                if (Controller.user.id != 1)
                                {
                                    bool insertSuccess = dataHandler.InsertLetter(selectedBuilding.PM, sendCustomer.Email, txtSubject.Text + ": " + sendCustomer.accNumber + " " + DateTime.Now.ToString(), mailBody, true, cc, bcc, false, attachments.ToArray(), sendCustomer.accNumber, out insertStatus);
                                    if (!insertSuccess)
                                    {
                                        MessageBox.Show("Error processing..." + insertStatus);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                bool mailSuccess = dataHandler.InsertLetter(selectedBuilding.PM, sendCustomer.Email, txtSubject.Text + ": " + sendCustomer.accNumber + " " + DateTime.Now.ToString(), mailBody, true, cc, bcc, false, attachments.ToArray(), sendCustomer.accNumber, out insertStatus, true);
                                if (!mailSuccess)
                                {
                                    MessageBox.Show("Error sending mail to " + sendCustomer.Email[0] + ": " + status);
                                    return;
                                }
                                else
                                {
                                    txtStatus.Text += "Mail sent: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                                }
                                if (chkInbox.Checked)
                                {
                                    txtStatus.Text += "Uploading documents: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                                    foreach (KeyValuePair<String, byte[]> printAttachment in liveAttachments)
                                    {
                                        String uploadFileName = Path.Combine(attachmentLocation, printAttachment.Key);
                                        File.WriteAllBytes(uploadFileName, printAttachment.Value);
                                        String actFileTitle = Path.GetFileNameWithoutExtension(uploadFileName);
                                        String actFile = Path.GetFileName(uploadFileName);
                                        try
                                        {
                                            MySqlConnector mySqlConn = new MySqlConnector();
                                            mySqlConn.InsertStatement(actFileTitle, "Customer Statements", actFile, sendCustomer.accNumber, sendCustomer.Email);
                                            Classes.Sftp ftpClient = new Classes.Sftp(null, true);
                                            ftpClient.Upload(uploadFileName, actFile, false);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Error uploading documents..." + ex.Message);
                                            return;
                                        }
                                    }
                                    txtStatus.Text += "Completed Uploading documents: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                                }
                            }
                        }
                        else if (!chkDisablePrint.Checked)
                        {
                            txtStatus.Text += "Printing documents: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                            foreach (String attachment in attachments) { SendToPrinter(attachment); }
                            txtStatus.Text += "Completed printing documents: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + Environment.NewLine;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nothing to print or send");
                    }
                }
            }
            this.Cursor = Cursors.Arrow;
            if (sendNow)
            {
                String updateJobQuery = "UPDATE tblPMJob SET status = 'APPROVED', completedate = getdate() WHERE id = " + jobID.ToString();
                if (dataHandler.SetData(updateJobQuery, null, out status) > 0)
                {
                    MessageBox.Show("Processing complete");
                }
            }
        }

        private bool CreateDocument(String attachmentLocation, String fileName, byte[] fileStream, out String status)
        {
            if (File.Exists(Path.Combine(attachmentLocation, fileName)))
            {
                status = "OK";
                return true;
            }
            else
            {
                try
                {
                    File.WriteAllBytes(Path.Combine(attachmentLocation, fileName), fileStream);
                    status = "OK";
                    return true;
                }
                catch (Exception ex)
                {
                    status = ex.Message;
                    return false;
                }
            }
        }

        private void SendToPrinter(String fileName)
        {
            if (!printerSet)
            {
                frmPrintDialog printDialog = new frmPrintDialog();
                if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SetDefaultPrinter(printDialog.selectedPrinter);
                    Properties.Settings.Default.defaultPrinter = printDialog.selectedPrinter;
                    Properties.Settings.Default.Save();
                    printerSet = true;
                }
            }

            using (Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Verb = "print",
                    FileName = fileName
                }
            })
            {
                p.Start();
                System.Threading.Thread.Sleep(5000);
            }
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        private User GetPM()
        {
            if (creator != 0)
            {
                Users u = new Users();
                User pm = u.GetUser(creator);
                return pm;
            }
            else
            {
                return null;
            }
        }

        private void btnSave4_Click(object sender, EventArgs e)
        {
            String emailQuery = "UPDATE tblPMJob SET cc = @cc, bcc = @bcc, subject = @subject, body = @body, sms = @sms, billcustomer = @bill WHERE id = @jobID";
            sqlParms.Clear();
            sqlParms.Add("@cc", (String.IsNullOrEmpty(txtCC.Text) ? " " : txtCC.Text));
            sqlParms.Add("@bcc", (String.IsNullOrEmpty(txtBCC.Text) ? " " : txtBCC.Text));
            sqlParms.Add("@subject", txtSubject.Text);
            sqlParms.Add("@body", (String.IsNullOrEmpty(txtBody.Text) ? " " : txtBody.Text));
            sqlParms.Add("@sms", (String.IsNullOrEmpty(sms) ? " " : sms));
            sqlParms.Add("@bill", chkBill.Checked);
            sqlParms.Add("@jobID", jobID);
            dataHandler.SetData(emailQuery, sqlParms, out status);
            ProcessMessage("updated job");
        }

        private void btnCancel4_Click(object sender, EventArgs e)
        {
            ResetForm(tbEmail);
        }

        private void UpdateJobStatus(String status)
        {
            String query = "INSERT INTO tblPMJobStatus(jobID, actioned, status) VALUES(@jobID, @actioned, @status)";
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@jobID", jobID);
            sqlParms.Add("@actioned", Controller.user.id);
            sqlParms.Add("@status", status);
            dataHandler.SetData(query, sqlParms, out status);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            String newStatus = (Controller.user.usertype == 2 ? "PENDING" : "REVIEW");
            UpdateJobStatus(newStatus);
            ProcessMessage("submitted job for " + (newStatus == "PENDING" ? "action" : "review"));
            String submitQuery = "UPDATE tblPMJob SET status = '" + newStatus + "' WHERE (status = 'NEW' or status = 'ASSIGNED' or status = 'REWORK') AND id = " + jobID.ToString();
            if (newStatus == "REVIEW")
            {
                String updateQuery = "UPDATE tblJobUpdate SET pmLastUpdated = getdate(), pmid = " + creator;
                dataHandler.SetData(updateQuery, null, out status);
            }
            dataHandler.SetData(submitQuery, null, out status);
            if (Controller.user.usertype == 4)
            {
                String avPAQuery = "UPDATE tblPAStatus SET paStatus = 'True', availableSince = getdate() WHERE paID = " + Controller.user.id.ToString();
                dataHandler.SetData(avPAQuery, null, out status);
                Controller.AssignJob();
            }
            Controller.mainF.ShowJobs();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            //if (ValidateProcess())
            //{
            UpdateJobStatus("COMPLETED");
            ProcessDocuments(true);
            String selfProcess = "UPDATE tblPMJob SET processedBy = " + Controller.user.id.ToString() + ", completedate = getdate() WHERE id = " + jobID.ToString();
            dataHandler.SetData(selfProcess, null, out status);
            Controller.mainF.ShowJobs();
            //}
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            //if (ValidateProcess())
            //{
            UpdateJobStatus("COMPLETED");
            ProcessDocuments(true);
            Controller.mainF.ShowJobs();
            //}
        }

        private void btnRework_Click(object sender, EventArgs e)
        {
            UpdateJobStatus("REWORK");
            String submitQuery = "UPDATE tblPMJob SET status = 'REWORK' WHERE id = " + jobID.ToString();
            String updateQuery = "UPDATE tblJobUpdate SET paLastUpdated = getdate(), paid = " + processor;
            dataHandler.SetData(updateQuery, null, out status);
            ProcessMessage("submitted job for rework");
            dataHandler.SetData(submitQuery, null, out status);
            Controller.mainF.ShowJobs();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(rtfEditor.SelectedText)) { Clipboard.SetText(rtfEditor.SelectedText, TextDataFormat.Text); }
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(rtfEditor.SelectedText))
            {
                Clipboard.SetText(rtfEditor.SelectedText, TextDataFormat.Text);
                rtfEditor.SelectedText = "";
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Rtf) || Clipboard.ContainsText(TextDataFormat.Text))
            {
                DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Text);
                rtfEditor.Paste(myFormat);
            }
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            if (rtfEditor.SelectionFont.Bold)
            {
                rtfEditor.SelectionFont = new Font(rtfEditor.SelectionFont, rtfEditor.SelectionFont.Style ^ FontStyle.Bold);
            }
            else
            {
                rtfEditor.SelectionFont = new Font(rtfEditor.SelectionFont, rtfEditor.SelectionFont.Style | FontStyle.Bold);
            }
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            if (rtfEditor.SelectionFont.Italic)
            {
                rtfEditor.SelectionFont = new Font(rtfEditor.SelectionFont, rtfEditor.SelectionFont.Style ^ FontStyle.Italic);
            }
            else
            {
                rtfEditor.SelectionFont = new Font(rtfEditor.SelectionFont, rtfEditor.SelectionFont.Style | FontStyle.Italic);
            }
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            if (rtfEditor.SelectionFont.Underline)
            {
                rtfEditor.SelectionFont = new Font(rtfEditor.SelectionFont, rtfEditor.SelectionFont.Style ^ FontStyle.Underline);
            }
            else
            {
                rtfEditor.SelectionFont = new Font(rtfEditor.SelectionFont, rtfEditor.SelectionFont.Style | FontStyle.Underline);
            }
        }

        private void btnSpell_Click(object sender, EventArgs e)
        {
            spellCheck.Text = rtfEditor.Text;
            spellCheck.SpellCheck();
        }

        private void btnSpellEmail_Click(object sender, EventArgs e)
        {
            spellCheck.Text = txtBody.Text;
            spellCheck.SpellCheck();
        }

        private void txtSMS_TextChanged(object sender, EventArgs e)
        {
            int length = txtSMS.Text.Length;
            sms = txtSMS.Text;
            lblSMS.Text = (160 - length).ToString();
        }

        private void ctxPageBreak_Click(object sender, EventArgs e)
        {
            rtfEditor.SelectedText = "[pagebreak]";
        }

        private void ctxInsertImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "";
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                String sep = String.Empty;
                foreach (ImageCodecInfo c in codecs)
                {
                    String codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                    ofd.Filter = String.Format("{0}{1}{2} ({3})|{3}", ofd.Filter, sep, codecName, c.FilenameExtension);
                    sep = "|";
                }
                ofd.Filter = String.Format("{0}{1}{2} ({3})|{3}", ofd.Filter, sep, "All Files", "*.*");
                if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                {
                    Image image = Image.FromFile(ofd.FileName);
                    int orgHeight = image.Height;
                    int orgWidth = image.Width;
                    int rtbWidth = rtfEditor.Width - 15;
                    Bitmap bmp = new Bitmap(image);
                    if (orgWidth > rtbWidth)
                    {
                        double ratio = (double)rtbWidth / (double)orgWidth;
                        double nh = ratio * (double)orgHeight;
                        int newHeight = (int)nh;
                        bmp = new Bitmap(image, new Size(rtbWidth, newHeight));
                    }
                    Clipboard.SetImage(bmp);
                    rtfEditor.Paste();
                }
            }
        }

        private void dgSupportDocs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProcessAttachments(dgSupportDocs, e.RowIndex, e.ColumnIndex);
        }

        private void dgAttachments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProcessAttachments(dgAttachments, e.RowIndex, e.ColumnIndex);
        }

        private void ProcessAttachments(DataGridView dg, int row, int cell)
        {
            int attachmentType = (dg == dgSupportDocs ? 1 : 2);
            String fileName = dg[2, row].Value.ToString();
            int fileID;
            byte[] file = GetAttachment(fileName, attachmentType, out fileID);
            if (cell == 0)
            {
                OpenInAnotherApp(file, fileName);
            }
            else if (cell == 1 && MessageBox.Show("Confirm delete file " + fileName + "?", "File Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                String fileDeleteQuery = "DELETE FROM tblAttachments WHERE id = " + fileID.ToString();
                dataHandler.SetData(fileDeleteQuery, null, out status);
                LoadDocuments();
            }
        }

        private void OpenInAnotherApp(byte[] data, string filename)
        {
            if (data != null)
            {
                try
                {
                    String tempFolder = System.IO.Path.GetTempPath();
                    String tempFileName = filename.Replace(".pdf", "");

                    filename = System.IO.Path.Combine(tempFolder, filename.Replace("/", "_"));
                    if (filename.Length > 260)
                    {
                        int tempLength = tempFolder.Length + 2;
                        tempFileName = tempFileName.Substring(0, 256 - tempLength);
                        filename = System.IO.Path.Combine(tempFolder, tempFileName + ".pdf");
                    }
                    bool fileExists = false;
                    if (!File.Exists(filename))
                    {
                        try
                        {
                            File.WriteAllBytes(filename, data);
                            fileExists = File.Exists(filename);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error creating document: " + ex.Message);
                        }
                    }
                    else
                    {
                        fileExists = true;
                    }
                    if (fileExists) { System.Diagnostics.Process.Start(filename); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    if (MessageBox.Show("The document " + filename.Replace("/", "_") + " is currently open. Please close and try again", "File Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry)
                    {
                        OpenInAnotherApp(data, filename);
                    }
                }
            }
        }

        private byte[] GetAttachment(String fileName, int attachmentType, out int fileID)
        {
            String fileQuery = "SELECT * FROM tblAttachments WHERE fileName = '" + fileName + "' AND attachmentType = " + attachmentType.ToString();
            sqlParms.Clear();
            sqlParms.Add("@fileName", fileName);
            sqlParms.Add("@at", attachmentType);
            DataSet dsAttachment = dataHandler.GetData(fileQuery, sqlParms, out status);
            if (dsAttachment != null && dsAttachment.Tables.Count > 0 && dsAttachment.Tables[0].Rows.Count > 0)
            {
                byte[] file = (byte[])dsAttachment.Tables[0].Rows[0]["fileContent"];
                fileID = int.Parse(dsAttachment.Tables[0].Rows[0]["id"].ToString());
                return file;
            }
            else
            {
                fileID = 0;
                return null;
            }
        }

        private void cmbFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFolder.SelectedItem != null && cmbFolder.SelectedIndex > 0)
            {
                uploadDirectory = selectedBuilding.webFolder + "//" + cmbFolder.SelectedItem.ToString();
            }
            else
            {
                uploadDirectory = selectedBuilding.webFolder;
            }
        }

        private void ProcessMessage(String message)
        {
            String jobMessage = String.Empty;
            if (message != "added new job")
            {
                jobMessage = Controller.user.name + " " + message + ": Job ID - " + jobID.ToString();
            }
            else
            {
                jobMessage = Controller.user.name + message + ": Job ID - " + jobID.ToString();
            }
            Controller.commClient.SendMessage(jobMessage);
        }

        private void txtLetter_TextChanged(object sender, EventArgs e)
        {
            txtSubject.TextChanged -= txtSubject_TextChanged;
            if (txtSubject.Text != txtLetter.Text) { txtSubject.Text = txtLetter.Text; }
            txtSubject.TextChanged += txtSubject_TextChanged;
            ActivateLetter();
        }

        private void txtSubject_TextChanged(object sender, EventArgs e)
        {
            txtLetter.TextChanged -= txtLetter_TextChanged;
            if (txtLetter.Text != txtSubject.Text) { txtLetter.Text = txtSubject.Text; }
            txtLetter.TextChanged += txtLetter_TextChanged;
            ActivateLetter();
        }

        private void ActivateLetter()
        {
            txtLetter.ReadOnly = false;
            txtLetter.Enabled = true;
            if (!String.IsNullOrEmpty(txtSubject.Text))
            {
                DisableControls(tbLetter, false);
                DefaultLetter();
            }
            else
            {
                DisableControls(tbLetter, true);
                cmbLetter.Enabled = true;
                txtLetter.Enabled = true;
                txtLetter.ReadOnly = false;
            }
        }

        private void insertSupportingDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.frmSupport supportF = new Forms.frmSupport(supportDocs))
            {
                supportF.ShowDialog();
                if (supportF.outFileID != 0)
                {
                    String fileQuery = "SELECT fileContent FROM tblAttachments WHERE id = " + supportF.outFileID.ToString();
                    DataSet dsFile = dataHandler.GetData(fileQuery, null, out status);
                    try
                    {
                        if (dsFile != null && dsFile.Tables.Count > 0 && dsFile.Tables[0].Rows.Count > 0)
                        {
                            DataRow drFile = dsFile.Tables[0].Rows[0];
                            byte[] file = (byte[])drFile["fileContent"];
                            MemoryStream ms = new MemoryStream(file);
                            Image image = Image.FromStream(ms);
                            int orgHeight = image.Height;
                            int orgWidth = image.Width;
                            int rtbWidth = rtfEditor.Width - 15;
                            Bitmap bmp = new Bitmap(image);
                            if (orgWidth > rtbWidth)
                            {
                                double ratio = (double)rtbWidth / (double)orgWidth;
                                double nh = ratio * (double)orgHeight;
                                int newHeight = (int)nh;
                                bmp = new Bitmap(image, new Size(rtbWidth, newHeight));
                            }
                            Clipboard.SetImage(bmp);
                            rtfEditor.Paste();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Cannot embed this file into the document", "Insert File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(rtfEditor.Rtf) && !String.IsNullOrEmpty(txtLetter.Text))
            {
                Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                String templateQuery = String.Empty;
                List<LetterTemplates> lts = templates.Where(c => c.title == txtLetter.Text && c.buildingID == selectedBuilding.ID).ToList();
                if (lts.Count > 1)
                {
                    string deleteQuery = "DELETE FROM tblTemplates WHERE templateName = '" + txtLetter.Text + "' AND buildingID = " + selectedBuilding.ID.ToString();
                    dataHandler.SetData(deleteQuery, null, out status);
                    lts.Clear();
                }
                if (lts.Count == 1)
                {
                    int ltID = Convert.ToInt16(lts[0].id);
                    templateQuery = "UPDATE tblTemplates SET templateContent = @templateContent WHERE id = @id";
                    sqlParms.Add("@id", ltID);
                    sqlParms.Add("@templateContent", rtfEditor.Rtf);
                }
                else
                {
                    templateQuery = "INSERT INTO tblTemplates(buildingID, templateName, templateContent)";
                    templateQuery += " VALUES(@buildingID, @templateName, @templateContent)";
                    sqlParms.Add("@buildingID", selectedBuilding.ID);
                    sqlParms.Add("@templateName", txtLetter.Text);
                    sqlParms.Add("@templateContent", rtfEditor.Rtf);
                }
                if (!String.IsNullOrEmpty(templateQuery) && dataHandler.SetData(templateQuery, sqlParms, out status) > 0)
                {
                    LoadTemplates();
                }
                else
                {
                    MessageBox.Show("Error saving content: " + status, "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (String.IsNullOrEmpty(rtfEditor.Rtf))
            {
                MessageBox.Show("Please enter letter content", "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rtfEditor.Focus();
            }
            else if (String.IsNullOrEmpty(txtLetter.Text))
            {
                MessageBox.Show("Please enter letter title", "Job Management", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLetter.Focus();
            }
        }

        private void cmbLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLetter.SelectedItem != null)
            {
                if (cmbLetter.SelectedValue.ToString() == "0")
                {
                    DisableControls(tbLetter, false);
                    DefaultLetter();
                }
                else
                {
                    String templateID = cmbLetter.SelectedValue.ToString();
                    foreach (LetterTemplates template in templates)
                    {
                        if (template.id == templateID)
                        {
                            txtLetter.Text = template.title;
                            rtfEditor.Rtf = template.content;
                            DisableControls(tbLetter, false);
                            break;
                        }
                    }
                }
            }
        }

        private void tbContent_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = !e.TabPage.Enabled;
        }

        private void btnJustify_Click(object sender, EventArgs e)
        {
            justify = !justify;
            btnJustify.BackColor = (!justify ? Color.Red : Color.Green);
        }

        private void usrJob_Resize(object sender, EventArgs e)
        {
            tbContent.Location = new Point(0, 0);
            tbContent.Width = this.Width;
            tbContent.Height = this.Height;
            dgCustomers.Width = this.Width - 20;
            btnCancel1.Left = this.Width - btnCancel1.Width - 10;
            btnSave1.Left = btnCancel1.Left - btnSave1.Width - 10;
            dgSupportDocs.Width = dgCustomers.Width;
            dgAttachments.Width = dgCustomers.Width;
            txtBody.Width = this.Width - 100;
            txtSMS.Width = this.Width - chkBill.Width - 20;
            chkBill.Left = this.Width - chkBill.Width - 10;
            btnSave4.Left = btnSave1.Left;
            btnCancel4.Left = btnCancel1.Left;
            pnlToolbar.Width = this.Width;
            rtfEditor.Width = this.Width;
            btnSave3.Left = btnSave1.Left;
            btnCancel3.Left = btnCancel1.Left;
            btnTemplate.Left = btnSave3.Left - 10 - btnTemplate.Width;
            btnView.Left = btnTemplate.Left - 10 - btnView.Width;
        }

        private void chkEmail1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (jobCustomers jc in JobCustomers)
            {
                try
                {
                    if (!String.IsNullOrEmpty(jc.email1) && jc.email1 != "" && chkEmail1.Checked) { jc.sEmail1 = true; } else if (!chkEmail1.Checked) { jc.sEmail1 = false; }
                }
                catch { }
            }
            dgCustomers.Invalidate();
        }

        private void chkEmail2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (jobCustomers jc in JobCustomers)
            {
                try
                {
                    if (!String.IsNullOrEmpty(jc.email2) && jc.email2 != "" && chkEmail2.Checked) { jc.sEmail2 = true; } else if (!chkEmail2.Checked) { jc.sEmail2 = false; }
                }
                catch { }
            }
            dgCustomers.Invalidate();
        }

        private void chkEmail3_CheckedChanged(object sender, EventArgs e)
        {
            foreach (jobCustomers jc in JobCustomers)
            {
                try
                {
                    if (!String.IsNullOrEmpty(jc.email3) && jc.email3 != "" && chkEmail3.Checked) { jc.sEmail3 = true; } else if (!chkEmail3.Checked) { jc.sEmail3 = false; }
                }
                catch { }
            }
            dgCustomers.Invalidate();
        }

        private void chkEmail4_CheckedChanged(object sender, EventArgs e)
        {
            foreach (jobCustomers jc in JobCustomers)
            {
                try
                {
                    if (!String.IsNullOrEmpty(jc.email4) && jc.email4 != "" && chkEmail4.Checked) { jc.sEmail4 = true; } else if (!chkEmail4.Checked) { jc.sEmail4 = false; }
                }
                catch { }
            }
            dgCustomers.Invalidate();
        }

        private void chkSMS_CheckedChanged(object sender, EventArgs e)
        {
            foreach (jobCustomers jc in JobCustomers)
            {
                try
                {
                    if (!String.IsNullOrEmpty(jc.cell) && jc.cell != "" && chkSMS.Checked) { jc.sCell = true; } else if (!chkSMS.Checked) { jc.sCell = false; }
                }
                catch { }
            }
            dgCustomers.Invalidate();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using NetSpell.SpellChecker;

namespace Astrodon.Controls {
    public partial class usrJobList : UserControl {
        #region Variables
        List<Building> buildings;
        Building build;
        Building b = null;
        private List<Customer> customers = new List<Customer>();
        private int jID;
        SqlDataHandler dm;
        int qPM;
        bool isnewjob = false;
        private Microsoft.Office.Interop.Word.Application MSdoc;
        object Unknown = Type.Missing;
        NetSpell.SpellChecker.Spelling spellCheck;
        NetSpell.SpellChecker.Dictionary.WordDictionary dictionary;
        #endregion

        #region Startup
        public usrJobList(int jobID) {
            InitializeComponent();

            spellCheck = new Spelling(this.components);
            dictionary = new NetSpell.SpellChecker.Dictionary.WordDictionary(this.components);
            dictionary.DictionaryFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dic");
            spellCheck.Dictionary = dictionary;
            spellCheck.EndOfText += spellCheck_EndOfText;
            spellCheck.DeletedWord += spellCheck_DeletedWord;
            spellCheck.ReplacedWord += spellCheck_ReplacedWord;

            dm = new SqlDataHandler();
            tblEmbedTypeTableAdapter.Fill(this.astrodonDataSet.tblEmbedType);
            try {
                jID = jobID;
                if (jID == 0) {
                    isnewjob = true;
                    List<Building> allBuildings = new Buildings(false).buildings;
                    buildings = new List<Building>();
                    foreach (int bid in Controller.user.buildings) {
                        foreach (Building b in allBuildings) {
                            if (bid == b.ID && !buildings.Contains(b)) {
                                buildings.Add(b);
                                break;
                            }
                        }
                    }
                    buildings.Sort(new BuildingComparer("Name", SortOrder.Ascending));
                }
            } catch {

            }
        }
        void spellCheck_ReplacedWord(object sender, ReplaceWordEventArgs e) {
            int start = this.txtEmail.SelectionStart;
            int length = this.txtEmail.SelectionLength;

            this.txtEmail.Select(e.TextIndex, e.Word.Length);
            this.txtEmail.SelectedText = e.ReplacementWord;

            if (start > this.txtEmail.Text.Length) { start = this.txtEmail.Text.Length; }

            if ((start + length) > this.txtEmail.Text.Length) { length = 0; }

            this.txtEmail.Select(start, length);
        }

        void spellCheck_DeletedWord(object sender, SpellingEventArgs e) {

            int start = this.txtEmail.SelectionStart;
            int length = this.txtEmail.SelectionLength;

            this.txtEmail.Select(e.TextIndex, e.Word.Length);
            this.txtEmail.SelectedText = "";

            if (start > this.txtEmail.Text.Length) { start = this.txtEmail.Text.Length; }
            if ((start + length) > this.txtEmail.Text.Length) { length = 0; }

            this.txtEmail.Select(start, length);
        }


        void spellCheck_EndOfText(object sender, EventArgs e) {
            MessageBox.Show("End of text");
        }
        private void usrJobList_Load(object sender, EventArgs e) {
            foreach (Control c in grpCustomer.Controls) {
                if (c.GetType() == typeof(TextBox)) { (c as TextBox).ReadOnly = true; }
            }
            if (Controller.user.usertype == 2 && jID == 0) {
                cmbBuildings.SelectedIndexChanged -= cmbBuildings_SelectedIndexChanged;
                cmbBuildings.DataSource = buildings;
                cmbBuildings.DisplayMember = "Name";
                cmbBuildings.ValueMember = "ID";
                cmbBuildings.SelectedIndex = -1;
                cmbBuildings.SelectedIndexChanged += cmbBuildings_SelectedIndexChanged;
                ClearJob();

                btnApprove.Enabled = false;
                btnRework.Enabled = false;
                btnSave.Enabled = true;
                btnSaveNewJob.Enabled = false;
                btnProcess.Enabled = false;
                btnView.Enabled = false;
                btnAddAttachment.Enabled = false;
                btnNewDoc.Enabled = false;

                btnUpdateAttach.Enabled = false;
                btnCreateTemplate.Enabled = false;

                lblNewMessage.Visible = true;
            } else {
                btnSave.Enabled = false;
                LoadJob();
            }

        }
        private void btnCancelNewJob_Click(object sender, EventArgs e) {
            ClearJob();
        }
        private void ClearJob() {
            cmbBuildings.SelectedIndex = -1;
            chkCustomers.Items.Clear();
            cmbAction.SelectedIndex = -1;
            attachmentAdapter.Fill(this.astrodonDataSet.attachments, 0);
            txtTopic.Text = String.Empty;
            txtSubject.Text = String.Empty;
            txtNotes.Text = String.Empty;
            cmbTemplate.Items.Clear();
            foreach (Control c in grpCustomer.Controls) {
                if (c.GetType() == typeof(TextBox)) { (c as TextBox).Text = ""; }
            }
        }

        #endregion

        #region New Job
        private void cmbBuildings_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (cmbBuildings.SelectedItem != null && cmbBuildings.SelectedValue.ToString() != "0") {
                    String code = cmbBuildings.SelectedValue.ToString();
                    if (buildings == null) { buildings = new Buildings(false).buildings; }
                    foreach (Building b in buildings) {
                        if (b.ID.ToString() == code) {
                            build = b;
                            break;
                        }
                    }

                    customers = Controller.pastel.AddCustomers(build.Abbr, build.DataPath);

                    if (build != null) {
                        chkCustomers.Items.Clear();
                        if (customers.Count > 0) {
                            foreach (Customer c in customers) {
                                chkCustomers.Items.Add(c.accNumber);
                            }
                        }
                        LoadTemplates(build.ID);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadTemplates(int buildID) {
            String templateQuery = "SELECT id, buildingID, templateName, templateContent FROM tblTemplates WHERE buildingID = " + buildID.ToString();
            String status;
            DataSet dsTemplate = dm.GetData(templateQuery, null, out status);
            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0) {
                cmbTemplate.Items.Clear();
                foreach (DataRow dr in dsTemplate.Tables[0].Rows) {
                    cmbTemplate.Items.Add(dr["templateName"].ToString());
                }
            }
        }
        private void btnAddAttachment_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileNames.Length > 0) {
                foreach (String fName in ofd.FileNames) {
                    String fileName = "";
                    if (Path.GetExtension(fName).StartsWith(".doc")) {
                        fileName = word2PDF(fName);
                    } else {
                        fileName = fName;
                    }
                    String attachQuery = "INSERT INTO tblJobAttachments(jobID, fileType, fileName, fileContent, creator, createDate)";
                    attachQuery += " VALUES(@jobID, 1, @fileName, @fileContent, @user, getdate())";
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@jobID", jID);
                    sqlParms.Add("@fileName", Path.GetFileName(fileName));
                    sqlParms.Add("@user", Controller.user.id);
                    byte[] file;
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) { using (var reader = new BinaryReader(stream)) { file = reader.ReadBytes((int)stream.Length); } }
                    sqlParms.Add("@fileContent", file);
                    String status;
                    dm.SetData(attachQuery, sqlParms, out status);
                    attachmentAdapter.Fill(this.astrodonDataSet.attachments, jID);
                    ProcessMessage("added file");
                }
            }
        }


        #endregion


        private void LoadJob() {
            String status;
            String query = "SELECT id, creator, buildingID, topic, subject, notes, upload, email, emailBody, currentStatus, customerAction FROM tblJob WHERE (id = " + jID.ToString() + ")";
            String customerQuery = "SELECT id, jobID, customerID FROM tblJobCustomer WHERE jobID = " + jID.ToString();
            DataSet ds = dm.GetData(query, null, out status);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                btnView.Enabled = true;
                DataRow dr = ds.Tables[0].Rows[0];
                qPM = int.Parse(dr["creator"].ToString());
                if (Controller.user.usertype == 4) {
                    if (dr["currentStatus"].ToString() == "ASSIGNED") {
                        String updQuery = "UPDATE tblJob SET currentStatus = 'IN PROGRESS' WHERE id = " + jID.ToString();
                        String statQuery = "INSERT INTO tblJobStatus(jobID, status, statusDate) VALUES(" + jID.ToString() + ", 'IN PROGRESS', getdate())";
                        dm.SetData(updQuery, null, out status);
                        dm.SetData(statQuery, null, out status);
                    }
                    btnProcess.Enabled = false;
                    btnCancelNewJob.Enabled = false;
                    btnApprove.Enabled = false;
                    btnRework.Enabled = false;
                } else if (Controller.user.usertype == 2 && jID != 0) {
                    String cStatus = dr["currentStatus"].ToString();
                    if (cStatus == "NEW" || cStatus == "ASSIGNED") {
                        btnProcess.Enabled = true;
                        btnSaveNewJob.Enabled = true;
                        btnCancelNewJob.Enabled = false;
                        btnApprove.Enabled = false;
                        btnRework.Enabled = false;
                        btnAddAttachment.Enabled = true;
                        btnNewDoc.Enabled = true;
                        btnCreateTemplate.Enabled = true;
                        cmbTemplate.Enabled = true;
                    } else if (cStatus == "REVIEW") {
                        btnProcess.Enabled = false;
                        btnSaveNewJob.Enabled = false;
                        btnCancelNewJob.Enabled = false;
                        btnApprove.Enabled = true;
                        btnAddAttachment.Enabled = false;
                        btnNewDoc.Enabled = false;
                        btnCreateTemplate.Enabled = false;
                        cmbTemplate.Enabled = false;
                        btnRework.Enabled = true;
                    } else {
                        btnProcess.Enabled = false;
                        btnSaveNewJob.Enabled = false;
                        btnCancelNewJob.Enabled = false;
                        btnApprove.Enabled = false;
                        btnAddAttachment.Enabled = false;
                        btnNewDoc.Enabled = false;
                        btnCreateTemplate.Enabled = false;
                        cmbTemplate.Enabled = false;
                        btnRework.Enabled = false;
                    }
                }
                int bid = int.Parse(dr["buildingID"].ToString());
                GetBuilding(bid);
                cmbBuildings.SelectedIndexChanged -= cmbBuildings_SelectedIndexChanged;
                cmbBuildings.Items.Clear();
                cmbBuildings.Items.Add(b.Name);
                cmbBuildings.SelectedIndex = 0;
                cmbBuildings.Enabled = false;
                LoadTemplates(b.ID);

                String action = (bool.Parse(dr["upload"].ToString()) ? "Upload to Building" : "Send to Customer");
                cmbAction.SelectedItem = action;
                cmbAction.Enabled = false;
                String customerAction = (!bool.Parse(dr["customerAction"].ToString()) ? "All customers" : "Selected customers");
                //MessageBox.Show(customerAction);
                cmbCustomers.SelectedItem = customerAction;
                DataSet dsCustomers = dm.GetData(customerQuery, null, out status);
                if (dsCustomers != null && dsCustomers.Tables.Count > 0 && dsCustomers.Tables[0].Rows.Count > 0) {
                    foreach (DataRow drCustomer in dsCustomers.Tables[0].Rows) {
                        chkCustomers.Items.Add(drCustomer["customerID"].ToString(), true);
                    }
                }
                if (chkCustomers.Items.Count == 1) {
                    try {
                        String customerCode = chkCustomers.Items[0].ToString();
                        String customerString = Controller.pastel.GetCustomer(build.DataPath, customerCode);
                        Customer c = new Customer(customerString);
                        c.SetDeliveryInfo(Controller.pastel.DeliveryInfo(build.DataPath, c.accNumber));
                        txtAccount.Text = c.accNumber;
                        if (c.address.Length > 0) {
                            txtAddress1.Text = c.address[0];
                            if (c.address.Length > 1) {
                                txtAddress2.Text = c.address[1];
                                if (c.address.Length > 2) {
                                    txtAddress3.Text = c.address[2];
                                    if (c.address.Length > 3) {
                                        txtAddress4.Text = c.address[3];
                                        if (c.address.Length > 4) {
                                            txtAddress5.Text = c.address[4];
                                        }
                                    }
                                }
                            }
                        }
                        txtDescription.Text = c.description;
                        txtTelephone.Text = c.Telephone;
                        txtFax.Text = c.Fax;
                        txtCell.Text = c.CellPhone;
                        txtContact.Text = c.Contact;
                        if (c.Email.Length > 0) {
                            try { txtEmail0.Text = c.Email[0]; } catch { }
                            try { txtEmail1.Text = c.Email[1]; } catch { }
                            try { txtEmail2.Text = c.Email[2]; } catch { }
                            try { txtEmail3.Text = c.Email[3]; } catch { }

                        }
                    } catch { }
                }
                attachmentAdapter.Fill(this.astrodonDataSet.attachments, jID);
                txtTopic.Text = dr["topic"].ToString();
                txtSubject.Text = dr["subject"].ToString();
                btnNewDoc.Enabled = !String.IsNullOrEmpty(txtSubject.Text);
                //txtSubject.ReadOnly = true;
                txtNotes.Text = dr["notes"].ToString();
                txtEmail.Text = dr["emailBody"].ToString();
            }
        }


        private void OpenInAnotherApp(byte[] data, string filename) {
            if (data != null) {
                String tempFolder = System.IO.Path.GetTempPath();
                filename = System.IO.Path.Combine(tempFolder, filename);
                File.WriteAllBytes(filename, data);
            }
            System.Diagnostics.Process.Start(filename);
        }
        private String word2PDF(object Source) {   //Creating the instance of Word Application
            String target = Path.GetFileName(Source.ToString());
            target = Path.GetFileNameWithoutExtension(target);
            target = target + ".pdf";
            target = Path.Combine(System.IO.Path.GetTempPath(), target);
            object Target = target;
            if (MSdoc == null) MSdoc = new Microsoft.Office.Interop.Word.Application();

            try {
                MSdoc.Visible = false;
                MSdoc.Documents.Open(ref Source, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown);
                MSdoc.Application.Visible = false;
                MSdoc.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMinimize;

                object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;

                MSdoc.ActiveDocument.SaveAs(ref Target, ref format, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown);
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            } finally {
                if (MSdoc != null) {
                    MSdoc.Documents.Close(ref Unknown, ref Unknown, ref Unknown);
                    //WordDoc.Application.Quit(ref Unknown, ref Unknown, ref Unknown);
                }
                // for closing the application
                MSdoc.Quit(ref Unknown, ref Unknown, ref Unknown);
            }
            return target;
        }



        private void GetBuilding(int buildingID) {
            customers = new List<Customer>();
            Buildings builds = new Buildings(false);
            b = null;
            foreach (Building build in builds.buildings) {
                if (build.ID == buildingID) {
                    b = build;
                    break;
                }
            }
            if (b != null) {
                customers = Controller.pastel.AddCustomers(b.Abbr, b.DataPath);
                this.build = b;
            }

        }


        private void chkCustomers_MouseDown(object sender, MouseEventArgs e) {
            int lno = 0;
            int idx = chkCustomers.IndexFromPoint(e.Location);
            try {
                String customerCode = chkCustomers.Items[idx].ToString();
                String customerString = Controller.pastel.GetCustomer(build.DataPath, customerCode);
                Customer c = new Customer(customerString);
                c.SetDeliveryInfo(Controller.pastel.DeliveryInfo(build.DataPath, c.accNumber));
                txtAccount.Text = c.accNumber;
                if (c.address.Length > 0) {
                    txtAddress1.Text = c.address[0];
                    if (c.address.Length > 1) {
                        txtAddress2.Text = c.address[1];
                        if (c.address.Length > 2) {
                            txtAddress3.Text = c.address[2];
                            if (c.address.Length > 3) {
                                txtAddress4.Text = c.address[3];
                                if (c.address.Length > 4) {
                                    txtAddress5.Text = c.address[4];
                                }
                            }
                        }
                    }
                }
                txtDescription.Text = c.description;
                txtTelephone.Text = c.Telephone;
                txtFax.Text = c.Fax;
                txtCell.Text = c.CellPhone;
                txtContact.Text = c.Contact;
                if (c.Email.Length > 0) {
                    try { txtEmail0.Text = c.Email[0]; } catch { }
                    try { txtEmail1.Text = c.Email[1]; } catch { }
                    try { txtEmail2.Text = c.Email[2]; } catch { }
                    try { txtEmail3.Text = c.Email[3]; } catch { }

                }
                chkCustomers.SetItemChecked(idx, chkCustomers.GetItemChecked(idx));
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void cmbTemplate_SelectedIndexChanged(object sender, EventArgs e) {
            if (cmbTemplate.SelectedItem != null) {
                String fileName = cmbTemplate.SelectedItem.ToString();
                String templateQuery = "SELECT templateContent FROM tblTemplates WHERE buildingID = " + build.ID.ToString() + " AND templateName = '" + fileName + "'";
                String status;
                DataSet dsTemplate = dm.GetData(templateQuery, null, out status);
                if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0) {
                    DataRow dr = dsTemplate.Tables[0].Rows[0];
                    String attachQuery = "INSERT INTO tblJobAttachments(jobID, fileType, fileName, fileString, creator, createDate)";
                    attachQuery += " VALUES(@jobID, 2, @fileName, @fileContent, @user, getdate())";
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@jobID", jID);
                    sqlParms.Add("@fileName", fileName);
                    sqlParms.Add("@fileContent", dr["templateContent"].ToString());
                    sqlParms.Add("@user", Controller.user.id);
                    dm.SetData(attachQuery, sqlParms, out status);
                    attachmentAdapter.Fill(this.astrodonDataSet.attachments, jID);
                    ProcessMessage("added file");
                }
            }
        }

        private void btnNewDoc_Click(object sender, EventArgs e) {
            Forms.frmDocument doc = new Forms.frmDocument(null, build);
            String insertQuery = String.Empty;
            if (doc.ShowDialog() == DialogResult.OK) {
                String fileName = (cmbCustomers.SelectedItem.ToString() == "All customers" ? "ALL" : txtAccount.Text) + "_" + txtSubject.Text + "_" + DateTime.Now.ToString("yyyyMMdd");
                //Forms.frmPrompt prompt = new Forms.frmPrompt();
                //DialogResult dr = prompt.ShowDialog();
                //if (dr == DialogResult.OK) {
                insertQuery = "INSERT INTO tblJobAttachments(jobID, fileType, fileName, fileString, creator, createDate)";
                insertQuery += " VALUES(@jobID, 2, @fileName, @fileContent, @user, getdate())";

                //} else if (dr == DialogResult.Abort) {
                //    if (MessageBox.Show("No filename provided!", "Documents", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Retry) {
                //        dr = prompt.ShowDialog();
                //        if (dr == DialogResult.OK) {
                //            insertQuery = "INSERT INTO tblJobAttachments(jobID, fileType, fileName, fileString, creator, createDate)";
                //            insertQuery += " VALUES(@jobID, 2, @fileName, @fileContent, @user, getdate())";
                //        }
                //    }
                //}
                if (!String.IsNullOrEmpty(insertQuery)) {
                    String status;
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@jobID", jID);
                    sqlParms.Add("@fileName", fileName);
                    sqlParms.Add("@fileContent", doc.rtf);
                    sqlParms.Add("@user", Controller.user.id);
                    dm.SetData(insertQuery, sqlParms, out status);
                    attachmentAdapter.Fill(this.astrodonDataSet.attachments, jID);
                    ProcessMessage("added file");
                }
            }
        }

        private void btnCreateTemplate_Click(object sender, EventArgs e) {
            Forms.frmDocument doc = new Forms.frmDocument(null, build);
            String insertQuery = String.Empty;
            if (doc.ShowDialog() == DialogResult.OK) {
                Forms.frmPrompt prompt = new Forms.frmPrompt();
                DialogResult dr = prompt.ShowDialog();
                if (dr == DialogResult.OK) {
                    insertQuery = "INSERT INTO tblTemplates(buildingID, templateName, templateContent) VALUES(" + build.ID + ", '" + prompt.fileName + "', '" + doc.rtf + "')";
                } else if (dr == DialogResult.Abort) {
                    if (MessageBox.Show("No filename provided!", "Template", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Retry) {
                        dr = prompt.ShowDialog();
                        if (dr == DialogResult.OK) {
                            insertQuery = "INSERT INTO tblTemplates(buildingID, templateName, templateContent) VALUES(" + build.ID + ", '" + prompt.fileName + "', '" + doc.rtf + "')";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(insertQuery)) {
                    String status;
                    dm.SetData(insertQuery, null, out status);
                    LoadTemplates(build.ID);
                }
            }
        }

        private void dgAttachments_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            String status;
            if (e.RowIndex > -1) {
                String attachmentQuery = "SELECT fileName, fileContent, fileString, fileType FROM tblJobAttachments WHERE id = " + dgAttachments.Rows[e.RowIndex].Cells[0].Value.ToString();
                String DeleteQuery = "DELETE FROM tblJobAttachments WHERE id = " + dgAttachments.Rows[e.RowIndex].Cells[0].Value.ToString();
                DataSet dsAttach = dm.GetData(attachmentQuery, null, out status);
                if (dsAttach != null && dsAttach.Tables.Count > 0 && dsAttach.Tables[0].Rows.Count > 0) {
                    String fileName = dsAttach.Tables[0].Rows[0]["fileName"].ToString();
                    String fileString = dsAttach.Tables[0].Rows[0]["fileString"].ToString();
                    int fileType = int.Parse(dsAttach.Tables[0].Rows[0]["fileType"].ToString());
                    byte[] file = fileType == 1 ? (byte[])dsAttach.Tables[0].Rows[0]["fileContent"] : null;
                    if (e.ColumnIndex == 4) {
                        if (fileType == 1) {
                            OpenInAnotherApp(file, fileName);
                        } else {
                            DialogResult docResult = MessageBox.Show("Click YES to edit, NO to display", "Documents", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (docResult == DialogResult.Yes) {
                                UpdateDocument(fileName, fileString);
                            } else if (docResult == DialogResult.No) {
                                CreateTestDoc();
                            }
                        }
                    } else if (e.ColumnIndex == 5) {
                        if (MessageBox.Show("Confirm delete of this file?", "Attachments", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                            dm.SetData(DeleteQuery, null, out status);
                            attachmentAdapter.Fill(this.astrodonDataSet.attachments, jID);
                        }
                    }
                }
            }
        }

        private void UpdateDocument(String fileName, String fileString) {
            Forms.frmDocument doc = new Forms.frmDocument(fileString, build);
            String insertQuery = String.Empty;
            if (doc.ShowDialog() == DialogResult.OK) {
                insertQuery = "UPDATE tblJobAttachments SET fileString = @fileContent WHERE jobID = @jobID AND fileName = @fileName";
                if (!String.IsNullOrEmpty(insertQuery)) {
                    String status;
                    Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                    sqlParms.Add("@jobID", jID);
                    sqlParms.Add("@fileName", fileName);
                    sqlParms.Add("@fileContent", doc.rtf);
                    dm.SetData(insertQuery, sqlParms, out status);
                    attachmentAdapter.Fill(this.astrodonDataSet.attachments, jID);
                    ProcessMessage("updated file");
                }
            }
        }

        private void btnSaveNewJob_Click(object sender, EventArgs e) {
            UpdateAttachments();
            String status;
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@pm", Controller.user.id);
            sqlParms.Add("@buildID", build.ID);
            sqlParms.Add("@topic", txtTopic.Text);
            sqlParms.Add("@subject", txtSubject.Text);
            sqlParms.Add("@notes", txtNotes.Text);
            sqlParms.Add("@upload", cmbAction.SelectedItem.ToString() == "Upload to Building" ? true : false);
            sqlParms.Add("@email", cmbAction.SelectedItem.ToString() == "Send To Customer" ? true : false);
            sqlParms.Add("@body", txtEmail.Text);
            sqlParms.Add("@customerAction", (cmbCustomers.SelectedItem != null ? (cmbCustomers.SelectedItem.ToString() == "All customers" ? false : true) : false));


            if (jID == 0) {
                String insertJobQuery = "INSERT INTO tblJob(creator, createDate, buildingID, topic, subject, notes, upload, email, currentStatus, emailBody, customerAction)";
                insertJobQuery += " VALUES(@pm, getdate(), @buildID, @topic, @subject, @notes, @upload, @email, 'NEW', @body, @customerAction);";
                insertJobQuery += " SELECT max(id) as jid FROM tblJob";
                DataSet dsJob = dm.GetData(insertJobQuery, sqlParms, out status);
                if (dsJob != null && dsJob.Tables.Count > 0 && dsJob.Tables[0].Rows.Count > 0) {
                    jID = int.Parse(dsJob.Tables[0].Rows[0]["jid"].ToString());
                    String updateAttachmentQuery = "UPDATE tblJobAttachments SET jobID = " + jID.ToString() + " WHERE jobID = 0";
                    dm.SetData(updateAttachmentQuery, null, out status);
                    for (int i = 0; i < chkCustomers.Items.Count; i++) {
                        if (chkCustomers.GetItemChecked(i)) {
                            String customerQuery = "INSERT INTO tblJobCustomer(jobID, customerID) VALUES(@jobID, @customer)";
                            sqlParms.Add("@jobID", jID);
                            sqlParms.Add("@customer", chkCustomers.Items[i].ToString());
                            dm.SetData(customerQuery, sqlParms, out status);
                        }
                    }
                    MessageBox.Show("Job has been submitted");
                    ProcessMessage("added new job");
                    Controller.mainF.ShowJobs();
                }
            } else if (isnewjob) {
                String updateQuery = "UPDATE tblJob SET topic = @topic, subject = @subject, emailBody = @body, notes = @notes WHERE (id = " + jID.ToString() + ");";
                updateQuery += " UPDATE tblPAStatus SET pastatus = 'True' WHERE paid = " + Controller.user.id.ToString();
                dm.SetData(updateQuery, sqlParms, out status);
                MessageBox.Show("Job has been submitted");
                ProcessMessage("added new job");
                Controller.mainF.ShowJobs();
            } else {
                String updateQuery = "UPDATE tblJob SET topic = @topic, subject = @subject, notes = @notes, emailBody = @body, currentStatus = 'REVIEW' WHERE (id = " + jID.ToString() + ");";
                updateQuery += "INSERT INTO tblJobStatus(jobID, status, statusDate) VALUES(" + jID.ToString() + ", 'REVIEW', getdate());";
                updateQuery += " UPDATE tblPAStatus SET pastatus = 'True' WHERE paid = " + Controller.user.id.ToString();
                dm.SetData(updateQuery, sqlParms, out status);
                if (Controller.user.usertype == 4) {
                    Controller.AssignJob();
                    MessageBox.Show("Job has been submitted");
                    ProcessMessage("changed status to review");
                    Controller.mainF.ShowJobs();
                }
            }
        }

        private void btnProcess_Click(object sender, EventArgs e) {
            ShowProcessing(true);
            UpdateAttachments();
            ProcessDocs();
            bool singleCustomer = (GetCustomerCount() == 1);
            ProcessDocuments(singleCustomer);
            ProcessMessage("changed job status to complete");
            ShowProcessing(false);
        }

        private int GetCustomerCount() {
            int customerCount = 0;
            for (int i = 0; i < chkCustomers.Items.Count; i++) {
                if (chkCustomers.GetItemChecked(i)) { customerCount++; }
            }
            return customerCount;
        }

        private void ShowProcessing(bool trigger) {
            if (trigger) {
                lblProcess.Visible = true;
                this.Cursor = Cursors.WaitCursor;
            } else {
                lblProcess.Visible = false;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnApprove_Click(object sender, EventArgs e) {
            ShowProcessing(true);
            btnApprove.Enabled = false;
            UpdateAttachments();
            ProcessDocs();


            bool singleCustomer = (GetCustomerCount() == 1);
            if (ProcessDocuments(singleCustomer)) {
                CompleteJob();
                ProcessMessage("changed job status to complete");
                ShowProcessing(false);
                Controller.mainF.ShowJobs();
            } else {
                ShowProcessing(false);
            }
        }

        private void CompleteJob() {
            String status;
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@pm", Controller.user.id);
            sqlParms.Add("@buildID", build.ID);
            sqlParms.Add("@subject", txtTopic.Text);
            sqlParms.Add("@notes", txtNotes.Text);
            sqlParms.Add("@upload", cmbAction.SelectedItem.ToString() == "Upload to Building" ? true : false);
            sqlParms.Add("@email", cmbAction.SelectedItem.ToString() == "Send To Customer" ? true : false);
            String updateQuery = "UPDATE tblJob SET notes = @notes, currentStatus = 'APPROVED' WHERE (id = " + jID.ToString() + ");";
            updateQuery += "INSERT INTO tblJobStatus(jobID, status, statusDate) VALUES(" + jID.ToString() + ", 'APPROVED', getdate());";
            dm.SetData(updateQuery, sqlParms, out status);
        }
        private void ProcessDocs() {
            String status;
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@pm", Controller.user.id);
            sqlParms.Add("@buildID", build.ID);
            sqlParms.Add("@topic", txtTopic.Text);
            sqlParms.Add("@subject", txtSubject.Text);
            sqlParms.Add("@notes", txtNotes.Text);
            sqlParms.Add("@upload", cmbAction.SelectedItem.ToString() == "Upload to Building" ? true : false);
            sqlParms.Add("@email", cmbAction.SelectedItem.ToString() == "Send To Customer" ? true : false);
            sqlParms.Add("@body", txtEmail.Text);
            sqlParms.Add("@customerAction", (cmbCustomers.SelectedItem != null ? (cmbCustomers.SelectedItem == "All Customers" ? true : false) : true));
            if (jID == 0) {
                String insertJobQuery = "INSERT INTO tblJob(creator, createDate, buildingID, topic, subject, notes, upload, email, currentStatus, emailBody, customerAction)";
                insertJobQuery += " VALUES(@pm, getdate(), @buildID, @topic, @subject, @notes, @upload, @email, 'REVIEW', @body, @customerAction);";
                insertJobQuery += " SELECT max(id) as jid FROM tblJob";
                DataSet dsJob = dm.GetData(insertJobQuery, sqlParms, out status);
                if (dsJob != null && dsJob.Tables.Count > 0 && dsJob.Tables[0].Rows.Count > 0) {
                    jID = int.Parse(dsJob.Tables[0].Rows[0]["jid"].ToString());
                    String updateAttachmentQuery = "UPDATE tblJobAttachments SET jobID = " + jID.ToString() + " WHERE jobID = 0";
                    String updateStatusQuery = "INSERT INTO tblJobStatus(jobID, status, statusDate) VALUES(" + jID.ToString() + ", 'REVIEW', getdate());";
                    dm.SetData(updateAttachmentQuery, null, out status);
                    dm.SetData(updateStatusQuery, null, out status);
                    for (int i = 0; i < chkCustomers.Items.Count; i++) {
                        if (chkCustomers.GetItemChecked(i)) {
                            String customerQuery = "INSERT INTO tblJobCustomer(jobID, customerID) VALUES(@jobID, @customer)";
                            sqlParms.Add("@jobID", jID);
                            sqlParms.Add("@customer", chkCustomers.Items[i].ToString());
                            dm.SetData(customerQuery, sqlParms, out status);
                        }
                    }
                }
            }

        }

        private User GetPM() {
            if (qPM != 0) {
                Users u = new Users();
                User pm = u.GetUser(qPM);
                return pm;
            } else {
                return null;
            }
        }

        private void CreateTestDoc() {
            String status;
            Dictionary<String, byte[]> fixedAttachments = new Dictionary<string, byte[]>();
            Dictionary<String, String> variableAttachments = new Dictionary<string, string>();
            List<byte[]> embedAttachments = new List<byte[]>();
            String attachmentQuery = "SELECT fileName, fileContent, fileString, fileType FROM tblJobAttachments WHERE jobID = " + jID;
            DataSet dsAttach = dm.GetData(attachmentQuery, null, out status);
            if (dsAttach != null && dsAttach.Tables.Count > 0 && dsAttach.Tables[0].Rows.Count > 0) {
                for (int i = 0; i < dsAttach.Tables[0].Rows.Count; i++) {
                    int attType = (dgAttachments.Rows[i].Cells[6].Value != null ? int.Parse(dgAttachments.Rows[i].Cells[6].Value.ToString()) : 2);

                    DataRow dr = dsAttach.Tables[0].Rows[i];
                    String fileName = dr["fileName"].ToString();
                    String fileString = dr["fileString"].ToString();
                    int fileType = int.Parse(dr["fileType"].ToString());
                    byte[] file = fileType == 1 ? (byte[])dr["fileContent"] : null;
                    if (fileType == 1) {
                        if (attType != 1) {
                            if (attType == 2) {
                                fixedAttachments.Add(fileName, file);
                            } else {
                                embedAttachments.Add(file);
                            }
                        }
                    } else {
                        variableAttachments.Add(fileName, fileString);
                    }
                }
            }

            Dictionary<String, byte[]> myAttachments = new Dictionary<string, byte[]>();
            foreach (KeyValuePair<String, byte[]> fileItem in fixedAttachments) {
                if (!myAttachments.Keys.Contains(fileItem.Key)) { myAttachments.Add(fileItem.Key, fileItem.Value); }
            }
            PDF pdf = new PDF();
            if (variableAttachments.Count > 0) {
                foreach (KeyValuePair<String, String> vItem in variableAttachments) {
                    if (!myAttachments.Keys.Contains(vItem.Key)) {
                        byte[] fStream;
                        Customer c = new Customer();
                        if (chkCustomers.CheckedIndices.Count == 1) {
                            c = new Customer(Controller.pastel.GetCustomer(build.DataPath, chkCustomers.Items[0].ToString()));
                            c.SetDeliveryInfo(Controller.pastel.DeliveryInfo(build.DataPath, c.accNumber));
                            List<String> incEmail = new List<string>();
                            if (chkIncEmail0.Checked) { incEmail.Add(txtEmail0.Text); }
                            if (chkIncEmail1.Checked) { incEmail.Add(txtEmail1.Text); }
                            if (chkIncEmail2.Checked) { incEmail.Add(txtEmail2.Text); }
                            if (chkIncEmail3.Checked) { incEmail.Add(txtEmail3.Text); }
                            for (int e = 0; e < c.Email.Length; e++) { if (!incEmail.Contains(c.Email[e])) { c.Email[e] = ""; } }
                        } else {
                            c.accNumber = "<Account>";
                            c.address = new string[] { "<Line 1>", "<Line 2>", "<Line 3>", "<Line 4>", "<Line 5>" };
                            c.description = "<Account Name>";
                        }
                        pdf.CreatePALetter(c, build, GetPM(), txtSubject.Text, DateTime.Now, vItem.Value, embedAttachments, out fStream);
                        OpenInAnotherApp(fStream, vItem.Key + ".pdf");
                    }
                }
            } else {
                byte[] fStream;
                Customer c = new Customer();
                if (chkCustomers.CheckedIndices.Count == 1) {
                    c = new Customer(Controller.pastel.GetCustomer(build.DataPath, chkCustomers.Items[0].ToString()));
                    c.SetDeliveryInfo(Controller.pastel.DeliveryInfo(build.DataPath, c.accNumber));
                    List<String> incEmail = new List<string>();
                    if (chkIncEmail0.Checked) { incEmail.Add(txtEmail0.Text); }
                    if (chkIncEmail1.Checked) { incEmail.Add(txtEmail1.Text); }
                    if (chkIncEmail2.Checked) { incEmail.Add(txtEmail2.Text); }
                    if (chkIncEmail3.Checked) { incEmail.Add(txtEmail3.Text); }
                    for (int e = 0; e < c.Email.Length; e++) { if (!incEmail.Contains(c.Email[e])) { c.Email[e] = ""; } }
                } else {
                    c.accNumber = "<Account>";
                    c.address = new string[] { "<Line 1>", "<Line 2>", "<Line 3>", "<Line 4>", "<Line 5>" };
                    c.description = "<Account Name>";
                }
                pdf.CreatePALetter(c, build, GetPM(), txtSubject.Text, DateTime.Now, "", embedAttachments, out fStream);
                OpenInAnotherApp(fStream, "Test View.pdf");
            }
        }

        private bool ProcessDocuments(bool checkedCustomers) {
            bool success = false;
            String status;
            PDF pdf = new PDF();
            bool delay = false;
            if (cmbAction.SelectedItem.ToString() == "Send to Customer") {
                List<Customer> sendToCustomers = new List<Customer>();
                if (!checkedCustomers) {
                    if (cmbCustomers.SelectedItem != null && cmbCustomers.SelectedItem.ToString() == "All customers") {
                        sendToCustomers = Controller.pastel.AddCustomers("", build.DataPath);
                        delay = true;
                    } else { 
                        for (int i = 0; i < chkCustomers.Items.Count; i++) {
                            Customer c = new Customer(Controller.pastel.GetCustomer(build.DataPath, chkCustomers.Items[i].ToString()));
                            c.SetDeliveryInfo(Controller.pastel.DeliveryInfo(build.DataPath, c.accNumber));
                            sendToCustomers.Add(c);
                        }
                    }
                } else {
                    if (chkCustomers.SelectedIndices.Count > 1) {
                        for (int i = 0; i < chkCustomers.Items.Count; i++) {
                            if (chkCustomers.GetItemChecked(i)) {
                                Customer c = new Customer(Controller.pastel.GetCustomer(build.DataPath, chkCustomers.Items[i].ToString()));
                                c.SetDeliveryInfo(Controller.pastel.DeliveryInfo(build.DataPath, c.accNumber));
                                sendToCustomers.Add(c);
                            }
                        }
                    } else {
                        for (int i = 0; i < chkCustomers.Items.Count; i++) {
                            if (chkCustomers.GetItemChecked(i)) {
                                Customer c = new Customer(Controller.pastel.GetCustomer(build.DataPath, chkCustomers.Items[i].ToString()));
                                c.SetDeliveryInfo(Controller.pastel.DeliveryInfo(build.DataPath, c.accNumber));
                                List<String> incEmail = new List<string>();
                                if (chkIncEmail0.Checked) { incEmail.Add(txtEmail0.Text); }
                                if (chkIncEmail1.Checked) { incEmail.Add(txtEmail1.Text); }
                                if (chkIncEmail2.Checked) { incEmail.Add(txtEmail2.Text); }
                                if (chkIncEmail3.Checked) { incEmail.Add(txtEmail3.Text); }
                                for (int e = 0; e < c.Email.Length; e++) { if (!incEmail.Contains(c.Email[e])) { c.Email[e] = ""; } }
                                sendToCustomers.Add(c);
                            }
                        }
                    }

                }
                if (sendToCustomers.Count > 0) {
                    Dictionary<String, byte[]> fixedAttachments = new Dictionary<string, byte[]>();
                    Dictionary<String, String> variableAttachments = new Dictionary<string, string>();
                    List<byte[]> embedAttachments = new List<byte[]>();
                    String attachmentQuery = "SELECT fileName, fileContent, fileString, fileType FROM tblJobAttachments WHERE jobID = " + jID;
                    DataSet dsAttach = dm.GetData(attachmentQuery, null, out status);
                    if (dsAttach != null && dsAttach.Tables.Count > 0 && dsAttach.Tables[0].Rows.Count > 0) {
                        for (int i = 0; i < dsAttach.Tables[0].Rows.Count; i++) {
                            int attType = (dgAttachments.Rows[i].Cells[6].Value != null ? int.Parse(dgAttachments.Rows[i].Cells[6].Value.ToString()) : 2);

                            DataRow dr = dsAttach.Tables[0].Rows[i];
                            String fileName = dr["fileName"].ToString();
                            String fileString = dr["fileString"].ToString();
                            int fileType = int.Parse(dr["fileType"].ToString());
                            byte[] file = fileType == 1 ? (byte[])dr["fileContent"] : null;
                            if (fileType == 1) {
                                if (attType != 1) {
                                    if (attType == 2) {
                                        fixedAttachments.Add(fileName, file);
                                    } else {
                                        embedAttachments.Add(file);
                                    }
                                }
                            } else {
                                variableAttachments.Add(fileName, fileString);
                            }
                        }
                    }
                    foreach (Customer c in sendToCustomers) {
                        Dictionary<String, byte[]> myAttachments = new Dictionary<string, byte[]>();
                        foreach (KeyValuePair<String, byte[]> fileItem in fixedAttachments) {
                            if (!myAttachments.Keys.Contains(fileItem.Key)) { myAttachments.Add(fileItem.Key, fileItem.Value); }
                        }
                        foreach (KeyValuePair<String, String> vItem in variableAttachments) {
                            if (!myAttachments.Keys.Contains(vItem.Key)) {
                                byte[] fStream;
                                pdf.CreatePALetter(c, build, GetPM(), txtSubject.Text, DateTime.Now, vItem.Value, embedAttachments, out fStream);
                                //pdf.CreatePALetter(null, build, Controller.user, "", DateTime.Now, vItem.Value, out fStream);
                                //OpenInAnotherApp(fStream, "WTF.pdf");
                                myAttachments.Add(vItem.Key + ".pdf", fStream);
                            }
                        }
                        bool hasEmail = false;
                        foreach (String cemail in c.Email) {
                            if (!String.IsNullOrEmpty(cemail) || cemail != "") {
                                hasEmail = true;
                                break;
                            }
                        }
                        if (((c.statPrintorEmail == 1 || c.statPrintorEmail == 2) && myAttachments.Count > 0) || (!hasEmail)) {
                            if (c.statPrintorEmail == 1 || !hasEmail) {
                                foreach (KeyValuePair<String, byte[]> printAttachment in myAttachments) {
                                    String fName = Path.Combine(Path.GetTempPath(), printAttachment.Key);
                                    FileStream fileStream = new FileStream(fName, FileMode.Create, FileAccess.ReadWrite);
                                    fileStream.Write(printAttachment.Value, 0, printAttachment.Value.Length);
                                    fileStream.Close();
                                    SendToPrinter(fName);
                                    success = true;
                                }
                            }
                            if (hasEmail) {
                                String mailBody = "Dear Owner" + Environment.NewLine + Environment.NewLine;
                                if (String.IsNullOrEmpty(txtEmail.Text)) {
                                    mailBody += "Attached please find correspondence for your attention." + Environment.NewLine + Environment.NewLine;
                                } else {
                                    mailBody += txtEmail.Text + Environment.NewLine + Environment.NewLine;
                                }

                                mailBody += "Account #: " + c.accNumber + ". For any queries on your account, please contact " + Controller.user.name + " on email: " + Controller.user.email + ", tel: " + Controller.user.phone + "." + Environment.NewLine + Environment.NewLine;
                                mailBody += "Regards" + Environment.NewLine;
                                mailBody += "Astrodon (Pty) Ltd" + Environment.NewLine;
                                mailBody += "You're in good hands";

                                String msgStatus;
                                String letterDir = (Directory.Exists("K:\\Debtors System\\Letters\\") ? "K:\\Debtors System\\Letters\\" : "C:\\Pastel11\\Debtors System\\Letters\\");

                                if (!delay) {
                                    if (!Mailer.SendMail(Controller.user.email, c.Email, txtSubject.Text, mailBody, false, false, true, out status, myAttachments)) {
                                        MessageBox.Show("Error sending mail to " + c.accNumber + ": " + status);
                                    } else {
                                        success = true;
                                    }
                                    foreach (KeyValuePair<String, byte[]> printAttachment in myAttachments) {
                                        String fileName = Path.Combine(letterDir, printAttachment.Key);
                                        File.WriteAllBytes(fileName, printAttachment.Value);
                                        String actFileTitle = Path.GetFileNameWithoutExtension(fileName);
                                        String actFile = Path.GetFileName(fileName);
                                        try {
                                            MySqlConnector mySqlConn = new MySqlConnector();
                                            mySqlConn.InsertStatement(actFileTitle, "Customer Statements", actFile, c.accNumber, c.Email);
                                            Classes.Sftp ftpClient = new Classes.Sftp(null, true);
                                            ftpClient.Upload(fileName, actFile, false);
                                        } catch { }
                                    }
                                } else {
                                    SqlDataHandler dh = new SqlDataHandler();
                                    List<String> attachments = new List<string>();
                                    foreach (KeyValuePair<String, byte[]> printAttachment in myAttachments) {
                                        String fileName = Path.Combine(letterDir, printAttachment.Key);
                                        File.WriteAllBytes(fileName, printAttachment.Value);
                                        attachments.Add(fileName);
                                    }

                                    if (attachments.Count > 0) {
                                        success = dh.InsertLetter(Controller.user.email, c.Email, txtSubject.Text + ": " + c.accNumber + " " + DateTime.Now.ToString(), mailBody, true, false, true, attachments.ToArray(), c.accNumber, out msgStatus);
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                //upload
            }
            return success;
        }

        private bool TestDocuments() {
            bool success = false;
            String status;
            PDF pdf = new PDF();
            if (cmbAction.SelectedItem.ToString() == "Send to Customer") {
                Dictionary<String, byte[]> fixedAttachments = new Dictionary<string, byte[]>();
                Dictionary<String, String> variableAttachments = new Dictionary<string, string>();
                List<byte[]> embedAttachments = new List<byte[]>();
                String attachmentQuery = "SELECT fileName, fileContent, fileString, fileType FROM tblJobAttachments WHERE jobID = " + jID;
                DataSet dsAttach = dm.GetData(attachmentQuery, null, out status);
                if (dsAttach != null && dsAttach.Tables.Count > 0 && dsAttach.Tables[0].Rows.Count > 0) {
                    for (int i = 0; i < dsAttach.Tables[0].Rows.Count; i++) {
                        int attType = (dgAttachments.Rows[i].Cells[6].Value != null ? int.Parse(dgAttachments.Rows[i].Cells[6].Value.ToString()) : 2);

                        DataRow dr = dsAttach.Tables[0].Rows[i];
                        String fileName = dr["fileName"].ToString();
                        String fileString = dr["fileString"].ToString();
                        int fileType = int.Parse(dr["fileType"].ToString());
                        byte[] file = fileType == 1 ? (byte[])dr["fileContent"] : null;
                        if (fileType == 1) {
                            if (attType != 1) {
                                if (attType == 2) {
                                    fixedAttachments.Add(fileName, file);
                                } else {
                                    embedAttachments.Add(file);
                                }
                            }
                        } else {
                            variableAttachments.Add(fileName, fileString);
                        }
                    }
                }

                Dictionary<String, byte[]> myAttachments = new Dictionary<string, byte[]>();
                foreach (KeyValuePair<String, byte[]> fileItem in fixedAttachments) {
                    if (!myAttachments.Keys.Contains(fileItem.Key)) { myAttachments.Add(fileItem.Key, fileItem.Value); }
                }
                foreach (KeyValuePair<String, String> vItem in variableAttachments) {
                    if (!myAttachments.Keys.Contains(vItem.Key)) {
                        byte[] fStream;
                        pdf.CreatePALetter(null, build, Controller.user, "RE: " + txtSubject.Text, DateTime.Now, vItem.Value, embedAttachments, out fStream);
                        OpenInAnotherApp(fStream, "WTF.pdf");
                        myAttachments.Add(vItem.Key + ".pdf", fStream);
                    }
                }
            } else {
                //upload
            }
            return success;
        }


        bool printerSet = false;
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);
        private void SendToPrinter(String fileName) { //C:\statement.pdf
            if (!printerSet) {
                frmPrintDialog printDialog = new frmPrintDialog();
                if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    SetDefaultPrinter(printDialog.selectedPrinter);
                    Properties.Settings.Default.defaultPrinter = printDialog.selectedPrinter;
                    Properties.Settings.Default.Save();
                    printerSet = true;
                }
            }

            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = fileName;
            Process p = new Process();
            p.StartInfo = info;
            p.Start();
            System.Threading.Thread.Sleep(5000);
        }
        private void btnRework_Click(object sender, EventArgs e) {
            String status;
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@notes", txtNotes.Text);

            String updateQuery = "UPDATE tblJob SET notes = @notes, currentStatus = 'REWORK' WHERE (id = " + jID.ToString() + ");";
            updateQuery += "INSERT INTO tblJobStatus(jobID, status, statusDate) VALUES(" + jID.ToString() + ", 'REWORK', getdate());";
            dm.SetData(updateQuery, sqlParms, out status);
            MessageBox.Show("Job has been submitted");
            ProcessMessage("changed job status to rework");
            Controller.mainF.ShowJobs();
        }

        private void ProcessMessage(String message) {
            String jobMessage = String.Empty;
            if (message != "added new job") {
                jobMessage = Controller.user.name + " " + message + ": Job ID - " + jID.ToString();
            } else {
                jobMessage = Controller.user.name + message + ": Job ID - " + jID.ToString();
            }
            Controller.commClient.SendMessage(jobMessage);
        }

        private void dgAttachments_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {

        }

        private void btnUpdateAttach_Click(object sender, EventArgs e) {
            UpdateAttachments();

        }
        private void UpdateAttachments() {
            for (int i = 0; i < dgAttachments.Rows.Count; i++) {
                String updateQuery = "UPDATE tblJobAttachments SET attType = " + dgAttachments.Rows[i].Cells[6].Value.ToString() + " WHERE id = " + dgAttachments.Rows[i].Cells[0].Value.ToString();
                String status;
                dm.SetData(updateQuery, null, out status);
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            UpdateAttachments();
            String status;
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            sqlParms.Add("@pm", Controller.user.id);
            sqlParms.Add("@buildID", build.ID);
            sqlParms.Add("@topic", txtTopic.Text);
            sqlParms.Add("@subject", txtSubject.Text);
            sqlParms.Add("@notes", txtNotes.Text);
            sqlParms.Add("@upload", cmbAction.SelectedItem.ToString() == "Upload to Building" ? true : false);
            sqlParms.Add("@email", cmbAction.SelectedItem.ToString() == "Send To Customer" ? true : false);
            sqlParms.Add("@body", txtEmail.Text);
            sqlParms.Add("@customerAction", (cmbCustomers.SelectedItem != null ? (cmbCustomers.SelectedItem.ToString() == "All customers" ? false : true) : false));

            String insertJobQuery = "INSERT INTO tblJob(creator, createDate, buildingID, topic, subject, notes, upload, email, currentStatus, emailBody, customerAction)";
            insertJobQuery += " VALUES(@pm, getdate(), @buildID, @topic, @subject, @notes, @upload, @email, 'NEW', @body, @customerAction);";
            insertJobQuery += " SELECT max(id) as jid FROM tblJob";
            DataSet dsJob = dm.GetData(insertJobQuery, sqlParms, out status);
            if (dsJob != null && dsJob.Tables.Count > 0 && dsJob.Tables[0].Rows.Count > 0) {
                jID = int.Parse(dsJob.Tables[0].Rows[0]["jid"].ToString());
                String updateAttachmentQuery = "UPDATE tblJobAttachments SET jobID = " + jID.ToString() + " WHERE jobID = 0";
                dm.SetData(updateAttachmentQuery, null, out status);
                for (int i = 0; i < chkCustomers.Items.Count; i++) {
                    if (chkCustomers.GetItemChecked(i)) {
                        String customerQuery = "INSERT INTO tblJobCustomer(jobID, customerID) VALUES(@jobID, @customer)";
                        sqlParms.Add("@jobID", jID);
                        sqlParms.Add("@customer", chkCustomers.Items[i].ToString());
                        dm.SetData(customerQuery, sqlParms, out status);
                    }
                }
                MessageBox.Show("Job has been saved");
                btnProcess.Enabled = true;
                btnSaveNewJob.Enabled = true;
                btnAddAttachment.Enabled = true;
                btnNewDoc.Enabled = true;

                btnUpdateAttach.Enabled = true;
                btnCreateTemplate.Enabled = true;
                lblNewMessage.Visible = false;
                LoadJob();
            }
        }

        private void txtSubject_TextChanged(object sender, EventArgs e) {
            btnNewDoc.Enabled = !String.IsNullOrEmpty(txtSubject.Text);
        }

        private void cmbCustomers_SelectedIndexChanged(object sender, EventArgs e) {
            if (cmbCustomers.SelectedItem != null) {
                if (cmbCustomers.SelectedItem.ToString() == "All customers") {
                    chkCustomers.Enabled = false;
                    for (int i = 0; i < chkCustomers.Items.Count; i++) { chkCustomers.SetItemChecked(i, true); }
                } else {
                    chkCustomers.Enabled = true;
                }
            }
        }

        private void btnView_Click(object sender, EventArgs e) {
            CreateTestDoc();
        }

        private void btnSpell_Click(object sender, EventArgs e) {
            spellCheck.Text = txtEmail.Text;
            spellCheck.SpellCheck();
        }






    }
}

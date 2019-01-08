namespace Astrodon {
    partial class usrBulkEmail {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgCustomers = new System.Windows.Forms.DataGridView();
            this.chkIncludeAll = new System.Windows.Forms.CheckBox();
            this.txtBCC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkBCC = new System.Windows.Forms.CheckBox();
            this.lstAttachments = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAttachment = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSendNow = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbBill = new System.Windows.Forms.ComboBox();
            this.txtBill = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkPriority = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.chkTrustees = new System.Windows.Forms.CheckBox();
            this.htmlMessage = new GvS.Controls.HtmlTextbox();
            ((System.ComponentModel.ISupportInitialize)(this.dgCustomers)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(111, 16);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(195, 21);
            this.cmbBuilding.TabIndex = 30;
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Select Building";
            // 
            // dgCustomers
            // 
            this.dgCustomers.AllowUserToAddRows = false;
            this.dgCustomers.AllowUserToDeleteRows = false;
            this.dgCustomers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCustomers.Location = new System.Drawing.Point(17, 93);
            this.dgCustomers.Name = "dgCustomers";
            this.dgCustomers.Size = new System.Drawing.Size(623, 183);
            this.dgCustomers.TabIndex = 31;
            // 
            // chkIncludeAll
            // 
            this.chkIncludeAll.AutoSize = true;
            this.chkIncludeAll.Location = new System.Drawing.Point(17, 70);
            this.chkIncludeAll.Name = "chkIncludeAll";
            this.chkIncludeAll.Size = new System.Drawing.Size(127, 17);
            this.chkIncludeAll.TabIndex = 32;
            this.chkIncludeAll.Text = "Include All Customers";
            this.chkIncludeAll.UseVisualStyleBackColor = true;
            this.chkIncludeAll.CheckedChanged += new System.EventHandler(this.chkIncludeAll_CheckedChanged);
            // 
            // txtBCC
            // 
            this.txtBCC.Location = new System.Drawing.Point(111, 287);
            this.txtBCC.Name = "txtBCC";
            this.txtBCC.Size = new System.Drawing.Size(195, 20);
            this.txtBCC.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 290);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "BCC:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Subject:";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(111, 315);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(529, 20);
            this.txtSubject.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 344);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Message:";
            // 
            // chkBCC
            // 
            this.chkBCC.AutoSize = true;
            this.chkBCC.Checked = true;
            this.chkBCC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBCC.Location = new System.Drawing.Point(312, 289);
            this.chkBCC.Name = "chkBCC";
            this.chkBCC.Size = new System.Drawing.Size(53, 17);
            this.chkBCC.TabIndex = 39;
            this.chkBCC.Text = "BCC?";
            this.chkBCC.UseVisualStyleBackColor = true;
            // 
            // lstAttachments
            // 
            this.lstAttachments.FormattingEnabled = true;
            this.lstAttachments.Location = new System.Drawing.Point(111, 498);
            this.lstAttachments.Name = "lstAttachments";
            this.lstAttachments.Size = new System.Drawing.Size(529, 95);
            this.lstAttachments.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 498);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Attachments:";
            // 
            // btnAttachment
            // 
            this.btnAttachment.Location = new System.Drawing.Point(16, 514);
            this.btnAttachment.Name = "btnAttachment";
            this.btnAttachment.Size = new System.Drawing.Size(75, 23);
            this.btnAttachment.TabIndex = 42;
            this.btnAttachment.Text = "Add";
            this.btnAttachment.UseVisualStyleBackColor = true;
            this.btnAttachment.Click += new System.EventHandler(this.btnAttachment_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(565, 599);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 44;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSendNow
            // 
            this.btnSendNow.Location = new System.Drawing.Point(484, 599);
            this.btnSendNow.Name = "btnSendNow";
            this.btnSendNow.Size = new System.Drawing.Size(75, 23);
            this.btnSendNow.TabIndex = 45;
            this.btnSendNow.Text = "Send Now";
            this.btnSendNow.UseVisualStyleBackColor = true;
            this.btnSendNow.Click += new System.EventHandler(this.btnSendNow_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 604);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Bill:";
            // 
            // cmbBill
            // 
            this.cmbBill.FormattingEnabled = true;
            this.cmbBill.Items.AddRange(new object[] {
            "None",
            "Building",
            "Customer"});
            this.cmbBill.Location = new System.Drawing.Point(111, 601);
            this.cmbBill.Name = "cmbBill";
            this.cmbBill.Size = new System.Drawing.Size(121, 21);
            this.cmbBill.TabIndex = 47;
            // 
            // txtBill
            // 
            this.txtBill.Location = new System.Drawing.Point(238, 602);
            this.txtBill.Name = "txtBill";
            this.txtBill.Size = new System.Drawing.Size(80, 20);
            this.txtBill.TabIndex = 48;
            this.txtBill.Text = "0.00";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(111, 43);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(195, 21);
            this.cmbCategory.TabIndex = 52;
            this.cmbCategory.Visible = false;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 51;
            this.label8.Text = "Category";
            this.label8.Visible = false;
            // 
            // chkPriority
            // 
            this.chkPriority.AutoSize = true;
            this.chkPriority.Location = new System.Drawing.Point(315, 47);
            this.chkPriority.Name = "chkPriority";
            this.chkPriority.Size = new System.Drawing.Size(97, 17);
            this.chkPriority.TabIndex = 53;
            this.chkPriority.Text = "Mark as urgent";
            this.chkPriority.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(16, 543);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 54;
            this.btnDelete.Text = "Remove";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // chkTrustees
            // 
            this.chkTrustees.AutoSize = true;
            this.chkTrustees.Location = new System.Drawing.Point(150, 70);
            this.chkTrustees.Name = "chkTrustees";
            this.chkTrustees.Size = new System.Drawing.Size(89, 17);
            this.chkTrustees.TabIndex = 55;
            this.chkTrustees.Text = "Trustees only";
            this.chkTrustees.UseVisualStyleBackColor = true;
            this.chkTrustees.CheckedChanged += new System.EventHandler(this.chkTrustees_CheckedChanged);
            // 
            // htmlMessage
            // 
            this.htmlMessage.Fonts = new string[] {
        "Corbel",
        "Corbel, Verdana, Arial, Helvetica, sans-serif",
        "Georgia, Times New Roman, Times, serif",
        "Consolas, Courier New, Courier, monospace"};
            this.htmlMessage.IllegalPatterns = new string[] {
        "<script.*?>",
        "<\\w+\\s+.*?(j|java|vb|ecma)script:.*?>",
        "<\\w+(\\s+|\\s+.*?\\s+)on\\w+\\s*=.+?>",
        "</?input.*?>"};
            this.htmlMessage.Location = new System.Drawing.Point(111, 344);
            this.htmlMessage.Name = "htmlMessage";
            this.htmlMessage.Padding = new System.Windows.Forms.Padding(1);
            this.htmlMessage.ShowHtmlSource = false;
            this.htmlMessage.Size = new System.Drawing.Size(529, 148);
            this.htmlMessage.TabIndex = 56;
            this.htmlMessage.ToolbarStyle = GvS.Controls.ToolbarStyles.AlwaysInternal;
            // 
            // usrBulkEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htmlMessage);
            this.Controls.Add(this.chkTrustees);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.chkPriority);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtBill);
            this.Controls.Add(this.cmbBill);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSendNow);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAttachment);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lstAttachments);
            this.Controls.Add(this.chkBCC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBCC);
            this.Controls.Add(this.chkIncludeAll);
            this.Controls.Add(this.dgCustomers);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label1);
            this.Name = "usrBulkEmail";
            this.Size = new System.Drawing.Size(655, 657);
            this.Load += new System.EventHandler(this.usrBulkEmail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgCustomers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgCustomers;
        private System.Windows.Forms.CheckBox chkIncludeAll;
        private System.Windows.Forms.TextBox txtBCC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkBCC;
        private System.Windows.Forms.ListBox lstAttachments;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAttachment;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSendNow;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbBill;
        private System.Windows.Forms.TextBox txtBill;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkPriority;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkTrustees;
        private GvS.Controls.HtmlTextbox htmlMessage;
    }
}

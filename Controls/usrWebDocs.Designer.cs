namespace Astrodon.Controls {
    partial class usrWebDocs {
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
            this.trvWeb = new System.Windows.Forms.TreeView();
            this.picWeb = new System.Windows.Forms.PictureBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblBuildings = new System.Windows.Forms.Label();
            this.chkImage = new System.Windows.Forms.CheckBox();
            this.chkRules = new System.Windows.Forms.CheckBox();
            this.chkInsurance = new System.Windows.Forms.CheckBox();
            this.chkMinutes = new System.Windows.Forms.CheckBox();
            this.chkNotices = new System.Windows.Forms.CheckBox();
            this.chkPlans = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkFPlans = new System.Windows.Forms.CheckBox();
            this.chkFNotices = new System.Windows.Forms.CheckBox();
            this.chkFMinutes = new System.Windows.Forms.CheckBox();
            this.chkFInsurance = new System.Windows.Forms.CheckBox();
            this.chkFConduct = new System.Windows.Forms.CheckBox();
            this.chkFFinancial = new System.Windows.Forms.CheckBox();
            this.chkOther = new System.Windows.Forms.CheckBox();
            this.chkFinance = new System.Windows.Forms.CheckBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtNewFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.fileList = new System.Windows.Forms.CheckedListBox();
            this.cmbFolder = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picWeb)).BeginInit();
            this.SuspendLayout();
            // 
            // trvWeb
            // 
            this.trvWeb.Location = new System.Drawing.Point(18, 42);
            this.trvWeb.Name = "trvWeb";
            this.trvWeb.Size = new System.Drawing.Size(417, 601);
            this.trvWeb.TabIndex = 0;
            this.trvWeb.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvWeb_NodeMouseClick);
            // 
            // picWeb
            // 
            this.picWeb.Location = new System.Drawing.Point(441, 191);
            this.picWeb.Name = "picWeb";
            this.picWeb.Size = new System.Drawing.Size(280, 200);
            this.picWeb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picWeb.TabIndex = 1;
            this.picWeb.TabStop = false;
            this.picWeb.Click += new System.EventHandler(this.picImage_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(441, 620);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblBuildings
            // 
            this.lblBuildings.AutoSize = true;
            this.lblBuildings.Location = new System.Drawing.Point(15, 16);
            this.lblBuildings.Name = "lblBuildings";
            this.lblBuildings.Size = new System.Drawing.Size(49, 13);
            this.lblBuildings.TabIndex = 3;
            this.lblBuildings.Text = "Buildings";
            // 
            // chkImage
            // 
            this.chkImage.AutoSize = true;
            this.chkImage.Location = new System.Drawing.Point(441, 442);
            this.chkImage.Name = "chkImage";
            this.chkImage.Size = new System.Drawing.Size(71, 17);
            this.chkImage.TabIndex = 4;
            this.chkImage.Text = "No image";
            this.chkImage.UseVisualStyleBackColor = true;
            // 
            // chkRules
            // 
            this.chkRules.AutoSize = true;
            this.chkRules.Location = new System.Drawing.Point(441, 465);
            this.chkRules.Name = "chkRules";
            this.chkRules.Size = new System.Drawing.Size(107, 17);
            this.chkRules.TabIndex = 5;
            this.chkRules.Text = "No conduct rules";
            this.chkRules.UseVisualStyleBackColor = true;
            // 
            // chkInsurance
            // 
            this.chkInsurance.AutoSize = true;
            this.chkInsurance.Location = new System.Drawing.Point(441, 488);
            this.chkInsurance.Name = "chkInsurance";
            this.chkInsurance.Size = new System.Drawing.Size(143, 17);
            this.chkInsurance.TabIndex = 6;
            this.chkInsurance.Text = "No insurance information";
            this.chkInsurance.UseVisualStyleBackColor = true;
            // 
            // chkMinutes
            // 
            this.chkMinutes.AutoSize = true;
            this.chkMinutes.Location = new System.Drawing.Point(441, 511);
            this.chkMinutes.Name = "chkMinutes";
            this.chkMinutes.Size = new System.Drawing.Size(119, 17);
            this.chkMinutes.TabIndex = 7;
            this.chkMinutes.Text = "No meeting minutes";
            this.chkMinutes.UseVisualStyleBackColor = true;
            // 
            // chkNotices
            // 
            this.chkNotices.AutoSize = true;
            this.chkNotices.Location = new System.Drawing.Point(441, 534);
            this.chkNotices.Name = "chkNotices";
            this.chkNotices.Size = new System.Drawing.Size(117, 17);
            this.chkNotices.TabIndex = 8;
            this.chkNotices.Text = "No meeting notices";
            this.chkNotices.UseVisualStyleBackColor = true;
            // 
            // chkPlans
            // 
            this.chkPlans.AutoSize = true;
            this.chkPlans.Location = new System.Drawing.Point(441, 557);
            this.chkPlans.Name = "chkPlans";
            this.chkPlans.Size = new System.Drawing.Size(68, 17);
            this.chkPlans.TabIndex = 9;
            this.chkPlans.Text = "No plans";
            this.chkPlans.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 426);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Filter:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(620, 426);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Show:";
            // 
            // chkFPlans
            // 
            this.chkFPlans.AutoSize = true;
            this.chkFPlans.Location = new System.Drawing.Point(620, 557);
            this.chkFPlans.Name = "chkFPlans";
            this.chkFPlans.Size = new System.Drawing.Size(52, 17);
            this.chkFPlans.TabIndex = 16;
            this.chkFPlans.Text = "Plans";
            this.chkFPlans.UseVisualStyleBackColor = true;
            // 
            // chkFNotices
            // 
            this.chkFNotices.AutoSize = true;
            this.chkFNotices.Location = new System.Drawing.Point(620, 534);
            this.chkFNotices.Name = "chkFNotices";
            this.chkFNotices.Size = new System.Drawing.Size(103, 17);
            this.chkFNotices.TabIndex = 15;
            this.chkFNotices.Text = "Meeting Notices";
            this.chkFNotices.UseVisualStyleBackColor = true;
            // 
            // chkFMinutes
            // 
            this.chkFMinutes.AutoSize = true;
            this.chkFMinutes.Location = new System.Drawing.Point(620, 511);
            this.chkFMinutes.Name = "chkFMinutes";
            this.chkFMinutes.Size = new System.Drawing.Size(104, 17);
            this.chkFMinutes.TabIndex = 14;
            this.chkFMinutes.Text = "Meeting Minutes";
            this.chkFMinutes.UseVisualStyleBackColor = true;
            // 
            // chkFInsurance
            // 
            this.chkFInsurance.AutoSize = true;
            this.chkFInsurance.Location = new System.Drawing.Point(620, 488);
            this.chkFInsurance.Name = "chkFInsurance";
            this.chkFInsurance.Size = new System.Drawing.Size(128, 17);
            this.chkFInsurance.TabIndex = 13;
            this.chkFInsurance.Text = "Insurance Information";
            this.chkFInsurance.UseVisualStyleBackColor = true;
            // 
            // chkFConduct
            // 
            this.chkFConduct.AutoSize = true;
            this.chkFConduct.Location = new System.Drawing.Point(620, 465);
            this.chkFConduct.Name = "chkFConduct";
            this.chkFConduct.Size = new System.Drawing.Size(96, 17);
            this.chkFConduct.TabIndex = 12;
            this.chkFConduct.Text = "Conduct Rules";
            this.chkFConduct.UseVisualStyleBackColor = true;
            // 
            // chkFFinancial
            // 
            this.chkFFinancial.AutoSize = true;
            this.chkFFinancial.Location = new System.Drawing.Point(620, 442);
            this.chkFFinancial.Name = "chkFFinancial";
            this.chkFFinancial.Size = new System.Drawing.Size(124, 17);
            this.chkFFinancial.TabIndex = 11;
            this.chkFFinancial.Text = "Financial Statements";
            this.chkFFinancial.UseVisualStyleBackColor = true;
            // 
            // chkOther
            // 
            this.chkOther.AutoSize = true;
            this.chkOther.Location = new System.Drawing.Point(620, 580);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new System.Drawing.Size(73, 17);
            this.chkOther.TabIndex = 18;
            this.chkOther.Text = "Other files";
            this.chkOther.UseVisualStyleBackColor = true;
            // 
            // chkFinance
            // 
            this.chkFinance.AutoSize = true;
            this.chkFinance.Location = new System.Drawing.Point(441, 580);
            this.chkFinance.Name = "chkFinance";
            this.chkFinance.Size = new System.Drawing.Size(87, 17);
            this.chkFinance.TabIndex = 19;
            this.chkFinance.Text = "No financials";
            this.chkFinance.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(805, 73);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(125, 23);
            this.btnCreate.TabIndex = 30;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtNewFolder
            // 
            this.txtNewFolder.Location = new System.Drawing.Point(570, 73);
            this.txtNewFolder.Name = "txtNewFolder";
            this.txtNewFolder.Size = new System.Drawing.Size(229, 20);
            this.txtNewFolder.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(443, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "or create New Folder";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(646, 397);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Remove";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(565, 397);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 26;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(805, 99);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(127, 23);
            this.btnAdd.TabIndex = 24;
            this.btnAdd.Text = "Add new file";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(805, 128);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(125, 23);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "Delete Selected";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // fileList
            // 
            this.fileList.CheckOnClick = true;
            this.fileList.FormattingEnabled = true;
            this.fileList.Location = new System.Drawing.Point(441, 99);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(358, 79);
            this.fileList.TabIndex = 22;
            this.fileList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.fileList_MouseDoubleClick);
            // 
            // cmbFolder
            // 
            this.cmbFolder.FormattingEnabled = true;
            this.cmbFolder.Location = new System.Drawing.Point(570, 41);
            this.cmbFolder.Name = "cmbFolder";
            this.cmbFolder.Size = new System.Drawing.Size(229, 21);
            this.cmbFolder.TabIndex = 21;
            this.cmbFolder.SelectedIndexChanged += new System.EventHandler(this.cmbFolder_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(443, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Please select folder:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "Building folders",
            "Trustee folders"});
            this.cmbCategory.Location = new System.Drawing.Point(570, 16);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(229, 21);
            this.cmbCategory.TabIndex = 32;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(443, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Please select category:";
            // 
            // usrWebDocs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtNewFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.fileList);
            this.Controls.Add(this.cmbFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkFinance);
            this.Controls.Add(this.chkOther);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkFPlans);
            this.Controls.Add(this.chkFNotices);
            this.Controls.Add(this.chkFMinutes);
            this.Controls.Add(this.chkFInsurance);
            this.Controls.Add(this.chkFConduct);
            this.Controls.Add(this.chkFFinancial);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkPlans);
            this.Controls.Add(this.chkNotices);
            this.Controls.Add(this.chkMinutes);
            this.Controls.Add(this.chkInsurance);
            this.Controls.Add(this.chkRules);
            this.Controls.Add(this.chkImage);
            this.Controls.Add(this.lblBuildings);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.picWeb);
            this.Controls.Add(this.trvWeb);
            this.Name = "usrWebDocs";
            this.Size = new System.Drawing.Size(948, 660);
            this.Load += new System.EventHandler(this.usrWebDocs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picWeb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView trvWeb;
        private System.Windows.Forms.PictureBox picWeb;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblBuildings;
        private System.Windows.Forms.CheckBox chkImage;
        private System.Windows.Forms.CheckBox chkRules;
        private System.Windows.Forms.CheckBox chkInsurance;
        private System.Windows.Forms.CheckBox chkMinutes;
        private System.Windows.Forms.CheckBox chkNotices;
        private System.Windows.Forms.CheckBox chkPlans;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkFPlans;
        private System.Windows.Forms.CheckBox chkFNotices;
        private System.Windows.Forms.CheckBox chkFMinutes;
        private System.Windows.Forms.CheckBox chkFInsurance;
        private System.Windows.Forms.CheckBox chkFConduct;
        private System.Windows.Forms.CheckBox chkFFinancial;
        private System.Windows.Forms.CheckBox chkOther;
        private System.Windows.Forms.CheckBox chkFinance;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtNewFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckedListBox fileList;
        private System.Windows.Forms.ComboBox cmbFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label5;
    }
}

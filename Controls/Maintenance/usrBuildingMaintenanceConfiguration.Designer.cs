namespace Astrodon.Controls.Maintenance
{
    partial class usrBuildingMaintenanceConfiguration
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbAccount = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbClassification = new System.Windows.Forms.GroupBox();
            this.rbInsurance = new System.Windows.Forms.RadioButton();
            this.rbProject = new System.Windows.Forms.RadioButton();
            this.rbMaintenancePlan = new System.Windows.Forms.RadioButton();
            this.rbRemedialMaintenance = new System.Windows.Forms.RadioButton();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgItems = new System.Windows.Forms.DataGridView();
            this.gbClassification.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(102, 14);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(233, 21);
            this.cmbBuilding.TabIndex = 9;
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Select Building";
            // 
            // cmbAccount
            // 
            this.cmbAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccount.Enabled = false;
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new System.Drawing.Point(102, 50);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new System.Drawing.Size(233, 21);
            this.cmbAccount.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Select Account";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(102, 83);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(233, 20);
            this.tbName.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Classification";
            // 
            // gbClassification
            // 
            this.gbClassification.Controls.Add(this.rbInsurance);
            this.gbClassification.Controls.Add(this.rbProject);
            this.gbClassification.Controls.Add(this.rbMaintenancePlan);
            this.gbClassification.Controls.Add(this.rbRemedialMaintenance);
            this.gbClassification.Enabled = false;
            this.gbClassification.Location = new System.Drawing.Point(102, 120);
            this.gbClassification.Name = "gbClassification";
            this.gbClassification.Size = new System.Drawing.Size(233, 131);
            this.gbClassification.TabIndex = 15;
            this.gbClassification.TabStop = false;
            // 
            // rbInsurance
            // 
            this.rbInsurance.AutoSize = true;
            this.rbInsurance.Location = new System.Drawing.Point(7, 92);
            this.rbInsurance.Name = "rbInsurance";
            this.rbInsurance.Size = new System.Drawing.Size(72, 17);
            this.rbInsurance.TabIndex = 3;
            this.rbInsurance.TabStop = true;
            this.rbInsurance.Text = "Insurance";
            this.rbInsurance.UseVisualStyleBackColor = true;
            // 
            // rbProject
            // 
            this.rbProject.AutoSize = true;
            this.rbProject.Location = new System.Drawing.Point(7, 68);
            this.rbProject.Name = "rbProject";
            this.rbProject.Size = new System.Drawing.Size(58, 17);
            this.rbProject.TabIndex = 2;
            this.rbProject.TabStop = true;
            this.rbProject.Text = "Project";
            this.rbProject.UseVisualStyleBackColor = true;
            // 
            // rbMaintenancePlan
            // 
            this.rbMaintenancePlan.AutoSize = true;
            this.rbMaintenancePlan.Location = new System.Drawing.Point(7, 44);
            this.rbMaintenancePlan.Name = "rbMaintenancePlan";
            this.rbMaintenancePlan.Size = new System.Drawing.Size(111, 17);
            this.rbMaintenancePlan.TabIndex = 1;
            this.rbMaintenancePlan.TabStop = true;
            this.rbMaintenancePlan.Text = "Maintenance Plan";
            this.rbMaintenancePlan.UseVisualStyleBackColor = true;
            // 
            // rbRemedialMaintenance
            // 
            this.rbRemedialMaintenance.AutoSize = true;
            this.rbRemedialMaintenance.Location = new System.Drawing.Point(7, 20);
            this.rbRemedialMaintenance.Name = "rbRemedialMaintenance";
            this.rbRemedialMaintenance.Size = new System.Drawing.Size(134, 17);
            this.rbRemedialMaintenance.TabIndex = 0;
            this.rbRemedialMaintenance.TabStop = true;
            this.rbRemedialMaintenance.Text = "Remedial Maintenance";
            this.rbRemedialMaintenance.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.AutoEllipsis = true;
            this.btnNew.Location = new System.Drawing.Point(10, 265);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 17;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(260, 265);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(179, 265);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgItems
            // 
            this.dgItems.AllowUserToAddRows = false;
            this.dgItems.AllowUserToDeleteRows = false;
            this.dgItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItems.Location = new System.Drawing.Point(10, 306);
            this.dgItems.Name = "dgItems";
            this.dgItems.ReadOnly = true;
            this.dgItems.Size = new System.Drawing.Size(582, 150);
            this.dgItems.TabIndex = 20;
            this.dgItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgItems_CellContentClick);
            // 
            // usrBuildingMaintenanceConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgItems);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.gbClassification);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbAccount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label3);
            this.Name = "usrBuildingMaintenanceConfiguration";
            this.Size = new System.Drawing.Size(601, 473);
            this.gbClassification.ResumeLayout(false);
            this.gbClassification.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbClassification;
        private System.Windows.Forms.RadioButton rbInsurance;
        private System.Windows.Forms.RadioButton rbProject;
        private System.Windows.Forms.RadioButton rbMaintenancePlan;
        private System.Windows.Forms.RadioButton rbRemedialMaintenance;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgItems;
    }
}

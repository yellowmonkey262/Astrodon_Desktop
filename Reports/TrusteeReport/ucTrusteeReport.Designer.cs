namespace Astrodon.Reports.TrusteeReport
{
    partial class ucTrusteeReport
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
            this.dgvTrusteeReport = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblBuilding = new System.Windows.Forms.Label();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BuildingName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Portfolio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CellNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmailAddress1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrusteeReport)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTrusteeReport
            // 
            this.dgvTrusteeReport.AllowUserToAddRows = false;
            this.dgvTrusteeReport.AllowUserToDeleteRows = false;
            this.dgvTrusteeReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrusteeReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.BuildingName,
            this.Portfolio,
            this.AccountNumber,
            this.CustomerFullName,
            this.CellNumber,
            this.EmailAddress1});
            this.dgvTrusteeReport.Location = new System.Drawing.Point(3, 37);
            this.dgvTrusteeReport.Name = "dgvTrusteeReport";
            this.dgvTrusteeReport.ReadOnly = true;
            this.dgvTrusteeReport.Size = new System.Drawing.Size(966, 549);
            this.dgvTrusteeReport.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(849, 592);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 26);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblBuilding
            // 
            this.lblBuilding.AutoSize = true;
            this.lblBuilding.Location = new System.Drawing.Point(3, 13);
            this.lblBuilding.Name = "lblBuilding";
            this.lblBuilding.Size = new System.Drawing.Size(80, 13);
            this.lblBuilding.TabIndex = 2;
            this.lblBuilding.Text = "Select Building:";
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(89, 10);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(213, 21);
            this.cmbBuilding.TabIndex = 3;
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // Code
            // 
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // BuildingName
            // 
            this.BuildingName.HeaderText = "Building Name";
            this.BuildingName.Name = "BuildingName";
            this.BuildingName.ReadOnly = true;
            // 
            // Portfolio
            // 
            this.Portfolio.HeaderText = "Portfolio";
            this.Portfolio.Name = "Portfolio";
            this.Portfolio.ReadOnly = true;
            // 
            // AccountNumber
            // 
            this.AccountNumber.HeaderText = "Account Number";
            this.AccountNumber.Name = "AccountNumber";
            this.AccountNumber.ReadOnly = true;
            // 
            // CustomerFullName
            // 
            this.CustomerFullName.HeaderText = "Customer Full Name";
            this.CustomerFullName.Name = "CustomerFullName";
            this.CustomerFullName.ReadOnly = true;
            // 
            // CellNumber
            // 
            this.CellNumber.HeaderText = "Cell Number";
            this.CellNumber.Name = "CellNumber";
            this.CellNumber.ReadOnly = true;
            // 
            // EmailAddress1
            // 
            this.EmailAddress1.HeaderText = "Email Address";
            this.EmailAddress1.Name = "EmailAddress1";
            this.EmailAddress1.ReadOnly = true;
            // 
            // ucTrusteeReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.lblBuilding);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgvTrusteeReport);
            this.Name = "ucTrusteeReport";
            this.Size = new System.Drawing.Size(972, 623);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrusteeReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTrusteeReport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblBuilding;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn BuildingName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Portfolio;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CellNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmailAddress1;
    }
}

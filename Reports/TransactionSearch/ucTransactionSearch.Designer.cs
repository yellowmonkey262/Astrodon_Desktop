namespace Astrodon.Reports.TransactionSearch
{
    partial class ucTransactionSearch
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
            this.lblBuildingTranactionSearch = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblMinAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReferenceContains = new System.Windows.Forms.Label();
            this.lblDescriptionContains = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblMaxAmount = new System.Windows.Forms.Label();
            this.tbMinAmount = new System.Windows.Forms.TextBox();
            this.tbMaxAmount = new System.Windows.Forms.TextBox();
            this.tbReferenceContains = new System.Windows.Forms.TextBox();
            this.tbDescriptionContains = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.dgvSearchResults = new System.Windows.Forms.DataGridView();
            this.buildingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linkAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSearchStatus = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBuildingTranactionSearch
            // 
            this.lblBuildingTranactionSearch.AutoSize = true;
            this.lblBuildingTranactionSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuildingTranactionSearch.Location = new System.Drawing.Point(12, 13);
            this.lblBuildingTranactionSearch.Name = "lblBuildingTranactionSearch";
            this.lblBuildingTranactionSearch.Size = new System.Drawing.Size(234, 20);
            this.lblBuildingTranactionSearch.TabIndex = 0;
            this.lblBuildingTranactionSearch.Text = "Building Transaction Search";
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(13, 55);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(60, 13);
            this.lblFromDate.TabIndex = 1;
            this.lblFromDate.Text = "From Date*";
            // 
            // lblMinAmount
            // 
            this.lblMinAmount.AutoSize = true;
            this.lblMinAmount.Location = new System.Drawing.Point(13, 82);
            this.lblMinAmount.Name = "lblMinAmount";
            this.lblMinAmount.Size = new System.Drawing.Size(66, 13);
            this.lblMinAmount.TabIndex = 2;
            this.lblMinAmount.Text = "Min Amount:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-67, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // lblReferenceContains
            // 
            this.lblReferenceContains.AutoSize = true;
            this.lblReferenceContains.Location = new System.Drawing.Point(13, 118);
            this.lblReferenceContains.Name = "lblReferenceContains";
            this.lblReferenceContains.Size = new System.Drawing.Size(104, 13);
            this.lblReferenceContains.TabIndex = 4;
            this.lblReferenceContains.Text = "Reference Contains:";
            // 
            // lblDescriptionContains
            // 
            this.lblDescriptionContains.AutoSize = true;
            this.lblDescriptionContains.Location = new System.Drawing.Point(13, 144);
            this.lblDescriptionContains.Name = "lblDescriptionContains";
            this.lblDescriptionContains.Size = new System.Drawing.Size(107, 13);
            this.lblDescriptionContains.TabIndex = 5;
            this.lblDescriptionContains.Text = "Description Contains:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(85, 49);
            this.dtpFromDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(123, 20);
            this.dtpFromDate.TabIndex = 6;
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(227, 54);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(50, 13);
            this.lblToDate.TabIndex = 7;
            this.lblToDate.Text = "To Date*";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(302, 49);
            this.dtpToDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(129, 20);
            this.dtpToDate.TabIndex = 8;
            // 
            // lblMaxAmount
            // 
            this.lblMaxAmount.AutoSize = true;
            this.lblMaxAmount.Location = new System.Drawing.Point(227, 82);
            this.lblMaxAmount.Name = "lblMaxAmount";
            this.lblMaxAmount.Size = new System.Drawing.Size(69, 13);
            this.lblMaxAmount.TabIndex = 9;
            this.lblMaxAmount.Text = "Max Amount:";
            // 
            // tbMinAmount
            // 
            this.tbMinAmount.Location = new System.Drawing.Point(85, 79);
            this.tbMinAmount.Name = "tbMinAmount";
            this.tbMinAmount.Size = new System.Drawing.Size(123, 20);
            this.tbMinAmount.TabIndex = 10;
            // 
            // tbMaxAmount
            // 
            this.tbMaxAmount.Location = new System.Drawing.Point(302, 79);
            this.tbMaxAmount.Name = "tbMaxAmount";
            this.tbMaxAmount.Size = new System.Drawing.Size(129, 20);
            this.tbMaxAmount.TabIndex = 11;
            // 
            // tbReferenceContains
            // 
            this.tbReferenceContains.Location = new System.Drawing.Point(123, 115);
            this.tbReferenceContains.Name = "tbReferenceContains";
            this.tbReferenceContains.Size = new System.Drawing.Size(188, 20);
            this.tbReferenceContains.TabIndex = 12;
            // 
            // tbDescriptionContains
            // 
            this.tbDescriptionContains.Location = new System.Drawing.Point(123, 141);
            this.tbDescriptionContains.Name = "tbDescriptionContains";
            this.tbDescriptionContains.Size = new System.Drawing.Size(188, 20);
            this.tbDescriptionContains.TabIndex = 13;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(123, 177);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(85, 23);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(230, 177);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(81, 23);
            this.btnStop.TabIndex = 15;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // dgvSearchResults
            // 
            this.dgvSearchResults.AllowUserToAddRows = false;
            this.dgvSearchResults.AllowUserToDeleteRows = false;
            this.dgvSearchResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSearchResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.buildingID,
            this.transactionDate,
            this.account,
            this.linkAccount,
            this.reference,
            this.description,
            this.amount});
            this.dgvSearchResults.Location = new System.Drawing.Point(16, 243);
            this.dgvSearchResults.Name = "dgvSearchResults";
            this.dgvSearchResults.ReadOnly = true;
            this.dgvSearchResults.Size = new System.Drawing.Size(964, 430);
            this.dgvSearchResults.TabIndex = 17;
            // 
            // buildingID
            // 
            this.buildingID.HeaderText = "Building";
            this.buildingID.Name = "buildingID";
            this.buildingID.ReadOnly = true;
            // 
            // transactionDate
            // 
            this.transactionDate.HeaderText = "Transaction Date";
            this.transactionDate.Name = "transactionDate";
            this.transactionDate.ReadOnly = true;
            // 
            // account
            // 
            this.account.HeaderText = "Account";
            this.account.Name = "account";
            this.account.ReadOnly = true;
            // 
            // linkAccount
            // 
            this.linkAccount.HeaderText = "Link Account";
            this.linkAccount.Name = "linkAccount";
            this.linkAccount.ReadOnly = true;
            // 
            // reference
            // 
            this.reference.HeaderText = "Reference";
            this.reference.Name = "reference";
            this.reference.ReadOnly = true;
            // 
            // description
            // 
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            // 
            // amount
            // 
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            this.amount.ReadOnly = true;
            // 
            // lblSearchStatus
            // 
            this.lblSearchStatus.AutoSize = true;
            this.lblSearchStatus.Location = new System.Drawing.Point(13, 227);
            this.lblSearchStatus.Name = "lblSearchStatus";
            this.lblSearchStatus.Size = new System.Drawing.Size(10, 13);
            this.lblSearchStatus.TabIndex = 18;
            this.lblSearchStatus.Text = " ";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(865, 679);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(115, 23);
            this.btnExport.TabIndex = 19;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ucTransactionSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblSearchStatus);
            this.Controls.Add(this.dgvSearchResults);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbDescriptionContains);
            this.Controls.Add(this.tbReferenceContains);
            this.Controls.Add(this.tbMaxAmount);
            this.Controls.Add(this.tbMinAmount);
            this.Controls.Add(this.lblMaxAmount);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.lblDescriptionContains);
            this.Controls.Add(this.lblReferenceContains);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMinAmount);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.lblBuildingTranactionSearch);
            this.Name = "ucTransactionSearch";
            this.Size = new System.Drawing.Size(993, 712);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBuildingTranactionSearch;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblMinAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReferenceContains;
        private System.Windows.Forms.Label lblDescriptionContains;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblMaxAmount;
        private System.Windows.Forms.TextBox tbMinAmount;
        private System.Windows.Forms.TextBox tbMaxAmount;
        private System.Windows.Forms.TextBox tbReferenceContains;
        private System.Windows.Forms.TextBox tbDescriptionContains;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.DataGridView dgvSearchResults;
        private System.Windows.Forms.Label lblSearchStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn buildingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn account;
        private System.Windows.Forms.DataGridViewTextBoxColumn linkAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn reference;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.Button btnExport;
    }
}

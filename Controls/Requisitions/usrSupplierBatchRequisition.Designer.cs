namespace Astrodon.Controls.Requisitions
{
    partial class usrSupplierBatchRequisition
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDefaultDate = new System.Windows.Forms.Button();
            this.btnDefaultLedger = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbSupplierBankAccount = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSupplierLookup = new System.Windows.Forms.Button();
            this.lbSupplierName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.trnDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbLedger = new System.Windows.Forms.ComboBox();
            this.dgItems = new System.Windows.Forms.DataGridView();
            this.ofdAttachment = new System.Windows.Forms.OpenFileDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 52;
            this.label1.Text = "Supplier Bulk Payments";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDefaultDate);
            this.panel1.Controls.Add(this.btnDefaultLedger);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.cbSupplierBankAccount);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtInvoiceDate);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.btnSupplierLookup);
            this.panel1.Controls.Add(this.lbSupplierName);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.trnDatePicker);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.cmbLedger);
            this.panel1.Location = new System.Drawing.Point(9, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(621, 165);
            this.panel1.TabIndex = 53;
            // 
            // btnDefaultDate
            // 
            this.btnDefaultDate.Location = new System.Drawing.Point(456, 104);
            this.btnDefaultDate.Name = "btnDefaultDate";
            this.btnDefaultDate.Size = new System.Drawing.Size(106, 23);
            this.btnDefaultDate.TabIndex = 66;
            this.btnDefaultDate.Text = "Set Default";
            this.btnDefaultDate.UseVisualStyleBackColor = true;
            this.btnDefaultDate.Click += new System.EventHandler(this.btnDefaultDate_Click);
            // 
            // btnDefaultLedger
            // 
            this.btnDefaultLedger.Location = new System.Drawing.Point(456, 77);
            this.btnDefaultLedger.Name = "btnDefaultLedger";
            this.btnDefaultLedger.Size = new System.Drawing.Size(106, 23);
            this.btnDefaultLedger.TabIndex = 65;
            this.btnDefaultLedger.Text = "Set Default";
            this.btnDefaultLedger.UseVisualStyleBackColor = true;
            this.btnDefaultLedger.Click += new System.EventHandler(this.btnDefaultLedger_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(455, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 23);
            this.button1.TabIndex = 64;
            this.button1.Text = "Set as default";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbSupplierBankAccount
            // 
            this.cbSupplierBankAccount.FormattingEnabled = true;
            this.cbSupplierBankAccount.Location = new System.Drawing.Point(156, 134);
            this.cbSupplierBankAccount.Name = "cbSupplierBankAccount";
            this.cbSupplierBankAccount.Size = new System.Drawing.Size(293, 21);
            this.cbSupplierBankAccount.TabIndex = 63;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Default Bank Account";
            // 
            // dtInvoiceDate
            // 
            this.dtInvoiceDate.CustomFormat = "yyyy/MM/dd";
            this.dtInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtInvoiceDate.Location = new System.Drawing.Point(156, 108);
            this.dtInvoiceDate.Name = "dtInvoiceDate";
            this.dtInvoiceDate.Size = new System.Drawing.Size(204, 20);
            this.dtInvoiceDate.TabIndex = 4;
            this.dtInvoiceDate.ValueChanged += new System.EventHandler(this.dtInvoiceDate_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 114);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(105, 13);
            this.label14.TabIndex = 61;
            this.label14.Text = "Default Invoice Date";
            // 
            // btnSupplierLookup
            // 
            this.btnSupplierLookup.Location = new System.Drawing.Point(374, 46);
            this.btnSupplierLookup.Name = "btnSupplierLookup";
            this.btnSupplierLookup.Size = new System.Drawing.Size(75, 23);
            this.btnSupplierLookup.TabIndex = 1;
            this.btnSupplierLookup.Text = "Find";
            this.btnSupplierLookup.UseVisualStyleBackColor = true;
            this.btnSupplierLookup.Click += new System.EventHandler(this.btnSupplierLookup_Click);
            // 
            // lbSupplierName
            // 
            this.lbSupplierName.AutoSize = true;
            this.lbSupplierName.Location = new System.Drawing.Point(156, 46);
            this.lbSupplierName.Name = "lbSupplierName";
            this.lbSupplierName.Size = new System.Drawing.Size(92, 13);
            this.lbSupplierName.TabIndex = 60;
            this.lbSupplierName.Text = "-- none selected --";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 13);
            this.label12.TabIndex = 59;
            this.label12.Text = "Select Supplier";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 58;
            this.label10.Text = "Select Date";
            // 
            // trnDatePicker
            // 
            this.trnDatePicker.CustomFormat = "yyyy/MM/dd";
            this.trnDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.trnDatePicker.Location = new System.Drawing.Point(156, 18);
            this.trnDatePicker.Name = "trnDatePicker";
            this.trnDatePicker.Size = new System.Drawing.Size(206, 20);
            this.trnDatePicker.TabIndex = 0;
            this.trnDatePicker.Tag = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 13);
            this.label9.TabIndex = 57;
            this.label9.Text = "Default Ledger Account";
            // 
            // cmbLedger
            // 
            this.cmbLedger.FormattingEnabled = true;
            this.cmbLedger.Location = new System.Drawing.Point(156, 77);
            this.cmbLedger.Name = "cmbLedger";
            this.cmbLedger.Size = new System.Drawing.Size(293, 21);
            this.cmbLedger.TabIndex = 3;
            this.cmbLedger.SelectedIndexChanged += new System.EventHandler(this.cmbLedger_SelectedIndexChanged);
            // 
            // dgItems
            // 
            this.dgItems.AllowUserToAddRows = false;
            this.dgItems.AllowUserToDeleteRows = false;
            this.dgItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItems.Location = new System.Drawing.Point(9, 204);
            this.dgItems.Name = "dgItems";
            this.dgItems.ReadOnly = true;
            this.dgItems.Size = new System.Drawing.Size(1338, 426);
            this.dgItems.TabIndex = 69;
            this.dgItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgItems_CellContentClick);
            this.dgItems.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgItems_DataBindingComplete);
            this.dgItems.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgItems_DataError);
            // 
            // ofdAttachment
            // 
            this.ofdAttachment.FileName = "openFileDialog1";
            this.ofdAttachment.Filter = "PDF Files (*.pdf)|*.pdf";
            this.ofdAttachment.InitialDirectory = "c:\\\\";
            this.ofdAttachment.RestoreDirectory = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1275, 636);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 70;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // usrSupplierBatchRequisition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgItems);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "usrSupplierBatchRequisition";
            this.Size = new System.Drawing.Size(1350, 664);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtInvoiceDate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnSupplierLookup;
        private System.Windows.Forms.Label lbSupplierName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker trnDatePicker;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbLedger;
        private System.Windows.Forms.DataGridView dgItems;
        private System.Windows.Forms.OpenFileDialog ofdAttachment;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cbSupplierBankAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDefaultDate;
        private System.Windows.Forms.Button btnDefaultLedger;
    }
}

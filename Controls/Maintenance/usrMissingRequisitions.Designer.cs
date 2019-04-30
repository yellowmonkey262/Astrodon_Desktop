namespace Astrodon.Controls.Maintenance
{
    partial class usrMissingRequisitions
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
            this.btnLoadRequisitions = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbLedger = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbAmount = new System.Windows.Forms.Label();
            this.btnUploadInvoice = new System.Windows.Forms.Button();
            this.dtInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.txtInvoiceNumber = new System.Windows.Forms.TextBox();
            this.lbInvoiceNumber = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbAccountNumber = new System.Windows.Forms.Label();
            this.lbBankName = new System.Windows.Forms.Label();
            this.btnSupplierLookup = new System.Windows.Forms.Button();
            this.lbSupplierName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPaymentRef = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.ofdAttachment = new System.Windows.Forms.OpenFileDialog();
            this.lbAccount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dgItems = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(161, 21);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(233, 21);
            this.cmbBuilding.TabIndex = 11;
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Select Building";
            // 
            // btnLoadRequisitions
            // 
            this.btnLoadRequisitions.Location = new System.Drawing.Point(410, 21);
            this.btnLoadRequisitions.Name = "btnLoadRequisitions";
            this.btnLoadRequisitions.Size = new System.Drawing.Size(75, 23);
            this.btnLoadRequisitions.TabIndex = 12;
            this.btnLoadRequisitions.Text = "Load";
            this.btnLoadRequisitions.UseVisualStyleBackColor = true;
            this.btnLoadRequisitions.Click += new System.EventHandler(this.btnLoadRequisitions_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Date";
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Location = new System.Drawing.Point(158, 59);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(30, 13);
            this.lbDate.TabIndex = 23;
            this.lbDate.Text = "Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Ledger";
            // 
            // lbLedger
            // 
            this.lbLedger.AutoSize = true;
            this.lbLedger.Location = new System.Drawing.Point(158, 76);
            this.lbLedger.Name = "lbLedger";
            this.lbLedger.Size = new System.Drawing.Size(40, 13);
            this.lbLedger.TabIndex = 25;
            this.lbLedger.Text = "Ledger";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Amount";
            // 
            // lbAmount
            // 
            this.lbAmount.AutoSize = true;
            this.lbAmount.Location = new System.Drawing.Point(158, 94);
            this.lbAmount.Name = "lbAmount";
            this.lbAmount.Size = new System.Drawing.Size(43, 13);
            this.lbAmount.TabIndex = 27;
            this.lbAmount.Text = "Amount";
            // 
            // btnUploadInvoice
            // 
            this.btnUploadInvoice.Location = new System.Drawing.Point(371, 247);
            this.btnUploadInvoice.Name = "btnUploadInvoice";
            this.btnUploadInvoice.Size = new System.Drawing.Size(101, 23);
            this.btnUploadInvoice.TabIndex = 54;
            this.btnUploadInvoice.Text = "Upload Invoice";
            this.btnUploadInvoice.UseVisualStyleBackColor = true;
            this.btnUploadInvoice.Click += new System.EventHandler(this.btnUploadInvoice_Click);
            // 
            // dtInvoiceDate
            // 
            this.dtInvoiceDate.CustomFormat = "yyyy/MM/dd";
            this.dtInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtInvoiceDate.Location = new System.Drawing.Point(157, 247);
            this.dtInvoiceDate.Name = "dtInvoiceDate";
            this.dtInvoiceDate.Size = new System.Drawing.Size(204, 20);
            this.dtInvoiceDate.TabIndex = 53;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 253);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 13);
            this.label14.TabIndex = 52;
            this.label14.Text = "Invoice Date";
            // 
            // txtInvoiceNumber
            // 
            this.txtInvoiceNumber.Location = new System.Drawing.Point(158, 220);
            this.txtInvoiceNumber.Name = "txtInvoiceNumber";
            this.txtInvoiceNumber.Size = new System.Drawing.Size(204, 20);
            this.txtInvoiceNumber.TabIndex = 43;
            // 
            // lbInvoiceNumber
            // 
            this.lbInvoiceNumber.AutoSize = true;
            this.lbInvoiceNumber.Location = new System.Drawing.Point(12, 220);
            this.lbInvoiceNumber.Name = "lbInvoiceNumber";
            this.lbInvoiceNumber.Size = new System.Drawing.Size(82, 13);
            this.lbInvoiceNumber.TabIndex = 51;
            this.lbInvoiceNumber.Text = "Invoice Number";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 161);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "Banking Details";
            // 
            // lbAccountNumber
            // 
            this.lbAccountNumber.AutoSize = true;
            this.lbAccountNumber.Location = new System.Drawing.Point(158, 177);
            this.lbAccountNumber.Name = "lbAccountNumber";
            this.lbAccountNumber.Size = new System.Drawing.Size(105, 13);
            this.lbAccountNumber.TabIndex = 49;
            this.lbAccountNumber.Text = "-- Account Number --";
            // 
            // lbBankName
            // 
            this.lbBankName.AutoSize = true;
            this.lbBankName.Location = new System.Drawing.Point(158, 161);
            this.lbBankName.Name = "lbBankName";
            this.lbBankName.Size = new System.Drawing.Size(81, 13);
            this.lbBankName.TabIndex = 48;
            this.lbBankName.Text = "-- Bank Name --";
            // 
            // btnSupplierLookup
            // 
            this.btnSupplierLookup.Location = new System.Drawing.Point(377, 138);
            this.btnSupplierLookup.Name = "btnSupplierLookup";
            this.btnSupplierLookup.Size = new System.Drawing.Size(75, 23);
            this.btnSupplierLookup.TabIndex = 47;
            this.btnSupplierLookup.Text = "Find";
            this.btnSupplierLookup.UseVisualStyleBackColor = true;
            this.btnSupplierLookup.Click += new System.EventHandler(this.btnSupplierLookup_Click);
            // 
            // lbSupplierName
            // 
            this.lbSupplierName.AutoSize = true;
            this.lbSupplierName.Location = new System.Drawing.Point(158, 138);
            this.lbSupplierName.Name = "lbSupplierName";
            this.lbSupplierName.Size = new System.Drawing.Size(92, 13);
            this.lbSupplierName.TabIndex = 46;
            this.lbSupplierName.Text = "-- none selected --";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 138);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 13);
            this.label12.TabIndex = 45;
            this.label12.Text = "Select Supplier";
            // 
            // txtPaymentRef
            // 
            this.txtPaymentRef.Location = new System.Drawing.Point(158, 194);
            this.txtPaymentRef.Name = "txtPaymentRef";
            this.txtPaymentRef.Size = new System.Drawing.Size(204, 20);
            this.txtPaymentRef.TabIndex = 42;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 194);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(126, 13);
            this.label11.TabIndex = 44;
            this.label11.Text = "Enter Supplier Reference";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(371, 276);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 55;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ofdAttachment
            // 
            this.ofdAttachment.FileName = "openFileDialog1";
            this.ofdAttachment.Filter = "PDF Files (*.pdf)|*.pdf";
            this.ofdAttachment.InitialDirectory = "c:\\\\";
            this.ofdAttachment.Multiselect = true;
            this.ofdAttachment.RestoreDirectory = true;
            // 
            // lbAccount
            // 
            this.lbAccount.AutoSize = true;
            this.lbAccount.Location = new System.Drawing.Point(158, 110);
            this.lbAccount.Name = "lbAccount";
            this.lbAccount.Size = new System.Drawing.Size(47, 13);
            this.lbAccount.TabIndex = 57;
            this.lbAccount.Text = "Account";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 56;
            this.label6.Text = "Type";
            // 
            // dgItems
            // 
            this.dgItems.AllowUserToAddRows = false;
            this.dgItems.AllowUserToDeleteRows = false;
            this.dgItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItems.Location = new System.Drawing.Point(3, 305);
            this.dgItems.Name = "dgItems";
            this.dgItems.ReadOnly = true;
            this.dgItems.Size = new System.Drawing.Size(795, 369);
            this.dgItems.TabIndex = 58;
            this.dgItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgItems_CellContentClick);
            // 
            // usrMissingRequisitions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgItems);
            this.Controls.Add(this.lbAccount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUploadInvoice);
            this.Controls.Add(this.dtInvoiceDate);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtInvoiceNumber);
            this.Controls.Add(this.lbInvoiceNumber);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lbAccountNumber);
            this.Controls.Add(this.lbBankName);
            this.Controls.Add(this.btnSupplierLookup);
            this.Controls.Add(this.lbSupplierName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtPaymentRef);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lbAmount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbLedger);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoadRequisitions);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label3);
            this.Name = "usrMissingRequisitions";
            this.Size = new System.Drawing.Size(801, 676);
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLoadRequisitions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbLedger;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbAmount;
        private System.Windows.Forms.Button btnUploadInvoice;
        private System.Windows.Forms.DateTimePicker dtInvoiceDate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtInvoiceNumber;
        private System.Windows.Forms.Label lbInvoiceNumber;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbAccountNumber;
        private System.Windows.Forms.Label lbBankName;
        private System.Windows.Forms.Button btnSupplierLookup;
        private System.Windows.Forms.Label lbSupplierName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtPaymentRef;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog ofdAttachment;
        private System.Windows.Forms.Label lbAccount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgItems;
    }
}

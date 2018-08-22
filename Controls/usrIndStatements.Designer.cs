namespace Astrodon
{
    partial class usrIndStatements
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
            this.stmtDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenSend = new System.Windows.Forms.Button();
            this.btnGenView = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.txtAttachment = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblOS = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dgTransactions = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransactions)).BeginInit();
            this.SuspendLayout();
            // 
            // stmtDatePicker
            // 
            this.stmtDatePicker.CustomFormat = "yyyy/MM/dd";
            this.stmtDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.stmtDatePicker.Location = new System.Drawing.Point(131, 12);
            this.stmtDatePicker.Name = "stmtDatePicker";
            this.stmtDatePicker.Size = new System.Drawing.Size(108, 20);
            this.stmtDatePicker.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Statement Date:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(548, 487);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenSend
            // 
            this.btnGenSend.Location = new System.Drawing.Point(424, 487);
            this.btnGenSend.Name = "btnGenSend";
            this.btnGenSend.Size = new System.Drawing.Size(117, 23);
            this.btnGenSend.TabIndex = 25;
            this.btnGenSend.Text = "Generate and Send";
            this.btnGenSend.UseVisualStyleBackColor = true;
            this.btnGenSend.Click += new System.EventHandler(this.btnGenSend_Click);
            // 
            // btnGenView
            // 
            this.btnGenView.Location = new System.Drawing.Point(310, 487);
            this.btnGenView.Name = "btnGenView";
            this.btnGenView.Size = new System.Drawing.Size(108, 23);
            this.btnGenView.TabIndex = 24;
            this.btnGenView.Text = "Generate and View";
            this.btnGenView.UseVisualStyleBackColor = true;
            this.btnGenView.Click += new System.EventHandler(this.btnGenView_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(369, 116);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 23;
            this.btnUpload.Text = "Select File";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Visible = false;
            // 
            // txtAttachment
            // 
            this.txtAttachment.Location = new System.Drawing.Point(131, 118);
            this.txtAttachment.Name = "txtAttachment";
            this.txtAttachment.Size = new System.Drawing.Size(231, 20);
            this.txtAttachment.TabIndex = 22;
            this.txtAttachment.Visible = false;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(131, 92);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(313, 20);
            this.txtMessage.TabIndex = 21;
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.DisplayMember = "accNumber";
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(131, 65);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(177, 21);
            this.cmbCustomer.TabIndex = 20;
            this.cmbCustomer.ValueMember = "accNumber";
            this.cmbCustomer.SelectedIndexChanged += new System.EventHandler(this.cmbCustomer_SelectedIndexChanged);
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DisplayMember = "Building";
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(131, 38);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(177, 21);
            this.cmbBuilding.TabIndex = 19;
            this.cmbBuilding.ValueMember = "Building";
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Additional Message:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Attachment:";
            this.label4.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Select Customer:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Select Building:";
            // 
            // lblOS
            // 
            this.lblOS.AutoSize = true;
            this.lblOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOS.Location = new System.Drawing.Point(175, 456);
            this.lblOS.Name = "lblOS";
            this.lblOS.Size = new System.Drawing.Size(0, 20);
            this.lblOS.TabIndex = 57;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(12, 456);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(157, 20);
            this.label12.TabIndex = 56;
            this.label12.Text = "Total Outstanding:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 148);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 13);
            this.label11.TabIndex = 55;
            this.label11.Text = "Transactions";
            // 
            // dgTransactions
            // 
            this.dgTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTransactions.Location = new System.Drawing.Point(15, 173);
            this.dgTransactions.Name = "dgTransactions";
            this.dgTransactions.Size = new System.Drawing.Size(563, 271);
            this.dgTransactions.TabIndex = 54;
            // 
            // usrIndStatements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblOS);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dgTransactions);
            this.Controls.Add(this.stmtDatePicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenSend);
            this.Controls.Add(this.btnGenView);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.txtAttachment);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "usrIndStatements";
            this.Size = new System.Drawing.Size(639, 524);
            this.Load += new System.EventHandler(this.usrIndStatements_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgTransactions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker stmtDatePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenSend;
        private System.Windows.Forms.Button btnGenView;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TextBox txtAttachment;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOS;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dgTransactions;
    }
}

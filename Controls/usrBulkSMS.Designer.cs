namespace Astrodon {
    partial class usrBulkSMS {
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
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.dgCustomers = new System.Windows.Forms.DataGridView();
            this.pnlMsg = new System.Windows.Forms.Panel();
            this.chkImmediate = new System.Windows.Forms.CheckBox();
            this.chkMarketing = new System.Windows.Forms.CheckBox();
            this.chkBillCustomer = new System.Windows.Forms.CheckBox();
            this.chkBillBuilding = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentCredits = new System.Windows.Forms.Label();
            this.lblAfterSending = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgCustomers)).BeginInit();
            this.pnlMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(259, 15);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(123, 17);
            this.chkAll.TabIndex = 18;
            this.chkAll.Text = "Select / Deselect All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // dgCustomers
            // 
            this.dgCustomers.AllowUserToAddRows = false;
            this.dgCustomers.AllowUserToDeleteRows = false;
            this.dgCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCustomers.Location = new System.Drawing.Point(12, 40);
            this.dgCustomers.Name = "dgCustomers";
            this.dgCustomers.Size = new System.Drawing.Size(544, 293);
            this.dgCustomers.TabIndex = 17;
            this.dgCustomers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCustomers_CellValueChanged);
            // 
            // pnlMsg
            // 
            this.pnlMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlMsg.Controls.Add(this.chkImmediate);
            this.pnlMsg.Controls.Add(this.chkMarketing);
            this.pnlMsg.Controls.Add(this.chkBillCustomer);
            this.pnlMsg.Controls.Add(this.chkBillBuilding);
            this.pnlMsg.Controls.Add(this.btnSend);
            this.pnlMsg.Controls.Add(this.txtMessage);
            this.pnlMsg.Controls.Add(this.label4);
            this.pnlMsg.Controls.Add(this.btnCancel);
            this.pnlMsg.Location = new System.Drawing.Point(12, 339);
            this.pnlMsg.Name = "pnlMsg";
            this.pnlMsg.Size = new System.Drawing.Size(670, 79);
            this.pnlMsg.TabIndex = 16;
            // 
            // chkImmediate
            // 
            this.chkImmediate.AutoSize = true;
            this.chkImmediate.Location = new System.Drawing.Point(297, 39);
            this.chkImmediate.Name = "chkImmediate";
            this.chkImmediate.Size = new System.Drawing.Size(108, 17);
            this.chkImmediate.TabIndex = 13;
            this.chkImmediate.Text = "Send immediately";
            this.chkImmediate.UseVisualStyleBackColor = true;
            // 
            // chkMarketing
            // 
            this.chkMarketing.AutoSize = true;
            this.chkMarketing.Location = new System.Drawing.Point(192, 39);
            this.chkMarketing.Name = "chkMarketing";
            this.chkMarketing.Size = new System.Drawing.Size(99, 17);
            this.chkMarketing.TabIndex = 12;
            this.chkMarketing.Text = "Marketing SMS";
            this.chkMarketing.UseVisualStyleBackColor = true;
            this.chkMarketing.CheckedChanged += new System.EventHandler(this.chkMarketing_CheckedChanged);
            // 
            // chkBillCustomer
            // 
            this.chkBillCustomer.AutoSize = true;
            this.chkBillCustomer.Location = new System.Drawing.Point(100, 39);
            this.chkBillCustomer.Name = "chkBillCustomer";
            this.chkBillCustomer.Size = new System.Drawing.Size(86, 17);
            this.chkBillCustomer.TabIndex = 11;
            this.chkBillCustomer.Text = "Bill Customer";
            this.chkBillCustomer.UseVisualStyleBackColor = true;
            this.chkBillCustomer.CheckedChanged += new System.EventHandler(this.chkBillCustomer_CheckedChanged);
            // 
            // chkBillBuilding
            // 
            this.chkBillBuilding.AutoSize = true;
            this.chkBillBuilding.Location = new System.Drawing.Point(15, 39);
            this.chkBillBuilding.Name = "chkBillBuilding";
            this.chkBillBuilding.Size = new System.Drawing.Size(79, 17);
            this.chkBillBuilding.TabIndex = 10;
            this.chkBillBuilding.Text = "Bill Building";
            this.chkBillBuilding.UseVisualStyleBackColor = true;
            this.chkBillBuilding.CheckedChanged += new System.EventHandler(this.chkBillBuilding_CheckedChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(497, 39);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 9;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(72, 13);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(581, 20);
            this.txtMessage.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Message:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(578, 39);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
           
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DisplayMember = "Name";
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(59, 13);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(194, 21);
            this.cmbBuilding.TabIndex = 15;
            this.cmbBuilding.ValueMember = "ID";
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Building";
            // 
            // lblCurrentCredits
            // 
            this.lblCurrentCredits.AutoSize = true;
            this.lblCurrentCredits.Location = new System.Drawing.Point(562, 40);
            this.lblCurrentCredits.Name = "lblCurrentCredits";
            this.lblCurrentCredits.Size = new System.Drawing.Size(79, 13);
            this.lblCurrentCredits.TabIndex = 19;
            this.lblCurrentCredits.Text = "Current Credits:";
            // 
            // lblAfterSending
            // 
            this.lblAfterSending.AutoSize = true;
            this.lblAfterSending.Location = new System.Drawing.Point(562, 65);
            this.lblAfterSending.Name = "lblAfterSending";
            this.lblAfterSending.Size = new System.Drawing.Size(95, 13);
            this.lblAfterSending.TabIndex = 20;
            this.lblAfterSending.Text = "Credits Remaining:";
            // 
            // usrBulkSMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAfterSending);
            this.Controls.Add(this.lblCurrentCredits);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.dgCustomers);
            this.Controls.Add(this.pnlMsg);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label1);
            this.Name = "usrBulkSMS";
            this.Size = new System.Drawing.Size(694, 437);
            this.Load += new System.EventHandler(this.usrBulkSMS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgCustomers)).EndInit();
            this.pnlMsg.ResumeLayout(false);
            this.pnlMsg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.DataGridView dgCustomers;
        private System.Windows.Forms.Panel pnlMsg;
        private System.Windows.Forms.CheckBox chkMarketing;
        private System.Windows.Forms.CheckBox chkBillCustomer;
        private System.Windows.Forms.CheckBox chkBillBuilding;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentCredits;
        private System.Windows.Forms.Label lblAfterSending;
        private System.Windows.Forms.CheckBox chkImmediate;
    }
}

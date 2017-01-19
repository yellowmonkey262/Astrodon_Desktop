namespace Astrodon {
    partial class usrStatements {
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
            this.btnFile = new System.Windows.Forms.Button();
            this.txtAttachment = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnProcess = new System.Windows.Forms.Button();
            this.stmtDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dgBuildings = new System.Windows.Forms.DataGridView();
            this.lblCCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgBuildings)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(768, 31);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 23);
            this.btnFile.TabIndex = 19;
            this.btnFile.Text = "Select File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtAttachment
            // 
            this.txtAttachment.Location = new System.Drawing.Point(614, 31);
            this.txtAttachment.Name = "txtAttachment";
            this.txtAttachment.Size = new System.Drawing.Size(148, 20);
            this.txtAttachment.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(523, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Add attachment:";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(614, 8);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(392, 20);
            this.txtMessage.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(369, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Additional message to be displayed on statement:";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(931, 579);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 14;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // stmtDatePicker
            // 
            this.stmtDatePicker.CustomFormat = "yyyy/MM/dd";
            this.stmtDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.stmtDatePicker.Location = new System.Drawing.Point(243, 7);
            this.stmtDatePicker.Name = "stmtDatePicker";
            this.stmtDatePicker.Size = new System.Drawing.Size(106, 20);
            this.stmtDatePicker.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Statement Date:";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(12, 10);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(123, 17);
            this.chkSelectAll.TabIndex = 11;
            this.chkSelectAll.Text = "Select / Deselect All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // dgBuildings
            // 
            this.dgBuildings.AllowUserToAddRows = false;
            this.dgBuildings.AllowUserToDeleteRows = false;
            this.dgBuildings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgBuildings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBuildings.Location = new System.Drawing.Point(12, 59);
            this.dgBuildings.Name = "dgBuildings";
            this.dgBuildings.Size = new System.Drawing.Size(994, 514);
            this.dgBuildings.TabIndex = 10;
            this.dgBuildings.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgBuildings_DataBindingComplete);
            // 
            // lblCCount
            // 
            this.lblCCount.AutoSize = true;
            this.lblCCount.Location = new System.Drawing.Point(849, 36);
            this.lblCCount.Name = "lblCCount";
            this.lblCCount.Size = new System.Drawing.Size(0, 13);
            this.lblCCount.TabIndex = 20;
            // 
            // usrStatements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCCount);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.txtAttachment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.stmtDatePicker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.dgBuildings);
            this.Name = "usrStatements";
            this.Size = new System.Drawing.Size(1021, 618);
            this.Load += new System.EventHandler(this.usrStatements_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgBuildings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtAttachment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.DateTimePicker stmtDatePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.DataGridView dgBuildings;
        private System.Windows.Forms.Label lblCCount;
    }
}

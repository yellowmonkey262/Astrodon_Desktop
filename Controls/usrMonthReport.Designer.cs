namespace Astrodon.Controls {
    partial class usrMonthReport {
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
            this.label1 = new System.Windows.Forms.Label();
            this.dgMonthly = new System.Windows.Forms.DataGridView();
            this.colBuild = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFinPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.rdCompleted = new System.Windows.Forms.RadioButton();
            this.rdIncomplete = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgMonthly)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select period";
            // 
            // dgMonthly
            // 
            this.dgMonthly.AllowUserToAddRows = false;
            this.dgMonthly.AllowUserToDeleteRows = false;
            this.dgMonthly.AllowUserToResizeColumns = false;
            this.dgMonthly.AllowUserToResizeRows = false;
            this.dgMonthly.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgMonthly.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMonthly.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBuild,
            this.colCode,
            this.colFinPeriod,
            this.colDate,
            this.colUser});
            this.dgMonthly.Location = new System.Drawing.Point(20, 43);
            this.dgMonthly.Name = "dgMonthly";
            this.dgMonthly.ReadOnly = true;
            this.dgMonthly.Size = new System.Drawing.Size(842, 498);
            this.dgMonthly.TabIndex = 6;
            // 
            // colBuild
            // 
            this.colBuild.DataPropertyName = "Building";
            this.colBuild.HeaderText = "Building";
            this.colBuild.Name = "colBuild";
            this.colBuild.ReadOnly = true;
            // 
            // colCode
            // 
            this.colCode.DataPropertyName = "Code";
            this.colCode.HeaderText = "Code";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colFinPeriod
            // 
            this.colFinPeriod.DataPropertyName = "finPeriod";
            this.colFinPeriod.HeaderText = "Month / Year";
            this.colFinPeriod.Name = "colFinPeriod";
            this.colFinPeriod.ReadOnly = true;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "prcDate";
            this.colDate.HeaderText = "Process Date";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            // 
            // colUser
            // 
            this.colUser.DataPropertyName = "User";
            this.colUser.HeaderText = "User";
            this.colUser.Name = "colUser";
            this.colUser.ReadOnly = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(787, 547);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "MMM yyyy";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(140, 17);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(109, 20);
            this.dtStart.TabIndex = 1;
            this.dtStart.ValueChanged += new System.EventHandler(this.dtStart_ValueChanged);
            // 
            // rdCompleted
            // 
            this.rdCompleted.AutoSize = true;
            this.rdCompleted.Location = new System.Drawing.Point(255, 17);
            this.rdCompleted.Name = "rdCompleted";
            this.rdCompleted.Size = new System.Drawing.Size(69, 17);
            this.rdCompleted.TabIndex = 9;
            this.rdCompleted.TabStop = true;
            this.rdCompleted.Text = "Complete";
            this.rdCompleted.UseVisualStyleBackColor = true;
            this.rdCompleted.CheckedChanged += new System.EventHandler(this.rdCompleted_CheckedChanged);
            // 
            // rdIncomplete
            // 
            this.rdIncomplete.AutoSize = true;
            this.rdIncomplete.Location = new System.Drawing.Point(346, 17);
            this.rdIncomplete.Name = "rdIncomplete";
            this.rdIncomplete.Size = new System.Drawing.Size(77, 17);
            this.rdIncomplete.TabIndex = 10;
            this.rdIncomplete.TabStop = true;
            this.rdIncomplete.Text = "Incomplete";
            this.rdIncomplete.UseVisualStyleBackColor = true;
            this.rdIncomplete.CheckedChanged += new System.EventHandler(this.rdCompleted_CheckedChanged);
            // 
            // usrMonthReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdIncomplete);
            this.Controls.Add(this.rdCompleted);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dgMonthly);
            this.Controls.Add(this.dtStart);
            this.Controls.Add(this.label1);
            this.Name = "usrMonthReport";
            this.Size = new System.Drawing.Size(891, 664);
            this.Load += new System.EventHandler(this.usrMonthReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgMonthly)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgMonthly;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBuild;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFinPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.RadioButton rdCompleted;
        private System.Windows.Forms.RadioButton rdIncomplete;
    }
}

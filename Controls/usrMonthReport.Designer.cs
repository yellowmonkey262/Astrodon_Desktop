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
            this.dgMonthly = new System.Windows.Forms.DataGridView();
            this.colBuild = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFinPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPrint = new System.Windows.Forms.Button();
            this.rdCompleted = new System.Windows.Forms.RadioButton();
            this.rdIncomplete = new System.Windows.Forms.RadioButton();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbUserList = new System.Windows.Forms.ComboBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgMonthly)).BeginInit();
            this.SuspendLayout();
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
            this.dgMonthly.Location = new System.Drawing.Point(20, 130);
            this.dgMonthly.Name = "dgMonthly";
            this.dgMonthly.ReadOnly = true;
            this.dgMonthly.Size = new System.Drawing.Size(842, 411);
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
            // rdCompleted
            // 
            this.rdCompleted.AutoSize = true;
            this.rdCompleted.Location = new System.Drawing.Point(112, 84);
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
            this.rdIncomplete.Checked = true;
            this.rdIncomplete.Location = new System.Drawing.Point(187, 84);
            this.rdIncomplete.Name = "rdIncomplete";
            this.rdIncomplete.Size = new System.Drawing.Size(77, 17);
            this.rdIncomplete.TabIndex = 10;
            this.rdIncomplete.TabStop = true;
            this.rdIncomplete.Text = "Incomplete";
            this.rdIncomplete.UseVisualStyleBackColor = true;
            this.rdIncomplete.CheckedChanged += new System.EventHandler(this.rdCompleted_CheckedChanged);
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(115, 30);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(172, 21);
            this.cmbMonth.TabIndex = 14;
            this.cmbMonth.SelectedValueChanged += new System.EventHandler(this.cmbMonth_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Month";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(115, 3);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(172, 21);
            this.cmbYear.TabIndex = 12;
            this.cmbYear.SelectedValueChanged += new System.EventHandler(this.cmbYear_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Year";
            // 
            // cbUserList
            // 
            this.cbUserList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserList.FormattingEnabled = true;
            this.cbUserList.Location = new System.Drawing.Point(115, 57);
            this.cbUserList.Name = "cbUserList";
            this.cbUserList.Size = new System.Drawing.Size(172, 21);
            this.cbUserList.TabIndex = 16;
            this.cbUserList.SelectedValueChanged += new System.EventHandler(this.cbUserList_SelectedValueChanged);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(22, 57);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 13);
            this.lblUser.TabIndex = 15;
            this.lblUser.Text = "User";
            // 
            // dlgSave
            // 
            this.dlgSave.CheckPathExists = false;
            this.dlgSave.DefaultExt = "xlsx";
            this.dlgSave.Filter = "Microsoft Excel files (*.xlsx)|*.xlsx";
            this.dlgSave.Title = "Monthly Report";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(308, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Allocate Users";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // usrMonthReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbUserList);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.cmbMonth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rdIncomplete);
            this.Controls.Add(this.rdCompleted);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dgMonthly);
            this.Name = "usrMonthReport";
            this.Size = new System.Drawing.Size(891, 664);
            this.Load += new System.EventHandler(this.usrMonthReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgMonthly)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgMonthly;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBuild;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFinPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.RadioButton rdCompleted;
        private System.Windows.Forms.RadioButton rdIncomplete;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbUserList;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.Button button1;
    }
}

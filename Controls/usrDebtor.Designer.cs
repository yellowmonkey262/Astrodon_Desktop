namespace Astrodon.Controls {
    partial class usrDebtor {
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbDaily = new System.Windows.Forms.TabPage();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.dgDaily = new System.Windows.Forms.DataGridView();
            this.dailyPicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLetters = new System.Windows.Forms.TabPage();
            this.btnResetLetters = new System.Windows.Forms.Button();
            this.btnSubmitLetters = new System.Windows.Forms.Button();
            this.dgLetters = new System.Windows.Forms.DataGridView();
            this.letterDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tbStmts = new System.Windows.Forms.TabPage();
            this.btnResetStmt = new System.Windows.Forms.Button();
            this.btnSubmitStmt = new System.Windows.Forms.Button();
            this.dgStmt = new System.Windows.Forms.DataGridView();
            this.stmtPicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMonthly = new System.Windows.Forms.TabPage();
            this.btnResetMonth = new System.Windows.Forms.Button();
            this.btnSubmitMonth = new System.Windows.Forms.Button();
            this.dgMonth = new System.Windows.Forms.DataGridView();
            this.monthPicker = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tbDaily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDaily)).BeginInit();
            this.tbLetters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLetters)).BeginInit();
            this.tbStmts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgStmt)).BeginInit();
            this.tbMonthly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbDaily);
            this.tabControl1.Controls.Add(this.tbLetters);
            this.tabControl1.Controls.Add(this.tbStmts);
            this.tabControl1.Controls.Add(this.tbMonthly);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1007, 660);
            this.tabControl1.TabIndex = 0;
            // 
            // tbDaily
            // 
            this.tbDaily.Controls.Add(this.btnReset);
            this.tbDaily.Controls.Add(this.btnSubmit);
            this.tbDaily.Controls.Add(this.dgDaily);
            this.tbDaily.Controls.Add(this.dailyPicker);
            this.tbDaily.Controls.Add(this.label1);
            this.tbDaily.Location = new System.Drawing.Point(4, 22);
            this.tbDaily.Name = "tbDaily";
            this.tbDaily.Size = new System.Drawing.Size(999, 634);
            this.tbDaily.TabIndex = 3;
            this.tbDaily.Text = "Daily";
            this.tbDaily.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(907, 593);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(826, 593);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // dgDaily
            // 
            this.dgDaily.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgDaily.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDaily.Location = new System.Drawing.Point(15, 41);
            this.dgDaily.Name = "dgDaily";
            this.dgDaily.Size = new System.Drawing.Size(967, 546);
            this.dgDaily.TabIndex = 2;
            // 
            // dailyPicker
            // 
            this.dailyPicker.CustomFormat = "yyyy/MM/dd";
            this.dailyPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dailyPicker.Location = new System.Drawing.Point(115, 9);
            this.dailyPicker.Name = "dailyPicker";
            this.dailyPicker.Size = new System.Drawing.Size(106, 20);
            this.dailyPicker.TabIndex = 1;
            this.dailyPicker.ValueChanged += new System.EventHandler(this.dailyPicker_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select date:";
            // 
            // tbLetters
            // 
            this.tbLetters.Controls.Add(this.btnResetLetters);
            this.tbLetters.Controls.Add(this.btnSubmitLetters);
            this.tbLetters.Controls.Add(this.dgLetters);
            this.tbLetters.Controls.Add(this.letterDatePicker);
            this.tbLetters.Controls.Add(this.label2);
            this.tbLetters.Location = new System.Drawing.Point(4, 22);
            this.tbLetters.Name = "tbLetters";
            this.tbLetters.Padding = new System.Windows.Forms.Padding(3);
            this.tbLetters.Size = new System.Drawing.Size(999, 634);
            this.tbLetters.TabIndex = 0;
            this.tbLetters.Text = "Letters";
            this.tbLetters.UseVisualStyleBackColor = true;
            // 
            // btnResetLetters
            // 
            this.btnResetLetters.Location = new System.Drawing.Point(918, 593);
            this.btnResetLetters.Name = "btnResetLetters";
            this.btnResetLetters.Size = new System.Drawing.Size(75, 23);
            this.btnResetLetters.TabIndex = 9;
            this.btnResetLetters.Text = "Reset";
            this.btnResetLetters.UseVisualStyleBackColor = true;
            this.btnResetLetters.Click += new System.EventHandler(this.btnResetLetters_Click);
            // 
            // btnSubmitLetters
            // 
            this.btnSubmitLetters.Location = new System.Drawing.Point(837, 593);
            this.btnSubmitLetters.Name = "btnSubmitLetters";
            this.btnSubmitLetters.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitLetters.TabIndex = 8;
            this.btnSubmitLetters.Text = "Submit";
            this.btnSubmitLetters.UseVisualStyleBackColor = true;
            this.btnSubmitLetters.Click += new System.EventHandler(this.btnSubmitLetters_Click);
            // 
            // dgLetters
            // 
            this.dgLetters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgLetters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLetters.Location = new System.Drawing.Point(15, 41);
            this.dgLetters.Name = "dgLetters";
            this.dgLetters.Size = new System.Drawing.Size(978, 546);
            this.dgLetters.TabIndex = 7;
            // 
            // letterDatePicker
            // 
            this.letterDatePicker.CustomFormat = "yyyy/MM/dd";
            this.letterDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.letterDatePicker.Location = new System.Drawing.Point(115, 12);
            this.letterDatePicker.Name = "letterDatePicker";
            this.letterDatePicker.Size = new System.Drawing.Size(106, 20);
            this.letterDatePicker.TabIndex = 6;
            this.letterDatePicker.ValueChanged += new System.EventHandler(this.letterDatePicker_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Please select date:";
            // 
            // tbStmts
            // 
            this.tbStmts.Controls.Add(this.btnResetStmt);
            this.tbStmts.Controls.Add(this.btnSubmitStmt);
            this.tbStmts.Controls.Add(this.dgStmt);
            this.tbStmts.Controls.Add(this.stmtPicker);
            this.tbStmts.Controls.Add(this.label3);
            this.tbStmts.Location = new System.Drawing.Point(4, 22);
            this.tbStmts.Name = "tbStmts";
            this.tbStmts.Padding = new System.Windows.Forms.Padding(3);
            this.tbStmts.Size = new System.Drawing.Size(999, 634);
            this.tbStmts.TabIndex = 1;
            this.tbStmts.Text = "Statements";
            this.tbStmts.UseVisualStyleBackColor = true;
            // 
            // btnResetStmt
            // 
            this.btnResetStmt.Location = new System.Drawing.Point(918, 595);
            this.btnResetStmt.Name = "btnResetStmt";
            this.btnResetStmt.Size = new System.Drawing.Size(75, 23);
            this.btnResetStmt.TabIndex = 9;
            this.btnResetStmt.Text = "Reset";
            this.btnResetStmt.UseVisualStyleBackColor = true;
            this.btnResetStmt.Click += new System.EventHandler(this.btnResetStmt_Click);
            // 
            // btnSubmitStmt
            // 
            this.btnSubmitStmt.Location = new System.Drawing.Point(837, 595);
            this.btnSubmitStmt.Name = "btnSubmitStmt";
            this.btnSubmitStmt.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitStmt.TabIndex = 8;
            this.btnSubmitStmt.Text = "Submit";
            this.btnSubmitStmt.UseVisualStyleBackColor = true;
            this.btnSubmitStmt.Click += new System.EventHandler(this.btnSubmitStmt_Click);
            // 
            // dgStmt
            // 
            this.dgStmt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgStmt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgStmt.Location = new System.Drawing.Point(15, 43);
            this.dgStmt.Name = "dgStmt";
            this.dgStmt.Size = new System.Drawing.Size(978, 546);
            this.dgStmt.TabIndex = 7;
            // 
            // stmtPicker
            // 
            this.stmtPicker.CustomFormat = "yyyy/MM/dd";
            this.stmtPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.stmtPicker.Location = new System.Drawing.Point(115, 17);
            this.stmtPicker.Name = "stmtPicker";
            this.stmtPicker.Size = new System.Drawing.Size(106, 20);
            this.stmtPicker.TabIndex = 6;
            this.stmtPicker.ValueChanged += new System.EventHandler(this.stmtPicker_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Please select date:";
            // 
            // tbMonthly
            // 
            this.tbMonthly.Controls.Add(this.btnResetMonth);
            this.tbMonthly.Controls.Add(this.btnSubmitMonth);
            this.tbMonthly.Controls.Add(this.dgMonth);
            this.tbMonthly.Controls.Add(this.monthPicker);
            this.tbMonthly.Controls.Add(this.label4);
            this.tbMonthly.Location = new System.Drawing.Point(4, 22);
            this.tbMonthly.Name = "tbMonthly";
            this.tbMonthly.Size = new System.Drawing.Size(999, 634);
            this.tbMonthly.TabIndex = 2;
            this.tbMonthly.Text = "Month End";
            this.tbMonthly.UseVisualStyleBackColor = true;
            // 
            // btnResetMonth
            // 
            this.btnResetMonth.Location = new System.Drawing.Point(921, 595);
            this.btnResetMonth.Name = "btnResetMonth";
            this.btnResetMonth.Size = new System.Drawing.Size(75, 23);
            this.btnResetMonth.TabIndex = 9;
            this.btnResetMonth.Text = "Reset";
            this.btnResetMonth.UseVisualStyleBackColor = true;
            this.btnResetMonth.Click += new System.EventHandler(this.btnResetMonth_Click);
            // 
            // btnSubmitMonth
            // 
            this.btnSubmitMonth.Location = new System.Drawing.Point(840, 595);
            this.btnSubmitMonth.Name = "btnSubmitMonth";
            this.btnSubmitMonth.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitMonth.TabIndex = 8;
            this.btnSubmitMonth.Text = "Submit";
            this.btnSubmitMonth.UseVisualStyleBackColor = true;
            this.btnSubmitMonth.Click += new System.EventHandler(this.btnSubmitMonth_Click);
            // 
            // dgMonth
            // 
            this.dgMonth.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgMonth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMonth.Location = new System.Drawing.Point(15, 43);
            this.dgMonth.Name = "dgMonth";
            this.dgMonth.Size = new System.Drawing.Size(981, 546);
            this.dgMonth.TabIndex = 7;
            // 
            // monthPicker
            // 
            this.monthPicker.CustomFormat = "yyyy/MM/dd";
            this.monthPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.monthPicker.Location = new System.Drawing.Point(115, 17);
            this.monthPicker.Name = "monthPicker";
            this.monthPicker.Size = new System.Drawing.Size(106, 20);
            this.monthPicker.TabIndex = 6;
            this.monthPicker.ValueChanged += new System.EventHandler(this.monthPicker_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Please select date:";
            // 
            // usrDebtor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "usrDebtor";
            this.Size = new System.Drawing.Size(1017, 663);
            this.Load += new System.EventHandler(this.usrDebtor_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbDaily.ResumeLayout(false);
            this.tbDaily.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDaily)).EndInit();
            this.tbLetters.ResumeLayout(false);
            this.tbLetters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLetters)).EndInit();
            this.tbStmts.ResumeLayout(false);
            this.tbStmts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgStmt)).EndInit();
            this.tbMonthly.ResumeLayout(false);
            this.tbMonthly.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMonth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbDaily;
        private System.Windows.Forms.TabPage tbLetters;
        private System.Windows.Forms.TabPage tbStmts;
        private System.Windows.Forms.TabPage tbMonthly;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.DataGridView dgDaily;
        private System.Windows.Forms.DateTimePicker dailyPicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnResetLetters;
        private System.Windows.Forms.Button btnSubmitLetters;
        private System.Windows.Forms.DataGridView dgLetters;
        private System.Windows.Forms.DateTimePicker letterDatePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnResetStmt;
        private System.Windows.Forms.Button btnSubmitStmt;
        private System.Windows.Forms.DataGridView dgStmt;
        private System.Windows.Forms.DateTimePicker stmtPicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnResetMonth;
        private System.Windows.Forms.Button btnSubmitMonth;
        private System.Windows.Forms.DataGridView dgMonth;
        private System.Windows.Forms.DateTimePicker monthPicker;
        private System.Windows.Forms.Label label4;
    }
}

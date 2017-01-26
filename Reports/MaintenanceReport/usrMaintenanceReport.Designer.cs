namespace Astrodon.Reports.MaintenanceReport
{
    partial class usrMaintenanceReport
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
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbReportType = new System.Windows.Forms.GroupBox();
            this.rbDetailWithDocs = new System.Windows.Forms.RadioButton();
            this.rbDetailed = new System.Windows.Forms.RadioButton();
            this.rbSummaryReport = new System.Windows.Forms.RadioButton();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.gbReportType.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(248, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Run Report";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Building";
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(90, 39);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(241, 21);
            this.cmbMonth.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Month";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(90, 12);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(241, 21);
            this.cmbYear.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Year";
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(90, 66);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(241, 21);
            this.cmbBuilding.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Report Type";
            // 
            // gbReportType
            // 
            this.gbReportType.Controls.Add(this.rbDetailWithDocs);
            this.gbReportType.Controls.Add(this.rbDetailed);
            this.gbReportType.Controls.Add(this.rbSummaryReport);
            this.gbReportType.Location = new System.Drawing.Point(90, 100);
            this.gbReportType.Name = "gbReportType";
            this.gbReportType.Size = new System.Drawing.Size(241, 100);
            this.gbReportType.TabIndex = 16;
            this.gbReportType.TabStop = false;
            // 
            // rbDetailWithDocs
            // 
            this.rbDetailWithDocs.AutoSize = true;
            this.rbDetailWithDocs.Location = new System.Drawing.Point(7, 68);
            this.rbDetailWithDocs.Name = "rbDetailWithDocs";
            this.rbDetailWithDocs.Size = new System.Drawing.Size(232, 17);
            this.rbDetailWithDocs.TabIndex = 2;
            this.rbDetailWithDocs.Text = "Detailed Report with Supporting Documents";
            this.rbDetailWithDocs.UseVisualStyleBackColor = true;
            // 
            // rbDetailed
            // 
            this.rbDetailed.AutoSize = true;
            this.rbDetailed.Location = new System.Drawing.Point(7, 44);
            this.rbDetailed.Name = "rbDetailed";
            this.rbDetailed.Size = new System.Drawing.Size(99, 17);
            this.rbDetailed.TabIndex = 1;
            this.rbDetailed.TabStop = true;
            this.rbDetailed.Text = "Detailed Report";
            this.rbDetailed.UseVisualStyleBackColor = true;
            // 
            // rbSummaryReport
            // 
            this.rbSummaryReport.AutoSize = true;
            this.rbSummaryReport.Checked = true;
            this.rbSummaryReport.Location = new System.Drawing.Point(7, 20);
            this.rbSummaryReport.Name = "rbSummaryReport";
            this.rbSummaryReport.Size = new System.Drawing.Size(103, 17);
            this.rbSummaryReport.TabIndex = 0;
            this.rbSummaryReport.TabStop = true;
            this.rbSummaryReport.Text = "Summary Report";
            this.rbSummaryReport.UseVisualStyleBackColor = true;
            // 
            // dlgSave
            // 
            this.dlgSave.CheckPathExists = false;
            this.dlgSave.DefaultExt = "pdf";
            this.dlgSave.Filter = "Adobe PDF files (*.pdf)|*.pdf";
            this.dlgSave.InitialDirectory = "Y:\\";
            this.dlgSave.Title = "Levy Roll Report";
            // 
            // usrMaintenanceReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbReportType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbMonth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbBuilding);
            this.Name = "usrMaintenanceReport";
            this.Size = new System.Drawing.Size(348, 261);
            this.gbReportType.ResumeLayout(false);
            this.gbReportType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbReportType;
        private System.Windows.Forms.RadioButton rbDetailWithDocs;
        private System.Windows.Forms.RadioButton rbDetailed;
        private System.Windows.Forms.RadioButton rbSummaryReport;
        private System.Windows.Forms.SaveFileDialog dlgSave;
    }
}

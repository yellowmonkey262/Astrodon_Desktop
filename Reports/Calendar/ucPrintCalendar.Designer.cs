namespace Astrodon.Reports.Calendar
{
    partial class ucPrintCalendar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucPrintCalendar));
            this.ppdCalendar = new System.Windows.Forms.PrintPreviewDialog();
            this.pdocCalendar = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDialog2 = new System.Windows.Forms.PrintDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.rbStaff = new System.Windows.Forms.RadioButton();
            this.rbFinancial = new System.Windows.Forms.RadioButton();
            this.cbPM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbFilterPM = new System.Windows.Forms.CheckBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbUserInvites = new System.Windows.Forms.CheckedListBox();
            this.tbAttachment = new System.Windows.Forms.TextBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.tbBodyContent = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbBCC = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbNotifyTrustees = new System.Windows.Forms.CheckBox();
            this.dtpEventToTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEventToDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.tbVenue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbEvent = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbBuilding = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEventTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEventDate = new System.Windows.Forms.DateTimePicker();
            this.Date = new System.Windows.Forms.Label();
            this.dgItems = new System.Windows.Forms.DataGridView();
            this.fdOpen = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
            this.SuspendLayout();
            // 
            // ppdCalendar
            // 
            this.ppdCalendar.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.ppdCalendar.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.ppdCalendar.ClientSize = new System.Drawing.Size(400, 300);
            this.ppdCalendar.Document = this.pdocCalendar;
            this.ppdCalendar.Enabled = true;
            this.ppdCalendar.Icon = ((System.Drawing.Icon)(resources.GetObject("ppdCalendar.Icon")));
            this.ppdCalendar.Name = "ppdCalendar";
            this.ppdCalendar.Visible = false;
            // 
            // pdocCalendar
            // 
            this.pdocCalendar.DocumentName = "Calendar";
            this.pdocCalendar.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pdocCalendar_PrintPage);
            this.pdocCalendar.QueryPageSettings += new System.Drawing.Printing.QueryPageSettingsEventHandler(this.pdocCalendar_QueryPageSettings);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printDialog2
            // 
            this.printDialog2.UseEXDialog = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.rbStaff);
            this.panel1.Controls.Add(this.rbFinancial);
            this.panel1.Controls.Add(this.cbPM);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbFilterPM);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.cmbMonth);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbYear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(914, 151);
            this.panel1.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 80);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Meeting Type";
            // 
            // rbStaff
            // 
            this.rbStaff.AutoSize = true;
            this.rbStaff.Location = new System.Drawing.Point(191, 80);
            this.rbStaff.Name = "rbStaff";
            this.rbStaff.Size = new System.Drawing.Size(47, 17);
            this.rbStaff.TabIndex = 21;
            this.rbStaff.Text = "Staff";
            this.rbStaff.UseVisualStyleBackColor = true;
            this.rbStaff.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rbFinancial
            // 
            this.rbFinancial.AutoSize = true;
            this.rbFinancial.Checked = true;
            this.rbFinancial.Location = new System.Drawing.Point(106, 80);
            this.rbFinancial.Name = "rbFinancial";
            this.rbFinancial.Size = new System.Drawing.Size(67, 17);
            this.rbFinancial.TabIndex = 20;
            this.rbFinancial.TabStop = true;
            this.rbFinancial.Text = "Financial";
            this.rbFinancial.UseVisualStyleBackColor = true;
            this.rbFinancial.CheckedChanged += new System.EventHandler(this.rbFinancial_CheckedChanged);
            // 
            // cbPM
            // 
            this.cbPM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPM.FormattingEnabled = true;
            this.cbPM.Location = new System.Drawing.Point(106, 108);
            this.cbPM.Name = "cbPM";
            this.cbPM.Size = new System.Drawing.Size(233, 21);
            this.cbPM.TabIndex = 3;
            this.cbPM.SelectedIndexChanged += new System.EventHandler(this.cbPM_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Filter by PM";
            // 
            // cbFilterPM
            // 
            this.cbFilterPM.AutoSize = true;
            this.cbFilterPM.Location = new System.Drawing.Point(83, 111);
            this.cbFilterPM.Name = "cbFilterPM";
            this.cbFilterPM.Size = new System.Drawing.Size(15, 14);
            this.cbFilterPM.TabIndex = 2;
            this.cbFilterPM.UseVisualStyleBackColor = true;
            this.cbFilterPM.CheckedChanged += new System.EventHandler(this.cbFilterPM_CheckedChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(345, 106);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(106, 53);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(233, 21);
            this.cmbMonth.TabIndex = 1;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Month";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(106, 26);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(233, 21);
            this.cmbYear.TabIndex = 0;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Year";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbUserInvites);
            this.panel2.Controls.Add(this.tbAttachment);
            this.panel2.Controls.Add(this.btnUpload);
            this.panel2.Controls.Add(this.tbBodyContent);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.tbSubject);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.tbBCC);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.cbNotifyTrustees);
            this.panel2.Controls.Add(this.dtpEventToTime);
            this.panel2.Controls.Add(this.dtpEventToDate);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnNew);
            this.panel2.Controls.Add(this.tbVenue);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cbEvent);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cbBuilding);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dtpEventTime);
            this.panel2.Controls.Add(this.dtpEventDate);
            this.panel2.Controls.Add(this.Date);
            this.panel2.Location = new System.Drawing.Point(3, 160);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(914, 231);
            this.panel2.TabIndex = 14;
            // 
            // cbUserInvites
            // 
            this.cbUserInvites.FormattingEnabled = true;
            this.cbUserInvites.Location = new System.Drawing.Point(700, 13);
            this.cbUserInvites.Name = "cbUserInvites";
            this.cbUserInvites.Size = new System.Drawing.Size(181, 169);
            this.cbUserInvites.TabIndex = 17;
            // 
            // tbAttachment
            // 
            this.tbAttachment.Enabled = false;
            this.tbAttachment.Location = new System.Drawing.Point(511, 166);
            this.tbAttachment.Name = "tbAttachment";
            this.tbAttachment.Size = new System.Drawing.Size(170, 20);
            this.tbAttachment.TabIndex = 59;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(384, 164);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(121, 23);
            this.btnUpload.TabIndex = 16;
            this.btnUpload.Text = "Attachment =>";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // tbBodyContent
            // 
            this.tbBodyContent.AcceptsReturn = true;
            this.tbBodyContent.Location = new System.Drawing.Point(511, 65);
            this.tbBodyContent.Multiline = true;
            this.tbBodyContent.Name = "tbBodyContent";
            this.tbBodyContent.Size = new System.Drawing.Size(170, 93);
            this.tbBodyContent.TabIndex = 15;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(400, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Invite Body Content";
            // 
            // tbSubject
            // 
            this.tbSubject.Location = new System.Drawing.Point(511, 39);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(170, 20);
            this.tbSubject.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(400, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Invite Subject";
            // 
            // tbBCC
            // 
            this.tbBCC.Location = new System.Drawing.Point(511, 13);
            this.tbBCC.Name = "tbBCC";
            this.tbBCC.Size = new System.Drawing.Size(170, 20);
            this.tbBCC.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(400, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "BCC";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Notify Trustees";
            // 
            // cbNotifyTrustees
            // 
            this.cbNotifyTrustees.AutoSize = true;
            this.cbNotifyTrustees.Location = new System.Drawing.Point(106, 88);
            this.cbNotifyTrustees.Name = "cbNotifyTrustees";
            this.cbNotifyTrustees.Size = new System.Drawing.Size(15, 14);
            this.cbNotifyTrustees.TabIndex = 10;
            this.cbNotifyTrustees.UseVisualStyleBackColor = true;
            // 
            // dtpEventToTime
            // 
            this.dtpEventToTime.Location = new System.Drawing.Point(220, 36);
            this.dtpEventToTime.Name = "dtpEventToTime";
            this.dtpEventToTime.Size = new System.Drawing.Size(107, 20);
            this.dtpEventToTime.TabIndex = 8;
            // 
            // dtpEventToDate
            // 
            this.dtpEventToDate.Location = new System.Drawing.Point(107, 36);
            this.dtpEventToDate.Name = "dtpEventToDate";
            this.dtpEventToDate.Size = new System.Drawing.Size(107, 20);
            this.dtpEventToDate.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "To";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(184, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(265, 193);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.AutoEllipsis = true;
            this.btnNew.Location = new System.Drawing.Point(15, 193);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 20;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // tbVenue
            // 
            this.tbVenue.Location = new System.Drawing.Point(106, 138);
            this.tbVenue.Name = "tbVenue";
            this.tbVenue.Size = new System.Drawing.Size(234, 20);
            this.tbVenue.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Venue";
            // 
            // cbEvent
            // 
            this.cbEvent.FormattingEnabled = true;
            this.cbEvent.Items.AddRange(new object[] {
            "AGM",
            "ADJ AGM",
            "SGM",
            "ADJ SGM",
            "TM"});
            this.cbEvent.Location = new System.Drawing.Point(107, 112);
            this.cbEvent.Name = "cbEvent";
            this.cbEvent.Size = new System.Drawing.Size(233, 21);
            this.cbEvent.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Event";
            // 
            // cbBuilding
            // 
            this.cbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuilding.FormattingEnabled = true;
            this.cbBuilding.Location = new System.Drawing.Point(107, 62);
            this.cbBuilding.Name = "cbBuilding";
            this.cbBuilding.Size = new System.Drawing.Size(233, 21);
            this.cbBuilding.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Building";
            // 
            // dtpEventTime
            // 
            this.dtpEventTime.Location = new System.Drawing.Point(219, 10);
            this.dtpEventTime.Name = "dtpEventTime";
            this.dtpEventTime.Size = new System.Drawing.Size(107, 20);
            this.dtpEventTime.TabIndex = 6;
            // 
            // dtpEventDate
            // 
            this.dtpEventDate.Location = new System.Drawing.Point(106, 10);
            this.dtpEventDate.Name = "dtpEventDate";
            this.dtpEventDate.Size = new System.Drawing.Size(107, 20);
            this.dtpEventDate.TabIndex = 5;
            // 
            // Date
            // 
            this.Date.AutoSize = true;
            this.Date.Location = new System.Drawing.Point(13, 16);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(30, 13);
            this.Date.TabIndex = 0;
            this.Date.Text = "From";
            // 
            // dgItems
            // 
            this.dgItems.AllowUserToAddRows = false;
            this.dgItems.AllowUserToDeleteRows = false;
            this.dgItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItems.Location = new System.Drawing.Point(3, 400);
            this.dgItems.Name = "dgItems";
            this.dgItems.ReadOnly = true;
            this.dgItems.Size = new System.Drawing.Size(911, 308);
            this.dgItems.TabIndex = 21;
            this.dgItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgItems_CellContentClick);
            // 
            // fdOpen
            // 
            this.fdOpen.Filter = "Adobe PDF files (*.pdf)|*.pdf";
            // 
            // ucPrintCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgItems);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ucPrintCalendar";
            this.Size = new System.Drawing.Size(917, 734);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PrintPreviewDialog ppdCalendar;
        private System.Drawing.Printing.PrintDocument pdocCalendar;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintDialog printDialog2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbPM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbFilterPM;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpEventDate;
        private System.Windows.Forms.Label Date;
        private System.Windows.Forms.DateTimePicker dtpEventTime;
        private System.Windows.Forms.ComboBox cbBuilding;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbEvent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbVenue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView dgItems;
        private System.Windows.Forms.DateTimePicker dtpEventToTime;
        private System.Windows.Forms.DateTimePicker dtpEventToDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbNotifyTrustees;
        private System.Windows.Forms.TextBox tbBCC;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbBodyContent;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbSubject;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.OpenFileDialog fdOpen;
        private System.Windows.Forms.TextBox tbAttachment;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton rbStaff;
        private System.Windows.Forms.RadioButton rbFinancial;
        private System.Windows.Forms.CheckedListBox cbUserInvites;
    }
}

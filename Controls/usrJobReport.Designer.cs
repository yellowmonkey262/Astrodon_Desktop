namespace Astrodon.Controls {
    partial class usrJobReport {
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
            this.cmbSelector = new System.Windows.Forms.ComboBox();
            this.cmbCriteria = new System.Windows.Forms.ComboBox();
            this.lblSelector = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotAss = new System.Windows.Forms.Label();
            this.lblAvgAss = new System.Windows.Forms.Label();
            this.lblAvgComp = new System.Windows.Forms.Label();
            this.lblTotComp = new System.Windows.Forms.Label();
            this.cID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCompleted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBuilding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCreate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAssigned = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAssDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCompDelay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBreakdown = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "View By:";
            // 
            // cmbSelector
            // 
            this.cmbSelector.FormattingEnabled = true;
            this.cmbSelector.Items.AddRange(new object[] {
            "Please select",
            "Status",
            "PM",
            "PA"});
            this.cmbSelector.Location = new System.Drawing.Point(82, 7);
            this.cmbSelector.Name = "cmbSelector";
            this.cmbSelector.Size = new System.Drawing.Size(182, 21);
            this.cmbSelector.TabIndex = 1;
            this.cmbSelector.SelectedIndexChanged += new System.EventHandler(this.cmbSelector_SelectedIndexChanged);
            // 
            // cmbCriteria
            // 
            this.cmbCriteria.FormattingEnabled = true;
            this.cmbCriteria.Items.AddRange(new object[] {
            "Please select",
            "Status",
            "PM",
            "PA"});
            this.cmbCriteria.Location = new System.Drawing.Point(82, 34);
            this.cmbCriteria.Name = "cmbCriteria";
            this.cmbCriteria.Size = new System.Drawing.Size(182, 21);
            this.cmbCriteria.TabIndex = 3;
            this.cmbCriteria.SelectedIndexChanged += new System.EventHandler(this.cmbCriteria_SelectedIndexChanged);
            // 
            // lblSelector
            // 
            this.lblSelector.AutoSize = true;
            this.lblSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelector.Location = new System.Drawing.Point(18, 37);
            this.lblSelector.Name = "lblSelector";
            this.lblSelector.Size = new System.Drawing.Size(47, 13);
            this.lblSelector.TabIndex = 2;
            this.lblSelector.Text = "Status:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cID,
            this.cPM,
            this.cCompleted,
            this.cBuilding,
            this.cStatus,
            this.cCreate,
            this.cAssigned,
            this.cAssDiff,
            this.cEnd,
            this.cCompDelay,
            this.colBreakdown});
            this.dataGridView1.Location = new System.Drawing.Point(21, 61);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1057, 355);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "yyyy/MM/dd";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(309, 34);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(107, 20);
            this.dtStart.TabIndex = 5;
            this.dtStart.ValueChanged += new System.EventHandler(this.dtStart_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(270, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "From:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(422, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "To:";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "yyyy/MM/dd";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(461, 35);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(107, 20);
            this.dtTo.TabIndex = 7;
            this.dtTo.ValueChanged += new System.EventHandler(this.dtTo_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 430);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Total jobs:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 454);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Total Assigned Delay:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 477);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Avg Assigned Delay:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(234, 477);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Avg Completed Delay:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(234, 454);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Total Completed Delay:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(157, 430);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(13, 13);
            this.lblTotal.TabIndex = 14;
            this.lblTotal.Text = "0";
            // 
            // lblTotAss
            // 
            this.lblTotAss.AutoSize = true;
            this.lblTotAss.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotAss.Location = new System.Drawing.Point(157, 454);
            this.lblTotAss.Name = "lblTotAss";
            this.lblTotAss.Size = new System.Drawing.Size(13, 13);
            this.lblTotAss.TabIndex = 15;
            this.lblTotAss.Text = "0";
            // 
            // lblAvgAss
            // 
            this.lblAvgAss.AutoSize = true;
            this.lblAvgAss.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgAss.Location = new System.Drawing.Point(157, 477);
            this.lblAvgAss.Name = "lblAvgAss";
            this.lblAvgAss.Size = new System.Drawing.Size(13, 13);
            this.lblAvgAss.TabIndex = 16;
            this.lblAvgAss.Text = "0";
            // 
            // lblAvgComp
            // 
            this.lblAvgComp.AutoSize = true;
            this.lblAvgComp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgComp.Location = new System.Drawing.Point(379, 477);
            this.lblAvgComp.Name = "lblAvgComp";
            this.lblAvgComp.Size = new System.Drawing.Size(13, 13);
            this.lblAvgComp.TabIndex = 18;
            this.lblAvgComp.Text = "0";
            // 
            // lblTotComp
            // 
            this.lblTotComp.AutoSize = true;
            this.lblTotComp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotComp.Location = new System.Drawing.Point(379, 454);
            this.lblTotComp.Name = "lblTotComp";
            this.lblTotComp.Size = new System.Drawing.Size(13, 13);
            this.lblTotComp.TabIndex = 17;
            this.lblTotComp.Text = "0";
            // 
            // cID
            // 
            this.cID.DataPropertyName = "id";
            this.cID.HeaderText = "Job ID";
            this.cID.Name = "cID";
            this.cID.ReadOnly = true;
            this.cID.Width = 63;
            // 
            // cPM
            // 
            this.cPM.DataPropertyName = "creator";
            this.cPM.HeaderText = "PM";
            this.cPM.Name = "cPM";
            this.cPM.ReadOnly = true;
            this.cPM.Width = 48;
            // 
            // cCompleted
            // 
            this.cCompleted.DataPropertyName = "processor";
            this.cCompleted.HeaderText = "Completed By";
            this.cCompleted.Name = "cCompleted";
            this.cCompleted.ReadOnly = true;
            this.cCompleted.Width = 97;
            // 
            // cBuilding
            // 
            this.cBuilding.DataPropertyName = "buildingCode";
            this.cBuilding.HeaderText = "Building";
            this.cBuilding.Name = "cBuilding";
            this.cBuilding.ReadOnly = true;
            this.cBuilding.Width = 69;
            // 
            // cStatus
            // 
            this.cStatus.DataPropertyName = "status";
            this.cStatus.HeaderText = "Status";
            this.cStatus.Name = "cStatus";
            this.cStatus.ReadOnly = true;
            this.cStatus.Width = 62;
            // 
            // cCreate
            // 
            this.cCreate.DataPropertyName = "createDate";
            this.cCreate.HeaderText = "Created";
            this.cCreate.Name = "cCreate";
            this.cCreate.ReadOnly = true;
            this.cCreate.Width = 69;
            // 
            // cAssigned
            // 
            this.cAssigned.DataPropertyName = "assignedDate";
            this.cAssigned.HeaderText = "Assigned";
            this.cAssigned.Name = "cAssigned";
            this.cAssigned.ReadOnly = true;
            this.cAssigned.Width = 75;
            // 
            // cAssDiff
            // 
            this.cAssDiff.DataPropertyName = "assDiff";
            this.cAssDiff.HeaderText = "Assign Delay";
            this.cAssDiff.Name = "cAssDiff";
            this.cAssDiff.ReadOnly = true;
            this.cAssDiff.Width = 93;
            // 
            // cEnd
            // 
            this.cEnd.DataPropertyName = "completeDate";
            this.cEnd.HeaderText = "Completed";
            this.cEnd.Name = "cEnd";
            this.cEnd.ReadOnly = true;
            this.cEnd.Width = 82;
            // 
            // cCompDelay
            // 
            this.cCompDelay.DataPropertyName = "compDiff";
            this.cCompDelay.HeaderText = "Completed Delay";
            this.cCompDelay.Name = "cCompDelay";
            this.cCompDelay.ReadOnly = true;
            this.cCompDelay.Width = 103;
            // 
            // colBreakdown
            // 
            this.colBreakdown.HeaderText = "Breakdown";
            this.colBreakdown.Name = "colBreakdown";
            this.colBreakdown.Text = "View";
            this.colBreakdown.UseColumnTextForButtonValue = true;
            this.colBreakdown.Width = 67;
            // 
            // usrJobReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAvgComp);
            this.Controls.Add(this.lblTotComp);
            this.Controls.Add(this.lblAvgAss);
            this.Controls.Add(this.lblTotAss);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtStart);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmbCriteria);
            this.Controls.Add(this.lblSelector);
            this.Controls.Add(this.cmbSelector);
            this.Controls.Add(this.label1);
            this.Name = "usrJobReport";
            this.Size = new System.Drawing.Size(1081, 490);
            this.Load += new System.EventHandler(this.usrJobReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSelector;
        private System.Windows.Forms.ComboBox cmbCriteria;
        private System.Windows.Forms.Label lblSelector;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotAss;
        private System.Windows.Forms.Label lblAvgAss;
        private System.Windows.Forms.Label lblAvgComp;
        private System.Windows.Forms.Label lblTotComp;
        private System.Windows.Forms.DataGridViewTextBoxColumn cID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPM;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCompleted;
        private System.Windows.Forms.DataGridViewTextBoxColumn cBuilding;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCreate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAssigned;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAssDiff;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCompDelay;
        private System.Windows.Forms.DataGridViewButtonColumn colBreakdown;
    }
}

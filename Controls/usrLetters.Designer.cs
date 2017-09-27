namespace Astrodon {
    partial class usrLetters {
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
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.customerGrid = new System.Windows.Forms.DataGridView();
            this.accNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OSBAL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reminder = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FinalDemand = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rental = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SummonsPending = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DisconnectNotice = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Disconnect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.HandOver = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBuildings = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.customerGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(717, 480);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(137, 23);
            this.btnPrint.TabIndex = 30;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(860, 480);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(137, 23);
            this.btnProcess.TabIndex = 29;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // customerGrid
            // 
            this.customerGrid.AllowUserToAddRows = false;
            this.customerGrid.AllowUserToDeleteRows = false;
            this.customerGrid.AllowUserToResizeColumns = false;
            this.customerGrid.AllowUserToResizeRows = false;
            this.customerGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.customerGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customerGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.accNum,
            this.accName,
            this.OSBAL,
            this.Reminder,
            this.FinalDemand,
            this.rental,
            this.SummonsPending,
            this.DisconnectNotice,
            this.Disconnect,
            this.HandOver,
            this.Column1,
            this.Column2,
            this.Column3});
            this.customerGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.customerGrid.Location = new System.Drawing.Point(15, 115);
            this.customerGrid.Name = "customerGrid";
            this.customerGrid.Size = new System.Drawing.Size(982, 359);
            this.customerGrid.TabIndex = 28;
            this.customerGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customerGrid_CellContentClick);
            this.customerGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.customerGrid_CellEndEdit);
            this.customerGrid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.customerGrid_DataBindingComplete);
            // 
            // accNum
            // 
            this.accNum.DataPropertyName = "AccNumber";
            this.accNum.HeaderText = "Acc Number";
            this.accNum.Name = "accNum";
            this.accNum.ReadOnly = true;
            // 
            // accName
            // 
            this.accName.DataPropertyName = "AccName";
            this.accName.HeaderText = "Acc Description";
            this.accName.Name = "accName";
            this.accName.ReadOnly = true;
            // 
            // OSBAL
            // 
            this.OSBAL.DataPropertyName = "OSBal";
            this.OSBAL.HeaderText = "O/S Bal";
            this.OSBAL.Name = "OSBAL";
            this.OSBAL.ReadOnly = true;
            // 
            // Reminder
            // 
            this.Reminder.DataPropertyName = "Reminder";
            this.Reminder.HeaderText = "Reminder";
            this.Reminder.Name = "Reminder";
            // 
            // FinalDemand
            // 
            this.FinalDemand.DataPropertyName = "Final";
            this.FinalDemand.HeaderText = "Final Demand";
            this.FinalDemand.Name = "FinalDemand";
            // 
            // rental
            // 
            this.rental.DataPropertyName = "Rental";
            this.rental.HeaderText = "Rental";
            this.rental.Name = "rental";
            // 
            // SummonsPending
            // 
            this.SummonsPending.DataPropertyName = "Summons";
            this.SummonsPending.HeaderText = "Summons Pending";
            this.SummonsPending.Name = "SummonsPending";
            // 
            // DisconnectNotice
            // 
            this.DisconnectNotice.DataPropertyName = "DisconnectNotice";
            this.DisconnectNotice.HeaderText = "Disconnection Notice";
            this.DisconnectNotice.Name = "DisconnectNotice";
            // 
            // Disconnect
            // 
            this.Disconnect.DataPropertyName = "Disconnect";
            this.Disconnect.HeaderText = "Disconnect";
            this.Disconnect.Name = "Disconnect";
            // 
            // HandOver
            // 
            this.HandOver.DataPropertyName = "Handover";
            this.HandOver.HeaderText = "Hand Over";
            this.HandOver.Name = "HandOver";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Journal";
            this.Column1.HeaderText = "Journal";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "JournalAmt";
            this.Column2.HeaderText = "Journal Amt";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "JournalAcc";
            this.Column3.HeaderText = "Journal Acc";
            this.Column3.Name = "Column3";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(173, 79);
            this.dateTimePicker1.MinDate = new System.DateTime(2012, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(118, 20);
            this.dateTimePicker1.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Disconnect Date (if applicable):";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "00-Standard",
            "03-Arrangement",
            "04-Disconnect",
            "05-Legal",
            "07-Trustees"});
            this.cmbCategory.Location = new System.Drawing.Point(173, 42);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(187, 21);
            this.cmbCategory.TabIndex = 25;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Please select category:";
            // 
            // cmbBuildings
            // 
            this.cmbBuildings.FormattingEnabled = true;
            this.cmbBuildings.Location = new System.Drawing.Point(173, 10);
            this.cmbBuildings.Name = "cmbBuildings";
            this.cmbBuildings.Size = new System.Drawing.Size(187, 21);
            this.cmbBuildings.TabIndex = 23;
            this.cmbBuildings.SelectedIndexChanged += new System.EventHandler(this.cmbBuildings_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Please select building:";
            // 
            // usrLetters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.customerGrid);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBuildings);
            this.Controls.Add(this.label1);
            this.Name = "usrLetters";
            this.Size = new System.Drawing.Size(1000, 519);
            this.Load += new System.EventHandler(this.usrLetters_Load);
            this.Leave += new System.EventHandler(this.usrLetters_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.customerGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.DataGridView customerGrid;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBuildings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn accNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn accName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OSBAL;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Reminder;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FinalDemand;
        private System.Windows.Forms.DataGridViewCheckBoxColumn rental;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SummonsPending;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DisconnectNotice;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Disconnect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn HandOver;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}

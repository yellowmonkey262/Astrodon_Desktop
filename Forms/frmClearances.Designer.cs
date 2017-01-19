namespace Astrodon {
    partial class frmClearances {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClearances));
            this.astrodonDataSet1 = new Astrodon.DataSets.AstrodonDataSet1();
            this.tblBuildingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tblBuildingsTableAdapter = new Astrodon.DataSets.AstrodonDataSet1TableAdapters.tblBuildingsTableAdapter();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.dtValid = new System.Windows.Forms.DateTimePicker();
            this.label19 = new System.Windows.Forms.Label();
            this.dtPicker = new System.Windows.Forms.DateTimePicker();
            this.txtFaxNumber = new System.Windows.Forms.TextBox();
            this.txtYourRef = new System.Windows.Forms.TextBox();
            this.txtTrfAttorneys = new System.Windows.Forms.TextBox();
            this.txtPrepared = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.chkRegDate = new System.Windows.Forms.CheckBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtEmailPurchaser = new System.Windows.Forms.TextBox();
            this.txtTelPurchaser = new System.Windows.Forms.TextBox();
            this.txtAddPurchaser = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dtReg = new System.Windows.Forms.DateTimePicker();
            this.txtPurchaser = new System.Windows.Forms.TextBox();
            this.txtSeller = new System.Windows.Forms.TextBox();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.txtComplex = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.dgTransactions = new System.Windows.Forms.DataGridView();
            this.label16 = new System.Windows.Forms.Label();
            this.dgClearance = new System.Windows.Forms.DataGridView();
            this.label18 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSplit = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.chkExClearance = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.txtClearance = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblBuildingsBindingSource)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgClearance)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // astrodonDataSet1
            // 
            this.astrodonDataSet1.DataSetName = "AstrodonDataSet1";
            this.astrodonDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tblBuildingsBindingSource
            // 
            this.tblBuildingsBindingSource.DataMember = "tblBuildings";
            this.tblBuildingsBindingSource.DataSource = this.astrodonDataSet1;
            // 
            // tblBuildingsTableAdapter
            // 
            this.tblBuildingsTableAdapter.ClearBeforeFill = true;
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DataSource = this.tblBuildingsBindingSource;
            this.cmbBuilding.DisplayMember = "Building";
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(135, 6);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(217, 21);
            this.cmbBuilding.TabIndex = 1;
            this.cmbBuilding.ValueMember = "Code";
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.DisplayMember = "Code";
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(135, 33);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(217, 21);
            this.cmbCustomer.TabIndex = 3;
            this.cmbCustomer.ValueMember = "Code";
            this.cmbCustomer.SelectedIndexChanged += new System.EventHandler(this.cmbCustomer_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Please select customer:";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.dtValid);
            this.pnlHeader.Controls.Add(this.label19);
            this.pnlHeader.Controls.Add(this.dtPicker);
            this.pnlHeader.Controls.Add(this.txtFaxNumber);
            this.pnlHeader.Controls.Add(this.txtYourRef);
            this.pnlHeader.Controls.Add(this.txtTrfAttorneys);
            this.pnlHeader.Controls.Add(this.txtPrepared);
            this.pnlHeader.Controls.Add(this.label7);
            this.pnlHeader.Controls.Add(this.label6);
            this.pnlHeader.Controls.Add(this.label5);
            this.pnlHeader.Controls.Add(this.label4);
            this.pnlHeader.Controls.Add(this.label3);
            this.pnlHeader.Location = new System.Drawing.Point(15, 60);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(449, 135);
            this.pnlHeader.TabIndex = 4;
            // 
            // dtValid
            // 
            this.dtValid.CustomFormat = "yyyy/MM/dd";
            this.dtValid.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtValid.Location = new System.Drawing.Point(333, 110);
            this.dtValid.Name = "dtValid";
            this.dtValid.Size = new System.Drawing.Size(103, 20);
            this.dtValid.TabIndex = 12;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(259, 110);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(49, 13);
            this.label19.TabIndex = 11;
            this.label19.Text = "Vaild To:";
            // 
            // dtPicker
            // 
            this.dtPicker.CustomFormat = "yyyy/MM/dd";
            this.dtPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPicker.Location = new System.Drawing.Point(150, 110);
            this.dtPicker.Name = "dtPicker";
            this.dtPicker.Size = new System.Drawing.Size(103, 20);
            this.dtPicker.TabIndex = 10;
            // 
            // txtFaxNumber
            // 
            this.txtFaxNumber.Location = new System.Drawing.Point(150, 85);
            this.txtFaxNumber.Name = "txtFaxNumber";
            this.txtFaxNumber.Size = new System.Drawing.Size(286, 20);
            this.txtFaxNumber.TabIndex = 9;
            // 
            // txtYourRef
            // 
            this.txtYourRef.Location = new System.Drawing.Point(150, 60);
            this.txtYourRef.Name = "txtYourRef";
            this.txtYourRef.Size = new System.Drawing.Size(286, 20);
            this.txtYourRef.TabIndex = 8;
            // 
            // txtTrfAttorneys
            // 
            this.txtTrfAttorneys.Location = new System.Drawing.Point(150, 35);
            this.txtTrfAttorneys.Name = "txtTrfAttorneys";
            this.txtTrfAttorneys.Size = new System.Drawing.Size(286, 20);
            this.txtTrfAttorneys.TabIndex = 7;
            // 
            // txtPrepared
            // 
            this.txtPrepared.Location = new System.Drawing.Point(150, 10);
            this.txtPrepared.Name = "txtPrepared";
            this.txtPrepared.Size = new System.Drawing.Size(286, 20);
            this.txtPrepared.TabIndex = 6;
            this.txtPrepared.Text = "Mientjie Pretorius Tel: (011) 867 3183 ext 117";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Date:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Facsimile Number:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Attention Your Reference:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Transferring Attorneys:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Prepared By:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select building:";
            // 
            // pnlDetail
            // 
            this.pnlDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetail.Controls.Add(this.chkRegDate);
            this.pnlDetail.Controls.Add(this.txtNotes);
            this.pnlDetail.Controls.Add(this.label17);
            this.pnlDetail.Controls.Add(this.txtEmailPurchaser);
            this.pnlDetail.Controls.Add(this.txtTelPurchaser);
            this.pnlDetail.Controls.Add(this.txtAddPurchaser);
            this.pnlDetail.Controls.Add(this.label15);
            this.pnlDetail.Controls.Add(this.label14);
            this.pnlDetail.Controls.Add(this.label13);
            this.pnlDetail.Controls.Add(this.dtReg);
            this.pnlDetail.Controls.Add(this.txtPurchaser);
            this.pnlDetail.Controls.Add(this.txtSeller);
            this.pnlDetail.Controls.Add(this.txtUnit);
            this.pnlDetail.Controls.Add(this.txtComplex);
            this.pnlDetail.Controls.Add(this.label8);
            this.pnlDetail.Controls.Add(this.label9);
            this.pnlDetail.Controls.Add(this.label10);
            this.pnlDetail.Controls.Add(this.label11);
            this.pnlDetail.Controls.Add(this.label12);
            this.pnlDetail.Location = new System.Drawing.Point(15, 201);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(449, 270);
            this.pnlDetail.TabIndex = 5;
            // 
            // chkRegDate
            // 
            this.chkRegDate.AutoSize = true;
            this.chkRegDate.Location = new System.Drawing.Point(259, 192);
            this.chkRegDate.Name = "chkRegDate";
            this.chkRegDate.Size = new System.Drawing.Size(103, 17);
            this.chkRegDate.TabIndex = 18;
            this.chkRegDate.Text = "Unknown / N/A";
            this.chkRegDate.UseVisualStyleBackColor = true;
            this.chkRegDate.CheckedChanged += new System.EventHandler(this.chkRegDate_CheckedChanged);
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(150, 218);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(286, 47);
            this.txtNotes.TabIndex = 14;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(4, 218);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 13);
            this.label17.TabIndex = 17;
            this.label17.Text = "Other Notes:";
            // 
            // txtEmailPurchaser
            // 
            this.txtEmailPurchaser.Location = new System.Drawing.Point(150, 166);
            this.txtEmailPurchaser.Name = "txtEmailPurchaser";
            this.txtEmailPurchaser.Size = new System.Drawing.Size(286, 20);
            this.txtEmailPurchaser.TabIndex = 12;
            // 
            // txtTelPurchaser
            // 
            this.txtTelPurchaser.Location = new System.Drawing.Point(150, 140);
            this.txtTelPurchaser.Name = "txtTelPurchaser";
            this.txtTelPurchaser.Size = new System.Drawing.Size(286, 20);
            this.txtTelPurchaser.TabIndex = 11;
            // 
            // txtAddPurchaser
            // 
            this.txtAddPurchaser.Location = new System.Drawing.Point(150, 114);
            this.txtAddPurchaser.Name = "txtAddPurchaser";
            this.txtAddPurchaser.Size = new System.Drawing.Size(286, 20);
            this.txtAddPurchaser.TabIndex = 10;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 192);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(92, 13);
            this.label15.TabIndex = 13;
            this.label15.Text = "Registration Date:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 140);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(93, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "Tel No Purchaser:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(111, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Address of Purchaser:";
            // 
            // dtReg
            // 
            this.dtReg.CustomFormat = "yyyy/MM/dd";
            this.dtReg.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtReg.Location = new System.Drawing.Point(150, 192);
            this.dtReg.Name = "dtReg";
            this.dtReg.Size = new System.Drawing.Size(103, 20);
            this.dtReg.TabIndex = 13;
            // 
            // txtPurchaser
            // 
            this.txtPurchaser.Location = new System.Drawing.Point(150, 88);
            this.txtPurchaser.Name = "txtPurchaser";
            this.txtPurchaser.Size = new System.Drawing.Size(286, 20);
            this.txtPurchaser.TabIndex = 9;
            // 
            // txtSeller
            // 
            this.txtSeller.Location = new System.Drawing.Point(150, 62);
            this.txtSeller.Name = "txtSeller";
            this.txtSeller.Size = new System.Drawing.Size(286, 20);
            this.txtSeller.TabIndex = 8;
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(150, 36);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(286, 20);
            this.txtUnit.TabIndex = 7;
            // 
            // txtComplex
            // 
            this.txtComplex.Location = new System.Drawing.Point(150, 10);
            this.txtComplex.Name = "txtComplex";
            this.txtComplex.Size = new System.Drawing.Size(286, 20);
            this.txtComplex.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 166);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Email Address Purchaser:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Name of Purchaser:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Name of Seller:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 36);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Unit No:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Complex:";
            // 
            // dgTransactions
            // 
            this.dgTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTransactions.Location = new System.Drawing.Point(470, 76);
            this.dgTransactions.Name = "dgTransactions";
            this.dgTransactions.Size = new System.Drawing.Size(612, 208);
            this.dgTransactions.TabIndex = 500;
            this.dgTransactions.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgTransactions_ColumnHeaderMouseClick);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(470, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(118, 13);
            this.label16.TabIndex = 7;
            this.label16.Text = "Customer Transactions:";
            // 
            // dgClearance
            // 
            this.dgClearance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgClearance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgClearance.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgClearance.Location = new System.Drawing.Point(470, 309);
            this.dgClearance.Name = "dgClearance";
            this.dgClearance.Size = new System.Drawing.Size(609, 162);
            this.dgClearance.TabIndex = 501;
            this.dgClearance.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgClearance_CellValueChanged);
            this.dgClearance.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgClearance_RowsAdded);
            this.dgClearance.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgClearance_RowsRemoved);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(470, 293);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(96, 13);
            this.label18.TabIndex = 502;
            this.label18.Text = "Details of Charges:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtSplit);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.chkExClearance);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnProcess);
            this.panel1.Controls.Add(this.txtTotal);
            this.panel1.Controls.Add(this.txtClearance);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.label27);
            this.panel1.Location = new System.Drawing.Point(15, 473);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(449, 122);
            this.panel1.TabIndex = 503;
            // 
            // txtSplit
            // 
            this.txtSplit.Location = new System.Drawing.Point(222, 36);
            this.txtSplit.Name = "txtSplit";
            this.txtSplit.Size = new System.Drawing.Size(52, 20);
            this.txtSplit.TabIndex = 21;
            this.txtSplit.Text = "390.00";
            this.txtSplit.TextChanged += new System.EventHandler(this.txtSplit_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 36);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(213, 13);
            this.label20.TabIndex = 20;
            this.label20.Text = "Recon split Seller/Buyer date reconciliation:";
            // 
            // chkExClearance
            // 
            this.chkExClearance.AutoSize = true;
            this.chkExClearance.Location = new System.Drawing.Point(280, 13);
            this.chkExClearance.Name = "chkExClearance";
            this.chkExClearance.Size = new System.Drawing.Size(122, 17);
            this.chkExClearance.TabIndex = 19;
            this.chkExClearance.Text = "Extended Clearance";
            this.chkExClearance.UseVisualStyleBackColor = true;
            this.chkExClearance.CheckedChanged += new System.EventHandler(this.chkExClearance_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(361, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(280, 88);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(174, 88);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(100, 23);
            this.btnProcess.TabIndex = 8;
            this.btnProcess.Text = "Save && Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // txtTotal
            // 
            this.txtTotal.Enabled = false;
            this.txtTotal.Location = new System.Drawing.Point(150, 62);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(286, 20);
            this.txtTotal.TabIndex = 7;
            this.txtTotal.TextChanged += new System.EventHandler(this.txtTotal_TextChanged);
            // 
            // txtClearance
            // 
            this.txtClearance.Enabled = false;
            this.txtClearance.Location = new System.Drawing.Point(150, 10);
            this.txtClearance.Name = "txtClearance";
            this.txtClearance.Size = new System.Drawing.Size(124, 20);
            this.txtClearance.TabIndex = 6;
            this.txtClearance.TextChanged += new System.EventHandler(this.txtClearance_TextChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(3, 62);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(114, 13);
            this.label26.TabIndex = 2;
            this.label26.Text = "Total Due to Astrodon:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(3, 10);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(84, 13);
            this.label27.TabIndex = 1;
            this.label27.Text = "Clearance Fees:";
            // 
            // frmClearances
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 619);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.dgClearance);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.dgTransactions);
            this.Controls.Add(this.pnlDetail);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmClearances";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clearances";
            this.Load += new System.EventHandler(this.frmClearances_Load);
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblBuildingsBindingSource)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransactions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgClearance)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Astrodon.DataSets.AstrodonDataSet1 astrodonDataSet1;
        private System.Windows.Forms.BindingSource tblBuildingsBindingSource;
        private Astrodon.DataSets.AstrodonDataSet1TableAdapters.tblBuildingsTableAdapter tblBuildingsTableAdapter;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.DateTimePicker dtPicker;
        private System.Windows.Forms.TextBox txtFaxNumber;
        private System.Windows.Forms.TextBox txtYourRef;
        private System.Windows.Forms.TextBox txtTrfAttorneys;
        private System.Windows.Forms.TextBox txtPrepared;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.TextBox txtEmailPurchaser;
        private System.Windows.Forms.TextBox txtTelPurchaser;
        private System.Windows.Forms.TextBox txtAddPurchaser;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dtReg;
        private System.Windows.Forms.TextBox txtPurchaser;
        private System.Windows.Forms.TextBox txtSeller;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.TextBox txtComplex;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dgTransactions;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.DataGridView dgClearance;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TextBox txtClearance;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.DateTimePicker dtValid;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox chkRegDate;
        private System.Windows.Forms.CheckBox chkExClearance;
        private System.Windows.Forms.TextBox txtSplit;
        private System.Windows.Forms.Label label20;
    }
}
namespace Astrodon.Controls.Maintenance
{
    partial class usrMaintenanceDetail
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
            this.components = new System.ComponentModel.Container();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tbDetail = new System.Windows.Forms.TabPage();
            this.dtpInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.tbInvoiceNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgSupportingDocuments = new System.Windows.Forms.DataGridView();
            this.lblAllowedFiles = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblAttachment = new System.Windows.Forms.Label();
            this.lblSupportingDocuments = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWarrantyNotes = new System.Windows.Forms.TextBox();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.numWarrantyDuration = new System.Windows.Forms.NumericUpDown();
            this.cbWarrantyDurationType = new System.Windows.Forms.ComboBox();
            this.lblWarrantyNotesTitle = new System.Windows.Forms.Label();
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.lblWarrantyExpires = new System.Windows.Forms.Label();
            this.lblWarrantyExpiresTitle = new System.Windows.Forms.Label();
            this.lblWarranty = new System.Windows.Forms.Label();
            this.lblInvoiceDateTitle = new System.Windows.Forms.Label();
            this.lblInvoiceNumberTitle = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblEmailTitle = new System.Windows.Forms.Label();
            this.lblContactNumber = new System.Windows.Forms.Label();
            this.lblContactNumberTitle = new System.Windows.Forms.Label();
            this.lblContactPerson = new System.Windows.Forms.Label();
            this.lblVat = new System.Windows.Forms.Label();
            this.lblContactPersonTitle = new System.Windows.Forms.Label();
            this.lblVatTitle = new System.Windows.Forms.Label();
            this.lblCompanyReg = new System.Windows.Forms.Label();
            this.lblCompanyRegTitle = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.lblSupplierTitle = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblMaintenanceDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblClassification = new System.Windows.Forms.Label();
            this.lblLedgerAccount = new System.Windows.Forms.Label();
            this.lblMaintenanceType = new System.Windows.Forms.Label();
            this.lblBuildingName = new System.Windows.Forms.Label();
            this.lblMaintenanceForTitle = new System.Windows.Forms.Label();
            this.lblTotalAmountTitle = new System.Windows.Forms.Label();
            this.lblClassificationTitle = new System.Windows.Forms.Label();
            this.lblLedgerAccountTitle = new System.Windows.Forms.Label();
            this.lblMaintenanceTypeTitle = new System.Windows.Forms.Label();
            this.lblBuildingNameTitle = new System.Windows.Forms.Label();
            this.lblSupplierDetail = new System.Windows.Forms.Label();
            this.tbUnits = new System.Windows.Forms.TabPage();
            this.dgItems = new System.Windows.Forms.DataGridView();
            this.sfdDownloadAttachment = new System.Windows.Forms.SaveFileDialog();
            this.ofdAttachment = new System.Windows.Forms.OpenFileDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbTotalAmount = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tbMain.SuspendLayout();
            this.tbDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSupportingDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWarrantyDuration)).BeginInit();
            this.tbUnits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tbDetail);
            this.tbMain.Controls.Add(this.tbUnits);
            this.tbMain.Location = new System.Drawing.Point(3, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(843, 562);
            this.tbMain.TabIndex = 0;
            // 
            // tbDetail
            // 
            this.tbDetail.Controls.Add(this.dtpInvoiceDate);
            this.tbDetail.Controls.Add(this.tbInvoiceNumber);
            this.tbDetail.Controls.Add(this.label5);
            this.tbDetail.Controls.Add(this.label4);
            this.tbDetail.Controls.Add(this.dgSupportingDocuments);
            this.tbDetail.Controls.Add(this.lblAllowedFiles);
            this.tbDetail.Controls.Add(this.btnBrowse);
            this.tbDetail.Controls.Add(this.lblAttachment);
            this.tbDetail.Controls.Add(this.lblSupportingDocuments);
            this.tbDetail.Controls.Add(this.label3);
            this.tbDetail.Controls.Add(this.txtWarrantyNotes);
            this.tbDetail.Controls.Add(this.txtSerialNumber);
            this.tbDetail.Controls.Add(this.numWarrantyDuration);
            this.tbDetail.Controls.Add(this.cbWarrantyDurationType);
            this.tbDetail.Controls.Add(this.lblWarrantyNotesTitle);
            this.tbDetail.Controls.Add(this.lblSerialNumber);
            this.tbDetail.Controls.Add(this.lblWarrantyExpires);
            this.tbDetail.Controls.Add(this.lblWarrantyExpiresTitle);
            this.tbDetail.Controls.Add(this.lblWarranty);
            this.tbDetail.Controls.Add(this.lblInvoiceDateTitle);
            this.tbDetail.Controls.Add(this.lblInvoiceNumberTitle);
            this.tbDetail.Controls.Add(this.lblEmail);
            this.tbDetail.Controls.Add(this.lblEmailTitle);
            this.tbDetail.Controls.Add(this.lblContactNumber);
            this.tbDetail.Controls.Add(this.lblContactNumberTitle);
            this.tbDetail.Controls.Add(this.lblContactPerson);
            this.tbDetail.Controls.Add(this.lblVat);
            this.tbDetail.Controls.Add(this.lblContactPersonTitle);
            this.tbDetail.Controls.Add(this.lblVatTitle);
            this.tbDetail.Controls.Add(this.lblCompanyReg);
            this.tbDetail.Controls.Add(this.lblCompanyRegTitle);
            this.tbDetail.Controls.Add(this.lblSupplier);
            this.tbDetail.Controls.Add(this.lblSupplierTitle);
            this.tbDetail.Controls.Add(this.txtDescription);
            this.tbDetail.Controls.Add(this.lblMaintenanceDescription);
            this.tbDetail.Controls.Add(this.label1);
            this.tbDetail.Controls.Add(this.cbUnit);
            this.tbDetail.Controls.Add(this.lblTotalAmount);
            this.tbDetail.Controls.Add(this.lblClassification);
            this.tbDetail.Controls.Add(this.lblLedgerAccount);
            this.tbDetail.Controls.Add(this.lblMaintenanceType);
            this.tbDetail.Controls.Add(this.lblBuildingName);
            this.tbDetail.Controls.Add(this.lblMaintenanceForTitle);
            this.tbDetail.Controls.Add(this.lblTotalAmountTitle);
            this.tbDetail.Controls.Add(this.lblClassificationTitle);
            this.tbDetail.Controls.Add(this.lblLedgerAccountTitle);
            this.tbDetail.Controls.Add(this.lblMaintenanceTypeTitle);
            this.tbDetail.Controls.Add(this.lblBuildingNameTitle);
            this.tbDetail.Controls.Add(this.lblSupplierDetail);
            this.tbDetail.Location = new System.Drawing.Point(4, 22);
            this.tbDetail.Name = "tbDetail";
            this.tbDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tbDetail.Size = new System.Drawing.Size(835, 536);
            this.tbDetail.TabIndex = 0;
            this.tbDetail.Text = "Detail";
            this.tbDetail.UseVisualStyleBackColor = true;
            // 
            // dtpInvoiceDate
            // 
            this.dtpInvoiceDate.CustomFormat = "yyyy/MM/dd";
            this.dtpInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInvoiceDate.Location = new System.Drawing.Point(157, 489);
            this.dtpInvoiceDate.Name = "dtpInvoiceDate";
            this.dtpInvoiceDate.Size = new System.Drawing.Size(194, 20);
            this.dtpInvoiceDate.TabIndex = 109;
            this.dtpInvoiceDate.ValueChanged += new System.EventHandler(this.dtpInvoiceDate_ValueChanged);
            // 
            // tbInvoiceNumber
            // 
            this.tbInvoiceNumber.Location = new System.Drawing.Point(157, 466);
            this.tbInvoiceNumber.Name = "tbInvoiceNumber";
            this.tbInvoiceNumber.Size = new System.Drawing.Size(194, 20);
            this.tbInvoiceNumber.TabIndex = 108;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(-8, 521);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(824, 2);
            this.label5.TabIndex = 107;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(398, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(2, 493);
            this.label4.TabIndex = 106;
            // 
            // dgSupportingDocuments
            // 
            this.dgSupportingDocuments.AllowUserToAddRows = false;
            this.dgSupportingDocuments.AllowUserToDeleteRows = false;
            this.dgSupportingDocuments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgSupportingDocuments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSupportingDocuments.Location = new System.Drawing.Point(427, 285);
            this.dgSupportingDocuments.Name = "dgSupportingDocuments";
            this.dgSupportingDocuments.ReadOnly = true;
            this.dgSupportingDocuments.Size = new System.Drawing.Size(364, 223);
            this.dgSupportingDocuments.TabIndex = 105;
            // 
            // lblAllowedFiles
            // 
            this.lblAllowedFiles.AutoSize = true;
            this.lblAllowedFiles.Location = new System.Drawing.Point(424, 269);
            this.lblAllowedFiles.Name = "lblAllowedFiles";
            this.lblAllowedFiles.Size = new System.Drawing.Size(112, 13);
            this.lblAllowedFiles.TabIndex = 102;
            this.lblAllowedFiles.Text = "Only PDF files allowed";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(570, 235);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 101;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblAttachment
            // 
            this.lblAttachment.AutoSize = true;
            this.lblAttachment.Location = new System.Drawing.Point(424, 240);
            this.lblAttachment.Name = "lblAttachment";
            this.lblAttachment.Size = new System.Drawing.Size(61, 13);
            this.lblAttachment.TabIndex = 100;
            this.lblAttachment.Text = "Attachment";
            // 
            // lblSupportingDocuments
            // 
            this.lblSupportingDocuments.AutoSize = true;
            this.lblSupportingDocuments.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupportingDocuments.Location = new System.Drawing.Point(424, 206);
            this.lblSupportingDocuments.Name = "lblSupportingDocuments";
            this.lblSupportingDocuments.Size = new System.Drawing.Size(153, 15);
            this.lblSupportingDocuments.TabIndex = 99;
            this.lblSupportingDocuments.Text = "Supporting Documents";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(415, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(401, 2);
            this.label3.TabIndex = 98;
            // 
            // txtWarrantyNotes
            // 
            this.txtWarrantyNotes.Location = new System.Drawing.Point(570, 144);
            this.txtWarrantyNotes.Name = "txtWarrantyNotes";
            this.txtWarrantyNotes.Size = new System.Drawing.Size(194, 20);
            this.txtWarrantyNotes.TabIndex = 97;
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.Location = new System.Drawing.Point(570, 118);
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.Size = new System.Drawing.Size(194, 20);
            this.txtSerialNumber.TabIndex = 96;
            // 
            // numWarrantyDuration
            // 
            this.numWarrantyDuration.Location = new System.Drawing.Point(570, 39);
            this.numWarrantyDuration.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numWarrantyDuration.Name = "numWarrantyDuration";
            this.numWarrantyDuration.Size = new System.Drawing.Size(70, 20);
            this.numWarrantyDuration.TabIndex = 95;
            this.numWarrantyDuration.ValueChanged += new System.EventHandler(this.numWarrantyDuration_ValueChanged);
            // 
            // cbWarrantyDurationType
            // 
            this.cbWarrantyDurationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWarrantyDurationType.FormattingEnabled = true;
            this.cbWarrantyDurationType.Location = new System.Drawing.Point(570, 65);
            this.cbWarrantyDurationType.Name = "cbWarrantyDurationType";
            this.cbWarrantyDurationType.Size = new System.Drawing.Size(194, 21);
            this.cbWarrantyDurationType.TabIndex = 94;
            this.cbWarrantyDurationType.SelectedIndexChanged += new System.EventHandler(this.cbWarrantyDurationType_SelectedIndexChanged);
            // 
            // lblWarrantyNotesTitle
            // 
            this.lblWarrantyNotesTitle.AutoSize = true;
            this.lblWarrantyNotesTitle.Location = new System.Drawing.Point(424, 147);
            this.lblWarrantyNotesTitle.Name = "lblWarrantyNotesTitle";
            this.lblWarrantyNotesTitle.Size = new System.Drawing.Size(81, 13);
            this.lblWarrantyNotesTitle.TabIndex = 93;
            this.lblWarrantyNotesTitle.Text = "Warranty Notes";
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.AutoSize = true;
            this.lblSerialNumber.Location = new System.Drawing.Point(424, 121);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(73, 13);
            this.lblSerialNumber.TabIndex = 92;
            this.lblSerialNumber.Text = "Serial Number";
            // 
            // lblWarrantyExpires
            // 
            this.lblWarrantyExpires.AutoSize = true;
            this.lblWarrantyExpires.Location = new System.Drawing.Point(567, 96);
            this.lblWarrantyExpires.Name = "lblWarrantyExpires";
            this.lblWarrantyExpires.Size = new System.Drawing.Size(35, 13);
            this.lblWarrantyExpires.TabIndex = 91;
            this.lblWarrantyExpires.Text = "label7";
            // 
            // lblWarrantyExpiresTitle
            // 
            this.lblWarrantyExpiresTitle.AutoSize = true;
            this.lblWarrantyExpiresTitle.Location = new System.Drawing.Point(424, 96);
            this.lblWarrantyExpiresTitle.Name = "lblWarrantyExpiresTitle";
            this.lblWarrantyExpiresTitle.Size = new System.Drawing.Size(41, 13);
            this.lblWarrantyExpiresTitle.TabIndex = 90;
            this.lblWarrantyExpiresTitle.Text = "Expires";
            // 
            // lblWarranty
            // 
            this.lblWarranty.AutoSize = true;
            this.lblWarranty.Location = new System.Drawing.Point(424, 41);
            this.lblWarranty.Name = "lblWarranty";
            this.lblWarranty.Size = new System.Drawing.Size(50, 13);
            this.lblWarranty.TabIndex = 89;
            this.lblWarranty.Text = "Warrenty";
            // 
            // lblInvoiceDateTitle
            // 
            this.lblInvoiceDateTitle.AutoSize = true;
            this.lblInvoiceDateTitle.Location = new System.Drawing.Point(11, 495);
            this.lblInvoiceDateTitle.Name = "lblInvoiceDateTitle";
            this.lblInvoiceDateTitle.Size = new System.Drawing.Size(68, 13);
            this.lblInvoiceDateTitle.TabIndex = 88;
            this.lblInvoiceDateTitle.Text = "Invoice Date";
            // 
            // lblInvoiceNumberTitle
            // 
            this.lblInvoiceNumberTitle.AutoSize = true;
            this.lblInvoiceNumberTitle.Location = new System.Drawing.Point(11, 469);
            this.lblInvoiceNumberTitle.Name = "lblInvoiceNumberTitle";
            this.lblInvoiceNumberTitle.Size = new System.Drawing.Size(82, 13);
            this.lblInvoiceNumberTitle.TabIndex = 87;
            this.lblInvoiceNumberTitle.Text = "Invoice Number";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(154, 446);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 86;
            this.lblEmail.Text = "label2";
            // 
            // lblEmailTitle
            // 
            this.lblEmailTitle.AutoSize = true;
            this.lblEmailTitle.Location = new System.Drawing.Point(11, 446);
            this.lblEmailTitle.Name = "lblEmailTitle";
            this.lblEmailTitle.Size = new System.Drawing.Size(73, 13);
            this.lblEmailTitle.TabIndex = 85;
            this.lblEmailTitle.Text = "Email Address";
            // 
            // lblContactNumber
            // 
            this.lblContactNumber.AutoSize = true;
            this.lblContactNumber.Location = new System.Drawing.Point(154, 420);
            this.lblContactNumber.Name = "lblContactNumber";
            this.lblContactNumber.Size = new System.Drawing.Size(35, 13);
            this.lblContactNumber.TabIndex = 84;
            this.lblContactNumber.Text = "label2";
            // 
            // lblContactNumberTitle
            // 
            this.lblContactNumberTitle.AutoSize = true;
            this.lblContactNumberTitle.Location = new System.Drawing.Point(11, 420);
            this.lblContactNumberTitle.Name = "lblContactNumberTitle";
            this.lblContactNumberTitle.Size = new System.Drawing.Size(84, 13);
            this.lblContactNumberTitle.TabIndex = 83;
            this.lblContactNumberTitle.Text = "Contact Number";
            // 
            // lblContactPerson
            // 
            this.lblContactPerson.AutoSize = true;
            this.lblContactPerson.Location = new System.Drawing.Point(154, 396);
            this.lblContactPerson.Name = "lblContactPerson";
            this.lblContactPerson.Size = new System.Drawing.Size(35, 13);
            this.lblContactPerson.TabIndex = 82;
            this.lblContactPerson.Text = "label9";
            // 
            // lblVat
            // 
            this.lblVat.AutoSize = true;
            this.lblVat.Location = new System.Drawing.Point(154, 372);
            this.lblVat.Name = "lblVat";
            this.lblVat.Size = new System.Drawing.Size(35, 13);
            this.lblVat.TabIndex = 81;
            this.lblVat.Text = "label8";
            // 
            // lblContactPersonTitle
            // 
            this.lblContactPersonTitle.AutoSize = true;
            this.lblContactPersonTitle.Location = new System.Drawing.Point(11, 396);
            this.lblContactPersonTitle.Name = "lblContactPersonTitle";
            this.lblContactPersonTitle.Size = new System.Drawing.Size(80, 13);
            this.lblContactPersonTitle.TabIndex = 80;
            this.lblContactPersonTitle.Text = "Contact Person";
            // 
            // lblVatTitle
            // 
            this.lblVatTitle.AutoSize = true;
            this.lblVatTitle.Location = new System.Drawing.Point(11, 372);
            this.lblVatTitle.Name = "lblVatTitle";
            this.lblVatTitle.Size = new System.Drawing.Size(38, 13);
            this.lblVatTitle.TabIndex = 79;
            this.lblVatTitle.Text = "VAT #";
            // 
            // lblCompanyReg
            // 
            this.lblCompanyReg.AutoSize = true;
            this.lblCompanyReg.Location = new System.Drawing.Point(154, 347);
            this.lblCompanyReg.Name = "lblCompanyReg";
            this.lblCompanyReg.Size = new System.Drawing.Size(35, 13);
            this.lblCompanyReg.TabIndex = 78;
            this.lblCompanyReg.Text = "label5";
            // 
            // lblCompanyRegTitle
            // 
            this.lblCompanyRegTitle.AutoSize = true;
            this.lblCompanyRegTitle.Location = new System.Drawing.Point(11, 347);
            this.lblCompanyRegTitle.Name = "lblCompanyRegTitle";
            this.lblCompanyRegTitle.Size = new System.Drawing.Size(81, 13);
            this.lblCompanyRegTitle.TabIndex = 77;
            this.lblCompanyRegTitle.Text = "Company Reg#";
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(154, 323);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(35, 13);
            this.lblSupplier.TabIndex = 76;
            this.lblSupplier.Text = "label3";
            // 
            // lblSupplierTitle
            // 
            this.lblSupplierTitle.AutoSize = true;
            this.lblSupplierTitle.Location = new System.Drawing.Point(11, 323);
            this.lblSupplierTitle.Name = "lblSupplierTitle";
            this.lblSupplierTitle.Size = new System.Drawing.Size(45, 13);
            this.lblSupplierTitle.TabIndex = 75;
            this.lblSupplierTitle.Text = "Supplier";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(14, 224);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(337, 86);
            this.txtDescription.TabIndex = 74;
            // 
            // lblMaintenanceDescription
            // 
            this.lblMaintenanceDescription.AutoSize = true;
            this.lblMaintenanceDescription.Location = new System.Drawing.Point(11, 208);
            this.lblMaintenanceDescription.Name = "lblMaintenanceDescription";
            this.lblMaintenanceDescription.Size = new System.Drawing.Size(139, 13);
            this.lblMaintenanceDescription.TabIndex = 73;
            this.lblMaintenanceDescription.Text = "Description Of Maintenance";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(-8, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 2);
            this.label1.TabIndex = 72;
            // 
            // cbUnit
            // 
            this.cbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(160, 150);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(191, 21);
            this.cbUnit.TabIndex = 71;
            this.cbUnit.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Location = new System.Drawing.Point(157, 118);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(35, 13);
            this.lblTotalAmount.TabIndex = 70;
            this.lblTotalAmount.Text = "label1";
            // 
            // lblClassification
            // 
            this.lblClassification.AutoSize = true;
            this.lblClassification.Location = new System.Drawing.Point(157, 94);
            this.lblClassification.Name = "lblClassification";
            this.lblClassification.Size = new System.Drawing.Size(35, 13);
            this.lblClassification.TabIndex = 69;
            this.lblClassification.Text = "label1";
            // 
            // lblLedgerAccount
            // 
            this.lblLedgerAccount.AutoSize = true;
            this.lblLedgerAccount.Location = new System.Drawing.Point(157, 71);
            this.lblLedgerAccount.Name = "lblLedgerAccount";
            this.lblLedgerAccount.Size = new System.Drawing.Size(35, 13);
            this.lblLedgerAccount.TabIndex = 68;
            this.lblLedgerAccount.Text = "label1";
            // 
            // lblMaintenanceType
            // 
            this.lblMaintenanceType.AutoSize = true;
            this.lblMaintenanceType.Location = new System.Drawing.Point(157, 50);
            this.lblMaintenanceType.Name = "lblMaintenanceType";
            this.lblMaintenanceType.Size = new System.Drawing.Size(35, 13);
            this.lblMaintenanceType.TabIndex = 67;
            this.lblMaintenanceType.Text = "label1";
            // 
            // lblBuildingName
            // 
            this.lblBuildingName.AutoSize = true;
            this.lblBuildingName.Location = new System.Drawing.Point(157, 28);
            this.lblBuildingName.Name = "lblBuildingName";
            this.lblBuildingName.Size = new System.Drawing.Size(35, 13);
            this.lblBuildingName.TabIndex = 66;
            this.lblBuildingName.Text = "label1";
            // 
            // lblMaintenanceForTitle
            // 
            this.lblMaintenanceForTitle.AutoSize = true;
            this.lblMaintenanceForTitle.Location = new System.Drawing.Point(11, 153);
            this.lblMaintenanceForTitle.Name = "lblMaintenanceForTitle";
            this.lblMaintenanceForTitle.Size = new System.Drawing.Size(143, 13);
            this.lblMaintenanceForTitle.TabIndex = 65;
            this.lblMaintenanceForTitle.Text = "Who is the maintenance for?";
            // 
            // lblTotalAmountTitle
            // 
            this.lblTotalAmountTitle.AutoSize = true;
            this.lblTotalAmountTitle.Location = new System.Drawing.Point(11, 118);
            this.lblTotalAmountTitle.Name = "lblTotalAmountTitle";
            this.lblTotalAmountTitle.Size = new System.Drawing.Size(70, 13);
            this.lblTotalAmountTitle.TabIndex = 64;
            this.lblTotalAmountTitle.Text = "Total Amount";
            // 
            // lblClassificationTitle
            // 
            this.lblClassificationTitle.AutoSize = true;
            this.lblClassificationTitle.Location = new System.Drawing.Point(11, 94);
            this.lblClassificationTitle.Name = "lblClassificationTitle";
            this.lblClassificationTitle.Size = new System.Drawing.Size(68, 13);
            this.lblClassificationTitle.TabIndex = 63;
            this.lblClassificationTitle.Text = "Classification";
            // 
            // lblLedgerAccountTitle
            // 
            this.lblLedgerAccountTitle.AutoSize = true;
            this.lblLedgerAccountTitle.Location = new System.Drawing.Point(11, 71);
            this.lblLedgerAccountTitle.Name = "lblLedgerAccountTitle";
            this.lblLedgerAccountTitle.Size = new System.Drawing.Size(83, 13);
            this.lblLedgerAccountTitle.TabIndex = 62;
            this.lblLedgerAccountTitle.Text = "Ledger Account";
            // 
            // lblMaintenanceTypeTitle
            // 
            this.lblMaintenanceTypeTitle.AutoSize = true;
            this.lblMaintenanceTypeTitle.Location = new System.Drawing.Point(11, 49);
            this.lblMaintenanceTypeTitle.Name = "lblMaintenanceTypeTitle";
            this.lblMaintenanceTypeTitle.Size = new System.Drawing.Size(96, 13);
            this.lblMaintenanceTypeTitle.TabIndex = 61;
            this.lblMaintenanceTypeTitle.Text = "Maintenance Type";
            // 
            // lblBuildingNameTitle
            // 
            this.lblBuildingNameTitle.AutoSize = true;
            this.lblBuildingNameTitle.Location = new System.Drawing.Point(11, 28);
            this.lblBuildingNameTitle.Name = "lblBuildingNameTitle";
            this.lblBuildingNameTitle.Size = new System.Drawing.Size(75, 13);
            this.lblBuildingNameTitle.TabIndex = 60;
            this.lblBuildingNameTitle.Text = "Building Name";
            // 
            // lblSupplierDetail
            // 
            this.lblSupplierDetail.AutoSize = true;
            this.lblSupplierDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplierDetail.Location = new System.Drawing.Point(11, 8);
            this.lblSupplierDetail.Name = "lblSupplierDetail";
            this.lblSupplierDetail.Size = new System.Drawing.Size(90, 15);
            this.lblSupplierDetail.TabIndex = 59;
            this.lblSupplierDetail.Text = "Maintenance";
            // 
            // tbUnits
            // 
            this.tbUnits.Controls.Add(this.lbTotalAmount);
            this.tbUnits.Controls.Add(this.dgItems);
            this.tbUnits.Location = new System.Drawing.Point(4, 22);
            this.tbUnits.Name = "tbUnits";
            this.tbUnits.Padding = new System.Windows.Forms.Padding(3);
            this.tbUnits.Size = new System.Drawing.Size(835, 536);
            this.tbUnits.TabIndex = 1;
            this.tbUnits.Text = "Split";
            this.tbUnits.UseVisualStyleBackColor = true;
            // 
            // dgItems
            // 
            this.dgItems.AllowUserToAddRows = false;
            this.dgItems.AllowUserToDeleteRows = false;
            this.dgItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItems.Location = new System.Drawing.Point(6, 38);
            this.dgItems.Name = "dgItems";
            this.dgItems.ReadOnly = true;
            this.dgItems.Size = new System.Drawing.Size(823, 492);
            this.dgItems.TabIndex = 70;
            this.dgItems.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgItems_DataBindingComplete);
            this.dgItems.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgItems_DataError);
            // 
            // sfdDownloadAttachment
            // 
            this.sfdDownloadAttachment.Filter = "PDF Files (*.pdf)|*.pdf";
            this.sfdDownloadAttachment.InitialDirectory = "c:\\\\";
            this.sfdDownloadAttachment.RestoreDirectory = true;
            // 
            // ofdAttachment
            // 
            this.ofdAttachment.FileName = "openFileDialog1";
            this.ofdAttachment.Filter = "PDF Files (*.pdf)|*.pdf";
            this.ofdAttachment.InitialDirectory = "c:\\\\";
            this.ofdAttachment.Multiselect = true;
            this.ofdAttachment.RestoreDirectory = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(686, 568);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 106;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(767, 568);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 105;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbTotalAmount
            // 
            this.lbTotalAmount.AutoSize = true;
            this.lbTotalAmount.Location = new System.Drawing.Point(7, 19);
            this.lbTotalAmount.Name = "lbTotalAmount";
            this.lbTotalAmount.Size = new System.Drawing.Size(73, 13);
            this.lbTotalAmount.TabIndex = 71;
            this.lbTotalAmount.Text = "Total Amount:";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // usrMaintenanceDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbMain);
            this.Name = "usrMaintenanceDetail";
            this.Size = new System.Drawing.Size(850, 600);
            this.tbMain.ResumeLayout(false);
            this.tbDetail.ResumeLayout(false);
            this.tbDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSupportingDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWarrantyDuration)).EndInit();
            this.tbUnits.ResumeLayout(false);
            this.tbUnits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tbDetail;
        private System.Windows.Forms.DateTimePicker dtpInvoiceDate;
        private System.Windows.Forms.TextBox tbInvoiceNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgSupportingDocuments;
        private System.Windows.Forms.Label lblAllowedFiles;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblAttachment;
        private System.Windows.Forms.Label lblSupportingDocuments;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWarrantyNotes;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private System.Windows.Forms.NumericUpDown numWarrantyDuration;
        private System.Windows.Forms.ComboBox cbWarrantyDurationType;
        private System.Windows.Forms.Label lblWarrantyNotesTitle;
        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.Label lblWarrantyExpires;
        private System.Windows.Forms.Label lblWarrantyExpiresTitle;
        private System.Windows.Forms.Label lblWarranty;
        private System.Windows.Forms.Label lblInvoiceDateTitle;
        private System.Windows.Forms.Label lblInvoiceNumberTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblEmailTitle;
        private System.Windows.Forms.Label lblContactNumber;
        private System.Windows.Forms.Label lblContactNumberTitle;
        private System.Windows.Forms.Label lblContactPerson;
        private System.Windows.Forms.Label lblVat;
        private System.Windows.Forms.Label lblContactPersonTitle;
        private System.Windows.Forms.Label lblVatTitle;
        private System.Windows.Forms.Label lblCompanyReg;
        private System.Windows.Forms.Label lblCompanyRegTitle;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Label lblSupplierTitle;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblMaintenanceDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblClassification;
        private System.Windows.Forms.Label lblLedgerAccount;
        private System.Windows.Forms.Label lblMaintenanceType;
        private System.Windows.Forms.Label lblBuildingName;
        private System.Windows.Forms.Label lblMaintenanceForTitle;
        private System.Windows.Forms.Label lblTotalAmountTitle;
        private System.Windows.Forms.Label lblClassificationTitle;
        private System.Windows.Forms.Label lblLedgerAccountTitle;
        private System.Windows.Forms.Label lblMaintenanceTypeTitle;
        private System.Windows.Forms.Label lblBuildingNameTitle;
        private System.Windows.Forms.Label lblSupplierDetail;
        private System.Windows.Forms.TabPage tbUnits;
        private System.Windows.Forms.SaveFileDialog sfdDownloadAttachment;
        private System.Windows.Forms.OpenFileDialog ofdAttachment;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgItems;
        private System.Windows.Forms.Label lbTotalAmount;
        private System.Windows.Forms.Timer timer1;
    }
}

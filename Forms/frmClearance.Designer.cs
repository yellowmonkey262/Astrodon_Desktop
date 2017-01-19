namespace Astrodon {
    partial class frmClearance {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClearance));
            this.label1 = new System.Windows.Forms.Label();
            this.dgUnprocessed = new System.Windows.Forms.DataGridView();
            this.processed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgProcessed = new System.Windows.Forms.DataGridView();
            this.bc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hoa = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdateProcessed = new System.Windows.Forms.Button();
            this.idDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.certDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerCodeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.astrodonTotalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trfAttorneysDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.validDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paidDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tblClearances1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.astrodonDataSet1 = new Astrodon.DataSets.AstrodonDataSet1();
            this.tblClearancesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tblClearancesTableAdapter = new Astrodon.DataSets.AstrodonDataSet1TableAdapters.tblClearancesTableAdapter();
            this.tblClearances1TableAdapter = new Astrodon.DataSets.AstrodonDataSet1TableAdapters.tblClearances1TableAdapter();
            this.certDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buildingCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clearances = new Astrodon.DataSets.Clearances();
            this.tblClearancesBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tblClearancesTableAdapter1 = new Astrodon.DataSets.ClearancesTableAdapters.tblClearancesTableAdapter();
            this.journal = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgUnprocessed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProcessed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblClearances1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblClearancesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearances)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblClearancesBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Unprocessed clearances";
            // 
            // dgUnprocessed
            // 
            this.dgUnprocessed.AllowUserToAddRows = false;
            this.dgUnprocessed.AutoGenerateColumns = false;
            this.dgUnprocessed.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgUnprocessed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUnprocessed.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.buildingCodeDataGridViewTextBoxColumn,
            this.customerCodeDataGridViewTextBoxColumn,
            this.certDateDataGridViewTextBoxColumn,
            this.processed,
            this.journal});
            this.dgUnprocessed.DataSource = this.tblClearancesBindingSource1;
            this.dgUnprocessed.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgUnprocessed.Location = new System.Drawing.Point(12, 25);
            this.dgUnprocessed.Name = "dgUnprocessed";
            this.dgUnprocessed.Size = new System.Drawing.Size(949, 206);
            this.dgUnprocessed.TabIndex = 1;
            this.dgUnprocessed.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgUnprocessed_CellMouseClick);
            // 
            // processed
            // 
            this.processed.DataPropertyName = "processed";
            this.processed.HeaderText = "Process";
            this.processed.Name = "processed";
            // 
            // dgProcessed
            // 
            this.dgProcessed.AllowUserToAddRows = false;
            this.dgProcessed.AllowUserToDeleteRows = false;
            this.dgProcessed.AutoGenerateColumns = false;
            this.dgProcessed.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgProcessed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProcessed.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn1,
            this.certDateDataGridViewTextBoxColumn1,
            this.customerCodeDataGridViewTextBoxColumn1,
            this.astrodonTotalDataGridViewTextBoxColumn,
            this.trfAttorneysDataGridViewTextBoxColumn,
            this.validDateDataGridViewTextBoxColumn,
            this.paidDataGridViewCheckBoxColumn,
            this.bc,
            this.hoa});
            this.dgProcessed.DataSource = this.tblClearances1BindingSource;
            this.dgProcessed.Location = new System.Drawing.Point(12, 266);
            this.dgProcessed.Name = "dgProcessed";
            this.dgProcessed.Size = new System.Drawing.Size(949, 251);
            this.dgProcessed.TabIndex = 3;
            this.dgProcessed.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProcessed_CellContentClick);
            this.dgProcessed.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgProcessed_CellMouseClick);
            // 
            // bc
            // 
            this.bc.DataPropertyName = "bc";
            this.bc.HeaderText = "Body Corp Docs";
            this.bc.Name = "bc";
            // 
            // hoa
            // 
            this.hoa.DataPropertyName = "hoa";
            this.hoa.HeaderText = "HOA Docs";
            this.hoa.Name = "hoa";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Processed clearances";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(886, 237);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(750, 237);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(130, 23);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "New Clearance";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Processed clearances";
            // 
            // btnUpdateProcessed
            // 
            this.btnUpdateProcessed.Location = new System.Drawing.Point(886, 523);
            this.btnUpdateProcessed.Name = "btnUpdateProcessed";
            this.btnUpdateProcessed.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateProcessed.TabIndex = 7;
            this.btnUpdateProcessed.Text = "Update";
            this.btnUpdateProcessed.UseVisualStyleBackColor = true;
            this.btnUpdateProcessed.Click += new System.EventHandler(this.btnUpdateProcessed_Click);
            // 
            // idDataGridViewTextBoxColumn1
            // 
            this.idDataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn1.HeaderText = "Certificate ID";
            this.idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            this.idDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // certDateDataGridViewTextBoxColumn1
            // 
            this.certDateDataGridViewTextBoxColumn1.DataPropertyName = "certDate";
            this.certDateDataGridViewTextBoxColumn1.HeaderText = "Date";
            this.certDateDataGridViewTextBoxColumn1.Name = "certDateDataGridViewTextBoxColumn1";
            this.certDateDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // customerCodeDataGridViewTextBoxColumn1
            // 
            this.customerCodeDataGridViewTextBoxColumn1.DataPropertyName = "customerCode";
            this.customerCodeDataGridViewTextBoxColumn1.HeaderText = "Customer";
            this.customerCodeDataGridViewTextBoxColumn1.Name = "customerCodeDataGridViewTextBoxColumn1";
            this.customerCodeDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // astrodonTotalDataGridViewTextBoxColumn
            // 
            this.astrodonTotalDataGridViewTextBoxColumn.DataPropertyName = "astrodonTotal";
            this.astrodonTotalDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.astrodonTotalDataGridViewTextBoxColumn.Name = "astrodonTotalDataGridViewTextBoxColumn";
            this.astrodonTotalDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // trfAttorneysDataGridViewTextBoxColumn
            // 
            this.trfAttorneysDataGridViewTextBoxColumn.DataPropertyName = "trfAttorneys";
            this.trfAttorneysDataGridViewTextBoxColumn.HeaderText = "Attorney";
            this.trfAttorneysDataGridViewTextBoxColumn.Name = "trfAttorneysDataGridViewTextBoxColumn";
            this.trfAttorneysDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // validDateDataGridViewTextBoxColumn
            // 
            this.validDateDataGridViewTextBoxColumn.DataPropertyName = "validDate";
            this.validDateDataGridViewTextBoxColumn.HeaderText = "Valid Date";
            this.validDateDataGridViewTextBoxColumn.Name = "validDateDataGridViewTextBoxColumn";
            this.validDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // paidDataGridViewCheckBoxColumn
            // 
            this.paidDataGridViewCheckBoxColumn.DataPropertyName = "paid";
            this.paidDataGridViewCheckBoxColumn.HeaderText = "Paid";
            this.paidDataGridViewCheckBoxColumn.Name = "paidDataGridViewCheckBoxColumn";
            // 
            // tblClearances1BindingSource
            // 
            this.tblClearances1BindingSource.DataMember = "tblClearances1";
            this.tblClearances1BindingSource.DataSource = this.astrodonDataSet1;
            // 
            // astrodonDataSet1
            // 
            this.astrodonDataSet1.DataSetName = "AstrodonDataSet1";
            this.astrodonDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tblClearancesBindingSource
            // 
            this.tblClearancesBindingSource.DataMember = "tblClearances";
            this.tblClearancesBindingSource.DataSource = this.astrodonDataSet1;
            // 
            // tblClearancesTableAdapter
            // 
            this.tblClearancesTableAdapter.ClearBeforeFill = true;
            // 
            // tblClearances1TableAdapter
            // 
            this.tblClearances1TableAdapter.ClearBeforeFill = true;
            // 
            // certDateDataGridViewTextBoxColumn
            // 
            this.certDateDataGridViewTextBoxColumn.DataPropertyName = "certDate";
            this.certDateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.certDateDataGridViewTextBoxColumn.Name = "certDateDataGridViewTextBoxColumn";
            this.certDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customerCodeDataGridViewTextBoxColumn
            // 
            this.customerCodeDataGridViewTextBoxColumn.DataPropertyName = "customerCode";
            this.customerCodeDataGridViewTextBoxColumn.HeaderText = "Customer";
            this.customerCodeDataGridViewTextBoxColumn.Name = "customerCodeDataGridViewTextBoxColumn";
            this.customerCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // buildingCodeDataGridViewTextBoxColumn
            // 
            this.buildingCodeDataGridViewTextBoxColumn.DataPropertyName = "buildingCode";
            this.buildingCodeDataGridViewTextBoxColumn.HeaderText = "Building";
            this.buildingCodeDataGridViewTextBoxColumn.Name = "buildingCodeDataGridViewTextBoxColumn";
            this.buildingCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Certificate ID";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // clearances
            // 
            this.clearances.DataSetName = "Clearances";
            this.clearances.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tblClearancesBindingSource1
            // 
            this.tblClearancesBindingSource1.DataMember = "tblClearances";
            this.tblClearancesBindingSource1.DataSource = this.clearances;
            // 
            // tblClearancesTableAdapter1
            // 
            this.tblClearancesTableAdapter1.ClearBeforeFill = true;
            // 
            // journal
            // 
            this.journal.DataPropertyName = "journal";
            this.journal.HeaderText = "Process Journal";
            this.journal.Name = "journal";
            // 
            // frmClearance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 551);
            this.Controls.Add(this.btnUpdateProcessed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.dgProcessed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgUnprocessed);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmClearance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clearances";
            this.Load += new System.EventHandler(this.frmClearance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgUnprocessed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProcessed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblClearances1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblClearancesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearances)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblClearancesBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgUnprocessed;
        private System.Windows.Forms.BindingSource tblClearancesBindingSource;
        private Astrodon.DataSets.AstrodonDataSet1 astrodonDataSet1;
        private Astrodon.DataSets.AstrodonDataSet1TableAdapters.tblClearancesTableAdapter tblClearancesTableAdapter;
        private System.Windows.Forms.DataGridView dgProcessed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn processed;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn buildingCodeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button btnUpdateProcessed;
        private System.Windows.Forms.BindingSource tblClearances1BindingSource;
        private Astrodon.DataSets.AstrodonDataSet1TableAdapters.tblClearances1TableAdapter tblClearances1TableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn certDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerCodeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn astrodonTotalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trfAttorneysDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn validDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn paidDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hoa;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buildingCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn certDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn journal;
        private System.Windows.Forms.BindingSource tblClearancesBindingSource1;
        private Astrodon.DataSets.Clearances clearances;
        private Astrodon.DataSets.ClearancesTableAdapters.tblClearancesTableAdapter tblClearancesTableAdapter1;
    }
}
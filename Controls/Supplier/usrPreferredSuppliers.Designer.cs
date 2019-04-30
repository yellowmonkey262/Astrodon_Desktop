namespace Astrodon.Controls.Supplier
{
    partial class usrPreferredSuppliers
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
            this.lblPreferredSuppliers = new System.Windows.Forms.Label();
            this.lblBuildings = new System.Windows.Forms.Label();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.lblSpecialInstructions = new System.Windows.Forms.Label();
            this.txtSpecialInstructions = new System.Windows.Forms.TextBox();
            this.btnSaveInstructions = new System.Windows.Forms.Button();
            this.dgvPreferredSuppliers = new System.Windows.Forms.DataGridView();
            this.Supplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CellNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecialInstructions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSupplierName = new System.Windows.Forms.Label();
            this.lblSupplierNameEdit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreferredSuppliers)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPreferredSuppliers
            // 
            this.lblPreferredSuppliers.AutoSize = true;
            this.lblPreferredSuppliers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblPreferredSuppliers.Location = new System.Drawing.Point(18, 9);
            this.lblPreferredSuppliers.Name = "lblPreferredSuppliers";
            this.lblPreferredSuppliers.Size = new System.Drawing.Size(150, 17);
            this.lblPreferredSuppliers.TabIndex = 0;
            this.lblPreferredSuppliers.Text = "Preferred Suppliers";
            // 
            // lblBuildings
            // 
            this.lblBuildings.AutoSize = true;
            this.lblBuildings.Location = new System.Drawing.Point(19, 46);
            this.lblBuildings.Name = "lblBuildings";
            this.lblBuildings.Size = new System.Drawing.Size(80, 13);
            this.lblBuildings.TabIndex = 1;
            this.lblBuildings.Text = "Select Building:";
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(142, 43);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(212, 21);
            this.cmbBuilding.TabIndex = 2;
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // lblSpecialInstructions
            // 
            this.lblSpecialInstructions.AutoSize = true;
            this.lblSpecialInstructions.Location = new System.Drawing.Point(18, 111);
            this.lblSpecialInstructions.Name = "lblSpecialInstructions";
            this.lblSpecialInstructions.Size = new System.Drawing.Size(102, 13);
            this.lblSpecialInstructions.TabIndex = 3;
            this.lblSpecialInstructions.Text = "Special Instructions:";
            // 
            // txtSpecialInstructions
            // 
            this.txtSpecialInstructions.Location = new System.Drawing.Point(142, 111);
            this.txtSpecialInstructions.Multiline = true;
            this.txtSpecialInstructions.Name = "txtSpecialInstructions";
            this.txtSpecialInstructions.Size = new System.Drawing.Size(337, 86);
            this.txtSpecialInstructions.TabIndex = 75;
            // 
            // btnSaveInstructions
            // 
            this.btnSaveInstructions.Location = new System.Drawing.Point(404, 203);
            this.btnSaveInstructions.Name = "btnSaveInstructions";
            this.btnSaveInstructions.Size = new System.Drawing.Size(75, 23);
            this.btnSaveInstructions.TabIndex = 76;
            this.btnSaveInstructions.Text = "Save";
            this.btnSaveInstructions.UseVisualStyleBackColor = true;
            this.btnSaveInstructions.Click += new System.EventHandler(this.btnSaveInstructions_Click);
            // 
            // dgvPreferredSuppliers
            // 
            this.dgvPreferredSuppliers.AllowUserToAddRows = false;
            this.dgvPreferredSuppliers.AllowUserToDeleteRows = false;
            this.dgvPreferredSuppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPreferredSuppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreferredSuppliers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Supplier,
            this.Contact,
            this.CellNumber,
            this.SpecialInstructions,
            this.Action});
            this.dgvPreferredSuppliers.Location = new System.Drawing.Point(21, 248);
            this.dgvPreferredSuppliers.Name = "dgvPreferredSuppliers";
            this.dgvPreferredSuppliers.ReadOnly = true;
            this.dgvPreferredSuppliers.Size = new System.Drawing.Size(864, 341);
            this.dgvPreferredSuppliers.TabIndex = 77;
            this.dgvPreferredSuppliers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPreferredSuppliers_CellContentClick);
            // 
            // Supplier
            // 
            this.Supplier.HeaderText = "Supplier";
            this.Supplier.Name = "Supplier";
            this.Supplier.ReadOnly = true;
            // 
            // Contact
            // 
            this.Contact.HeaderText = "Contact Person";
            this.Contact.Name = "Contact";
            this.Contact.ReadOnly = true;
            // 
            // CellNumber
            // 
            this.CellNumber.HeaderText = "Cell Number";
            this.CellNumber.Name = "CellNumber";
            this.CellNumber.ReadOnly = true;
            // 
            // SpecialInstructions
            // 
            this.SpecialInstructions.HeaderText = "Special Instructions";
            this.SpecialInstructions.Name = "SpecialInstructions";
            this.SpecialInstructions.ReadOnly = true;
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.Location = new System.Drawing.Point(19, 79);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(79, 13);
            this.lblSupplierName.TabIndex = 78;
            this.lblSupplierName.Text = "Supplier Name:";
            // 
            // lblSupplierNameEdit
            // 
            this.lblSupplierNameEdit.AutoSize = true;
            this.lblSupplierNameEdit.Location = new System.Drawing.Point(142, 79);
            this.lblSupplierNameEdit.Name = "lblSupplierNameEdit";
            this.lblSupplierNameEdit.Size = new System.Drawing.Size(0, 13);
            this.lblSupplierNameEdit.TabIndex = 79;
            // 
            // usrPreferredSuppliers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSupplierNameEdit);
            this.Controls.Add(this.lblSupplierName);
            this.Controls.Add(this.dgvPreferredSuppliers);
            this.Controls.Add(this.btnSaveInstructions);
            this.Controls.Add(this.txtSpecialInstructions);
            this.Controls.Add(this.lblSpecialInstructions);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.lblBuildings);
            this.Controls.Add(this.lblPreferredSuppliers);
            this.Name = "usrPreferredSuppliers";
            this.Size = new System.Drawing.Size(903, 608);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreferredSuppliers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPreferredSuppliers;
        private System.Windows.Forms.Label lblBuildings;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label lblSpecialInstructions;
        private System.Windows.Forms.TextBox txtSpecialInstructions;
        private System.Windows.Forms.Button btnSaveInstructions;
        private System.Windows.Forms.DataGridView dgvPreferredSuppliers;
        private System.Windows.Forms.DataGridViewTextBoxColumn Supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn CellNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecialInstructions;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.Label lblSupplierName;
        private System.Windows.Forms.Label lblSupplierNameEdit;
    }
}

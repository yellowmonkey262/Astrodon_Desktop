namespace Astrodon.Controls.Supplier
{
    partial class usrSupplierLookup
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblCompanyReg = new System.Windows.Forms.Label();
            this.lblContactPerson = new System.Windows.Forms.Label();
            this.lblContactNumber = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtCompanyReg = new System.Windows.Forms.TextBox();
            this.txtContactPerson = new System.Windows.Forms.TextBox();
            this.txtContactNumber = new System.Windows.Forms.TextBox();
            this.dgSuppliers = new System.Windows.Forms.DataGridView();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnNewSupplier = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgSuppliers)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(23, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(126, 17);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Supplier Lookup";
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Location = new System.Drawing.Point(23, 66);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(82, 13);
            this.lblCompanyName.TabIndex = 1;
            this.lblCompanyName.Text = "Company Name";
            // 
            // lblCompanyReg
            // 
            this.lblCompanyReg.AutoSize = true;
            this.lblCompanyReg.Location = new System.Drawing.Point(23, 96);
            this.lblCompanyReg.Name = "lblCompanyReg";
            this.lblCompanyReg.Size = new System.Drawing.Size(81, 13);
            this.lblCompanyReg.TabIndex = 2;
            this.lblCompanyReg.Text = "Company Reg#";
            // 
            // lblContactPerson
            // 
            this.lblContactPerson.AutoSize = true;
            this.lblContactPerson.Location = new System.Drawing.Point(23, 125);
            this.lblContactPerson.Name = "lblContactPerson";
            this.lblContactPerson.Size = new System.Drawing.Size(80, 13);
            this.lblContactPerson.TabIndex = 3;
            this.lblContactPerson.Text = "Contact Person";
            // 
            // lblContactNumber
            // 
            this.lblContactNumber.AutoSize = true;
            this.lblContactNumber.Location = new System.Drawing.Point(23, 154);
            this.lblContactNumber.Name = "lblContactNumber";
            this.lblContactNumber.Size = new System.Drawing.Size(84, 13);
            this.lblContactNumber.TabIndex = 4;
            this.lblContactNumber.Text = "Contact Number";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(129, 63);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(257, 20);
            this.txtCompanyName.TabIndex = 5;
            // 
            // txtCompanyReg
            // 
            this.txtCompanyReg.Location = new System.Drawing.Point(129, 93);
            this.txtCompanyReg.Name = "txtCompanyReg";
            this.txtCompanyReg.Size = new System.Drawing.Size(257, 20);
            this.txtCompanyReg.TabIndex = 6;
            // 
            // txtContactPerson
            // 
            this.txtContactPerson.Location = new System.Drawing.Point(129, 122);
            this.txtContactPerson.Name = "txtContactPerson";
            this.txtContactPerson.Size = new System.Drawing.Size(257, 20);
            this.txtContactPerson.TabIndex = 7;
            // 
            // txtContactNumber
            // 
            this.txtContactNumber.Location = new System.Drawing.Point(129, 151);
            this.txtContactNumber.Name = "txtContactNumber";
            this.txtContactNumber.Size = new System.Drawing.Size(257, 20);
            this.txtContactNumber.TabIndex = 8;
            // 
            // dgSuppliers
            // 
            this.dgSuppliers.AllowUserToAddRows = false;
            this.dgSuppliers.AllowUserToDeleteRows = false;
            this.dgSuppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgSuppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSuppliers.Location = new System.Drawing.Point(26, 190);
            this.dgSuppliers.Name = "dgSuppliers";
            this.dgSuppliers.ReadOnly = true;
            this.dgSuppliers.Size = new System.Drawing.Size(795, 369);
            this.dgSuppliers.TabIndex = 9;
            this.dgSuppliers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgSuppliers_CellContentClick);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(415, 149);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnNewSupplier
            // 
            this.btnNewSupplier.Location = new System.Drawing.Point(496, 149);
            this.btnNewSupplier.Name = "btnNewSupplier";
            this.btnNewSupplier.Size = new System.Drawing.Size(119, 23);
            this.btnNewSupplier.TabIndex = 11;
            this.btnNewSupplier.Text = "New Supplier";
            this.btnNewSupplier.UseVisualStyleBackColor = true;
            this.btnNewSupplier.Visible = false;
            this.btnNewSupplier.Click += new System.EventHandler(this.btnNewSupplier_Click);
            // 
            // usrSupplierLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNewSupplier);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgSuppliers);
            this.Controls.Add(this.txtContactNumber);
            this.Controls.Add(this.txtContactPerson);
            this.Controls.Add(this.txtCompanyReg);
            this.Controls.Add(this.txtCompanyName);
            this.Controls.Add(this.lblContactNumber);
            this.Controls.Add(this.lblContactPerson);
            this.Controls.Add(this.lblCompanyReg);
            this.Controls.Add(this.lblCompanyName);
            this.Controls.Add(this.lblTitle);
            this.Name = "usrSupplierLookup";
            this.Size = new System.Drawing.Size(844, 578);
            ((System.ComponentModel.ISupportInitialize)(this.dgSuppliers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblCompanyReg;
        private System.Windows.Forms.Label lblContactPerson;
        private System.Windows.Forms.Label lblContactNumber;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtCompanyReg;
        private System.Windows.Forms.TextBox txtContactPerson;
        private System.Windows.Forms.TextBox txtContactNumber;
        private System.Windows.Forms.DataGridView dgSuppliers;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnNewSupplier;
    }
}

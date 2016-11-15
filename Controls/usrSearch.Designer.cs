namespace Astrodon.Controls {
    partial class usrSearch {
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
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.colBuild = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Customer Search";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Building:";
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(110, 49);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(252, 21);
            this.cmbBuilding.TabIndex = 2;
            // 
            // txtCustomer
            // 
            this.txtCustomer.Location = new System.Drawing.Point(110, 76);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(252, 20);
            this.txtCustomer.TabIndex = 3;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(110, 102);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(252, 20);
            this.txtEmail.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Customer Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Email Address:";
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBuild,
            this.colAcc,
            this.colDescription,
            this.colEmail});
            this.dgResults.Location = new System.Drawing.Point(18, 166);
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.Size = new System.Drawing.Size(692, 445);
            this.dgResults.TabIndex = 7;
            // 
            // colBuild
            // 
            this.colBuild.DataPropertyName = "building";
            this.colBuild.HeaderText = "Building";
            this.colBuild.Name = "colBuild";
            this.colBuild.ReadOnly = true;
            // 
            // colAcc
            // 
            this.colAcc.DataPropertyName = "acc";
            this.colAcc.HeaderText = "Account Number";
            this.colAcc.Name = "colAcc";
            this.colAcc.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "description";
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "email";
            this.colEmail.HeaderText = "Email Address";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(206, 128);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(287, 128);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(368, 133);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(16, 13);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(368, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "or";
            // 
            // usrSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgResults);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtCustomer);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "usrSearch";
            this.Size = new System.Drawing.Size(724, 637);
            this.Load += new System.EventHandler(this.usrSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBuild;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label5;
    }
}

namespace Astrodon.Controls {
    partial class usrStatementRun {
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
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.rdSent = new System.Windows.Forms.RadioButton();
            this.rdUnsent = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.rdBoth = new System.Windows.Forms.RadioButton();
            this.dgStatements = new System.Windows.Forms.DataGridView();
            this.lblStatements = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgStatements)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select building";
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(132, 16);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(233, 21);
            this.cmbBuilding.TabIndex = 1;
            // 
            // rdSent
            // 
            this.rdSent.AutoSize = true;
            this.rdSent.Location = new System.Drawing.Point(20, 43);
            this.rdSent.Name = "rdSent";
            this.rdSent.Size = new System.Drawing.Size(47, 17);
            this.rdSent.TabIndex = 2;
            this.rdSent.TabStop = true;
            this.rdSent.Text = "Sent";
            this.rdSent.UseVisualStyleBackColor = true;
            // 
            // rdUnsent
            // 
            this.rdUnsent.AutoSize = true;
            this.rdUnsent.Location = new System.Drawing.Point(135, 43);
            this.rdUnsent.Name = "rdUnsent";
            this.rdUnsent.Size = new System.Drawing.Size(59, 17);
            this.rdUnsent.TabIndex = 3;
            this.rdUnsent.TabStop = true;
            this.rdUnsent.Text = "Unsent";
            this.rdUnsent.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Date From";
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "yyyy/MM/dd";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(77, 79);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(103, 20);
            this.dtFrom.TabIndex = 5;
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "yyyy/MM/dd";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(262, 79);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(103, 20);
            this.dtTo.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Date To";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(290, 119);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // rdBoth
            // 
            this.rdBoth.AutoSize = true;
            this.rdBoth.Location = new System.Drawing.Point(262, 43);
            this.rdBoth.Name = "rdBoth";
            this.rdBoth.Size = new System.Drawing.Size(47, 17);
            this.rdBoth.TabIndex = 9;
            this.rdBoth.TabStop = true;
            this.rdBoth.Text = "Both";
            this.rdBoth.UseVisualStyleBackColor = true;
            // 
            // dgStatements
            // 
            this.dgStatements.AllowUserToAddRows = false;
            this.dgStatements.AllowUserToResizeRows = false;
            this.dgStatements.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dgStatements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgStatements.Location = new System.Drawing.Point(20, 157);
            this.dgStatements.Name = "dgStatements";
            this.dgStatements.Size = new System.Drawing.Size(1144, 453);
            this.dgStatements.TabIndex = 10;
            this.dgStatements.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgStatements_UserDeletingRow);
            // 
            // lblStatements
            // 
            this.lblStatements.AutoSize = true;
            this.lblStatements.Location = new System.Drawing.Point(17, 119);
            this.lblStatements.Name = "lblStatements";
            this.lblStatements.Size = new System.Drawing.Size(109, 13);
            this.lblStatements.TabIndex = 11;
            this.lblStatements.Text = "Statement count: ___";
            // 
            // usrStatementRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStatements);
            this.Controls.Add(this.dgStatements);
            this.Controls.Add(this.rdBoth);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rdUnsent);
            this.Controls.Add(this.rdSent);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(5, 5);
            this.Name = "usrStatementRun";
            this.Size = new System.Drawing.Size(1187, 627);
            this.Load += new System.EventHandler(this.usrStatementRun_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgStatements)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.RadioButton rdSent;
        private System.Windows.Forms.RadioButton rdUnsent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton rdBoth;
        private System.Windows.Forms.DataGridView dgStatements;
        private System.Windows.Forms.Label lblStatements;
    }
}

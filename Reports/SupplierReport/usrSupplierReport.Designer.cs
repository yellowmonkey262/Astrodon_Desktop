namespace Astrodon.Reports.SupplierReport
{
    partial class usrSupplierReport
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
            this.cmbFromMonth = new System.Windows.Forms.ComboBox();
            this.cmbFromYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.cmbToMonth = new System.Windows.Forms.ComboBox();
            this.cmbToYear = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbSupplierName = new System.Windows.Forms.Label();
            this.btnFindSupplier = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbFromMonth
            // 
            this.cmbFromMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromMonth.FormattingEnabled = true;
            this.cmbFromMonth.Location = new System.Drawing.Point(151, 12);
            this.cmbFromMonth.Name = "cmbFromMonth";
            this.cmbFromMonth.Size = new System.Drawing.Size(102, 21);
            this.cmbFromMonth.TabIndex = 15;
            // 
            // cmbFromYear
            // 
            this.cmbFromYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromYear.FormattingEnabled = true;
            this.cmbFromYear.Location = new System.Drawing.Point(90, 12);
            this.cmbFromYear.Name = "cmbFromYear";
            this.cmbFromYear.Size = new System.Drawing.Size(55, 21);
            this.cmbFromYear.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "From";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(90, 127);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Run Report";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dlgSave
            // 
            this.dlgSave.CheckPathExists = false;
            this.dlgSave.DefaultExt = "pdf";
            this.dlgSave.Filter = "Adobe PDF files (*.pdf)|*.pdf";
            this.dlgSave.InitialDirectory = "Y:\\";
            this.dlgSave.Title = "Levy Roll Report";
            // 
            // cmbToMonth
            // 
            this.cmbToMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToMonth.FormattingEnabled = true;
            this.cmbToMonth.Location = new System.Drawing.Point(151, 43);
            this.cmbToMonth.Name = "cmbToMonth";
            this.cmbToMonth.Size = new System.Drawing.Size(102, 21);
            this.cmbToMonth.TabIndex = 20;
            // 
            // cmbToYear
            // 
            this.cmbToYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToYear.FormattingEnabled = true;
            this.cmbToYear.Location = new System.Drawing.Point(90, 43);
            this.cmbToYear.Name = "cmbToYear";
            this.cmbToYear.Size = new System.Drawing.Size(55, 21);
            this.cmbToYear.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Building";
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(90, 70);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(241, 21);
            this.cmbBuilding.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Supplier";
            // 
            // lbSupplierName
            // 
            this.lbSupplierName.AutoSize = true;
            this.lbSupplierName.Location = new System.Drawing.Point(87, 101);
            this.lbSupplierName.Name = "lbSupplierName";
            this.lbSupplierName.Size = new System.Drawing.Size(0, 13);
            this.lbSupplierName.TabIndex = 24;
            // 
            // btnFindSupplier
            // 
            this.btnFindSupplier.Location = new System.Drawing.Point(256, 101);
            this.btnFindSupplier.Name = "btnFindSupplier";
            this.btnFindSupplier.Size = new System.Drawing.Size(75, 23);
            this.btnFindSupplier.TabIndex = 26;
            this.btnFindSupplier.Text = "Find";
            this.btnFindSupplier.UseVisualStyleBackColor = true;
            this.btnFindSupplier.Click += new System.EventHandler(this.btnFindSupplier_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(337, 101);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 27;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // usrSupplierReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnFindSupplier);
            this.Controls.Add(this.lbSupplierName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.cmbToMonth);
            this.Controls.Add(this.cmbToYear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbFromMonth);
            this.Controls.Add(this.cmbFromYear);
            this.Controls.Add(this.label1);
            this.Name = "usrSupplierReport";
            this.Size = new System.Drawing.Size(428, 207);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbFromMonth;
        private System.Windows.Forms.ComboBox cmbFromYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.ComboBox cmbToMonth;
        private System.Windows.Forms.ComboBox cmbToYear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbSupplierName;
        private System.Windows.Forms.Button btnFindSupplier;
        private System.Windows.Forms.Button btnClear;
    }
}

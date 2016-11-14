namespace Astrodon {
    partial class usrImports {
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
            this.btnHardImport = new System.Windows.Forms.Button();
            this.txtPeriod = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHardImport
            // 
            this.btnHardImport.Location = new System.Drawing.Point(304, 44);
            this.btnHardImport.Name = "btnHardImport";
            this.btnHardImport.Size = new System.Drawing.Size(75, 23);
            this.btnHardImport.TabIndex = 13;
            this.btnHardImport.Text = "Hard Import";
            this.btnHardImport.UseVisualStyleBackColor = true;
            this.btnHardImport.Visible = false;
            // 
            // txtPeriod
            // 
            this.txtPeriod.Location = new System.Drawing.Point(92, 51);
            this.txtPeriod.Name = "txtPeriod";
            this.txtPeriod.Size = new System.Drawing.Size(76, 20);
            this.txtPeriod.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Period:";
            // 
            // txtProgress
            // 
            this.txtProgress.Location = new System.Drawing.Point(11, 77);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.Size = new System.Drawing.Size(368, 127);
            this.txtProgress.TabIndex = 10;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(304, 19);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 9;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(92, 21);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(206, 20);
            this.txtFileName.TabIndex = 8;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(11, 18);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 7;
            this.btnSelect.Text = "Select File";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // usrImports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnHardImport);
            this.Controls.Add(this.txtPeriod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtProgress);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnSelect);
            this.Name = "usrImports";
            this.Size = new System.Drawing.Size(410, 227);
            this.Load += new System.EventHandler(this.usrImports_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHardImport;
        private System.Windows.Forms.TextBox txtPeriod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnSelect;
    }
}

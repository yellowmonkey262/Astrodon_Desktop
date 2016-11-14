namespace Astrodon {
    partial class usrImportBank {
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
            this.btnUpload = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.txtReconPeriod = new System.Windows.Forms.TextBox();
            this.btnAllocate = new System.Windows.Forms.Button();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(15, 18);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "Upload File";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Trust Recon Period:";
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(15, 47);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(301, 134);
            this.lstFiles.TabIndex = 4;
            // 
            // txtReconPeriod
            // 
            this.txtReconPeriod.Location = new System.Drawing.Point(124, 188);
            this.txtReconPeriod.Name = "txtReconPeriod";
            this.txtReconPeriod.Size = new System.Drawing.Size(66, 20);
            this.txtReconPeriod.TabIndex = 5;
            // 
            // btnAllocate
            // 
            this.btnAllocate.Location = new System.Drawing.Point(196, 186);
            this.btnAllocate.Name = "btnAllocate";
            this.btnAllocate.Size = new System.Drawing.Size(120, 23);
            this.btnAllocate.TabIndex = 6;
            this.btnAllocate.Text = "Allocate";
            this.btnAllocate.UseVisualStyleBackColor = true;
            this.btnAllocate.Click += new System.EventHandler(this.btnAllocate_Click);
            // 
            // txtProgress
            // 
            this.txtProgress.BackColor = System.Drawing.Color.Black;
            this.txtProgress.ForeColor = System.Drawing.Color.White;
            this.txtProgress.Location = new System.Drawing.Point(19, 223);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProgress.Size = new System.Drawing.Size(297, 141);
            this.txtProgress.TabIndex = 7;
            // 
            // usrImportBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtProgress);
            this.Controls.Add(this.btnAllocate);
            this.Controls.Add(this.txtReconPeriod);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnUpload);
            this.Name = "usrImportBank";
            this.Size = new System.Drawing.Size(340, 385);
            this.Load += new System.EventHandler(this.usrImportBank_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.TextBox txtReconPeriod;
        private System.Windows.Forms.Button btnAllocate;
        private System.Windows.Forms.TextBox txtProgress;
    }
}

namespace Astrodon.Controls {
    partial class usrEnvelopes {
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
            this.cmbPaper = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.chkBuildings = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select paper size:";
            // 
            // cmbPaper
            // 
            this.cmbPaper.FormattingEnabled = true;
            this.cmbPaper.Items.AddRange(new object[] {
            "Please select",
            "C5",
            "A4",
            "Standard"});
            this.cmbPaper.Location = new System.Drawing.Point(147, 14);
            this.cmbPaper.Name = "cmbPaper";
            this.cmbPaper.Size = new System.Drawing.Size(121, 21);
            this.cmbPaper.TabIndex = 1;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(299, 14);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // chkBuildings
            // 
            this.chkBuildings.CheckOnClick = true;
            this.chkBuildings.FormattingEnabled = true;
            this.chkBuildings.Location = new System.Drawing.Point(20, 54);
            this.chkBuildings.Name = "chkBuildings";
            this.chkBuildings.Size = new System.Drawing.Size(354, 544);
            this.chkBuildings.TabIndex = 3;
            // 
            // usrEnvelopes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBuildings);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.cmbPaper);
            this.Controls.Add(this.label1);
            this.Name = "usrEnvelopes";
            this.Size = new System.Drawing.Size(389, 620);
            this.Load += new System.EventHandler(this.usrEnvelopes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPaper;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckedListBox chkBuildings;
    }
}

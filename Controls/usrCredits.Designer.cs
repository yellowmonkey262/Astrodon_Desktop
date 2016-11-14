namespace Astrodon {
    partial class usrCredits {
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
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgCredits = new System.Windows.Forms.DataGridView();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgCredits)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(109, 13);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(195, 21);
            this.cmbBuilding.TabIndex = 30;
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Select Building";
            // 
            // dgCredits
            // 
            this.dgCredits.AllowUserToDeleteRows = false;
            this.dgCredits.AllowUserToResizeColumns = false;
            this.dgCredits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgCredits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCredits.Location = new System.Drawing.Point(15, 40);
            this.dgCredits.Name = "dgCredits";
            this.dgCredits.Size = new System.Drawing.Size(562, 257);
            this.dgCredits.TabIndex = 33;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(424, 303);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 34;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(502, 303);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 35;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // usrCredits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dgCredits);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label1);
            this.Name = "usrCredits";
            this.Size = new System.Drawing.Size(597, 342);
            this.Load += new System.EventHandler(this.usrCredits_Load);
            this.Leave += new System.EventHandler(this.usrCredits_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.dgCredits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgCredits;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnProcess;
    }
}

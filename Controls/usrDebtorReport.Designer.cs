namespace Astrodon.Controls {
    partial class usrDebtorReport {
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
            this.dgReport = new System.Windows.Forms.DataGridView();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDebtor = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgReport)).BeginInit();
            this.SuspendLayout();
            // 
            // dgReport
            // 
            this.dgReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgReport.Location = new System.Drawing.Point(14, 41);
            this.dgReport.Name = "dgReport";
            this.dgReport.ReadOnly = true;
            this.dgReport.Size = new System.Drawing.Size(1155, 600);
            this.dgReport.TabIndex = 0;
            this.dgReport.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgReport_DataError);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(1094, 648);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Filter by debtor:";
            // 
            // cmbDebtor
            // 
            this.cmbDebtor.FormattingEnabled = true;
            this.cmbDebtor.Location = new System.Drawing.Point(96, 14);
            this.cmbDebtor.Name = "cmbDebtor";
            this.cmbDebtor.Size = new System.Drawing.Size(289, 21);
            this.cmbDebtor.TabIndex = 3;
            // 
            // usrDebtorReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbDebtor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dgReport);
            this.Name = "usrDebtorReport";
            this.Size = new System.Drawing.Size(1181, 674);
            this.Load += new System.EventHandler(this.usrDebtorReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgReport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDebtor;
    }
}

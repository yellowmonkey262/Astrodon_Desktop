namespace Astrodon.Forms {
    partial class frmBuildingTrans {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.dgTransactions = new System.Windows.Forms.DataGridView();
            this.btnPrint = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransactions)).BeginInit();
            this.SuspendLayout();
            // 
            // dgTransactions
            // 
            this.dgTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTransactions.Location = new System.Drawing.Point(0, 0);
            this.dgTransactions.Name = "dgTransactions";
            this.dgTransactions.Size = new System.Drawing.Size(583, 348);
            this.dgTransactions.TabIndex = 0;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(496, 354);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // frmBuildingTrans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 384);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dgTransactions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBuildingTrans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transactions";
            this.Load += new System.EventHandler(this.frmBuildingTrans_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgTransactions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgTransactions;
        private System.Windows.Forms.Button btnPrint;
    }
}
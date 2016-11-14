namespace Astrodon {
    partial class usrSummaryReport {
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
            this.btnExcel = new System.Windows.Forms.Button();
            this.dgSummary = new System.Windows.Forms.DataGridView();
            this.clCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clBB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clCB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(603, 446);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 5;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // dgSummary
            // 
            this.dgSummary.AllowUserToAddRows = false;
            this.dgSummary.AllowUserToDeleteRows = false;
            this.dgSummary.AllowUserToOrderColumns = true;
            this.dgSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clCode,
            this.clName,
            this.clBB,
            this.clCB,
            this.clDiff});
            this.dgSummary.Location = new System.Drawing.Point(13, 12);
            this.dgSummary.Name = "dgSummary";
            this.dgSummary.Size = new System.Drawing.Size(665, 428);
            this.dgSummary.TabIndex = 4;
            // 
            // clCode
            // 
            this.clCode.DataPropertyName = "TrustCode";
            this.clCode.HeaderText = "Trust Code";
            this.clCode.Name = "clCode";
            this.clCode.ReadOnly = true;
            // 
            // clName
            // 
            this.clName.DataPropertyName = "BuildingName";
            this.clName.HeaderText = "Building Name";
            this.clName.Name = "clName";
            this.clName.ReadOnly = true;
            // 
            // clBB
            // 
            this.clBB.DataPropertyName = "BuildBal";
            this.clBB.HeaderText = "Building Balance";
            this.clBB.Name = "clBB";
            this.clBB.ReadOnly = true;
            // 
            // clCB
            // 
            this.clCB.DataPropertyName = "CentrecBal";
            this.clCB.HeaderText = "Centrec Balance";
            this.clCB.Name = "clCB";
            this.clCB.ReadOnly = true;
            // 
            // clDiff
            // 
            this.clDiff.DataPropertyName = "Difference";
            this.clDiff.HeaderText = "Difference";
            this.clDiff.Name = "clDiff";
            this.clDiff.ReadOnly = true;
            // 
            // usrSummaryReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.dgSummary);
            this.Name = "usrSummaryReport";
            this.Size = new System.Drawing.Size(688, 483);
            this.Load += new System.EventHandler(this.usrSummaryReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgSummary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.DataGridView dgSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn clCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn clName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clBB;
        private System.Windows.Forms.DataGridViewTextBoxColumn clCB;
        private System.Windows.Forms.DataGridViewTextBoxColumn clDiff;
    }
}

namespace Astrodon.Forms
{
    partial class frmWebDocumentAccessLog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.dgMaintenance = new System.Windows.Forms.DataGridView();
            this.lbDocumentTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaintenance)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(594, 371);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgMaintenance
            // 
            this.dgMaintenance.AllowUserToAddRows = false;
            this.dgMaintenance.AllowUserToDeleteRows = false;
            this.dgMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgMaintenance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgMaintenance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMaintenance.Location = new System.Drawing.Point(12, 38);
            this.dgMaintenance.Name = "dgMaintenance";
            this.dgMaintenance.ReadOnly = true;
            this.dgMaintenance.Size = new System.Drawing.Size(657, 327);
            this.dgMaintenance.TabIndex = 10;
            // 
            // lbDocumentTitle
            // 
            this.lbDocumentTitle.AutoSize = true;
            this.lbDocumentTitle.Location = new System.Drawing.Point(13, 13);
            this.lbDocumentTitle.Name = "lbDocumentTitle";
            this.lbDocumentTitle.Size = new System.Drawing.Size(79, 13);
            this.lbDocumentTitle.TabIndex = 11;
            this.lbDocumentTitle.Text = "Document Title";
            // 
            // frmWebDocumentAccessLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 406);
            this.Controls.Add(this.lbDocumentTitle);
            this.Controls.Add(this.dgMaintenance);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmWebDocumentAccessLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Web Document Access Log";
            ((System.ComponentModel.ISupportInitialize)(this.dgMaintenance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgMaintenance;
        private System.Windows.Forms.Label lbDocumentTitle;
    }
}
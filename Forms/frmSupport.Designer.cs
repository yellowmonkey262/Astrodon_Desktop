namespace Astrodon.Forms {
    partial class frmSupport {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSupport));
            this.dgSupportDocs = new System.Windows.Forms.DataGridView();
            this.sptFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sptView = new System.Windows.Forms.DataGridViewButtonColumn();
            this.sptRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgSupportDocs)).BeginInit();
            this.SuspendLayout();
            // 
            // dgSupportDocs
            // 
            this.dgSupportDocs.AllowUserToAddRows = false;
            this.dgSupportDocs.AllowUserToDeleteRows = false;
            this.dgSupportDocs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgSupportDocs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSupportDocs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sptFileName,
            this.sptView,
            this.sptRemove});
            this.dgSupportDocs.Location = new System.Drawing.Point(12, 12);
            this.dgSupportDocs.Name = "dgSupportDocs";
            this.dgSupportDocs.Size = new System.Drawing.Size(697, 219);
            this.dgSupportDocs.TabIndex = 2;
            this.dgSupportDocs.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgSupportDocs_CellContentClick);
            // 
            // sptFileName
            // 
            this.sptFileName.DataPropertyName = "FileName";
            this.sptFileName.HeaderText = "File Name";
            this.sptFileName.Name = "sptFileName";
            // 
            // sptView
            // 
            this.sptView.HeaderText = "";
            this.sptView.Name = "sptView";
            this.sptView.Text = "View";
            this.sptView.UseColumnTextForButtonValue = true;
            // 
            // sptRemove
            // 
            this.sptRemove.HeaderText = "";
            this.sptRemove.Name = "sptRemove";
            this.sptRemove.Text = "Attach";
            this.sptRemove.UseColumnTextForButtonValue = true;
            // 
            // frmSupport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 244);
            this.Controls.Add(this.dgSupportDocs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSupport";
            this.Text = "Supporting Documents";
            this.Load += new System.EventHandler(this.frmSupport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgSupportDocs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgSupportDocs;
        private System.Windows.Forms.DataGridViewTextBoxColumn sptFileName;
        private System.Windows.Forms.DataGridViewButtonColumn sptView;
        private System.Windows.Forms.DataGridViewButtonColumn sptRemove;
    }
}
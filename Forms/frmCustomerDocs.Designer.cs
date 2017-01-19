namespace Astrodon.Forms {
    partial class frmCustomerDocs {
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
            this.dgDocs = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPurge = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgDocs)).BeginInit();
            this.SuspendLayout();
            // 
            // dgDocs
            // 
            this.dgDocs.AllowUserToAddRows = false;
            this.dgDocs.AllowUserToDeleteRows = false;
            this.dgDocs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgDocs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDocs.Location = new System.Drawing.Point(12, 25);
            this.dgDocs.Name = "dgDocs";
            this.dgDocs.Size = new System.Drawing.Size(838, 550);
            this.dgDocs.TabIndex = 0;
            this.dgDocs.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgDocs_CellMouseClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 581);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(227, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Deleted Selected";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPurge
            // 
            this.btnPurge.Location = new System.Drawing.Point(623, 581);
            this.btnPurge.Name = "btnPurge";
            this.btnPurge.Size = new System.Drawing.Size(227, 23);
            this.btnPurge.TabIndex = 2;
            this.btnPurge.Text = "Purge files older than 16 months";
            this.btnPurge.UseVisualStyleBackColor = true;
            this.btnPurge.Click += new System.EventHandler(this.btnPurge_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Right click title to view file";
            // 
            // frmCustomerDocs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 607);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPurge);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dgDocs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerDocs";
            this.Text = "Customer Files";
            this.Load += new System.EventHandler(this.frmCustomerDocs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgDocs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgDocs;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPurge;
        private System.Windows.Forms.Label label1;
    }
}
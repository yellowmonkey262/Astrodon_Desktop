namespace Astrodon {
    partial class usrClearance {
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
            this.btnUpdateProcessed = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.dgProcessed = new System.Windows.Forms.DataGridView();
            this.bc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hoa = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgUnprocessed = new System.Windows.Forms.DataGridView();
            this.processed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.journal = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgProcessed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgUnprocessed)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpdateProcessed
            // 
            this.btnUpdateProcessed.Location = new System.Drawing.Point(888, 525);
            this.btnUpdateProcessed.Name = "btnUpdateProcessed";
            this.btnUpdateProcessed.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateProcessed.TabIndex = 14;
            this.btnUpdateProcessed.Text = "Update";
            this.btnUpdateProcessed.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Processed clearances";
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(752, 239);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(130, 23);
            this.btnNew.TabIndex = 12;
            this.btnNew.Text = "New Clearance";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(888, 239);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // dgProcessed
            // 
            this.dgProcessed.AllowUserToAddRows = false;
            this.dgProcessed.AllowUserToDeleteRows = false;
            this.dgProcessed.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgProcessed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProcessed.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bc,
            this.hoa});
            this.dgProcessed.Location = new System.Drawing.Point(14, 268);
            this.dgProcessed.Name = "dgProcessed";
            this.dgProcessed.Size = new System.Drawing.Size(949, 251);
            this.dgProcessed.TabIndex = 10;
            // 
            // bc
            // 
            this.bc.DataPropertyName = "bc";
            this.bc.HeaderText = "Body Corp Docs";
            this.bc.Name = "bc";
            // 
            // hoa
            // 
            this.hoa.DataPropertyName = "hoa";
            this.hoa.HeaderText = "HOA Docs";
            this.hoa.Name = "hoa";
            // 
            // dgUnprocessed
            // 
            this.dgUnprocessed.AllowUserToAddRows = false;
            this.dgUnprocessed.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgUnprocessed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUnprocessed.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.processed,
            this.journal});
            this.dgUnprocessed.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgUnprocessed.Location = new System.Drawing.Point(14, 27);
            this.dgUnprocessed.Name = "dgUnprocessed";
            this.dgUnprocessed.Size = new System.Drawing.Size(949, 206);
            this.dgUnprocessed.TabIndex = 9;
            // 
            // processed
            // 
            this.processed.DataPropertyName = "processed";
            this.processed.HeaderText = "Process";
            this.processed.Name = "processed";
            // 
            // journal
            // 
            this.journal.DataPropertyName = "journal";
            this.journal.HeaderText = "Process Journal";
            this.journal.Name = "journal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Unprocessed clearances";
            // 
            // usrClearance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnUpdateProcessed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.dgProcessed);
            this.Controls.Add(this.dgUnprocessed);
            this.Controls.Add(this.label1);
            this.Name = "usrClearance";
            this.Size = new System.Drawing.Size(988, 563);
            this.Load += new System.EventHandler(this.usrClearance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgProcessed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgUnprocessed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdateProcessed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.DataGridView dgProcessed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hoa;
        private System.Windows.Forms.DataGridView dgUnprocessed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn processed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn journal;
        private System.Windows.Forms.Label label1;
    }
}

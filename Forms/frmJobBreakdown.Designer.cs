namespace Astrodon.Forms {
    partial class frmJobBreakdown {
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDelay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cUser,
            this.cStatus,
            this.cDate,
            this.cDelay});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(682, 350);
            this.dataGridView1.TabIndex = 0;
            // 
            // cUser
            // 
            this.cUser.DataPropertyName = "user";
            this.cUser.HeaderText = "Actioned By";
            this.cUser.Name = "cUser";
            this.cUser.ReadOnly = true;
            // 
            // cStatus
            // 
            this.cStatus.DataPropertyName = "status";
            this.cStatus.HeaderText = "Status";
            this.cStatus.Name = "cStatus";
            this.cStatus.ReadOnly = true;
            // 
            // cDate
            // 
            this.cDate.DataPropertyName = "date";
            this.cDate.HeaderText = "Date";
            this.cDate.Name = "cDate";
            this.cDate.ReadOnly = true;
            // 
            // cDelay
            // 
            this.cDelay.DataPropertyName = "delay";
            this.cDelay.HeaderText = "Delay";
            this.cDelay.Name = "cDelay";
            this.cDelay.ReadOnly = true;
            // 
            // frmJobBreakdown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 350);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmJobBreakdown";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Job Breakdown";
            this.Load += new System.EventHandler(this.frmJobBreakdown_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDelay;
    }
}
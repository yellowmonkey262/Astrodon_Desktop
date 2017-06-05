namespace Astrodon.Controls.Requisitions
{
    partial class usrRequisitionBatch
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.tbBatches = new System.Windows.Forms.TabControl();
            this.tbNewBatch = new System.Windows.Forms.TabPage();
            this.lbProcessing = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgPendingTransactions = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.tbBatchesTab = new System.Windows.Forms.TabPage();
            this.dgItems = new System.Windows.Forms.DataGridView();
            this.btnDownload = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.tbBatches.SuspendLayout();
            this.tbNewBatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPendingTransactions)).BeginInit();
            this.tbBatchesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
            this.SuspendLayout();
            // 
            // dlgSave
            // 
            this.dlgSave.CheckPathExists = false;
            this.dlgSave.DefaultExt = "pdf";
            this.dlgSave.Filter = "Adobe PDF files (*.pdf)|*.pdf";
            this.dlgSave.InitialDirectory = "Y:\\";
            this.dlgSave.Title = "Levy Roll Report";
            // 
            // tbBatches
            // 
            this.tbBatches.Controls.Add(this.tbNewBatch);
            this.tbBatches.Controls.Add(this.tbBatchesTab);
            this.tbBatches.Location = new System.Drawing.Point(6, 3);
            this.tbBatches.Name = "tbBatches";
            this.tbBatches.SelectedIndex = 0;
            this.tbBatches.Size = new System.Drawing.Size(948, 512);
            this.tbBatches.TabIndex = 63;
            // 
            // tbNewBatch
            // 
            this.tbNewBatch.Controls.Add(this.lbProcessing);
            this.tbNewBatch.Controls.Add(this.label2);
            this.tbNewBatch.Controls.Add(this.label1);
            this.tbNewBatch.Controls.Add(this.dgPendingTransactions);
            this.tbNewBatch.Controls.Add(this.button1);
            this.tbNewBatch.Location = new System.Drawing.Point(4, 22);
            this.tbNewBatch.Name = "tbNewBatch";
            this.tbNewBatch.Padding = new System.Windows.Forms.Padding(3);
            this.tbNewBatch.Size = new System.Drawing.Size(940, 486);
            this.tbNewBatch.TabIndex = 0;
            this.tbNewBatch.Text = "New Batch";
            this.tbNewBatch.UseVisualStyleBackColor = true;
            // 
            // lbProcessing
            // 
            this.lbProcessing.AutoSize = true;
            this.lbProcessing.Location = new System.Drawing.Point(138, 35);
            this.lbProcessing.Name = "lbProcessing";
            this.lbProcessing.Size = new System.Drawing.Size(77, 13);
            this.lbProcessing.TabIndex = 71;
            this.lbProcessing.Text = "XXXXXXXXXX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(289, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "Please click on the Create Batch button to create this batch";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 13);
            this.label1.TabIndex = 69;
            this.label1.Text = "Below are the requisitons pending processing.";
            // 
            // dgPendingTransactions
            // 
            this.dgPendingTransactions.AllowUserToAddRows = false;
            this.dgPendingTransactions.AllowUserToDeleteRows = false;
            this.dgPendingTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgPendingTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPendingTransactions.Location = new System.Drawing.Point(13, 59);
            this.dgPendingTransactions.Name = "dgPendingTransactions";
            this.dgPendingTransactions.ReadOnly = true;
            this.dgPendingTransactions.Size = new System.Drawing.Size(921, 421);
            this.dgPendingTransactions.TabIndex = 68;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 62;
            this.button1.Text = "Create PM Batch";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbBatchesTab
            // 
            this.tbBatchesTab.Controls.Add(this.dgItems);
            this.tbBatchesTab.Controls.Add(this.btnDownload);
            this.tbBatchesTab.Controls.Add(this.label3);
            this.tbBatchesTab.Controls.Add(this.cmbBuilding);
            this.tbBatchesTab.Location = new System.Drawing.Point(4, 22);
            this.tbBatchesTab.Name = "tbBatchesTab";
            this.tbBatchesTab.Padding = new System.Windows.Forms.Padding(3);
            this.tbBatchesTab.Size = new System.Drawing.Size(940, 486);
            this.tbBatchesTab.TabIndex = 1;
            this.tbBatchesTab.Text = "Batch History";
            this.tbBatchesTab.UseVisualStyleBackColor = true;
            // 
            // dgItems
            // 
            this.dgItems.AllowUserToAddRows = false;
            this.dgItems.AllowUserToDeleteRows = false;
            this.dgItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItems.Location = new System.Drawing.Point(18, 49);
            this.dgItems.Name = "dgItems";
            this.dgItems.ReadOnly = true;
            this.dgItems.Size = new System.Drawing.Size(916, 431);
            this.dgItems.TabIndex = 67;
            this.dgItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgItems_CellContentClick);
            // 
            // btnDownload
            // 
            this.btnDownload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDownload.Location = new System.Drawing.Point(369, 20);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(144, 23);
            this.btnDownload.TabIndex = 66;
            this.btnDownload.Text = "Create Requisiton Batch";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Visible = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 64;
            this.label3.Text = "Building";
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(122, 20);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(241, 21);
            this.cmbBuilding.TabIndex = 65;
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // usrRequisitionBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbBatches);
            this.Name = "usrRequisitionBatch";
            this.Size = new System.Drawing.Size(957, 528);
            this.tbBatches.ResumeLayout(false);
            this.tbNewBatch.ResumeLayout(false);
            this.tbNewBatch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPendingTransactions)).EndInit();
            this.tbBatchesTab.ResumeLayout(false);
            this.tbBatchesTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.TabControl tbBatches;
        private System.Windows.Forms.TabPage tbNewBatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgPendingTransactions;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tbBatchesTab;
        private System.Windows.Forms.DataGridView dgItems;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label lbProcessing;
    }
}

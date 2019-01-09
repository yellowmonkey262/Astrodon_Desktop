namespace Astrodon.Reports.BuildingPMDebtor
{
    partial class ucBuildingPMDebtorList
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
            this.buildingPMDebtorGridView = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.buildingPMDebtorGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buildingPMDebtorGridView
            // 
            this.buildingPMDebtorGridView.AllowUserToAddRows = false;
            this.buildingPMDebtorGridView.AllowUserToDeleteRows = false;
            this.buildingPMDebtorGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buildingPMDebtorGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.buildingPMDebtorGridView.Location = new System.Drawing.Point(3, 3);
            this.buildingPMDebtorGridView.Name = "buildingPMDebtorGridView";
            this.buildingPMDebtorGridView.Size = new System.Drawing.Size(947, 630);
            this.buildingPMDebtorGridView.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(823, 639);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(127, 22);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ucBuildingPMDebtorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.buildingPMDebtorGridView);
            this.Name = "ucBuildingPMDebtorList";
            this.Size = new System.Drawing.Size(953, 672);
            ((System.ComponentModel.ISupportInitialize)(this.buildingPMDebtorGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView buildingPMDebtorGridView;
        private System.Windows.Forms.Button btnExport;
    }
}

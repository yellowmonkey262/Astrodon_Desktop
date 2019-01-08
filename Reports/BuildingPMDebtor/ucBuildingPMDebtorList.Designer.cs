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
            ((System.ComponentModel.ISupportInitialize)(this.buildingPMDebtorGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buildingPMDebtorGridView
            // 
            this.buildingPMDebtorGridView.AllowUserToAddRows = false;
            this.buildingPMDebtorGridView.AllowUserToDeleteRows = false;
            this.buildingPMDebtorGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.buildingPMDebtorGridView.Location = new System.Drawing.Point(3, 3);
            this.buildingPMDebtorGridView.Name = "buildingPMDebtorGridView";
            this.buildingPMDebtorGridView.Size = new System.Drawing.Size(947, 652);
            this.buildingPMDebtorGridView.TabIndex = 0;
            // 
            // ucBuildingPMDebtorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buildingPMDebtorGridView);
            this.Name = "ucBuildingPMDebtorList";
            this.Size = new System.Drawing.Size(953, 658);
            ((System.ComponentModel.ISupportInitialize)(this.buildingPMDebtorGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView buildingPMDebtorGridView;
    }
}

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
            this.cbPMDropDown = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDebtorDropDown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.buildingPMDebtorGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.buildingPMDebtorGridView.Location = new System.Drawing.Point(3, 63);
            this.buildingPMDebtorGridView.Name = "buildingPMDebtorGridView";
            this.buildingPMDebtorGridView.Size = new System.Drawing.Size(947, 570);
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
            // cbPMDropDown
            // 
            this.cbPMDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPMDropDown.FormattingEnabled = true;
            this.cbPMDropDown.Location = new System.Drawing.Point(41, 27);
            this.cbPMDropDown.Name = "cbPMDropDown";
            this.cbPMDropDown.Size = new System.Drawing.Size(225, 21);
            this.cbPMDropDown.TabIndex = 2;
            this.cbPMDropDown.SelectedValueChanged += new System.EventHandler(this.cbPMDropDown_SelectedValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbDebtorDropDown);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbPMDropDown);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(601, 54);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Debtor:";
            // 
            // cbDebtorDropDown
            // 
            this.cbDebtorDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDebtorDropDown.FormattingEnabled = true;
            this.cbDebtorDropDown.Location = new System.Drawing.Point(356, 27);
            this.cbDebtorDropDown.Name = "cbDebtorDropDown";
            this.cbDebtorDropDown.Size = new System.Drawing.Size(225, 21);
            this.cbDebtorDropDown.TabIndex = 4;
            this.cbDebtorDropDown.SelectedValueChanged += new System.EventHandler(this.cbDebtorDropDown_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "PM:";
            // 
            // ucBuildingPMDebtorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.buildingPMDebtorGridView);
            this.Name = "ucBuildingPMDebtorList";
            this.Size = new System.Drawing.Size(953, 672);
            ((System.ComponentModel.ISupportInitialize)(this.buildingPMDebtorGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView buildingPMDebtorGridView;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ComboBox cbPMDropDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDebtorDropDown;
        private System.Windows.Forms.Label label1;
    }
}

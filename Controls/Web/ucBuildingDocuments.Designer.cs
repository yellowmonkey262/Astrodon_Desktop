namespace Astrodon.Controls.Web
{
    partial class ucBuildingDocuments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucBuildingDocuments));
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fdOpen = new System.Windows.Forms.OpenFileDialog();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tbDocuments = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.cbDocumentCategory = new System.Windows.Forms.ComboBox();
            this.dgItems = new System.Windows.Forms.DataGridView();
            this.tbPreview = new System.Windows.Forms.TabPage();
            this.axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            this.openImage = new System.Windows.Forms.OpenFileDialog();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.picBuilding = new System.Windows.Forms.PictureBox();
            this.tbMain.SuspendLayout();
            this.tbDocuments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
            this.tbPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuilding)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(107, 15);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(233, 21);
            this.cmbBuilding.TabIndex = 75;
            this.cmbBuilding.SelectedIndexChanged += new System.EventHandler(this.cmbBuilding_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 74;
            this.label3.Text = "Building";
            // 
            // fdOpen
            // 
            this.fdOpen.Filter = "Adobe PDF files (*.pdf)|*.pdf";
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tbDocuments);
            this.tbMain.Controls.Add(this.tbPreview);
            this.tbMain.Location = new System.Drawing.Point(3, 42);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(995, 651);
            this.tbMain.TabIndex = 79;
            // 
            // tbDocuments
            // 
            this.tbDocuments.Controls.Add(this.btnLoadImage);
            this.tbDocuments.Controls.Add(this.picBuilding);
            this.tbDocuments.Controls.Add(this.label2);
            this.tbDocuments.Controls.Add(this.btnSelectFile);
            this.tbDocuments.Controls.Add(this.label1);
            this.tbDocuments.Controls.Add(this.tbTitle);
            this.tbDocuments.Controls.Add(this.cbDocumentCategory);
            this.tbDocuments.Controls.Add(this.dgItems);
            this.tbDocuments.Location = new System.Drawing.Point(4, 22);
            this.tbDocuments.Name = "tbDocuments";
            this.tbDocuments.Padding = new System.Windows.Forms.Padding(3);
            this.tbDocuments.Size = new System.Drawing.Size(987, 625);
            this.tbDocuments.TabIndex = 0;
            this.tbDocuments.Text = "Documents";
            this.tbDocuments.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 15);
            this.label2.TabIndex = 84;
            this.label2.Text = "Title";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(127, 84);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(287, 23);
            this.btnSelectFile.TabIndex = 83;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 15);
            this.label1.TabIndex = 82;
            this.label1.Text = "Document Type";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(127, 48);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(287, 20);
            this.tbTitle.TabIndex = 81;
            // 
            // cbDocumentCategory
            // 
            this.cbDocumentCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocumentCategory.FormattingEnabled = true;
            this.cbDocumentCategory.Location = new System.Drawing.Point(127, 12);
            this.cbDocumentCategory.Name = "cbDocumentCategory";
            this.cbDocumentCategory.Size = new System.Drawing.Size(287, 21);
            this.cbDocumentCategory.TabIndex = 80;
            // 
            // dgItems
            // 
            this.dgItems.AllowUserToAddRows = false;
            this.dgItems.AllowUserToDeleteRows = false;
            this.dgItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItems.Location = new System.Drawing.Point(10, 113);
            this.dgItems.Name = "dgItems";
            this.dgItems.ReadOnly = true;
            this.dgItems.Size = new System.Drawing.Size(971, 506);
            this.dgItems.TabIndex = 74;
            this.dgItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgItems_CellContentClick);
            // 
            // tbPreview
            // 
            this.tbPreview.Controls.Add(this.axAcroPDF1);
            this.tbPreview.Location = new System.Drawing.Point(4, 22);
            this.tbPreview.Name = "tbPreview";
            this.tbPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tbPreview.Size = new System.Drawing.Size(987, 625);
            this.tbPreview.TabIndex = 1;
            this.tbPreview.Text = "Preview";
            this.tbPreview.UseVisualStyleBackColor = true;
            // 
            // axAcroPDF1
            // 
            this.axAcroPDF1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axAcroPDF1.Enabled = true;
            this.axAcroPDF1.Location = new System.Drawing.Point(3, 3);
            this.axAcroPDF1.Name = "axAcroPDF1";
            this.axAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
            this.axAcroPDF1.Size = new System.Drawing.Size(978, 616);
            this.axAcroPDF1.TabIndex = 76;
            // 
            // openImage
            // 
            this.openImage.Filter = "Images (*.jpg)|*.jpg";
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(493, 84);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(109, 23);
            this.btnLoadImage.TabIndex = 86;
            this.btnLoadImage.Text = "Load Image";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // picBuilding
            // 
            this.picBuilding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picBuilding.Location = new System.Drawing.Point(608, 3);
            this.picBuilding.Name = "picBuilding";
            this.picBuilding.Size = new System.Drawing.Size(373, 104);
            this.picBuilding.TabIndex = 85;
            this.picBuilding.TabStop = false;
            // 
            // ucBuildingDocuments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbMain);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.label3);
            this.Name = "ucBuildingDocuments";
            this.Size = new System.Drawing.Size(1008, 693);
            this.tbMain.ResumeLayout(false);
            this.tbDocuments.ResumeLayout(false);
            this.tbDocuments.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
            this.tbPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuilding)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog fdOpen;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tbDocuments;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.ComboBox cbDocumentCategory;
        private System.Windows.Forms.DataGridView dgItems;
        private System.Windows.Forms.TabPage tbPreview;
        private AxAcroPDFLib.AxAcroPDF axAcroPDF1;
        private System.Windows.Forms.OpenFileDialog openImage;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.PictureBox picBuilding;
    }
}

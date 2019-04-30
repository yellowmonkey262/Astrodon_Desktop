namespace Astrodon.Forms
{
    partial class frmPDFView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPDFView));
            this.axAcroPDFNew = new AxAcroPDFLib.AxAcroPDF();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDFNew)).BeginInit();
            this.SuspendLayout();
            // 
            // axAcroPDFNew
            // 
            this.axAcroPDFNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axAcroPDFNew.Enabled = true;
            this.axAcroPDFNew.Location = new System.Drawing.Point(-1, 3);
            this.axAcroPDFNew.Name = "axAcroPDFNew";
            this.axAcroPDFNew.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDFNew.OcxState")));
            this.axAcroPDFNew.Size = new System.Drawing.Size(986, 762);
            this.axAcroPDFNew.TabIndex = 66;
            // 
            // frmPDFView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 768);
            this.Controls.Add(this.axAcroPDFNew);
            this.Name = "frmPDFView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDFNew)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF axAcroPDFNew;
    }
}
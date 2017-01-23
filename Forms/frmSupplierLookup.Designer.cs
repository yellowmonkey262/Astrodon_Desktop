namespace Astrodon.Forms
{
    partial class frmSupplierLookup
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
            this.pnlContents = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlContents
            // 
            this.pnlContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContents.Location = new System.Drawing.Point(0, 0);
            this.pnlContents.Name = "pnlContents";
            this.pnlContents.Size = new System.Drawing.Size(284, 261);
            this.pnlContents.TabIndex = 3;
            // 
            // frmSupplierLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.pnlContents);
            this.Name = "frmSupplierLookup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Supplier Lookup";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContents;
    }
}
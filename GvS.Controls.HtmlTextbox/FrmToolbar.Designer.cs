namespace GvS.Controls {
    partial class FrmToolbar {
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
            this.components = new System.ComponentModel.Container();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tsbBold = new System.Windows.Forms.ToolStripButton();
            this.tsbItalic = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbOrderedList = new System.Windows.Forms.ToolStripButton();
            this.tsbBulletList = new System.Windows.Forms.ToolStripButton();
            this.tsbUnIndent = new System.Windows.Forms.ToolStripButton();
            this.tsbIndent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCut = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.tsbPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbFont = new System.Windows.Forms.ToolStripComboBox();
            this.tsbFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbViewSource = new System.Windows.Forms.ToolStripButton();
            this.tmrAppear = new System.Windows.Forms.Timer(this.components);
            this.toolBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbBold,
            this.tsbItalic,
            this.toolStripSeparator2,
            this.tsbOrderedList,
            this.tsbBulletList,
            this.tsbUnIndent,
            this.tsbIndent,
            this.toolStripSeparator1,
            this.tsbCut,
            this.tsbCopy,
            this.tsbPaste});
            this.toolBar.Location = new System.Drawing.Point(0, 25);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(246, 25);
            this.toolBar.TabIndex = 2;
            this.toolBar.Text = "toolStrip1";
            // 
            // tsbBold
            // 
            this.tsbBold.CheckOnClick = true;
            this.tsbBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbBold.Image = global::GvS.Controls.Properties.Resources.boldhs;
            this.tsbBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBold.Name = "tsbBold";
            this.tsbBold.Size = new System.Drawing.Size(23, 22);
            this.tsbBold.Text = "Bold";
            this.tsbBold.Click += new System.EventHandler(this.tsbBold_Click);
            // 
            // tsbItalic
            // 
            this.tsbItalic.CheckOnClick = true;
            this.tsbItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbItalic.Image = global::GvS.Controls.Properties.Resources.ItalicHS;
            this.tsbItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbItalic.Name = "tsbItalic";
            this.tsbItalic.Size = new System.Drawing.Size(23, 22);
            this.tsbItalic.Text = "Italic";
            this.tsbItalic.Click += new System.EventHandler(this.tsbItalic_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbOrderedList
            // 
            this.tsbOrderedList.CheckOnClick = true;
            this.tsbOrderedList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOrderedList.Image = global::GvS.Controls.Properties.Resources.List_NumberedHS;
            this.tsbOrderedList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOrderedList.Name = "tsbOrderedList";
            this.tsbOrderedList.Size = new System.Drawing.Size(23, 22);
            this.tsbOrderedList.Text = "Ordered list";
            this.tsbOrderedList.Click += new System.EventHandler(this.tsbOrderedList_Click);
            // 
            // tsbBulletList
            // 
            this.tsbBulletList.CheckOnClick = true;
            this.tsbBulletList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbBulletList.Image = global::GvS.Controls.Properties.Resources.List_BulletsHS;
            this.tsbBulletList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBulletList.Name = "tsbBulletList";
            this.tsbBulletList.Size = new System.Drawing.Size(23, 22);
            this.tsbBulletList.Text = "Bullet List";
            this.tsbBulletList.Click += new System.EventHandler(this.tsbBulletList_Click);
            // 
            // tsbUnIndent
            // 
            this.tsbUnIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUnIndent.Image = global::GvS.Controls.Properties.Resources.OutdentHS;
            this.tsbUnIndent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUnIndent.Name = "tsbUnIndent";
            this.tsbUnIndent.Size = new System.Drawing.Size(23, 22);
            this.tsbUnIndent.Text = "Unindent";
            this.tsbUnIndent.Click += new System.EventHandler(this.tsbUnIndent_Click);
            // 
            // tsbIndent
            // 
            this.tsbIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbIndent.Image = global::GvS.Controls.Properties.Resources.IndentHS;
            this.tsbIndent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbIndent.Name = "tsbIndent";
            this.tsbIndent.Size = new System.Drawing.Size(23, 22);
            this.tsbIndent.Text = "Indent";
            this.tsbIndent.Click += new System.EventHandler(this.tsbIndent_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCut
            // 
            this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCut.Image = global::GvS.Controls.Properties.Resources.CutHS;
            this.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCut.Name = "tsbCut";
            this.tsbCut.Size = new System.Drawing.Size(23, 22);
            this.tsbCut.Text = "Cut";
            this.tsbCut.Click += new System.EventHandler(this.tsbCut_Click);
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.Image = global::GvS.Controls.Properties.Resources.CopyHS;
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(23, 22);
            this.tsbCopy.Text = "Copy";
            this.tsbCopy.Click += new System.EventHandler(this.tsbCopy_Click);
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.Image = global::GvS.Controls.Properties.Resources.PasteHS;
            this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.Size = new System.Drawing.Size(23, 22);
            this.tsbPaste.Text = "Paste";
            this.tsbPaste.Click += new System.EventHandler(this.tsbPaste_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbFont,
            this.tsbFontSize,
            this.toolStripSeparator5,
            this.tsbViewSource});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(246, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbFont
            // 
            this.tsbFont.Items.AddRange(new object[] {
            "Corbel",
            "Corbel, Verdana, Arial, Helvetica, sans-serif",
            "Georgia, Times New Roman, Times, serif",
            "Consolas, Courier New, Courier, monospace"});
            this.tsbFont.Name = "tsbFont";
            this.tsbFont.Size = new System.Drawing.Size(121, 25);
            this.tsbFont.SelectedIndexChanged += new System.EventHandler(this.tsbFont_SelectedIndexChanged);
            this.tsbFont.Leave += new System.EventHandler(this.tsbFont_Leave);
            // 
            // tsbFontSize
            // 
            this.tsbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsbFontSize.DropDownWidth = 75;
            this.tsbFontSize.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.tsbFontSize.Name = "tsbFontSize";
            this.tsbFontSize.Size = new System.Drawing.Size(75, 25);
            this.tsbFontSize.SelectedIndexChanged += new System.EventHandler(this.tsbFontSize_SelectedIndexChanged);
            this.tsbFontSize.Leave += new System.EventHandler(this.tsbFontSize_Leave);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbViewSource
            // 
            this.tsbViewSource.CheckOnClick = true;
            this.tsbViewSource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbViewSource.Image = global::GvS.Controls.Properties.Resources.EditCodeHS;
            this.tsbViewSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbViewSource.Name = "tsbViewSource";
            this.tsbViewSource.Size = new System.Drawing.Size(23, 20);
            this.tsbViewSource.Text = "View Html Source";
            this.tsbViewSource.CheckedChanged += new System.EventHandler(this.tsbViewSource_CheckedChanged);
            // 
            // tmrAppear
            // 
            this.tmrAppear.Tick += new System.EventHandler(this.tmrAppear_Tick);
            // 
            // FrmToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 52);
            this.ControlBox = false;
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmToolbar";
            this.Opacity = 0.7;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.ToolStripButton tsbPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbBold;
        private System.Windows.Forms.ToolStripButton tsbItalic;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbOrderedList;
        private System.Windows.Forms.ToolStripButton tsbBulletList;
        private System.Windows.Forms.ToolStripButton tsbUnIndent;
        private System.Windows.Forms.ToolStripButton tsbIndent;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox tsbFontSize;
        private System.Windows.Forms.ToolStripButton tsbViewSource;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Timer tmrAppear;
        internal System.Windows.Forms.ToolStripComboBox tsbFont;
    }
}
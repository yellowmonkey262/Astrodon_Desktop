namespace Astrodon.Forms {
    partial class frmDocument {
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
            this.rtfEditor = new System.Windows.Forms.RichTextBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.btnBold = new System.Windows.Forms.Button();
            this.btnUnder = new System.Windows.Forms.Button();
            this.btnItalic = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnAttach = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSpell = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtfEditor
            // 
            this.rtfEditor.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfEditor.Location = new System.Drawing.Point(12, 41);
            this.rtfEditor.Name = "rtfEditor";
            this.rtfEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtfEditor.Size = new System.Drawing.Size(793, 382);
            this.rtfEditor.TabIndex = 0;
            this.rtfEditor.Text = "";
            this.rtfEditor.TextChanged += new System.EventHandler(this.rtfEditor_TextChanged);
            // 
            // btnBold
            // 
            this.btnBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBold.Location = new System.Drawing.Point(12, 12);
            this.btnBold.Name = "btnBold";
            this.btnBold.Size = new System.Drawing.Size(27, 23);
            this.btnBold.TabIndex = 9;
            this.btnBold.Text = "B";
            this.btnBold.UseVisualStyleBackColor = true;
            this.btnBold.Click += new System.EventHandler(this.btnBold_Click);
            // 
            // btnUnder
            // 
            this.btnUnder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnder.Location = new System.Drawing.Point(45, 12);
            this.btnUnder.Name = "btnUnder";
            this.btnUnder.Size = new System.Drawing.Size(27, 23);
            this.btnUnder.TabIndex = 10;
            this.btnUnder.Text = "U";
            this.btnUnder.UseVisualStyleBackColor = true;
            this.btnUnder.Click += new System.EventHandler(this.btnUnder_Click);
            // 
            // btnItalic
            // 
            this.btnItalic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnItalic.Location = new System.Drawing.Point(78, 12);
            this.btnItalic.Name = "btnItalic";
            this.btnItalic.Size = new System.Drawing.Size(27, 23);
            this.btnItalic.TabIndex = 11;
            this.btnItalic.Text = "I";
            this.btnItalic.UseVisualStyleBackColor = true;
            this.btnItalic.Click += new System.EventHandler(this.btnItalic_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(647, 429);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(730, 429);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(111, 12);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(75, 23);
            this.btnPaste.TabIndex = 19;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnAttach
            // 
            this.btnAttach.Location = new System.Drawing.Point(192, 12);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(134, 23);
            this.btnAttach.TabIndex = 20;
            this.btnAttach.Text = "Insert Attachment";
            this.btnAttach.UseVisualStyleBackColor = true;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(332, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "Insert Page Break";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSpell
            // 
            this.btnSpell.Image = global::Astrodon.Properties.Resources.base_text;
            this.btnSpell.Location = new System.Drawing.Point(472, 12);
            this.btnSpell.Name = "btnSpell";
            this.btnSpell.Size = new System.Drawing.Size(75, 23);
            this.btnSpell.TabIndex = 22;
            this.btnSpell.UseVisualStyleBackColor = true;
            this.btnSpell.Click += new System.EventHandler(this.btnSpell_Click);
            // 
            // frmDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 468);
            this.ControlBox = false;
            this.Controls.Add(this.btnSpell);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAttach);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnItalic);
            this.Controls.Add(this.btnUnder);
            this.Controls.Add(this.btnBold);
            this.Controls.Add(this.rtfEditor);
            this.Name = "frmDocument";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Document Creator";
            this.Load += new System.EventHandler(this.frmDocument_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtfEditor;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button btnBold;
        private System.Windows.Forms.Button btnUnder;
        private System.Windows.Forms.Button btnItalic;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnAttach;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSpell;
    }
}
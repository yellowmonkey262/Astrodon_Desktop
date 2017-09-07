namespace Astrodon.Reports
{
    partial class ManangementPackUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManangementPackUserControl));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbBuilding = new System.Windows.Forms.ComboBox();
            this.dgTocGrid = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            this.button2 = new System.Windows.Forms.Button();
            this.cbIncludeSundries = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddLevyRoll = new System.Windows.Forms.Button();
            this.btnCheckList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgTocGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Year";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(109, 17);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(233, 21);
            this.cmbYear.TabIndex = 1;
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(109, 75);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(233, 21);
            this.cmbMonth.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Month";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Building";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(455, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Generate TOC";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbBuilding
            // 
            this.cmbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuilding.FormattingEnabled = true;
            this.cmbBuilding.Location = new System.Drawing.Point(109, 44);
            this.cmbBuilding.Name = "cmbBuilding";
            this.cmbBuilding.Size = new System.Drawing.Size(233, 21);
            this.cmbBuilding.TabIndex = 7;
            // 
            // dgTocGrid
            // 
            this.dgTocGrid.AllowUserToAddRows = false;
            this.dgTocGrid.AllowUserToDeleteRows = false;
            this.dgTocGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTocGrid.Location = new System.Drawing.Point(19, 131);
            this.dgTocGrid.MultiSelect = false;
            this.dgTocGrid.Name = "dgTocGrid";
            this.dgTocGrid.Size = new System.Drawing.Size(821, 383);
            this.dgTocGrid.TabIndex = 8;
            this.dgTocGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTocGrid_CellContentClick);
            this.dgTocGrid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgTocGrid_DataBindingComplete);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(590, 102);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(124, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Save Report";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "Adobe PDF files (*.pdf)|*.pdf";
            this.dlgOpen.InitialDirectory = "Y:\\";
            this.dlgOpen.Title = "Management Pack Open File";
            // 
            // dlgSave
            // 
            this.dlgSave.Filter = "Adobe PDF files (*.pdf)|*.pdf";
            this.dlgSave.InitialDirectory = "Y:\\";
            this.dlgSave.Title = "Management Pack Save Report";
            // 
            // axAcroPDF1
            // 
            this.axAcroPDF1.Enabled = true;
            this.axAcroPDF1.Location = new System.Drawing.Point(846, 3);
            this.axAcroPDF1.Name = "axAcroPDF1";
            this.axAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
            this.axAcroPDF1.Size = new System.Drawing.Size(452, 511);
            this.axAcroPDF1.TabIndex = 65;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(455, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Add Additional PDF";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbIncludeSundries
            // 
            this.cbIncludeSundries.AutoSize = true;
            this.cbIncludeSundries.Checked = true;
            this.cbIncludeSundries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeSundries.Location = new System.Drawing.Point(680, 78);
            this.cbIncludeSundries.Name = "cbIncludeSundries";
            this.cbIncludeSundries.Size = new System.Drawing.Size(15, 14);
            this.cbIncludeSundries.TabIndex = 7;
            this.cbIncludeSundries.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(587, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 69;
            this.label5.Text = "Include Sundries";
            // 
            // btnAddLevyRoll
            // 
            this.btnAddLevyRoll.Location = new System.Drawing.Point(455, 72);
            this.btnAddLevyRoll.Name = "btnAddLevyRoll";
            this.btnAddLevyRoll.Size = new System.Drawing.Size(124, 23);
            this.btnAddLevyRoll.TabIndex = 6;
            this.btnAddLevyRoll.Text = "Add Levy Roll";
            this.btnAddLevyRoll.UseVisualStyleBackColor = true;
            this.btnAddLevyRoll.Click += new System.EventHandler(this.btnAddLevyRoll_Click);
            // 
            // btnCheckList
            // 
            this.btnCheckList.Location = new System.Drawing.Point(455, 102);
            this.btnCheckList.Name = "btnCheckList";
            this.btnCheckList.Size = new System.Drawing.Size(124, 23);
            this.btnCheckList.TabIndex = 70;
            this.btnCheckList.Text = "Add Check List";
            this.btnCheckList.UseVisualStyleBackColor = true;
            this.btnCheckList.Click += new System.EventHandler(this.btnCheckList_Click);
            // 
            // ManangementPackUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCheckList);
            this.Controls.Add(this.btnAddLevyRoll);
            this.Controls.Add(this.cbIncludeSundries);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.axAcroPDF1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dgTocGrid);
            this.Controls.Add(this.cmbBuilding);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbMonth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.label1);
            this.Name = "ManangementPackUserControl";
            this.Size = new System.Drawing.Size(1301, 529);
            ((System.ComponentModel.ISupportInitialize)(this.dgTocGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbBuilding;
        private System.Windows.Forms.DataGridView dgTocGrid;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private AxAcroPDFLib.AxAcroPDF axAcroPDF1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cbIncludeSundries;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddLevyRoll;
        private System.Windows.Forms.Button btnCheckList;
    }
}

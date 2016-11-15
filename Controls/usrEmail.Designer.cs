namespace Astrodon.Controls {
    partial class usrEmail {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.cmbCrit = new System.Windows.Forms.ComboBox();
            this.lblSeach = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmbSearchBy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.astrodonDataSet = new Astrodon.DataSets.AstrodonDataSet();
            this.astrodonDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnPrint = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSetBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search:";
            // 
            // cmbSearch
            // 
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Items.AddRange(new object[] {
            "Statements",
            "Letters"});
            this.cmbSearch.Location = new System.Drawing.Point(102, 9);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(263, 21);
            this.cmbSearch.TabIndex = 1;
            this.cmbSearch.SelectedIndexChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
            // 
            // cmbCrit
            // 
            this.cmbCrit.FormattingEnabled = true;
            this.cmbCrit.Location = new System.Drawing.Point(102, 63);
            this.cmbCrit.Name = "cmbCrit";
            this.cmbCrit.Size = new System.Drawing.Size(263, 21);
            this.cmbCrit.TabIndex = 3;
            // 
            // lblSeach
            // 
            this.lblSeach.AutoSize = true;
            this.lblSeach.Location = new System.Drawing.Point(15, 66);
            this.lblSeach.Name = "lblSeach";
            this.lblSeach.Size = new System.Drawing.Size(29, 13);
            this.lblSeach.TabIndex = 2;
            this.lblSeach.Text = "Unit:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 119);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(867, 256);
            this.dataGridView1.TabIndex = 4;
            // 
            // cmbSearchBy
            // 
            this.cmbSearchBy.FormattingEnabled = true;
            this.cmbSearchBy.Items.AddRange(new object[] {
            "Sent Status",
            "Delivery Status",
            "Unit"});
            this.cmbSearchBy.Location = new System.Drawing.Point(102, 36);
            this.cmbSearchBy.Name = "cmbSearchBy";
            this.cmbSearchBy.Size = new System.Drawing.Size(263, 21);
            this.cmbSearchBy.TabIndex = 6;
            this.cmbSearchBy.SelectedIndexChanged += new System.EventHandler(this.cmbSearchBy_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Search By:";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(290, 90);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // astrodonDataSet
            // 
            this.astrodonDataSet.DataSetName = "AstrodonDataSet";
            this.astrodonDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // astrodonDataSetBindingSource
            // 
            this.astrodonDataSetBindingSource.DataSource = this.astrodonDataSet;
            this.astrodonDataSetBindingSource.Position = 0;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(209, 90);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // usrEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.cmbSearchBy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmbCrit);
            this.Controls.Add(this.lblSeach);
            this.Controls.Add(this.cmbSearch);
            this.Controls.Add(this.label1);
            this.Name = "usrEmail";
            this.Size = new System.Drawing.Size(898, 393);
            this.Load += new System.EventHandler(this.usrEmail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSetBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSearch;
        private System.Windows.Forms.ComboBox cmbCrit;
        private System.Windows.Forms.Label lblSeach;
        private DataSets.AstrodonDataSet astrodonDataSet;
        private System.Windows.Forms.BindingSource astrodonDataSetBindingSource;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmbSearchBy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
    }
}

namespace Astrodon.Controls {
    partial class usrPMJobs {
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
            this.dgJobs = new System.Windows.Forms.DataGridView();
            this.jobListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.astrodonDataSet = new Astrodon.AstrodonDataSet();
            this.jobListAdapter = new Astrodon.AstrodonDataSetTableAdapters.JobListAdapter();
            this.tmrJob = new System.Windows.Forms.Timer(this.components);
            this.jobIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buildingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subjectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgJobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jobListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dgJobs
            // 
            this.dgJobs.AllowUserToAddRows = false;
            this.dgJobs.AllowUserToDeleteRows = false;
            this.dgJobs.AllowUserToResizeColumns = false;
            this.dgJobs.AllowUserToResizeRows = false;
            this.dgJobs.AutoGenerateColumns = false;
            this.dgJobs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgJobs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.jobIDDataGridViewTextBoxColumn,
            this.pMDataGridViewTextBoxColumn,
            this.processedByDataGridViewTextBoxColumn,
            this.buildingDataGridViewTextBoxColumn,
            this.subjectDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.Column1,
            this.Delete});
            this.dgJobs.DataSource = this.jobListBindingSource;
            this.dgJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgJobs.Location = new System.Drawing.Point(0, 0);
            this.dgJobs.Name = "dgJobs";
            this.dgJobs.Size = new System.Drawing.Size(1148, 668);
            this.dgJobs.TabIndex = 0;
            this.dgJobs.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgJobs_CellContentClick);
            this.dgJobs.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgJobs_DataBindingComplete);
            this.dgJobs.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgJobs_DataError);
            this.dgJobs.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgJobs_UserDeletingRow);
            // 
            // jobListBindingSource
            // 
            this.jobListBindingSource.DataMember = "JobList";
            this.jobListBindingSource.DataSource = this.astrodonDataSet;
            // 
            // astrodonDataSet
            // 
            this.astrodonDataSet.DataSetName = "AstrodonDataSet";
            this.astrodonDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // jobListAdapter
            // 
            this.jobListAdapter.ClearBeforeFill = true;
            // 
            // tmrJob
            // 
            this.tmrJob.Interval = 30000;
            this.tmrJob.Tick += new System.EventHandler(this.tmrJob_Tick);
            // 
            // jobIDDataGridViewTextBoxColumn
            // 
            this.jobIDDataGridViewTextBoxColumn.DataPropertyName = "Job ID";
            this.jobIDDataGridViewTextBoxColumn.HeaderText = "Job ID";
            this.jobIDDataGridViewTextBoxColumn.Name = "jobIDDataGridViewTextBoxColumn";
            this.jobIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pMDataGridViewTextBoxColumn
            // 
            this.pMDataGridViewTextBoxColumn.DataPropertyName = "PM";
            this.pMDataGridViewTextBoxColumn.HeaderText = "PM";
            this.pMDataGridViewTextBoxColumn.Name = "pMDataGridViewTextBoxColumn";
            // 
            // processedByDataGridViewTextBoxColumn
            // 
            this.processedByDataGridViewTextBoxColumn.DataPropertyName = "Processed By";
            this.processedByDataGridViewTextBoxColumn.HeaderText = "Processed By";
            this.processedByDataGridViewTextBoxColumn.Name = "processedByDataGridViewTextBoxColumn";
            // 
            // buildingDataGridViewTextBoxColumn
            // 
            this.buildingDataGridViewTextBoxColumn.DataPropertyName = "Building";
            this.buildingDataGridViewTextBoxColumn.HeaderText = "Building";
            this.buildingDataGridViewTextBoxColumn.Name = "buildingDataGridViewTextBoxColumn";
            // 
            // subjectDataGridViewTextBoxColumn
            // 
            this.subjectDataGridViewTextBoxColumn.DataPropertyName = "Subject";
            this.subjectDataGridViewTextBoxColumn.HeaderText = "Subject";
            this.subjectDataGridViewTextBoxColumn.Name = "subjectDataGridViewTextBoxColumn";
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Action";
            this.Column1.Name = "Column1";
            this.Column1.Text = "Select";
            this.Column1.UseColumnTextForButtonValue = true;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.Visible = false;
            // 
            // usrPMJobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgJobs);
            this.Name = "usrPMJobs";
            this.Size = new System.Drawing.Size(1148, 668);
            this.Load += new System.EventHandler(this.usrPMJobs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgJobs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jobListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.astrodonDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgJobs;
        private System.Windows.Forms.BindingSource jobListBindingSource;
        private AstrodonDataSet astrodonDataSet;
        private AstrodonDataSetTableAdapters.JobListAdapter jobListAdapter;
        private System.Windows.Forms.Timer tmrJob;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn processedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buildingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subjectDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
    }
}

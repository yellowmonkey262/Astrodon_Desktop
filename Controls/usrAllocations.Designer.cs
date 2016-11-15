namespace Astrodon {
    partial class usrAllocations {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpAllocated = new System.Windows.Forms.TabPage();
            this.btnUpdateAllocated = new System.Windows.Forms.Button();
            this.dgAllocated = new System.Windows.Forms.DataGridView();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStmtID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBuilding = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrust = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpUnallocated = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.dgUnallocated = new System.Windows.Forms.DataGridView();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAllocate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tpUnaAccount = new System.Windows.Forms.TabPage();
            this.tpPrevUna = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tpAllocated.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAllocated)).BeginInit();
            this.tpUnallocated.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgUnallocated)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpAllocated);
            this.tabControl1.Controls.Add(this.tpUnallocated);
            this.tabControl1.Controls.Add(this.tpUnaAccount);
            this.tabControl1.Controls.Add(this.tpPrevUna);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1127, 663);
            this.tabControl1.TabIndex = 3;
            // 
            // tpAllocated
            // 
            this.tpAllocated.Controls.Add(this.btnUpdateAllocated);
            this.tpAllocated.Controls.Add(this.dgAllocated);
            this.tpAllocated.Location = new System.Drawing.Point(4, 22);
            this.tpAllocated.Name = "tpAllocated";
            this.tpAllocated.Padding = new System.Windows.Forms.Padding(3);
            this.tpAllocated.Size = new System.Drawing.Size(1119, 637);
            this.tpAllocated.TabIndex = 0;
            this.tpAllocated.Text = "Allocated";
            this.tpAllocated.UseVisualStyleBackColor = true;
            // 
            // btnUpdateAllocated
            // 
            this.btnUpdateAllocated.Location = new System.Drawing.Point(1038, 608);
            this.btnUpdateAllocated.Name = "btnUpdateAllocated";
            this.btnUpdateAllocated.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateAllocated.TabIndex = 4;
            this.btnUpdateAllocated.Text = "Update";
            this.btnUpdateAllocated.UseVisualStyleBackColor = true;
            // 
            // dgAllocated
            // 
            this.dgAllocated.AllowUserToAddRows = false;
            this.dgAllocated.AllowUserToDeleteRows = false;
            this.dgAllocated.AllowUserToResizeColumns = false;
            this.dgAllocated.AllowUserToResizeRows = false;
            this.dgAllocated.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgAllocated.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAllocated.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDelete,
            this.colID,
            this.colStmtID,
            this.colDate,
            this.colDesc,
            this.colAmt,
            this.colBuilding,
            this.colCode,
            this.colRef,
            this.colTrust,
            this.colCash,
            this.colPath});
            this.dgAllocated.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgAllocated.Location = new System.Drawing.Point(6, 6);
            this.dgAllocated.Name = "dgAllocated";
            this.dgAllocated.Size = new System.Drawing.Size(1107, 596);
            this.dgAllocated.TabIndex = 3;
            // 
            // colDelete
            // 
            this.colDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDelete.FillWeight = 77.66522F;
            this.colDelete.Frozen = true;
            this.colDelete.HeaderText = "";
            this.colDelete.Name = "colDelete";
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            this.colDelete.Width = 50;
            // 
            // colID
            // 
            this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colID.DataPropertyName = "id";
            this.colID.FillWeight = 62.86634F;
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Width = 50;
            // 
            // colStmtID
            // 
            this.colStmtID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colStmtID.DataPropertyName = "lid";
            this.colStmtID.FillWeight = 60.91368F;
            this.colStmtID.HeaderText = "Stmt ID";
            this.colStmtID.Name = "colStmtID";
            this.colStmtID.ReadOnly = true;
            this.colStmtID.Width = 50;
            // 
            // colDate
            // 
            this.colDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDate.DataPropertyName = "trnDate";
            this.colDate.FillWeight = 91.3312F;
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            this.colDate.Width = 75;
            // 
            // colDesc
            // 
            this.colDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDesc.DataPropertyName = "description";
            this.colDesc.FillWeight = 150.6625F;
            this.colDesc.HeaderText = "Description";
            this.colDesc.Name = "colDesc";
            this.colDesc.ReadOnly = true;
            this.colDesc.Width = 200;
            // 
            // colAmt
            // 
            this.colAmt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAmt.DataPropertyName = "amount";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colAmt.DefaultCellStyle = dataGridViewCellStyle4;
            this.colAmt.FillWeight = 150.6625F;
            this.colAmt.HeaderText = "Amount";
            this.colAmt.Name = "colAmt";
            this.colAmt.ReadOnly = true;
            this.colAmt.Width = 75;
            // 
            // colBuilding
            // 
            this.colBuilding.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colBuilding.DataPropertyName = "building";
            this.colBuilding.FillWeight = 150.6625F;
            this.colBuilding.HeaderText = "Building";
            this.colBuilding.Name = "colBuilding";
            this.colBuilding.Width = 200;
            // 
            // colCode
            // 
            this.colCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCode.DataPropertyName = "code";
            this.colCode.FillWeight = 66.64542F;
            this.colCode.HeaderText = "Code";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 40;
            // 
            // colRef
            // 
            this.colRef.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRef.DataPropertyName = "reference";
            this.colRef.FillWeight = 150.6625F;
            this.colRef.HeaderText = "Reference";
            this.colRef.Name = "colRef";
            this.colRef.Width = 60;
            // 
            // colTrust
            // 
            this.colTrust.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTrust.DataPropertyName = "accnumber";
            this.colTrust.FillWeight = 114.2993F;
            this.colTrust.HeaderText = "Trust Account";
            this.colTrust.Name = "colTrust";
            this.colTrust.ReadOnly = true;
            this.colTrust.Width = 75;
            // 
            // colCash
            // 
            this.colCash.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCash.DataPropertyName = "contra";
            this.colCash.FillWeight = 63.10304F;
            this.colCash.HeaderText = "Contra";
            this.colCash.Name = "colCash";
            this.colCash.ReadOnly = true;
            this.colCash.Width = 75;
            // 
            // colPath
            // 
            this.colPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPath.DataPropertyName = "datapath";
            this.colPath.FillWeight = 60.52555F;
            this.colPath.HeaderText = "Data Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 75;
            // 
            // tpUnallocated
            // 
            this.tpUnallocated.Controls.Add(this.button1);
            this.tpUnallocated.Controls.Add(this.dgUnallocated);
            this.tpUnallocated.Location = new System.Drawing.Point(4, 22);
            this.tpUnallocated.Name = "tpUnallocated";
            this.tpUnallocated.Padding = new System.Windows.Forms.Padding(3);
            this.tpUnallocated.Size = new System.Drawing.Size(1119, 637);
            this.tpUnallocated.TabIndex = 1;
            this.tpUnallocated.Text = "Unallocated";
            this.tpUnallocated.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1038, 608);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // dgUnallocated
            // 
            this.dgUnallocated.AllowUserToAddRows = false;
            this.dgUnallocated.AllowUserToDeleteRows = false;
            this.dgUnallocated.AllowUserToResizeColumns = false;
            this.dgUnallocated.AllowUserToResizeRows = false;
            this.dgUnallocated.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgUnallocated.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUnallocated.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewButtonColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewComboBoxColumn1,
            this.dataGridViewTextBoxColumn7,
            this.colPeriod,
            this.colAllocate});
            this.dgUnallocated.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgUnallocated.Location = new System.Drawing.Point(6, 6);
            this.dgUnallocated.Name = "dgUnallocated";
            this.dgUnallocated.Size = new System.Drawing.Size(1107, 596);
            this.dgUnallocated.TabIndex = 5;
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewButtonColumn1.FillWeight = 77.66522F;
            this.dataGridViewButtonColumn1.Frozen = true;
            this.dataGridViewButtonColumn1.HeaderText = "";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.Text = "Delete";
            this.dataGridViewButtonColumn1.UseColumnTextForButtonValue = true;
            this.dataGridViewButtonColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.FillWeight = 62.86634F;
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "lid";
            this.dataGridViewTextBoxColumn2.FillWeight = 60.91368F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Stmt ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 50;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "trnDate";
            this.dataGridViewTextBoxColumn3.FillWeight = 91.3312F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 75;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "description";
            this.dataGridViewTextBoxColumn4.FillWeight = 150.6625F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Description";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "amount";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn5.FillWeight = 150.6625F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 75;
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewComboBoxColumn1.DataPropertyName = "building";
            this.dataGridViewComboBoxColumn1.FillWeight = 150.6625F;
            this.dataGridViewComboBoxColumn1.HeaderText = "Building";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "reference";
            this.dataGridViewTextBoxColumn7.FillWeight = 150.6625F;
            this.dataGridViewTextBoxColumn7.HeaderText = "Reference";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // colPeriod
            // 
            this.colPeriod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPeriod.HeaderText = "Period";
            this.colPeriod.Name = "colPeriod";
            this.colPeriod.Width = 50;
            // 
            // colAllocate
            // 
            this.colAllocate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAllocate.HeaderText = "";
            this.colAllocate.Name = "colAllocate";
            this.colAllocate.Text = "Allocate";
            this.colAllocate.UseColumnTextForButtonValue = true;
            this.colAllocate.Width = 50;
            // 
            // tpUnaAccount
            // 
            this.tpUnaAccount.Location = new System.Drawing.Point(4, 22);
            this.tpUnaAccount.Name = "tpUnaAccount";
            this.tpUnaAccount.Size = new System.Drawing.Size(1119, 637);
            this.tpUnaAccount.TabIndex = 2;
            this.tpUnaAccount.Text = "Unallocated Account";
            this.tpUnaAccount.UseVisualStyleBackColor = true;
            // 
            // tpPrevUna
            // 
            this.tpPrevUna.Location = new System.Drawing.Point(4, 22);
            this.tpPrevUna.Name = "tpPrevUna";
            this.tpPrevUna.Size = new System.Drawing.Size(1119, 637);
            this.tpPrevUna.TabIndex = 3;
            this.tpPrevUna.Text = "Previous Unallocated";
            this.tpPrevUna.UseVisualStyleBackColor = true;
            // 
            // usrAllocations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "usrAllocations";
            this.Size = new System.Drawing.Size(1130, 666);
            this.Load += new System.EventHandler(this.usrAllocations_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpAllocated.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAllocated)).EndInit();
            this.tpUnallocated.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgUnallocated)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpAllocated;
        private System.Windows.Forms.Button btnUpdateAllocated;
        private System.Windows.Forms.DataGridView dgAllocated;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStmtID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmt;
        private System.Windows.Forms.DataGridViewComboBoxColumn colBuilding;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrust;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCash;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;
        private System.Windows.Forms.TabPage tpUnallocated;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dgUnallocated;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriod;
        private System.Windows.Forms.DataGridViewButtonColumn colAllocate;
        private System.Windows.Forms.TabPage tpUnaAccount;
        private System.Windows.Forms.TabPage tpPrevUna;

    }
}

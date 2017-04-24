using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using BankData = Astrodon.Data.BankData;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace Astrodon.Controls.Bank
{
    public partial class usrBankConfiguration : UserControl
    {
        private DataContext _Context;
        private BankData.Bank _Item;
        private List<BankData.Bank> _Data;
        private List<BankData.BankAudit> _AuditData;

        private int userid;

        public usrBankConfiguration(DataContext context)
        {
            _Context = context;
            userid = Controller.user.id;

            InitializeComponent();

            LoadBanks();
            GotoReadOnly();
         
        }

        private void GotoReadOnly()
        {
            cbActive.Checked = true;
            cbActive.Enabled = false;
            tbName.Text = "";
            tbName.ReadOnly = true;

            tbBranchName.Text = "";
            tbBranchName.ReadOnly = true;

            tbBranchCode.Text = "";
            tbBranchCode.ReadOnly = true;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnNew.Visible = true;
            dgItems.Enabled = true;

            _AuditData = new List<Data.BankData.BankAudit>();
            BindAuditGrid();

        }

        private void GotoEditable()
        {
            cbActive.Checked = true;
            cbActive.Enabled = true;
            tbName.ReadOnly = false;
            tbBranchName.ReadOnly = false;
            tbBranchCode.ReadOnly = false;

            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnNew.Visible = false;
            dgItems.Enabled = false;

        }

        private void LoadBanks()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                _Data = _Context.BankSet.OrderBy(a => a.Name).ToList();
                BindDataGrid();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BindDataGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = _Data;

            dgItems.Columns.Clear();

            dgItems.DataSource = bs;

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Select",
                UseColumnTextForButtonValue = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName =  "BranchName",
                HeaderText = "Branch Name",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BranchCode",
                HeaderText = "Branch Code",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ActiveString",
                HeaderText = "Active",
                ReadOnly = true
            });

            dgItems.AutoResizeColumns();
            
        }

        private void BindAuditGrid()
        {

            dgAuditItems.ClearSelection();
            dgAuditItems.MultiSelect = false;
            dgAuditItems.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = _AuditData;

            dgAuditItems.Columns.Clear();

            dgAuditItems.DataSource = bs;

            dgAuditItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AuditTimeStampStr",
                HeaderText = "Date",
                ReadOnly = true
            });
            dgAuditItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UserName",
                HeaderText = "User",
                ReadOnly = true
            });

            dgAuditItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FieldName",
                HeaderText = "Field",
                ReadOnly = true
            });

            dgAuditItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "OldValue",
                HeaderText = "Old Value",
                ReadOnly = true
            });

            dgAuditItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "NewValue",
                HeaderText = "New Value",
                ReadOnly = true
            });

            dgAuditItems.AutoResizeColumns();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _Item = null;
            GotoEditable();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _Context.ClearChanges();
            GotoReadOnly();
            LoadBanks();

        }

        private void EditItem()
        {
             tbName.Text = _Item.Name;
             tbBranchName.Text = _Item.BranchName;
             tbBranchCode.Text = _Item.BranchCode;
             cbActive.Checked = _Item.IsActive;
            GotoEditable();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbName.Text))
            {
                Controller.HandleError("Name is required", "Validation Error");
                return;
            }


            var user = _Context.tblUsers.Single(a => a.id == userid);

            if (_Item == null)
            {
                _Item = new BankData.Bank();
                _Context.BankSet.Add(_Item);
            }
            var auditTimeStamp = DateTime.Now;
            if (_Item.Name != tbName.Text)
            {
                _Context.BankAuditSet.Add(new BankData.BankAudit()
                {
                    Bank = _Item,
                    User = user,
                    UserId = user.id,
                    FieldName = "Name",
                    OldValue = _Item.Name,
                    NewValue = tbName.Text,
                    AuditTimeStamp = auditTimeStamp
                });
                _Item.Name = tbName.Text;
            }

            if (_Item.BranchName != tbBranchName.Text)
            {
                _Context.BankAuditSet.Add(new BankData.BankAudit()
                {
                    Bank = _Item,
                    User = user,
                    UserId = user.id,
                    FieldName = "BranchName",
                    OldValue = _Item.BranchName,
                    NewValue = tbBranchName.Text,
                    AuditTimeStamp = auditTimeStamp
                });
                _Item.BranchName = tbBranchName.Text;
            }

            if (_Item.BranchCode != tbBranchCode.Text)
            {
                _Context.BankAuditSet.Add(new BankData.BankAudit()
                {
                    Bank = _Item,
                    User = user,
                    UserId = user.id,
                    FieldName = "BranchCode",
                    OldValue = _Item.BranchCode,
                    NewValue = tbBranchCode.Text,
                    AuditTimeStamp = auditTimeStamp
                });
                _Item.BranchCode = tbBranchCode.Text;
            }

            if (_Item.IsActive != cbActive.Checked)
            {
                _Context.BankAuditSet.Add(new BankData.BankAudit()
                {
                    Bank = _Item,
                    User = user,
                    UserId = user.id,
                    FieldName = "IsActive",
                    OldValue = _Item.IsActive.ToString(),
                    NewValue = cbActive.Checked.ToString(),
                    AuditTimeStamp = auditTimeStamp
                });
                _Item.IsActive = cbActive.Checked;
            }
            

            try
            {
                bool isNew = _Item.id == 0;
                _Context.SaveChanges();

                if (isNew)
                    _Data.Insert(0, _Item);
                BindDataGrid();
                GotoReadOnly();
            }
            catch (DbUpdateException ex)
            {
                Controller.HandleError("Possible duplicate record detected", "Database Error");
            }
            catch (Exception ex2)
            {
                Controller.HandleError(ex2.Message);
            }
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as BankData.Bank;

                if (_Item != null)
                {
                    EditItem();
                    LoadAuditItem(_Item);
                }
            }
        }


        private void LoadAuditItem(BankData.Bank dataItem)
        {
            _AuditData = _Context.BankAuditSet.Include(a => a.User).Where(a => a.BankId == dataItem.id).OrderByDescending(a => a.AuditTimeStamp).ToList();
            BindAuditGrid();
        }
    }
}

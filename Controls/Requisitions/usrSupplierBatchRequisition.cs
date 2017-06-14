using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astro.Library.Entities;
using Astrodon.Forms;
using System.Data.Entity;
using iTextSharp.text.pdf;
using Astrodon.Data;
using Astrodon.Data.RequisitionData;
using System.IO;
using Astrodon.Data.BankData;
using Astrodon.Data.SupplierData;

namespace Astrodon.Controls.Requisitions
{
    public partial class usrSupplierBatchRequisition : UserControl
    {
        private List<Building> _Buildings;
        private DateTime _minDate = new DateTime(2000, 1, 1);
        private Data.SupplierData.Supplier _Supplier = null;
        private List<BuildingRequisitionItem> _SupplierBuildingList = null;
        private List<PastelAccount> _DefaultList = null;
        private bool _AttachmentRequired = true;
        private List<Astrodon.Data.BankData.Bank> _BankList = null;
        private List<SupplierBankAccountDetail> _SupplierBankAccounts = null;

        public usrSupplierBatchRequisition()
        {
            InitializeComponent();
            LoadBuildings();
            LoadAccountList();
            LoadBanks();
            dtInvoiceDate.Value = _minDate;
            dtInvoiceDate.MinDate = _minDate;
        }

        private void LoadBanks()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                _BankList = context.BankSet.OrderBy(a => a.Name).ToList();
            }
        }

        private void LoadAccountList()
        {
            if (_Buildings != null && _Buildings.Count > 0)
            {
                _DefaultList = LoadPastelAccountsForBuilding(_Buildings[0].DataPath);
                cmbLedger.DataSource = _DefaultList;
                cmbLedger.ValueMember = "AccountNumber";
                cmbLedger.DisplayMember = "DisplayString";
            }
        }


        private void cmbLedger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_SupplierBuildingList != null)
            {
                foreach (var itm in _SupplierBuildingList)
                {
                    itm.SetSelectedAccount(cmbLedger.SelectedValue as string);
                }
            }
        }

        private void dtInvoiceDate_ValueChanged(object sender, EventArgs e)
        {
            if (_SupplierBuildingList != null)
            {
                foreach (var itm in _SupplierBuildingList)
                {
                    itm.SetSelectedInvoiceDate(dtInvoiceDate.Value);
                }
            }
        }

        private List<PastelAccount> LoadPastelAccountsForBuilding(string dataPath)
        {
            try
            {
                Dictionary<String, String> accounts = Controller.pastel.GetAccountList(dataPath);
                List<PastelAccount> result = new List<PastelAccount>();

                foreach (KeyValuePair<String, String> reqAcc in accounts)
                {
                    result.Add(new PastelAccount() { AccountNumber = reqAcc.Key, AccountName = reqAcc.Value });
                }
                if (result == null || result.Count == 0)
                    result = _DefaultList.ToList();
                return result.OrderBy(a => a.AccountNumber).ToList();
            }
            catch (Exception e)
            {
                Controller.HandleError(e);
                return new List<PastelAccount>();
            }
        }


        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var userid = Controller.user.id;

                if (Controller.user.username == "sheldon" || Controller.user.username == "tertia")
                {
                    _AttachmentRequired = false;
                    _Buildings = new Buildings(false).buildings; //all buildings
                }
                else
                {
                    _AttachmentRequired = true;
                    _Buildings = new Buildings(Controller.user.id).buildings; //only pm buildings
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnSupplierLookup_Click(object sender, EventArgs e)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var frmSupplierLookup = new frmSupplierLookup(context);
                var dialogResult = frmSupplierLookup.ShowDialog();
                var supplier = frmSupplierLookup.SelectedSupplier;

                if (dialogResult == DialogResult.OK && supplier != null)
                {
                    _Supplier = supplier;
                    lbSupplierName.Text = _Supplier.CompanyName;

                    int[] buildings = _Buildings.Select(a => a.ID).ToArray();

                    DateTime invoiceDate = dtInvoiceDate.Value;


                    var q = from sb in context.SupplierBuildingSet
                            join b in context.tblBuildings on sb.BuildingId equals b.id
                            join bank in context.BankSet on sb.BankId equals bank.id
                            join s in context.SupplierSet on sb.SupplierId equals s.id
                            where s.id == _Supplier.id
                            && buildings.Contains(b.id)
                            select new BuildingRequisitionItem()
                            {
                                BuildingId = b.id,
                                BuildingName = b.Building,
                                BuildingAbreviatio = b.Code,
                                BuildingTrustAccount = b.AccNumber,
                                BuildingDataPath = b.DataPath,
                                InvoiceDate = invoiceDate,
                                SupplierBankAccount = sb.AccountNumber,
                                SupplierBank = bank.Name,
                                InvoiceAttachmentRequired = _AttachmentRequired,
                                BranchCode = bank.BranchCode,
                                BranchName = bank.BranchName,
                                BankId = bank.id,
                                BankAlreadyLinked = true,
                                OwnTrustAccount = "OWN",
                                IsOwnAccount = b.bank == "OWN" ? true: false
                            };

                    _SupplierBuildingList = q.ToList();

                    //now add all the buildings not in the list
                    int[] exclude = _SupplierBuildingList.Select(a => a.BuildingId).Distinct().ToArray();
                    int[] buldingList = buildings.Except(exclude).ToArray();

                    var q2 = from b in context.tblBuildings
                             where buldingList.Contains(b.id)
                             select new BuildingRequisitionItem()
                             {
                                 BuildingId = b.id,
                                 BuildingName = b.Building,
                                 BuildingAbreviatio = b.Code,
                                 BuildingTrustAccount = b.AccNumber,
                                 BuildingDataPath = b.DataPath,
                                 InvoiceDate = invoiceDate,
                                 InvoiceAttachmentRequired = _AttachmentRequired,
                                 BankId = null,
                                 SupplierBankAccount = null,
                                 SupplierBank = null,
                                 BranchCode = null,
                                 BranchName = null,
                                 BankAlreadyLinked = false,
                                 OwnTrustAccount = "OWN",
                                 IsOwnAccount = b.bank == "OWN" ? true : false
                             };

                    _SupplierBuildingList.AddRange(q2.ToList());

                    _SupplierBuildingList = _SupplierBuildingList.OrderBy(a => a.BuildingName).ToList();

                    var qBank = from sb in context.SupplierBuildingSet
                            select new SupplierBankAccountDetail()
                            {
                                BankId = sb.BankId,
                                BankName = sb.Bank.Name,
                                BranchCode = sb.BranceCode,
                                AccountNumber = sb.AccountNumber,
                                BranchName = sb.BranchName
                            };

                    _SupplierBankAccounts = qBank.Distinct().OrderBy(a => a.BankName).ToList();
                    cbSupplierBankAccount.DataSource = _SupplierBankAccounts;
                    cbSupplierBankAccount.ValueMember = "BankId";
                    cbSupplierBankAccount.DisplayMember = "DisplayString";

                    LoadBuildingsGrid();
                }
                else
                {
                    ClearSupplier();
                }
            }
        }


        private void ClearSupplier()
        {
            _Supplier = null;
            _SupplierBuildingList = null;
            dgItems.DataSource = null;
            lbSupplierName.Text = "";
            dtInvoiceDate.Value = _minDate;
        }

        private void LoadBuildingsGrid()
        {
            if (_SupplierBuildingList == null)
                return;

            //load the accounts
            foreach (var sb in _SupplierBuildingList)
            {
                sb.PastelAccountList = LoadPastelAccountsForBuilding(sb.BuildingDataPath);
                sb.SetSelectedAccount(cmbLedger.SelectedValue as string);
                sb.BankList = _BankList.ToList();
            }
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";


            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _SupplierBuildingList;

            dgItems.Columns.Clear();
            dgItems.ReadOnly = false;
            dgItems.EditMode = DataGridViewEditMode.EditOnEnter;


            dgItems.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                DataPropertyName = "IsValid",
                HeaderText = "Valid",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingName",
                HeaderText = "Building",
                ReadOnly = true,
                DefaultCellStyle = dateColumnStyle,
                MinimumWidth = 80
            });


            dgItems.Columns.Add(new DataGridViewComboBoxColumn()
            {
                Name = "Account",
                HeaderText = "Ledger",
                ReadOnly = false,
                MinimumWidth = 120
            });

            dgItems.Columns.Add(new DataGridViewComboBoxColumn()
            {
                Name = "OwnTrust",
                DataPropertyName = "OwnTrustAccount",
                HeaderText = "Account",
                ReadOnly = false
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Amount",
                HeaderText = "Amount",
                ReadOnly = false,
                DefaultCellStyle = currencyColumnStyle,
                MinimumWidth = 80
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SupplierReference",
                HeaderText = "Supplier Reference",
                ReadOnly = false,
                MinimumWidth = 80
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "InvoiceNumber",
                DataPropertyName = "InvoiceNumber",
                HeaderText = "Invoice Number",
                ReadOnly = false,
                MinimumWidth = 80
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "InvoiceDateX",
                DataPropertyName = "InvoiceDate",
                HeaderText = "Invoice Date",
                ReadOnly = false,
                MinimumWidth = 80,
                DefaultCellStyle = dateColumnStyle
            });


            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Invoice",
                Text = "Upload",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 30
            });

            dgItems.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                DataPropertyName = "FileLoaded",
                HeaderText = "",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewComboBoxColumn()
            {
                Name = "Bank",
                DataPropertyName = "BankId",
                HeaderText = "Bank",
                ReadOnly = false
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BranchName",
                DataPropertyName = "BranchName",
                HeaderText = "Branch",
                ReadOnly = false
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BranchCode",
                DataPropertyName = "BranchCode",
                HeaderText = "Branch Code",
                ReadOnly = false
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "SupplierBankAccount",
                DataPropertyName = "SupplierBankAccount",
                HeaderText = "Account Number",
                ReadOnly = true
            });

            dgItems.DataSource = bs;

            dgItems.AutoResizeColumns();
        }

        private void dgItems_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            foreach (DataGridViewRow row in dgItems.Rows)
            {
                BuildingRequisitionItem reqItem = row.DataBoundItem as BuildingRequisitionItem;

                reqItem.DataRow = row;


                var comboBox = row.Cells["Account"] as DataGridViewComboBoxCell;
                comboBox.ReadOnly = false;
                comboBox.DataSource = reqItem.PastelAccountList;
                comboBox.DisplayMember = "DisplayString";
                comboBox.ValueMember = "AccountNumber";

                reqItem.InvoiceDateControl = row.Cells["InvoiceDateX"] as DataGridViewTextBoxCell;

                if (reqItem.SelectedAccount != null)
                    comboBox.Value = reqItem.SelectedAccount.AccountNumber;

                reqItem.ComboBox = comboBox;

                //load bank combo


                var bankCombo = row.Cells["Bank"] as DataGridViewComboBoxCell;
                bankCombo.ReadOnly = reqItem.BankAlreadyLinked;
                bankCombo.DataSource = reqItem.BankList;
                bankCombo.DisplayMember = "Name";
                bankCombo.ValueMember = "id";

                var trustCombo = row.Cells["OwnTrust"] as DataGridViewComboBoxCell;
                trustCombo.ReadOnly = false;
                trustCombo.DataSource = new List<string>() { "OWN", "TRUST" };
                reqItem.OwnTrustAccount = reqItem.IsOwnAccount ? "OWN" : "TRUST";

                var tbc = row.Cells["BranchCode"] as DataGridViewTextBoxCell;
                tbc.ReadOnly = reqItem.BankAlreadyLinked;

                tbc = row.Cells["BranchName"] as DataGridViewTextBoxCell;
                tbc.ReadOnly = reqItem.BankAlreadyLinked;

                tbc = row.Cells["SupplierBankAccount"] as DataGridViewTextBoxCell;
                tbc.ReadOnly = reqItem.BankAlreadyLinked;

            }
        }

        private void dgItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Controller.HandleError("Invalid value added");

            e.Cancel = true;
        }


        private bool IsValidPdf(string filepath)
        {
            bool Ret = true;

            PdfReader reader = null;

            try
            {
                using (reader = new PdfReader(filepath))
                {
                    reader.Close();
                }
            }
            catch
            {
                Ret = false;
            }

            return Ret;
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var item = senderGrid.Rows[e.RowIndex].DataBoundItem as BuildingRequisitionItem;


                if (ofdAttachment.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < ofdAttachment.FileNames.Count(); i++)
                    {
                        if (IsValidPdf(ofdAttachment.FileNames[i]))
                        {
                            item.AttachmentFileName = ofdAttachment.FileNames[i];
                            item.Refresh();
                        }
                        else
                        {
                            Controller.HandleError("Invalid PDF\n" + ofdAttachment.FileNames[i] + "\n Please load a different pdf");
                        }
                    }
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_SupplierBuildingList == null)
                return;

            if (_Supplier == null)
                return;

            if (trnDatePicker.Value.Date <= _minDate)
            {
                Controller.HandleError("Please select a tranasction date", "Validation Error");
                return;
            }


            var requisitionsToSave = _SupplierBuildingList.Where(a => a.Amount > 0).ToList();


            var x = requisitionsToSave.Where(a => a.IsValid == false).Count();
            if (x > 0)
            {
                Controller.HandleError("There are " + x.ToString() +
                    " invalid requisitions.\n Please check that all items\n" +
                    "To ignore a building clear the Amount column");
                return;
            }


            if (_AttachmentRequired)
            {
                int c = requisitionsToSave.Where(a => a.FileLoaded == false).Count();
                if (c > 0)
                {
                    Controller.HandleError("There are " + c.ToString() + " missing invoices.\n Please check that all invoices are loaded");
                    return;
                }
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {

                    //validate duplicates
                    foreach (var requisition in requisitionsToSave)
                    {

                        var q = (from r in context.tblRequisitions
                                 where r.SupplierId == _Supplier.id
                                 && r.InvoiceNumber == requisition.InvoiceNumber
                                 && r.amount == requisition.Amount
                                 select r);

                        if (q.Count() > 0)
                        {
                            Controller.HandleError("Duplicate requisition detected.\n" +
                                                   requisition.BuildingName + "\n" +
                                                   requisition.InvoiceNumber + " invoice has already been processed", "Validation Error");
                            this.Cursor = Cursors.Arrow;
                            return;
                        }
                    }

                    //Now save all the new requisitions

                    foreach (var requisition in requisitionsToSave)
                    {
                        var item = new tblRequisition();
                        context.tblRequisitions.Add(item);

                        if (!requisition.BankAlreadyLinked)
                        {
                            requisition.SupplierBank = _BankList.Where(a => a.id == requisition.BankId).Single().Name;
                        }

                        item.trnDate = trnDatePicker.Value.Date;
                        item.account = requisition.OwnTrustAccount;
                        item.reference = requisition.BuildingAbreviatio + (requisition.OwnTrustAccount == "TRUST" ? " (" + requisition.BuildingTrustAccount + ")" : "");
                        item.ledger = requisition.AccountNumberToDisplay;
                        item.amount = requisition.Amount.Value;
                        item.payreference = requisition.SupplierReference;
                        item.userID = Controller.user.id;
                        item.building = requisition.BuildingId;
                        item.SupplierId = _Supplier.id;
                        item.InvoiceNumber = requisition.InvoiceNumber;
                        item.InvoiceDate = dtInvoiceDate.Value;
                        item.BankName = requisition.SupplierBank;
                        item.BranchCode = requisition.BranchCode;
                        item.BranchName = requisition.BranchName;
                        item.AccountNumber = requisition.SupplierBankAccount;
                        item.processed = false;
                        if (!String.IsNullOrWhiteSpace(requisition.AttachmentFileName))
                        {
                            context.RequisitionDocumentSet.Add(new RequisitionDocument()
                            {
                                Requisition = item,
                                FileData = File.ReadAllBytes(requisition.AttachmentFileName),
                                FileName = requisition.AttachmentFileName,
                                IsInvoice = true
                            });
                        }

                        if (!requisition.BankAlreadyLinked)
                        {
                            //link the bank to the supplier
                            var supBank = new SupplierBuilding()
                            {
                                BuildingId = requisition.BuildingId,
                                AccountNumber = requisition.SupplierBankAccount,
                                BankId = requisition.BankId.Value,
                                BranceCode = requisition.BranchCode,
                                BranchName = requisition.BranchName,
                                SupplierId = _Supplier.id
                            };
                            context.SupplierBuildingSet.Add(supBank);
                            LoadBuildingAudit(supBank, context);
                        }
                    }

                    context.SaveChanges();
                    Controller.ShowMessage("Saved " + requisitionsToSave.Count().ToString() + " requisitions");
                    ClearSupplier();
                }

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void LoadBuildingAudit(SupplierBuilding updatedBuildingItem, DataContext context)
        {
            var bank = context.BankSet.Single(a => a.id == updatedBuildingItem.BankId);

            context.SupplierBuildingAuditSet.Add(new SupplierBuildingAudit()
            {
                SupplierBuilding = updatedBuildingItem,
                UserId = Controller.user.id,
                AuditTimeStamp = DateTime.Now,
                FieldName = "Bank",
                OldValue = null,
                NewValue = bank.Name
            });
            context.SupplierBuildingAuditSet.Add(new SupplierBuildingAudit()
            {
                SupplierBuilding = updatedBuildingItem,
                UserId = Controller.user.id,
                AuditTimeStamp = DateTime.Now,
                FieldName = "BranchName",
                OldValue = null,
                NewValue = updatedBuildingItem.BranchName
            });
            context.SupplierBuildingAuditSet.Add(new SupplierBuildingAudit()
            {
                SupplierBuilding = updatedBuildingItem,
                UserId = Controller.user.id,
                AuditTimeStamp = DateTime.Now,
                FieldName = "BranceCode",
                OldValue = null,
                NewValue = updatedBuildingItem.BranceCode
            });
            context.SupplierBuildingAuditSet.Add(new SupplierBuildingAudit()
            {
                SupplierBuilding = updatedBuildingItem,
                UserId = Controller.user.id,
                AuditTimeStamp = DateTime.Now,
                FieldName = "AccountNumber",
                OldValue = null,
                NewValue = updatedBuildingItem.AccountNumber,
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selItem = cbSupplierBankAccount.SelectedItem as SupplierBankAccountDetail;

            if (selItem != null)
            {
                if (_SupplierBuildingList != null && _SupplierBuildingList.Count > 0)
                {
                    var items = _SupplierBuildingList.Where(a => a.BankAlreadyLinked == false).ToList();
                    foreach (var itm in items)
                    {
                        itm.BankId = selItem.BankId;
                        itm.BranchCode = selItem.BranchCode;
                        itm.BranchName = selItem.BranchName;
                        itm.SupplierBankAccount = selItem.AccountNumber;
                        itm.Refresh();
                    }
                }
            }

        }
    }




    class BuildingRequisitionItem
    {
        public int BuildingId { get; set; }

        public string BuildingName { get; set; }

        public string BuildingDataPath { get; set; }

        public List<PastelAccount> PastelAccountList { get; set; }

        public PastelAccount SelectedAccount { get; set; }

        public string AccountNumberToUse
        {
            get
            {
                if (ComboBox == null)
                    return null;
                var acc = (ComboBox.Value as string);
                if (acc == null)
                    return null;
                return acc;
            }
        }

        public string AccountNumberToDisplay
        {
            get
            {
                if (AccountNumberToUse != null)
                {
                    var itm = PastelAccountList.SingleOrDefault(a => a.AccountNumber == AccountNumberToUse);
                    if (itm != null)
                        return itm.DisplayString;
                }
                return null;
            }
        }

        public decimal? Amount { get; set; }

        public string SupplierReference { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string AttachmentFileName { get; set; }

        public bool FileLoaded { get { return !String.IsNullOrWhiteSpace(AttachmentFileName); } }

        public string SupplierBankAccount { get; set; }

        public string SupplierBank { get; set; }

        public DataGridViewComboBoxCell ComboBox { get; set; }

     
        public void SetSelectedAccount(string accountNumber)
        {
            if (this.PastelAccountList != null && !String.IsNullOrWhiteSpace(accountNumber))
            {

                this.SelectedAccount = this.PastelAccountList.Where(a => a.AccountNumber == accountNumber).SingleOrDefault();

                if (ComboBox != null)
                {
                    if (SelectedAccount != null)
                        ComboBox.Value = SelectedAccount.AccountNumber;
                    else
                        ComboBox.Value = null;
                }

            }
            Refresh();
        }

        public DataGridViewTextBoxCell InvoiceDateControl { get; set; }

        public void SetSelectedInvoiceDate(DateTime invoiceDate)
        {
            this.InvoiceDate = invoiceDate;
            Refresh();
        }

        public bool IsValid
        {
            get
            {
                if(Amount == null)
                    return true;

                bool fileOk = FileLoaded;
                if (InvoiceAttachmentRequired == false)
                    fileOk = true;

                if (Amount > 0
                    && !String.IsNullOrWhiteSpace(SupplierReference)
                    && !String.IsNullOrWhiteSpace(InvoiceNumber)
                    && InvoiceDate > new DateTime(2000, 1, 1)
                    && fileOk
                    && ComboBox != null && ComboBox.Value != null
                    && !String.IsNullOrWhiteSpace( AccountNumberToUse)
                    && !String.IsNullOrWhiteSpace(OwnTrustAccount)
                    )
                {
                    if (BankAlreadyLinked)
                        return true;

                    return BankId > 0 && !String.IsNullOrWhiteSpace(BranchName) && !String.IsNullOrWhiteSpace(BranchCode) && !String.IsNullOrWhiteSpace(SupplierBankAccount);
                }
                else
                    return false;
            }
        }

        public DataGridViewRow DataRow { get;  set; }
        public bool InvoiceAttachmentRequired { get;  set; }
        public string BuildingAbreviatio { get;  set; }
        public string BuildingTrustAccount { get;  set; }
        public string BranchCode { get;  set; }
        public string BranchName { get;  set; }
        public string OwnTrustAccount { get; set; }
        private int? _BankId;
        public int? BankId
        {
            get
            {
                return _BankId;
            }
            set
            {
                _BankId = value;
                if(value != null && String.IsNullOrWhiteSpace(BranchCode) && String.IsNullOrWhiteSpace(BranchName))
                {
                    var b = BankList.SingleOrDefault(a => a.id == _BankId);
                    if(b != null && !String.IsNullOrWhiteSpace( b.BranchCode))
                    {
                        this.BranchCode = b.BranchCode;
                        this.BranchName = b.BranchName;
                        Refresh();
                    }
                }
            }
        }

        public List<Data.BankData.Bank> BankList { get;  set; }
        public bool BankAlreadyLinked { get; internal set; }
        public bool IsOwnAccount { get; internal set; }

        public void Refresh()
        {
            if (DataRow != null)
                DataRow.DataGridView.Refresh();
        }
    }

    class PastelAccount
    {
        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string DisplayString
        {
            get
            {
                return AccountNumber + ": " + AccountName;
            }
        }
    }

    class SupplierBankAccountDetail
    {
        public int BankId { get; set; }

        public string BranchCode { get; set; }

        public string BankName { get; set; }

        public string AccountNumber { get; set; }

        public string DisplayString { get { return BankName + ": " + AccountNumber; } }

        public string BranchName { get; internal set; }
    }
}

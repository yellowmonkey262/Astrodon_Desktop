using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astro.Library.Entities;
using Astrodon.ReportService;
using System.Data.Entity;
using Astrodon.Data.RequisitionData;
using Astrodon.Forms;
using System.IO;

namespace Astrodon.Controls.Maintenance
{
    public partial class usrMissingRequisitions : UserControl
    {
        private DataContext dataContext;
        private List<Building> _Buildings;
        private List<PastelMaintenanceTransaction> _Data;
        private int userid;
        private PastelMaintenanceTransaction _Item;
        private Data.SupplierData.Supplier _Supplier;
        private DateTime _minDate = new DateTime(2000, 1, 1);


        public usrMissingRequisitions(DataContext context)
        {
            dataContext = context;
            userid = Controller.user.id;
            _Data = new List<PastelMaintenanceTransaction>();
            InitializeComponent();
            LoadBuildings();
            ClearItem();
        }

        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));
                _Buildings = bManager.buildings;
                cmbBuilding.DataSource = _Buildings;
                cmbBuilding.ValueMember = "ID";
                cmbBuilding.DisplayMember = "Name";
                if (_Buildings.Count > 0)
                    cmbBuilding.SelectedIndex = 0;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnLoadRequisitions_Click(object sender, EventArgs e)
        {
            LoadRequisitions();
        }

        private void LoadRequisitions()
        {
            this.Cursor = Cursors.WaitCursor;
            var building = cmbBuilding.SelectedItem as Building;
            try
            {
                _Data = new List<PastelMaintenanceTransaction>();
                using (var reportService = new ReportServiceClient())
                {
                    var items = reportService.MissingMaintenanceRecordsGet(SqlDataHandler.GetConnectionString(), building.ID);
                    if (items == null || items.Length == 0)
                        Controller.ShowMessage("No transactions found", "Warning");
                    else
                    {
                        _Data = items.ToList();
                        BindDataGrid();
                    }
                }
            }
            catch (Exception e)
            {
                var settings = dataContext.tblSettings.FirstOrDefault();

                Controller.HandleError("Error loading requisitions, please confirm the ODBC setup for \n" + building.DataPath + "\n as well as \n" + settings.trust + "\n"+
                                       e.Message);

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

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";
            
            
            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _Data;

            dgItems.Columns.Clear();

            dgItems.DataSource = bs;

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 30
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TransactionDate",
                HeaderText = "Date",
                ReadOnly = true,
                DefaultCellStyle = dateColumnStyle,
                MinimumWidth = 80
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountType",
                HeaderText = "Type",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountDesc",
                HeaderText = "Account",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Amount",
                HeaderText = "Amount",
                ReadOnly = true,
                DefaultCellStyle = currencyColumnStyle,
                
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Reference",
                HeaderText = "Reference",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                ReadOnly = true
            });

            dgItems.AutoResizeColumns();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as PastelMaintenanceTransaction;

                if (_Item != null)
                {
                    EditItem();
                }
            }
        }

        private void EditItem()
        {
            _Documents.Clear();
            ClearSupplier();
            lbDate.Text = _Item.TransactionDate.ToString("yyyy/MM/dd");
            lbLedger.Text = _Item.AccountDesc;
            lbAmount.Text = _Item.Amount.ToString("###,##0.00");
            txtPaymentRef.Text = _Item.Reference;
            lbAccount.Text = _Item.AccountType;

            btnUploadInvoice.Visible = true;
            btnSupplierLookup.Visible = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dtInvoiceDate.Value <= _minDate)
            {
                Controller.HandleError("Invoice Date required for Maintenance. Please select a date.", "Validation Error");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtInvoiceNumber.Text))
            {
                Controller.HandleError("Invoice Number required for Maintenance. Please supply an invoice number.", "Validation Error");
                return;
            }
            if (_Documents.Count == 0)
            {
                Controller.HandleError("Invoice attachment required, please upload Invoice PDF", "Validation Error");
                return;
            }

            if(_Supplier == null)
            {
                Controller.HandleError("Supplier not selected, please select a supplier.", "Validation Error");
                return;
            }

            using (var context = SqlDataHandler.GetDataContext())
            {
                var building = cmbBuilding.SelectedItem as Building;
                var buildingId = building.ID;

                var bankDetails = context.SupplierBuildingSet
                                           .Include(a => a.Bank)
                                           .SingleOrDefault(a => a.BuildingId == buildingId && a.SupplierId == _Supplier.id);

                if (bankDetails == null)
                {
                    Controller.HandleError("Supplier banking details for this building is not configured.\n" +
                                        "Please capture bank details for this building on the suppier detail screen.", "Validation Error");
                    return;
                }

                var item = new tblRequisition()
                {
                    trnDate = _Item.TransactionDate,
                    PastelLedgerAutoNumber = _Item.AutoNumber,
                    PastelDataPath = _Item.DataPath,
                    account = _Item.AccountType,
                    reference = building.Abbr + " (" + _Item.Account + ")",
                    ledger = _Item.AccountDesc,
                    amount = _Item.Amount,
                    payreference = txtPaymentRef.Text,
                    userID = Controller.user.id,
                    building = buildingId,
                    SupplierId = _Supplier == null ? (int?)null : _Supplier.id,
                    InvoiceNumber = txtInvoiceNumber.Text,
                    InvoiceDate = dtInvoiceDate.Value,
                    BankName = _Supplier == null ? (string)null : bankDetails.Bank.Name,
                    BranchCode = _Supplier == null ? (string)null : bankDetails.BranceCode,
                    BranchName = _Supplier == null ? (string)null : bankDetails.BranchName,
                    AccountNumber = _Supplier == null ? (string)null : bankDetails.AccountNumber,
                    processed = true
                };

                context.tblRequisitions.Add(item);
                foreach (var key in _Documents.Keys)
                {
                    context.RequisitionDocumentSet.Add(new RequisitionDocument()
                    {
                        Requisition = item,
                        FileData = _Documents[key],
                        FileName = key,
                        IsInvoice = true
                    });
                }

                var config = (from c in context.BuildingMaintenanceConfigurationSet.Include(a => a.Building)
                              where c.BuildingId == item.building
                              && c.PastelAccountNumber == _Item.Account
                              select c).SingleOrDefault();

                if (config != null)
                {
                    if (item.SupplierId == null)
                    {
                        Controller.HandleError("Supplier required for Maintenance. Please select a supplier.", "Validation Error");
                        return;
                    }

                    //capture the maintenance as part of the same unit of work
                    var frmMaintenance = new frmMaintenanceDetail(context, item, config);
                    var dialogResult = frmMaintenance.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                        context.SaveChanges();
                }
                else
                {
                    context.SaveChanges();
                }
                _Data.Remove(_Item);
                ClearItem();
                BindDataGrid();
            }
        }

        private void ClearItem()
        {
            _Documents = new Dictionary<string, byte[]>();
            _Item = null;
            ClearSupplier();
            lbDate.Text ="";
            lbLedger.Text = "";
            lbAmount.Text = "";
            txtPaymentRef.Text = "";
            lbAccount.Text = "";
            txtInvoiceNumber.Text = "";
            dtInvoiceDate.Value = _minDate;
        }

        private void ClearSupplier()
        {
            _Supplier = null;
            lbSupplierName.Text = "";
            lbAccountNumber.Text = "";
            lbBankName.Text = "";
            btnSave.Enabled = false;
            btnUploadInvoice.Visible = false;
            btnSupplierLookup.Visible = false;
        }

        private void btnSupplierLookup_Click(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedIndex < 0)
            {
                Controller.HandleError("Please select a building first.", "Validation Error");
                return;

            }
            var building = cmbBuilding.SelectedItem as Building;
            var buildingId = building.ID;

            using (var context = SqlDataHandler.GetDataContext())
            {
                var frmSupplierLookup = new Astrodon.Forms.frmSupplierLookup(context, buildingId);

                var dialogResult = frmSupplierLookup.ShowDialog();
                var supplier = frmSupplierLookup.SelectedSupplier;

                if (dialogResult == DialogResult.OK && supplier != null)
                {
                    _Supplier = supplier;
                    lbSupplierName.Text = _Supplier.CompanyName;


                    var bankDetails = context.SupplierBuildingSet
                                             .Include(a => a.Bank)
                                             .SingleOrDefault(a => a.BuildingId == buildingId && a.SupplierId == _Supplier.id);
                    if (bankDetails == null)
                    {
                        Controller.HandleError("Supplier banking details for this building is not configured.\n" +
                                            "Please capture bank details for this building on the suppier detail screen.", "Validation Error");


                        var frmSupplierDetail = new Astrodon.Forms.frmSupplierDetail(context, _Supplier.id, buildingId);
                        frmSupplierDetail.ShowDialog();

                        bankDetails = context.SupplierBuildingSet
                                       .Include(a => a.Bank)
                                       .SingleOrDefault(a => a.BuildingId == buildingId && a.SupplierId == _Supplier.id);
                        if (bankDetails == null)
                        {
                            _Supplier = null;
                            return;
                        }

                    }

                    lbBankName.Text = bankDetails.Bank.Name + " (" + bankDetails.BranceCode + ")";
                    lbAccountNumber.Text = bankDetails.AccountNumber;
                    btnSave.Enabled = true;
                }
                else
                {
                    ClearSupplier();
                }
            }
        }

        private Dictionary<string, byte[]> _Documents = new Dictionary<string, byte[]>();
        private void btnUploadInvoice_Click(object sender, EventArgs e)
        {
            if (ofdAttachment.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < ofdAttachment.FileNames.Count(); i++)
                {
                    _Documents.Add(ofdAttachment.SafeFileNames[i], File.ReadAllBytes(ofdAttachment.FileNames[i]));
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Data.MaintenanceData;
using Astrodon.Data.SupplierData;
using Astradon.Data.Utility;
using Astrodon.Data.Base;
using Astrodon.Controls.Events;
using System.IO;
using System.Globalization;

namespace Astrodon.Controls.Maintenance
{
    public partial class usrMaintenanceDetail : UserControl
    {
        private DataContext _DataContext;
        private Astrodon.Data.MaintenanceData.Maintenance _Maintenance;
        private List<SupportingDocument> _Documents;
        private tblRequisition _requisition;
        private bool _Readonly = false;

        public usrMaintenanceDetail(DataContext context, int maintenanceId, bool readonlyScreen)
        {
            this.Cursor = Cursors.WaitCursor;
            _Readonly = readonlyScreen;
            try
            {
                _DataContext = context;

                _Maintenance = _DataContext.MaintenanceSet
                               .Include(a => a.Supplier)
                               .Include(a => a.Requisition)
                               .Include(a => a.BuildingMaintenanceConfiguration)
                               .Include(a => a.DetailItems)
                               .Single(a => a.id == maintenanceId);

                _requisition = _DataContext.tblRequisitions.Single(a => a.id == _Maintenance.RequisitionId);

                _Documents = _DataContext.MaintenanceDocumentSet
                             .Where(a => a.MaintenanceId == maintenanceId)
                             .Select(a => new SupportingDocument
                             {
                                 Id = a.id,
                                 FileName = a.FileName
                             }).ToList();

                SetupControl();

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public usrMaintenanceDetail(DataContext context, tblRequisition requisition, BuildingMaintenanceConfiguration config, bool ignoreInvoiceNumber = false)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                _DataContext = context;
                _requisition = requisition;

                if (!ignoreInvoiceNumber)
                {
                    if (string.IsNullOrEmpty(requisition.InvoiceNumber))
                        throw new MaintenanceException("No Invoice Number found.");

                    if (!requisition.InvoiceDate.HasValue)
                        throw new MaintenanceException("No Invoice Date found.");
                }

                if (requisition.SupplierId == null)
                    throw new MaintenanceException("No Supplier found.");
                else if (requisition.Supplier == null)
                    requisition.Supplier = _DataContext.SupplierSet.Single(a => a.id == requisition.SupplierId);

                _Maintenance = _DataContext.MaintenanceSet.Include(a => a.DetailItems).SingleOrDefault(a => a.RequisitionId == requisition.id);
                if (_Maintenance == null)
                {
                    _Maintenance = new Data.MaintenanceData.Maintenance()
                    {
                        DateLogged = DateTime.Now,
                        BuildingMaintenanceConfiguration = config,
                        Requisition = requisition,
                        Supplier = requisition.Supplier,
                        InvoiceNumber = requisition.InvoiceNumber,
                        InvoiceDate = requisition.InvoiceDate == null ? requisition.trnDate : requisition.InvoiceDate.Value,
                        TotalAmount = requisition.amount,
                        WarrentyExpires = requisition.InvoiceDate == null ? requisition.trnDate : requisition.InvoiceDate.Value,
                        DetailItems = new List<MaintenanceDetailItem>()
                    };
                    _DataContext.MaintenanceSet.Add(_Maintenance);
                    _Documents = new List<SupportingDocument>();
                }
                else
                {
                    _Documents = _DataContext.MaintenanceDocumentSet.Where(a => a.MaintenanceId == _Maintenance.id)
                        .Select(d => new SupportingDocument()
                        {
                            Id = d.id,
                            FileName = d.FileName
                        }).ToList();
                }

                SetupControl();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SetupControl()
        {
            if (_Maintenance.BuildingMaintenanceConfiguration.Building == null)
                _Maintenance.BuildingMaintenanceConfiguration.Building = _DataContext.tblBuildings.Single(a => a.id == _Maintenance.BuildingMaintenanceConfiguration.BuildingId);

            InitializeComponent();


            BindInputs();
            BindCustomers();
            BindWarrantyDurationType();
            BindDocumentsDataGrid();
            if (_Readonly)
            {
                btnSave.Enabled = false;
                btnBrowse.Enabled = false;
                dtpInvoiceDate.Enabled = false;
                txtDescription.Enabled = false;
                txtSerialNumber.Enabled = false;
                txtWarrantyNotes.Enabled = false;
                cbUnit.Enabled = false;
                tbInvoiceNumber.Enabled = false;
                cbWarrantyDurationType.Enabled = false;
                numWarrantyDuration.Enabled = false;
            }
            timer1.Enabled = true;
        }

        #region Custom Events

        public event SaveResultEventHandler SaveResultEvent;

        private void RaiseSaveSuccess()
        {
            if (SaveResultEvent != null)
                SaveResultEvent(this, new SaveResultEventArgs(true));
        }

        private void RaiseCancel()
        {
            if (SaveResultEvent != null)
                SaveResultEvent(this, new SaveResultEventArgs());
        }

        #endregion

        private void numWarrantyDuration_ValueChanged(object sender, EventArgs e)
        {
            lblWarrantyExpires.Text = CalculateWarrantyExpires().ToString("yyyy/MM/dd");
        }

        private void cbWarrantyDurationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblWarrantyExpires.Text = CalculateWarrantyExpires().ToString("yyyy/MM/dd");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (ofdAttachment.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < ofdAttachment.FileNames.Count(); i++)
                {
                    _Documents.Add(new SupportingDocument()
                    {
                        Id = null,
                        FileName = ofdAttachment.SafeFileNames[i],
                        FilePath = ofdAttachment.FileNames[i]
                    });
                }

                BindDocumentsDataGrid();
            }
        }

        private void dgSupportingDocuments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            int removeColumnIndex = senderGrid.Columns.Count - 2;
            int downloadColumnIndex = senderGrid.Columns.Count - 1;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var selectedDocument = senderGrid.Rows[e.RowIndex].DataBoundItem as SupportingDocument;

                if (selectedDocument != null)
                {
                    if (e.ColumnIndex == removeColumnIndex)
                    {
                        if (selectedDocument.Id.HasValue)
                        {
                            var document = _DataContext.MaintenanceDocumentSet.Single(a => a.id == selectedDocument.Id);
                            _DataContext.MaintenanceDocumentSet.Remove(document);
                        }

                        _Documents.Remove(selectedDocument);

                        BindDocumentsDataGrid();
                    }
                    else if (e.ColumnIndex == downloadColumnIndex)
                    {
                        if (!selectedDocument.Id.HasValue)
                        {
                            Controller.HandleError("Cannot download documents that haven't been saved.");
                        }
                        else
                        {
                            if (sfdDownloadAttachment.ShowDialog() == DialogResult.OK)
                            {
                                var document = _DataContext.MaintenanceDocumentSet.Single(a => a.id == selectedDocument.Id.Value);

                                using (var file = sfdDownloadAttachment.OpenFile())
                                {
                                    file.Write(document.FileData, 0, document.FileData.Length);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if(cbUnit.SelectedIndex == 1)
                {
                    //multiple items
                    //remove all items assigned to the unit and clear all amounts
                    List<MaintenanceDetailItem> toRemove = new List<MaintenanceDetailItem>();
                    foreach (var dcurr in _Maintenance.DetailItems)
                    {
                        var x = _MaintenanceCustomers.Where(a => a.Id == dcurr.id).SingleOrDefault();
                        if (x != null)
                        {
                            if (x.Amount > 0)
                            {
                                dcurr.Amount = x.Amount.Value;
                                dcurr.CustomerAccount = x.Account;
                                dcurr.IsForBodyCorporate = x.IsBodyCorporate;
                            }
                            else
                                toRemove.Add(dcurr);

                        }
                    }
                    foreach (var dcurr in toRemove)
                        _Maintenance.DetailItems.Remove(dcurr);

                    _DataContext.MaintenanceDetailItemSet.RemoveRange(toRemove);

                    //add all items not yet in the list
                    foreach(var itm in _MaintenanceCustomers.Where(a => a.Amount > 0 && a.Id == null))
                    {
                        _DataContext.MaintenanceDetailItemSet.Add(new MaintenanceDetailItem()
                        {
                            MaintenanceId = _Maintenance.id,
                            Maintenance = _Maintenance,
                            Amount = itm.Amount.Value,
                            CustomerAccount = itm.Account,
                            IsForBodyCorporate = itm.IsBodyCorporate
                        });
                    }
                }
                else
                {
                    //single item
                    var item = _Maintenance.DetailItems.FirstOrDefault();

                    if(item != null)
                    {
                        var allOthers = _Maintenance.DetailItems.Where(a => a.id != item.id).ToList();
                        _DataContext.MaintenanceDetailItemSet.RemoveRange(allOthers);
                        foreach (var itm in allOthers)
                            _Maintenance.DetailItems.Remove(itm);
                    }
                    else
                    {
                        item = new MaintenanceDetailItem()
                        {
                            MaintenanceId = _Maintenance.id,
                            Maintenance = _Maintenance
                        };
                        _Maintenance.DetailItems.Add(item);
                    }

                    if (cbUnit.SelectedIndex == 0)
                    {
                        //body corporate
                        item.CustomerAccount = MaintenanceDetailItem.BodyCorporateAccountName;
                        item.IsForBodyCorporate = true;
                        item.Amount = _Maintenance.TotalAmount;
                    }
                    else
                    {
                        var selectedUnit = cbUnit.SelectedItem as StringKeyValue;
                        item.CustomerAccount = selectedUnit.Id;
                        item.IsForBodyCorporate = false;
                        item.Amount = _Maintenance.TotalAmount;
                    }
                }

                if(_Maintenance.TotalAmount != _Maintenance.DetailItems.Sum(a => a.Amount))
                {

                    this.Cursor = Cursors.Default;
                    Controller.HandleError("Full amount not assigned, please check the total split");
                    return;
                }

                var cntNegative = _MaintenanceCustomers.Count(a => a.Amount < 0);
                if (cntNegative > 0)
                {
                    btnSave.Enabled = false;
                    this.Cursor = Cursors.Default;
                    Controller.HandleError("Negative maintenance amounts are not allowed");
                    return;
                }

                if (string.IsNullOrEmpty(tbInvoiceNumber.Text))
                {
                    this.Cursor = Cursors.Default;
                    Controller.HandleError("No Invoice Number found.");
                    return;
                }

           

                _Maintenance.Description = txtDescription.Text;
                _Maintenance.WarrantyDuration = Convert.ToInt32(numWarrantyDuration.Value);
                _Maintenance.WarrantyDurationType = ((KeyValuePair<DurationType, string>)cbWarrantyDurationType.SelectedItem).Key;
                _Maintenance.WarrentyExpires = CalculateWarrantyExpires();
                _Maintenance.WarrantySerialNumber = txtSerialNumber.Text;
                _Maintenance.WarrantyNotes = txtWarrantyNotes.Text;
                _Maintenance.InvoiceDate = dtpInvoiceDate.Value;
                _Maintenance.InvoiceNumber = tbInvoiceNumber.Text;
                _requisition.InvoiceDate = dtpInvoiceDate.Value;
                _requisition.InvoiceNumber = tbInvoiceNumber.Text;


                _DataContext.SaveChanges();

                foreach (var document in _Documents.Where(a => a.Id == null))
                {
                    _DataContext.MaintenanceDocumentSet.Add(new MaintenanceDocument()
                    {
                        MaintenanceId = _Maintenance.id,
                        FileName = document.FileName,
                        FileData = File.ReadAllBytes(document.FilePath)
                    });

                    _DataContext.SaveChanges();
                }
                RaiseSaveSuccess();
            }
            catch (Exception ex)
            {
                Controller.HandleError("An error occured saving the record." + " " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            RaiseCancel();
        }

        #region Helper Functions

        private void BindInputs()
        {
            lblBuildingName.Text = _Maintenance.BuildingMaintenanceConfiguration.Building.Building;
            lblMaintenanceType.Text = _Maintenance.BuildingMaintenanceConfiguration.Name;
            lblLedgerAccount.Text = _Maintenance.BuildingMaintenanceConfiguration.PastelAccountName;
            lblClassification.Text = _Maintenance.BuildingMaintenanceConfiguration.Classification;
            lblTotalAmount.Text = "R" + _Maintenance.TotalAmount.ToString("#,###.00");
            txtDescription.Text = _Maintenance.Description;
            lblSupplier.Text = _Maintenance.Supplier.CompanyName;
            lblCompanyReg.Text = _Maintenance.Supplier.CompanyRegistration;
            lblVat.Text = _Maintenance.Supplier.VATNumber;
            lblContactPerson.Text = _Maintenance.Supplier.ContactPerson;
            lblContactNumber.Text = _Maintenance.Supplier.ContactNumber;
            lblEmail.Text = _Maintenance.Supplier.EmailAddress;
            tbInvoiceNumber.Text = _Maintenance.InvoiceNumber;
            dtpInvoiceDate.Value = _Maintenance.InvoiceDate;
            lblWarrantyExpires.Text = _Maintenance.WarrentyExpires.Value.ToString("yyyy/MM/dd");
            txtSerialNumber.Text = _Maintenance.WarrantySerialNumber;
            txtWarrantyNotes.Text = _Maintenance.WarrantyNotes;
        }

        private List<MaintenanceCustomer> _MaintenanceCustomers = new List<MaintenanceCustomer>();
        private List<string> _BuildingCustomers = new List<string>();

        private void BindCustomers()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                try
                {
                    var customers = new List<StringKeyValue>();
                    customers.Add(new StringKeyValue() { Id = string.Empty, Value = "Body Corporate" });
                    customers.Add(new StringKeyValue() { Id = string.Empty, Value = "Multiple Units" });

                    _BuildingCustomers = Controller.pastel.GetCustomers(_Maintenance.BuildingMaintenanceConfiguration.Building.DataPath);

                    customers.AddRange(_BuildingCustomers.Select(a => new StringKeyValue()
                    {
                        Id = a.Split('|')[2],
                        Value = a.Split('|')[3]
                    }));

                    cbUnit.DataSource = customers;
                    cbUnit.ValueMember = "Id";
                    cbUnit.DisplayMember = "Display";
                    LoadBuildingCustomers();
                    if (_Maintenance.DetailItems.Count > 0)
                    {
                        if (_Maintenance.DetailItems.Count() == 1)
                        {
                            var item = _Maintenance.DetailItems.First();
                            if (item.IsForBodyCorporate)
                                cbUnit.SelectedIndex = 0;
                            else
                                cbUnit.SelectedIndex = cbUnit.FindString(item.CustomerAccount);

                        }
                        else
                        {
                            cbUnit.SelectedIndex = 1;
                            tbUnits.Show();
                        }
                    }
                    LoadBuildingCustomers();

                }
                catch (Exception e)
                {
                    Controller.HandleError(e);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void LoadBuildingCustomers()
        {

            _MaintenanceCustomers = _BuildingCustomers.Select(a => new MaintenanceCustomer()
            {
                Account = a.Split('|')[2],
                Name = a.Split('|')[3],
                IsBodyCorporate = false,
                Amount = null
            }).OrderBy(a => a.Account).ToList();

            _MaintenanceCustomers.Insert(0, new MaintenanceCustomer()
            {
                Account = MaintenanceDetailItem.BodyCorporateAccountName,
                IsBodyCorporate = true,
                Name = "Body Corporate"
            });

            //merge with existing items

            foreach (var item in _Maintenance.DetailItems)
            {
                var x = _MaintenanceCustomers.Where(a => a.Account == item.CustomerAccount).SingleOrDefault();
                if (x != null)
                {
                    x.Id = item.id;
                    x.Amount = item.Amount;
                }
            }
            BindCustomerGrid();
        }

        private void BindCustomerGrid()
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
            bs.DataSource = _MaintenanceCustomers;

            dgItems.Columns.Clear();
            dgItems.ReadOnly = false;
            dgItems.EditMode = DataGridViewEditMode.EditOnEnter;

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Account",
                HeaderText = "Account",
                ReadOnly = true,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                ReadOnly = true,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Amount",
                HeaderText = "Amount",
                ReadOnly = _Readonly,
            });

            dgItems.DataSource = bs;

         
        }

        private void BindWarrantyDurationType()
        {
            var durationTypes = new List<KeyValuePair<DurationType, string>>();

            foreach (DurationType item in Enum.GetValues(typeof(DurationType)))
            {
                durationTypes.Add(new KeyValuePair<DurationType, string>(item, item.ToString()));
            }

            cbWarrantyDurationType.DataSource = durationTypes;
            cbWarrantyDurationType.ValueMember = "Key";
            cbWarrantyDurationType.DisplayMember = "Value";
        }

        private void BindDocumentsDataGrid()
        {
            dgSupportingDocuments.ClearSelection();
            dgSupportingDocuments.MultiSelect = false;
            dgSupportingDocuments.AutoGenerateColumns = false;

            dgSupportingDocuments.Columns.Clear();
            dgSupportingDocuments.DataSource = null;

            if (_Documents != null && _Documents.Count > 0)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = _Documents;

                dgSupportingDocuments.DataSource = bs;

                dgSupportingDocuments.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "FileName",
                    HeaderText = "File",
                    ReadOnly = true
                });

                dgSupportingDocuments.Columns.Add(new DataGridViewButtonColumn()
                {
                    HeaderText = "Action",
                    Text = "Remove",
                    UseColumnTextForButtonValue = true
                });

                dgSupportingDocuments.Columns.Add(new DataGridViewButtonColumn()
                {
                    HeaderText = "Action",
                    Text = "Download",
                    UseColumnTextForButtonValue = true
                });
            }
        }

        private DateTime CalculateWarrantyExpires()
        {
            var warrantyDurationType = DurationType.Day;

            int warrantyDuration = Convert.ToInt32(numWarrantyDuration.Value);
            if (cbWarrantyDurationType.SelectedItem != null)
                warrantyDurationType = ((KeyValuePair<DurationType, string>)cbWarrantyDurationType.SelectedItem).Key;

            var dateUsed = dtpInvoiceDate.Value;

            switch (warrantyDurationType)
            {
                case DurationType.Day:
                    return dateUsed.AddDays(warrantyDuration);
                case DurationType.Week:
                    return dateUsed.AddDays(warrantyDuration * 7);
                case DurationType.Month:
                    return dateUsed.AddMonths(warrantyDuration);
                case DurationType.Year:
                    return dateUsed.AddYears(warrantyDuration);
                default:
                    throw new Exception("Unsupported Duration Type supplied.");
            }
        }

        #endregion

        private void dtpInvoiceDate_ValueChanged(object sender, EventArgs e)
        {
            lblWarrantyExpires.Text = CalculateWarrantyExpires().ToString("yyyy/MM/dd");
        }

        private void dgItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Controller.HandleError("Invalid value added");

            e.Cancel = true;
        }

        private void dgItems_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgItems.Rows)
            {
                MaintenanceCustomer reqItem = row.DataBoundItem as MaintenanceCustomer;
                reqItem.DataRow = row;
                reqItem.Form = this;
            }
        }

        private void cbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUnit.SelectedIndex == 1)
                ShowUnits();
            else
                HideUnits();
        }

        private void HideUnits()
        {
            UpdateTotalAmount();
            if (tbMain.TabPages.Count > 1)
                tbMain.TabPages.Remove(tbUnits);
        }

        private void ShowUnits()
        {
            if (tbMain.TabPages.Count < 2)
                tbMain.TabPages.Add(tbUnits);
            UpdateTotalAmount();
        }

        public void UpdateTotalAmount()
        {
            if (cbUnit.SelectedIndex == 1)
            {
                btnSave.Enabled = false;
                var total = _MaintenanceCustomers.Where(a => a.Amount > 0).Sum(a => a.Amount.Value);
                lbTotalAmount.Text ="R" + total.ToString("#,##0.00", CultureInfo.InvariantCulture) + " of R" + _Maintenance.TotalAmount.ToString("#,##0.00", CultureInfo.InvariantCulture) + " assigned";
                btnSave.Enabled = false;
                if (total < _Maintenance.TotalAmount)
                {
                    lbTotalAmount.Text = lbTotalAmount.Text + " R" + (_Maintenance.TotalAmount - total).ToString("#,##0.00", CultureInfo.InvariantCulture) + " short";
                }
                else if (total > _Maintenance.TotalAmount)
                    lbTotalAmount.Text = lbTotalAmount.Text + " R" + (total - _Maintenance.TotalAmount).ToString("#,##0.00", CultureInfo.InvariantCulture) + " over";
                else
                    btnSave.Enabled = true;

                var x = _MaintenanceCustomers.Count(a => a.Amount < 0);
                if(x > 0)
                {
                    btnSave.Enabled = false;
                    Controller.HandleError("Negative maintenance amounts are not allowed");
                }
            }
            else
            {
                btnSave.Enabled = true;
                lbTotalAmount.Text = " not applicable";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            dgItems.AutoResizeColumns();
            cbUnit_SelectedIndexChanged(this, EventArgs.Empty);
        }
    }

    public class SupportingDocument
    {
        public int? Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }

    class MaintenanceCustomer
    {
        public int? Id { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        public bool IsBodyCorporate { get; set; }

        private decimal? _Amount;
        public decimal? Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                if (Form != null)
                {
                    Form.UpdateTotalAmount();

                }
            }
        }

        public DataGridViewRow DataRow { get; set; }
        public usrMaintenanceDetail Form { get; set; }

        public void Refresh()
        {
            if (DataRow != null)
                DataRow.DataGridView.Refresh();
        }
    }
}


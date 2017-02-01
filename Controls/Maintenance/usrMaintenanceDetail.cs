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

namespace Astrodon.Controls.Maintenance
{
    public partial class usrMaintenanceDetail : UserControl
    {
        private DataContext _DataContext;
        private Astrodon.Data.MaintenanceData.Maintenance _Maintenance;
        private List<SupportingDocument> _Documents;

        public usrMaintenanceDetail(DataContext context, int maintenanceId)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                _DataContext = context;

                _Maintenance = _DataContext.MaintenanceSet
                               .Include(a => a.Supplier)
                               .Include(a => a.Requisition)
                               .Include(a => a.BuildingMaintenanceConfiguration)
                               .Single(a => a.id == maintenanceId);

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

        public usrMaintenanceDetail(DataContext context, tblRequisition requisition, BuildingMaintenanceConfiguration config)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                _DataContext = context;

                if (string.IsNullOrEmpty(requisition.InvoiceNumber))
                    throw new MaintenanceException("No Invoice Number found.");

                if (!requisition.InvoiceDate.HasValue)
                    throw new MaintenanceException("No Invoice Date found.");

                if (requisition.SupplierId == null)
                    throw new MaintenanceException("No Supplier found.");
                else if (requisition.Supplier == null)
                    requisition.Supplier = _DataContext.SupplierSet.Single(a => a.id == requisition.SupplierId);

                _Maintenance = new Data.MaintenanceData.Maintenance()
                {
                    DateLogged = DateTime.Now,
                    BuildingMaintenanceConfiguration = config,
                    Requisition = requisition,
                    Supplier = requisition.Supplier,
                    InvoiceNumber = requisition.InvoiceNumber,
                    InvoiceDate = requisition.InvoiceDate.Value,
                    TotalAmount = requisition.amount,
                    WarrentyExpires = requisition.InvoiceDate.Value
                };

                _DataContext.MaintenanceSet.Add(_Maintenance);

                _Documents = new List<SupportingDocument>();

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
            lblWarrantyExpires.Text =  CalculateWarrantyExpires().ToString("yyyy/MM/dd");
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
                        if(selectedDocument.Id.HasValue)
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
                var selectedUnit = cbUnit.SelectedItem as StringKeyValue;

                if (string.IsNullOrEmpty(selectedUnit.Id))
                {
                    _Maintenance.IsForBodyCorporate = true;
                    _Maintenance.CustomerAccount = string.Empty;
                }
                else
                {
                    _Maintenance.IsForBodyCorporate = false;
                    _Maintenance.CustomerAccount = selectedUnit.Id;
                }

                _Maintenance.Description = txtDescription.Text;
                _Maintenance.WarrantyDuration = Convert.ToInt32(numWarrantyDuration.Value);
                _Maintenance.WarrantyDurationType = ((KeyValuePair<DurationType, string>)cbWarrantyDurationType.SelectedItem).Key;
                _Maintenance.WarrentyExpires = CalculateWarrantyExpires();
                _Maintenance.WarrantySerialNumber = txtSerialNumber.Text;
                _Maintenance.WarrantyNotes = txtWarrantyNotes.Text;

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
            }
            catch (Exception ex)
            {
                Controller.HandleError("An error occured saving the record.");
            }
            finally
            {
                this.Cursor = Cursors.Default;
                RaiseSaveSuccess();
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
            lblInvoiceNumber.Text = _Maintenance.InvoiceNumber;
            lblInvoiceDate.Text = _Maintenance.InvoiceDate.ToString("yyyy/MM/dd");
            lblWarrantyExpires.Text = _Maintenance.WarrentyExpires.Value.ToString("yyyy/MM/dd");
            txtSerialNumber.Text = _Maintenance.WarrantySerialNumber;
            txtWarrantyNotes.Text = _Maintenance.WarrantyNotes;
        }

        private void BindCustomers()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                try
                {
                    var customers = new List<StringKeyValue>();
                    customers.Add(new StringKeyValue() { Id = string.Empty, Value = "Body Corporate" });

                    var loadedCustomers = Controller.pastel.GetCustomers(_Maintenance.BuildingMaintenanceConfiguration.Building.DataPath);

                    customers.AddRange(loadedCustomers.Select(a => new StringKeyValue()
                    {
                        Id = a.Split('|')[2],
                        Value = a.Split('|')[3]
                    }));

                    cbUnit.DataSource = customers;
                    cbUnit.ValueMember = "Id";
                    cbUnit.DisplayMember = "Display";

                    if (!string.IsNullOrEmpty(_Maintenance.CustomerAccount))
                        cbUnit.SelectedIndex = cbUnit.FindString(_Maintenance.CustomerAccount);
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

            if (_Documents.Count > 0)
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
            int warrantyDuration = Convert.ToInt32(numWarrantyDuration.Value);
            var warrantyDurationType = ((KeyValuePair<DurationType, string>)cbWarrantyDurationType.SelectedItem).Key;

            switch (warrantyDurationType)
            {
                case DurationType.Day:
                    return _Maintenance.InvoiceDate.AddDays(warrantyDuration);
                case DurationType.Week:
                    return _Maintenance.InvoiceDate.AddDays(warrantyDuration * 7);
                case DurationType.Month:
                    return _Maintenance.InvoiceDate.AddMonths(warrantyDuration);
                case DurationType.Year:
                    return _Maintenance.InvoiceDate.AddYears(warrantyDuration);
                default:
                    throw new Exception("Unsupported Duration Type supplied.");
            }
        }

        #endregion
    }

    public class SupportingDocument
    {
        public int? Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }
}

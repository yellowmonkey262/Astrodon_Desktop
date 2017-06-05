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
using Astrodon.Data.RequisitionData;
using Astrodon.ReportService;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.Entity;

namespace Astrodon.Controls.Requisitions
{
    public partial class usrRequisitionBatch : UserControl
    {
        private List<Building> _Buildings;
        private List<BatchItem> _Data;
        private List<RequisitionItem> _PendingRequisitions;

        public usrRequisitionBatch()
        {
            InitializeComponent();
            lbProcessing.Text = "";
            _Data = new List<BatchItem>();
            LoadBuildings();
            LoadGrid();
            LoadPendingRequisions();
        }

        private void LoadBuildings()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var userid = Controller.user.id;
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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                var building = cmbBuilding.SelectedItem as Building;
                var batch = CreateRequisitionBatch(building.ID);
                if (batch != null)
                {
                    LoadGrid();
                    DownloadReport(batch.id);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnDownload.Enabled = true;
            }
        }

        private RequisitionBatch CreateRequisitionBatch(int buildingId, bool warnIfNoRequisitions = true)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                int batchNumber = 0;
                var requisitions = context.tblRequisitions.Where(a => a.building == buildingId && a.processed == false).ToList();
                if (requisitions.Count <= 0)
                {
                    if(warnIfNoRequisitions)
                      Controller.ShowMessage("There are no outstanding requisitions to process.");

                    return null;
                }
                var previousBatch = context.RequisitionBatchSet.Where(a => a.BuildingId == buildingId).OrderByDescending(a => a.BatchNumber).FirstOrDefault();
                if (previousBatch == null)
                    batchNumber = 1;
                else
                    batchNumber = previousBatch.BatchNumber + 1;

                //find all requisitions

                var batch = new RequisitionBatch()
                {
                    BuildingId = buildingId,
                    BatchNumber = batchNumber,
                    UserId = Controller.user.id,
                    Created = DateTime.Now,
                    Entries = requisitions.Count()
                };

                foreach (var requisition in requisitions)
                {
                    requisition.RequisitionBatch = batch;
                    requisition.processed = true;
                }

                context.RequisitionBatchSet.Add(batch);

                context.SaveChanges();

                return batch;


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
                Text = "Download",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 30
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Created",
                HeaderText = "Created",
                ReadOnly = true,
                DefaultCellStyle = dateColumnStyle,
                MinimumWidth = 80
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CreatedBy",
                HeaderText = "Created By",
                ReadOnly = true,
                DefaultCellStyle = dateColumnStyle,
                MinimumWidth = 80
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Building",
                HeaderText = "Building",
                ReadOnly = true
            });
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BatchNumber",
                HeaderText = "BatchNumber",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Entries",
                HeaderText = "Entries",
                ReadOnly = true
            });


            dgItems.AutoResizeColumns();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var item = senderGrid.Rows[e.RowIndex].DataBoundItem as BatchItem;
                if (item != null)
                {
                    try
                    {

                        DownloadReport(item.Id);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void LoadGrid()
        {
            var building = cmbBuilding.SelectedItem as Building;
            if (building == null)
                return;
            using (var context = SqlDataHandler.GetDataContext())
            {
                _Data = context.RequisitionBatchSet
                        .Where(a => a.BuildingId == building.ID)
                        .Select(b => new BatchItem()
                        {
                            Id = b.id,
                            Building = b.Building.Building,
                            Created = b.Created,
                            BatchNumber = b.BatchNumber,
                            Entries = b.Entries,
                            CreatedBy = b.UserCreated.name
                        }).OrderByDescending(a => a.Created).Take(400).ToList();
            }
            BindDataGrid();
        }

        private byte[] CreateReport(int requisitionBatchId, out byte[] combinedReport)
        {
            combinedReport = null;
            using (var reportService = ReportServiceClient.CreateInstance())
            {
                var reportData = reportService.RequisitionBatchReport(SqlDataHandler.GetConnectionString(), requisitionBatchId);
                if (reportData != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (Document doc = new Document())
                        {
                            using (PdfCopy copy = new PdfCopy(doc, ms))
                            {
                                doc.Open();

                                AddPdfDocument(copy, reportData);

                                using (var context = SqlDataHandler.GetDataContext())
                                {
                                    foreach (var requisitionId in context.tblRequisitions.Where(a => a.RequisitionBatchId == requisitionBatchId).OrderBy(a => a.trnDate).Select(a => a.id).ToList())
                                    {
                                        foreach (var invoice in context.RequisitionDocumentSet.Where(a => a.RequisitionId == requisitionId && a.IsInvoice == true).Select(a => a.FileData).ToList())
                                        {
                                            AddPdfDocument(copy, invoice);
                                            Application.DoEvents();
                                        }
                                    }
                                }
                            }
                        }
                        combinedReport = ms.ToArray();
                    }
                }
                return reportData;
            }
        }

        private void DownloadReport(int requisitionBatchId)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                byte[] combinedReport;
                var reportData = CreateReport(requisitionBatchId, out combinedReport);
                File.WriteAllBytes(dlgSave.FileName, combinedReport);
                Process.Start(dlgSave.FileName);
            }
        }

        private void AddPdfDocument(PdfCopy copy, byte[] document)
        {
            PdfReader.unethicalreading = true;
            using (PdfReader reader = new PdfReader(document))
            {
                PdfReader.unethicalreading = true;
                int n = reader.NumberOfPages;
                for (int page = 0; page < n;)
                {
                    copy.AddPage(copy.GetImportedPage(reader, ++page));
                }
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbBuilding.SelectedItem != null)
            {

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    LoadGrid();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void LoadPendingRequisions()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var buildingIds = _Buildings.Select(a => a.ID).ToArray();
                var qry = from b in context.tblBuildings
                          join r in context.tblRequisitions on b.id equals r.building
                          where r.processed == false
                          && buildingIds.Contains(b.id)
                          select new RequisitionItem()
                          {
                              Building = b.Building,
                              BuildingCode = b.Code,
                              Bank = r.BankName,
                              BranchCode = r.BranchCode,
                              AccountNumber = r.AccountNumber,
                              SupplierName = r.Supplier != null ? r.Supplier.CompanyName : r.contractor,
                              LedgerAccount = r.ledger,
                              Amount = r.amount,
                              SupplierReference = r.payreference,
                              InvoiceNumber = r.InvoiceNumber
                          };
                _PendingRequisitions = qry.OrderBy(a => a.Building).ThenBy(a => a.SupplierName).ToList();
                LoadPendingRequisitionsGrid();
            }
        }

        private void LoadPendingRequisitionsGrid()
        {
            dgPendingTransactions.ClearSelection();
            dgPendingTransactions.MultiSelect = false;
            dgPendingTransactions.AutoGenerateColumns = false;

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";


            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _PendingRequisitions;

            dgPendingTransactions.Columns.Clear();

            dgPendingTransactions.DataSource = bs;

            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Building",
                HeaderText = "Building",
                ReadOnly = true,
            });
            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "InvoiceNumber",
                HeaderText = "Invoice",
                ReadOnly = true,
            });
            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "LedgerAccount",
                HeaderText = "Ledger",
                ReadOnly = true
            });
            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SupplierName",
                HeaderText = "Supplier",
                ReadOnly = true
            });

            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SupplierReference",
                HeaderText = "Reference",
                ReadOnly = true
            });

            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Amount",
                HeaderText = "Amount",
                ReadOnly = true,
                DefaultCellStyle = currencyColumnStyle
            });

            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Bank",
                HeaderText = "Bank",
                ReadOnly = true
            });
            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountNumber",
                HeaderText = "Account",
                ReadOnly = true
            });
            dgPendingTransactions.AutoResizeColumns();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to process these requisitions?", "Question", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes)
                return;
            this.Cursor = Cursors.WaitCursor;
            button1.Enabled = false;
            try
            {
                int emailCount = 0;

                using (var context = SqlDataHandler.GetDataContext())
                {

                    var buildingIds = _Buildings.Select(a => a.ID).ToArray();
                    var qry = from b in context.tblBuildings
                              join r in context.tblRequisitions on b.id equals r.building
                              where r.processed == false
                              && buildingIds.Contains(b.id)
                             select b;

                    var buildings = qry.Distinct().ToList();
                    foreach(var b in buildings)
                    {
                        if(!b.CheckIfFolderExists())
                        {
                            Controller.HandleError("Unable to access " + b.DataFolder);
                            button1.Enabled = true;
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }

                    MemoryStream ms;
                    Document doc;
                    PdfCopy copy;
                    using (ms = new MemoryStream())
                    {
                        using (doc = new Document())
                        {
                            using (copy = new PdfCopy(doc, ms))
                            {
                                doc.Open();

                                foreach (var building in buildings)
                                {
                                    try
                                    {
                                        lbProcessing.Text = "Processing " + building.Building;
                                        Application.DoEvents();
                                        var batch = CreateRequisitionBatch(building.id, false);
                                        if (batch != null)
                                        {
                                            byte[] combinedReport;
                                            var reportData = CreateReport(batch.id, out combinedReport);
                                            if (combinedReport != null && reportData != null)
                                            {
                                                string fileName = building.Code + "-" + batch.BatchNumber.ToString().PadLeft(6, '0') + ".pdf";
                                                string folder = "Invoices"+@"\"+ DateTime.Today.ToString("MMM yyyy");
                                                string outputPath = building.DataFolder + folder + @"\";
                                                if (!Directory.Exists(outputPath))
                                                    Directory.CreateDirectory(outputPath);
                                                string outputFilename = outputPath + fileName;
                                                if (File.Exists(outputFilename))
                                                    File.Delete(outputFilename);

                                                File.WriteAllBytes(outputFilename, combinedReport);
                                                emailCount++;
                                                AddPdfDocument(copy, reportData);
                                            }
                                        }

                                    }
                                    catch (Exception er)
                                    {
                                        Controller.HandleError(er);
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }

                                    Application.DoEvents();
                                }

                            }
                        }
                        if (emailCount > 0)
                        {
                            var combinedEmailPDF = ms.ToArray();
                            var attachments = new Dictionary<string, byte[]>();
                            attachments.Add("Requisitions.pdf", combinedEmailPDF);
                            SendEmail(context, Controller.user.email, attachments);
                        }
                    }
                }
                if (emailCount > 0)
                    Controller.ShowMessage("Process completed - " + emailCount.ToString() + " buildings processed");
                else
                    Controller.ShowMessage("There were no outstanding requisitions to process");
                lbProcessing.Text = "";
                //Find all buildings linked to this user
            }
            finally
            {
                button1.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            LoadPendingRequisions();
        }

        private void SendEmail(DataContext context, string emailAddress,  Dictionary<string, byte[]> attachments)
        {
            string status;
            if(!Mailer.SendMailWithAttachments("noreply@astrodon.co.za",new string[] { "payments@astrodon.co.za", emailAddress },  
                "Payment Requisitions" , 
                "Please find attached requisitions", false, false, false, out status, attachments))
            {
                Controller.HandleError("Error seding email " + status, "Email error");
            }
        }

        private void btnDownload_Click_1(object sender, EventArgs e)
        {

        }
    }

    class BatchItem
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public DateTime Created { get; set; }
        public int BatchNumber { get; set; }
        public int Entries { get; set; }
        public string CreatedBy { get;  set; }
    }

    class RequisitionItem
    {
        public string AccountNumber { get;  set; }
        public decimal Amount { get;  set; }
        public string Bank { get;  set; }
        public string BranchCode { get;  set; }
        public string Building { get;  set; }
        public string BuildingCode { get;  set; }
        public string InvoiceNumber { get;  set; }
        public string LedgerAccount { get;  set; }
        public string SupplierName { get;  set; }
        public string SupplierReference { get;  set; }
    }
}

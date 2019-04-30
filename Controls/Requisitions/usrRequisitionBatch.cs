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
using System.Globalization;
using Astrodon.Email;

namespace Astrodon.Controls.Requisitions
{
    public partial class usrRequisitionBatch : UserControl
    {
        private List<Building> _Buildings;
        private List<BatchItem> _Data;
        private List<RequisitionItem> _PendingRequisitions;
        private bool _allBuildings = false;

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
                Buildings bManager = new Buildings(false);

                if (Controller.UserIsSheldon())
                {
                    _Buildings = bManager.buildings; //all buildings
                    _allBuildings = true;
                }
                else
                {
                    _Buildings = bManager.buildings.Where(a => a.PM == Controller.user.email).ToList(); //only pm buildings
                }
                _Buildings.Insert(0, new Building() { Name = "", ID = 0 });
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



        private RequisitionBatch CreateRequisitionBatch(int buildingId,bool buildingHasCSVExport, bool warnIfNoRequisitions = true)
        {
            ShowDebug("Start batch for building");
            using (var context = SqlDataHandler.GetDataContext())
            {
                ShowDebug("Context created");
                int batchNumber = 0;
                var requisitions = context.tblRequisitions
                    .Where(a => a.building == buildingId 
                             && a.processed == false 
                             && a.RequisitionBatchId == null).ToList();
                ShowDebug("Query executed");

                if (requisitions.Count <= 0)
                {
                    context.ClearStuckRequisitons(buildingId);
                    requisitions = context.tblRequisitions.Where(a => a.building == buildingId
                                            && a.processed == false
                                            && a.RequisitionBatchId == null).ToList();
                }

                if (requisitions.Count <= 0)
                {
                    if (warnIfNoRequisitions)
                        Controller.ShowMessage("There are no outstanding requisitions to process.");
                    ShowDebug("Return NULL in batch");

                    return null;
                }
                ShowDebug("Found Requisitions");

                var previousBatch = context.RequisitionBatchSet.Where(a => a.BuildingId == buildingId).OrderByDescending(a => a.BatchNumber).FirstOrDefault();
                if (previousBatch == null)
                    batchNumber = 1;
                else
                    batchNumber = previousBatch.BatchNumber + 1;
                ShowDebug("Batch Number: " + batchNumber.ToString());

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
                    if (buildingHasCSVExport && requisition.SupplierId != null)
                    {
                        var supplierBank = context.SupplierBuildingSet.Where(a => a.SupplierId == requisition.SupplierId && a.BuildingId == buildingId).FirstOrDefault();
                        if (supplierBank != null && !String.IsNullOrWhiteSpace(supplierBank.BeneficiaryReferenceNumber))
                        {
                            requisition.NedbankCSVBenificiaryReferenceNumber = supplierBank.BeneficiaryReferenceNumber;
                            requisition.UseNedbankCSV = true;
                        }
                    }
                    requisition.RequisitionBatch = batch;
                }
                ShowDebug("Save batch for building");
                context.RequisitionBatchSet.Add(batch);

                context.SaveChanges();

                Application.DoEvents();
                ShowDebug("Return batch for building");
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

        private byte[] CreateReport(int requisitionBatchId, string combinedFilePath)
        {
            bool processedOk = true;

            byte[] reportData = null;
            try
            {
                using (var reportService = ReportServiceClient.CreateInstance())
                {
                    reportData = reportService.RequisitionBatchReport(SqlDataHandler.GetConnectionString(), requisitionBatchId);
                    if (reportData != null)
                    {
                        using (FileStream ms = new FileStream(combinedFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            using (Document doc = new Document())
                            {
                                using (PdfCopy copy = new PdfCopy(doc, ms))
                                {
                                    doc.Open();

                                    AddPdfDocument(copy, reportData,"Generated Report");

                                    using (var context = SqlDataHandler.GetDataContext())
                                    {
                                        foreach (var requisitionId in context.tblRequisitions.Where(a => a.RequisitionBatchId == requisitionBatchId).OrderBy(a => a.trnDate).Select(a => a.id).ToList())
                                        {
                                            foreach (var invoice in context.RequisitionDocumentSet.Where(a => a.RequisitionId == requisitionId && a.IsInvoice == true).Select(a => a).ToList())
                                            {
                                                if (IsValidPdf(invoice.FileData))
                                                {
                                                    try
                                                    {
                                                        AddPdfDocument(copy, invoice.FileData, invoice.FileName);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        processedOk = false;
                                                        Controller.HandleError("Unable to attach invoice " + invoice.FileData);
                                                    }
                                                }
                                                else
                                                {
                                                    processedOk = false;
                                                    Controller.HandleError("Unable to attach invoice " + invoice.FileData + " pdf is invalid");
                                                }
                                                Application.DoEvents();
                                            }
                                        }
                                    }
                                    //write output file
                                    ms.Flush();
                                }
                               
                            }
                          
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                processedOk = false;
                Controller.HandleError(exp);
            }

            if (processedOk == false)
                return null;

            return reportData;

        }

        private string GetATempFile(string ext)
        {
            var workFile = System.IO.Path.GetTempPath();
            if (string.IsNullOrWhiteSpace(workFile))
            {
                workFile = @"C:\Temp\";
                if (!Directory.Exists(workFile))
                    Directory.CreateDirectory(workFile);
            }
            if (!workFile.EndsWith("\\"))
                workFile = workFile + "\\";

            if (string.IsNullOrWhiteSpace(ext))
                ext = ".temp";

            if (!ext.StartsWith("."))
                ext = "." + ext;
            workFile = workFile + System.Guid.NewGuid().ToString("N") + ext;
            return workFile;
        }

        private bool IsValidPdf(byte[] filepath)
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

        private void DownloadReport(int requisitionBatchId)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                var reportData = CreateReport(requisitionBatchId, dlgSave.FileName);
                Process.Start(dlgSave.FileName);
            }
        }

        private void AddPdfDocument(PdfCopy copy, byte[] document, string documentName)
        {
            try
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
            catch (Exception e)
            {
                Controller.HandleError("Unable to add PDF document " + Path.GetFileName( documentName));
                throw e;
            }
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedItem != null)
            {
                try
                {
                    Building selectedBuilding = cmbBuilding.SelectedItem as Building;

                    this.Cursor = Cursors.WaitCursor;

                    if (!Controller.VerifyBuildingDetailsEntered(selectedBuilding.ID))
                    {
                        cmbBuilding.SelectedIndex = -1;
                        dgItems.DataSource = null;
                        return;
                    }
                    else
                    {
                        LoadGrid();
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void LoadPendingRequisions()
        {
            this.axAcroPDF1.Visible = false;
            using (var context = SqlDataHandler.GetDataContext())
            {
                var buildingIds = _Buildings.Select(a => a.ID).ToArray();

                var qry = from b in context.tblBuildings
                          join r in context.tblRequisitions on b.id equals r.building
                          join pmUser in context.tblUsers on b.pm equals pmUser.email
                          where r.processed == false
                          && b.BuildingDisabled == false
                          && pmUser.Active
                          select new RequisitionItem()
                          {
                              RequisitionId = r.id,                              
                              Building = b.Building,
                              BuildingCode = b.Code,
                              Bank = r.BankName,
                              BranchCode = r.BranchCode,
                              AccountNumber = r.AccountNumber,
                              SupplierName = r.Supplier != null ? r.Supplier.CompanyName : r.contractor,
                              LedgerAccount = r.ledger,
                              Amount = r.amount,
                              SupplierReference = r.payreference,
                              InvoiceNumber = r.InvoiceNumber,
                              PortfolioManager = pmUser.name,
                              PortfolioUserId = pmUser.id,
                              InvoiceCount = r.Documents.Count(a => a.IsInvoice == true),
                          };

                if (_allBuildings)
                {
                    _PendingRequisitions = qry.OrderBy(a => a.Building).ThenBy(a => a.SupplierName).ToList();
                }
                else
                {
                    _PendingRequisitions = qry.Where(a => a.PortfolioUserId == Controller.user.id).OrderBy(a => a.Building).ThenBy(a => a.SupplierName).ToList();
                }

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

          
            dgPendingTransactions.Columns.Clear();


            //HasInvoice
            dgPendingTransactions.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                DataPropertyName = "HasInvoice",
                HeaderText = "Invoice Uploaded",
                ReadOnly = true,
            });
            dgPendingTransactions.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Invoice",
                Text = "View",
                UseColumnTextForButtonValue = true,
            });
            dgPendingTransactions.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Invoice",
                Text = "Upload",
                UseColumnTextForButtonValue = true,
            });


            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Building",
                HeaderText = "Building",
                ReadOnly = true,
                MinimumWidth = 30
            });
            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PortfolioManager",
                HeaderText = "PM",
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
                ReadOnly = true,
            });
            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SupplierName",
                HeaderText = "Supplier",
                ReadOnly = true,
            });

            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SupplierReference",
                HeaderText = "Reference",
                ReadOnly = true,
            });

            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Amount",
                HeaderText = "Amount",
                ReadOnly = true,
                DefaultCellStyle = currencyColumnStyle,
            });

            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Bank",
                HeaderText = "Bank",
                ReadOnly = true,
            });
            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountNumber",
                HeaderText = "Account",
                ReadOnly = true,
            });

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            BindingSource bs = new BindingSource();
            dgPendingTransactions.DataSource = bs;

            bs.DataSource = _PendingRequisitions;

            //dgPendingTransactions.AutoResizeColumns();

            foreach (DataGridViewRow row in dgPendingTransactions.Rows)
            {
                var itm = row.DataBoundItem as RequisitionItem;
                if (itm.HasInvoice == false)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime paymentDate = DateTime.Today;


            if (paymentDate.DayOfWeek == DayOfWeek.Friday)
            {
                while (paymentDate.DayOfWeek != DayOfWeek.Monday)
                    paymentDate = paymentDate.AddDays(1);
            }
            else if (paymentDate.AddDays(1).Month != paymentDate.Month) //I am on the last day of the month
                paymentDate = paymentDate.AddDays(1);


            if (_PendingRequisitions != null)
            {
                var cnt = _PendingRequisitions.Where(a => a.HasInvoice == false).Count();
                if (cnt > 0)
                {
                    Controller.HandleError("There are " + cnt.ToString() + " requisitions without an attached invoice.\n" +
                        "Please go to the requisition edit screen to upload the corresponding invoice documents.", "Validation Error");
                    return;
                }
            }

            if (paymentDate.Month != DateTime.Today.Month)
            {
                Controller.ShowMessage("Please note that this batch falls on the last day of the month.\n The batch will therefore be stored in " + paymentDate.ToString("MMM yyyy") + " folder.");
            }
            try
            {
                int emailCount = 0;

                using (var context = SqlDataHandler.GetDataContext())
                {
                   
                    var qry = from b in context.tblBuildings
                              join r in context.tblRequisitions on b.id equals r.building
                              where r.processed == false
                              && b.BuildingDisabled == false
                              && b.pm == Controller.user.email
                              select b;

                    var buildings = qry.Distinct().ToList();
                    if (buildings.Count <= 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("There are no requisitions to process for " + Controller.user.name, "Information", MessageBoxButtons.OK);
                        return;
                    }
                    if (buildings.Count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to process these requisitions?", "Question", MessageBoxButtons.YesNo);
                        if (dialogResult != DialogResult.Yes)
                            return;

                        this.Cursor = Cursors.WaitCursor;
                        button1.Enabled = false;

                        foreach (var b in buildings)
                        {
                            if (b.pm != Controller.user.email)
                            {
                                Controller.HandleError("Building not linked to this PM " + b.Building);
                                button1.Enabled = true;
                                this.Cursor = Cursors.Default;
                                return;
                            }
                            if (!b.CheckIfFolderExists())
                            {
                                Controller.HandleError("Unable to access " + b.DataFolder);
                                button1.Enabled = true;
                                this.Cursor = Cursors.Default;
                                return;
                            }
                        }

                        Document doc;
                        PdfCopy copy;

                        var tempCombinedReport = GetATempFile(".pdf");
                        var attachments = new Dictionary<string, byte[]>();
                        ShowDebug("Process start "+ tempCombinedReport);

                        try
                        {
                            using (var ms = new FileStream(tempCombinedReport, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {

                                using (doc = new Document())
                                {
                                    using (copy = new PdfCopy(doc, ms))
                                    {
                                        ShowDebug("doc.Open() " + tempCombinedReport);

                                        doc.Open();

                                        foreach (var building in buildings)
                                        {
                                            #region Requisition Pdf
                                            try
                                            {
                                                lbProcessing.Text = "Processing " + building.Building;
                                                ShowDebug("Processing " + building.Building);

                                                Application.DoEvents();
                                                var batch = CreateRequisitionBatch(building.id, building.IsUsingNedbank, false);
                                                if (batch != null)
                                                {
                                                    ShowDebug("Created Batch " + building.Building);

                                                    #region Create PDF For Batch
                                                    var tempFile = GetATempFile(".pdf");
                                                    try
                                                    {
                                                        ShowDebug("Generate Report " + building.Building);

                                                        var reportData = CreateReport(batch.id, tempFile);
                                                        if (reportData != null)
                                                        {

                                                            try
                                                            {
                                                                string fileName = building.Code + "-" + batch.BatchNumber.ToString().PadLeft(6, '0') + ".pdf";
                                                                string folder = "Invoices" + @"\" + paymentDate.ToString("yyyy") + @"\" + paymentDate.ToString("MMM yyyy");
                                                                string outputPath = building.DataFolder + folder + @"\";
                                                                if (!Directory.Exists(outputPath))
                                                                    Directory.CreateDirectory(outputPath);
                                                                string outputFilename = outputPath + fileName;
                                                                if (File.Exists(outputFilename))
                                                                    File.Delete(outputFilename);
                                                                try
                                                                {
                                                                    ShowDebug("Write output file " + outputFilename + " " + building.Building);

                                                                    File.Copy(tempFile, outputFilename);
                                                                }
                                                                catch (Exception fEx)
                                                                {
                                                                    Controller.HandleError(fEx);
                                                                }

                                                                emailCount++;
                                                                ShowDebug("AddPdfDocument " + "Batch Report" + " " + building.Building);

                                                                AddPdfDocument(copy, reportData,"Batch Report");

                                                                CommitRequisitionBatch(batch);

                                                                //string csvFileName = "";
                                                                //var csvFile = CreateCSVForBuildingBatch(building, batch, out csvFileName);
                                                                //if (!String.IsNullOrWhiteSpace(csvFileName))
                                                                //{
                                                                //    attachments.Add(csvFileName, csvFile);

                                                                //    string csvOutputPath = outputPath + csvFile;
                                                                //    try
                                                                //    {
                                                                //        File.WriteAllBytes(outputFilename, csvFile);
                                                                //    }
                                                                //    catch (Exception fEx)
                                                                //    {
                                                                //        Controller.HandleError(fEx);
                                                                //    }

                                                                //}

                                                            }
                                                            catch (Exception exr)
                                                            {
                                                                Controller.HandleError(exr);
                                                                RollbackRequsitionBatch(batch);// an error occured in this batch rollback it

                                                            }
                                                        }
                                                        else
                                                        {
                                                            ShowDebug("Report empty " + building.Building);

                                                            RollbackRequsitionBatch(batch);// an error occured in this batch rollback it
                                                        }
                                                    }
                                                    finally
                                                    {
                                                        if (File.Exists(tempFile))
                                                            File.Delete(tempFile);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    Controller.HandleError("An empty batch was returned");
                                                }

                                            }
                                            catch (Exception er)
                                            {
                                                Controller.HandleError(er);
                                                this.Cursor = Cursors.Default;
                                            }
                                            #endregion

                                            ms.Flush();
                                            Application.DoEvents();
                                        }

                                    }
                                }

                            }

                            if (emailCount > 0)
                            {

                                attachments.Add("Requisitions.pdf", File.ReadAllBytes(tempCombinedReport));
                                SendEmail(context, Controller.user.email, attachments);
                            }
                        }
                        finally
                        {
                            if (File.Exists(tempCombinedReport))
                                File.Delete(tempCombinedReport);
                        }
                    }

                    if (emailCount > 0)
                        Controller.ShowMessage("Process completed - " + emailCount.ToString() + " buildings processed");
                    else
                        Controller.ShowMessage("There were no outstanding requisitions to process");
                    lbProcessing.Text = "";
                    //Find all buildings linked to this user
                }
            }
            finally
            {
                button1.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            LoadPendingRequisions();
        }

        private void ShowDebug(string v)
        {
            //Controller.ShowMessage(v, "Debug");
        }

        private byte[] CreateCSVForBuildingBatch(tblBuilding building, RequisitionBatch batch,out string fileName)
        {
            fileName = string.Empty;
            if (!building.IsUsingNedbank)
                return null;

            return CreateNedbankCSV(building, batch, out fileName);
        }

        private static byte[] CreateNedbankCSV(tblBuilding building, RequisitionBatch batch, out string fileName)
        {
            fileName = null;


            string fromAccountNumber = building.bankAccNumber;
            string fromAccountDescription = building.accName;
            string fromAccountSubAccountNumber = string.Empty;

            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from r in context.tblRequisitions
                        where r.RequisitionBatchId == batch.id
                        && r.UseNedbankCSV == true
                        select new NedbankCSVRecord
                        {
                            FromAccountNumber = fromAccountNumber,
                            FromAccountDescription = fromAccountDescription,
                            MyStatementDescription = r.payreference,
                            BeneficiaryReferenceNumber = r.NedbankCSVBenificiaryReferenceNumber,
                            BeneficiaryStatementDescription = r.payreference,
                            Amount = r.amount
                        };
                var transactions = q.ToList();

                //var csvFile = new NedbankCSVFile(transactions);

                using (MemoryStream fs = new MemoryStream())
                {
                    using (StreamWriter tw = new StreamWriter(fs))
                    {
                        foreach (var t in transactions)
                            tw.WriteLine(t.ToString());
                    }

                    byte[] result = new byte[fs.Length];
                    fs.Flush();
                    fs.Read(result, 0, result.Length);
                    fileName = "Nedbank - " + building.Code + "-" + batch.BatchNumber.ToString().PadLeft( 6,'0') + ".csv";
                    return result;
                }
            }
        }

        private void RollbackRequsitionBatch(RequisitionBatch batch)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                context.RequisitionBatchRollback(batch.id);
            }
        }

        private void CommitRequisitionBatch(RequisitionBatch batch)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                context.CommitRequisitionBatch(batch.id);
                SendPaymentNotifications(context,batch.id,Controller.user.email);
            }
        }

        private void SendPaymentNotifications(DataContext context, int batchId, string fromEmail)
        {
            var q = (from req in context.tblRequisitions.Include(a => a.Supplier)
                     where req.RequisitionBatchId == batchId
                     && req.NotifySupplierByEmail == true
                     select new
                     {
                         NotifyEmailAddress = req.NotifyEmailAddress,
                         Payreference =  req.payreference,
                         ContactPerson = req.SupplierId == null ? "" : req.Supplier.ContactPerson,
                         Amount = req.amount
                     });

            foreach(var itm in q.Distinct().ToList())
            {
                if (!String.IsNullOrWhiteSpace(itm.NotifyEmailAddress))
                {
                    EmailProvider.RequisitionBatchSendPaymentNotifications(fromEmail, itm.NotifyEmailAddress, itm.ContactPerson, itm.Amount, itm.Payreference);
                }

            }

        }

        private void SendEmail(DataContext context, string emailAddress,  Dictionary<string, byte[]> attachments)
        {
            if(!EmailProvider.SendRequisitionNotification(emailAddress, attachments))
            {
                Controller.HandleError("Error seding email\n Please save your requisition report and email it manually", "Email error");

                foreach(var key in attachments.Keys)
                {
                    dlgSave.FileName = key;
                    if (dlgSave.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(dlgSave.FileName, attachments[key]);
                    }
                    else
                    {
                        Controller.HandleError("You have to save the file or it will be lost!");

                        if (dlgSave.ShowDialog() == DialogResult.OK) //try twice
                        {
                            File.WriteAllBytes(dlgSave.FileName, attachments[key]);
                        }
                        else
                        {
                            Controller.HandleError("You did not save the file, it is now lost. Please manually go through all created batches to process it.");
                        }
                    }
                }
            }
        }

        private void dgPendingTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            this.Cursor = Cursors.WaitCursor;
            try
            {

                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    var item = senderGrid.Rows[e.RowIndex].DataBoundItem as RequisitionItem;
                    if (item == null)
                        return;
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var requisitionDoc = context.RequisitionDocumentSet.FirstOrDefault(a => a.RequisitionId == item.RequisitionId);

                        if (e.ColumnIndex == 1) //view doc
                        {
                            this.axAcroPDF1.Visible = false;
                            if (requisitionDoc != null)
                            {
                                DisplayPDF(requisitionDoc.FileData);
                            }
                        }
                        else
                        {
                            ofdAttachment.Multiselect = false;
                            if (ofdAttachment.ShowDialog() == DialogResult.OK)
                            {
                                for (int i = 0; i < ofdAttachment.FileNames.Count(); i++)
                                {
                                    if (IsValidPdf(ofdAttachment.FileNames[i]))
                                    {
                                        byte[] pdfData = File.ReadAllBytes(ofdAttachment.FileNames[i]);
                                        if (requisitionDoc == null)
                                        {
                                            requisitionDoc = new RequisitionDocument()
                                            {
                                                RequisitionId = item.RequisitionId,
                                                IsInvoice = true
                                            };
                                            context.RequisitionDocumentSet.Add(requisitionDoc);
                                        }
                                        requisitionDoc.FileData = pdfData;
                                        requisitionDoc.FileName = ofdAttachment.SafeFileNames[i];
                                        context.SaveChanges();
                                        DisplayPDF(pdfData);
                                        item.InvoiceCount = 1;
                                        RefreshGrid();

                                    }
                                    else
                                        Controller.HandleError("Invalid PDF\n" + ofdAttachment.FileNames[i] + "\n Please load a different pdf");

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
            }
        }

    

        private string _TempPDFFile = string.Empty;
        private void DisplayPDF(byte[] pdfData)
        {
            if (pdfData == null)
            {
                this.axAcroPDF1.Visible = false;
                return;
            }
            if (!String.IsNullOrWhiteSpace(_TempPDFFile))
                File.Delete(_TempPDFFile);
            _TempPDFFile = Path.GetTempPath();
            if (!_TempPDFFile.EndsWith(@"\"))
                _TempPDFFile = _TempPDFFile + @"\";

            _TempPDFFile = _TempPDFFile + System.Guid.NewGuid().ToString("N") + ".pdf";
            File.WriteAllBytes(_TempPDFFile, pdfData);


            try
            {
                this.axAcroPDF1.Visible = true;
                this.axAcroPDF1.LoadFile(_TempPDFFile);
                this.axAcroPDF1.src = _TempPDFFile;
                this.axAcroPDF1.setShowToolbar(false);
                this.axAcroPDF1.setView("FitH");
                this.axAcroPDF1.setLayoutMode("SinglePage");
                this.axAcroPDF1.setShowToolbar(false);

                this.axAcroPDF1.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            File.Delete(_TempPDFFile);
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

    class NedbankCSVFile
    {
        public NedbankCSVFile(string batchDescription, 
                              DateTime batchDate, 
                              string hashSeed,
                              List<NedbankCSVRecord> transactions)
        {
            this.Records = transactions;
            this.BatchDescription = batchDescription;
            this.HashSeed = hashSeed;

        }
        public string HashSeed { get; set; }
        public string BatchDescription { get; set; }
        public DateTime BatchDate { get; set; }
        public string BatchTotal
        {
            get
            {
                var tot = Records.Sum(a => a.Amount);
                return tot.ToString("0.00", CultureInfo.InvariantCulture);
            }
        }
        public List<NedbankCSVRecord> Records { get; set; }
    }

    class NedbankCSVRecord
    {
        public string FromAccountNumber { get;  set; }
        public string FromAccountDescription { get;  set; }
        public string MyStatementDescription { get;  set; }

        public string BeneficiaryReferenceNumber { get;  set; }
        public string BeneficiaryStatementDescription { get;  set; }

        public decimal Amount { get;  set; }
        public string ProofOfPaymentFlag { get { return "false"; } }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Clean(FromAccountNumber));
            sb.Append(",");
            sb.Append("'"+ Clean(FromAccountDescription) +"'");
            sb.Append(",");
            sb.Append("'" + Clean(MyStatementDescription) + "'");
            sb.Append(",");
            sb.Append(Clean(BeneficiaryReferenceNumber));
            sb.Append(",");
            sb.Append("'" + Clean(BeneficiaryStatementDescription) + "'");
            sb.Append(",");
            sb.Append(Amount.ToString("0.00",CultureInfo.InvariantCulture));
            sb.Append(",");
            sb.Append(ProofOfPaymentFlag);
            return sb.ToString();

        }

        private string Clean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return value.Replace(",", "").Trim();
        }
    }

    class RequisitionItem
    {
        public int RequisitionId { get; set; }
        public int InvoiceCount { get; set; }
        public bool HasInvoice { get { return InvoiceCount > 0; } }

        public string AccountNumber { get;  set; }
        public decimal Amount { get;  set; }
        public string Bank { get;  set; }
        public string BranchCode { get;  set; }
        public string Building { get;  set; }
        public string BuildingCode { get;  set; }
        public string InvoiceNumber { get;  set; }
        public string LedgerAccount { get;  set; }
        public string PortfolioManager { get;  set; }
        public int PortfolioUserId { get;  set; }
        public string SupplierName { get;  set; }
        public string SupplierReference { get;  set; }
    }
}

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



        private RequisitionBatch CreateRequisitionBatch(int buildingId, bool warnIfNoRequisitions = true)
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                int batchNumber = 0;
                var requisitions = context.tblRequisitions.Where(a => a.building == buildingId 
                && a.processed == false 
                && a.RequisitionBatchId == null).ToList();
                if (requisitions.Count <= 0)
                {
                    if (warnIfNoRequisitions)
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
                }

                context.RequisitionBatchSet.Add(batch);

                context.SaveChanges();

                Application.DoEvents();

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

                                    AddPdfDocument(copy, reportData);

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
                                                        AddPdfDocument(copy, invoice.FileData);
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
            if (cmbBuilding.SelectedItem != null)
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
                          join pmUser in context.tblUsers on b.pm equals pmUser.email
                          where r.processed == false
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
                              InvoiceNumber = r.InvoiceNumber,
                              PortfolioManager = pmUser.name,
                              PortfolioUserId = pmUser.id,
                              InvoiceCount = r.Documents.Count(a => a.IsInvoice == true)
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

            BindingSource bs = new BindingSource();

            dgPendingTransactions.Columns.Clear();

            dgPendingTransactions.DataSource = bs;

            //HasInvoice
            dgPendingTransactions.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                DataPropertyName = "HasInvoice",
                HeaderText = "Invoice Uploaded",
                ReadOnly = true,
            });

            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Building",
                HeaderText = "Building",
                ReadOnly = true,
            });
            dgPendingTransactions.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PortfolioManager",
                HeaderText = "PM",
                ReadOnly = true
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



            bs.DataSource = _PendingRequisitions;

            dgPendingTransactions.AutoResizeColumns();

            foreach(DataGridViewRow row in dgPendingTransactions.Rows)
            {
                var itm = row.DataBoundItem as RequisitionItem;
                if(itm.HasInvoice == false)
                {
                    foreach(DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

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


            try
            {
                int emailCount = 0;

                using (var context = SqlDataHandler.GetDataContext())
                {

                    var qry = from b in context.tblBuildings
                              join r in context.tblRequisitions on b.id equals r.building
                              where r.processed == false
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

                        try
                        {
                            using (var ms = new FileStream(tempCombinedReport, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                             
                                using (doc = new Document())
                                {
                                    using (copy = new PdfCopy(doc, ms))
                                    {
                                        doc.Open();

                                        foreach (var building in buildings)
                                        {
                                            #region Requisition Pdf
                                            try
                                            {
                                                lbProcessing.Text = "Processing " + building.Building;
                                                Application.DoEvents();
                                                var batch = CreateRequisitionBatch(building.id, false);
                                                if (batch != null)
                                                {
                                                    #region Create PDF For Batch
                                                    var tempFile = GetATempFile(".pdf");
                                                    try
                                                    {
                                                        var reportData = CreateReport(batch.id, tempFile);
                                                        if (reportData != null)
                                                        {

                                                            try
                                                            {
                                                                string fileName = building.Code + "-" + batch.BatchNumber.ToString().PadLeft(6, '0') + ".pdf";
                                                                string folder = "Invoices" + @"\" + DateTime.Today.ToString("MMM yyyy");
                                                                string outputPath = building.DataFolder + folder + @"\";
                                                                if (!Directory.Exists(outputPath))
                                                                    Directory.CreateDirectory(outputPath);
                                                                string outputFilename = outputPath + fileName;
                                                                if (File.Exists(outputFilename))
                                                                    File.Delete(outputFilename);
                                                                try
                                                                {
                                                                    File.Copy(tempFile, outputFilename);
                                                                }
                                                                catch (Exception fEx)
                                                                {
                                                                    Controller.HandleError(fEx);
                                                                }

                                                                emailCount++;
                                                                AddPdfDocument(copy, reportData);

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

                                attachments.Add("Requisitions.pdf",File.ReadAllBytes(tempCombinedReport));
                                SendEmail(context, Controller.user.email, attachments);
                            }
                        }
                        finally
                        {
                            if(File.Exists(tempCombinedReport))
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

        private byte[] CreateCSVForBuildingBatch(tblBuilding building, RequisitionBatch batch,out string fileName)
        {
            fileName = string.Empty;
            if(!String.IsNullOrWhiteSpace(building.bank) && building.bank.Trim().ToLower() == "nedbank")
            {
                return CreateNedbankCSV(building, batch, out fileName);
            }
            else
              return null;
        }

        private static byte[] CreateNedbankCSV(tblBuilding building, RequisitionBatch batch, out string fileName)
        {
            fileName = null;
            return null;
            
            string fromAccountNumber = building.bankAccNumber;
            string fromAccountDescription = building.accName;
            string fromAccountSubAccountNumber = string.Empty;

            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from r in context.tblRequisitions
                        where r.RequisitionBatchId == batch.id
                        select new NedbankCSVRecord
                        {
                            FromAccountNumber = fromAccountNumber,
                            FromAccountDescription = r.reference,
                            //FromAccountSubAccountNumber = fromAccountSubAccountNumber,
                            //MyStatementDescription = r.reference,
                            //BeneficiaryAccountNumber = r.AccountNumber,
                            //ToAccountSubAccountNumber = "",
                            //ToAccountDescription = r.AccountNumber,
                            //BeneficiaryStatementDescription = r.payreference,
                            Amount = r.amount
                        };
                var transactions = q.ToList();

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
                SendPaymentNotifications(context,batch.id);
            }
        }

        private void SendPaymentNotifications(DataContext context, int batchId)
        {
            var q = (from req in context.tblRequisitions
                     where req.RequisitionBatchId == batchId
                     && req.NotifySupplierByEmail == true
                     select new
                     {
                         req.NotifyEmailAddress,
                         req.payreference
                     });

            foreach(var itm in q.Distinct().ToList())
            {
                if(!String.IsNullOrWhiteSpace(itm.NotifyEmailAddress))
                {
                    string status;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Good Day");
                    sb.AppendLine("");
                    sb.AppendLine("Your payment with reference " + itm.payreference + " has been instruced for processing");
                    sb.AppendLine("");
                    sb.AppendLine("Kind Regards");
                    sb.AppendLine("Astrodon PTY LTD");
                    try
                    {
                        Mailer.SendMail("noreply@astrodon.co.za", new string[] { itm.NotifyEmailAddress }, "Payment Scheduled",
                            sb.ToString(),false,false,false,out status,new string[] { });
                    }
                    catch(Exception e)
                    {
                        Controller.HandleError(e);
                    }

                }

            }

        }

        private void SendEmail(DataContext context, string emailAddress,  Dictionary<string, byte[]> attachments)
        {
            string status;
            if(!Mailer.SendMailWithAttachments("noreply@astrodon.co.za",new string[] { "payments@astrodon.co.za", emailAddress },  
                "Payment Requisitions" , 
                "Please find attached requisitions", false, false, false, out status, attachments))
            {
                Controller.HandleError("Error seding email " + status +"\n Please save your requisition report and email it manually", "Email error");

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

    class NedbankCSVRecord
    {
        public string FromAccountNumber { get; internal set; }
        public string FromAccountDescription { get; internal set; }
        public string MyStatementDescription { get; internal set; }


        public string BeneficiaryAccountNumber { get; internal set; }
        public string BeneficiaryStatementDescription { get; internal set; }

        public decimal Amount { get; internal set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Clean(FromAccountNumber));
            sb.Append(",");
            sb.Append("'"+ Clean(FromAccountDescription) +"'");
            sb.Append(",");
            sb.Append("'" + Clean(MyStatementDescription) + "'");
            sb.Append(",");
            sb.Append(Clean(BeneficiaryAccountNumber));
            sb.Append(",");
            sb.Append("'" + Clean(BeneficiaryStatementDescription) + "'");
            sb.Append(",");
            sb.Append(Amount.ToString("0.00",CultureInfo.InvariantCulture));
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

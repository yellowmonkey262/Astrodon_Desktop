using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Desktop.Lib.Pervasive;
using System.Data.Odbc;
using System.Diagnostics;
using System.Collections;
using System.IO;
using Astrodon.ReportService;
using Astro.Library.Entities;
using Astrodon.Data.Base;
using Astrodon.Classes;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Astrodon.Reports
{
    public partial class ManangementPackUserControl : UserControl
    {
        private List<Building> _Buildings;
        private List<IdValue> _Years;
        private List<IdValue> _Months;
        private List<TableOfContentForPdfRecord> _TableOfContents;
        private List<string> _DescriptionList = new List<string>();

        private SqlDataHandler dh = new SqlDataHandler();

        public ManangementPackUserControl()
        {
            InitializeComponent();
            LoadCheckLists();
            LoadBuildings();
            LoadYears();
            button2.Enabled = false;
            button3.Enabled = false;
            btnAddLevyRoll.Enabled = false;
            btnCheckList.Enabled = false;
            btnMaintenance.Enabled = false;
        }

        private void LoadCheckLists()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var itms = context.ManagementPackTOCItemSet.ToList();
                var tmpItems = new List<string>() {
                        "Detail income statement",
                        "Balance sheet",
                        "Bank statement",
                        "Sundry customers",
                        "Sundry suppliers",
                        "Council reconciliations",
                        "Cash movement statement",
                        "Levy Roll",
                        "Financial checklist",
                        "Invoicing",
                        "POP",
                        "Maintenance"
                    };
                if (itms == null || itms.Count < tmpItems.Count())
                {
                  

                    foreach(var itm in tmpItems)
                    {
                        context.ManagementPackTOCItemSet.Add(new Data.ManagementPackData.ManagementPackTOCItem(){Description = itm });
                    }
                    context.SaveChanges();
                    _DescriptionList = tmpItems.OrderBy(a => a).ToList();
                }
                else
                {
                    _DescriptionList = itms.OrderBy(a => a.Description).Select(a => a.Description).ToList();
                }
            }
            _DescriptionList.Insert(0, "");
        }

        private List<string> GetDescriptionList()
        {
            return _DescriptionList;
        }

        private void LoadYears()
        {
            _Years = new List<IdValue>();
            _Years.Add(new IdValue() { Id = DateTime.Now.Year - 1, Value = (DateTime.Now.Year - 1).ToString() });
            _Years.Add(new IdValue() { Id = DateTime.Now.Year, Value = (DateTime.Now.Year).ToString() });
            _Years.Add(new IdValue() { Id = DateTime.Now.Year + 1, Value = (DateTime.Now.Year + 1).ToString() });

            _Months = new List<IdValue>();
            for (int x = 1; x <= 12; x++)
            {
                _Months.Add(new IdValue()
                {
                    Id = x,
                    Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x)
                });
            }

            cmbYear.DataSource = _Years;
            cmbYear.ValueMember = "Id";
            cmbYear.DisplayMember = "Value";
            cmbYear.SelectedValue = DateTime.Now.AddMonths(-1).Year;

            cmbMonth.DataSource = _Months;
            cmbMonth.ValueMember = "Id";
            cmbMonth.DisplayMember = "Value";
            cmbMonth.SelectedValue = DateTime.Now.AddMonths(-1).Month;
        }

        private void LoadBuildings()
        {
            var userid = Controller.user.id;
            Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));

            _Buildings = bManager.buildings;
            cmbBuilding.DataSource = _Buildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_TableOfContents != null && _TableOfContents.Count > 0)
            {
                if (!Controller.AskQuestion("You have already started with a report, are you sure you want to start again? Please note that your current TOC will be cleared."))
                    return;
            }
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            btnAddLevyRoll.Enabled = false;
            btnCheckList.Enabled = false;
            try
            {
                LoadFiles();
                PopulateTableOfContents();
                button2.Enabled = true;
                button3.Enabled = true;
                btnAddLevyRoll.Enabled = true;
                btnCheckList.Enabled = true;
                btnMaintenance.Enabled = true;
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private List<string> GetFilesInDirectory(int buildingId, int year, int month)
        {
            var dataFolder = "";
            using (var context = SqlDataHandler.GetDataContext())
            {
                dataFolder = context.tblBuildings
                        .FirstOrDefault(a => a.id == buildingId)?.DataFolder;
            }

            var dt = new DateTime(year, month, 1);

            if (string.IsNullOrWhiteSpace(dataFolder))
                return null;

            string folder = "Invoices" + @"\" + dt.ToString("yyyy") + @"\" + dt.ToString("MMM yyyy"); 
            string outputPath = (dataFolder + folder).Trim();

            if(!Directory.Exists(outputPath))
            {
                folder = "Invoices" + @"\" + dt.ToString("MMM yyyy");
                outputPath = (dataFolder + folder).Trim();
                if (!Directory.Exists(outputPath))
                {
                    if (!Directory.Exists(outputPath))
                    {
                        folder = "Invoices" + @"\" + dt.ToString("yyyy") + @"\" + dt.ToString("MMM");
                        outputPath = (dataFolder + folder).Trim();

                    }


                }
            }

            if (Directory.Exists(outputPath))
            {
                var items = Directory.GetFiles(outputPath).ToList();
                if (items.Count == 0)
                    Controller.ShowMessage("No files found in [" + outputPath + "]");
                return items;
            }
            else
            {
                Controller.ShowMessage("Folder does not exist [" + outputPath + "]");
                return new List<string>();
            }
        }

        private int GetTotalPages(string path)
        {
            PdfReader reader = null;
            try
            {
                var returnVal = 0;
                using (reader = new PdfReader(path))
                {
                    returnVal = reader.NumberOfPages;
                    reader.Close();
                }
                return returnVal;
            }
            catch
            {
                return 0;
            }
        }

        private int GetTotalPages(byte[] path)
        {
            PdfReader reader = null;
            try
            {
                var returnVal = 0;
                using (reader = new PdfReader(path))
                {
                    returnVal = reader.NumberOfPages;
                    reader.Close();
                }
                return returnVal;
            }
            catch
            {
                return 0;
            }
        }

        private void LoadFiles()
        {
            _TableOfContents = new List<TableOfContentForPdfRecord>();
            var building = cmbBuilding.SelectedItem as Building;
            var year = cmbYear.SelectedItem as IdValue;
            var month = cmbMonth.SelectedItem as IdValue;
            try
            {
                var files = GetFilesInDirectory(building.ID, year.Id, month.Id);
                for (int i = 0; i < files?.Count; i++)
                {
                    var totalPages = GetTotalPages(files[i]);
                    if (totalPages > 0)
                    {
                        var fileDate = File.GetCreationTime(files[i]);

                        _TableOfContents.Add(new TableOfContentForPdfRecord()
                        {
                            Path = files[i],
                            File = Path.GetFileName( files[i]),
                            Position = 0,
                            Pages = totalPages,
                            FileDate = fileDate,
                            IsTempFile = false,
                            IncludeInTOC = true
                        });
                    }
                }
                _TableOfContents = _TableOfContents.OrderBy(a => a.FileDate).ToList();
                for (int x = 1; x < _TableOfContents.Count; x++)
                    _TableOfContents[x].Position = x;
            }
            catch (Exception e)
            {
                Controller.HandleError(e);
                MessageBox.Show($"No files found for ({building.Name}) in ({month.Value} {year.Value})");
                return;
            }
        }

        private void PopulateTableOfContents()
        {
            dgTocGrid.ClearSelection();
            dgTocGrid.MultiSelect = false;
            dgTocGrid.AutoGenerateColumns = false;

            dgTocGrid.Columns.Clear();
            dgTocGrid.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "",
                Text = "Remove",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 20,
            });
            dgTocGrid.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "",
                Text = "Up",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 20,
            });
            dgTocGrid.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "",
                Text = "Down",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 20,
            });
            dgTocGrid.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "",
                Text = "Preview",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 20,
            });
            dgTocGrid.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                DataPropertyName = "IncludeInTOC",
                HeaderText = "TOC",
                ReadOnly = false,
            });
            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Position",
                HeaderText = "Position",
                ReadOnly = true,
            });
            //dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            //{
            //    DataPropertyName = "Path",
            //    HeaderText = "Path",
            //    ReadOnly = true,
            //    Width = 100
            //});
            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "File",
                HeaderText = "File",
                ReadOnly = true
            });

            dgTocGrid.Columns.Add(new DataGridViewComboBoxColumn()
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                Name = "Description",
                ReadOnly = false,
                MinimumWidth = 200
            });

            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Description2",
                HeaderText = "Description2",
                Name = "Description2",
                ReadOnly = false,
                MinimumWidth = 200
            });

            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Pages",
                HeaderText = "Pages",
                ReadOnly = true
            });

            RefreshGrid();
        }

        private void dgTocGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            foreach (DataGridViewRow row in dgTocGrid.Rows)
            {
                TableOfContentForPdfRecord itm = row.DataBoundItem as TableOfContentForPdfRecord;
                itm.DataRow = row;

                var comboBox = row.Cells["Description"] as DataGridViewComboBoxCell;
                comboBox.ReadOnly = false;
                comboBox.DataSource = GetDescriptionList();
            }
        }


        private void RefreshGrid()
        {
            BindingSource bs = new BindingSource();
            dgTocGrid.DataSource = bs;
            _TableOfContents = _TableOfContents.OrderBy(a => a.Position).ToList();
            var pos = 1;
            foreach (var item in _TableOfContents)
                item.Position = pos++;
            bs.DataSource = _TableOfContents;
            dgTocGrid.AutoResizeColumns();
        }

        private void dgTocGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    var item = senderGrid.Rows[e.RowIndex].DataBoundItem as TableOfContentForPdfRecord;
                    if (item == null)
                        return;
                    switch (e.ColumnIndex)
                    {
                        case 0:
                            _TableOfContents.RemoveAll(a => a.Position == item.Position);
                            break;
                        case 1:
                            if (item.Position == 1)
                                return;
                            _TableOfContents.First(a => a.Position == item.Position - 1).Position = item.Position;
                            item.Position = item.Position - 1;
                            break;
                        case 2:
                            if (item.Position == _TableOfContents.Max(a => a.Position))
                                return;
                            _TableOfContents.First(a => a.Position == item.Position + 1).Position = item.Position;
                            item.Position = item.Position + 1;
                            break;
                        case 3:
                            DisplayPDF(File.ReadAllBytes(item.Path));
                            break;
                        default:
                            break;
                    }
                    RefreshGrid();
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

        private void button2_Click(object sender, EventArgs e)
        {
            var building = cmbBuilding.SelectedItem as Building;
            var buildingName = "";
            using (var context = SqlDataHandler.GetDataContext())
            {
                buildingName = context.tblBuildings
                        .FirstOrDefault(a => a.id == building.ID)?.DataFolder;
            }
            if (Directory.Exists(buildingName))
                dlgOpen.InitialDirectory = buildingName;
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = false;
                try
                {
                    var totalPages = GetTotalPages(dlgOpen.FileName);
                    if (totalPages > 0)
                    {
                        var fileDate = File.GetCreationTime(dlgOpen.FileName);

                        _TableOfContents.Insert(0, new TableOfContentForPdfRecord()
                        {
                            Path = dlgOpen.FileName,
                            File = Path.GetFileName(dlgOpen.FileName),
                            Position = -1,
                            Pages = totalPages,
                            FileDate = fileDate,
                            IncludeInTOC = true
                        });

                        RefreshTOC();
                    }
                    else
                    {
                        MessageBox.Show("Corrupted file selected. Please select another file.");
                    }
                }
                finally
                {
                    button2.Enabled = true;
                }
            }
        }

        private void RefreshTOC()
        {
            var ordered = _TableOfContents.OrderBy(a => a.Position).ToArray();
            for (int x = 0; x < ordered.Length; x++)
                ordered[x].Position = x + 1;
            RefreshGrid();
        }

        private void CreateReport(List<TableOfContentForPdfRecord> records)
        {
            if (dlgSave.ShowDialog() != DialogResult.OK)
                return;


            button3.Enabled = false;
            byte[] reportData = null;
            try
            {
                var building = cmbBuilding.SelectedItem as Building;
                var year = cmbYear.SelectedItem as IdValue;
                var month = cmbMonth.SelectedItem as IdValue;
                var tocList = new List<TOCDataItem>();
                int pos = 1;
                foreach (var item in records.Where(a => a.IncludeInTOC).OrderBy(a => a.Position))
                {
                    var toc = new TOCDataItem()
                    {
                        ItemNumber = pos.ToString(),
                        ItemDescription = item.PrintDescription,
                        PageNumber = item.Pages,
                    };
                    pos++;
                    tocList.Add(toc);
                }

                int x = records.Count(a => a.IncludeInTOC == false);
                if(x > 0)
                {
                    int maxPos = tocList.Count();
                    var toc = new TOCDataItem()
                    {
                        ItemNumber = (maxPos+1).ToString(),
                        ItemDescription = "Invoicing",
                        PageNumber = x,
                    };
                    tocList.Add(toc);
                }

                using (var reportService = ReportServiceClient.CreateInstance())
                {
                    reportData = reportService.ManagementPackCoverPage(
                        new DateTime(year.Id, month.Id, 1),
                        building.Name, Controller.user.name, tocList.ToArray());
                    if (reportData != null)
                    {

                        if (File.Exists(dlgSave.FileName))
                            File.Delete(dlgSave.FileName);

                        using (FileStream ms = new FileStream(dlgSave.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            using (Document doc = new Document())
                            {
                                using (PdfCopy copy = new PdfCopy(doc, ms))
                                {
                                    doc.Open();

                                    AddPdfDocument(copy, reportData);
                                    foreach (var record in records)
                                    {
                                        var fileBytes = File.ReadAllBytes(record.Path);
                                        if (GetTotalPages(fileBytes) > 0)
                                        {
                                            AddPdfDocument(copy, fileBytes);
                                        }
                                        else
                                        {
                                            Controller.HandleError("Unable to verify " + record.File + " pdf is invalid");
                                            return;
                                        }
                                        Application.DoEvents();
                                    }
                                    //write output file
                                    ms.Flush();
                                }

                            }

                        }

                        var dt = new DateTime(year.Id, month.Id, 1);
                        DateTime now = DateTime.Now;

                        using (var context = SqlDataHandler.GetDataContext())
                        {
                            var managementPackReport = context.ManagementPackSet.SingleOrDefault(a => a.BuildingId == building.ID && a.Period == dt);
                            if(managementPackReport == null)
                            {
                                managementPackReport = new Data.ManagementPackData.ManagementPack()
                                {
                                    BuildingId = building.ID,
                                    Period = dt,
                                    DateCreated = now,
                                };
                                context.ManagementPackSet.Add(managementPackReport);
                            }
                            managementPackReport.UserId = Controller.user.id;
                            managementPackReport.DateUpdated = now;
                            managementPackReport.ReportData = File.ReadAllBytes(dlgSave.FileName);
                            context.SaveChanges();
                        }
                        Process.Start(dlgSave.FileName);
                    }
                    else
                    {
                        Controller.HandleError("Management pack failed to generate.");
                    }
                }
            }
            catch (Exception exp)
            {
                Controller.HandleError(exp);
            }
            finally
            {
                button3.Enabled = true;
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

        private void button3_Click(object sender, EventArgs e)
        {
            CreateReport(_TableOfContents);
        }

        private void btnAddLevyRoll_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            try
            {
                using (var reportService = ReportServiceClient.CreateInstance())
                {
                    try
                    {
                        DateTime dDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
                        byte[] reportData = null;
                        if (cbIncludeSundries.Checked)
                            reportData = reportService.LevyRollReport(dDate, (cmbBuilding.SelectedItem as Building).Name, (cmbBuilding.SelectedItem as Building).DataPath);
                        else
                            reportData = reportService.LevyRollExcludeSundries(dDate, (cmbBuilding.SelectedItem as Building).Name, (cmbBuilding.SelectedItem as Building).DataPath);
                        var reportFileName = Path.GetTempFileName();
                        File.WriteAllBytes(reportFileName, reportData);

                        var fileDate = DateTime.Now;
                        var totalPages = GetTotalPages(reportFileName);

                        _TableOfContents.Insert(0, new TableOfContentForPdfRecord()
                        {
                            Path = reportFileName,
                            File = Path.GetFileName(reportFileName),
                            Description = GetDescriptionList().Where(a => a.StartsWith("Levy")).FirstOrDefault(),
                            Position = -1,
                            Pages = totalPages,
                            FileDate = fileDate,
                            IsTempFile = true,
                            IncludeInTOC = true
                        });

                        RefreshTOC();

                    }
                    catch (Exception ex)
                    {
                        Controller.HandleError(ex);
                    }
                }
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btnCheckList_Click(object sender, EventArgs e)
        {
            var building = cmbBuilding.SelectedItem as Building;
            var year = cmbYear.SelectedItem as IdValue;
            var month = cmbMonth.SelectedItem as IdValue;
            var dt = new DateTime(year.Id, month.Id, 1);
            using (var context = SqlDataHandler.GetDataContext())
            {
                var checkList = context.tblMonthFins.SingleOrDefault(a => a.buildingID == building.Abbr && a.findate == dt);
                if(checkList == null || checkList.CheckListPDF == null)
                {
                    Controller.ShowMessage("No check list found, please create and save checklist for this building first.");
                    return;
                }

                var reportFileName = Path.GetTempFileName();
                File.WriteAllBytes(reportFileName, checkList.CheckListPDF);

                var fileDate = DateTime.Now;
                var totalPages = GetTotalPages(reportFileName);

                _TableOfContents.Insert(0, new TableOfContentForPdfRecord()
                {
                    Path = reportFileName,
                    File = Path.GetFileName(reportFileName),
                    Description = GetDescriptionList().Where(a => a.Contains("checklist")).FirstOrDefault(),
                    Position = -1,
                    Pages = totalPages,
                    FileDate = fileDate,
                    IsTempFile = true,
                    IncludeInTOC = true
                });

                RefreshTOC();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(_TableOfContents != null)
            {
                foreach(var itm in _TableOfContents.Where( a => a.IsTempFile))
                {
                    File.Delete(itm.Path);
                }
                _TableOfContents.Clear();
                RefreshTOC();
            }
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            try
            {
                var reportFileName = Path.GetTempFileName();

                using (var reportService = ReportServiceClient.CreateInstance())
                {
                    DateTime startDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    MaintenanceReportType repType;
                    if (rbDetailed.Checked)
                        repType = MaintenanceReportType.DetailedReport;
                    else if (rbDetailWithDocs.Checked)
                        repType = MaintenanceReportType.DetailedReportWithSupportingDocuments;
                    else
                        repType = MaintenanceReportType.SummaryReport;

                    var building = cmbBuilding.SelectedItem as Building;

                    var reportData = reportService.MaintenanceReport(SqlDataHandler.GetConnectionString(), repType, startDate, endDate, building.ID, building.Name, (cmbBuilding.SelectedItem as Building).DataPath);
                    if (reportData == null)
                    {
                        Controller.HandleError("No data found for " + startDate.ToString("MMM yyyy") + " - " + endDate.ToString("MMM yyyy"), "Maintenance Report");
                        return;
                    }

                    if (repType == MaintenanceReportType.DetailedReportWithSupportingDocuments)
                    {
                        byte[] combinedReport = null;


                        using (var dataContext = SqlDataHandler.GetDataContext())
                        {
                            var documentIds = (from m in dataContext.MaintenanceSet
                                               from d in m.MaintenanceDocuments
                                               where m.BuildingMaintenanceConfiguration.BuildingId == building.ID
                                                  && m.Requisition.trnDate >= startDate && m.Requisition.trnDate <= endDate
                                               orderby m.DateLogged
                                               select d.id).ToList();

                            var reqDocIds = (from m in dataContext.MaintenanceSet
                                             from d in m.Requisition.Documents
                                             where m.BuildingMaintenanceConfiguration.BuildingId == building.ID
                                                && m.Requisition.trnDate >= startDate && m.Requisition.trnDate <= endDate
                                             orderby m.DateLogged
                                             select d.id).ToList();

                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (Document doc = new Document())
                                {
                                    using (PdfCopy copy = new PdfCopy(doc, ms))
                                    {
                                        doc.Open();

                                        AddPdfDocument(copy, reportData);

                                        foreach (var documentId in reqDocIds)
                                        {
                                            var document = dataContext.RequisitionDocumentSet.Where(a => a.id == documentId).Select(a => a.FileData).Single();
                                            AddPdfDocument(copy, document);
                                        }

                                        foreach (var documentId in documentIds)
                                        {
                                            var document = dataContext.MaintenanceDocumentSet.Where(a => a.id == documentId).Select(a => a.FileData).Single();
                                            AddPdfDocument(copy, document);
                                        }

                                    }
                                }

                                combinedReport = ms.ToArray();
                            }
                        }

                        File.WriteAllBytes(reportFileName, combinedReport);
                    }
                    else
                        File.WriteAllBytes(reportFileName, reportData);

                    var fileDate = DateTime.Now;
                    var totalPages = GetTotalPages(reportFileName);

                    _TableOfContents.Insert(0, new TableOfContentForPdfRecord()
                    {
                        Path = reportFileName,
                        File = Path.GetFileName(reportFileName),
                        Description = GetDescriptionList().Where(a => a.StartsWith("Maintenance")).FirstOrDefault(),
                        Position = -1,
                        Pages = totalPages,
                        FileDate = fileDate,
                        IsTempFile = true,
                        IncludeInTOC = true
                    });
                    RefreshTOC();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                button1.Enabled = true;
            }
        }
    }

    class TableOfContentForPdfRecord
    {
        public TableOfContentForPdfRecord()
        {
            IsTempFile = false;
        }
        public string Path { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string PrintDescription
        {
            get
            {
                string result = string.Empty;
                if (!String.IsNullOrWhiteSpace(Description))
                    result = Description;

                if (!String.IsNullOrWhiteSpace(Description2))
                {
                    if (string.IsNullOrWhiteSpace(result))
                        result = Description2;
                    else
                        result = result + " - " + Description2;
                }

                return result.Trim();
            }
        }
        public int Pages { get; set; }
        public int Position { get; set; }
        public DateTime FileDate { get; set; }
        public DataGridViewRow DataRow { get;  set; }
        public bool IsTempFile { get;  set; }
        public bool IncludeInTOC { get;  set; }
    }
}
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

        private SqlDataHandler dh = new SqlDataHandler();

        public ManangementPackUserControl()
        {
            InitializeComponent();
            LoadBuildings();
            LoadYears();
            button2.Enabled = false;
            button3.Enabled = false;
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
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            try
            {
                LoadFiles();
                PopulateTableOfContents();
                button2.Enabled = true;
                button3.Enabled = true;
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private List<string> GetFilesInDirectory(int buildingId, string year, string month)
        {
            var buildingName = "";
            using (var context = SqlDataHandler.GetDataContext())
            {
                buildingName = context.tblBuildings
                        .FirstOrDefault(a => a.id == buildingId)?.DataFolder;
            }
            if (string.IsNullOrWhiteSpace(buildingName) ||
                string.IsNullOrWhiteSpace(month))
                return null;
            var fileLocation = $"{buildingName}\\Invoices\\{month.Substring(0,3)} {year}";
            return Directory.GetFiles(fileLocation).ToList();
        }

        private int GetTotalPages(byte[] filepath)
        {
            PdfReader reader = null;
            try
            {
                var returnVal = 0;
                using (reader = new PdfReader(filepath))
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
                var files = GetFilesInDirectory(building.ID, year.Value, month.Value);
                for (int i = 0; i < files?.Count; i++)
                {
                    var totalPages = GetTotalPages(File.ReadAllBytes(files[i]));
                    if (totalPages > 0)
                    {
                        _TableOfContents.Add(new TableOfContentForPdfRecord()
                        {
                            Path = files[i],
                            File = files[i].Split('\\').Last(),
                            Position = i + 1,
                            Pages = totalPages
                        });
                    }
                }
            }
            catch (Exception)
            {
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

            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Position",
                HeaderText = "Position",
                ReadOnly = true,
            });
            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Path",
                HeaderText = "Path",
                ReadOnly = true,
                Width = 100
            });
            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "File",
                HeaderText = "File",
                ReadOnly = true
            });
            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                ReadOnly = false,
                Width = 200
            });
            dgTocGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Pages",
                HeaderText = "Pages",
                ReadOnly = true
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
                Text = "Remove",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 20,
            });

            RefreshGrid();
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
                        case 5:
                            if (item.Position == 1)
                                return;
                            _TableOfContents.First(a => a.Position == item.Position - 1).Position = item.Position;
                            item.Position = item.Position - 1;
                            break;
                        case 6:
                            if (item.Position == _TableOfContents.Max(a=>a.Position))
                                return;
                            _TableOfContents.First(a => a.Position == item.Position + 1).Position = item.Position;
                            item.Position = item.Position + 1;
                            break;
                        case 7:
                            _TableOfContents.RemoveAll(a=>a.Position == item.Position);
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
                    var totalPages = GetTotalPages(File.ReadAllBytes(dlgOpen.FileName));
                    if (totalPages > 0)
                    {
                        _TableOfContents.Add(new TableOfContentForPdfRecord()
                        {
                            Path = dlgOpen.FileName,
                            File = dlgOpen.FileName.Split('\\').Last(),
                            Position = _TableOfContents.Any() ? _TableOfContents.Max(a => a.Position) + 1 : 1,
                            Pages = totalPages
                        });
                        RefreshGrid();
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

        private byte[] CreateReport(List<TableOfContentForPdfRecord> records)
        {
            bool processedOk = true;
            button3.Enabled = false;
            byte[] reportData = null;
            try
            {
                var building = cmbBuilding.SelectedItem as Building;
                var year = cmbYear.SelectedItem as IdValue;
                var month = cmbMonth.SelectedItem as IdValue;
                var tocList = new List<TOCDataItem>();
                var pageCount = 1;
                foreach (var item in records.OrderBy(a => a.Position))
                {
                    var toc = new TOCDataItem()
                    {
                        ItemNumber = item.Position.ToString(),
                        ItemDescription = item.Description,
                        PageNumber = pageCount,
                    };
                    tocList.Add(toc);
                    pageCount += item.Pages;
                }
                using (var reportService = ReportServiceClient.CreateInstance())
                {
                    reportData = reportService.ManagementPackCoverPage(
                        new DateTime(year.Id, month.Id, 1), 
                        building.Name, tocList.ToArray());
                    if (reportData != null)
                    {
                        if (dlgSave.ShowDialog() == DialogResult.OK)
                        {
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
                                                processedOk = false;
                                                Controller.HandleError("Unable to verify " + record.File + " pdf is invalid");
                                                throw new Exception("Unable to verify " + record.File + " pdf is invalid");
                                            }
                                            Application.DoEvents();
                                        }
                                        //write output file
                                        ms.Flush();
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cover page failed to generate.");
                    }
                }
            }
            catch (Exception exp)
            {
                processedOk = false;
                Controller.HandleError(exp);
            }
            finally
            {
                button3.Enabled = true;
            }

            if (processedOk == false)
                return null;

            return reportData;
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
            if (_TableOfContents.Any(a=>a.Description == "" || a.Description == null))
            {
                MessageBox.Show("All records should contain a Description. Please supply a description for all records");
                return;
            }
            var report = CreateReport(_TableOfContents);
            if (report == null)
                MessageBox.Show("Report could not be created.");
            else
                MessageBox.Show("Report was created successfully!");
        }
    }

    class TableOfContentForPdfRecord
    {
        public string Path { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public int Position { get; set; }
    }
}
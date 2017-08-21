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
            try
            {
                LoadFiles();
                PopulateTableOfContents();
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private List<string> GetFilesInDirectory(string buildingName, string year, string month)
        {
            if (string.IsNullOrWhiteSpace(buildingName) ||
                string.IsNullOrWhiteSpace(month))
                return null;
            var fileLocation = $"Y:/USERS - DO NOT MOVE!!/Buildings Managed (Do not Remove)/{StripSpecialCharacters(buildingName)}/Invoices/{month.Substring(0,3)} {year}";
            return Directory.GetFiles(fileLocation).ToList();
        }

        private string StripSpecialCharacters(string value)
        {
            var returnString = new StringBuilder();
            foreach (var c in value)
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    returnString.Append(c);
            return returnString.ToString();
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

        private void LoadFiles()
        {
            _TableOfContents = new List<TableOfContentForPdfRecord>();
            var building = cmbBuilding.SelectedItem as Building;
            var year = cmbYear.SelectedItem as IdValue;
            var month = cmbMonth.SelectedItem as IdValue;
            try
            {
                var files = GetFilesInDirectory(building.Name, year.Value, month.Value);
                for (int i = 0; i < files?.Count; i++)
                {
                    _TableOfContents.Add(new TableOfContentForPdfRecord()
                    {
                        Path = files[i],
                        File = files[i].Split('\\').Last(),
                        Position = i+1
                    });
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
                DataPropertyName = "Path",
                HeaderText = "Path",
                ReadOnly = true,
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
                MinimumWidth = 30
            });
            dgTocGrid.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "",
                Text = "Down",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 30
            });

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            BindingSource bs = new BindingSource();
            dgTocGrid.DataSource = bs;

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
                    MessageBox.Show($"C:{e.ColumnIndex} R:{e.RowIndex} I:{item.File}");
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
            }
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
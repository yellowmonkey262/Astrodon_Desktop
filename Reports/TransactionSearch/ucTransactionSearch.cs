using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.ReportService;
using System.Globalization;
using ExcelExportExample.ExcelHelper;
using System.IO;
using System.Diagnostics;
using Astrodon.Classes;
using ClosedXML.Excel;

namespace Astrodon.Reports.TransactionSearch
{
    public partial class ucTransactionSearch : UserControl
    {
        public ucTransactionSearch()
        {
            InitializeComponent();
        }

        bool searchStopped = false;
        private List<TransactionDataItem> _AllResults = new List<TransactionDataItem>();

        private void SearchTransactions()
        {
            searchStopped = false;
            using (var context = SqlDataHandler.GetDataContext())
            {
                var buildings = context.tblBuildings.ToList();

                foreach (var building in buildings)
                {
                    if (searchStopped)
                        break;

                    lblSearchStatus.Text = "Searching Building" + " " + building.Building;
                    Application.DoEvents();

                    using (var reportService = ReportServiceClient.CreateInstance())
                    {
                        try
                        {
                            DateTime fromDate = new DateTime(dtpFromDate.Value.Year, dtpFromDate.Value.Month, dtpFromDate.Value.Day, 0, 0, 0);
                            DateTime toDate = new DateTime(dtpToDate.Value.Year, dtpToDate.Value.Month, dtpToDate.Value.Day, 23, 59, 59);
                            string reference = tbReferenceContains.Text;
                            string description = tbDescriptionContains.Text;
                            decimal temp;
                            decimal? minimumAmount = decimal.TryParse(tbMinAmount.Text, out temp) ? temp : (decimal?)null;
                            decimal? maximumAmount = decimal.TryParse(tbMaxAmount.Text, out temp) ? temp : (decimal?)null;

                            var buildingResult = reportService.SearchPastel(building.DataPath, fromDate, toDate, reference, description, minimumAmount, maximumAmount);
                            _AllResults.AddRange(buildingResult);
                            UpdateDataGrid();
                        }
                        catch (Exception ex)
                        {
                            Controller.HandleError(ex.Message);
                        }
                    }
                }

                lblSearchStatus.Text = "Search Complete!";
            }
        }

        private void UpdateDataGrid()
        {
            dgvSearchResults.ClearSelection();
            dgvSearchResults.MultiSelect = false;
            dgvSearchResults.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();

            bs.DataSource = _AllResults;

            dgvSearchResults.Columns.Clear();

            dgvSearchResults.DataSource = bs;

            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BuildingPath",
                HeaderText = "Building",
                ReadOnly = true,
            });

            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TransactionDate",
                HeaderText = "Transaction Date",
                ReadOnly = true,
            });

            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AccountNumber",
                HeaderText = "Account",
                ReadOnly = true
            });

            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "LinkAccount",
                HeaderText = "Link Account",
                ReadOnly = true
            });

            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Refrence",
                HeaderText = "Reference",
                ReadOnly = true
            });

            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                ReadOnly = true
            });

            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Amount",
                HeaderText = "Amount",
                ReadOnly = true
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SearchTransactions();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            searchStopped = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files | *.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (sfd.FileName != "" && sfd.FileName.EndsWith(".xlsx"))
                        {
                            var excelProvider = new ExcelProvider();

                            var results = _AllResults.Select(a => new TransactionSearchModel
                            {
                                BuildingPath = a.BuildingPath,
                                TransactionDate = a.TransactionDate,
                                AccountNumber = a.AccountNumber,
                                LinkAccount = (a.LinkAccount == "\0\0\0\0\0\0\0") ? string.Empty : a.LinkAccount, //Pastel handles null values as \0\0\0\0\0\0\0
                                Reference = a.Refrence,
                                Description = a.Description,
                                Amount = a.Amount
                            }).AsQueryable();

                            var fileBytes = excelProvider.ExportQuery("Transaction Search", results, new ExcelStyleSheet());

                            File.WriteAllBytes(sfd.FileName, fileBytes);

                            Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}


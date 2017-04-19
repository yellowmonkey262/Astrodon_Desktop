using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Astrodon.Forms
{
    public partial class frmBuildingTrans : Form
    {
        private List<Trns> transactions;
        private String building;

        public frmBuildingTrans(String buildingName, List<Trns> trns)
        {
            building = buildingName;
            transactions = trns;
            InitializeComponent();
        }

        private void frmBuildingTrans_Load(object sender, EventArgs e)
        {
            dgTransactions.DataSource = transactions;
            dgTransactions.Columns[dgTransactions.ColumnCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgTransactions.Columns[dgTransactions.ColumnCount - 2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }

        private void CreateExcel()
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();

                if (xlApp == null)
                {
                    MessageBox.Show("EXCEL could not be started. Check that your office installation and project references are correct.");
                    return;
                }
                xlApp.Visible = true;

                Excel.Workbook wb = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

                if (ws == null)
                {
                    MessageBox.Show("Worksheet could not be created. Check that your office installation and project references are correct.");
                    return;
                }
                ws.Name = "Transaction Report";
                ws.Cells[1, "A"].Value2 = "Building";
                ws.Cells[1, "B"].Value2 = building;
                ws.Cells[3, "A"].Value2 = "Date";
                ws.Cells[3, "B"].Value2 = "Description";
                ws.Cells[3, "C"].Value2 = "Reference";
                ws.Cells[3, "D"].Value2 = "Amount";
                ws.Cells[3, "E"].Value2 = "Cumulative Amount";

                int rowIdx = 4;
                foreach (DataGridViewRow dvr in dgTransactions.Rows)
                {
                    try
                    {
                        ws.Cells[rowIdx, "A"].Value2 = (dvr.Cells[0].Value != null ? dvr.Cells[0].Value.ToString() : "");
                        ws.Cells[rowIdx, "B"].Value2 = (dvr.Cells[1].Value != null ? dvr.Cells[1].Value.ToString() : "");
                        ws.Cells[rowIdx, "C"].Value2 = (dvr.Cells[2].Value != null ? dvr.Cells[2].Value.ToString() : "");
                        ws.Cells[rowIdx, "D"].Value2 = (dvr.Cells[3].Value != null ? dvr.Cells[3].Value.ToString() : "");
                        ws.Cells[rowIdx, "D"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.Cells[rowIdx, "E"].Value2 = (dvr.Cells[5].Value != null ? dvr.Cells[5].Value.ToString() : "");
                        ws.Cells[rowIdx, "E"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        rowIdx++;
                    }
                    catch { }
                }

                ws.Columns.AutoFit();
                //ws.Application.ActiveWindow.SplitRow = 3;
                //ws.Application.ActiveWindow.SplitColumn = 1;
                //ws.Application.ActiveWindow.FreezePanes = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
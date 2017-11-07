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
using Astrodon.Data.DebitOrder;
using OfficeOpenXml;

namespace Astrodon.Reports.DebitOrder
{
    public partial class DebitOrderUserControl : UserControl
    {

        private List<Building> _Buildings;
        private List<IdValue> _Years;
        private List<IdValue> _Months;
        private SqlDataHandler dh = new SqlDataHandler();

        public DebitOrderUserControl()
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
            _Buildings.Insert(0, new Building() { ID = 0, Name = "All Buildings" });
            cmbBuilding.DataSource = _Buildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = false;
                try
                {
                    lbProgress.Text = "Starting";
                    var buildingList = _Buildings.Where(a => a.ID > 0).ToList();
                    if((cmbBuilding.SelectedItem as Building).ID != 0)
                        buildingList = _Buildings.Where(a => a.ID == (cmbBuilding.SelectedItem as Building).ID).ToList();

                    lbStatus.Items.Clear();
                    int errorCount = 0;

                    DateTime dDate = new DateTime((cmbYear.SelectedItem as IdValue).Id, (cmbMonth.SelectedItem as IdValue).Id, 1);
                    List<ExcelLineItem> compiledList = new List<ExcelLineItem>();
                    int buildingNum = 1;
                    using (var reportService = ReportServiceClient.CreateInstance())
                    {
                        foreach(var building in buildingList)
                        {
                            lbProgress.Text = "Processing " + buildingNum.ToString() +"/"+ buildingList.Count().ToString() +" => "+building.Name;
                            buildingNum++;
                            Application.DoEvents();
                            try
                            {
                                var items = reportService.RunDebitOrderForBuilding(SqlDataHandler.GetConnectionString(), building.ID, dDate, cbShowBreakdown.Checked);
                                if(items != null && items.Length > 0)
                                   compiledList.AddRange(items.Select(a => new ExcelLineItem(a,building.Abbr,building.Name)).ToList());

                                lbStatus.Items.Insert(0,building.Name + " => "+ items.Length.ToString() + " records");
                            }
                            catch(Exception ex)
                            {
                                lbStatus.Items.Insert(0, "ERROR => " +building.Name +" " + ex.Message);
                                errorCount++;
                            }
                            Application.DoEvents();
                        }
                        lbProgress.Text = "Compiling Excel File " + compiledList.Count().ToString() + " records total";
                        Application.DoEvents();
                        if (errorCount > 0)
                        {
                            Controller.HandleError("Warning " + errorCount.ToString() + " buildings that could not be processed.", "Warning");
                        }

                        byte[] reportData = null;

                        reportData = ExportDebitOrder(compiledList, cbShowBreakdown.Checked);

                        File.WriteAllBytes(dlgSave.FileName, reportData);
                        Process.Start(dlgSave.FileName);
                        lbProgress.Text = "Completed " + compiledList.Count().ToString() + " records total";
                        Application.DoEvents();

                    }
                }
                finally
                {
                    button1.Enabled = true;
                }
            }
        }

        byte[] ExportDebitOrder(List<ExcelLineItem> debitOrderItems, bool showFeeBreakdown)
        {
            byte[] result = null;
            using (var memStream = new MemoryStream())
            {
                using (ExcelPackage excelPkg = new ExcelPackage())
                {

                    using (ExcelWorksheet wsSheet1 = excelPkg.Workbook.Worksheets.Add("Debtors"))
                    {

                        wsSheet1.Cells["A1"].Value = "SUPPLIER ID";
                        wsSheet1.Cells["B1"].Value = "REFERENCE";
                        wsSheet1.Cells["C1"].Value = "SUPPLIER NAME";
                        wsSheet1.Cells["D1"].Value = "HOLNES";
                        wsSheet1.Cells["E1"].Value = "ACCOUNT NAME";
                        wsSheet1.Cells["F1"].Value = "DESCRIPTION";
                        wsSheet1.Cells["G1"].Value = "BRANCH CODE";
                        wsSheet1.Cells["H1"].Value = "ACCOUNT TYPE";
                        wsSheet1.Cells["I1"].Value = "ACCOUNT NO";
                        wsSheet1.Cells["J1"].Value = "COLLECTION DAY";
                        wsSheet1.Cells["K1"].Value = "COLLECTION AMOUNT";
                        if (showFeeBreakdown)
                        {
                            wsSheet1.Cells["L1"].Value = "DEBIT ORDER FEE";
                            wsSheet1.Cells["M1"].Value = "AMOUNT DUE";
                            wsSheet1.Cells["N1"].Value = "COMMENT";

                            wsSheet1.Cells["O1"].Value = "LEVY ROLL PAYMENTS";
                            wsSheet1.Cells["P1"].Value = "LEVY ROLL DUE";

                            wsSheet1.Cells["Q1"].Value = "BUILDING CODE";
                            wsSheet1.Cells["R1"].Value = "BUILDING NAME";

                            wsSheet1.Cells["S1"].Value = "CANCELLED";
                            wsSheet1.Cells["T1"].Value = "CANCEL DATE";

                        }
                        int rowNum = 1;
                        foreach (var row in debitOrderItems.Where(a => a.AmountDue > 0).OrderBy(a => a.Holnes).ThenBy(a => a.CustomerCode))
                        {
                            rowNum++;
                            wsSheet1.Cells["A" + rowNum.ToString()].Value = "";
                            wsSheet1.Cells["B" + rowNum.ToString()].Value = row.Reference;
                            wsSheet1.Cells["C" + rowNum.ToString()].Value = row.SupplierName;
                            wsSheet1.Cells["D" + rowNum.ToString()].Value = row.Holnes;
                            wsSheet1.Cells["E" + rowNum.ToString()].Value = row.CustomerName;
                            wsSheet1.Cells["F" + rowNum.ToString()].Value = row.Description;
                            wsSheet1.Cells["G" + rowNum.ToString()].Value = row.BranchCode;
                            wsSheet1.Cells["H" + rowNum.ToString()].Value = row.AccountType;
                            wsSheet1.Cells["I" + rowNum.ToString()].Value = row.AccountNumber;

                            wsSheet1.Cells["J" + rowNum.ToString()].Style.Numberformat.Format = "yyyy/MM/dd";
                            wsSheet1.Cells["J" + rowNum.ToString()].Value = row.CollectionDay;

                            wsSheet1.Cells["K" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                            wsSheet1.Cells["K" + rowNum.ToString()].Value = row.CollectionAmount;



                            if (showFeeBreakdown)
                            {
                                if (row.ExportDebitOrderFee != 0)
                                {
                                    wsSheet1.Cells["L" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                                    wsSheet1.Cells["L" + rowNum.ToString()].Value = row.ExportDebitOrderFee;
                                }

                                wsSheet1.Cells["M" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                                wsSheet1.Cells["M" + rowNum.ToString()].Value = row.AmountDue;

                                string debitOrderComment = "";
                                if (row.IsDebitOrderFeeDisabledOnBuilding)
                                {
                                    if (row.DebitOrderFeeDisabled)
                                        debitOrderComment = "Fee disabled on building and unit.";
                                    else
                                        debitOrderComment = "Fee disabled on building.";
                                }
                                else
                                {
                                    if (row.DebitOrderFeeDisabled)
                                        debitOrderComment = "Fee disabled on unit.";
                                }

                                wsSheet1.Cells["N" + rowNum.ToString()].Value = debitOrderComment;

                                wsSheet1.Cells["O" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                                wsSheet1.Cells["O" + rowNum.ToString()].Value = row.LevyRollPayments;

                                wsSheet1.Cells["P" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                                wsSheet1.Cells["P" + rowNum.ToString()].Value = row.LevyRollDue;

                                wsSheet1.Cells["Q" + rowNum.ToString()].Value = row.BuildingCode;
                                wsSheet1.Cells["R" + rowNum.ToString()].Value = row.BuildingName;

                                wsSheet1.Cells["S" + rowNum.ToString()].Value = row.DebitOrderCancelled ? "YES" : "";
                                if (row.DebitOrderCancelDate != null)
                                {
                                    wsSheet1.Cells["T" + rowNum.ToString()].Style.Numberformat.Format = "yyyy/MM/dd";
                                    wsSheet1.Cells["T" + rowNum.ToString()].Value = row.DebitOrderCancelDate.Value;
                                }
                            }
                        }


                        if(showFeeBreakdown && rowNum >= 1)
                        {
                            //ADD TOTALS
                            rowNum++;

                            wsSheet1.Cells["J" + rowNum.ToString()].Value = "TOTAL:";
                            wsSheet1.Cells["j" + rowNum.ToString()].Style.Font.Bold = true;

                            wsSheet1.Cells["K" + rowNum.ToString()].Formula = "SUM(K2:K"+(rowNum-1).ToString()+")";
                            wsSheet1.Cells["K" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                            wsSheet1.Cells["K" + rowNum.ToString()].Style.Font.Bold = true;

                            wsSheet1.Cells["L" + rowNum.ToString()].Formula = "SUM(L2:L" + (rowNum - 1).ToString() + ")";
                            wsSheet1.Cells["L" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                            wsSheet1.Cells["L" + rowNum.ToString()].Style.Font.Bold = true;

                            wsSheet1.Cells["M" + rowNum.ToString()].Formula = "SUM(M2:M" + (rowNum - 1).ToString() + ")";
                            wsSheet1.Cells["M" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                            wsSheet1.Cells["M" + rowNum.ToString()].Style.Font.Bold = true;

                            wsSheet1.Cells["O" + rowNum.ToString()].Formula = "SUM(O2:O" + (rowNum - 1).ToString() + ")";
                            wsSheet1.Cells["O" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                            wsSheet1.Cells["O" + rowNum.ToString()].Style.Font.Bold = true;

                            wsSheet1.Cells["P" + rowNum.ToString()].Formula = "SUM(P2:P" + (rowNum - 1).ToString() + ")";
                            wsSheet1.Cells["P" + rowNum.ToString()].Style.Numberformat.Format = "#,##0.00";
                            wsSheet1.Cells["P" + rowNum.ToString()].Style.Font.Bold = true;

                        }

                        wsSheet1.Protection.IsProtected = false;
                        wsSheet1.Protection.AllowSelectLockedCells = false;
                        wsSheet1.Cells.AutoFitColumns();

                        excelPkg.SaveAs(memStream);
                        memStream.Flush();
                        result = memStream.ToArray();
                    }
                }
            }
            return result;
        }
    }

    class ExcelLineItem
    {
        public ExcelLineItem(DebitOrderItem itm, string buildingCode, string buildingName)
        {
            BuildingId = itm.BuildingId;
            IsDebitOrderFeeDisabledOnBuilding = itm.IsDebitOrderFeeDisabledOnBuilding;
            CustomerCode = itm.CustomerCode;
            CustomerName = itm.CustomerName;
            AccountTypeId = itm.AccountTypeId;
            BranchCode = itm.BranchCode;
            AccountNumber = itm.AccountNumber;
            CollectionDay = itm.CollectionDay;
            DebitOrderCollectionDay = itm.DebitOrderCollectionDay;
            DebitOrderFee = itm.DebitOrderFee;
            AmountDue = itm.AmountDue;
            DebitOrderFeeDisabled = itm.DebitOrderFeeDisabled;
            BuildingCode = buildingCode;
            BuildingName = buildingName;
            LevyRollDue = itm.LevyRollDue;
            LevyRollPayments = itm.Payments;
            DebitOrderCancelled = itm.DebitOrderCancelled;
            DebitOrderCancelDate = itm.DebitOrderCancelDate;

        }
        public int BuildingId { get; set; }
        public bool IsDebitOrderFeeDisabledOnBuilding { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public AccountTypeType AccountTypeId { get; set; }
        public string BranchCode { get; set; }

        public string AccountNumber { get; set; }
        public DateTime CollectionDay { get; set; }
        public DebitOrderDayType DebitOrderCollectionDay { get; set; }
        public decimal DebitOrderFee { get; set; }
        public decimal AmountDue { get; set; }

        public decimal LevyRollDue { get; set; }
        public decimal LevyRollPayments { get; set; }

        public bool DebitOrderFeeDisabled { get; set; }

        public bool DebitOrderCancelled { get; set; }

        public DateTime? DebitOrderCancelDate { get; set; }

        #region ExportField
        public string SupplierId { get { return string.Empty; } }
        public string Reference { get { return "D/" + CustomerCode; } }
        public string SupplierName { get { return "ASTRODON"; } }
        public string Holnes { get { return CustomerCode + " " + CustomerName; } }
        public string Description { get { return "ASTRODON"; } }

        public decimal ExportDebitOrderFee
        {
            get
            {
                if (IsDebitOrderFeeDisabledOnBuilding || DebitOrderFeeDisabled)
                    return 0;
                else
                    return DebitOrderFee;
            }
        }

        public decimal CollectionAmount
        {
            get
            {
                return AmountDue + ExportDebitOrderFee;
            }
        }

        public string AccountType { get { return ((int)AccountTypeId).ToString(); } }

        public string BuildingCode { get; private set; }
        public string BuildingName { get; private set; }
        #endregion

    }
}

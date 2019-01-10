﻿using ClosedXML.Excel;
using ExcelExportExample.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.BuildingPMDebtor
{
    public class BuildingPMDebtorResult
    {
        [ExcelExport(1, "Building ID", true, false, XLCellValues.Number)]
        public int BuildingId { get; set; }

        [ExcelExport(2, "Building Name", true, false, XLCellValues.Text)]
        public string BuildingName { get; set; }

        [ExcelExport(3, "ABR", true, false, XLCellValues.Text)]
        public string ABR { get; set; }

        [ExcelExport(4, "Units", true, false, XLCellValues.Number)]
        public int Units { get; set; }

        public string YearEndPeriod { get; set; }

        [ExcelExport(5, "Year End", true, false, XLCellValues.Text)]
        public string YearEnd
        {
            get
            {
                if (String.IsNullOrWhiteSpace(YearEndPeriod))
                    return string.Empty;


                int month = 0;
                if (int.TryParse(YearEndPeriod, out month))
                {
                    var dt = new DateTime(2000, 01, 01);
                    dt = dt.AddMonths(month + 1);
                    return dt.ToString("MMM");
                }

                return string.Empty;
            }
        }

        [ExcelExport(6, "Code", true, false, XLCellValues.Text)]
        public string Code { get; set; }

        [ExcelExport(7, "Data Path", true, false, XLCellValues.Text)]
        public string DataPath { get; set; }

        public int PortfolioManagerId { get; set; }

        [ExcelExport(8, "PM", true, false, XLCellValues.Text)]
        public string PortfolioManager { get; set; }

        [ExcelExport(9, "PM Email", true, false, XLCellValues.Text)]
        public string PortfolioManagerEmail { get; set; }

        public int DebtorId { get; set; }

        [ExcelExport(10, "Debtor", true, false, XLCellValues.Text)]
        public string Debtor { get; set; }

        [ExcelExport(11, "Debtor Email", true, false, XLCellValues.Text)]
        public string DebtorEmail { get; set; }

        [ExcelExport(12, "Bank", true, false, XLCellValues.Text)]
        public string Bank { get; set; }

        [ExcelExport(13, "Account Number", true, false, XLCellValues.Text)]
        public string AccountNumber { get; set; }

        [ExcelExport(14, "Branch Code", true, false, XLCellValues.Text)]
        public string BranchCode { get; set; }

        [ExcelExport(15, "Address Line 1", true, false, XLCellValues.Text)]
        public string AddressLine1 { get; set; }

        [ExcelExport(16, "Address Line 2", true, false, XLCellValues.Text)]
        public string AddressLine2 { get; set; }

        [ExcelExport(17, "Address Line 3", true, false, XLCellValues.Text)]
        public string AddressLine3 { get; set; }

        [ExcelExport(18, "Address Line 4", true, false, XLCellValues.Text)]
        public string AddressLine4 { get; set; }

        [ExcelExport(19, "Address Line 5", true, false, XLCellValues.Text)]
        public string AddressLine5 { get; set; }
    }
}

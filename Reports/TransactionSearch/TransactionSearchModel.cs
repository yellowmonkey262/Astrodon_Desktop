using ClosedXML.Excel;
using ExcelExportExample.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.TransactionSearch
{
    public class TransactionSearchModel
    {
        [ExcelExport(1, "Building", true, false, XLCellValues.Text)]
        public string BuildingPath { get; set; }

        [ExcelExport(2, "Transaction Date", true, false, XLCellValues.DateTime)]
        public DateTime TransactionDate { get; set; }

        [ExcelExport(3, "Account Number", true, false, XLCellValues.Text)]
        public string AccountNumber { get; set; }

        [ExcelExport(4, "Link Account", true, false, XLCellValues.Text)]
        public string LinkAccount { get; set; }

        [ExcelExport(5, "Reference", true, false, XLCellValues.Text)]
        public string Reference { get; set; }

        [ExcelExport(6, "Description", true, false, XLCellValues.Text)]
        public string Description { get; set; }

        [ExcelExport(7, "Amount", true, false, XLCellValues.Number)]
        public decimal Amount { get; set; }
    }
}

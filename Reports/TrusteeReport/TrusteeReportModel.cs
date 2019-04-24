using ClosedXML.Excel;
using ExcelExportExample.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.TrusteeReport
{
    class TrusteeReportModel
    {
        [ExcelExport(1, "Code", true, false, XLCellValues.Text)]
        public string Code { get; set; }

        [ExcelExport(2, "Building Name", true, false, XLCellValues.Text)]
        public string BuildingName { get; set; }

        [ExcelExport(3, "Portfolio", true, false, XLCellValues.Text)]
        public string Portfolio { get; set; }

        [ExcelExport(4, "Account Number", true, false, XLCellValues.Text)]
        public string AccountNumber { get; set; }

        [ExcelExport(5, "Customer Full Name", true, false, XLCellValues.Text)]
        public string CustomerFullName { get; set; }

        [ExcelExport(6, "Cell Number", true, false, XLCellValues.Text)]
        public string CellNumber { get; set; }

        [ExcelExport(7, "Email Address", true, false, XLCellValues.Text)]
        public string EmailAddress { get; set; }
    }
}

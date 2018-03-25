using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.Data;
using System.IO;
using OfficeOpenXml;
using System.Globalization;

namespace Astrodon.Reports.MonthlyReport
{
    public class MonthlyReportExport
    {
        private DataContext _DataContext;

        public MonthlyReportExport(DataContext dc)
        {
            this._DataContext = dc;
        }

        public byte[] RunReport(DateTime period, bool completedItems, int? userId)
        {
            var processMonth = new DateTime(period.Year, period.Month, 1);

            //root query
            var qRoot = from b in _DataContext.tblBuildings
                        join f in _DataContext.tblMonthFins on b.Code equals f.buildingID into fx
                        from fin in fx.Where(a => a.findate == processMonth).DefaultIfEmpty()
                        where b.BuildingDisabled == false
                        && b.BuildingFinancialsEnabled == true
                        select new
                        {
                            BuildingId = b.id,
                            Building = b.Building,
                            Code = b.Code,
                            FinancialPeriod = period,
                            ProcessedDate = fin != null ? fin.completeDate : (DateTime?)null,
                            UserId = fin != null ? fin.userID != 0 ? fin.userID : (int?)null : (int?)null
                        };

            var qData = from r in qRoot
                        join u in _DataContext.tblUsers on r.UserId equals u.id into ux
                        from usr in ux.DefaultIfEmpty()
                        select new MonthlyReportItem
                        {
                            BuildingId = r.BuildingId,
                            Building = r.Building,
                            Code = r.Code,
                            FinancialPeriod = r.FinancialPeriod,
                            ProcessedDate = r.ProcessedDate,
                            UserId = r.UserId,
                            UserName = usr != null ? usr.name : (string)null
                        };

            var allData = qData.ToList();

            if (completedItems)
                allData = allData.Where(a => a.ProcessedDate != null).OrderBy(a => a.Code).ToList();
            if (userId != null & userId > 0)
                allData = allData.Where(a => a.UserId == userId).OrderBy(a => a.Code).ToList();
                

            byte[] result = null;
            using (var memStream = new MemoryStream())
            {
                using (ExcelPackage excelPkg = new ExcelPackage())
                {

                    using (ExcelWorksheet wsSheet1 = excelPkg.Workbook.Worksheets.Add("Financial Checklist Report"))
                    {
                        var headerCells = wsSheet1.Cells[1, 1, 1, 5];
                        var headerFont = headerCells.Style.Font;
                        headerFont.Bold = true;
                        
                        wsSheet1.Cells["A1"].Value = "Building";
                        wsSheet1.Cells["B1"].Value = "Code";
                        wsSheet1.Cells["C1"].Value = "Financial Period";
                        wsSheet1.Cells["D1"].Value = "Processed Date";
                        wsSheet1.Cells["E1"].Value = "User";

                        int rowNum = 2;
                        foreach (var row in allData)
                        {
                            wsSheet1.Cells["A" + rowNum.ToString()].Value = row.Building;
                            wsSheet1.Cells["B" + rowNum.ToString()].Value = row.Code;
                            wsSheet1.Cells["C" + rowNum.ToString()].Value = row.FinancialPeriod.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                            if (row.ProcessedDate != null)
                                wsSheet1.Cells["D" + rowNum.ToString()].Value = row.ProcessedDate.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                            if (!String.IsNullOrWhiteSpace(row.UserName))
                                wsSheet1.Cells["E" + rowNum.ToString()].Value = row.UserName;
                            rowNum++;
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
}

#region Usings

using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

#endregion

namespace ExcelExportExample.ExcelHelper
{
    public interface IExcelProvider
    {
        byte[] ExportQuery<T>(string sheetName, IQueryable<T> query, IExcelStyleSheet styleSheet,
            bool hasProtection = false);

        byte[] ExportQueryAsIs<T>(string sheetName, List<T> query, IExcelStyleSheet styleSheet,
            bool hasProtection = false);

        ICollection<T> ReadXLS<T>(byte[] xlsFile, string sheetName);

        void AddWorkSheet<T>(string sheetName, XLWorkbook workBook, List<T> query, IExcelStyleSheet styleSheet,
            bool hasProtection = false, bool hasManualHeader = false, bool addCreatedDate = false, bool searchForAttribute = true);
    }
}
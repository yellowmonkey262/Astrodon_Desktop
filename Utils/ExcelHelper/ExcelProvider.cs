#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

#endregion

namespace ExcelExportExample.ExcelHelper
{
    public class ExcelProvider : IExcelProvider
    {
        public byte[] ExportQuery<T>(string sheetName, IQueryable<T> query, IExcelStyleSheet styleSheet,
            bool hasProtection = false)
        {
            byte[] result = null;
            using (var workBook = new XLWorkbook())
            {
                AddWorkSheet(sheetName, workBook, query.ToList(), styleSheet, hasProtection);

                using (var memStream = new MemoryStream())
                {
                    workBook.SaveAs(memStream);
                    result = memStream.ToArray();
                }
                workBook.CalculateMode = XLCalculateMode.Auto;
            }
            return result;
        }

        public byte[] ExportQueryAsIs<T>(string sheetName, List<T> query, IExcelStyleSheet styleSheet,
            bool hasProtection = false)
        {
            byte[] result = null;
            using (var workBook = new XLWorkbook())
            {
                AddWorkSheet(sheetName, workBook, query, styleSheet, hasProtection, hasManualHeader: true, searchForAttribute: false);

                using (var memStream = new MemoryStream())
                {
                    workBook.SaveAs(memStream);
                    result = memStream.ToArray();
                }
                workBook.CalculateMode = XLCalculateMode.Auto;
            }
            return result;
        }

        public ICollection<T> ReadXLS<T>(byte[] xlsFile, string sheetName)
        {
            ICollection<T> result = new List<T>();

            using (var memStream = new MemoryStream(xlsFile))
            {
                using (var workBook = new XLWorkbook(memStream))
                {
                    using (var workSheet = workBook.Worksheet(sheetName))
                    {
                        if (workSheet.Protection.Protected)
                            workSheet.Protection.Unprotect(ExcelProtectKey);

                        List<ExcelDataProperty> columns;
                        var headerRow = Activator.CreateInstance<T>().ExportHeaderRow(out columns);
                        bool emptyRow;
                        var emptyCount = 0;
                        //read all data from sheet into result object
                        var rowCount = workSheet.RowCount();
                        for (var row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                emptyRow = true;
                                var obj = Activator.CreateInstance<T>();
                                for (var idx = 0; idx < columns.Count(); idx++)
                                {
                                    var rawValue = workSheet.Cell(row, idx + 1).Value;
                                    if (rawValue != null && !string.IsNullOrWhiteSpace(rawValue.ToString()))
                                    {
                                        emptyRow = false;
                                        var config = columns[idx];
                                        if (config.Property.CanWrite && config.Attribute.IsFormula == false)
                                        {
                                            var cellValue = ReadValue(config, rawValue);
                                            config.Property.SetValue(obj, cellValue, null);
                                        }
                                    }
                                }
                                if (!emptyRow)
                                {
                                    emptyCount = 0;
                                    result.Add(obj);
                                }
                                else
                                {
                                    emptyCount++; //100 consecutive empty rows stop reading the spread sheet
                                    if (emptyCount > 100)
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public string ExcelProtectKey
        {
            get { return "1qweSDerf%$3ds@"; }
        }

        public void AddWorkSheet<T>(string sheetName, XLWorkbook workBook, List<T> query, IExcelStyleSheet styleSheet,
            bool hasProtection = false, bool hasManualHeader = false,
            bool addCreatedDate = false, bool searchForAttribute = true)
        {
            using (var worksheet = workBook.Worksheets.Add(sheetName))
            {
                if (hasProtection)
                {
                    var ws = worksheet;
                    ws.Protect(ExcelProtectKey) // On this sheet we will only allow:
                        .SetFormatCells() // Cell Formatting
                        .SetInsertColumns() // Inserting Columns
                        .SetSort()
                        .SetSelectUnlockedCells();
                }
                var rowNo = 1;
                List<ExcelDataProperty> columns;
                var headerRow = Activator.CreateInstance<T>().ExportHeaderRow(out columns, searchForAttribute);
                var totalColumns = query.First().GetType().GetProperties().Count();
                styleSheet.HeadingFormat.Format(worksheet.Range(rowNo, 1, rowNo, totalColumns), false);
                if (!hasManualHeader)
                {
                    headerRow.InsertRow(worksheet, rowNo, 1);
                    rowNo++;
                }
                foreach (var data in query.ToList())
                {
                    var dtRow = data.ExportExcelRow(rowNo, searchForAttribute);
                    dtRow.InsertRow(worksheet, rowNo, 1);
                    if (rowNo != 1)
                        styleSheet.DetailFormat.Format(worksheet.Range(rowNo, 1, rowNo, totalColumns), false);
                    rowNo++;
                }

                for (var x = 0; x < columns.Count(); x++)
                {
                    if (columns[x].Attribute.IsHidden)
                    {
                        worksheet.Column(x + 1).Hide();
                    }
                    if (rowNo >= 2)
                    {
                        foreach (var cell in worksheet.Column(x + 1).Cells(2, rowNo - 1))
                        {
                            //if (columns[x].Attribute.IsLocked)
                            //    hasProtection = true;
                            cell.Style.Protection.Locked = columns[x].Attribute.IsLocked;
                        }
                    }
                }

                if (addCreatedDate)
                {
                    var cell = worksheet.Cell(rowNo + 1, 1);
                    cell.Value = "Created: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                worksheet.Columns().AdjustToContents();
            }
        }


        private object ReadValue(ExcelDataProperty config, object xlsValue)
        {
            var pType = config.Property.PropertyType;
            var underType = Nullable.GetUnderlyingType(pType);
            var myType = underType ?? pType;

            switch (Type.GetTypeCode(myType))
            {
                case TypeCode.Boolean:
                    if (xlsValue as string == "Y" || xlsValue as string == "y")
                        return true;
                    return false;
                case TypeCode.DateTime:
                    return (DateTime)xlsValue;
                case TypeCode.Decimal:
                    return Convert.ToDecimal(xlsValue);
                case TypeCode.Double:
                    return Convert.ToDouble(xlsValue);
                case TypeCode.Int16:
                    return Convert.ToInt16(xlsValue);
                case TypeCode.Int32:
                    return Convert.ToInt32(xlsValue);
                case TypeCode.Int64:
                    return Convert.ToInt64(xlsValue);
                case TypeCode.Single:
                    return Convert.ToSingle(xlsValue);
                case TypeCode.UInt16:
                    return Convert.ToUInt16(xlsValue);
                case TypeCode.UInt32:
                    return Convert.ToUInt32(xlsValue);
                case TypeCode.UInt64:
                    return Convert.ToUInt64(xlsValue);
                case TypeCode.String:
                    return ReadXLSString(xlsValue);
                default:
                    return xlsValue;
            }
        }

        private string ReadXLSString(object xlsValue)
        {
            if (xlsValue == null)
                return null;

            long lVal;
            if (long.TryParse(xlsValue.ToString(), out lVal))
                return lVal.ToString();
            return xlsValue.ToString().Trim();
        }
    }
}
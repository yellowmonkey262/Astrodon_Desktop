#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;

#endregion

namespace ExcelExportExample.ExcelHelper
{
    public static class ExcelExtentions
    {
        public static ExcelRow ExportExcelRow(this object source, int rowNumber, bool searchForAttribute = true)
        {
            var Cells = new List<ExcelCell>();
            var ExcelProperties = source.GetExcelDataProperties(searchForAttribute);

            foreach (var property in ExcelProperties)
            {
                var cellValue = property.Property.GetValue(source, null);
                if (property.Attribute.IsFormula)
                {
                    if (!string.IsNullOrWhiteSpace(cellValue as string))
                    {
                        var formula = cellValue as string;
                        cellValue = formula.Replace("[row]", rowNumber.ToString());
                    }
                }
                var cell = new ExcelCell(cellValue,
                    property.Attribute.FieldOrder,
                    property.Attribute.ExportDataType,
                    property.Attribute.Format,
                    property.Attribute.PadLeft,
                    property.Attribute.IsFormula,
                    property.Attribute.PaddingChar,
                    property.Attribute.FieldLength);
                Cells.Add(cell);
            }

            return new ExcelRow(Cells);
        }

        public static ExcelRow ExportHeaderRow(this object source, out List<ExcelDataProperty> excelProperties, bool searchForAttribute = true)
        {
            var cells = new List<ExcelCell>();
            excelProperties = source.GetExcelDataProperties(searchForAttribute);

            foreach (var property in excelProperties)
            {
                var cell = new ExcelCell(property.Attribute.Heading,
                    property.Attribute.FieldOrder,
                    XLCellValues.Text,
                    string.Empty,
                    property.Attribute.PadLeft,
                    property.Attribute.IsFormula,
                    property.Attribute.PaddingChar,
                    property.Attribute.FieldLength);
                cells.Add(cell);
            }

            return new ExcelRow(cells);
        }

        public static List<ExcelDataProperty> GetExcelDataProperties(this object source, bool searchForAttribute = true)
        {
            var result = new List<ExcelDataProperty>();
            var intFieldOrder = 1;
            foreach (var property in source.GetType().GetProperties())
            {
                if (searchForAttribute)
                {
                    foreach (var attrib in property.GetCustomAttributes(true))
                    {
                        if (attrib is ExcelExportAttribute)
                        {
                            result.Add(new ExcelDataProperty((ExcelExportAttribute)attrib, property));
                        }
                    }
                }
                else
                {
                    result.Add(new ExcelDataProperty(new ExcelExportAttribute(intFieldOrder++, intFieldOrder.ToString(), false, false, XLCellValues.Text), property));
                }
            }

            return result.OrderBy(a => a.Attribute.FieldOrder).ToList();
        }
    }

    public class ExcelDataProperty
    {
        public ExcelDataProperty(ExcelExportAttribute attribute, PropertyInfo property)
        {
            Attribute = attribute;
            Property = property;
        }

        public ExcelExportAttribute Attribute { get; private set; }
        public PropertyInfo Property { get; private set; }
    }
}
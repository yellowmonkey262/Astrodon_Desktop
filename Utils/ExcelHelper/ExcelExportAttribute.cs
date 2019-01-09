#region Usings

using System;
using ClosedXML.Excel;

#endregion

namespace ExcelExportExample.ExcelHelper
{
    public class ExcelExportAttribute : Attribute
    {
        private ExcelExportAttribute(int fieldOrder, string heading, bool isLocked, bool isHidden,
            XLCellValues exportDataType,
            string format, bool padLeft, bool isFormula, char? paddingChar, int? fieldLength)
        {
            Heading = heading;
            FieldOrder = fieldOrder;
            ExportDataType = exportDataType;
            Format = format;
            IsFormula = isFormula;
            PadLeft = padLeft;
            FieldLength = fieldLength;
            PaddingChar = paddingChar;
            FieldLength = fieldLength;
            IsLocked = isLocked;
            IsHidden = isHidden;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelExportAttribute"/> class.
        /// Aimed at numbers, text or date with no custom format required.
        /// </summary>
        /// <param name="fieldOrder">The field order.</param>
        /// <param name="exportDataType">Type of the export data.</param>
        public ExcelExportAttribute(int fieldOrder, string heading, bool isLocked, bool isHidden,
            XLCellValues exportDataType)
            : this(fieldOrder, heading, isLocked, isHidden, exportDataType, string.Empty, false, false, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelExportAttribute"/> class.
        /// Aimed at numbers that needs to be displayed as text with padding.
        /// </summary>
        /// <param name="fieldOrder">The field order.</param>
        /// <param name="paddingChar">The padding character.</param>
        /// <param name="fieldLength">Length of the field.</param>
        public ExcelExportAttribute(int fieldOrder, string heading, bool isLocked, bool isHidden, char paddingChar,
            int fieldLength)
            : this(
                fieldOrder, heading, isLocked, isHidden, XLCellValues.Text, string.Empty, true, false, paddingChar,
                fieldLength)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelExportAttribute"/> class.
        /// Aimed at numbers and dates that requires a custom format.
        /// </summary>
        /// <param name="fieldOrder">The field order.</param>
        /// <param name="format">The format.</param>
        /// <param name="exportDataType">Type of the export data.</param>
        public ExcelExportAttribute(int fieldOrder, string heading, bool isLocked, bool isHidden, string format,
            XLCellValues exportDataType)
            : this(fieldOrder, heading, isLocked, isHidden, exportDataType, format, false, false, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelExportAttribute"/> class.
        /// Aimed at numbers that needs to be calculated based on a formula with formatting.
        /// </summary>
        /// <param name="fieldOrder">The field order.</param>
        /// <param name="format">The format.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="exportDataType">Type of the export data.</param>
        public ExcelExportAttribute(int fieldOrder, string heading, bool isLocked, bool isHidden, string format)
            : this(fieldOrder, heading, isLocked, isHidden, XLCellValues.Number, format, false, true, null, null)
        {
        }

        public int FieldOrder { get; set; }

        public XLCellValues ExportDataType { get; set; }

        public int? FieldLength { get; set; }

        public bool PadLeft { get; set; }

        public char? PaddingChar { get; set; }

        public string Format { get; set; }

        public bool IsFormula { get; set; }

        public string Heading { get; set; }

        public bool IsLocked { get; set; }

        public bool IsHidden { get; set; }
    }
}
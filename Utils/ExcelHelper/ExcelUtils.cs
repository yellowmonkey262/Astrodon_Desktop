using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelExportExample.ExcelHelper
{
    public class ExcelFormatter
    {
        #region Constructor

        public ExcelFormatter()
        {
            BackgroundColor = null;

            #region Border Properties

            CellTopBorderColor = null;
            CellBottomBorderColor = null;
            CellLeftBorderColor = null;
            CellRightBorderColor = null;

            CellTopBorderStyle = null;
            CellBottomBorderStyle = null;
            CellLeftBorderStyle = null;
            CellRightBorderStyle = null;

            #endregion

            #region Cell Justification Properties

            WrapText = null;
            ShrinkToFit = null;
            HorizontalAlignment = null;
            VerticalAlignment = null;
            IndentAmount = null;
            TextRotation = null;

            #endregion

            #region Font Properties

            Bold = null;
            FontColor = null;
            FontName = string.Empty;
            FontSize = null;
            Italic = null;
            Shadow = null;
            Strikethrough = null;
            Underline = null;

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cell background color.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        public XLColor BackgroundColor { get; set; }

        #region Border Properties

        /// <summary>
        /// Gets or sets the top border color.
        /// </summary>
        /// <value>
        /// The color of the top border.
        /// </value>
        public XLColor CellTopBorderColor { get; set; }

        /// <summary>
        /// Gets or sets the bottom border color.
        /// </summary>
        /// <value>
        /// The color of the bottom border.
        /// </value>
        public XLColor CellBottomBorderColor { get; set; }

        /// <summary>
        /// Gets or sets the left border color.
        /// </summary>
        /// <value>
        /// The color of the left border.
        /// </value>
        public XLColor CellLeftBorderColor { get; set; }

        /// <summary>
        /// Gets or sets the right border color.
        /// </summary>
        /// <value>
        /// The color of the right border.
        /// </value>
        public XLColor CellRightBorderColor { get; set; }

        /// <summary>
        /// Gets or sets the range outside border color.
        /// </summary>
        /// <value>
        /// The color of the range outside border.
        /// </value>
        public XLColor RangeOutsideBorderColor { get; set; }

        /// <summary>
        /// Gets or sets the top border style.
        /// </summary>
        /// <value>
        /// The top border style type.
        /// </value>
        public XLBorderStyleValues? CellTopBorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the bottom border style.
        /// </summary>
        /// <value>
        /// The bottom border style type.
        /// </value>
        public XLBorderStyleValues? CellBottomBorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the left border style.
        /// </summary>
        /// <value>
        /// The left border style type.
        /// </value>
        public XLBorderStyleValues? CellLeftBorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the right border style.
        /// </summary>
        /// <value>
        /// The right border style type.
        /// </value>
        public XLBorderStyleValues? CellRightBorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the range outside border style. Used when you want to apply an outer border to a range.
        /// </summary>
        /// <value>
        /// The range outside border style.
        /// </value>
        public XLBorderStyleValues? RangeOutsideBorderStyle { get; set; }

        #endregion

        #region Cell Justification Properties

        /// <summary>
        /// Gets or sets a value indicating whether to wrap the text.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [wrap text]; otherwise, <c>false</c>.
        /// </value>
        public bool? WrapText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to shrink the text to fit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [shrink to fit]; otherwise, <c>false</c>.
        /// </value>
        public bool? ShrinkToFit { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment for the text.
        /// </summary>
        /// <value>
        /// The horizontal alignment type.
        /// </value>
        public XLAlignmentHorizontalValues? HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment for the text.
        /// </summary>
        /// <value>
        /// The vertical alignment type.
        /// </value>
        public XLAlignmentVerticalValues? VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the indent amount.
        /// </summary>
        /// <value>
        /// The amount of indentation on the text
        /// </value>
        public int? IndentAmount { get; set; }

        /// <summary>
        /// Gets or sets the text rotation.
        /// </summary>
        /// <value>
        /// The number of digress to rotate the text
        /// </value>
        public int? TextRotation { get; set; }

        #endregion

        #region Font Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExcelFormatter"/> is bold.
        /// </summary>
        /// <value>
        ///   <c>true</c> if bold; otherwise, <c>false</c>.
        /// </value>
        public bool? Bold { get; set; }

        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        /// <value>
        /// The color of the font.
        /// </value>
        public XLColor FontColor { get; set; }

        /// <summary>
        /// Gets or sets the font name.
        /// </summary>
        /// <value>
        /// The name of the font.
        /// </value>
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        public double? FontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExcelFormatter"/> is italic.
        /// </summary>
        /// <value>
        ///   <c>true</c> if italic; otherwise, <c>false</c>.
        /// </value>
        public bool? Italic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExcelFormatter"/> is shadow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if shadow; otherwise, <c>false</c>.
        /// </value>
        public bool? Shadow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExcelFormatter"/> is strikethrough.
        /// </summary>
        /// <value>
        ///   <c>true</c> if strikethrough; otherwise, <c>false</c>.
        /// </value>
        public bool? Strikethrough { get; set; }

        /// <summary>
        /// Gets or sets the underlining.
        /// </summary> 
        /// <value>
        /// The underline type.
        /// </value>
        public XLFontUnderlineValues? Underline { get; set; }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Formats the selected Cell based on the property values in the class.
        /// </summary>
        /// <param name="cell">The cell.</param>
        public void Format(IXLCell cell)
        {
            if (BackgroundColor != null)
                cell.Style.Fill.BackgroundColor = BackgroundColor;

            #region Set Cell Border Color

            if (CellTopBorderColor != null)
                cell.Style.Border.TopBorderColor = CellTopBorderColor;

            if (CellBottomBorderColor != null)
                cell.Style.Border.BottomBorderColor = CellBottomBorderColor;

            if (CellLeftBorderColor != null)
                cell.Style.Border.LeftBorderColor = CellLeftBorderColor;

            if (CellRightBorderColor != null)
                cell.Style.Border.RightBorderColor = CellRightBorderColor;

            #endregion

            #region Set Cell Border Style

            if (CellTopBorderStyle != null)
                cell.Style.Border.TopBorder = CellTopBorderStyle.Value;

            if (CellBottomBorderStyle != null)
                cell.Style.Border.BottomBorder = CellBottomBorderStyle.Value;

            if (CellLeftBorderStyle != null)
                cell.Style.Border.LeftBorder = CellLeftBorderStyle.Value;

            if (CellRightBorderStyle != null)
                cell.Style.Border.RightBorder = CellRightBorderStyle.Value;

            #endregion

            #region Set Cell Justification

            if (WrapText != null)
                cell.Style.Alignment.WrapText = WrapText.Value;

            if (ShrinkToFit != null)
                cell.Style.Alignment.ShrinkToFit = ShrinkToFit.Value;

            if (HorizontalAlignment != null)
                cell.Style.Alignment.Horizontal = HorizontalAlignment.Value;

            if (VerticalAlignment != null)
                cell.Style.Alignment.Vertical = VerticalAlignment.Value;

            if (IndentAmount != null)
                cell.Style.Alignment.Indent = IndentAmount.Value;

            if (TextRotation != null)
                cell.Style.Alignment.TextRotation = TextRotation.Value;

            #endregion

            #region Set Cell Font

            if (Bold != null)
                cell.Style.Font.Bold = Bold.Value;

            if (FontColor != null)
                cell.Style.Font.FontColor = FontColor;

            if (!string.IsNullOrEmpty(FontName))
                cell.Style.Font.FontName = FontName;

            if (Italic != null)
                cell.Style.Font.Italic = Italic.Value;

            if (Shadow != null)
                cell.Style.Font.Shadow = Shadow.Value;

            if (Strikethrough != null)
                cell.Style.Font.Strikethrough = Strikethrough.Value;

            if (Underline != null)
                cell.Style.Font.Underline = Underline.Value;

            if (FontSize != null)
                cell.Style.Font.FontSize = FontSize.Value;

            #endregion
        }

        /// <summary>
        /// Formats the selected Range based on the property values in the class.
        /// </summary>
        /// <param name="range">The range.</param>
        public void Format(IXLRange range, bool rangeOuterBorderOnly = true)
        {
            if (BackgroundColor != null)
                range.Style.Fill.BackgroundColor = BackgroundColor;

            #region Set Range Border Color

            if (CellTopBorderColor != null)
                range.Style.Border.TopBorderColor = CellTopBorderColor;

            if (CellBottomBorderColor != null)
                range.Style.Border.BottomBorderColor = CellBottomBorderColor;

            if (CellLeftBorderColor != null)
                range.Style.Border.LeftBorderColor = CellLeftBorderColor;

            if (CellRightBorderColor != null)
                range.Style.Border.RightBorderColor = CellRightBorderColor;

            if (RangeOutsideBorderColor != null)
                range.Style.Border.OutsideBorderColor = RangeOutsideBorderColor;

            #endregion

            #region Set Range Border Style

            if (rangeOuterBorderOnly == false)
            {
                if (CellTopBorderStyle != null)
                    range.Style.Border.TopBorder = CellTopBorderStyle.Value;

                if (CellBottomBorderStyle != null)
                    range.Style.Border.BottomBorder = CellBottomBorderStyle.Value;

                if (CellLeftBorderStyle != null)
                    range.Style.Border.LeftBorder = CellLeftBorderStyle.Value;

                if (CellRightBorderStyle != null)
                    range.Style.Border.RightBorder = CellRightBorderStyle.Value;
            }

            if (RangeOutsideBorderStyle != null)
                range.Style.Border.OutsideBorder = RangeOutsideBorderStyle.Value;

            #endregion

            #region Set Range Justification

            if (WrapText != null)
                range.Style.Alignment.WrapText = WrapText.Value;

            if (ShrinkToFit != null)
                range.Style.Alignment.ShrinkToFit = ShrinkToFit.Value;

            if (HorizontalAlignment != null)
                range.Style.Alignment.Horizontal = HorizontalAlignment.Value;

            if (VerticalAlignment != null)
                range.Style.Alignment.Vertical = VerticalAlignment.Value;

            if (IndentAmount != null)
                range.Style.Alignment.Indent = IndentAmount.Value;

            if (TextRotation != null)
                range.Style.Alignment.TextRotation = TextRotation.Value;

            #endregion

            #region Set Range Font

            if (Bold != null)
                range.Style.Font.Bold = Bold.Value;

            if (FontColor != null)
                range.Style.Font.FontColor = FontColor;

            if (!string.IsNullOrEmpty(FontName))
                range.Style.Font.FontName = FontName;

            if (Italic != null)
                range.Style.Font.Italic = Italic.Value;

            if (Shadow != null)
                range.Style.Font.Shadow = Shadow.Value;

            if (Strikethrough != null)
                range.Style.Font.Strikethrough = Strikethrough.Value;

            if (Underline != null)
                range.Style.Font.Underline = Underline.Value;

            if (FontSize != null)
                range.Style.Font.FontSize = FontSize.Value;

            #endregion
        }

        #endregion
    }

    public class ExcelRow
    {
        public ExcelRow(List<ExcelCell> cellList)
        {
            Cells = cellList.OrderBy(a => a.FieldOrder).ToList();
        }

        public List<ExcelCell> Cells { get; private set; }

        public void InsertRow(IXLWorksheet worksheet, int rowNumber, int cellNumber)
        {
            for (int i = 0; i < Cells.Count(); i++)
            {
                int celln = i + cellNumber;
                PopulateCell(worksheet.Cell(rowNumber, i + cellNumber), Cells[i].PropertyValue, Cells[i].ExportDataType, Cells[i].Format, Cells[i].PadLeft, Cells[i].IsFormula, Cells[i].PaddingChar, Cells[i].FieldLength);
            }

            //Adjust cell widths to content.
            //worksheet.Columns().AdjustToContents();
        }

        private void PopulateCell(IXLCell cell, object value, XLCellValues exportDataType, string format, bool padLeft, bool isFormula,
            char? paddingChar, int? fieldLength)
        {
            //Set DataType and Format
            switch (exportDataType)
            {
                case XLCellValues.Text:

                    if (value == null)
                        value = string.Empty;

                    cell.Value = "'" + ((padLeft == true && paddingChar != null) ? value.ToString().PadLeft(fieldLength.Value, paddingChar.Value) : value);
                    cell.DataType = exportDataType;
                    break;
                case XLCellValues.Number:

                    if (value != null)
                    {
                        //If a formula is assigned and a value is assigned the value overrides the formula, thus the value is only assigned if no formula is present.
                        if (isFormula)
                        {
                            cell.FormulaA1 = value.ToString().Replace(";", ",");
                        }
                        else
                        {
                            switch (Type.GetTypeCode(value.GetType()))
                            {
                                case TypeCode.String:
                                case TypeCode.Char:
                                    {
                                        long longResult = 0;
                                        if (value.ToString() == "")
                                            cell.Value = "";
                                        else
                                        {


                                            //Instead of using trying to parse all the types I just use long as it is the biggest signed integer. - (Willie SNR this does not make sense.... ?)
                                            if (long.TryParse(value.ToString(), out longResult))
                                                cell.Value = longResult;
                                            else
                                                throw new Exception("Unable to parse object to a Number.");
                                        }
                                        break;
                                    }
                                case TypeCode.Decimal:
                                case TypeCode.Double:
                                case TypeCode.Int16:
                                case TypeCode.Int32:
                                case TypeCode.Int64:
                                case TypeCode.Single:
                                case TypeCode.UInt16:
                                case TypeCode.UInt32:
                                case TypeCode.UInt64:
                                    {
                                        cell.Value = value;
                                        break;
                                    }
                                default:
                                    {
                                        throw new Exception("Object type not supported.");
                                    }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(format))
                        cell.Style.NumberFormat.Format = format;
                    else
                        cell.Style.NumberFormat.Format = "0";   //Assign default to display number with no decimal places, as without this the column type is General instead of Number.

                    cell.DataType = exportDataType;

                    break;
                case XLCellValues.DateTime:

                    if (value != null)
                    {
                        if (value.GetType() == typeof(string))
                        {
                            DateTime dateTimeResult;
                            if (DateTime.TryParse((string)value, out dateTimeResult))
                            {
                                cell.Value = dateTimeResult;

                                if (!string.IsNullOrEmpty(format))
                                {
                                    String dateTimeFormattedString = dateTimeResult.ToString(format, CultureInfo.InvariantCulture);
                                    cell.Value = "'" + dateTimeFormattedString;
                                    cell.DataType = XLCellValues.Text;
                                }
                                else
                                    cell.DataType = exportDataType;
                            }
                            else
                                throw new Exception("Unable to parse object to type DateTime.");
                        }
                        else if (value.GetType() == typeof(DateTime))
                        {
                            cell.Value = value;

                            if (!string.IsNullOrEmpty(format))
                            {
                                //Have to convert DateTime to string for formatting to work because if it isn't a string field the value is displayed in the Excel local format.
                                String dateTimeFormattedString = ((DateTime)value).ToString(format, CultureInfo.InvariantCulture);
                                cell.Value = "'" + dateTimeFormattedString;
                                cell.DataType = XLCellValues.Text;
                            }
                            else
                                cell.DataType = exportDataType;
                        }
                    }

                    break;
            }

            //Set column width
            //if (!string.IsNullOrEmpty(format) && format.IndexOf('.') != -1)
            //    cell.Worksheet.Column(cell.Address.ColumnNumber).Width = cell.Value.ToString().Length + format.Substring(format.IndexOf('.'), (format.Length - format.IndexOf('.'))).Length;
            //else
            //    cell.Worksheet.Column(cell.Address.ColumnNumber).Width = cell.Value.ToString().Length;
        }
    }

    public class ExcelCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelCell"/> class.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="fieldOrder">The field order.</param>
        /// <param name="fieldLength">Length of the field.</param>
        /// <param name="padLeft">if set to <c>true</c> [pad left].</param>
        /// <param name="paddingChar">The padding character.</param>
        /// <param name="format">The custom format.</param>
        /// <param name="exportType">Type of the export.</param> 
        public ExcelCell(object propertyValue, int fieldOrder, XLCellValues exportType, string format, bool padLeft, bool isFormula, char? paddingChar, int? fieldLength)
        {
            PropertyValue = propertyValue;
            FieldOrder = fieldOrder;
            ExportDataType = exportType;
            Format = format;
            IsFormula = isFormula;
            PadLeft = padLeft;

            FieldLength = fieldLength;
            PaddingChar = paddingChar;
        }

        public object PropertyValue { get; set; }

        public int FieldOrder { get; set; }

        public int? FieldLength { get; set; }

        public bool PadLeft { get; set; }

        public char? PaddingChar { get; set; }

        public string Format { get; set; }

        public bool IsFormula { get; set; }

        public XLCellValues ExportDataType { get; set; }

    }

}

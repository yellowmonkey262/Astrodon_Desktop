#region Usings

using ClosedXML.Excel;

#endregion

namespace ExcelExportExample.ExcelHelper
{
    public class ExcelStyleSheet : IExcelStyleSheet
    {
        public ExcelStyleSheet()
        {
            HeadingFormat = new ExcelFormatter
            {
                BackgroundColor = XLColor.White,
                CellTopBorderColor = XLColor.Black,
                CellBottomBorderColor = XLColor.Black,
                CellLeftBorderColor = XLColor.Black,
                CellRightBorderColor = XLColor.Black,
                CellTopBorderStyle = XLBorderStyleValues.Thin,
                CellBottomBorderStyle = XLBorderStyleValues.Thin,
                CellLeftBorderStyle = XLBorderStyleValues.Thin,
                CellRightBorderStyle = XLBorderStyleValues.Thin,
                FontName = "Arial",
                FontColor = XLColor.Black,
                FontSize = 8,
                Bold = true
            };


            DetailFormat = new ExcelFormatter
            {
                FontName = "Arial",
                FontColor = XLColor.Black,
                FontSize = 8,
                Bold = false
            };
        }

        public ExcelFormatter HeadingFormat { get; private set; }

        public ExcelFormatter DetailFormat { get; private set; }
    }
}
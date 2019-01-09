#region Usings


#endregion

namespace ExcelExportExample.ExcelHelper
{
    public interface IExcelStyleSheet
    {
        ExcelFormatter HeadingFormat { get; }

        ExcelFormatter DetailFormat { get; }
    }
}
using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace Astrodon.Classes
{
    public class ReportWriter
    {
        public void CreateSummaryReport(List<SummRep> summaries, String FileName, out String msg)
        {
            msg = "";
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(misValue, misValue, misValue, misValue);
            xlWorkSheet.Name = "Summary report";
            int y = 1;
            xlWorkSheet.Cells[y, 1] = "BANK";
            xlWorkSheet.Cells[y, 2] = "ABV";
            xlWorkSheet.Cells[y, 3] = "TRUST CODE";
            xlWorkSheet.Cells[y, 4] = "BUILDING NAME";
            xlWorkSheet.Cells[y, 5] = "BUILDING BALANCE";
            xlWorkSheet.Cells[y, 6] = "CENTREC BALANCE";
            xlWorkSheet.Cells[y, 7] = "DIFFERENCE";
            y++;
            foreach (SummRep summary in summaries)
            {
                xlWorkSheet.Cells[y, 1] = summary.Bank;
                xlWorkSheet.Cells[y, 2] = summary.Code;
                xlWorkSheet.Cells[y, 3] = summary.TrustCode;
                xlWorkSheet.Cells[y, 4] = summary.BuildingName;
                xlWorkSheet.Cells[y, 5] = summary.BuildBal;
                xlWorkSheet.Cells[y, 6] = summary.CentrecBal;
                xlWorkSheet.Cells[y, 7] = summary.Difference;
                y++;
            }
            releaseObject(xlWorkSheet, out msg);

            xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            releaseObject(xlWorkBook, out msg);
            releaseObject(xlApp, out msg);
            msg = (msg == "" ? "Excel Report Saved" : msg);
        }

        public void CreateDetailReport(ParentDetail summary, String FileName, out String msg)
        {
            msg = "";
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(misValue, misValue, misValue, misValue);
            xlWorkSheet.Name = CreateValidWorksheetName(summary.BuildName);
            int y = 1;
            xlWorkSheet.Cells[y, 1] = "TRUST CODE";
            xlWorkSheet.Cells[y, 2] = "BUILDING NAME";
            y++;
            xlWorkSheet.Cells[y, 1] = summary.TrustCode;
            xlWorkSheet.Cells[y, 2] = summary.BuildName;
            y += 2;
            xlWorkSheet.Cells[y, 1] = "DATE";
            xlWorkSheet.Cells[y, 2] = "DESCRIPTION";
            xlWorkSheet.Cells[y, 3] = "REFERENCE";
            xlWorkSheet.Cells[y, 4] = "AMOUNT";
            y++;
            foreach (Detail transaction in summary.Transactions)
            {
                xlWorkSheet.Cells[y, 1] = transaction.TrnDate.ToString("yyyy/MM/dd");
                xlWorkSheet.Cells[y, 2] = transaction.Description;
                xlWorkSheet.Cells[y, 3] = transaction.Reference;
                xlWorkSheet.Cells[y, 4] = transaction.Amt.ToString();
                y++;
            }
            releaseObject(xlWorkSheet, out msg);
            xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            releaseObject(xlWorkBook, out msg);
            releaseObject(xlApp, out msg);
            msg = (msg == "" ? "Excel Report Saved" : msg);
        }

        private string CreateValidWorksheetName(string name)
        {
            // Worksheet name cannot be longer than 31 characters.

            System.Text.StringBuilder escapedString;

            if (name.Length <= 31)
            {
                escapedString = new System.Text.StringBuilder(name);
            }
            else
            {
                escapedString = new System.Text.StringBuilder(name, 0, 31, 31);
            }
            String newString = "";
            for (int i = 0; i < escapedString.Length; i++)
            {
                String escapedStringIdea = escapedString.ToString().Substring(i, 1);
                if (escapedStringIdea == ":" || escapedStringIdea == "\\" || escapedStringIdea == "/" || escapedStringIdea == "?" ||
                    escapedStringIdea == "*" || escapedStringIdea == "[" || escapedStringIdea == "]")
                {
                    escapedStringIdea = "_";
                }
                newString += escapedStringIdea;
            }

            return newString;
        }

        private void releaseObject(object obj, out String msg)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);
                obj = null;
                msg = "";
            }
            catch (Exception ex)
            {
                obj = null;
                msg = "Exception Occured while releasing object " + ex.ToString();
            }
            finally
            {
                GC.Collect();
            }
        }

        public List<Dictionary<String, String>> ExtractData(String fileName, out String returnMessage)
        {
            object missing = System.Reflection.Missing.Value;

            String msg = String.Empty;
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            string str;
            int rCnt = 0;
            int cCnt = 0;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(fileName, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            List<Dictionary<String, String>> contents = new List<Dictionary<string, string>>();
            List<String> keys = new List<string>();
            returnMessage = "";
            for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
            {
                try
                {
                    if (rCnt == 1)
                    {
                        for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                        {
                            str = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value;
                            if (!String.IsNullOrEmpty(str)) { keys.Add(str); }
                        }
                    }
                    else
                    {
                        Dictionary<String, String> rowContent = new Dictionary<string, string>();
                        for (cCnt = 1; cCnt <= keys.Count; cCnt++)
                        {
                            String val = "";
                            try
                            {
                                object obj = (range.Cells[rCnt, cCnt] as Excel.Range).Value;
                                val = obj.ToString();
                            }
                            catch (Exception ex)
                            {
                                returnMessage += ex.Message + ";";
                            }

                            rowContent.Add(keys[cCnt - 1], val);
                        }
                        if (rowContent[keys[0]] != "")
                        {
                            contents.Add(rowContent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    returnMessage += ex.Message + ";";
                }
            }

            xlWorkBook.Close(true, missing, missing);
            xlApp.Quit();

            releaseObject(xlWorkSheet, out msg);
            releaseObject(xlWorkBook, out msg);
            releaseObject(xlApp, out msg);

            return contents;
        }
    }
}
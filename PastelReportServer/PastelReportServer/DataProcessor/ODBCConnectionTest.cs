using Astrodon.Classes;
using Astrodon.Data;
using Desktop.Lib.Pervasive;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Astrodon.DataProcessor
{
    public class ODBCConnectionTest
    {
        private DataContext _Context;

        public ODBCConnectionTest(DataContext dc)
        {
            _Context = dc;
        }

        public void Process()
        {
            DateTime checkDate = DateTime.Today.AddDays(-2);

            var q = from b in _Context.tblBuildings
                    where b.BuildingDisabled == false
                    && (b.ODBCConnectionOK == false || b.LastODBConnectionTest == null || b.LastODBConnectionTest <= checkDate)
                    select b;

            foreach (var building in q.ToList())
            {
                building.ODBCConnectionOK = TestBuilding(building.DataPath);
                building.LastODBConnectionTest = DateTime.Today;
                _Context.SaveChanges();
            }

            var failedBuildings = _Context.tblBuildings.Where(a => a.BuildingDisabled == false && a.ODBCConnectionOK == false).ToList();
            if (failedBuildings.Count > 0)
                EmailBuildingErrors(failedBuildings);
        }


        private void EmailBuildingErrors(List<tblBuilding> failedBuildings)
        {
            var excelFile = CreateExcelFile(failedBuildings);

            Dictionary<string, byte[]> attachments = new Dictionary<string, byte[]>();
            attachments.Add("ODBCErrors_" + DateTime.Today.ToString("yyyyMMdd") + ".xlsx", excelFile);
            string subject = "Building with ODBC Connection Errors";

            string bodyContent = Environment.NewLine + Environment.NewLine;

            bodyContent += "Kind Regards" + Environment.NewLine;
            bodyContent += "Tel: 011 867 3183" + Environment.NewLine;
            bodyContent += "Fax: 011 867 3163" + Environment.NewLine;
            bodyContent += "Direct Fax: 086 657 6199" + Environment.NewLine;
            bodyContent += "BEE Level 4 Contributor" + Environment.NewLine;

            bodyContent += "FOR AND ON BEHALF OF ASTRODON(PTY) LTD" + Environment.NewLine;
            bodyContent += "The information contained in this communication is confidential and may be legally privileged.It is intended solely for the use of the individual or entity to whom it is addressed and others authorized to receive it.If you are not the intended recipient you are hereby notified that any disclosure, copying, distribution or taking action in reliance of the contents of this information is strictly prohibited and may be unlawful.The company is neither liable for proper, complete transmission of the information contained in this communication nor any delay in its receipt." + Environment.NewLine;

            string status;

            if (!Mailer.SendMailWithAttachments("noreply@astrodon.co.za", new string[] { "tertia@astrodon.co.za" },
                subject, bodyContent,
                false, false, false, out status, attachments, "sheldon@astrodon.co.za"))
            {
                Console.WriteLine("Email failed " + status);

                throw new Exception("Unable to send email " + status);
            }

            Console.WriteLine("Email Sent!");

        }


        private byte[] CreateExcelFile(List<tblBuilding> failedBuildings)
        {
            byte[] result = null;
            using (var memStream = new MemoryStream())
            {
                using (ExcelPackage excelPkg = new ExcelPackage())
                {

                    using (ExcelWorksheet wsSheet1 = excelPkg.Workbook.Worksheets.Add("Buildings"))
                    {

                        wsSheet1.Cells["A1"].Value = "Building Name";
                        wsSheet1.Cells["A1"].Style.Font.Bold = true;

                        wsSheet1.Cells["B1"].Value = "Code";
                        wsSheet1.Cells["B1"].Style.Font.Bold = true;

                        wsSheet1.Cells["C1"].Value = "DataPath";
                        wsSheet1.Cells["C1"].Style.Font.Bold = true;

                        int rowNum = 1;
                        foreach (var row in failedBuildings.OrderBy(a => a.Building).ToList())
                        {
                            rowNum++;
                            wsSheet1.Cells["A" + rowNum.ToString()].Value = row.Building;
                            wsSheet1.Cells["B" + rowNum.ToString()].Value = row.Code;
                            wsSheet1.Cells["C" + rowNum.ToString()].Value = row.DataPath;
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


        private bool TestBuilding(string dataPath)
        {
            try
            {
                string odbcQuery = "select ID from [DataSet].LedgerParameters";

                odbcQuery = PervasiveSqlUtilities.SetDataSource(odbcQuery, dataPath);

                foreach (DataRow row in PervasiveSqlUtilities.FetchPervasiveData(odbcQuery).Rows)
                {
                    return true; //data found
                }

                throw new Exception("LedgerParameters returned zero rows");

            }
            catch (Exception e)
            {
                _Context.SystemLogSet.Add(new Data.Log.SystemLog()
                {
                    EventTime = DateTime.Now,
                    Message = "ODBC Connection Test " + e.Message,
                    StackTrace = e.StackTrace
                });
            }
            return false;
        }
    }
}

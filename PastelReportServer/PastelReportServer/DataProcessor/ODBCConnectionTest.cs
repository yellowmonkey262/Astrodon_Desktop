using Astrodon.Classes;
using Astrodon.CustomerMaintenance;
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
            DateTime checkDate = DateTime.Today;
            List<BuildingCategoryErrorModel> categoryErrors = new List<BuildingCategoryErrorModel>();

            var q = from b in _Context.tblBuildings
                    where b.BuildingDisabled == false
                    select b;

            var normalCategories = CustomerCategory.CategoryList.Where(a =>a.CategoryId < 100).ToList();
            var rentalCategories = CustomerCategory.CategoryList.Where(a => a.CategoryId > 100 || a.CategoryId == 0).ToList();


            foreach (var building in q.ToList())
            {
                building.ODBCConnectionOK = TestBuilding(building.DataPath);

                if (building.ODBCConnectionOK)
                {
                    if (building.DataPath.ToUpper().StartsWith("RENTAL"))
                        FixPastelCustomerCategories(building.DataPath, rentalCategories);
                    else
                        FixPastelCustomerCategories(building.DataPath, normalCategories);
                }

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

        private List<CustomerCategory> GetCustomerCategories(string buildPath)
        {
            string qry = "select  CCCode,CCDesc from [DataSet].CustomerCategories Order by CCCode";
            qry = PervasiveSqlUtilities.SetDataSource(qry, buildPath);

            List<CustomerCategory> result = new List<CustomerCategory>();

            var data = PervasiveSqlUtilities.FetchPervasiveData(qry);
            foreach (DataRow row in data.Rows)
            {
                CustomerCategory c = new CustomerCategory(row);
                result.Add(c);
            }

            return result;


        }

        private bool FixPastelCustomerCategories(string dataPath, List<CustomerCategory> categories)
        {
            try
            {

                var allowedIds = categories.Select(a => a.CategoryId).ToArray();

                string ids = "(" + string.Join(",", allowedIds) + ")";

                var catListOnDB = GetCustomerCategories(dataPath);

                var listToUpdate = new List<CustomerCategory>();
                var listToInsert = new List<CustomerCategory>();

                foreach (var cat in categories)
                {
                    var curr = catListOnDB.FirstOrDefault(a => a.CategoryId == cat.CategoryId);
                    if (curr == null)
                    {
                        listToInsert.Add(cat);
                    }
                    else if (curr.CategoryName != cat.CategoryName)
                    {
                        listToUpdate.Add(cat);
                    }

                }

                foreach(var i in listToInsert)
                {
                    string q = "Insert into [DataSet].CustomerCategories (CCCode,CCDesc) values (" + i.CategoryId.ToString() + ",'" + i.CategoryName + "')";
                    q = PervasiveSqlUtilities.SetDataSource(q, dataPath);
                    PervasiveSqlUtilities.ExecuteSQLCommand(q);
                }

                foreach (var i in listToUpdate)
                {
                    Console.WriteLine("Update " + i.CategoryName + " for " + dataPath);
                    string q = "Update [DataSet].CustomerCategories set CCDesc = '" + i.CategoryName + "' where CCCode = " + i.CategoryId.ToString();
                    q = PervasiveSqlUtilities.SetDataSource(q, dataPath);
                    PervasiveSqlUtilities.ExecuteSQLCommand(q);
                }


                string odbcQuery = "update [DataSet].CustomerMaster set Category = 0 where Category not in " + ids;
                odbcQuery = PervasiveSqlUtilities.SetDataSource(odbcQuery, dataPath);
                PervasiveSqlUtilities.ExecuteSQLCommand(odbcQuery);

                odbcQuery = "delete from [DataSet].CustomerCategories where CCCode not in " + ids;
                odbcQuery = PervasiveSqlUtilities.SetDataSource(odbcQuery, dataPath);
                PervasiveSqlUtilities.ExecuteSQLCommand(odbcQuery);

                return true;
            }
            catch (Exception e)
            {
                _Context.SystemLogSet.Add(new Data.Log.SystemLog()
                {
                    EventTime = DateTime.Now,
                    Message =  e.Message,
                    StackTrace = e.StackTrace
                });
            }
            return false;
        }
    }
}

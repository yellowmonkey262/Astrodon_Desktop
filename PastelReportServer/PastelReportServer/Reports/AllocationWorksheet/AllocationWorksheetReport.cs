using Astrodon.Classes;
using Astrodon.Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.AllocationWorksheet
{
    public class AllocationWorksheetReport
    {
        private DataContext context;

        public AllocationWorksheetReport(DataContext dataContext)
        {
            this.context = dataContext;
        }

        internal void EmailAllocations(int userId = 0)
        {
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                return; //do not schedule weekends

            if (context.PublicHolidaySet.Where(a => a.Date == DateTime.Today).Count() > 0)
                return; //today is a public holiday

            var userList = context.tblUsers.Where(a => a.ProcessCheckLists && (userId == 0 || a.id == userId)).ToList();

            foreach (var user in userList)
            {
                try
                {
                    List<AllocationItem> allocatedItems = new List<AllocationItem>();
                    var allocationItems = ProcessAllocation(context, user, 6, allocatedItems);
                    if (allocationItems.Count > 0)
                    {
                        EmailAllocationsToUser(user.email, allocationItems);
                        allocatedItems.AddRange(allocatedItems);
                    }
                }
                catch (Exception e)
                {
                    LogException(e,"Unable to process user " + user.name);
                }
            }
        }

    
        private void EmailAllocationsToUser(string email, List<AllocationItem> allocationItems)
        {
            var excelFile = CreateExcelFile(allocationItems);

            Dictionary<string, byte[]> attachments = new Dictionary<string, byte[]>();
            attachments.Add("WorkList_"+DateTime.Today.ToString("yyyyMMdd")+".xlsx", excelFile);
            string subject = "Building Allocation";

            string bodyContent = Environment.NewLine + Environment.NewLine;

            bodyContent += "Kind Regards" + Environment.NewLine;
            bodyContent += "Tel: 011 867 3183" + Environment.NewLine;
            bodyContent += "Fax: 011 867 3163" + Environment.NewLine;
            bodyContent += "Direct Fax: 086 657 6199" + Environment.NewLine;
            bodyContent += "BEE Level 4 Contributor" + Environment.NewLine;

            bodyContent += "FOR AND ON BEHALF OF ASTRODON(PTY) LTD" + Environment.NewLine;
            bodyContent += "The information contained in this communication is confidential and may be legally privileged.It is intended solely for the use of the individual or entity to whom it is addressed and others authorized to receive it.If you are not the intended recipient you are hereby notified that any disclosure, copying, distribution or taking action in reliance of the contents of this information is strictly prohibited and may be unlawful.The company is neither liable for proper, complete transmission of the information contained in this communication nor any delay in its receipt." + Environment.NewLine;

            string status;

            if (!Mailer.SendMailWithAttachments("noreply@astrodon.co.za", new string[] { email },
                subject, bodyContent,
                false, false, false, out status, attachments, "tertia@astrodon.co.za"))
            {
                Console.WriteLine("Email failed " + status);

                throw new Exception("Unable to send email " + status);
            }

            Console.WriteLine("Email Sent!");

        }

        private void LogException(Exception e,string section)
        {
            context.SystemLogSet.Add(new Data.Log.SystemLog()
            {
                EventTime = DateTime.Now,
                Message = section + "=>"+e.Message,
                StackTrace = e.StackTrace
            });
            context.SaveChanges();
        }

        private byte[] CreateExcelFile(List<AllocationItem> allocationItems)
        {
            byte[] result = null;
            using (var memStream = new MemoryStream())
            {
                using (ExcelPackage excelPkg = new ExcelPackage())
                {

                    using (ExcelWorksheet wsSheet1 = excelPkg.Workbook.Worksheets.Add("Debtors"))
                    {

                        wsSheet1.Cells["A1"].Value = "Name";
                        wsSheet1.Cells["A1"].Style.Font.Bold = true;

                        wsSheet1.Cells["B1"].Value = "Priority";
                        wsSheet1.Cells["B1"].Style.Font.Bold = true;

                        wsSheet1.Cells["C1"].Value = "Building";
                        wsSheet1.Cells["C1"].Style.Font.Bold = true;

                        wsSheet1.Cells["D1"].Value = "Building Name";
                        wsSheet1.Cells["D1"].Style.Font.Bold = true;

                        wsSheet1.Cells["E1"].Value = "Comment";
                        wsSheet1.Cells["E1"].Style.Font.Bold = true;

                        wsSheet1.Cells["F1"].Value = "Financial Period";
                        wsSheet1.Cells["F1"].Style.Font.Bold = true;

                        wsSheet1.Cells["G1"].Value = "YED";
                        wsSheet1.Cells["G1"].Style.Font.Bold = true;

                      


                        int rowNum = 1;
                        foreach (var row in allocationItems.OrderBy(a => a.UserName).ThenBy(a => a.Priority).ToList())
                        {
                            rowNum++;
                            wsSheet1.Cells["A" + rowNum.ToString()].Value = row.UserName;
                            wsSheet1.Cells["B" + rowNum.ToString()].Value = row.Priority;
                            wsSheet1.Cells["C" + rowNum.ToString()].Value = row.BuildingCode;
                            wsSheet1.Cells["D" + rowNum.ToString()].Value = row.BuildingName;
                            wsSheet1.Cells["E" + rowNum.ToString()].Value = row.AllocationReason;
                            wsSheet1.Cells["F" + rowNum.ToString()].Value = row.PeriodStart.ToString("MMM") + " - " + row.PeriodEnd.ToString("MMM");
                            wsSheet1.Cells["G" + rowNum.ToString()].Value = row.YearEndDays.ToString("MMM");

                            //wsSheet1.Cells["E" + rowNum.ToString()].Style.Numberformat.Format = "yyyy/MM/dd";
                            //wsSheet1.Cells["E" + rowNum.ToString()].Value = row.FinancialPeriod;

                            //wsSheet1.Cells["F" + rowNum.ToString()].Style.Numberformat.Format = "yyyy/MM/dd HH:mm";
                            //wsSheet1.Cells["F" + rowNum.ToString()].Value = row.MeetingDate;

                            //wsSheet1.Cells["G" + rowNum.ToString()].Value = row.MeetingType;
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

        private List<AllocationItem> ProcessAllocation(DataContext context, tblUser user, int buildingsToAllocate, List<AllocationItem> alreadyAllocated)
        {


            List<AllocationItem> result = new List<AllocationItem>();
            //find the buildings allocated to this user to process check lists for

            //limit financials to last month
            DateTime finMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            var query = from m in context.tblMonthFins
                        join u in context.tblUsers on m.userID equals u.id
                        join b in context.tblBuildings on m.buildingID equals b.Code
                        where m.completeDate == null
                        && b.BuildingDisabled == false
                        && u.id == user.id
                        && b.BuildingFinancialsEnabled == true
                        && m.findate <= finMonth
                        select new BuildingProspect
                        {
                            Building = b,
                            Financial = m,
                            FinancialStartDate = b.FinancialStartDate,
                            FinancialEndDate = b.FinancialEndDate,
                            FinancialMonth = m.findate
                        };

            var myBuildingsToProcess = query.Distinct().ToList().Where(a => a.IsCandidate)
                                                                .OrderBy(a => a.Financial.findate).ToList();


            var buildingIdList = myBuildingsToProcess.Select(a => a.Building.id).Distinct().ToList();

            var toRemove = alreadyAllocated.Where(a => buildingIdList.Contains(a.BuildingId)).Select(a => a.BuildingId).ToList();

            foreach (var x in toRemove)
                buildingIdList.Remove(x);

            var buildingIds = buildingIdList.Distinct().ToArray();

            var dtStart = DateTime.Today;
            var dtEnd = dtStart.AddHours(72);

            //find all calendar entries for these buildings > today and < today + 72 hours
            var calendarEntries = from c in context.BuildingCalendarEntrySet
                                  where c.BuildingId != null 
                                  && c.CalendarEntryType == Data.Calendar.CalendarEntryType.Financial
                                  && buildingIds.Contains(c.BuildingId.Value)
                                  && c.EntryDate >= dtStart
                                  && c.EntryDate <= dtEnd
                                  && c.Building.BuildingFinancialsEnabled == true
                                  select new
                                  {
                                      c.BuildingId,
                                      c.Building.Building,
                                      c.Building.Code,  
                                      c.EntryDate,
                                      c.Building.FinancialStartDate,
                                      c.Building.FinancialEndDate                                 
                                  };

            var calendarGroup = calendarEntries
                                .GroupBy(n => new { n.BuildingId, n.Building, n.Code })
                                .Select(g => new { g.Key.BuildingId, g.Key.Building, g.Key.Code, EventDate = g.Min(x => x.EntryDate) });




            foreach (var itm in calendarGroup.ToList())
            {
                var existing = result.FirstOrDefault(a => a.BuildingId == itm.BuildingId);
                if (existing == null)
                {
                    int priortiy = result.Count + 1;
                    result.Add(new AllocationItem()
                    {
                        BuildingId = itm.BuildingId.Value,
                        BuildingName = itm.Building,
                        BuildingCode = itm.Code,
                        Priority = priortiy,
                        UserId = user.id,
                        UserName = user.name,
                        OrderDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                        ReasonDate = itm.EventDate,
                        Reason = "Building has financial meeting Scheduled on",
                    
                    });
                }
            }

            //next allocate all other buildings not yet in the list
            buildingIds = result.Select(a => a.BuildingId).Distinct().ToArray();
            List<AllocationItem> randomBuildings = new List<AllocationItem>();

            foreach (var itm in myBuildingsToProcess
                                        .Where(a => !buildingIds.Contains(a.Building.id))
                                        .OrderBy(a => a.YearEndDays)
                                        .ThenBy(a => a.Financial.findate)
                                        .Select(a => a))
            {
                var existing = result.FirstOrDefault(a => a.BuildingId == itm.Building.id);
                if (existing == null)
                {

                    var finDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, itm.Building.FinancialDayOfMonth);
                    int priority = Math.Abs((DateTime.Today - finDate).Days);


                    randomBuildings.Add(new AllocationItem()
                    {
                        BuildingId = itm.Building.id,
                        BuildingName = itm.Building.Building,
                        BuildingCode = itm.Building.Code,
                        Priority = priority,
                        UserId = user.id,
                        UserName = user.name,
                        OrderDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, itm.Building.FinancialDayOfMonth),
                        ReasonDate = itm.Financial.findate,
                        FinancialStartDate = itm.Building.FinancialStartDate,
                        FinancialEndDate = itm.Building.FinancialEndDate,
                        Reason = "Financial is outstanding for period",
                        PeriodStart = itm.Building.FinancialPeriodStart,
                        PeriodEnd = itm.Building.FinancialPeriodEnd,
                        YearEndDays = itm.YearEndDays
                    });
                }
            }

            foreach (var itm in randomBuildings.OrderBy(a => a.Priority))
            {
                itm.Priority = result.Count() + 1;
                result.Add(itm);
            }

            var returnResult = result.Where(a => a.IsFinancialReady).OrderBy(a => a.Priority).Take(buildingsToAllocate).ToList();

            return returnResult;
        }

        class BuildingProspect
        {
            public tblBuilding Building { get; internal set; }
            public tblMonthFin Financial { get; internal set; }
            public DateTime? FinancialEndDate { get; internal set; }
            public DateTime FinancialMonth { get; internal set; }
            public DateTime? FinancialStartDate { get; internal set; }

            public double YearEndDays
            {
                get
                {
                    var dt = Building.FinancialPeriodEnd;
                    var checkDt = new DateTime(dt.Year, DateTime.Today.Month, 1);
                    var days = Math.Abs((checkDt - dt).TotalDays);
                    return days;
                }
            }

            public bool IsCandidate
            {
                get
                {
                    DateTime checkStart;
                    DateTime checkEnd;
                    DateTime chekFinMonth = new DateTime(FinancialMonth.Year, FinancialMonth.Month, 1);

                    if (FinancialEndDate != null)
                        checkStart = new DateTime(FinancialEndDate.Value.Year, FinancialEndDate.Value.Month, 1);
                    else
                        checkStart = FinancialMonth;

                    if (FinancialStartDate != null)
                        checkEnd = new DateTime(FinancialStartDate.Value.Year, FinancialStartDate.Value.Month, 1);
                    else
                        checkEnd = FinancialMonth;

                    return FinancialMonth >= checkStart && FinancialMonth <= checkEnd;

                }
            }
        }


    }
}

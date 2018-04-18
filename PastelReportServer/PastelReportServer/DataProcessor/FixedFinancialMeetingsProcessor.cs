using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.Data;
using Astrodon.Data.Calendar;
using Astrodon.Classes;

namespace Astrodon.DataProcessor
{
    public class FixedFinancialMeetingsProcessor
    {
        private DataContext _Context;
        public FixedFinancialMeetingsProcessor(DataContext dc)
        {
            _Context = dc;
        }

        internal void ScheduleMeetings()
        {
            //Step 1 - find all buildings with meetings allocated

            var buildings = _Context.tblBuildings
                                    .Where(a => a.FixedMonthyFinancialMeetingEnabled
                                             && a.BuildingDisabled == false
                                             && (a.FinancialStartDate == null || a.FinancialStartDate <= DateTime.Today)
                                             && (a.FinancialEndDate == null || a.FinancialEndDate >= DateTime.Today)
                                             ).ToList();

            foreach(var building in buildings)
            {
                try
                {
                    ScheduleBuildingMeeting(building);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }

        private void ScheduleBuildingMeeting(tblBuilding building)
        {
            //find a calendare entry for next month.

            DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var dtStart = dt.AddMonths(1); // next month
            var dtEnd = dt.AddMonths(2).AddDays(-1);

            var entryCount = _Context.BuildingCalendarEntrySet.Where(a => a.BuildingId == building.id
                                                                     && a.EventToDate >= dtStart
                                                                     && a.EventToDate <= dtEnd
                                                                     && a.Event == building.FinancialMeetingEvent).Count();

            if (entryCount > 0)
                return; //already scheduled

            //find the day to use

            var day = building.FinancialMeetingDayOfMonth;
            if (day == null)
                return;

            DateTime meetingDate = dtStart;
            try
            {
                meetingDate = new DateTime(dtStart.Year, dtStart.Month, day.Value);
            }
            catch
            {
                meetingDate = dtEnd; //must be end of month gone past the end of the month
            }

            //now find closest work day.
            var workDay = ClosestWorkDay(meetingDate);

            //find allocated PM
            var monthFin = _Context.tblMonthFins.Where(a => a.buildingID == building.Code && a.findate == dtStart).FirstOrDefault();
            if (monthFin == null)
                DoRandomAllocations(dtStart);


             monthFin = _Context.tblMonthFins.Where(a => a.buildingID == building.Code && a.findate == dtStart).FirstOrDefault();
            if(monthFin != null)
            {
                var meetingStart = new DateTime(workDay.Year, workDay.Month, workDay.Day, building.FinancialMeetingStartTime.Value.Hour, building.FinancialMeetingStartTime.Value.Minute,1);
                var meetingEnd = new DateTime(workDay.Year, workDay.Month, workDay.Day, building.FinancialMeetingEndTime.Value.Hour, building.FinancialMeetingEndTime.Value.Minute,1);
                //Schedule the meeting
                var calEntry = new BuildingCalendarEntry()
                {
                    CalendarEntryType = CalendarEntryType.Financial,
                    BuildingId = building.id,
                    UserId = monthFin.userID,
                    EntryDate = meetingStart,
                    EventToDate = meetingEnd,
                    Event = building.FinancialMeetingEvent,
                    Venue = building.FinancialMeetingVenue,
                    NotifyTrustees = building.FinancialMeetingSendInviteToAllTrustees,
                    BCCEmailAddress = building.FinancialMeetingBCC,
                    InviteSubject = building.FinancialMeetingSubject,
                    InviteBody = building.FinancialMeetingBodyText
                };
                _Context.BuildingCalendarEntrySet.Add(calEntry);
                _Context.SaveChanges();
                SendInviteOut(calEntry);
            }
        }

        private byte[] CreateCalendarInvite(string subject, string description,
         string location, DateTime startDate, DateTime endDate)
        {
            string contents =
                "BEGIN:VCALENDAR" + "\n" +
                "PRODID:-//" + "HCC" + "//" + "HealthCloud" + "//EN" + "\n" +
                "BEGIN:VEVENT" + "\n" +
                "DTSTART:" + startDate.ToString("yyyyMMdd\\THHmmss") + "\n" +
                "DTEND:" + endDate.ToString("yyyyMMdd\\THHmmss") + "\n" +
                "LOCATION:" + location + "\n" +
                "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + description + "\n" +
                "SUMMARY:" + subject + "\n" +
                "PRIORITY:3" + "\n" +
                "END:VEVENT" + "\n" +
                "END:VCALENDAR";
            return System.Text.Encoding.UTF8.GetBytes(contents.ToString());
        }

        private void SendInviteOut(BuildingCalendarEntry entry)
        {
            string status;
            var pm = _Context.tblUsers.Single(a => a.id == entry.UserId);
            var building = _Context.tblBuildings.Single(a => a.id == entry.BuildingId.Value);

            if (string.IsNullOrWhiteSpace(pm.email))
                return;

            var calendarInvite = CreateCalendarInvite(building.Building + " - " + entry.Event, "", entry.Venue, entry.EntryDate, entry.EventToDate);
            Dictionary<string, byte[]> attachments = new Dictionary<string, byte[]>();
            attachments.Add("Appointment.ics", calendarInvite);
            string subject = entry.InviteSubject;
            if (String.IsNullOrWhiteSpace(subject))
                subject = building.Building + " - " + entry.Event;

            string bodyContent = entry.InviteBody;
            if (String.IsNullOrWhiteSpace(bodyContent))
                bodyContent = "";

            bodyContent = bodyContent + Environment.NewLine + Environment.NewLine;

            bodyContent += "Kind Regards" + Environment.NewLine;
            bodyContent += pm.name + Environment.NewLine;
            bodyContent += "Tel: 011 867 3183" + Environment.NewLine;
            bodyContent += "Fax: 011 867 3163" + Environment.NewLine;
            bodyContent += "Direct Fax: 086 657 6199" + Environment.NewLine;
            bodyContent += "BEE Level 4 Contributor" + Environment.NewLine;

            bodyContent += "FOR AND ON BEHALF OF ASTRODON(PTY) LTD" + Environment.NewLine;
            bodyContent += "The information contained in this communication is confidential and may be legally privileged.It is intended solely for the use of the individual or entity to whom it is addressed and others authorized to receive it.If you are not the intended recipient you are hereby notified that any disclosure, copying, distribution or taking action in reliance of the contents of this information is strictly prohibited and may be unlawful.The company is neither liable for proper, complete transmission of the information contained in this communication nor any delay in its receipt." + Environment.NewLine;

            string bccEmail = entry.BCCEmailAddress;
            if (bccEmail == string.Empty)
                bccEmail = null;

            //add aditional attachments
            var attach = _Context.CalendarEntryAttachmentSet.Where(a => a.BuildingCalendarEntryId == entry.id).ToList();
            foreach (var a in attach)
            {
                attachments.Add(a.FileName, a.FileData);
            }

            List<string> toAddress = new List<string>();
            toAddress.Add(pm.email);


            if (!Mailer.SendMailWithAttachments("noreply@astrodon.co.za", toAddress.Distinct().ToArray(),
                subject, bodyContent,
                false, false, false, out status, attachments, bccEmail))
            {
                Console.WriteLine("Error seding email " + status, "Email error");
            }

            //disabled until tested 100%
            /*
            if (entry.NotifyTrustees && entry.CalendarEntryType == CalendarEntryType.Financial)
            {
                var customers = Controller.pastel.AddCustomers(entry.BuildingAbreviation, entry.BuildingDataPath);
                var trustees = customers.Where(a => a.IsTrustee).ToList();
                if (trustees.Count() > 0 && Controller.AskQuestion("Are you sure you want to send the invite to " + trustees.Count().ToString() + " trustees?"))
                {
                    foreach (var trustee in trustees)
                    {
                        if (trustee.Email != null && trustee.Email.Length > 0)
                        {
                            if (!Mailer.SendMailWithAttachments("noreply@astrodon.co.za", trustee.Email,
                                 subject, bodyContent, false, false, false, out status, attachments, bccEmail))
                            {
                                Controller.HandleError("Error seding email " + status, "Email error");
                            }
                        }
                    }

                    var itm = context.BuildingCalendarEntrySet.Single(a => a.id == entry.Id);
                    itm.TrusteesNotified = true;
                    entry.TrusteesNotified = true;
                    context.SaveChanges();
                    BindDataGrid();
                }
            }*/
        }

        private void DoRandomAllocations(DateTime dtStart)
        {
            var dt = new DateTime(dtStart.Year, dtStart.Month, 1);
            var context = _Context;

            //create a list of building id and user id
            var userIdList = context.tblUsers
                                    .Where(a => a.ProcessCheckLists == true && a.Active == true)
                                    .Select(a => a.id).ToList();

            var buildingCodeList = context.tblBuildings.Where(a => a.BuildingFinancialsEnabled == true && a.BuildingDisabled == false).Select(a => a.Code).ToList();

            Dictionary<string, int> randomAllocations = new Dictionary<string, int>();

            //load the current allocations
            foreach (var itm in context.tblMonthFins.Where(a => a.findate == dt)
                                                   .Select(a => new { a.buildingID, a.userID }).ToList())
            {
                randomAllocations.Add(itm.buildingID, itm.userID);
                if (buildingCodeList.Contains(itm.buildingID))
                    buildingCodeList.Remove(itm.buildingID);
            }

            //get last months allocations
            Dictionary<string, int> lastMonthsAllocations = new Dictionary<string, int>();
            var lastMonth = dt.AddMonths(-1);
            foreach (var itm in context.tblMonthFins.Where(a => a.findate == lastMonth)
                                                   .Select(a => new { a.buildingID, a.userID }).ToList())
            {
                lastMonthsAllocations.Add(itm.buildingID, itm.userID);
            }

            var userProcesslist = userIdList.ToList();
            var random = new Random();
            //bublle through buildings to build random dictionary
            #region Random Allocations
            while (buildingCodeList.Count > 0)
            {
                int userId = 0;
                var buildingCode = buildingCodeList[0];

                if (userProcesslist.Count > 1)
                {
                    var idx = random.Next(0, userProcesslist.Count);

                    userId = userProcesslist[idx];
                    userProcesslist.Remove(userId);
                }
                else
                {
                    if (userProcesslist.Count == 1)
                    {
                        userId = userProcesslist[0];
                        userProcesslist.Remove(userId);
                    }
                }

                if (userProcesslist.Count < 1)
                    userProcesslist = userIdList.ToList();

                if (userId > 0)
                {

                    if (lastMonthsAllocations.ContainsKey(buildingCode) && lastMonthsAllocations[buildingCode] == userId)
                    {
                        if (userProcesslist.Count > 1)
                        {
                            var idx = random.Next(0, userProcesslist.Count);
                            int newUserId = userProcesslist[idx];
                            userProcesslist.Remove(newUserId);
                            userProcesslist.Add(userId);
                            if (newUserId > 0)
                            {
                                randomAllocations.Add(buildingCode, newUserId);
                                buildingCodeList.Remove(buildingCode);
                            }
                        }
                    }
                    else
                    {
                        if (userId > 0)
                        {
                            randomAllocations.Add(buildingCode, userId);
                            buildingCodeList.Remove(buildingCode);
                        }
                    }
                }
            }


            #endregion

            //Process dictionary to tblMonthFin

            foreach (var key in randomAllocations.Keys)
            {
                var curr = context.tblMonthFins.Where(a => a.buildingID == key && a.findate == dt).SingleOrDefault();
                if (curr == null)
                {
                    curr = new Data.tblMonthFin()
                    {
                        buildingID = key,
                        findate = dt,
                        finPeriod = dt.Month,
                        year = dt.Year
                    };
                    context.tblMonthFins.Add(curr);
                }
                curr.userID = randomAllocations[key];
            }

            context.SaveChanges();

        }

        private DateTime ClosestWorkDay(DateTime dt)
        {
            var dtStart = dt.AddDays(-30);
            var dtEnd = dt.AddDays(30);

            var holidayList = _Context.PublicHolidaySet.Where(a => a.Date >= dtStart && a.Date <= dtEnd).Select(a => a.Date);


            bool isValidStart = false;
            bool isValidEnd = false;
            DateTime dtLess = dt;
            isValidStart = holidayList.Contains(dtLess) == false && dtLess.DayOfWeek != DayOfWeek.Saturday && dtLess.DayOfWeek != DayOfWeek.Sunday;
            if (isValidStart)
                return dt;
            while (!isValidStart)
            {
                dtLess = dtLess.AddDays(-1); //go backwards
                isValidStart = holidayList.Contains(dtLess) == false && dtLess.DayOfWeek != DayOfWeek.Saturday && dtLess.DayOfWeek != DayOfWeek.Sunday;
            }

            DateTime dtMore = dt;
            isValidEnd = holidayList.Contains(dtLess) == false && dtLess.DayOfWeek != DayOfWeek.Saturday && dtLess.DayOfWeek != DayOfWeek.Sunday;
            while (!isValidEnd)
            {
                dtLess = dtLess.AddDays(1); //go forwards
                isValidEnd = holidayList.Contains(dtLess) == false && dtLess.DayOfWeek != DayOfWeek.Saturday && dtLess.DayOfWeek != DayOfWeek.Sunday;
            }

            var ts1 = Math.Abs((dt - dtLess).TotalDays);

            var ts2 = Math.Abs((dt - dtMore).TotalDays);

            if (ts1 < ts2)
                return dtLess;
            else
                return dtMore;

        }
    }
}

using Astrodon.Data;
using Astrodon.Reports.AllocationWorksheet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Astrodon.DataProcessor
{
    public class ProcessorThread
    {
        private static String connStringDefault = "Data Source=SERVER-SQL;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=@str0d0n"; //Astrodon
        private static String connStringL = "Data Source=STEPHEN-PC\\MTDNDSQL;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=m3t@p@$$"; //Local

      

        private static String connStringD = "Data Source=DEVELOPERPC\\SQLEXPRESS;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=$DEVELOPER$"; //Astrodon
        private static String connStringLocal = "Data Source=.;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=1q2w#E$R"; //LamaDev

        private DateTime _NextRun = DateTime.Now.AddMinutes(1);
        private DateTime _NextSchedule = DateTime.Today;
        

        public ProcessorThread()
        {
            _NextRun = DateTime.Now.AddMinutes(1);// new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0).AddHours(1);
            if (System.Diagnostics.Debugger.IsAttached)
                _NextRun = DateTime.Now.AddSeconds(5);
            Terminated = false;
            new Thread(Run).Start();
        }

        public bool Terminated { get; set; }


        public void Run()
        {
            //run tomorrow the new schedule
            _NextSchedule = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 04, 30, 00).AddDays(1);
            while (!Terminated)
            {
                try
                {
                    if (DateTime.Now > _NextRun)
                    {
                        _NextRun = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0).AddHours(1);

                        ProcessBuildingMatches();
                        DataContext.HouseKeepSystemLog(GetConnectionString());
                    }

                }
                catch (Exception e)
                {
                    LogException(e, "ProcessWorkSchedule");
                }

                if(DateTime.Now > _NextSchedule)
                {
                    _NextSchedule = _NextSchedule.AddDays(1);
                    try
                    {
                        ProcessWorkSchedule();
                    }
                    catch (Exception e)
                    {
                        LogException(e, "ProcessWorkSchedule");
                    }
                    try
                    {
                        ScheduleFinancialMeetings();
                    }
                    catch (Exception e)
                    {
                        LogException(e, "ScheduleFinancialMeetings");
                    }

                    try
                    {
                        ProcessBirthdaySMS();
                    }
                    catch (Exception e)
                    {
                        LogException(e, "ScheduleFinancialMeetings");
                    }
                }

                Thread.Sleep(1000);
            }
        }

    
        private void LogException(Exception e, string customMessage = null)
        {
            string error = string.Empty;
            if (!String.IsNullOrWhiteSpace(customMessage))
                error = customMessage + "=>" + e.Message;
            else
                error = e.Message;
            using (var context = new DataContext(GetConnectionString()))
            {
                context.SystemLogSet.Add(new Data.Log.SystemLog()
                {
                    EventTime = DateTime.Now,
                    Message = error,
                    StackTrace = e.StackTrace
                });
                context.SaveChanges();
            }
        }

        private static string GetConnectionString()
        {
            if (Environment.MachineName == "STEPHEN-PC")
            {
                return connStringL;
            }
            else if (Environment.MachineName == "DEVELOPERPC")
            {
                return connStringD;
            }
            else if (Environment.MachineName == "PASTELPARTNER")
                return connStringLocal;
            return connStringDefault;
        }

        private void ProcessBuildingMatches()
        {
            List<int> buildingList;
            using (var context = new DataContext(GetConnectionString()))
            {
                buildingList = context.tblBuildings.Where(a => a.BuildingDisabled == false).Select(a => a.id).ToList();
            }

            foreach (var buildingId in buildingList)
            {
                if (Terminated)
                    return;
                try
                {
                    using (var context = new DataContext(GetConnectionString()))
                    {
                        var processor = new RequisitionProcessor(context, buildingId);
                        var linked = processor.LinkPayments();
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    LogException(e,"Link Payments buildingId:" + buildingId.ToString());
                }
            }
        }

        public static void ProcessWorkSchedule()
        {
            using (var dc = new DataContext(GetConnectionString()))
            {
                var rp = new AllocationWorksheetReport(dc);
              
                rp.EmailAllocations();
            }
        }

        public static void ScheduleFinancialMeetings()
        {
            using (var dc = new DataContext(GetConnectionString()))
            {
                var rp = new FixedFinancialMeetingsProcessor(dc);

                rp.ScheduleMeetings();
            }
        }

        public static void ProcessBirthdaySMS()
        {
            using (var dc = new DataContext(GetConnectionString()))
            {
                var rp = new BirthdayProcessor(dc);

                rp.Process();
            }
        }

    }
}

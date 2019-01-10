using Astrodon;
using Astrodon.DataProcessor;
using PastelReportServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SelfHosted
{
    class Program
    {
        private static string _connectionString = "Data Source=.;Initial Catalog=Astrodon;Persist Security Info=True;User ID=sa;Password=1q2w#E$R";

        static void Main(string[] args)
        {
            //  ProcessorThread.ProcessWorkSchedule();
            // ProcessorThread.ProcessBirthdaySMS();
            //ProcessorThread.ScheduleFinancialMeetings();

        //   ProcessorThread.TestBuildingODBCConnections();
        //    return;

            ReportServiceHost host = new ReportServiceHost();
            Console.WriteLine("Service is running");

            //var processorThread = new ProcessorThread();
            //Console.WriteLine("ProcessorThread is running");

            //Console.ReadLine();
            //host.Terminated = true;
        }

        private static void TestMissingMaintenance()
        {
            var repService = new PastelDataService.ReportService();
            repService.MissingMaintenanceRecordsGet(_connectionString, 134);
        }
    }
}

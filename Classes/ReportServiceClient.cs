using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Astrodon.ReportService
{
    public partial class ReportServiceClient : System.ServiceModel.ClientBase<Astrodon.ReportService.IReportService>, Astrodon.ReportService.IReportService
    {
        public static ReportServiceClient CreateInstance()
        {
            string url = "10.0.1.10";
            if (Environment.MachineName == "STEPHEN-PC")
            {
                url = "Localhost";
            }
            else if (Environment.MachineName == "DEVELOPERPC")
            {
                url = "Localhost";
            }
            else if (Environment.MachineName == "PASTELPARTNER")
                url = "Localhost";


            return new ReportServiceClient("BasicHttpBinding_IReportService", "http://" + url + ":8080");
        }
    }
}

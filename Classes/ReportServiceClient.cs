using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
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

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.CloseTimeout = new TimeSpan(00, 05, 00);
            binding.OpenTimeout = new TimeSpan(00, 05, 00);
            binding.ReceiveTimeout = new TimeSpan(00, 05, 00);
            binding.SendTimeout = new TimeSpan(00, 05, 00);
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            //"BasicHttpBinding_IReportService"
            return new ReportServiceClient(binding, new EndpointAddress("http://" + url + ":8080"));
        }
    }
}

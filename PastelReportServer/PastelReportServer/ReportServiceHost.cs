using PastelDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;

namespace PastelReportServer
{
    public class ReportServiceHost
    {
        public ReportServiceHost()
        {
            Terminated = false;
            Thread t = new Thread(Run);
            t.Start();
        }
        private void Run()
        {
            Uri baseAddress = new Uri("http://localhost:8080");



            using (ServiceHost host = new ServiceHost(typeof(ReportService), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                // Add MEX endpoint
                host.AddServiceEndpoint(
                  ServiceMetadataBehavior.MexContractName,
                  MetadataExchangeBindings.CreateMexHttpBinding(),
                  "mex"
                );

                // Add application endpoint
                host.AddServiceEndpoint(typeof(IReportService), new BasicHttpBinding(BasicHttpSecurityMode.None), "");

                host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
                host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();
                //foreach(var dispatcher in host.ChannelDispatchers)
                //{
                //    var icl = dispatcher.Listener;
                //    var disp = new ChannelDispatcher(icl);
                //    disp.IncludeExceptionDetailInFaults = true;
                //}


                while (!Terminated)
                {
                    Thread.Sleep(1000);
                }

                // Close the ServiceHost.
                host.Close();
            }
        }

        public bool Terminated { get; set; }
    }
}

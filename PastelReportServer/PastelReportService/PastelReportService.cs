using Astrodon.DataProcessor;
using PastelReportServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PastelReportService
{
    public partial class PastelReportService : ServiceBase
    {
        private ReportServiceHost _service;
        private ProcessorThread _ProcessorThread;
        public PastelReportService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _service = new ReportServiceHost();
            _ProcessorThread = new ProcessorThread();
        }

        protected override void OnStop()
        {
            _service.Terminated = true;
            _ProcessorThread.Terminated = true;
        }
    }
}

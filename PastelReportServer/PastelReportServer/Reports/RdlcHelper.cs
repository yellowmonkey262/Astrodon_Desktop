using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace Astrodon.Reports
{
    public class RdlcHelper : IDisposable
    {
        #region Private Fields

        private Dictionary<string, IEnumerable> _DataSets;
        private Dictionary<string, string> _Parameters;

        private readonly string _reportName;
        private ReportViewer _reportViewer;
        private readonly List<StreamReader> _streams = new List<StreamReader>();

        #endregion

        #region Constructors

        public RdlcHelper(string reportName,
            Dictionary<string, IEnumerable> reportData,
            Dictionary<string, string> parameters,
            bool reportNameIsFileOnDisk = false)
        {
            if (string.IsNullOrEmpty(reportName))
                throw new ArgumentNullException("reportName");

            _reportName = reportName;
            _DataSets = reportData;
            _Parameters = parameters;
            _reportViewer = new ReportViewer();

            _reportViewer.ProcessingMode = ProcessingMode.Remote;
            if (!reportNameIsFileOnDisk)
                _reportViewer.LocalReport.ReportEmbeddedResource = _reportName;
            else
                _reportViewer.LocalReport.ReportPath = _reportName;


            _reportViewer.LocalReport.DisplayName = Path.GetFileNameWithoutExtension(_reportName);

            foreach (string key in _DataSets.Keys)
            {
                _reportViewer.LocalReport.DataSources.Add(new ReportDataSource(key, _DataSets[key]));
            }

            //var temp = _reportViewer.LocalReport.GetParameters();
            foreach (string key in _Parameters.Keys)
            {
                _reportViewer.LocalReport.SetParameters(new ReportParameter(key, _Parameters[key]));
            }
        }

        #endregion

        #region Public Methods

        public void AddDataSet(string dataSetName, IEnumerable dataSource)
        {
            _reportViewer.LocalReport.DataSources.Add(new ReportDataSource(dataSetName, dataSource));
        }

        public ReportViewer GetReport()
        {
            return _reportViewer;
        }

        public byte[] GetReportAsFile()
        {
            _reportViewer.LocalReport.Refresh();
            string mimeType, encoding, fileExtension; Warning[] warnings; string[] streams;
            byte[] file = _reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileExtension, out streams, out warnings);
            return file;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {

            foreach (var stream in _streams)
            {
                try
                {
                    stream.DiscardBufferedData();
                }
                catch { }
                stream.Close();
                stream.Dispose();
            }

            _streams.Clear();

            if (_reportViewer != null)
            {
                if (_reportViewer.LocalReport != null)
                    _reportViewer.LocalReport.Dispose();

                try
                {
                    _reportViewer.Dispose();
                }
                catch
                {
                    // Empty catch here to get unit tests to execute. Dispose fails when called from non-web context.
                }

                _reportViewer = null;
            }
        }

        #endregion

        public LocalReport Report
        {
            get
            {
                return _reportViewer.LocalReport;
            }
        }
    }
}

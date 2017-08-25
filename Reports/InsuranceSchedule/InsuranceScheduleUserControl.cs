using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Desktop.Lib.Pervasive;
using System.Data.Odbc;
using System.Diagnostics;
using System.Collections;
using System.IO;
using Astrodon.ReportService;
using Astro.Library.Entities;
using Astrodon.Data.Base;
using Astrodon.Classes;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Astrodon.Reports
{
    public partial class InsuranceScheduleUserControl : UserControl
    {
        private List<Building> _Buildings;

        private SqlDataHandler dh = new SqlDataHandler();

        public InsuranceScheduleUserControl()
        {
            InitializeComponent();
            LoadBuildings();
        }



        private void LoadBuildings()
        {
            var userid = Controller.user.id;
            Buildings bManager = (userid == 0 ? new Buildings(false) : new Buildings(userid));

            _Buildings = bManager.buildings;
            cmbBuilding.DataSource = _Buildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                CreateReport();
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private byte[] CreateReport()
        {
            bool processedOk = true;
            byte[] reportData = null;
            try
            {
                var building = cmbBuilding.SelectedItem as Building;
                using (var reportService = ReportServiceClient.CreateInstance())
                {
                    reportData = reportService.InsuranceSchedule(SqlDataHandler.GetConnectionString(), building.ID);
                    if (reportData != null)
                    {
                        if (dlgSave.ShowDialog() == DialogResult.OK)
                        {
                            using (FileStream ms = new FileStream(dlgSave.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                using (Document doc = new Document())
                                {
                                    using (PdfCopy copy = new PdfCopy(doc, ms))
                                    {
                                        doc.Open();

                                        AddPdfDocument(copy, reportData);

                                        //write output file
                                        ms.Flush();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cover page failed to generate.");
                    }
                }
            }
            catch (Exception exp)
            {
                processedOk = false;
                Controller.HandleError(exp);
            }

            if (processedOk == false)
                return null;

            return reportData;
        }

        private void AddPdfDocument(PdfCopy copy, byte[] document)
        {
            PdfReader.unethicalreading = true;
            using (PdfReader reader = new PdfReader(document))
            {
                PdfReader.unethicalreading = true;
                int n = reader.NumberOfPages;
                for (int page = 0; page < n;)
                {
                    copy.AddPage(copy.GetImportedPage(reader, ++page));
                }
            }
        }

    }
}
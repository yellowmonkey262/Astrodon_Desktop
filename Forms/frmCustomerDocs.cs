using Astro.Library.Entities;
using Astrodon.ClientPortal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Astrodon.Forms
{
    public partial class frmCustomerDocs : Form
    {
        private BindingList<CustomerDocument> docs = new BindingList<CustomerDocument>();
        private Building building;
        private AstrodonClientPortal _ClientPortal = new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString());

        public frmCustomerDocs(Building Building)
        {
            building = Building;
            InitializeComponent();
        }

        private void frmCustomerDocs_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            String status;
            try
            {

                var docs = _ClientPortal.GetBuildingUnitFiles(building.ID);

                var customerDocuments = docs.Select(a => new CustomerDocument()
                {
                    Select = false,
                    Customer = a.AccountNumber,
                    Title = a.Title,
                    FileName = a.File,
                    Upload_Date = a.DocumentDate,
                    FileID = a.Id
                }).OrderByDescending(a => a.Upload_Date).ToList();
                dgDocs.DataSource = customerDocuments;
               
            }
            catch (Exception ex)
            {
                Controller.HandleError(ex);
            }
            this.Cursor = Cursors.Arrow;
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<String> myFiles = new List<string>();
            List<Guid> myIDS = new List<Guid>();
            foreach (CustomerDocument cd in docs)
            {
                if (cd.Select)
                {
                    myFiles.Add(cd.FileName);
                    myIDS.Add(cd.FileID);
                }
            }
            _ClientPortal.MarkUnitFilesInactive(myIDS);
        }

        private void TransferFiles(List<String> myFiles)
        {
          /*  String status;
            String buildPath = "Y:\\Buildings Managed\\" + building.Name;
            if (!Directory.Exists(buildPath)) { try { Directory.CreateDirectory(buildPath); } catch (Exception ex) { Controller.HandleError(ex); } }
            Classes.Sftp transferClient = new Classes.Sftp(String.Empty, true);
            foreach (String remoteFile in myFiles)
            {
                String fileName = remoteFile;
                String localPath = Path.Combine(buildPath, fileName);
                transferClient.Download(localPath, transferClient.WorkingDirectory + "//" + remoteFile, false, out status);
                transferClient.DeleteFile(transferClient.WorkingDirectory + "//" + remoteFile, false);
            }*/
        }

        private void btnPurge_Click(object sender, EventArgs e)
        {
            List<String> myFiles = new List<string>();
            List<Guid> myIDS = new List<Guid>();
            DateTime checkDate = DateTime.Now.AddMonths(-16);
            foreach (CustomerDocument cd in docs)
            {
                if (cd.Upload_Date <= checkDate)
                {
                    myFiles.Add(cd.FileName);
                    myIDS.Add(cd.FileID);
                }
            }
            _ClientPortal.MarkUnitFilesInactive(myIDS);
        }

        private void dgDocs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                try
                {
                    var doc = dgDocs.Rows[e.RowIndex].DataBoundItem as CustomerDocument;
                    byte[] fileData = _ClientPortal.GetUnitFile(doc.FileID);

                    String localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dgDocs.Rows[e.RowIndex].Cells[3].Value.ToString());

                    if (File.Exists(localPath))
                        File.Delete(localPath);

                    File.WriteAllBytes(localPath, fileData);

                    System.Diagnostics.Process.Start(localPath);
                }
                catch (Exception ex)
                {
                    Controller.HandleError(ex);
                }
            }
        }
    }
}
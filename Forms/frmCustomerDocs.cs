using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Astrodon.Forms {

    public partial class frmCustomerDocs : Form {
        private MySqlConnector mySql = new MySqlConnector();
        private BindingList<CustomerDocument> docs = new BindingList<CustomerDocument>();
        private Building building;

        public frmCustomerDocs(Building Building) {
            building = Building;
            InitializeComponent();
        }

        private void frmCustomerDocs_Load(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            String status;
            try {
                DataSet dsFiles = mySql.GetCustomerDocs(building.Name, out status);
                if (dsFiles != null && dsFiles.Tables.Count > 0 && dsFiles.Tables[0].Rows.Count > 0) {
                    foreach (DataRow drFile in dsFiles.Tables[0].Rows) {
                        CustomerDocument cd = new CustomerDocument();
                        cd.Select = false;
                        cd.FileID = drFile["uid"].ToString();
                        cd.Customer = drFile["unitno"].ToString();
                        cd.Title = drFile["title"].ToString();
                        cd.FileName = drFile["file"].ToString();
                        cd.Upload_Date = UnixTimeStampToDateTime(double.Parse(drFile["tstamp"].ToString()));
                        docs.Add(cd);
                    }
                    dgDocs.DataSource = docs;
                } else {
                    MessageBox.Show(status);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            this.Cursor = Cursors.Arrow;
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            List<String> myFiles = new List<string>();
            List<String> myIDS = new List<string>();
            foreach (CustomerDocument cd in docs) {
                if (cd.Select) {
                    myFiles.Add(cd.FileName);
                    myIDS.Add(cd.FileID);
                }
            }
            TransferFiles(myFiles);
            String query = "DELETE FROM tx_astro_docs WHERE uid in (";
            query += String.Join(",", myIDS.ToArray());
            query += ")";
            String status;
            mySql.SetData(query, null, out status);
        }

        private void TransferFiles(List<String> myFiles) {
            String status;
            String buildPath = "Y:\\Buildings Managed\\" + building.Name;
            if (!Directory.Exists(buildPath)) { try { Directory.CreateDirectory(buildPath); } catch { } }
            Classes.Sftp transferClient = new Classes.Sftp(String.Empty, true);
            foreach (String remoteFile in myFiles) {
                String fileName = remoteFile;
                String localPath = Path.Combine(buildPath, fileName);
                transferClient.Download(localPath, transferClient.WorkingDirectory + "//" + remoteFile, false, out status);
                transferClient.DeleteFile(transferClient.WorkingDirectory + "//" + remoteFile, false);
            }
        }

        private void btnPurge_Click(object sender, EventArgs e) {
            List<String> myFiles = new List<string>();
            List<String> myIDS = new List<string>();
            DateTime checkDate = DateTime.Now.AddMonths(-16);
            foreach (CustomerDocument cd in docs) {
                if (cd.Upload_Date <= checkDate) {
                    myFiles.Add(cd.FileName);
                    myIDS.Add(cd.FileID);
                }
            }
            TransferFiles(myFiles);
            String query = "DELETE FROM tx_astro_docs WHERE uid in (";
            query += String.Join(",", myIDS.ToArray());
            query += ")";
            String status;
            mySql.SetData(query, null, out status);
        }

        private void dgDocs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.ColumnIndex == 3 && e.RowIndex >= 0) {
                try {
                    String localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dgDocs.Rows[e.RowIndex].Cells[3].Value.ToString());
                    Classes.Sftp ftpClient = new Classes.Sftp(String.Empty, true);
                    String status;
                    ftpClient.Download(localPath, ftpClient.WorkingDirectory + "//" + dgDocs.Rows[e.RowIndex].Cells[3].Value.ToString(), false, out status);
                    System.Diagnostics.Process.Start(localPath);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }

    public class CustomerDocument {
        public bool Select { get; set; }

        public String FileID { get; set; }

        public String Customer { get; set; }

        public String Title { get; set; }

        public DateTime Upload_Date { get; set; }

        public String FileName { get; set; }
    }
}
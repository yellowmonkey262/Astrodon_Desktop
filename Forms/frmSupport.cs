using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Astrodon.Forms
{
    public partial class frmSupport : Form
    {
        private List<Attachments> myAttachments;
        public int outFileID;

        public frmSupport(List<Attachments> attachments)
        {
            myAttachments = attachments;
            InitializeComponent();
        }

        private void dgSupportDocs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int attachmentType = 1;
            String fileName = dgSupportDocs[2, e.RowIndex].Value.ToString();
            byte[] file = GetAttachment(fileName, attachmentType, out outFileID);
            if (e.ColumnIndex == 0)
            {
                OpenInAnotherApp(file, fileName);
            }
            else if (e.ColumnIndex == 1)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void OpenInAnotherApp(byte[] data, string filename)
        {
            if (data != null)
            {
                String tempFolder = System.IO.Path.GetTempPath();
                filename = System.IO.Path.Combine(tempFolder, filename);
                File.WriteAllBytes(filename, data);
            }
            System.Diagnostics.Process.Start(filename);
        }

        private byte[] GetAttachment(String fileName, int attachmentType, out int fileID)
        {
            String fileQuery = "SELECT * FROM tblAttachments WHERE fileName = '" + fileName + "' AND attachmentType = " + attachmentType.ToString();
            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
            String status;
            SqlDataHandler dataHandler = new SqlDataHandler();
            sqlParms.Add("@fileName", fileName);
            sqlParms.Add("@at", attachmentType);
            DataSet dsAttachment = dataHandler.GetData(fileQuery, sqlParms, out status);
            if (dsAttachment != null && dsAttachment.Tables.Count > 0 && dsAttachment.Tables[0].Rows.Count > 0)
            {
                byte[] file = (byte[])dsAttachment.Tables[0].Rows[0]["fileContent"];
                fileID = int.Parse(dsAttachment.Tables[0].Rows[0]["id"].ToString());
                return file;
            }
            else
            {
                fileID = 0;
                return null;
            }
        }

        private void frmSupport_Load(object sender, EventArgs e)
        {
            dgSupportDocs.DataSource = myAttachments;
            outFileID = 0;
        }
    }
}
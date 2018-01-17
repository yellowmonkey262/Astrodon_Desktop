using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using System.IO;
using System.Diagnostics;
using Astrodon.Data.ManagementPackData;

namespace Astrodon.Reports.ManagementPack
{
    public partial class ucPublishManagementPack : UserControl
    {
        private List<ManagementPackPreviewItem> _Data;
        private string _Webfolder = "Financial packs";

        public ucPublishManagementPack()
        {
            InitializeComponent();
        }

        private void ucPublishManagementPack_Load(object sender, EventArgs e)
        {
            LoadManagementPacks();
        }

        private void LoadManagementPacks()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                var q = from pack in context.ManagementPackSet
                        where pack.Published == false
                        select new ManagementPackPreviewItem
                        {
                            Id = pack.id,
                            BuildingId = pack.BuildingId,
                            Building = pack.Building.Building,
                            Period = pack.Period,
                            UserCreated = pack.UserCreated.name,
                            UserCreatedEmail = pack.UserCreated.email,
                            Updated = pack.DateUpdated,
                            Comments = pack.Commments
                        };

                _Data = q.OrderBy(a => a.Updated).ToList();
                BindDataGrid();
            }
        }

        private void BindDataGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";

            var periodStyle = new DataGridViewCellStyle();
            periodStyle.Format = "yyyy/MMM";


            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _Data;

            dgItems.Columns.Clear();

            dgItems.DataSource = bs;

            dgItems.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                DataPropertyName = "Processed",
                HeaderText = "",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "View",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 30
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Building",
                HeaderText = "Building",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Period",
                HeaderText = "Period",
                ReadOnly = true,
                DefaultCellStyle = periodStyle,
                MinimumWidth = 80
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UserCreated",
                HeaderText = "User",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Updated",
                HeaderText = "Updated",
                ReadOnly = true,
                DefaultCellStyle = dateColumnStyle,
                MinimumWidth = 80
            });
            dgItems.AutoResizeColumns();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var item = senderGrid.Rows[e.RowIndex].DataBoundItem as ManagementPackPreviewItem;
                if (item != null)
                {
                    DisplayManagementPack(item);
                }
            }
        }

        private ManagementPackPreviewItem _SelectedItem = null;
        private void DisplayManagementPack(ManagementPackPreviewItem item)
        {
            _SelectedItem = item;
            Cursor = Cursors.WaitCursor;
            try
            {
                Application.DoEvents();

                using (var context = SqlDataHandler.GetDataContext())
                {
                    var dataItem = context.ManagementPackSet.Single(a => a.id == _SelectedItem.Id);
                    tbComments.Text = dataItem.Commments;

                    string tempPDFFile = "";

                    tempPDFFile = Path.GetTempPath();
                    if (!tempPDFFile.EndsWith(@"\"))
                        tempPDFFile = tempPDFFile + @"\";

                    tempPDFFile = tempPDFFile + System.Guid.NewGuid().ToString("N") + ".pdf";
                    _SelectedItem.PDFFileName = tempPDFFile;
                    File.WriteAllBytes(_SelectedItem.PDFFileName, dataItem.ReportData);

                    DisplayPDF();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        private void DisplayPDF()
        {
            if (_SelectedItem == null)
            {
                this.axAcroPDF1.Visible = false;
                return;
            }
            try
            {
                this.axAcroPDF1.Visible = true;
                this.axAcroPDF1.LoadFile(_SelectedItem.PDFFileName);
                this.axAcroPDF1.src = _SelectedItem.PDFFileName;
                this.axAcroPDF1.setShowToolbar(false);
                this.axAcroPDF1.setView("FitH");
                this.axAcroPDF1.setLayoutMode("SinglePage");
                this.axAcroPDF1.setShowToolbar(false);

                this.axAcroPDF1.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {

            if (_SelectedItem == null)
            {
                Controller.HandleError("Please select an item from the list.", "Validation Error");
                return;
            }
            if (String.IsNullOrWhiteSpace(tbComments.Text))
            {
                Controller.HandleError("Please provide a reason for declining the report.", "Validation Error");
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                Application.DoEvents();

                using (var context = SqlDataHandler.GetDataContext())
                {
                    var dataItem = context.ManagementPackSet.Single(a => a.id == _SelectedItem.Id);
                    dataItem.Commments = tbComments.Text;
                    _SelectedItem.Comments = tbComments.Text;
                    _SelectedItem.Processed = true;

                    string emailContent = Controller.ReadResourceString("Astrodon.Reports.ManagementPack.ManagementPackDeclined.txt");
                    emailContent = emailContent.Replace("{NAME}", _SelectedItem.UserCreated);
                    emailContent = emailContent.Replace("{BUILDINGNAME}", _SelectedItem.Building);
                    emailContent = emailContent.Replace("{PERIOD}", _SelectedItem.Period.ToString("MMM yyyy"));
                    emailContent = emailContent.Replace("{COMMENTS}", _SelectedItem.Comments);

                    string[] toEmail = { _SelectedItem.UserCreatedEmail };
                    string status;
                    if (!Mailer.SendDirectMail(Controller.user.email, toEmail, "", "", _SelectedItem.Building + "financial pack declined.", emailContent, false, false, out status))
                    {
                        Controller.HandleError("Unable to notify trustees by email : " + status);
                    }

                    context.SaveChanges();
                    BindDataGrid();
                    ClosePDF();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ClosePDF()
        {
            this.axAcroPDF1.Visible = false;
            File.Delete(_SelectedItem.PDFFileName);
            _SelectedItem.PDFFileName = "";
            tbComments.Text = "";
            _SelectedItem = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_SelectedItem == null || String.IsNullOrWhiteSpace(_SelectedItem.PDFFileName))
            {
                Controller.HandleError("Please select an item from the list.", "Validation Error");
                return;
            }

            Process.Start(_SelectedItem.PDFFileName);
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (_SelectedItem == null || String.IsNullOrWhiteSpace(_SelectedItem.PDFFileName))
            {
                Controller.HandleError("Please select an item from the list.", "Validation Error");
                return;
            }

            if (!Controller.AskQuestion("Are you sure you want to publish this management pack?"))
                return;

            using (var context = SqlDataHandler.GetDataContext())
            {
                var dataItem = context.ManagementPackSet.Single(a => a.id == _SelectedItem.Id);
                var building = context.tblBuildings.Single(a => a.id == _SelectedItem.BuildingId);

                string emailContent = Controller.ReadResourceString("Astrodon.Reports.ManagementPack.ManagementPackEmail.txt");

                var customers = Controller.pastel.AddCustomers(building.Code, building.DataPath);
                var trustees = customers.Where(a => a.IsTrustee).ToList();

                emailContent = emailContent.Replace("{MANAGEMENTPACKPERIOD}", _SelectedItem.Period.ToString("MMMM yyyy"));
                emailContent = emailContent.Replace("{BUILDINGNAME}", building.Building);
                emailContent = emailContent.Replace("{PMEMAILADDRESS}", building.pm);

                if (UploadFileToBuilding(building, dataItem))
                {
                    String status = "";
                    if (!Mailer.SendDirectMail(building.pm, new string[] { building.pm }, "", "", "Monthly financial pack", emailContent, false, false, out status))
                    {
                        Controller.HandleError("Unable to notify trustees by email : " + status);
                    }

                    foreach (var trustee in trustees)
                    {
                        if (trustee.Email != null && trustee.Email.Length > 0)
                        {
                            if (!Mailer.SendDirectMail(building.pm, trustee.Email, "", "", "Monthly financial pack", emailContent, false, false, out status))
                            {
                                Controller.HandleError("Unable to notify trustees by email : " + status);
                            }
                        }
                    }
                    _SelectedItem.Processed = true;
                    dataItem.Published = true;
                    context.SaveChanges();
                    BindDataGrid();
                    ClosePDF();
                }
            }
        }

        private bool UploadFileToBuilding(tblBuilding building, Data.ManagementPackData.ManagementPack dataItem)
        {
            try
            {
                bool result = false;

                var web = building.web;
                var ftpClient = new Classes.Sftp(web, false);
                try
                {
                    string workingDirectory = ftpClient.WorkingDirectory;

                    ftpClient.ChangeDirectory(false);

                    string uploadDirectory = workingDirectory + "/" + _Webfolder + "/" + dataItem.Period.Year.ToString() + "/" + dataItem.Period.ToString("MMM") ;
                    string uploadFile = uploadDirectory + "/ManagementPack_" + dataItem.Period.ToString("yyyy_MMM") + ".pdf";

                    ftpClient.ForceDirectory(uploadDirectory, false);



                    if (!ftpClient.ForceUpload(_SelectedItem.PDFFileName, uploadFile, false))
                    {
                        Controller.HandleError("Unable to upload file to FTP server:" + uploadFile);
                        return false;
                    }

                    result = true;
                }
                finally
                {
                    ftpClient.DisconnectClient();
                }
                return result;
            }
            catch (Exception e)
            {
                Controller.HandleError(e);
                return false;
            }
        }
    }

    class ManagementPackPreviewItem
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }

        public string UserCreated { get;  set; }
        public string Building { get;  set; }
        public DateTime Period { get;  set; }
        public DateTime Updated { get;  set; }
        public string Comments { get; set; }
        public bool Processed { get;  set; }
        public string PDFFileName { get; internal set; }
        public string UserCreatedEmail { get; internal set; }
    }
}

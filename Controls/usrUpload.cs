using Astro.Library.Entities;
using Astrodon.ClientPortal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon.Controls
{
    public partial class usrUpload : UserControl
    {
        private List<Building> buildings;
        private String workingDirectory = String.Empty;
        private Building building;
        private String web;

        private String webPic1 = String.Empty;
        private String webPic2 = String.Empty;
        private String copyPic1 = String.Empty;
        private String copyPic2 = String.Empty;

        private Image imgWeb = null;
        private Image imgLocal1 = null;
        private AstrodonClientPortal _ClientPortal = new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString());

        public usrUpload()
        {
            InitializeComponent();
        }

        private void usrUpload_Load(object sender, EventArgs e)
        {
            LoadBuildings();
            picImage.Image = null;
        }

        private void LoadBuildings()
        {
            buildings = new Buildings(false).buildings.OrderBy(c => c.Name).ToList();
            cmbBuilding.SelectedIndexChanged -= cmbBuilding_SelectedIndexChanged;
            cmbBuilding.Items.Add("Please select");
            foreach (Building b in buildings) { cmbBuilding.Items.Add(b.Name); }
            cmbBuilding.SelectedIndex = 0;
            cmbBuilding.SelectedIndexChanged += cmbBuilding_SelectedIndexChanged;
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbFolder.SelectedIndexChanged -= cmbFolder_SelectedIndexChanged;
            if (cmbBuilding.SelectedItem != null && cmbBuilding.SelectedIndex > 0)
            {
                building = buildings[cmbBuilding.SelectedIndex - 1];
                web = building.webFolder;


                var logoData = _ClientPortal.GetBuildingImage(building.ID);
                if (logoData != null)
                {
                    using (var memLogo = new MemoryStream(logoData))
                    {
                        Image img = Image.FromStream(memLogo);
                        picImage.Image = img;
                    }
                }
            }
            cmbFolder.SelectedIndexChanged += cmbFolder_SelectedIndexChanged;
        }

        private void cmbFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListFiles();
        }

        private void ListFiles()
        {
            building = buildings[cmbBuilding.SelectedIndex - 1];

            this.Cursor = Cursors.WaitCursor;
            var files = _ClientPortal.BuildingDocumentList(building.ID);

            fileList.Items.Clear();
            foreach (var itm in files.OrderByDescending(a => a.DocumentDate))
                fileList.Items.Add(itm);

            this.Cursor = Cursors.Arrow;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            building = buildings[cmbBuilding.SelectedIndex - 1];
            List<Guid> deleteMe = new List<Guid>();
            for (int i = 0; i < fileList.Items.Count; i++)
            {
                if (fileList.GetItemChecked(i))
                {
                    deleteMe.Add((fileList.Items[i] as FileDetail).Id);
                }
            }
            _ClientPortal.DeleteBuildingFiles(building.ID,deleteMe);

            ListFiles();
        }

        private void fileList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.fileList.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                building = buildings[cmbBuilding.SelectedIndex - 1];

                var file = (fileList.Items[index] as FileDetail);
                byte[] fileData = _ClientPortal.GetBuildingFile(building.ID, file.Id);

                string fileName = Path.Combine(Path.GetTempPath(), file.File);
                if (File.Exists(fileName))
                    File.Delete(fileName);

                File.WriteAllBytes(fileName, fileData);

                Process.Start(fileName);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                {
                    var fileData = File.ReadAllBytes(ofd.FileName);
                    _ClientPortal.UploadBuildingDocument(DocumentCategoryType.Letter, building.ID, Path.GetFileName(ofd.FileName), Path.GetFileName(ofd.FileName), fileData);
                    ListFiles();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (picImage.Image != null)
            {
                _ClientPortal.DeleteBuildingImage(building.ID);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                String newImage = (imgWeb != imgLocal1 && !String.IsNullOrEmpty(copyPic1) ? Path.GetFileName(copyPic2) : Path.GetFileName(webPic2));
                var result = _ClientPortal.SaveBuildingImage(building.ID, File.ReadAllBytes(newImage));
                using (var mem = new MemoryStream(result))
                {
                    picImage.Image = Image.FromStream(mem);
                }
            }
            catch (Exception ex)
            {
                Controller.HandleError(ex);
            }
        }

        private void picImage_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "";
                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    String sep = String.Empty;
                    foreach (ImageCodecInfo c in codecs)
                    {
                        String codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                        ofd.Filter = String.Format("{0}{1}{2} ({3})|{3}", ofd.Filter, sep, codecName, c.FilenameExtension);
                        sep = "|";
                    }
                    ofd.Filter = String.Format("{0}{1}{2} ({3})|{3}", ofd.Filter, sep, "All Files", "*.*");
                    if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                    {
                        String thisDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        String file1 = Path.GetFileName(ofd.FileName).Replace(Path.GetFileNameWithoutExtension(ofd.FileName), Path.GetFileNameWithoutExtension(ofd.FileName) + "_1");
                        String file2 = file1.Replace("_1", "_2");
                        copyPic1 = Path.Combine(thisDirectory, file1);
                        copyPic2 = Path.Combine(thisDirectory, file2);
                        try
                        {
                            Utilities.ProcessKiller(copyPic1);
                            if (File.Exists(copyPic1)) { File.Delete(copyPic1); }
                            File.Copy(ofd.FileName, copyPic1);
                            imgLocal1 = Image.FromFile(copyPic1);
                        }
                        catch
                        {
                        }
                        try
                        {
                            Utilities.ProcessKiller(copyPic2);
                            if (File.Exists(copyPic2)) { File.Delete(copyPic2); }
                            File.Copy(ofd.FileName, copyPic2);
                        }
                        catch
                        {
                        }
                        picImage.Image = imgLocal1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
        }
    }
}
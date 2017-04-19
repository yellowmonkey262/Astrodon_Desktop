using Astro.Library.Entities;
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
        private Classes.Sftp ftpClient;
        private String workingDirectory = String.Empty;
        private MySqlConnector mysql = new MySqlConnector();
        private Building building;
        private String web;

        private String webPic1 = String.Empty;
        private String webPic2 = String.Empty;
        private String copyPic1 = String.Empty;
        private String copyPic2 = String.Empty;

        private Image imgWeb = null;
        private Image imgLocal1 = null;

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
                String status;
                DataSet dsPic = mysql.GetData("SELECT image FROM tt_content WHERE pid = " + building.pid + " AND CType = 'image'", null, out status);
                if (dsPic != null && dsPic.Tables.Count > 0 && dsPic.Tables[0].Rows.Count > 0)
                {
                    String picture = dsPic.Tables[0].Rows[0]["image"].ToString();
                    Classes.Sftp sftpClient = new Classes.Sftp(String.Empty, true) { WorkingDirectory = "/srv/www/htdocs/uploads/pics" };
                    String thisDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    String file1 = picture.Replace(Path.GetFileNameWithoutExtension(picture), Path.GetFileNameWithoutExtension(picture) + "_web1");
                    String file2 = picture.Replace(Path.GetFileNameWithoutExtension(picture), Path.GetFileNameWithoutExtension(picture) + "_web2");
                    String tempPath = Path.Combine(thisDirectory, file1);
                    bool success = sftpClient.Download(tempPath, picture, false, out status);
                    if (!success)
                    {
                        MessageBox.Show(status);
                    }
                    else
                    {
                        webPic1 = tempPath;
                        webPic2 = Path.Combine(thisDirectory, file2);
                        imgWeb = Image.FromFile(tempPath);
                        picImage.Image = imgWeb;
                    }
                }
                LoadFolders();
            }
            cmbFolder.SelectedIndexChanged += cmbFolder_SelectedIndexChanged;
        }

        private List<String> LoadFolders()
        {
            try
            {
                ftpClient = new Classes.Sftp(web, false);
                workingDirectory = ftpClient.WorkingDirectory;
                List<String> folders = ftpClient.RemoteFolders(false);
                folders.Sort();
                folders.Insert(0, "Root");
                cmbFolder.Items.Clear();
                foreach (String folder in folders) { cmbFolder.Items.Add(folder); }
                return folders;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void cmbFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListFiles();
        }

        private void ListFiles()
        {
            this.Cursor = Cursors.WaitCursor;
            String uploadDirectory = String.Empty;
            if (cmbFolder.SelectedItem != null && cmbFolder.SelectedIndex > 0)
            {
                uploadDirectory = workingDirectory + "//" + cmbFolder.SelectedItem.ToString();
            }
            else
            {
                uploadDirectory = workingDirectory;
            }
            fileList.Items.Clear();
            if (!String.IsNullOrEmpty(uploadDirectory))
            {
                ftpClient.WorkingDirectory = uploadDirectory;
                List<String> files = ftpClient.RemoteFiles(false);
                foreach (String file in files) { fileList.Items.Add(file); }
            }
            this.Cursor = Cursors.Arrow;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<String> deleteMe = new List<string>();
            for (int i = 0; i < fileList.Items.Count; i++)
            {
                if (fileList.GetItemChecked(i))
                {
                    deleteMe.Add(fileList.Items[i].ToString());
                }
            }
            if (deleteMe.Count > 0) { ftpClient.DeleteFile(deleteMe, false); }
            ListFiles();
        }

        private void fileList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.fileList.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                String file = fileList.Items[index].ToString();
                String status;
                try
                {
                    ftpClient.Download(Path.Combine(Path.GetTempPath(), file), ftpClient.WorkingDirectory + "/" + file, false, out status);
                    Process.Start(Path.Combine(Path.GetTempPath(), file));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                {
                    if (ftpClient.Upload(ofd.FileName, ftpClient.WorkingDirectory + "/" + Path.GetFileName(ofd.FileName), false))
                    {
                        ListFiles();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (picImage.Image != null)
            {
                String deleteQuery = "DELETE FROM tt_content WHERE pid = " + building.pid + " AND CType = 'image'";
                String status;
                if (mysql.SetData(deleteQuery, null, out status)) { picImage.Image = null; }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                String newImage = (imgWeb != imgLocal1 && !String.IsNullOrEmpty(copyPic1) ? Path.GetFileName(copyPic2) : Path.GetFileName(webPic2));
                String status;

                Classes.Sftp sftpClient = new Classes.Sftp(String.Empty, true) { WorkingDirectory = "/srv/www/htdocs/uploads/pics" };
                picImage.Image = null;
                if (sftpClient.Upload((imgWeb != imgLocal1 && !String.IsNullOrEmpty(copyPic1) ? copyPic2 : webPic1), newImage, false))
                {
                    DataSet dsPic = mysql.GetData("SELECT image FROM tt_content WHERE pid = " + building.pid + " AND CType = 'image'", null, out status);
                    String query = "";
                    if (dsPic != null && dsPic.Tables.Count > 0 && dsPic.Tables[0].Rows.Count > 0)
                    {
                        query = "UPDATE tt_content SET image = '" + newImage + "' WHERE pid = " + building.pid + " AND CType = 'image'";
                    }
                    else
                    {
                        query = "INSERT INTO tt_content(image, pid, CType) VALUES('" + newImage + "', '" + building.pid + "', 'image')";
                    }
                    picImage.Image = (Image)Image.FromFile(copyPic1).Clone();
                    mysql.SetData(query, null, out status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            if (cmbBuilding.SelectedItem != null && !String.IsNullOrEmpty(txtNewFolder.Text))
            {
                String path = txtNewFolder.Text.Replace("\'", " ");
                if (!LoadFolders().Contains(path))
                {
                    bool success = ftpClient.CreateDirectory(path, false);
                    if (success) { LoadFolders(); }
                }
            }
        }
    }
}
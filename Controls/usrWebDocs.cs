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
using Excel = Microsoft.Office.Interop.Excel;

namespace Astrodon.Controls
{
    public partial class usrWebDocs : UserControl
    {
        private MySqlConnector mysql = new MySqlConnector();
        private ImageList buildingImages = new ImageList();
        private String status;
        private String web;
        private Classes.Sftp ftpClient;
        private Classes.Sftp myClient;
        private Classes.Sftp trusteeClient;
        private String buildDirectory = String.Empty;
        private String webPic1, webPic2, file1, file2;
        private Image imgWeb;
        private int buildingIdx;
        private int selectedIdx;
        private readonly string[] Columns = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH" };
        private String copyPic1, copyPic2;
        private Image imgLocal1;

        private String[] workingDirectories;
        private String[] imageFiles;
        private List<WebBuildingPrint> printBuildings = new List<WebBuildingPrint>();
        private TreeNode selectedNode;

        private List<Building> buildings = new List<Building>();

        public usrWebDocs()
        {
            InitializeComponent();
        }

        private void usrWebDocs_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            LoadEverything();
        }

        private void LoadEverything()
        {
            this.Cursor = Cursors.WaitCursor;
            buildingImages.Images.Clear();
            trvWeb.Nodes.Clear();
            buildings.Clear();
            List<Building> allBuildings = new Buildings(false).buildings;
            if (Controller.user.id == 1) { MessageBox.Show(allBuildings.Count.ToString() + " buildings"); }
            if (Controller.user.id == 1) { MessageBox.Show(Controller.user.buildings.Count.ToString() + " user buildings"); }
            foreach (int bid in Controller.user.buildings)
            {
                foreach (Building b in allBuildings)
                {
                    if (bid == b.ID && !buildings.Contains(b))
                    {
                        buildings.Add(b);
                        break;
                    }
                }
            }
            if (Controller.user.id == 1) { MessageBox.Show(buildings.Count.ToString() + " selected buildings"); }
            buildings = buildings.OrderBy(c => c.Name).ToList();
            workingDirectories = new string[buildings.Count];
            TreeNode rootNode = new TreeNode("Buildings");
            imageFiles = new String[buildings.Count];
            int totalBuildings = 0;
            int wbpBuild = 0;
            for (int i = 0; i < buildings.Count; i++)
            {
                lblBuildings.Text = "Processing: " + buildings[i].Name;
                Application.DoEvents();
                WebBuildingPrint wbp = new WebBuildingPrint();
                TreeNode buildNode = LoadDetails(i, ref wbp);
                if (buildNode.Nodes.Count > 0)
                {
                    buildNode.Nodes.Add(LoadPicture(i, ref wbp));
                    buildNode.Nodes.Add(LoadFiles(i, ref wbp));
                    buildNode.Nodes.Add(LoadTrusteeFiles(i, ref wbp));
                }
                rootNode.Nodes.Add(buildNode);
                totalBuildings++;
                printBuildings.Add(wbp);
            }
            wbpBuild = printBuildings.Count;
            if (Controller.user.id == 1) { MessageBox.Show(wbpBuild.ToString() + " listed buildings"); }
            trvWeb.Nodes.Add(rootNode);
            rootNode.Expand();
            this.Cursor = Cursors.Arrow;
            lblBuildings.Text = "Buildings";
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void ShowImage(String imageFile)
        {
            if (imageFile.ToUpper().Contains("."))
            {
                Classes.Sftp sftpClient = new Classes.Sftp(String.Empty, true);
                sftpClient.WorkingDirectory = "/srv/www/htdocs/uploads/pics";
                String thisDirectory = AppDomain.CurrentDomain.BaseDirectory;
                String tempPath = Path.Combine(thisDirectory, imageFile);
                bool success = sftpClient.Download(tempPath, sftpClient.WorkingDirectory + "//" + imageFile, false, out status);
                picWeb.Image = Image.FromFile(tempPath);
                file2 = imageFile.Replace(Path.GetFileNameWithoutExtension(imageFile), Path.GetFileNameWithoutExtension(imageFile) + "_web2");
                webPic1 = tempPath;
                webPic2 = Path.Combine(thisDirectory, file2);
                imgWeb = Image.FromFile(tempPath);
            }
        }

        private TreeNode LoadPicture(int buildIdx, ref WebBuildingPrint wbp)
        {
            Building building = buildings[buildIdx];
            String imageDate = "";
            String picture = "";
            TreeNode imageNode;
            DataSet dsPic = mysql.GetData("SELECT image, crdate FROM tt_content WHERE pid = " + building.pid + " AND CType = 'image'", null, out status);
            if (dsPic != null && dsPic.Tables.Count > 0 && dsPic.Tables[0].Rows.Count > 0)
            {
                picture = dsPic.Tables[0].Rows[0]["image"].ToString();
                double imageUnixDate = double.Parse(dsPic.Tables[0].Rows[0]["crdate"].ToString());
                DateTime imageSysDate = UnixTimeStampToDateTime(imageUnixDate);
                imageDate = imageSysDate.ToString("yyyy/MM/dd");
                imageFiles[buildIdx] = picture;
                TreeNode imageFileNode = new TreeNode(picture);
                TreeNode imageDateNode = new TreeNode("Web image updated: " + imageDate);
                imageFileNode.Tag = picture;
                wbp.Image = picture;
                wbp.ImageUpload = imageDate;
                imageNode = new TreeNode("Web Image");
                imageNode.Nodes.Add(imageFileNode);
                imageNode.Nodes.Add(imageDateNode);
            }
            else
            {
                imageNode = new TreeNode("No web image");
                wbp.Image = "N/A";
                wbp.ImageUpload = "N/A";
            }
            imageNode.Tag = "image" + buildIdx.ToString();
            return imageNode;
        }

        private TreeNode LoadDetails(int buildIdx, ref WebBuildingPrint wbp)
        {
            Building building = buildings[buildIdx];
            wbp.BuildingName = building.Name;
            String query = "SELECT uid, name, abbr, debtors_email, agent_email, address FROM tx_astro_complex WHERE abbr = '" + building.Abbr + "'";
            DataSet dsBuilding = mysql.GetData(query, null, out status);
            TreeNode buildNode = new TreeNode(building.Name);
            if (dsBuilding != null && dsBuilding.Tables.Count > 0 && dsBuilding.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsBuilding.Tables[0].Rows[0];
                TreeNode buildDataNode = new TreeNode("Info");
                wbp.Debtor = dr["debtors_email"].ToString();
                wbp.PM = dr["agent_email"].ToString();
                wbp.Address = dr["address"].ToString();
                buildDataNode.Nodes.Add("Debtor: " + wbp.Debtor);
                buildDataNode.Nodes.Add("PM: " + wbp.PM);
                buildDataNode.Nodes.Add("Address: " + wbp.Address);
                buildNode.Nodes.Add(buildDataNode);
                workingDirectories[buildIdx] = building.webFolder;
            }
            else
            {
                buildNode = new TreeNode(building.Name + " not on web");
                wbp.Debtor = "N/A";
                wbp.PM = "N/A";
                wbp.Address = "N/A";
            }
            buildNode.Tag = buildIdx;

            return buildNode;
        }

        private TreeNode LoadFiles(int buildIdx, ref WebBuildingPrint wbp)
        {
            Building building = buildings[buildIdx];
            Dictionary<String, List<String>> Files = new Dictionary<string, List<string>>();
            web = building.webFolder;
            ftpClient = new Classes.Sftp(web, false);
            String workingDirectory = ftpClient.WorkingDirectory;
            workingDirectories[buildIdx] = workingDirectory;
            String uploadDirectory = workingDirectory;
            TreeNode rootNode = new TreeNode("Root folder");

            TreeNode rootFileNode = new TreeNode("Files") { Tag = workingDirectory };
            Dictionary<String, String> rootFiles = GetFiles(uploadDirectory);
            List<String> rootFileList = new List<string>();
            foreach (KeyValuePair<String, String> rootFile in rootFiles)
            {
                TreeNode fileNode = new TreeNode(rootFile.Key + ": " + rootFile.Value);
                rootFileList.Add(rootFile.Key + ": " + rootFile.Value);
                fileNode.Tag = uploadDirectory + "//" + rootFile.Key;
                rootFileNode.Nodes.Add(fileNode);
            }
            Files.Add("Root folder", rootFileList);
            rootNode.Nodes.Add(rootFileNode);
            TreeNode rootFolderNode = new TreeNode("Folders") { Tag = uploadDirectory };
            List<String> folders = CleanFolders(ftpClient.RemoteFolders(false), uploadDirectory);
            folders.Sort();
            foreach (String folder in folders)
            {
                List<String> folderFileList = new List<string>();
                TreeNode folderNode = new TreeNode(folder);
                uploadDirectory = workingDirectory + "//" + folder;
                folderNode.Tag = uploadDirectory;
                Dictionary<String, String> folderFiles = GetFiles(uploadDirectory);
                foreach (KeyValuePair<String, String> folderFile in folderFiles)
                {
                    TreeNode fileNode = new TreeNode(folderFile.Key + ": " + folderFile.Value);
                    folderFileList.Add(folderFile.Key + ": " + folderFile.Value);
                    fileNode.Tag = uploadDirectory + "//" + folderFile.Key;
                    folderNode.Nodes.Add(fileNode);
                }
                Files.Add(folder, folderFileList);
                rootFolderNode.Nodes.Add(folderNode);
            }
            rootNode.Nodes.Add(rootFolderNode);
            wbp.Files = Files;
            return rootNode;
        }

        private TreeNode LoadTrusteeFiles(int buildIdx, ref WebBuildingPrint wbp)
        {
            Building building = buildings[buildIdx];
            Dictionary<String, List<String>> Files = new Dictionary<string, List<string>>();
            web = building.webFolder;
            ftpClient = new Classes.Sftp(web, false, true);
            String workingDirectory = ftpClient.WorkingDirectory;
            workingDirectories[buildIdx] = workingDirectory;
            String uploadDirectory = workingDirectory;
            TreeNode rootNode = new TreeNode("Trustee folder");
            TreeNode rootFileNode = new TreeNode("Files") { Tag = workingDirectory };
            Dictionary<String, String> rootFiles = GetFiles(uploadDirectory);
            List<String> rootFileList = new List<string>();
            foreach (KeyValuePair<String, String> rootFile in rootFiles)
            {
                TreeNode fileNode = new TreeNode(rootFile.Key + ": " + rootFile.Value);
                rootFileList.Add(rootFile.Key + ": " + rootFile.Value);
                fileNode.Tag = uploadDirectory + "//" + rootFile.Key;
                rootFileNode.Nodes.Add(fileNode);
            }
            Files.Add("Trustee folder", rootFileList);
            rootNode.Nodes.Add(rootFileNode);
            TreeNode rootFolderNode = new TreeNode("Folders") { Tag = uploadDirectory };
            List<String> folders = CleanFolders(ftpClient.RemoteFolders(false), uploadDirectory);
            folders.Sort();
            foreach (String folder in folders)
            {
                List<String> folderFileList = new List<string>();
                TreeNode folderNode = new TreeNode(folder);
                uploadDirectory = workingDirectory + "//" + folder;
                folderNode.Tag = uploadDirectory;
                Dictionary<String, String> folderFiles = GetFiles(uploadDirectory);
                foreach (KeyValuePair<String, String> folderFile in folderFiles)
                {
                    TreeNode fileNode = new TreeNode(folderFile.Key + ": " + folderFile.Value);
                    folderFileList.Add(folderFile.Key + ": " + folderFile.Value);
                    fileNode.Tag = uploadDirectory + "//" + folderFile.Key;
                    folderNode.Nodes.Add(fileNode);
                }
                Files.Add(folder, folderFileList);
                rootFolderNode.Nodes.Add(folderNode);
            }
            rootNode.Nodes.Add(rootFolderNode);
            wbp.Files = Files;
            return rootNode;
        }

        private List<String> LoadFiles(Building building)
        {
            List<String> files = new List<string>();
            web = building.webFolder;
            ftpClient = new Classes.Sftp(web, false);
            String workingDirectory = ftpClient.WorkingDirectory;
            List<String> remoteRootFiles = ftpClient.RemoteFiles(false);
            foreach (String rootFile in remoteRootFiles) { files.Add(workingDirectory + "//" + rootFile); }
            List<String> folders = ftpClient.RemoteFolders(false);
            foreach (String folder in folders)
            {
                List<String> folderFileList = new List<string>();
                String uploadDirectory = workingDirectory + "//" + folder;
                Dictionary<String, String> folderFiles = GetFiles(uploadDirectory);
                foreach (KeyValuePair<String, String> folderFile in folderFiles) { files.Add(uploadDirectory + "//" + folderFile.Key); }
            }
            return files;
        }

        private List<String> CleanFolders(List<String> folders, String directory)
        {
            List<String> newFolders = new List<string>();
            Classes.Sftp cleanClient = new Classes.Sftp(String.Empty, false);
            String oldPath;
            String newPath;
            String success;
            foreach (String folder in folders)
            {
                switch (folder)
                {
                    case "Insurance_Information":
                    case "Insurance_information":
                    case "Insurance_Policy":
                    case "Insurance Policy":
                        oldPath = directory + "//" + folder;
                        newPath = directory + "//Insurance Information";
                        if (!cleanClient.RenameDirectory(oldPath, newPath, false, out success))
                        {
                            MessageBox.Show(success + " to rename directory " + folder);
                            newFolders.Add(folder);
                        }
                        else
                        {
                            newFolders.Add("Insurance Information");
                        }
                        break;

                    case "Annual_Financial_Statements":
                        oldPath = directory + "//" + folder;
                        newPath = directory + "//Annual Financial Statements";
                        if (!cleanClient.RenameDirectory(oldPath, newPath, false, out success))
                        {
                            MessageBox.Show(success + " to rename directory " + folder);
                            newFolders.Add(folder);
                        }
                        else
                        {
                            newFolders.Add("Annual Financial Statements");
                        }
                        break;

                    case "Conduct_Rules":
                        oldPath = directory + "//" + folder;
                        newPath = directory + "//Conduct Rules";
                        if (!cleanClient.RenameDirectory(oldPath, newPath, false, out success))
                        {
                            MessageBox.Show(success + " to rename directory " + folder);
                            newFolders.Add(folder);
                        }
                        else
                        {
                            newFolders.Add("Conduct Rules");
                        }
                        break;

                    case "Meeting_Minutes":
                        oldPath = directory + "//" + folder;
                        newPath = directory + "//Meeting Minutes";
                        if (!cleanClient.RenameDirectory(oldPath, newPath, false, out success))
                        {
                            MessageBox.Show(success + " to rename directory " + folder);
                            newFolders.Add(folder);
                        }
                        else
                        {
                            newFolders.Add("Meeting Minutes");
                        }
                        break;

                    case "Meeting_Notices":
                        oldPath = directory + "//" + folder;
                        newPath = directory + "//Meeting Notices";
                        if (!cleanClient.RenameDirectory(oldPath, newPath, false, out success))
                        {
                            MessageBox.Show(success + " to rename directory " + folder);
                            newFolders.Add(folder);
                        }
                        else
                        {
                            newFolders.Add("Meeting Notices");
                        }
                        break;

                    case "Sectional_Title_Plans":
                        oldPath = directory + "//" + folder;
                        newPath = directory + "//Sectional Title Plans";
                        if (!cleanClient.RenameDirectory(oldPath, newPath, false, out success))
                        {
                            MessageBox.Show(success + " to rename directory " + folder);
                            newFolders.Add(folder);
                        }
                        else
                        {
                            newFolders.Add("Sectional Title Plans");
                        }
                        break;

                    default:
                        newFolders.Add(folder);
                        break;
                }
            }
            return newFolders;
        }

        private Dictionary<String, String> GetFiles(String directory)
        {
            ftpClient.WorkingDirectory = directory;
            List<String> files = ftpClient.RemoteFiles(false);
            Dictionary<String, String> fileList = new Dictionary<string, string>();
            foreach (String file in files)
            {
                DateTime accessDate = ftpClient.GetAccessDate(directory + "//" + file, false);
                fileList.Add(file, accessDate.ToString("yyyy/MM/dd"));
            }
            return fileList;
        }

        private void trvWeb_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            String here = "";
            try
            {
                TreeNode clickedNode = trvWeb.GetNodeAt(e.X, e.Y);
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    TreeNode tn = e.Node;
                    selectedNode = clickedNode;
                    ContextMenu mnu;
                    try
                    {
                        if (tn.Parent != null && tn.Nodes.Count == 0 && tn.Parent.Text != "Folders")
                        {
                            if (tn.Parent.Text == "Files")
                            {
                                switch (tn.Parent.Text)
                                {
                                    case "Web Image":
                                        mnu = CreateContextMenu(false);
                                        mnu.Show(trvWeb, e.Location);
                                        break;

                                    case "Files":
                                    default:
                                        mnu = CreateContextMenu(true);
                                        mnu.Show(trvWeb, e.Location);
                                        break;
                                }
                            }
                            else if (tn.Parent.Parent != null && tn.Parent.Parent.Text == "Folders")
                            {
                                mnu = CreateContextMenu(true);
                                mnu.Show(trvWeb, e.Location);
                            }
                            else if (tn.Text == "Folders")
                            {
                                mnu = CreateContextMenu("Folders");
                                mnu.Show(trvWeb, e.Location);
                            }
                            else if (tn.Text == "No web image")
                            {
                                mnu = CreateContextMenu("Image");
                                mnu.Show(trvWeb, e.Location);
                            }
                            else if (tn.Parent.Text == "Web Image" || tn.Text == "Web Image")
                            {
                                mnu = CreateContextMenu(false);
                                mnu.Show(trvWeb, e.Location);
                            }
                        }
                        else if (tn.Text == "Files" || tn.Parent.Text == "Folders")
                        {
                            mnu = CreateContextMenu(tn.Text == "Files" ? "root" : "subfolder");
                            mnu.Show(trvWeb, e.Location);
                        }
                        else if (tn.Text == "Folders")
                        {
                            mnu = CreateContextMenu("Folders");
                            mnu.Show(trvWeb, e.Location);
                        }
                        else if (tn.Parent.Text == "Buildings")
                        {
                            mnu = CreateContextMenu("Building");
                            mnu.Show(trvWeb, e.Location);
                        }
                        else if (tn.Text == "No web image")
                        {
                            mnu = CreateContextMenu("Image");
                            mnu.Show(trvWeb, e.Location);
                        }
                    }
                    catch
                    {
                        if (tn.Text == "Buildings")
                        {
                            mnu = CreateContextMenu("new");
                            mnu.Show(trvWeb, e.Location);
                        }
                    }
                }
                else if (clickedNode.Parent.Parent == null)
                {
                    try
                    {
                        int buildIdx = int.Parse(clickedNode.Tag.ToString());
                        try
                        {
                            selectedIdx = buildIdx;
                            String imageFile = imageFiles[buildIdx];
                            ShowImage(imageFile);
                        }
                        catch
                        {
                            picWeb.Image = null;
                        }
                        buildingIdx = selectedIdx;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch
            {
                MessageBox.Show(here);
            }
        }

        private List<String> LoadFolders(int buildIdx, bool trustee)
        {
            try
            {
                cmbFolder.Items.Clear();
                fileList.Items.Clear();
                Application.DoEvents();
                myClient = new Classes.Sftp(buildings[buildIdx].webFolder, false, trustee);
                String workingDirectory = myClient.WorkingDirectory;
                List<String> folders = myClient.RemoteFolders(trustee);
                buildDirectory = workingDirectory;
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

        private ContextMenu CreateContextMenu(bool isFile)
        {
            ContextMenu mnu = new ContextMenu();
            MenuItem mnuShowImage = new MenuItem("Show Image");
            MenuItem mnuReplaceImage = new MenuItem("Replace Image");
            MenuItem mnuDeleteImage = new MenuItem("Delete Image");
            MenuItem mnuShowFile = new MenuItem("Show File");
            MenuItem mnuDeletefile = new MenuItem("Delete File");
            mnuShowImage.Click += mnuShowImage_Click;
            mnuReplaceImage.Click += mnuReplaceImage_Click;
            mnuDeleteImage.Click += mnuDeleteImage_Click;
            mnuShowFile.Click += mnuShowFile_Click;
            mnuDeletefile.Click += mnuDeletefile_Click;
            if (isFile)
            {
                mnu.MenuItems.Add(mnuShowFile);
                mnu.MenuItems.Add(mnuDeletefile);
            }
            else
            {
                mnu.MenuItems.Add(mnuShowImage);
                mnu.MenuItems.Add(mnuReplaceImage);
                mnu.MenuItems.Add(mnuDeleteImage);
            }
            return mnu;
        }

        private ContextMenu CreateContextMenu(String directory)
        {
            ContextMenu mnu = new ContextMenu();
            if (directory == "Folders")
            {
                MenuItem mnuAddFolder = new MenuItem("Add Folder");
                mnuAddFolder.Click += mnuAddFolder_Click;
                mnu.MenuItems.Add(mnuAddFolder);
            }
            else if (directory == "Building")
            {
                MenuItem mnuRemoveBuilding = new MenuItem("Remove Building");
                MenuItem mnuUpdateBuilding = new MenuItem("Update Building");
                MenuItem mnuShowCustomerDocs = new MenuItem("Show Customer Files");
                mnuRemoveBuilding.Click += mnuRemoveBuilding_Click;
                mnuUpdateBuilding.Click += mnuUpdateBuilding_Click;
                mnuShowCustomerDocs.Click += mnuShowCustomerDocs_Click;
                mnu.MenuItems.Add(mnuRemoveBuilding);
                mnu.MenuItems.Add(mnuUpdateBuilding);
                mnu.MenuItems.Add(mnuShowCustomerDocs);
            }
            else if (directory == "root" || directory == "subfolder")
            {
                MenuItem mnuAddFile = new MenuItem("Add File");
                mnuAddFile.Click += mnuAddFile_Click;
                mnu.MenuItems.Add(mnuAddFile);
                if (directory == "subfolder")
                {
                    MenuItem mnuRemoveFolder = new MenuItem("Remove Folder");
                    mnuRemoveFolder.Click += mnuRemoveFolder_Click;
                    mnu.MenuItems.Add(mnuRemoveFolder);
                }
            }
            else if (directory == "new")
            {
                MenuItem mnuNewBuilding = new MenuItem("Add Building");
                mnuNewBuilding.Click += mnuNewBuilding_Click;
                mnu.MenuItems.Add(mnuNewBuilding);
            }
            else if (directory == "Image")
            {
                MenuItem mnuImage = new MenuItem("Add image");
                mnuImage.Click += mnuImage_Click;
                mnu.MenuItems.Add(mnuImage);
            }
            return mnu;
        }

        private void mnuShowCustomerDocs_Click(object sender, EventArgs e)
        {
            Building selectedBuilding = null;
            foreach (Building b in buildings)
            {
                if (b.Name == selectedNode.Text)
                {
                    selectedBuilding = b;
                    break;
                }
            }
            if (selectedBuilding != null)
            {
                using (Forms.frmCustomerDocs fCustDocs = new Forms.frmCustomerDocs(selectedBuilding))
                {
                    fCustDocs.ShowDialog();
                }
            }
        }

        private void mnuImage_Click(object sender, EventArgs e)
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
                        String file = Path.Combine(thisDirectory, Path.GetFileName(ofd.FileName));
                        try
                        {
                            Utilities.ProcessKiller(file);
                            if (File.Exists(file)) { File.Delete(file); }
                            File.Copy(ofd.FileName, file);
                            Classes.Sftp sftpClient = new Classes.Sftp(String.Empty, true);
                            int buildIdx = int.Parse(selectedNode.Parent.Tag.ToString());
                            sftpClient.WorkingDirectory = "/srv/www/htdocs/uploads/pics";
                            sftpClient.ChangeDirectory(false);
                            ftpClient.UploadReplace(file, sftpClient.WorkingDirectory + "/" + Path.GetFileName(file), false);
                            String query = "INSERT INTO tt_content(image, pid, ctype) VALUES(@image, @pid, @ctype)";
                            Dictionary<String, Object> sqlParms = new Dictionary<string, object>();
                            sqlParms.Add("@image", Path.GetFileName(file));
                            sqlParms.Add("@pid", buildings[buildIdx].pid);
                            sqlParms.Add("@ctype", "image");
                            mysql.SetData(query, sqlParms, out status);

                            TreeNode imageFileNode = new TreeNode(Path.GetFileName(file));
                            TreeNode imageDateNode = new TreeNode("Web image updated: " + DateTime.Now.ToString("yyyy/MM/dd"));
                            imageFileNode.Tag = sftpClient.WorkingDirectory + "/" + Path.GetFileName(file);
                            selectedNode.Text = "Web Image";
                            selectedNode.Nodes.Add(imageFileNode);
                            selectedNode.Nodes.Add(imageDateNode);
                            WebBuildingPrint wbp = printBuildings[buildIdx];
                            selectedNode = LoadPicture(buildIdx, ref wbp);
                            printBuildings[buildIdx] = wbp;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error loading image: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuNewBuilding_Click(object sender, EventArgs e)
        {
            using (Forms.frmNewBuilding fNew = new Forms.frmNewBuilding(null))
            {
                if (fNew.ShowDialog() == DialogResult.OK)
                {
                    buildings.Add(fNew.building);
                    Array.Resize(ref workingDirectories, workingDirectories.Length + 1);
                    Array.Resize(ref imageFiles, workingDirectories.Length + 1);
                    int buildIdx = -1;
                    for (int i = 0; i < buildings.Count; i++)
                    {
                        if (buildings[i] == fNew.building)
                        {
                            buildIdx = i;
                            break;
                        }
                    }
                    if (buildIdx > -1)
                    {
                        WebBuildingPrint wbp = new WebBuildingPrint();
                        TreeNode buildNode = LoadDetails(buildIdx, ref wbp);
                        if (buildNode.Nodes.Count > 0)
                        {
                            buildNode.Nodes.Add(LoadPicture(buildIdx, ref wbp));
                            buildNode.Nodes.Add(LoadFiles(buildIdx, ref wbp));
                        }
                        printBuildings.Add(wbp);
                        trvWeb.Nodes[0].Nodes.Add(buildNode);
                    }
                }
            }
        }

        private void mnuUpdateBuilding_Click(object sender, EventArgs e)
        {
            int buildIdx = int.Parse(selectedNode.Tag.ToString());
            Building b = buildings[buildIdx];
            using (Forms.frmNewBuilding fNew = new Forms.frmNewBuilding(b))
            {
                if (fNew.ShowDialog() == DialogResult.OK)
                {
                    WebBuildingPrint wbp = printBuildings[buildIdx];
                    TreeNode buildNode = LoadDetails(buildIdx, ref wbp);
                    if (buildNode.Nodes.Count > 0)
                    {
                        buildNode.Nodes.Add(LoadPicture(buildIdx, ref wbp));
                        buildNode.Nodes.Add(LoadFiles(buildIdx, ref wbp));
                    }
                    printBuildings[buildIdx] = wbp;
                    selectedNode = buildNode;
                }
            }
        }

        private void mnuRemoveBuilding_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm delete " + selectedNode.Text, "Web Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (Forms.frmPrompt passwordForm1 = new Forms.frmPrompt("Confirm", "Please enter password"))
                {
                    using (Forms.frmPrompt passwordForm2 = new Forms.frmPrompt("Confirm", "Please confirm password"))
                    {
                        if (passwordForm1.ShowDialog() == DialogResult.OK && passwordForm1.fileName == "sheldon!@#")
                        {
                            if (passwordForm2.ShowDialog() == DialogResult.OK && passwordForm2.fileName == "sheldon!@#")
                            {
                                Classes.Sftp ftpPurgeClient = new Classes.Sftp(String.Empty, true);
                                ftpPurgeClient.ConnectClient(false);
                                Building deleteBuilding = null;
                                foreach (Building b in buildings)
                                {
                                    if (b.Name == selectedNode.Text)
                                    {
                                        deleteBuilding = b;
                                        String mySqlBuildQuery = "SELECT uid, name, abbr, debtors_email, agent_email, address FROM tx_astro_complex WHERE abbr = '" + b.Abbr + "'";
                                        DataSet dsBuilding = mysql.GetData(mySqlBuildQuery, null, out status);
                                        String buildID = "";
                                        if (dsBuilding != null && dsBuilding.Tables.Count > 0 && dsBuilding.Tables[0].Rows.Count > 0)
                                        {
                                            buildID = dsBuilding.Tables[0].Rows[0]["uid"].ToString();
                                            String fileQuery = "SELECT distinct file FROM tx_astro_docs AS d INNER JOIN tx_astro_account_user_mapping AS m";
                                            fileQuery += " ON d.cruser_id = m.cruser_id AND d.unitno = m.account_no WHERE m.complex_id = " + buildID;
                                            DataSet dsFiles = mysql.GetData(fileQuery, null, out status);
                                            if (dsFiles != null && dsFiles.Tables.Count > 0 && dsFiles.Tables[0].Rows.Count > 0)
                                            {
                                                String filePath = ftpPurgeClient.WorkingDirectory + "/";
                                                foreach (DataRow drFile in dsFiles.Tables[0].Rows)
                                                {
                                                    String deleteFile = filePath + drFile["file"].ToString();
                                                    List<String> deletedFiles = new List<string>();
                                                    deletedFiles.Add(deleteFile);
                                                    TransferFiles(true, b, deletedFiles);
                                                    ftpPurgeClient.DeleteFile(filePath + drFile["file"].ToString(), false);
                                                }
                                            }
                                            String deleteDocsQuery = "DELETE d.* FROM tx_astro_docs AS d INNER JOIN tx_astro_account_user_mapping AS m";
                                            deleteDocsQuery += " ON d.cruser_id = m.cruser_id AND d.unitno = m.account_no WHERE m.complex_id = " + buildID;
                                            mysql.SetData(deleteDocsQuery, null, out status);
                                            String deleteClientsQuery = "DELETE FROM tx_astro_account_user_mapping WHERE complex_id = " + buildID;
                                            mysql.SetData(deleteClientsQuery, null, out status);
                                            String clearLoginsQuery = "DELETE FROM fe_users WHERE uid NOT IN (SELECT distinct cruser_id FROM tx_astro_account_user_mapping)";
                                            mysql.SetData(clearLoginsQuery, null, out status);
                                            String deleteImageQuery = "DELETE FROM tt_content WHERE pid = " + b.pid.ToString();
                                            mysql.SetData(deleteImageQuery, null, out status);
                                            String deletePagesQuery = "DELETE FROM pages WHERE uid = " + b.pid.ToString();
                                            mysql.SetData(deletePagesQuery, null, out status);
                                            String buildingQuery = "DELETE FROM tx_astro_complex WHERE uid = " + buildID;
                                            mysql.SetData(buildingQuery, null, out status);
                                            TransferFiles(true, b, null);
                                            Classes.Sftp ftpPurgeBuilding = new Classes.Sftp(null, false);
                                            ftpPurgeBuilding.DeleteDirectory(b.webFolder, false);
                                        }
                                        break;
                                    }
                                }
                                if (deleteBuilding != null)
                                {
                                    String deleteConvertorQuery = "DELETE FROM tblBuildings WHERE id = " + deleteBuilding.ID.ToString();
                                    SqlDataHandler dh = new SqlDataHandler();
                                    dh.SetData(deleteConvertorQuery, null, out status);
                                }
                                trvWeb.Nodes.Remove(selectedNode);
                            }
                        }
                    }
                }
            }
        }

        private void TransferFiles(bool purge, Building b, List<String> myFiles)
        {
            String buildPath = "Y:\\Buildings " + (purge ? "PREVIOUSLY " : "") + "Managed\\" + b.Name;
            if (!Directory.Exists(buildPath)) { try { Directory.CreateDirectory(buildPath); } catch { } }
            List<String> remoteFiles = (myFiles == null ? LoadFiles(b) : myFiles);
            Classes.Sftp transferClient = new Classes.Sftp(String.Empty, false);
            foreach (String remoteFile in remoteFiles)
            {
                int fileStart = remoteFile.LastIndexOf("//", 0) + 2;
                String fileName = remoteFile.Substring(fileStart);
                String localPath = Path.Combine(buildPath, fileName);
                transferClient.Download(localPath, remoteFile, false, out status);
            }
        }

        private void mnuAddFolder_Click(object sender, EventArgs e)
        {
            using (Forms.frmPrompt promptF = new Forms.frmPrompt("Web Maintenance", "Please enter folder name"))
            {
                if (promptF.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(promptF.fileName))
                {
                    String directory = promptF.fileName;
                    ftpClient.WorkingDirectory = selectedNode.Tag.ToString();
                    ftpClient.ChangeDirectory(false);
                    if (ftpClient.CreateDirectory(directory, false))
                    {
                        TreeNode newNode = new TreeNode(promptF.fileName) { Tag = directory };
                        selectedNode.Nodes.Add(newNode);
                    }
                }
            }
        }

        private void mnuRemoveFolder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm delete folder?", "Web Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (ftpClient.DeleteDirectory(selectedNode.Tag.ToString(), false))
                {
                    trvWeb.Nodes.Remove(selectedNode);
                }
            }
        }

        private void mnuAddFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "";
                    if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                    {
                        String thisDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        String file = Path.Combine(thisDirectory, Path.GetFileName(ofd.FileName));
                        try
                        {
                            Utilities.ProcessKiller(file);
                            if (File.Exists(file)) { File.Delete(file); }
                            File.Copy(ofd.FileName, file);
                            ftpClient.UploadReplace(file, selectedNode.Tag.ToString() + "//" + Path.GetFileName(ofd.FileName), false);
                        }
                        catch
                        {
                        }
                        TreeNode tnNew = new TreeNode(Path.GetFileName(ofd.FileName)) { Tag = selectedNode.Tag.ToString() + "//" + Path.GetFileName(ofd.FileName) };
                        selectedNode.Nodes.Add(tnNew);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuDeletefile_Click(object sender, EventArgs e)
        {
            String fileName = selectedNode.Tag.ToString();
            if (ftpClient.DeleteFile(fileName, false))
            {
                trvWeb.Nodes.Remove(selectedNode);
            }
        }

        private void mnuShowImage_Click(object sender, EventArgs e)
        {
            if (selectedNode != null && selectedNode.Tag != null)
            {
                try { ShowImage(selectedNode.Tag.ToString()); } catch { }
            }
            else
            {
                MessageBox.Show("Null");
            }
        }

        private void mnuReplaceImage_Click(object sender, EventArgs e)
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
                        String file = Path.Combine(thisDirectory, Path.GetFileName(ofd.FileName));
                        try
                        {
                            Utilities.ProcessKiller(file);
                            if (File.Exists(file)) { File.Delete(file); }
                            File.Copy(ofd.FileName, file);
                            ftpClient.UploadReplace(file, selectedNode.Tag.ToString(), false);
                        }
                        catch
                        {
                        }
                        picWeb.Image = Image.FromFile(file);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuDeleteImage_Click(object sender, EventArgs e)
        {
            String buildPID = selectedNode.Parent.Parent.Tag.ToString();
            String deleteQuery = "DELETE FROM tt_content WHERE pid = " + buildPID + " AND CType = 'image'";
            String status;
            if (mysql.SetData(deleteQuery, null, out status))
            {
                picWeb.Image = null;
                selectedNode.Parent.Text = "No web image";
                selectedNode.Parent.Nodes.Clear();
            }
        }

        private void mnuShowFile_Click(object sender, EventArgs e)
        {
            try
            {
                String dlFile = selectedNode.Text.Split(new String[] { ":" }, StringSplitOptions.None)[0];
                String thisDirectory = AppDomain.CurrentDomain.BaseDirectory;
                String tempPath = Path.Combine(thisDirectory, dlFile);
                bool success = ftpClient.Download(tempPath, selectedNode.Tag.ToString(), false, out status);
                if (success) { System.Diagnostics.Process.Start(tempPath); } else { MessageBox.Show("status"); }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CreateExcel(chkImage.Checked, chkRules.Checked, chkInsurance.Checked, chkFinance.Checked, chkMinutes.Checked, chkNotices.Checked, chkPlans.Checked);
        }

        private void CreateExcel(bool image, bool rules, bool insurance, bool finance, bool minutes, bool notices, bool plans)
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();

                if (xlApp == null)
                {
                    MessageBox.Show("EXCEL could not be started. Check that your office installation and project references are correct.");
                    return;
                }
                xlApp.Visible = true;

                Excel.Workbook wb = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

                if (ws == null)
                {
                    MessageBox.Show("Worksheet could not be created. Check that your office installation and project references are correct.");
                    return;
                }
                ws.Name = "Building Maintenance";
                ws.Cells[1, "A"].Value2 = "Building";
                ws.Cells[1, "B"].Value2 = "Debtor";
                ws.Cells[1, "C"].Value2 = "PM";
                ws.Cells[1, "D"].Value2 = "Address";
                ws.Cells[1, "E"].Value2 = "Image Name";
                ws.Cells[1, "F"].Value2 = "Image Upload Date";
                int rowIdx = 2;
                int colIdx = 6;
                int nextRow = 2;
                Dictionary<String, int> columnHeaders = new Dictionary<string, int>();
                String[] folders = { "Annual Financial Statements", "Conduct Rules", "Insurance Information", "Meeting Minutes", "Meeting Notices", "Sectional Title Plans" };
                int printedBuilds = 0;
                foreach (WebBuildingPrint wbp in printBuildings)
                {
                    try
                    {
                        bool hasRules = false;
                        bool hasInsurance = false;
                        bool hasFinance = false;
                        bool hasMinutes = false;
                        bool hasNotices = false;
                        bool hasPlans = false;

                        List<String> hasfiles;

                        if (wbp.Files.TryGetValue("Conduct Rules", out hasfiles) && hasfiles.Count > 0) { hasRules = true; }
                        if (wbp.Files.TryGetValue("Insurance Information", out hasfiles) && hasfiles.Count > 0) { hasInsurance = true; }
                        if (wbp.Files.TryGetValue("Annual Financial Statements", out hasfiles) && hasfiles.Count > 0) { hasFinance = true; }
                        if (wbp.Files.TryGetValue("Meeting Minutes", out hasfiles) && hasfiles.Count > 0) { hasMinutes = true; }
                        if (wbp.Files.TryGetValue("Meeting Notices", out hasfiles) && hasfiles.Count > 0) { hasNotices = true; }
                        if (wbp.Files.TryGetValue("Sectional Title Plans", out hasfiles) && hasfiles.Count > 0) { hasPlans = true; }

                        if ((!image && !rules && !insurance && !minutes && !notices && !plans) || (image && wbp.Image == "N/A") || (rules && !hasRules)
                            || (insurance && !hasInsurance) || (finance && !hasFinance) || (minutes && !hasMinutes) || (notices && !hasNotices) || (plans && !hasPlans))
                        {
                            ws.Cells[rowIdx, "A"].Value2 = wbp.BuildingName;
                            ws.Cells[rowIdx, "B"].Value2 = wbp.Debtor;
                            ws.Cells[rowIdx, "C"].Value2 = wbp.PM;
                            ws.Cells[rowIdx, "D"].Value2 = wbp.Address;
                            ws.Cells[rowIdx, "E"].Value2 = wbp.Image;
                            ws.Cells[rowIdx, "F"].Value2 = wbp.ImageUpload;
                            bool fileListing = false;
                            foreach (KeyValuePair<String, List<String>> files in wbp.Files)
                            {
                                if ((chkFFinancial.Checked && files.Key == "Annual Financial Statements") || (chkFConduct.Checked && files.Key == "Conduct Rules")
                                || (chkFInsurance.Checked && files.Key == "Insurance Information") || (chkFMinutes.Checked && files.Key == "Meeting Minutes")
                                    || (chkFNotices.Checked && files.Key == "Meeting Notices") || (chkFPlans.Checked && files.Key == "Sectional Title Plans")
                                    || (chkOther.Checked && !folders.Contains(files.Key)))
                                {
                                    fileListing = true;
                                    int maxRowIdx = rowIdx;
                                    if (!columnHeaders.ContainsKey(files.Key))
                                    {
                                        colIdx++;
                                        columnHeaders.Add(files.Key, colIdx);
                                        ws.Cells[1, colIdx].Value2 = files.Key;
                                    }
                                    foreach (String file in files.Value)
                                    {
                                        ws.Cells[maxRowIdx, columnHeaders[files.Key]].Value2 = file;
                                        maxRowIdx++;
                                    }
                                    if (nextRow < maxRowIdx + 1) { nextRow = maxRowIdx + 1; }
                                }
                            }
                            if (!fileListing) { nextRow++; }
                            rowIdx = nextRow;
                            printedBuilds++;
                        }
                    }
                    catch { }
                }

                ws.Columns.AutoFit();
                ws.Application.ActiveWindow.SplitRow = 1;
                ws.Application.ActiveWindow.SplitColumn = 1;
                ws.Application.ActiveWindow.FreezePanes = true;
                //MessageBox.Show(printedBuilds.ToString());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private string IndexToColumn(int index)
        {
            if (index <= 0) { throw new IndexOutOfRangeException("index must be a positive number"); }
            return Columns[index - 1];
        }

        private void cmbFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListFiles();
        }

        private void ListFiles()
        {
            String where = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                String orgDirectory = myClient.WorkingDirectory;
                buildDirectory = orgDirectory;
                String uploadDirectory = myClient.WorkingDirectory;
                where = "872";
                if (cmbFolder.SelectedItem != null && cmbFolder.SelectedIndex > 1) { uploadDirectory += "//" + cmbFolder.SelectedItem.ToString(); }
                fileList.Items.Clear();
                if (!String.IsNullOrEmpty(uploadDirectory))
                {
                    where = "880";
                    myClient.WorkingDirectory = uploadDirectory;
                    //myClient.ChangeDirectory();
                    List<String> files = myClient.RemoteFiles(false);
                    foreach (String file in files) { fileList.Items.Add(file); }
                    where = "884";
                }
                myClient.WorkingDirectory = orgDirectory;
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(where);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(buildDirectory) && !String.IsNullOrEmpty(txtNewFolder.Text))
            {
                String path = txtNewFolder.Text.Replace("\'", " ");
                bool success = myClient.CreateDirectory(path, false);
                if (success)
                {
                    cmbFolder.Items.Add(path);
                    txtNewFolder.Text = "";
                    MessageBox.Show("Folder Created");
                }
                else
                {
                    MessageBox.Show("Folder Not Created");
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(ofd.FileName))
                {
                    String subfolder = (cmbFolder.SelectedItem != null && cmbFolder.SelectedIndex > 0 ? cmbFolder.SelectedItem.ToString() + "/" : "");
                    if (myClient.Upload(ofd.FileName, myClient.WorkingDirectory + "/" + subfolder + Path.GetFileName(ofd.FileName), cmbCategory.SelectedItem.ToString().StartsWith("Trustee"))) { ListFiles(); }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<String> deleteMe = new List<string>();
            String subfolder = (cmbFolder.SelectedItem != null && cmbFolder.SelectedIndex > 0 ? cmbFolder.SelectedItem.ToString() + "/" : "");

            for (int i = 0; i < fileList.Items.Count; i++)
            {
                if (fileList.GetItemChecked(i))
                {
                    deleteMe.Add(fileList.Items[i].ToString());
                }
            }
            if (deleteMe.Count > 0)
            {
                bool trustee = (subfolder == "Trustees");
                myClient.ChangeDirectory(trustee);
                String orgDir = myClient.WorkingDirectory;
                if (!trustee) { myClient.WorkingDirectory += "/" + subfolder; }
                myClient.DeleteFile(deleteMe, trustee);
                myClient.WorkingDirectory = orgDir;
            }
            ListFiles();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                String newImage = (imgWeb != imgLocal1 && !String.IsNullOrEmpty(copyPic1) ? Path.GetFileName(copyPic2) : Path.GetFileName(webPic2));
                String status;

                Classes.Sftp sftpClient = new Classes.Sftp(String.Empty, true) { WorkingDirectory = "/srv/www/htdocs/uploads/pics" };
                picWeb.Image = null;
                if (sftpClient.Upload((imgWeb != imgLocal1 && !String.IsNullOrEmpty(copyPic1) ? copyPic2 : webPic1), newImage, false))
                {
                    DataSet dsPic = mysql.GetData("SELECT image FROM tt_content WHERE pid = " + buildings[selectedIdx].pid + " AND CType = 'image'", null, out status);
                    String query = "";
                    if (dsPic != null && dsPic.Tables.Count > 0 && dsPic.Tables[0].Rows.Count > 0)
                    {
                        query = "UPDATE tt_content SET image = '" + newImage + "' WHERE pid = " + buildings[selectedIdx].pid + " AND CType = 'image'";
                    }
                    else
                    {
                        query = "INSERT INTO tt_content(image, pid, CType) VALUES('" + newImage + "', '" + buildings[selectedIdx].pid + "', 'image')";
                    }
                    picWeb.Image = (Image)Image.FromFile(copyPic1).Clone();
                    mysql.SetData(query, null, out status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (picWeb.Image != null)
            {
                String deleteQuery = "DELETE FROM tt_content WHERE pid = " + buildings[selectedIdx].pid + " AND CType = 'image'";
                String status;
                if (mysql.SetData(deleteQuery, null, out status)) { picWeb.Image = null; }
            }
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
                    String subfolder = (cmbFolder.SelectedItem != null && cmbFolder.SelectedIndex > 0 ? cmbFolder.SelectedItem.ToString() + "/" : "");
                    String remoteFile = myClient.WorkingDirectory + "/" + subfolder + file;
                    MessageBox.Show(remoteFile);
                    myClient.Download(Path.Combine(Path.GetTempPath(), file), remoteFile, false, out status);
                    Process.Start(Path.Combine(Path.GetTempPath(), file));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
                        imgLocal1 = null;
                        try
                        {
                            imgLocal1 = (Image)Image.FromFile(ofd.FileName).Clone();
                            imgLocal1.Save(copyPic1);
                            imgLocal1.Save(copyPic2);
                            //Utilities.ProcessKiller(copyPic1);
                            //if (File.Exists(copyPic1)) { File.Delete(copyPic1); }
                            //File.Copy(ofd.FileName, copyPic1);
                            //imgLocal1 = Image.FromFile(copyPic1);
                        }
                        catch
                        {
                        }
                        try
                        {
                            //Utilities.ProcessKiller(copyPic2);
                            //if (File.Exists(copyPic2)) { File.Delete(copyPic2); }
                            //File.Copy(ofd.FileName, copyPic2);
                        }
                        catch
                        {
                        }
                        picWeb.Image = imgLocal1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFolders(buildingIdx, cmbCategory.SelectedItem.ToString().StartsWith("Trustee"));
        }
    }
}
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Astrodon.Classes {

    public class Sftp {
        private const int port = 22;
        private const string host = "www.astrodon.co.za";
        private const string username = "root";
        private const string password = "root@66r94e!@#";
        private string workingdirectory = "/srv/www/htdocs/fileadmin/Complexes";
        private String trusteeDirectory = "/srv/www/htdocs/fileadmin/Trustees";
        private const string clientdirectory = "/srv/www/htdocs/uploads/tx_astro";
        private SftpClient client;
        private bool isBuilding;
        private String buildingFolder = String.Empty;

        public Sftp(String where, bool isclient) {
            isBuilding = !isclient;
            if (isclient) {
                workingdirectory = clientdirectory;
            } else {
                buildingFolder = where;
                workingdirectory += (!String.IsNullOrEmpty(where) ? "/" + where : "");
            }
        }

        public Sftp(String where, bool isclient, bool isTrustee) {
            isBuilding = !isclient;
            if (isTrustee) {
                workingdirectory = trusteeDirectory;
            }
            buildingFolder = where;
            workingdirectory += (!String.IsNullOrEmpty(where) ? "/" + where : "");
        }

        public String WorkingDirectory {
            get { return workingdirectory; }
            set { workingdirectory = value; }
        }

        public bool ConnectClient(bool trustee) {
            //if (Environment.MachineName == "PASTELPARTNER")
          //      return false;

            PasswordAuthenticationMethod PasswordConnection = new PasswordAuthenticationMethod("root", "root@66r94e!@#");
            KeyboardInteractiveAuthenticationMethod KeyboardInteractive = new KeyboardInteractiveAuthenticationMethod("root");
            ConnectionInfo connectionInfo = new ConnectionInfo("www.astrodon.co.za", port, "root", PasswordConnection, KeyboardInteractive);
            KeyboardInteractive.AuthenticationPrompt += delegate(object sender, AuthenticationPromptEventArgs e) {
                foreach (var prompt in e.Prompts) {
                    if (prompt.Request.Equals("Password: ", StringComparison.InvariantCultureIgnoreCase)) {
                        prompt.Response = password;
                    }
                }
            };
            client = new SftpClient(connectionInfo);
            try {
                client.Connect();
                Console.WriteLine("Connected to {0}", host);
                client.ChangeDirectory("..");
                if (!trustee) {
                    client.ChangeDirectory(workingdirectory);
                } else {
                    if (CheckTrusteeDirectory()) { client.ChangeDirectory(trusteeDirectory); }
                }
                return client.IsConnected;
            } catch (Exception e) {
                Controller.HandleError(e);
                return false;
            }
        }

        private bool CheckTrusteeDirectory() {
            bool exists = false;
            if (!String.IsNullOrEmpty(buildingFolder)) {
                List<String> folders = RemoteFolders(true);
                exists = !folders.Contains(buildingFolder);
                if (!exists) {
                    exists = CreateDirectory(buildingFolder, true);
                    if (exists) { trusteeDirectory += "/" + buildingFolder; }
                }
            }
            return exists;
        }

        public bool DisconnectClient() {
            try {
                client.Disconnect();
                return !client.IsConnected;
            } catch {
                return false;
            }
        }

        public bool Upload(String fileName, String remoteFile, bool trustee) {
            if (client == null || !client.IsConnected) {
                ConnectClient(trustee);
            }
            bool success = false;
            try {
                using (var fileStream = new FileStream(fileName, FileMode.Open)) {
                    Console.WriteLine("Uploading {0} ({1:N0} bytes)", fileName, fileStream.Length);
                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    client.UploadFile(fileStream, remoteFile);

                    var listDirectory = (!trustee ? client.ListDirectory(workingdirectory) : client.ListDirectory(trusteeDirectory));

                    foreach (var fi in listDirectory) {
                        if (fi.Name == remoteFile.Replace((trustee ? trusteeDirectory : workingdirectory) + "/", "")) {
                            success = true;
                            break;
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return success;
        }

        public bool UploadReplace(String fileName, String remoteFile, bool trustee) {
            if (client == null || !client.IsConnected) {
                ConnectClient(trustee);
            }
            bool success = false;
            try {
                using (var fileStream = new FileStream(fileName, FileMode.Open)) {
                    Console.WriteLine("Uploading {0} ({1:N0} bytes)", fileName, fileStream.Length);
                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    client.UploadFile(fileStream, remoteFile);
                    success = true;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return success;
        }

        public bool CreateDirectory(String path, bool trustee) {
            if (client == null || !client.IsConnected) { ConnectClient(trustee); }
            try {
                List<String> folders = RemoteFolders(trustee);
                if (trustee) { ChangeDirectory(trustee); }
                if (!folders.Contains(path)) {
                    client.CreateDirectory(client.WorkingDirectory + "/" + path);
                    client.ChangePermissions(client.WorkingDirectory + "/" + path, 777);
                    folders = RemoteFolders(trustee);
                    if (folders.Contains(path)) {
                        return true;
                    } else {
                        MessageBox.Show("No folder created");
                        return false;
                    }
                } else {
                    return true;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void ChangeDirectory(bool trustee) {
            if (client == null || !client.IsConnected) { ConnectClient(trustee); }
            client.ChangeDirectory(trustee ? trusteeDirectory : workingdirectory);
        }

        public bool DeleteDirectory(String path, bool trustee) {
            if (client == null || !client.IsConnected) { ConnectClient(trustee); }
            try {
                client.DeleteDirectory(path);
                List<String> folders = RemoteFolders(trustee);
                return !folders.Contains(path);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool RenameDirectory(String oldpath, String newpath, bool trustee, out String success) {
            if (client == null || !client.IsConnected) { ConnectClient(trustee); }
            try {
                client.RenameFile(oldpath, newpath);
                success = "OK";
                return true;
            } catch (Exception ex) {
                success = ex.Message;
                return false;
            }
        }

        public bool Download(String fileName, String remoteFile, bool trustee, out String status) {
            if (client == null || !client.IsConnected) {
                ConnectClient(trustee);
            }
            bool success = false;
            status = String.Empty;
            try {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite)) {
                    Console.WriteLine("Uploading {0} ({1:N0} bytes)", fileName, fileStream.Length);
                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    client.DownloadFile(remoteFile, fileStream);
                }
                if (File.Exists(fileName)) {
                    status = "OK";
                    success = true;
                }
            } catch (Exception ex) {
                status = ex.Message;
            }
            return success;
        }

        public bool DeleteFile(List<String> files, bool trustee) {
            try {
                if (client == null || !client.IsConnected) { ConnectClient(trustee); }
                foreach (String file in files) { client.DeleteFile((trustee ? trusteeDirectory : workingdirectory) + "/" + file); }
                return true;
            } catch {
                return false;
            }
        }

        public bool DeleteFile(String file, bool trustee) {
            try {
                if (client == null || !client.IsConnected) { ConnectClient(trustee); }
                client.DeleteFile(file);
                return true;
            } catch {
                return false;
            }
        }

        public List<String> RemoteFolders(bool trustee) {
            if (client == null || !client.IsConnected) {
                ConnectClient(trustee);
            }
            List<String> directories = new List<string>();
            try {
                var files = client.ListDirectory(trustee ? trusteeDirectory : workingdirectory);
                foreach (var file in files) {
                    if (!directories.Contains(file.FullName) && !file.Name.StartsWith(".") && !file.Name.Contains(".")) {
                        directories.Add(file.Name);
                    }
                }
            } catch { }
            return directories;
        }

        public List<String> RemoteFiles(bool trustee) {
            if (client == null || !client.IsConnected) { ConnectClient(trustee); }
            List<String> directories = new List<string>();
            try {
                //MessageBox.Show(workingdirectory);
                var files = client.ListDirectory(trustee ? trusteeDirectory : workingdirectory);
                foreach (var file in files) {
                    if (!directories.Contains(file.FullName) && !file.Name.StartsWith(".") && file.Name.Contains(".")) {
                        directories.Add(file.Name);
                    }
                }
            } catch { }
            return directories;
        }

        public DateTime GetAccessDate(String file, bool trustee) {
            if (client == null || !client.IsConnected) { ConnectClient(trustee); }
            DateTime accessDate = DateTime.Now.AddYears(-10);
            try {
                accessDate = client.GetLastWriteTime(file);
            } catch { }
            return accessDate;
        }
    }
}
﻿using Astro.Library.Entities;
using Astrodon.ClientPortal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Astrodon.Classes
{
    public class webreport
    {
        private List<Building> buildings;
        private List<WebReportData> report = new List<WebReportData>();

        public webreport()
        {
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            buildings = new Buildings(false).buildings.OrderBy(c => c.Name).ToList();
            foreach (Building b in buildings) { report.Add(GetBaseData(b)); }
        }

        private WebReportData GetBaseData(Building building)
        {
            WebReportData wrd = new WebReportData();
            wrd.building = building.Name;
            wrd.picture = null;
            var logoData = new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString()).GetBuildingImage(building.ID);
            if (logoData != null)
            {
                using (var memLogo = new MemoryStream(logoData))
                {
                    Image img = Image.FromStream(memLogo);
                    wrd.picture = img;
                }
            }

            //String status;
            //DataSet dsPic = mysql.GetData("SELECT image FROM tt_content WHERE pid = " + building.pid + " AND CType = 'image'", null, out status);
            //if (dsPic != null && dsPic.Tables.Count > 0 && dsPic.Tables[0].Rows.Count > 0)
            //{
            //    String picture = dsPic.Tables[0].Rows[0]["image"].ToString();
            //    Classes.Sftp sftpClient = new Classes.Sftp(String.Empty, true);
            //    sftpClient.WorkingDirectory = "/srv/www/htdocs/uploads/pics";
            //    String thisDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //    String file1 = picture.Replace(Path.GetFileNameWithoutExtension(picture), Path.GetFileNameWithoutExtension(picture) + "_web1");
            //    String tempPath = Path.Combine(thisDirectory, file1);
            //    bool success = sftpClient.Download(tempPath, picture, false, out status);
            //    if (!success)
            //    {
            //        MessageBox.Show(status);
            //        wrd.picture = null;
            //    }
            //    else
            //    {
            //        Image img = Image.FromFile(file1);
            //        wrd.picture = img;
            //    }
            //}
            //else
            //{
            //    wrd.picture = null;
            //}

            List<String> folders = LoadFolders(building.webFolder);
            List<String> rootFiles = ListFiles(building.webFolder, String.Empty);
            wrd.files.Add("Root", rootFiles);
            foreach (String folder in folders)
            {
                try
                {
                    wrd.files.Add(folder, ListFiles(building.webFolder, folder));
                }
                catch(Exception ex) { Controller.HandleError(ex); }
            }
            return wrd;
        }

        private List<String> LoadFolders(String web)
        {
            return new List<string>();
        }

        private List<String> ListFiles(String baseFolder, String folder)
        {
            return new List<string>();
        }
    }
}
﻿using Astrodon.ClientPortal.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Astrodon.ClientPortal
{
    public class AstrodonClientPortal
    {
        private string _ClientProtalConnection;
        public AstrodonClientPortal(string clientPortalConnection)
        {
            _ClientProtalConnection = clientPortalConnection;
        }
        //'api/building/GetUnitDocument/' + id;
        private const string _documentLinkURL = "https://clientportal.astrodon.co.za/api/building/";

        private const int MAX_TITLE_LENGTH = 500;
        private const int MAX_FILENAME_LENGTH = 200;

        public string GetUnitDocumentLink(Guid documentId, string accessToken)
        {
            
            string encryptedToken = "";
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                if (accessToken.Length > 30)
                    accessToken = accessToken.Substring(0, 30);
                encryptedToken = "?accessToken=" + Cipher.Encrypt(accessToken);
            }
            return _documentLinkURL + "GetUnitDocument/" + documentId.ToString("N")+encryptedToken;
        }

        public string GetBuildingDocumentLink(Guid documentId, string accessToken)
        {
            string encryptedToken = "";
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                if (accessToken.Length > 30)
                    accessToken = accessToken.Substring(0, 30);
                encryptedToken = "?accessToken=" + Cipher.Encrypt(accessToken);
            }
            return _documentLinkURL + "GetBuildingDocument/" + documentId.ToString("N") + encryptedToken;
        }

        #region Building Sync
        /*Update all Client accounts for a building and set the Trustee Flag on or off*/
        /*Ensure logins are created for all users of the building*/
        public void SyncBuildingAndClients(int buildingId)
        {
            string script = ReadSQLScript("SyncBuilding.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                { new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId) };
            SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);
        }

        /*Update all building records and client records*/
        public void SyncBuildings()
        {
            string script = ReadSQLScript("SyncBuildingsAndUnits.sql");

            SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script);
        }

        public void DeleteBuilding(int buildingId)
        {
            string script = ReadSQLScript("DeleteBuilding.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                { new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId) };
            SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);
        }

        #endregion

        #region User Setup

        public void CreateUserRecord(string emailAddress, string password, string accountNumber)
        {
            password = Cipher.Encrypt(password);

            string script = ReadSQLScript("CreateUserRecord.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                new System.Data.SqlClient.SqlParameter("@EmailAddress",emailAddress),
                 new System.Data.SqlClient.SqlParameter("@PasswordHash",password),
                   new System.Data.SqlClient.SqlParameter("@AccountNumber",accountNumber),

            };
            SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);
        }

        public string GetLoginPassword(string emailAddress)
        {
            string script = ReadSQLScript("GetLoginPassword.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                { new System.Data.SqlClient.SqlParameter("@EmailAddress",emailAddress) };
            var ds = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);

            if(ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
            {
                string password = (string)ds.Tables[0].Rows[0]["PasswordHash"];
                return Cipher.Decrypt(password);
            }
            return string.Empty; //no login found
        }

        public List<string> GetLinkedUnits(int buildingId, string emailAddress)
        {
            List<string> accountNumbers = new List<string>();

            string script = ReadSQLScript("GetLinkedUnits.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                { new System.Data.SqlClient.SqlParameter("@EmailAddress",emailAddress),
            new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId)};
            var ds = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);

            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    accountNumbers.Add((string)row["AccountNumber"]);
                }
            }
            return accountNumbers;
        }

        #endregion

        #region Building Image

        public byte[] GetBuildingImage(int buildingId)
        {
            byte[] result = null;
            string script = ReadSQLScript("GetBuildingImage.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                { new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId) };
            var ds = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
            {
                var val = ds.Tables[0].Rows[0]["BuildingImage"];
                result = ReadBinaryValue(val);
            }
            if(result == null)
                result = ReadDefaultImage();

            return result;
        }
   
        public byte[] SaveBuildingImage(int buildingId, byte[] image)
        {
            string script = ReadSQLScript("SaveBuildingImage.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                { new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                  new System.Data.SqlClient.SqlParameter("@ImageData",image)
                  };
            SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);

            return image;
        }

        public void DeleteBuildingImage(int buildingId)
        {
            string script = ReadSQLScript("DeleteBuildingImage.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                  new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                };
            SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);
        }

        #endregion

        #region Unit Documents

        public void UpdatePrimaryEmail(int buildingId, string accountNumber, string oldEmail, string newEmail)
        {
            if (!String.IsNullOrWhiteSpace(oldEmail) && !string.IsNullOrWhiteSpace(newEmail))
            {
                string script = ReadSQLScript("UpdatePrimaryEmail.sql");
                var parameters = new List<System.Data.SqlClient.SqlParameter>()
                { new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                  new System.Data.SqlClient.SqlParameter("@AccountNumber",accountNumber),
                  new System.Data.SqlClient.SqlParameter("@OldEmail",oldEmail),
                  new System.Data.SqlClient.SqlParameter("@NewEmail",newEmail),
                  };
                SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);
            }
        }

        public string InsertStatement(int buildingId, string accountNumber,  DateTime statementDate, string filename, byte[] fileData, string emailAddress)
        {
            string title = "Statement " + statementDate.ToString("dd-MMMM-yyyy", CultureInfo.InstalledUICulture);

            return UploadUnitDocument(DocumentCategoryType.FinancialStatement, statementDate, buildingId, accountNumber, filename, title, fileData, emailAddress);
        }

        public string UploadUnitDocument(DocumentCategoryType documentType, DateTime fileDate, int buildingId,
            string accountNumber,  string title, string filePath, string emailAddress)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return UploadUnitDocument(documentType, fileDate, buildingId, accountNumber, Path.GetFileName(filePath),title, buffer, emailAddress);
            }
        }

        public string UploadUnitDocument(DocumentCategoryType documentType,DateTime fileDate, int buildingId,
            string accountNumber, string filename, string title, byte[] data,string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "Correspondence ";

            filename = Path.GetFileName(filename);

            filename = filename.Replace(" ", "");
            //return a URL to the uploaded document

            Guid documentId = System.Guid.NewGuid(); 

            if (title.Length > MAX_TITLE_LENGTH)
                title = title.Substring(0, MAX_TITLE_LENGTH);

            if (filename.Length > MAX_FILENAME_LENGTH)
                filename = Path.GetFileNameWithoutExtension(filename).Substring(0, MAX_FILENAME_LENGTH - 4) + "." + Path.GetExtension(filename);

            string script = ReadSQLScript("UploadUnitDocument.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@DocumentType",(int)documentType),
                    new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                    new System.Data.SqlClient.SqlParameter("@AccountNumber",accountNumber),
                    new System.Data.SqlClient.SqlParameter("@Filename",filename),
                    new System.Data.SqlClient.SqlParameter("@Description",title),
                    new System.Data.SqlClient.SqlParameter("@FileDate",fileDate),
                    new System.Data.SqlClient.SqlParameter("@DocumentId",documentId),
                    new System.Data.SqlClient.SqlParameter("@Data",data),
                  };


            var result = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);

            documentId = (Guid)result.Tables[0].Rows[0]["Id"];

            return GetUnitDocumentLink(documentId, emailAddress);
        }
      
        public List<FileDetail> GetUnitFiles(int buildingId, string accountNumber,bool activeOnly = true)
        {
            string script = ReadSQLScript("GetUnitFiles.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                    new System.Data.SqlClient.SqlParameter("@AccountNumber",accountNumber),
                    new System.Data.SqlClient.SqlParameter("@ActiveOnly",activeOnly)
                  };

            var data = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);

            List<FileDetail> result = new List<FileDetail>();
            foreach(DataRow row in data.Tables[0].Rows)
            {
                result.Add(new FileDetail(row));
            }

            return result;
                 
        }

        public List<FileDetail> GetBuildingUnitFiles(int buildingId, bool activeOnly = true)
        {
            string script = ReadSQLScript("GetBuildingUnitFiles.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                    new System.Data.SqlClient.SqlParameter("@ActiveOnly",activeOnly)
                  };

            var data = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);

            List<FileDetail> result = new List<FileDetail>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                result.Add(new FileDetail(row));
            }

            return result;
        }

        public void MarkUnitFilesInactive(List<Guid> unitDocumentIds)
        {
            string script = "update UnitDocument set IsActive = 0 where Id = @Id";

            foreach(Guid id in unitDocumentIds)
            {
                var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@Id",id),
                };
                SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);
            }
        }

        public void DeleteUnitFile(Guid unitDocumentId)
        {
            MarkUnitFilesInactive(new List<Guid>() { unitDocumentId });
        }

        public byte[] GetUnitFile(Guid unitDocumentId)
        {
            string script = ReadSQLScript("GetUnitFile.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@UnitDocumentId",unitDocumentId),
                  };

            var ds = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
            {
                var val = ds.Tables[0].Rows[0]["DocumentData"];
                return ReadBinaryValue(val);
            }
            return null;
        }

        public List<WebDocumentAccessLogItem> GetUnitFileAccessHistory(Guid unitDocumentId)
        {
            string script = ReadSQLScript("GetUnitFileAccesHistory.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@UnitDocumentId",unitDocumentId),
                  };

            var ds = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);

            List<WebDocumentAccessLogItem> result = new List<WebDocumentAccessLogItem>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                result.Add(new WebDocumentAccessLogItem(row));
            }

            return result;
        }

        #endregion

        #region Building Document

        public string UploadBuildingDocument(DocumentCategoryType documentType, DateTime fileDate,
           int buildingId, string description, string filePath, string emailAddress)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return UploadBuildingDocument(documentType, fileDate, buildingId, description, Path.GetFileName(filePath), buffer, emailAddress);
            }
        }

        public string UploadBuildingDocument(DocumentCategoryType documentType, DateTime fileDate,
          int buildingId, string description, string filename, byte[] fileData, string emailAddress)
        {
            filename = Path.GetFileName(filename);
            //return a URL to the uploaded document


            if (description.Length > MAX_TITLE_LENGTH)
                description = description.Substring(0, MAX_TITLE_LENGTH);

            if (filename.Length > MAX_FILENAME_LENGTH)
                filename = Path.GetFileNameWithoutExtension(filename).Substring(0, MAX_FILENAME_LENGTH - 4) + "." + Path.GetExtension(filename);



            Guid documentId = System.Guid.NewGuid();

            string script = ReadSQLScript("UploadBuildingDocument.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@DocumentType",(int)documentType),
                    new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                    new System.Data.SqlClient.SqlParameter("@Filename",filename),
                    new System.Data.SqlClient.SqlParameter("@Description",description),
                    new System.Data.SqlClient.SqlParameter("@FileDate",fileDate),
                    new System.Data.SqlClient.SqlParameter("@DocumentId",documentId),
                    new System.Data.SqlClient.SqlParameter("@Data",fileData),
                  };
            var result = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);

            documentId = (Guid)result.Tables[0].Rows[0]["Id"];


            return GetBuildingDocumentLink(documentId, emailAddress);
        }

        public List<FileDetail> BuildingDocumentList(int buildingId, bool activeOnly = true)
        {
            string script = ReadSQLScript("BuildingDocumentList.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                    new System.Data.SqlClient.SqlParameter("@ActiveOnly",activeOnly)
                  };

            var data = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);

            List<FileDetail> result = new List<FileDetail>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                result.Add(new FileDetail(row));
            }

            return result;
        }

        public void DeleteBuildingFiles(List<Guid> fileIds)
        {
            string script = "update BuildingDocument set IsActive = 0 where Id = @Id";

            foreach (Guid id in fileIds)
            {
                var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@Id",id),
                };
                SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);
            }
        }

        public byte[] GetBuildingFile(int buildingId, Guid fileId)
        {
            string script = ReadSQLScript("GetBuildingFile.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@BuildingDocumentId",fileId),
                    new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                };

            var ds = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
            {
                var val = ds.Tables[0].Rows[0]["DocumentData"];
                return ReadBinaryValue(val);
            }
            return null;
        }

        #endregion

        #region Untilities

        private string ReadSQLScript(string scriptName)
        {
            string resourcePath = "Astrodon.ClientPortal.Scripts." + scriptName;
            return SQLUtilities.ReadResourceScript(this.GetType().Assembly, resourcePath);
        }

        private byte[] ReadDefaultImage()
        {
            string resourcePath = "Astrodon.ClientPortal.Images.logo.jpg";

            using (var stream = this.GetType().Assembly.GetManifestResourceStream(resourcePath))
            {
                var result = new byte[stream.Length];
                stream.Read(result, 0, result.Length);
                return result;
            }
        }

        private byte[] ReadBinaryValue(object fieldValue)
        {
            if (fieldValue is DBNull)
                return null;
            else
                return (byte[])fieldValue;
        }

       

        #endregion

    }
}

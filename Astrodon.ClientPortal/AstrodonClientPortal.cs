using Astrodon.ClientPortal.SQL;
using Astrodon.Data;
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

        private const string _documentLinkURL = "http://clientportal.astrodon.co.za/View";

        public string GetUnitDocumentLink(Guid documentId)
        {
            return _documentLinkURL + "?documentId=" + documentId.ToString("N") + "&documentType=unit";
        }

        public string GetBuildingDocumentLink(Guid documentId)
        {
            return _documentLinkURL + "?documentId=" + documentId.ToString("N") + "&documentType=building";
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
            string script = ReadSQLScript("GetBuildingImage.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                { new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId) };
            var ds = SQLUtilities.FetchData(_ClientProtalConnection, script, parameters);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
            {
                var val = ds.Tables[0].Rows[0]["BuildingImage"];
                return ReadBinaryValue(val);
            }
            return null;
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

        public string InsertStatement(int buildingId, string accountNumber,  DateTime statementDate, string filename, byte[] fileData)
        {
            return UploadUnitDocument(DocumentCategoryType.FinancialStatement, statementDate, buildingId, accountNumber, filename, statementDate.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture), fileData);
        }

        public string UploadUnitDocument(DocumentCategoryType documentType,DateTime fileDate, int buildingId,
            string accountNumber, string filename, string description, byte[] data)
        {
            filename = Path.GetFileName(filename);
            //return a URL to the uploaded document

            Guid documentId = System.Guid.NewGuid();

            string script = ReadSQLScript("UploadUnitDocument.sql");
            var parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@DocumentType",(int)documentType),
                    new System.Data.SqlClient.SqlParameter("@BuildingId",buildingId),
                    new System.Data.SqlClient.SqlParameter("@AccountNumber",accountNumber),
                    new System.Data.SqlClient.SqlParameter("@Filename",filename),
                    new System.Data.SqlClient.SqlParameter("@Description",description),
                    new System.Data.SqlClient.SqlParameter("@FileDate",fileDate),
                    new System.Data.SqlClient.SqlParameter("@DocumentId",documentId),
                    new System.Data.SqlClient.SqlParameter("@Data",data),
                  };
            SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);

            return GetUnitDocumentLink(documentId);
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

        #endregion

        #region Building Document

        public string UploadBuildingDocument(DocumentCategoryType documentType, DateTime fileDate,
          int buildingId, string description, string filename, byte[] fileData)
        {
            filename = Path.GetFileName(filename);
            //return a URL to the uploaded document

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
            SQLUtilities.ExecuteSqlCommand(_ClientProtalConnection, script, parameters);

            return GetBuildingDocumentLink(documentId);
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

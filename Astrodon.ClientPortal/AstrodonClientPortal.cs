using Astrodon.Data;
using System;
using System.Collections.Generic;
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

        /*Update all Client accounts for a building and set the Trustee Flag on or off*/
        /*Ensure logins are created for all users of the building*/
        public void SyncBuildingAndClients(int buildingId)
        {
            throw new NotImplementedException();
        }

        /*Update all building records and client records*/
        public void SyncBuildings()
        {
            throw new NotImplementedException();
        }

        public void DeleteBuilding(int buildingId)
        {
            throw new NotImplementedException();
        }

        public string GetLoginPassword(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public List<string> GetLinkedUnits(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBuildingImage(int buildingId)
        {
            throw new NotImplementedException();
        }

        public byte[] SaveBuildingImage(int buildingId, byte[] image)
        {
            throw new NotImplementedException();
        }

        public string InsertStatement(int buildingId, string accountNumber,  DateTime statementDate, string filename, byte[] fileData)
        {
            return UploadDocument(DocumentCategoryType.FinancialStatement, buildingId, accountNumber, filename, statementDate.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture), fileData);
        }

        public string UploadDocument(DocumentCategoryType documentType, int buildingId, string accountNumber, string filename, string description, byte[] data)
        {
            filename = Path.GetFileName(filename);
            //return a URL to the uploaded document
            throw new NotImplementedException();
        }

        public string UploadBuildingDocument(DocumentCategoryType documentType, int buildingId, string description, string filename, byte[] fileData)
        {
            throw new NotImplementedException();
        }

        public List<FileDetail> GetUnitFiles(int buildingId, string accountNumber)
        {
            throw new NotImplementedException();
        }

        public List<FileDetail> GetBuildingCustomerFiles(int buildingId)
        {
            //all files for all customers on a building
            throw new NotImplementedException();
        }

        public void MarkDocumentsInactive(List<Guid> unitDocumentIds)
        {
            throw new NotImplementedException();
        }

        public List<FileDetail> BuildingDocumentList(int buildingId)
        {
            throw new NotImplementedException();
        }

        public void DeleteBuildingImage(int buildingId)
        {
            throw new NotImplementedException();
        }

        public byte[] GetUnitFile(Guid id)
        {
            throw new NotImplementedException();
        }

        public void DeleteUnitFile(Guid id)
        {
            throw new NotImplementedException();
        }

        public void DeleteBuildingFiles(int buildingId, List<Guid> fileIds)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBuildingFile(int buildingId, Guid fileId)
        {
            throw new NotImplementedException();
        }
    }
}

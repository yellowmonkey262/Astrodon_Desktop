/*
declare @DocumentId UniqueIdentifier
declare @DocumentType int
declare @FileDate DateTime

declare @BuildingId int
declare @Filename  nvarchar(500)
declare @Description  nvarchar(500)
declare @Data  varbinary(max)*/

declare @LocalBuildingId UniqueIdentifier
declare @WebDocumentId UniqueIdentifier
set @WebDocumentId =  NEWID()

set @LocalBuildingId = ( select b.Id
						from Building b
						where b.BuildingId = @BuildingId)

Begin transaction tran_upload_building_document
BEGIN TRY

   insert into WebDocument
   (Id,DocumentData)
   values
   (@WebDocumentId,@Data)

   insert into BuildingDocument
   (Id,Title,FileYear,FileMonth,DateUploaded,DocumentCategory,BuildingId,FileName,WebDocumentId,IsActive)
   values
   (@DocumentId,@Description,DATEPART(year,@FileDate),DATEPART(month,@FileDate),GetDate(),@DocumentType,@LocalBuildingId,@Filename,@WebDocumentId,1)
   
    Commit transaction tran_upload_building_document
END TRY
BEGIN CATCH
   rollback transaction tran_upload_building_document
   RAISERROR('Unable to upload file - transaction aborted',16,1)
END CATCH

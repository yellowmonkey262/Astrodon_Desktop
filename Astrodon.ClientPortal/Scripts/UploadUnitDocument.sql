/*
declare @DocumentType int
declare @FileDate DateTime
declare @DocumentId UniqueIdentifier

declare @BuildingId int
declare @AccountNumber nvarchar(500)
declare @Filename  nvarchar(500)
declare @Description  nvarchar(500)
declare @Data  varbinary(max)
*/

declare @BuildingUnitId UniqueIdentifier
declare @WebDocumentId UniqueIdentifier
set @WebDocumentId =  NEWID()

set @BuildingUnitId = ( select bu.Id
						from BuildingUnit bu
						join Building b on b.Id = bu.BuildingId
						where b.BuildingId = @BuildingId
						  and bu.AccountNumber = @AccountNumber)

Begin transaction tran_upload_document
BEGIN TRY

   insert into WebDocument
   (Id,DocumentData)
   values
   (@WebDocumentId,@Data)


   insert into UnitDocument
   (Id,Title,FileYear,FileMonth,DateUploaded,DocumentCategory,BuildingUnitId,FileName,WebDocumentId,IsActive)
   values
   (@DocumentId,@Description,DATEPART(year,@FileDate),DATEPART(month,@FileDate),GetDate(),@DocumentType,@BuildingUnitId,@Filename,@WebDocumentId,1)
   
    Commit transaction tran_upload_document
END TRY
BEGIN CATCH
   rollback transaction tran_upload_document
    RAISERROR('Unable to upload file - transaction aborted',16,1)
END CATCH



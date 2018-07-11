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

   declare @ExistingDocumentId uniqueidentifier
   Select top 1 @ExistingDocumentId = id, @WebDocumentId = WebDocumentId
   from UnitDocument
   where BuildingUnitId = @BuildingUnitId
	and FileYear = DATEPART(year,@FileDate)
	and FileMonth = DATEPART(month,@FileDate)
	and DocumentCategory = @DocumentType
	and FileName = @Filename
	and IsActive = 1
  order by DateUploaded desc

	if(@ExistingDocumentId is null)
	begin
	   set @WebDocumentId =  NEWID()
	   insert into WebDocument
	   (Id,DocumentData)
	   values
	   (@WebDocumentId,@Data)
	   
	   insert into UnitDocument
	   (Id,Title,FileYear,FileMonth,DateUploaded,DocumentCategory,BuildingUnitId,FileName,WebDocumentId,IsActive)
	   values
	   (@DocumentId,@Description,DATEPART(year,@FileDate),DATEPART(month,@FileDate),GetDate(),@DocumentType,@BuildingUnitId,
		@Filename,@WebDocumentId,1)
	end
	else
	begin
	  Update WebDocument set DocumentData = @Data where Id = @WebDocumentId

	  Update UnitDocument
	  set Id = @DocumentId,
	      Title = @Description,
		  IsActive = 1,
		  DateUploaded = GetDate()
	  where Id = @ExistingDocumentId
	end
   
    Commit transaction tran_upload_document
END TRY
BEGIN CATCH
   rollback transaction tran_upload_document
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    -- Use RAISERROR inside the CATCH block to return error
    -- information about the original error that caused
    -- execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               )
END CATCH



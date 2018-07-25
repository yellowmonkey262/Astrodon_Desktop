/*
declare @EmailAddress varchar(200)
declare @PasswordHash varchar(200)
*/

declare @UId uniqueidentifier

set @UId = (select id from UserIdentity where EmailAddress = @EmailAddress)
if(@UId is null)
begin

   set @UId = NEWID()

   insert into UserIdentity
   (Id,DateCreated,EmailAddress,IsActive)
   values
   (@UId,GETDATE(),@EmailAddress,1)

   insert into PasswordControl
   (Id,UserIdentityId,PasswordHash,FailedLoginAttempts)
   values
   (NEWID(),@UId,@PasswordHash,0)

   insert into UserBuildingUnit
   (Id,BuildingUnitId,UserIdentityId)
   select NEWID(),Id,@UId
   from BuildingUnit 
   where EmailAddress1 = @EmailAddress
   or  EmailAddress2 = @EmailAddress
   or  EmailAddress3 = @EmailAddress
   or  EmailAddress4 = @EmailAddress

end


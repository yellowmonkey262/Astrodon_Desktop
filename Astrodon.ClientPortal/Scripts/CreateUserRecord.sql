/*
declare @EmailAddress varchar(200)
declare @PasswordHash varchar(200)
declare @AccountNumber varchar(200)
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
end

--Link Unit
insert into UserBuildingUnit
(Id,BuildingUnitId,UserIdentityId)
Select  NEWID(),bu.Id,@UId 
from BuildingUnit bu
left join UserBuildingUnit uu on bu.Id = uu.BuildingUnitId
          and uu.UserIdentityId = @UId
where bu.AccountNumber = @AccountNumber
and uu.Id is null



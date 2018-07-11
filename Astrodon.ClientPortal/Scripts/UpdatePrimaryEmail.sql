/*declare @BuildingId int
declare @AccountNumber nvarchar(max)
declare @OldEmail nvarchar(max)
declare @NewEmail nvarchar(max)
*/

declare @UnitId uniqueIdentifier
declare @UserIdentityId nvarchar(max)

select @UserIdentityId = id
from UserIdentity
where EmailAddress = @OldEmail

Select @UnitId = u.Id
	from BuildingUnit u
	join Building b on u.BuildingId = b.Id
	where u.AccountNumber = @AccountNumber
	  and b.BuildingId = @BuildingId

if(@UserIdentityId is not null)
begin

	delete from UserBuildingUnit 
	where BuildingUnitId = @UnitId 
	  and UserIdentityId = @UserIdentityId

	/*Are there any other users linked to this unit*/

	declare @OtherUsers int
	set @OtherUsers = (Select count(*) from UserBuildingUnit where BuildingUnitId = @UnitId)
	if(@OtherUsers = null)
	begin
	  --update all unit documents and mark them as Inactive
	  update UnitDocument set IsActive = 0 where BuildingUnitId = @UnitId
	end

	--check if the user has any other buildings linked
	declare @LinkedBuildings int
	select @LinkedBuildings = count(*) 
	from UserBuildingUnit
	where UserIdentityId = @UserIdentityId

	if(@LinkedBuildings = 0)
	begin
	  --de register user
	  delete from TempPassword where PasswordControlId in (Select id from PasswordControl where UserIdentityId = @UserIdentityId)
	  delete from PasswordControl where UserIdentityId = @UserIdentityId
	  delete from UserIdentity  where Id = @UserIdentityId
	end


end

Update BuildingUnit set EmailAddress1 = @NewEmail where Id = @UnitId



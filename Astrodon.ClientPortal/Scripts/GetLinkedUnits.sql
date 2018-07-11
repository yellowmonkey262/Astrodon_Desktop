--declare @EmailAddress nvarchar(500)
--declare @BuildingId int

SELECT bu.AccountNumber
from UserIdentity u
join UserBuildingUnit ubu on u.Id = ubu.UserIdentityId
join BuildingUnit bu on bu.Id = ubu.BuildingUnitId
join Building b on b.Id = bu.BuildingId
where u.EmailAddress = @EmailAddress
and b.BuildingId =@BuildingId
order by bu.AccountNumber
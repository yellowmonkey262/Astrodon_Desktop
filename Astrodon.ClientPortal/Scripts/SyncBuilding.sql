--declare @BuildingId int


/*CREATE/UPDATE BUILDINGS*/

insert into Building
(Id,BuildingId,BuildingName,BuildingImage,IsActive)
select NEWID() ,b.id,b.Building,null,
  case b.BuildingDisabled when 0 then 1  else 0 end
from Astrodon..tblBuildings b
left join Building bl on bl.BuildingId = b.id
where bl.Id is null
and b.id = @BuildingId

--update existing
Update Building 
SET Building.BuildingName = b.Building,
    Building.IsActive = case b.BuildingDisabled when 0 then 1  else 0 end
FROM Astrodon..tblBuildings b
JOIN Building bl on bl.BuildingId = b.id
where b.id = @BuildingId


/*COPY BUILDING UNITS*/

insert into BuildingUnit
(Id,UnitId,AccountNumber,BuildingId,IsTrustee)
select NEWID() ,c.Id, c.AccountNumber,b.Id,c.IsTrustee
from Astrodon..Customer c
join Building b on b.BuildingId = c.BuildingId
left join BuildingUnit bu on bu.UnitId = c.id
where bu.Id is null
  and b.BuildingId = @BuildingId

Update BuildingUnit
set BuildingUnit.AccountNumber = c.AccountNumber,
    BuildingUnit.BuildingId = b.Id,
	BuildingUnit.IsTrustee = c.IsTrustee
from Astrodon..Customer c
join Building b on b.BuildingId = c.BuildingId
join BuildingUnit bu on bu.UnitId = c.id
where b.BuildingId = @BuildingId


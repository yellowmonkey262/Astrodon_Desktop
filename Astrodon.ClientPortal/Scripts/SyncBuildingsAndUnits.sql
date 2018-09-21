

/*CREATE/UPDATE BUILDINGS*/

insert into Building
(Id,BuildingId,BuildingName,BuildingImage,IsActive,PortfolioManagerEmailAddress)
select NEWID() ,b.id,b.Building,null,
  case b.BuildingDisabled when 0 then 1  else 0 end,
  b.pm
from Astrodon..tblBuildings b
left join Building bl on bl.BuildingId = b.id
where bl.Id is null

--update existing
Update Building 
SET Building.BuildingName = b.Building,
    Building.IsActive = case b.BuildingDisabled when 0 then 1  else 0 end,
	PortfolioManagerEmailAddress = b.pm
FROM Astrodon..tblBuildings b
JOIN Building bl on bl.BuildingId = b.id


/*COPY BUILDING UNITS*/

insert into BuildingUnit
(Id,UnitId,AccountNumber,BuildingId,IsTrustee,EmailAddress1,EmailAddress2,EmailAddress3,EmailAddress4)
select NEWID() ,c.Id, c.AccountNumber,b.Id,c.IsTrustee,
       c.EmailAddress1,c.EmailAddress2,c.EmailAddress3,c.EmailAddress4
from Astrodon..Customer c
join Building b on b.BuildingId = c.BuildingId
left join BuildingUnit bu on bu.UnitId = c.id
where bu.Id is null


Update BuildingUnit
set BuildingUnit.AccountNumber = c.AccountNumber,
    BuildingUnit.BuildingId = b.Id,
	BuildingUnit.IsTrustee = c.IsTrustee,
	BuildingUnit.EmailAddress1 = c.EmailAddress1,
	BuildingUnit.EmailAddress2 = c.EmailAddress2,
	BuildingUnit.EmailAddress3 = c.EmailAddress3,
	BuildingUnit.EmailAddress4 = c.EmailAddress4
from Astrodon..Customer c
join Building b on b.BuildingId = c.BuildingId
join BuildingUnit bu on bu.UnitId = c.id


/*Link Users*/

insert into UserBuildingUnit
(Id,BuildingUnitId,UserIdentityId)
Select  NEWID(),bu.Id,ui.Id
from BuildingUnit bu
join UserIdentity ui on bu.EmailAddress1 = ui.EmailAddress 
left join UserBuildingUnit uu on bu.Id = uu.BuildingUnitId
          and uu.UserIdentityId = ui.Id
where uu.Id is null
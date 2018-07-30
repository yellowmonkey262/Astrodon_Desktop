/*
declare @BuildingId int
declare @AccountNumber nvarchar(500)
declare @ActiveOnly bit
*/

if(@ActiveOnly = 1)
begin
   select ud.Id,
          ud.DateUploaded as DocumentDate,
		  ud.Title,
		  ud.[FileName] as [File],
		  bu.AccountNumber as AccountNumber,
		  ud.DocumentCategory
   from BuildingUnit bu
   join Building b on b.Id = bu.BuildingId
   join UnitDocument ud on ud.BuildingUnitId = bu.Id
   where b.BuildingId = @BuildingId
	 and bu.AccountNumber = @AccountNumber
	 and ud.IsActive = 1
end
else
begin
   select ud.Id,
          ud.DateUploaded as DocumentDate,
		  ud.Title,
		  ud.[FileName] as [File],
		  bu.AccountNumber as AccountNumber,
		  ud.DocumentCategory
   from BuildingUnit bu
   join Building b on b.Id = bu.BuildingId
   join UnitDocument ud on ud.BuildingUnitId = bu.Id
   where b.BuildingId = @BuildingId
	 and bu.AccountNumber = @AccountNumber
end


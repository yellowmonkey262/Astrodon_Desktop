/*
declare @BuildingId int
declare @ActiveOnly bit
*/

if(@ActiveOnly = 1)
begin
   select ud.Id,
          ud.DateUploaded as DocumentDate,
		  ud.Title,
		  ud.[FileName] as [File],
		  null as AccountNumber
   from Building b 
   join BuildingDocument ud on ud.BuildingId = b.Id
   where b.BuildingId = @BuildingId
	 and ud.IsActive = 1
end
else
begin
   select ud.Id,
          ud.DateUploaded as DocumentDate,
		  ud.Title,
		  ud.[FileName] as [File],
		  null as AccountNumber
  from Building b 
   join BuildingDocument ud on ud.BuildingId = b.Id
   where b.BuildingId = @BuildingId
end


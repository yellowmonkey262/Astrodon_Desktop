
select wd.DocumentData
from BuildingDocument ud
join WebDocument wd on wd.Id = ud.WebDocumentId
join Building b on b.Id = ud.BuildingId
where ud.Id = @BuildingDocumentId
and b.BuildingId = @BuildingId
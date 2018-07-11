
select wd.DocumentData
from UnitDocument ud
join WebDocument wd on wd.Id = ud.WebDocumentId
where ud.Id = @UnitDocumentId
select l.AccessDate,l.AccessToken,u.EmailAddress
from WebDocumentAccessLog l
join UnitDocument ud on ud.WebDocumentId = l.WebDocumentId
left join UserIdentity u on l.UserIdentityId = u.Id 
where ud.Id = @UnitDocumentId
order by l.AccessDate desc
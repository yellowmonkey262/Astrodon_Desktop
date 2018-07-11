--declare @EmailAddress nvarchar(500)

SELECT pw.PasswordHash
from UserIdentity u
join PasswordControl pw on u.Id = pw.UserIdentityId
where EmailAddress = @EmailAddress
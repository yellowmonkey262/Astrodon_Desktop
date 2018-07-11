--declare @BuildingId int
Update Building set IsActive = 0 where BuildingId = @BuildingId

--declare @BuildingId int
--declare @ImageData varbinary(max)

update Building set BuildingImage = @ImageData
where BuildingId = @BuildingId

--declare @BuildingId int
update Building 
  set BuildingImage = null
where BuildingId = @BuildingId



CREATE TABLE [dbo].[VehicleModel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ModelName] VARCHAR(24) NOT NULL,
	[Abbreviation]  VARCHAR(24) NOT NULL,
	[MakeId] INT NOT NULL foreign key references VehicleMake(Id),
)

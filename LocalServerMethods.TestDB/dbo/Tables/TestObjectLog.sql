CREATE TABLE [dbo].[TestObjectLog]
(
	--LOCAL SERVER IDENTITY COLUMNS
	[Id] uniqueidentifier NOT NULL
	,ConflictId uniqueidentifier NULL
	,UpdatedLocally DateTime NOT NULL
	,UpdatedOnServer DateTime NOT NULL
	,UpdatedBy varchar(255) NULL
	,IsActive bit NOT NULL
	--ADDITIONAL COLUMNS
	,FirstName varchar(255) NULL
	,LastName varchar(255) NULL
	,FavouriteDate DateTime NULL
	,FavouriteFoods nvarchar(max) NULL
	,PRIMARY KEY (Id,UpdatedLocally)
)

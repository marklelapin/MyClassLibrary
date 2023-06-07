CREATE PROCEDURE [dbo].[spSaveLocalLastSyncDate]
	@UpdateType varchar(255)
	,@LastSyncDate DateTime2
	
AS
	DELETE 
	FROM dbo.LocalSyncInfo 
	WHERE UpdateType = @UpdateType

	INSERT dbo.LocalSyncInfo
	Values(@UpdateType, @LastSyncDate)

RETURN 0
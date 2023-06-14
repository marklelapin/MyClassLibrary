CREATE PROCEDURE [dbo].[spSaveLocalLastSyncDate]
	@UpdateType varchar(255)
	,@LastSyncDate DateTime2
	
AS
	DELETE l
	FROM dbo.LocalSyncInfo l
	WHERE UpdateType = @UpdateType

	INSERT dbo.LocalSyncInfo
	Values(@UpdateType, @LastSyncDate)

RETURN 0
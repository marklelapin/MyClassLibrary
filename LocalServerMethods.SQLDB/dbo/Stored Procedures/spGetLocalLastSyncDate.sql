CREATE PROCEDURE [dbo].[spGetLocalLastSyncDate]
@UpdateType varchar(255)
,@LastSyncDate DateTime2 OUTPUT
AS
SELECT @LastSyncDate = LastSyncDate
FROM LocalSyncInfo
WHERE UpdateType = @UpdateType;

RETURN;
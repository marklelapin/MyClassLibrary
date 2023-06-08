CREATE PROCEDURE [dbo].[spResetSampleData]
	@Location varchar(255)
	,@SampleUpdates nvarchar(max)
	,@SampleServerSyncLogs nvarchar(max)
	,@UpdateType varchar(255)
	,@Success bit OUTPUT
AS

IF @Location = 'Local'
BEGIN


	DELETE u
	FROM dbo.UpdateLog u
	WHERE u.IsSample = 1
	AND u.UpdateType = @UpdateType

	INSERT dbo.UpdateLog (Id,Created,CreatedBy,UpdatedOnServer,IsConflicted,UpdateType,IsActive,IsSample,JsonUpdate)
	
	SELECT u.Id,u.Created,u.CreatedBy,u.UpdatedOnServer,u.IsConflicted,@UpdateType,u.IsActive,u.IsSample,u.JsonUpdate
	FROM OPENJSON(@SampleUpdates)
	WITH (
    Id uniqueidentifier '$.Id'
    ,Created datetime2 '$.Created'
    ,CreatedBy varchar(255) '$.CreatedBy'
    ,UpdatedOnServer datetime2 '$.UpdatedOnServer'
    ,IsConflicted bit '$.IsConflicted'
    ,IsActive bit '$.IsActive'
	,IsSample bit '$.IsSample'
    ,JsonUpdate nvarchar(max) '$' AS JSON
    ) u
	


END
IF @Location = 'Server'
BEGIN
	
	
	DECLARE @SampleIdAndCreated Table(
	Id uniqueidentifier
	,Created datetime2
	)
	INSERT @SampleIdAndCreated
	SELECT Id,Created
	FROM dbo.UpdateLog 
	WHERE IsSample = 1
	AND UpdateType = @UpdateType
	

	DELETE u
	FROM dbo.UpdateLog u
	WHERE u.IsSample = 1
	AND u.UpdateType = @UpdateType


	INSERT dbo.UpdateLog(Id,Created,CreatedBy,UpdatedOnServer,IsConflicted,UpdateType,IsActive,IsSample,JsonUpdate)
	SELECT u.Id,u.Created,u.CreatedBy,u.UpdatedOnServer,u.IsConflicted,@UpdateType,u.IsActive,u.IsSample,u.JsonUpdate
	FROM OPENJSON(@SampleUpdates)
	WITH (
    Id uniqueidentifier '$.Id'
    ,Created datetime2 '$.Created'
    ,CreatedBy varchar(255) '$.CreatedBy'
    ,UpdatedOnServer datetime2 '$.UpdatedOnServer'
    ,IsConflicted bit '$.IsConflicted'
    ,IsActive bit '$.IsActive'
	,IsSample bit '$.IsSample'
    ,JsonUpdate nvarchar(max) '$' AS JSON
    ) u



	DELETE s
	FROM dbo.ServerSyncLog s
	INNER JOIN @SampleIdAndCreated u
	ON s.UpdateId = u.Id
	AND s.UpdateCreated = u.Created




	INSERT dbo.ServerSyncLog(UpdateId,UpdateCreated,CopyId)
	
	SELECT u.Id,u.Created,u.CopyId
	FROM OPENJSON(@SampleServerSyncLogs)
	WITH (
	Id uniqueidentifier
	,Created datetime2 
	,CopyId uniqueidentifier
	) u
	
	
	



END

Set @Success = 1

RETURN 0

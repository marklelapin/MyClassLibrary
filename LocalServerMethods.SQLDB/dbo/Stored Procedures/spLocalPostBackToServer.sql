CREATE PROCEDURE [dbo].[spLocalPostBackToServer]
	@CopyId uniqueidentifier,
	@PostBack nvarchar(max)
AS
	
	DECLARE @PostBackData TABLE (
		Id uniqueidentifier
		,Created datetime2
		,IsConflicted bit
		)

	INSERT @PostBackData

	SELECT u.*
	FROM OPENJSON(@PostBack) 
	WITH (
	Id uniqueidentifier
	,Created datetime2
	,IsConflicted bit
	) u
	

	INSERT dbo.ServerSyncLog(UpdateId,UpdateCreated,CopyID)
	SELECT p.Id, p.Created, @CopyId
	FROM @PostBackData p

	Update u
	Set u.IsConflicted = 1
	,u.JsonUpdate = JSON_MODIFY(u.JsonUpdate,'$.IsConflicted',p.IsConflicted)
	FROM dbo.UpdateLog u
	INNER JOIN @PostBackData p
	ON u.Id = p.Id
	AND u.Created = p.Created
	WHERE p.IsConflicted = 1

RETURN 0

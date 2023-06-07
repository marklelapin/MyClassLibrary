CREATE PROCEDURE [dbo].[spServerPostBackToLocal]
	@PostBack nvarchar(max)
AS

	WITH CTEPostBackData 
	AS
	(
		SELECT u.Id,u.Created,u.IsConflicted,u.UpdatedOnServer
		FROM OPENJSON(@PostBack) 
		WITH (
		Id uniqueidentifier
		,Created datetime2
		,IsConflicted bit
		,UpdatedOnServer datetime2
		) u
	)

	Update u
	Set u.UpdatedOnServer = p.UpdatedOnServer
	,u.IsConflicted = p.IsConflicted
	,u.JsonUpdate = JSON_MODIFY(
								JSON_MODIFY(
											u.JsonUpdate
											,'$.UpdatedOnServer'
											,CONVERT(VARCHAR,p.UpdatedOnServer,127)
											)
								,'$.IsConflicted'
								,p.IsConflicted
								)
	
	FROM dbo.UpdateLog u
	INNER JOIN CTEPostBackData p
	ON p.Id = u.Id
	AND p.Created = u.Created;

RETURN 0

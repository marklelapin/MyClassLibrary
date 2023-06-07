CREATE FUNCTION [dbo].[itfGetUnsyncedUpdatesFromServer]
(
	@CopyID uniqueidentifier
)
RETURNS TABLE AS RETURN
(

	WITH CTEUnsyncedIds
	AS
	(
	SELECT s.UpdateId
		,s.UpdateCreated
	FROM dbo.ServerSyncLog s
	GROUP BY UpdateId,UpdateCreated
	HAVING SUM (CASE WHEN s.CopyID = @CopyID THEN 1 ELSE 0 END) = 0
	)

	SELECT *
	FROM dbo.UpdateLog u
	INNER JOIN CTEUnsyncedIds s
	ON u.Id = s.UpdateId
	AND u.Created = s.UpdateCreated

)

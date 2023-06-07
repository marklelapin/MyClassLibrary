CREATE FUNCTION [dbo].[itfGetLatestUpdates]
(
	@UpdateType varchar(255)
)
RETURNS TABLE AS RETURN
(
	WITH CTELatestUpdates
	AS
	(Select u.Id
	,Max(Created) as Created
	FROM dbo.UpdateLog u
	WHERE u.UpdateType = @UpdateType
	GROUP BY Id
	)

	SELECT u.*
	FROM itfGetAllUpdates(@UpdateType) u
	INNER JOIN CTELatestUpdates l
	ON l.Id = u.Id
	AND l.Created = u.Created

)

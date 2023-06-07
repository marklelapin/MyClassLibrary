CREATE FUNCTION [dbo].[itfGetAllUpdates]
(
	@UpdateType varchar(255)
)
RETURNS TABLE AS RETURN
(
	SELECT u.Id
		, u.Created
		, u.CreatedBy
		, u.UpdatedOnServer
		, u.IsConflicted
		, u.IsActive
		, u.JsonUpdate
	FROM dbo.UpdateLog u
	WHERE u.UpdateType = @UpdateType
)

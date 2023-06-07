CREATE PROCEDURE [dbo].[spGetUnsyncedFromServer]
	@CopyId uniqueidentifier
	,@UpdateType varchar(255)
	,@Output nvarchar(max) OUTPUT
AS

Set @Output = (
				SELECT JSON_QUERY(u.JsonUpdate) JsonUpdate
				FROM itfGetUnsyncedUpdatesFromServer(@CopyId) u
				WHERE u.UpdateType = @UpdateType
				ORDER BY u.Created DESC
				FOR JSON AUTO
				)
RETURN



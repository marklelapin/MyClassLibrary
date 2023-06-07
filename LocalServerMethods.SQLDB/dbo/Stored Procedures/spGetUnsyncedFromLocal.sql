CREATE PROCEDURE [dbo].[spGetUnsyncedFromLocal]
	@UpdateType varchar(255)
	,@Output nvarchar(max) OUTPUT
AS
	BEGIN

		Set @Output = (
						SELECT JSON_QUERY(u.JsonUpdate) AS JsonUpdate
						FROM dbo.UpdateLog u
						WHERE u.UpdatedOnServer IS NULL
						AND u.UpdateType = @UpdateType
						Order By u.Created DESC
						FOR JSON AUTO
						)

		
	END
RETURN 

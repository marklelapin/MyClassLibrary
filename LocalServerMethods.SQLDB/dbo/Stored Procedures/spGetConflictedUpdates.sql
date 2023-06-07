CREATE PROCEDURE [dbo].[spGetConflictedUpdates]
	@UpdateIds nvarchar(max)
	,@UpdateType varchar(255)
	,@Output nvarchar(max) OUTPUT

AS
	
	IF OBJECT_ID('tempdb..#Ids') IS NOT NULL
	DROP TABLE #Ids

	CREATE TABLE #Ids (
	Id uniqueidentifier
	)
	
	IF ISNULL(@UpdateIds,'') = '' 
		BEGIN
			INSERT #Ids
			SELECT DISTINCT u.Id
			FROM dbo.UpdateLog u
			WHERE u.UpdateType = @UpdateType
		END
	ELSE
		BEGIN
			INSERT #Ids
			SELECT DISTINCT CAST(value AS uniqueidentifier)
			FROM STRING_SPLIT(@UpdateIds,',')
		END;
	
	SET @Output = (SELECT JSON_QUERY(u.JsonUpdate) AS JsonUpdate
					FROM itfGetAllUpdates(@UpdateType) u
					INNER JOIN #Ids i
					On i.Id = u.Id
					WHERE u.IsConflicted = 1
					Order By u.Id,u.Created DESC
					FOR JSON AUTO)
					RETURN 0

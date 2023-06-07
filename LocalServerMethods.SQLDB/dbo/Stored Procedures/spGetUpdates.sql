CREATE PROCEDURE [dbo].[spGetUpdates]
	@LatestOnly bit
	,@UpdateIds nvarchar(max)
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
	
	IF @LatestOnly = 1
	BEGIN
		Set @Output = (SELECT JSON_QUERY(u.JsonUpdate) JsonUpdate
									FROM dbo.itfGetLatestUpdates(@UpdateType) u
									INNER JOIN #Ids i
									ON i.Id = u.Id
									Order By u.Created DESC
									FOR JSON AUTO)
	END;

	IF @LatestOnly = 0
	BEGIN 
		Set @Output = (SELECT JSON_QUERY(u.JsonUpdate) JsonUpdate
						FROM dbo.itfGetAllUpdates(@UpdateType) u
						INNER JOIN #Ids i
						ON i.Id = u.Id
						Order By u.Created DESC
						FOR JSON AUTO
						)
	END

RETURN;
CREATE PROCEDURE [dbo].[spClearConflicts]
	@Ids nvarchar(max)
AS
	IF OBJECT_ID('tempdb..#Ids') IS NOT NULL
	DROP TABLE #Ids

	CREATE TABLE #Ids (
	Id uniqueidentifier
	)
	
	INSERT #Ids
	SELECT DISTINCT CAST(value AS uniqueidentifier)
	FROM STRING_SPLIT(@Ids,',')
	
	Update u
	set u.IsConflicted = 0
	,u.JsonUpdate = JSON_MODIFY(u.JsonUpdate,'$.IsConflicted',CAST(0 as Bit))
	FROM dbo.UpdateLog u
	INNER JOIN #Ids i
	ON u.Id = i.Id

RETURN 0

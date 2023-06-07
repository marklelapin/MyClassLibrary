CREATE PROCEDURE [dbo].[spDeleteUpdates]
	@UpdateType varchar(255),
	@Updates nvarchar(max)
AS
	DECLARE @TempTable Table (
	Id uniqueidentifier
	,Created datetime2
	)

	INSERT @TempTable

	SELECT * FROM OPENJSON(@Updates)
	WITH (
	Id uniqueidentifier
	,Created datetime2
	)

	BEGIN
		DELETE u
		FROM dbo.UpdateLog u
		INNER JOIN @TempTable f
		ON f.Id = u.Id
		and f.Created = u.Created
	END
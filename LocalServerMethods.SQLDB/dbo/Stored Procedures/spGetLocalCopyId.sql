CREATE PROCEDURE [dbo].[spGetLocalCopyId]
	@CopyID uniqueidentifier OUTPUT
AS

	IF (SELECT COUNT(1) FROM dbo.LocalCopyId) = 0 
	BEGIN
		INSERT dbo.LocalCopyId (Id)
		SELECT NEWID()
	END;

	SET @CopyID = (SELECT Top 1 Id FROM dbo.LocalCopyId)

RETURN

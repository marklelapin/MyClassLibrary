CREATE PROCEDURE [dbo].[spSaveUpdatesToServer]
	@CopyID uniqueidentifier
    ,@Updates nvarchar(max)
    ,@UpdateType varchar(255)
    ,@PostBack nvarchar(max) OUTPUT
AS

--Extract @Updates into table variable
BEGIN 
DECLARE @NewUpdates Table(
Id uniqueidentifier
    ,Created datetime2
    ,CreatedBy varchar(255)
    ,UpdatedOnServer datetime2
    ,IsConflicted bit
    ,IsActive bit
    ,IsSample bit
    ,JsonUpdate nvarchar(max)
    ,PRIMARY KEY (Id,Created)
)

INSERT @NewUpdates(Id,Created,CreatedBy,UpdatedOnServer,IsConflicted,IsActive,IsSample,JsonUpdate)
SELECT u.* FROM OPENJSON(@Updates)
WITH (
    Id uniqueidentifier '$.Id'
    ,Created datetime2 '$.Created'
    ,CreatedBy varchar(255) '$.CreatedBy'
    ,UpdatedOnServer datetime2 '$.UpdatedOnServer'
    ,IsConflicted bit '$.IsConflicted'
    ,IsActive bit '$.IsActive'
    ,IsSample bit '$.IsSample'
    ,JsonUpdate nvarchar(max) '$' AS JSON
    ) u

DECLARE @ConflictedIds Table (
Id uniqueidentifier PRIMARY KEY
)
END

 --Identify Conflicts
 BEGIN
    WITH CTENewUpdateIds
    AS
    (
    SELECT DISTINCT Id
    FROM @NewUpdates
    )
     
    INSERT @ConflictedIds(Id)
    SELECT DISTINCT u.Id
    FROM itfGetUnsyncedUpdatesFromServer(@CopyID) u
    INNER JOIN CTENewUpdateIds n
    ON u.id = n.Id
END

BEGIN TRY
BEGIN TRANSACTION
   
   DECLARE @UpdatedOnServer datetime2 = SYSDATETIME()

    --Mark up new Updates as Conflicted
    Update n
    Set n.IsConflicted = 1
        ,n.JsonUpdate =JSON_MODIFY(n.JsonUpdate,'$.IsConflicted',CAST(1 as bit))
    FROM @NewUpdates n
    INNER JOIN @ConflictedIds c
    ON n.Id = c.Id;


    --Mark Up existing Updates as Conflicted
    Update e
    Set e.IsConflicted = 1
    ,e.JsonUpdate = JSON_MODIFY(e.JsonUpdate,'$.IsConflicted',CAST(1 as bit))
    FROM dbo.UpdateLog e
    INNER JOIN @ConflictedIds c
    On e.Id = c.Id
    AND e.UpdatedOnServer IS NULL


    --Save To ServerSyncLog
    INSERT dbo.ServerSyncLog(UpdateId,UpdateCreated,CopyID)
    SELECT u.Id
            ,u.Created
            ,@CopyID
    FROM @NewUpdates u
    LEFT JOIN dbo.ServerSyncLog s
    ON s.UpdateId = u.Id
    AND s.UpdateCreated= u.Created
    AND s.CopyID = @CopyID
    WHERE s.UpdateId IS NULL




    --Insert New Updates into Update Table
    INSERT dbo.UpdateLog (Id,Created,CreatedBy,UpdatedOnServer,IsConflicted,UpdateType,IsActive,IsSample,JsonUpdate)
    SELECT n.Id
        ,n.Created
        ,n.CreatedBy
        ,@UpdatedOnServer as UpdatedOnServer
        ,n.IsConflicted
        ,@UpdateType
        ,n.IsActive
        ,n.IsSample
        ,n.JsonUpdate
    FROM @NewUpdates n
    LEFT JOIN @ConflictedIds c
    ON c.Id = n.Id
    LEFT JOIN dbo.UpdateLog e --prevents duplicate updates being performed.
    ON e.Id = n.Id
    AND e.Created = n.Created
    WHERE e.Id IS NULL; --------------------

    --Update UpdatedOnServer in JsonUpdate column
    Update u
    set u.JsonUpdate = JSON_MODIFY(u.JsonUpdate,'$.UpdatedOnServer',CONVERT(VARCHAR,@UpdatedOnServer,127))
    FROM dbo.UpdateLog u
    INNER JOIN @NewUpdates n
    ON u.Id = n.Id
    AND u.Created = n.Created
    WHERE u.UpdatedOnServer = @UpdatedOnServer






    SET @PostBack = (
                            SELECT u.Id,u.Created,u.IsConflicted,u.UpdatedOnServer
                            FROM dbo.UpdateLog u
                            INNER JOIN @NewUpdates n
                            ON u.Id = n.Id
                            AND u.Created = n.Created
                            
                            FOR JSON AUTO
                            )

 COMMIT
END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK

END CATCH

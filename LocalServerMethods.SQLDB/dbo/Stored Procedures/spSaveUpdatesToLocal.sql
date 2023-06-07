CREATE PROCEDURE [dbo].[spSaveUpdatesToLocal]
	@Updates nvarchar(max)
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
    INSERT @ConflictedIds (Id)
    
    SELECT n.Id
    FROM @NewUpdates n
    INNER JOIN dbo.UpdateLog e --existing
    ON n.Id = e.Id
    WHERE n.UpdatedOnServer IS NOT NULL --the newUpdates are coming from the server 
    AND e.UpdatedOnServer IS NULL --the existingUpdates have not yet been updated to Server.

    UNION
    --already known conflicts
    SELECT n.Id
    FROM @NewUpdates n
    WHERE n.IsConflicted = 1
END

BEGIN TRY
BEGIN TRANSACTION
   
    --Mark up new Updates as Conflicted
    Update n
    Set n.IsConflicted = 1
        ,n.JsonUpdate =JSON_MODIFY(n.JsonUpdate,'$.IsConflicted',CAST(1 as Bit))
    FROM @NewUpdates n
    INNER JOIN @ConflictedIds c
    ON n.Id = c.Id;


    --Mark Up existing Updates as Conflicted
    Update e
    Set e.IsConflicted = 1
    ,e.JsonUpdate = JSON_MODIFY(e.JsonUpdate,'$.IsConflicted',CAST(1 as Bit))
    FROM dbo.UpdateLog e
    INNER JOIN @ConflictedIds c
    On e.Id = c.Id
    AND e.UpdatedOnServer IS NULL


    --Insert New Updates into Update Table
    INSERT dbo.UpdateLog (Id,Created,CreatedBy,UpdatedOnServer,IsConflicted,UpdateType,IsActive,IsSample,JsonUpdate)
    SELECT n.Id
        ,n.Created
        ,n.CreatedBy
        ,n.UpdatedOnServer
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

    SET @PostBack = (
                            SELECT u.Id,u.Created,u.IsConflicted
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



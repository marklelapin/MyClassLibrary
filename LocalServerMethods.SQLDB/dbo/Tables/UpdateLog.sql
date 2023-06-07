CREATE TABLE [dbo].[UpdateLog] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Created]  DATETIME2         NOT NULL,
    [CreatedBy]       VARCHAR (255)    NULL,
    [UpdatedOnServer] DATETIME2         NULL,
    [IsConflicted]      bit NOT NULL,
    UpdateType      varchar(255) NOT NULL,
    [IsActive]        BIT              NOT NULL,
    [JsonUpdate]          nvarchar(max),
    [IsSample] BIT NULL DEFAULT 0
    ,PRIMARY KEY CLUSTERED ([Id] ASC, [Created] ASC)
    
);


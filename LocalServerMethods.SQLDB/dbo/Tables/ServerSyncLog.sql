CREATE TABLE [dbo].[ServerSyncLog]
(
	UpdateId uniqueidentifier NOT NULL
	,UpdateCreated datetime2 NOT NULL
	,[CopyId] uniqueidentifier NOT NULL
	,PRIMARY KEY (UpdateId,UpdateCreated,[CopyId])
)

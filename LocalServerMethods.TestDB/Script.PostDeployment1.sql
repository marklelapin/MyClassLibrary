/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DELETE 
FROM dbo.TestObjectLog

INSERT dbo.TestObjectLog (Id,ConflictId,UpdatedLocally,UpdatedOnServer,UpdatedBy,IsActive,FirstName,LastName,FavouriteDate,FavouriteFoods)

VALUES ('18cc44a2-be63-4515-b8aa-da33cd97bb55',null,'2023-03-01 09:00','2023-03-01 09:02','mcarter',1,'Mark','Carter','1978-07-02','Burger,Chicken,Chocolate')
,('089bb18d-4589-4bc2-a2ef-2aa2a88ea97b',null,'2023-03-02 09:00','2023-03-02 09:02','mcarter',1,'Bob','Hoskins','1934-05-02','Chips,Strawberries')
,('089bb18d-4589-4bc2-a2ef-2aa2a88ea97b',null,'2023-03-01 09:00','2023-03-01 09:02','mcarter',1,'Bob','Hoskins','1934-05-02','Chips')
,('18cc44a2-be63-4515-b8aa-da33cd97bb55',null,'2023-04-01 09:00','2023-04-01 09:02','mcarter',1,'Mark','Carter','1978-07-02','Burger,Chicken,Chocolate,Lindor Balls')
,('4752b19a-ce56-410f-80ed-9064f2260ce3',null,'2023-04-01 22:00','2023-04-02 05:02','mcarter',1,'Jim','Broadbent','2010-06-04','Peas,Carrots')
,('089bb18d-4589-4bc2-a2ef-2aa2a88ea97b',null,'2023-04-01 09:00','2023-04-01 09:02','mcarter',1,'Bob','Hoskins','1956-12-24','Chips,Strawberries,Tiramisu')
,('f3810060-7958-4744-9b0c-62c65103494b',null,'2023-03-01 09:00','2023-03-01 09:02','mcarter',1,'Tracey','Emin','1999-12-31','Cake')
,('f3810060-7958-4744-9b0c-62c65103494b',null,'2023-04-04 09:00','2023-04-04 09:02','mcarter',0,'Tracey','Emin','1999-12-31','Cake')
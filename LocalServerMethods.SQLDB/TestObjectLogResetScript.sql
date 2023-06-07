DELETE FROM dbo.TestObjectLog
WHERE isActive is NOT NULL


insert dbo.TestObjectLog (Id,ConflictID,UpdatedLocally,UpdatedOnServer,UpdatedBy,IsActive,FirstName,LastName,FavouriteDate,FavouriteFoods)

Values ('089BB18D-4589-4BC2-A2EF-2AA2A88EA97B',	NULL,'2023-03-01 09:00:00.000','2023-03-01 09:02:00.000','mcarter',1,'Bob','Hoskins','1934-05-02 00:00:00.000','Chips')
,('089BB18D-4589-4BC2-A2EF-2AA2A88EA97B',NULL,'2023-03-02 09:00:00.000','2023-03-02 09:02:00.000','mcarter',1,'Bob','Hoskins','1934-05-02 00:00:00.000','Chips,Strawberries')
,('089BB18D-4589-4BC2-A2EF-2AA2A88EA97B',NULL,'2023-04-01 09:00:00.000','2023-04-01 09:02:00.000','mcarter',1,'Bob','Hoskins','1956-12-24 00:00:00.000','Chips,Strawberries,Tiramisu')
,('F3810060-7958-4744-9B0C-62C65103494B',NULL,'2023-03-01 09:00:00.000','2023-03-01 09:02:00.000','mcarter',1,'Tracey','Emin','1999-12-31 00:00:00.000','Cake')
,('F3810060-7958-4744-9B0C-62C65103494B',NULL,'2023-04-04 09:00:00.000','2023-04-04 09:02:00.000','mcarter',0,'Tracey','Emin','1999-12-31 00:00:00.000','Cake')
,('4752B19A-CE56-410F-80ED-9064F2260CE3',NULL,'2023-04-01 22:00:00.000','2023-04-02 05:02:00.000','mcarter',1,'Jim','Broadbent','2010-06-04 00:00:00.000','Peas,Carrots')
,('18CC44A2-BE63-4515-B8AA-DA33CD97BB55',NULL,'2023-03-01 09:00:00.000','2023-03-01 09:02:00.000','mcarter',1,'Mark','Carter','1978-07-02 00:00:00.000','Burger,Chicken,Chocolate')
,('18CC44A2-BE63-4515-B8AA-DA33CD97BB55',NULL,'2023-04-01 09:00:00.000','2023-04-01 09:02:00.000','mcarter',1,'Mark','Carter','1978-07-02 00:00:00.000','Burger,Chicken,Chocolate,Lindor Balls')

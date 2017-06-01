CREATE TABLE [dbo].[Table]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [Subject] NVARCHAR(50) NOT NULL, 
    [Description] NCHAR(10) NOT NULL, 
    [StartTime] TIME NOT NULL, 
    [EndTime] TIME NOT NULL 
)

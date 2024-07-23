CREATE TABLE [dbo].[Products]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL,
    [RetailPrice] MONEY NOT NULL,
    [Qty] INT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [LastModified] DATETIME2 NOT NULL DEFAULT getutcdate()
)

CREATE TABLE [dbo].[Sales]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [SaleDate] DATETIME2 NOT NULL, 
    [Subtotal] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL, 
    [Total] MONEY NOT NULL
)

CREATE TABLE [dbo].[Inventory]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [ProductId] INT NOT NULL, 
    [Cost] MONEY NOT NULL, 
    [Qty] INT NOT NULL DEFAULT 1, 
    [PurchaseDate] DATETIME2 NOT NULL DEFAULT getutcdate()
)

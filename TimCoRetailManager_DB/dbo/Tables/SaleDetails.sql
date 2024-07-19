CREATE TABLE [dbo].[SaleDetails]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [SaleId] INT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [Qty] INT NOT NULL DEFAULT 1,
    [SellingPrice] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL DEFAULT 0 
)

CREATE TABLE [dbo].[SaleDetails]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [SaleId] INT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [Qty] INT NOT NULL DEFAULT 1,
    [SellingPrice] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_SaleDetails_Sales] FOREIGN KEY (SaleId) REFERENCES Sales(Id), 
    CONSTRAINT [FK_SaleDetails_Products] FOREIGN KEY (ProductId) REFERENCES Products(Id) 
)

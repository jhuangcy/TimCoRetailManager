CREATE PROCEDURE [dbo].[sp_AddSaleDetail]
	@SaleId int,
	@ProductId int,
	@Qty int,
	@SellingPrice money,
	@Tax money
AS
begin
	set nocount on;

	insert into dbo.SaleDetails (SaleId, ProductId, Qty, SellingPrice, Tax) 
	values (@SaleId, @ProductId, @Qty, @SellingPrice, @Tax);
end

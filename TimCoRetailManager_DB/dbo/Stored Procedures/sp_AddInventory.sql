CREATE PROCEDURE [dbo].[sp_AddInventory]
	@ProductId int,
	@Cost money,
	@Qty int,
	@PurchaseDate datetime2
AS
begin
	set nocount on;

	insert into dbo.Inventory (ProductId, Cost, Qty, PurchaseDate) 
	values (@ProductId, @Cost, @Qty, @PurchaseDate);
end

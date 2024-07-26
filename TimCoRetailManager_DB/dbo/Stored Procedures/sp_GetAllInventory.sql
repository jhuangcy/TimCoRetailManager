CREATE PROCEDURE [dbo].[sp_GetAllInventory]
AS
begin
	set nocount on;

	SELECT ProductId, Cost, Qty, PurchaseDate 
	from dbo.Inventory;
end

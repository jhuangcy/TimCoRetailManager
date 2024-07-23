CREATE PROCEDURE [dbo].[sp_GetAllProducts]
AS
begin
	set nocount on;

	SELECT Id, [Name], [Description], RetailPrice, Qty
	from dbo.Products
	order by Name
end

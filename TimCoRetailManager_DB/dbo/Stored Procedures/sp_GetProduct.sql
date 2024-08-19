CREATE PROCEDURE [dbo].[sp_GetProduct]
	@Id int
AS
begin
	set nocount on;
	
	SELECT Id, [Name], [Description], RetailPrice, Qty, Taxable, [Image]
	from dbo.Products
	where Id = @Id;
end

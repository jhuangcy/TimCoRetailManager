CREATE PROCEDURE [dbo].[sp_GetSale]
	@UserId nvarchar(128),
	@SaleDate datetime2
AS
begin
	set nocount on;

	SELECT Id from dbo.Sales
	where UserId = @UserId
	and SaleDate = @SaleDate;
end

CREATE PROCEDURE [dbo].[sp_AddSale]
	@Id int output,
	@UserId nvarchar(128),
	@SaleDate datetime2,
	@Subtotal money,
	@Tax money,
	@Total money
AS
begin
	set nocount on;

	insert into dbo.Sales (UserId, SaleDate, Subtotal, Tax, Total) 
	values (@UserId, @SaleDate, @Subtotal, @Tax, @Total);

	select @Id = scope_identity();
end

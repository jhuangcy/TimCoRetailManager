CREATE PROCEDURE [dbo].[sp_GetSalesWithUsers]
AS
begin
	set nocount on;

	SELECT s.SaleDate, s.Subtotal, s.Tax, s.Total, u.FirstName, u.LastName, u.Email
	from dbo.Sales s inner join dbo.Users u
	on s.UserId = u.IdentityUserId;
end

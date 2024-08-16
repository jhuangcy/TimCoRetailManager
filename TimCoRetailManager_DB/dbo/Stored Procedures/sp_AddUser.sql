CREATE PROCEDURE [dbo].[sp_AddUser]
	@IdentityUserId nvarchar(128),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(256)
AS
begin
	set nocount on;

	insert into dbo.Users (IdentityUserId, FirstName, LastName, Email)
	values (@IdentityUserId, @FirstName, @LastName, @Email)
end

CREATE PROCEDURE [dbo].[sp_GetUser]
	@Id nvarchar(128)
AS
begin
	set nocount on;

	SELECT * from dbo.Users
	where IdentityUserId = @Id;
end

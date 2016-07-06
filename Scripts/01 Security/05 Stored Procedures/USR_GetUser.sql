CREATE PROCEDURE USR_GetUser
	@UserAccountID int
AS

SELECT UserAccountID, DomainName, Firstname, Lastname
FROM UserAccounts
WHERE UserAccountID = @UserAccountID


CREATE PROCEDURE QA_GetQuestion
	@DomainName nvarchar(255)
AS

IF NOT EXISTS(SELECT * FROM UserAccounts WHERE DomainName = @DomainName)
BEGIN
	INSERT INTO UserAccounts(DomainName) VALUES (@DomainName)
END

SELECT UserAccountID, DomainName, Firstname, Lastname
FROM UserAccounts
WHERE DomainName = @DomainName


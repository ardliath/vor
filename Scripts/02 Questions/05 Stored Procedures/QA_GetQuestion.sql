CREATE PROCEDURE QA_GetQuestion
	@QuestionID int
AS
SELECT QuestionID, Text, Type From Questions where QuestionID = @QuestionID

SELECT UserAccountID, DomainName, Firstname, Lastname
FROM UserAccounts u
JOIN Questions q on q.CreatedBy = u.UserAccountID and q.QuestionID = @QuestionID

SELECT o.OptionID, o.Text, o.IsCorrect
FROM Options o
JOIN Questions q on q.QuestionID = o.QuestionID and q.QuestionID = @QuestionID
ORDER BY Ordinal
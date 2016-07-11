CREATE PROCEDURE QA_GetQuiz
	@QuizID int
AS

SELECT QuizID, Title, StartDate, EndDate
FROM Quiz
WHERE QuizID = @QuizID

SELECT q.QuestionID, q.Text, q.Type, q.CreatedBy
FROM Questions q
JOIN QuizQuestions qq on q.QuestionID = qq.QuestionID and qq.QuizID = @QuizID
ORDER BY qq.Ordinal

SELECT DISTINCT u.UserAccountID, u.DomainName, u.Firstname, u.Lastname
FROM UserAccounts u
JOIN Questions q on u.UserAccountID = q.CreatedBy
JOIN QuizQuestions qq on q.QuestionID = qq.QuestionID and qq.QuizID = @QuizID

SELECT o.OptionID, q.QuestionID, o.Text, o.IsCorrect
FROM Options o
JOIN Questions q on o.QuestionID = q.QuestionID
JOIN QuizQuestions qq on q.QuestionID = qq.QuestionID and qq.QuizID = @QuizID
ORDER BY o.Ordinal
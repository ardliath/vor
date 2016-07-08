CREATE PROCEDURE QA_GetOrCreateExam
	@QuizID int,
	@UserAccountID int,
	@Now datetime,
	@FirstQuestion int
AS

IF NOT EXISTS(SELECT * FROM Exams WHERE QuizID = @QuizID AND UserAccountID = @UserAccountID AND EndDate IS NULL)
BEGIN
	INSERT INTO Exams(QuizID, UserAccountID, QuestionID, StartDate) VALUES (@QuizID, @UserAccountID, @FirstQuestion, @Now)
END

SELECT ExamID, QuizID, UserAccountID, QuestionID, StartDate, EndDate, Score
FROM Exams
WHERE QuizID = @QuizID AND UserAccountID = @UserAccountID AND EndDate IS NULL


SELECT ao.AnswerID, ao.QuestionID, ao.OptionID
FROM AnswerOptions ao
JOIN Answers a on ao.AnswerID = a.AnswerID
JOIN Exams e on a.ExamID = e.ExamID
AND QuizID = @QuizID AND UserAccountID = @UserAccountID AND EndDate IS NULL
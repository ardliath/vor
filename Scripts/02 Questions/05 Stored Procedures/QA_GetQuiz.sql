CREATE PROCEDURE QA_GetQuiz
	@QuizID int
AS
SELECT QuizID, Title, StartDate, EndDate
FROM Quiz
WHERE QuizID = @QuizID
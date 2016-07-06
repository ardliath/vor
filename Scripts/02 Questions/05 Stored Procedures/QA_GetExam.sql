CREATE PROCEDURE QA_GetExam
	@ExamID int
AS

SELECT ExamID, QuizID, UserAccountID, QuestionID, StartDate, EndDate, Score
FROM Exams
WHERE ExamID = @ExamID
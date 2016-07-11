CREATE PROCEDURE QA_GetExam
	@ExamID int
AS

SELECT ExamID, QuizID, UserAccountID, QuestionID, StartDate, EndDate, Score
FROM Exams
WHERE ExamID = @ExamID

SELECT ao.AnswerID, ao.QuestionID, ao.OptionID
FROM AnswerOptions ao
JOIN Answers a on ao.AnswerID = a.AnswerID
WHERE a.ExamID = @ExamID
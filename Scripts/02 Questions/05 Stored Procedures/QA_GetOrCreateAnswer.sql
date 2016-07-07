CREATE PROC QA_GetOrCreateAnswer
	@QuestionID int,
    @ExamID int
AS
	IF NOT EXISTS(SELECT * FROM Answers WHERE QuestionID = @QuestionID AND ExamID = @ExamID)
	BEGIN
		INSERT INTO Answers(ExamID, QuestionID) VALUES (@ExamID, @QuestionID)
	END

SELECT AnswerID, ExamID, QuestionID FROM Answers WHERE QuestionID = @QuestionID AND ExamID = @ExamID

SELECT ao.AnswerID, ao.OptionID, ao.QuestionID FROM AnswerOptions ao
JOIN Answers a on ao.AnswerID = a.AnswerID
WHERE a.QuestionID = @QuestionID AND a.ExamID = @ExamID
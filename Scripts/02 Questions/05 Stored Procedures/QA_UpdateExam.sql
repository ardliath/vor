CREATE PROC QA_UpdateExam
	@ExamID int,
    @CurrentQuestionID int,
    @EndDate DateTime,
    @Score int
AS

UPDATE Exams SET
	QuestionID = @CurrentQuestionID,
	EndDate = @EndDate,
	Score = @Score
WHERE ExamID = @ExamID
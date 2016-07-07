CREATE PROC QA_ClearOptionsFromAnswer
	@AnswerID int
AS

DELETE FROM AnswerOptions WHERE AnswerID = @AnswerID
CREATE PROC QA_AddOptionToAnswer
	@AnswerID int,
    @OptionID int,
    @QuestionID int
AS

INSERT INTO AnswerOptions (AnswerID, OptionID, QuestionID) VALUES (@AnswerID, @OptionID, @QuestionID)
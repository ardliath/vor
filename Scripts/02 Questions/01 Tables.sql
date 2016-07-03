create table Questions
(
	QuestionID int not null identity(1,1),
	CreatedBy int not null,
	Text nvarchar(1024) not null,
	Type varchar(16) not null,
	constraint PK_Questions primary key (QuestionID),
	constraint FK_Questions_CreatedBy foreign key  (CreatedBy) references UserAccounts(UserAccountID)
)

GO

CREATE TABLE Options
(
	OptionID int identity(1,1) not null,
	QuestionID int not null,
	Text nvarchar(1024) not null,
	IsCorrect bit not null,
	constraint PK_Options primary key (OptionID),
	constraint FK_Options_Question foreign key (QuestionID) references Questions(QuestionID)
)
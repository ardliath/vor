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
	Ordinal smallint not null,
	constraint PK_Options primary key (OptionID),
	constraint FK_Options_Question foreign key (QuestionID) references Questions(QuestionID)
)

GO

create table Quiz
(
	QuizID int identity(1,1) not null,
	Title nvarchar(255) not null,
	StartDate DateTime null,
	EndDate DateTime null,
	constraint PK_Quiz primary key (QuizID)
)

GO

create table QuizQuestions
(
	QuizID int not null,
	QuestionID int not null,
	Ordinal smallint not null,
	constraint PK_QuizQuestions primary key (QuizID, QuestionID),
	constraint FK_QuizQuestions_Quiz foreign key (QuizID) references Quiz(QuizID),
	constraint FK_QuizQuestions_Question foreign key (QuestionID) references Questions(QuestionID),
)

GO

create table Exams
(
	ExamID int not null identity(1,1),
	QuizID int not null,
	UserAccountID int not null,
	QuestionID int not null,
	StartDate datetime not null,
	EndDate datetime null,
	Score int null,
	constraint PK_Exams primary key (ExamID),
	constraint FK_Exams_Quiz foreign key (QuizID) references Quiz(QuizID),
	constraint FK_Exams_Question foreign key (QuestionID) references Questions(QuestionID),
	constraint FK_Exams_UserAccounts foreign key (UserAccountID) references UserAccounts(UserAccountID)
)
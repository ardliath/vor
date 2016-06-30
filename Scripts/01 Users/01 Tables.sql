create table UserAccounts
(
	UserAccountID int not null identity(1,1),
	DomainName nvarchar(255) not null,
	Firstname nvarchar(64) null,
	Lastname nvarchar(64) null
	constraint PK_UserAccounts primary key (UserAccountID),
	constraint UQ_Username_DomainName unique(DomainName)
)
CREATE TABLE tblUserAccountProfileLink
(
	UserAccountProfileLink int IDENTITY(1,1) not null,
	UserID int not null,
	StaffID int  not null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)
GO


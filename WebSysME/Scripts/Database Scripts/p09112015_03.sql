CREATE TABLE tblDocumentPermissions
(
	DocumentPermissionID int IDENTITY(1,1) not null,
	DocumentID int not null,
	LevelOfSecurityID int  not null,
	ObjectID int not null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)
GO

alter table tblFiles
add ApplySecurity bit default 0
GO

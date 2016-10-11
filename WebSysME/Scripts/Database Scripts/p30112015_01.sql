CREATE TABLE tblGroupComitteeAttendants
(
	GroupComitteeAttendantID int IDENTITY(1,1) not null,
	GroupID int not null,
	AttendantID int not null,
	AttendantTypeID int null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)

GO


CREATE PROCEDURE [dbo].[sp_Save_GroupComitteAttendants] 
( 
	@GroupComitteeAttendantID int = null output,  
	@GroupID int = null, 
	@AttendantID int = null, 
	@AttendantTypeID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @GroupComitteeAttendantID IS NULL OR @GroupComitteeAttendantID <= 0 
	BEGIN 
		 
		SELECT @GroupComitteeAttendantID = NULL 
 
		INSERT INTO [tblGroupComitteeAttendants](
			[GroupID], 
			[AttendantID], 
			[AttendantTypeID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@GroupID, 
			@AttendantID, 
			@AttendantTypeID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblGroupComitteeAttendants] SET  
			[GroupID]=@GroupID, 
			[AttendantID]=@AttendantID, 
			[AttendantTypeID]=@AttendantTypeID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [GroupComitteeAttendantID]=@GroupComitteeAttendantID 
	END 
 
	SELECT @GroupComitteeAttendantID = ISNULL(@GroupComitteeAttendantID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @GroupComitteeAttendantID AS ReturnID) AS GroupComitteAttendantsReturnTable 
 
	RETURN @GroupComitteeAttendantID 
 
END 
GO

CREATE TABLE tblGroupAssociations
(
	GroupAssociationID int IDENTITY(1,1) not null,
	Association  varchar(255) null,
	Description varchar(MAX) null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)
 GO

CREATE TABLE tblGroupAssociationGroups
(
	GroupAssociationGroupID int IDENTITY(1,1) not null,
	GroupAssociationID int not null,
	GroupID int not null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)

GO


CREATE PROCEDURE [dbo].[sp_Save_GroupAssociations] 
( 
	@GroupAssociationID int = null output,  
	@UpdatedBy int = null, 
	@Association varchar(255) = null, 
	@Description varchar(MAX) = null
) 
AS 
BEGIN 
 
	IF @GroupAssociationID IS NULL OR @GroupAssociationID <= 0 
	BEGIN 
		 
		SELECT @GroupAssociationID = NULL 
 
		INSERT INTO [tblGroupAssociations](
			[Association], 
			[Description], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@Association, 
			@Description, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblGroupAssociations] SET  
			[UpdatedBy]=@UpdatedBy, 
			[Association]=@Association, 
			[Description]=@Description, 
			[UpdatedDate] = getdate()

		WHERE [GroupAssociationID]=@GroupAssociationID 
	END 
 
	SELECT @GroupAssociationID = ISNULL(@GroupAssociationID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @GroupAssociationID AS ReturnID) AS GroupAssociationsReturnTable 
 
	RETURN @GroupAssociationID 
 
END 
 
GO


CREATE PROCEDURE [dbo].[sp_Save_GroupAssociationsGroups] 
( 
	@GroupAssociationGroupID int = null output,  
	@GroupAssociationID int = null, 
	@GroupID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @GroupAssociationGroupID IS NULL OR @GroupAssociationGroupID <= 0 
	BEGIN 
		 
		SELECT @GroupAssociationGroupID = NULL 
 
		INSERT INTO [tblGroupAssociationGroups](
			[GroupAssociationID], 
			[GroupID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@GroupAssociationID, 
			@GroupID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblGroupAssociationGroups] SET  
			[GroupAssociationID]=@GroupAssociationID, 
			[GroupID]=@GroupID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [GroupAssociationGroupID]=@GroupAssociationGroupID 
	END 
 
	SELECT @GroupAssociationGroupID = ISNULL(@GroupAssociationGroupID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @GroupAssociationGroupID AS ReturnID) AS GroupAssociationsGroupsReturnTable 
 
	RETURN @GroupAssociationGroupID 
 
END 
 
GO
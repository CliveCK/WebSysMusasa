Alter table [tblStaffMembers]
add CellphoneNo varchar(255)
GO

Alter table [tblStaffMembers]
add tempcol varchar(255)
GO

update [tblStaffMembers] set tempcol = ContactNo
GO

Alter table [tblStaffMembers]
drop column ContactNo
GO

Alter table [tblStaffMembers]
add ContactNo varchar(255)
GO

update [tblStaffMembers] set ContactNo = tempcol
GO


Alter table [tblStaffMembers]
drop column tempcol
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Save_StaffMember] 
( 
	@StaffID int = null output,  
	@OrganizationID int = null, 
	@ContactNo varchar(255) = null, 
	@CellphoneNo varchar(255) = null,
	@UpdatedBy int = null, 
	@Name varchar(150) = null, 
	@FirstName varchar(150) = null, 
	@Surname varchar(150) = null, 
	@Sex varchar(50) = null, 
	@PositionID int = null, 
	@Address varchar(255) = null, 
	@EmailAddress varchar(150) = null
) 
AS 
BEGIN 
 
	IF @StaffID IS NULL OR @StaffID <= 0 
	BEGIN 
		 
		SELECT @StaffID = NULL 
 
		INSERT INTO [tblStaffMembers](
			[OrganizationID], 
			[ContactNo], 
			[CellphoneNo],
			[Name], 
			[FirstName], 
			[Surname], 
			[Sex], 
			[PositionID], 
			[Address], 
			[EmailAddress], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@OrganizationID, 
			@ContactNo,
			@CellphoneNo, 
			@Name, 
			@FirstName, 
			@Surname, 
			@Sex, 
			@PositionID, 
			@Address, 
			@EmailAddress, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblStaffMembers] SET  
			[OrganizationID]=@OrganizationID, 
			[ContactNo]=@ContactNo, 
			[CellphoneNo]=@CellphoneNo,
			[UpdatedBy]=@UpdatedBy, 
			[Name]=@Name, 
			[FirstName]=@FirstName, 
			[Surname]=@Surname, 
			[Sex]=@Sex, 
			[PositionID]=@PositionID, 
			[Address]=@Address, 
			[EmailAddress]=@EmailAddress, 
			[UpdatedDate] = getdate()

		WHERE [StaffID]=@StaffID 
	END 
 
	SELECT @StaffID = ISNULL(@StaffID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @StaffID AS ReturnID) AS StaffMemberReturnTable 
 
	RETURN @StaffID 
 
END 
 
GO

--======================================================================================================================

Alter table tblHealthCenters
add HasMotherShelter bit default 0
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Save_HealthCenter] 
( 
	@HealthCenterID int = null output,  
	@WardID int = null, 
	@HealthCenterTypeID int = null, 
	@NoOfDoctors int = null, 
	@NoOfNurses int = null, 
	@NoOfBeds int = null, 
	@UpdatedBy int = null, 
	@HasAmbulance bit = null, 
	@HasMotherShelter bit = null,
	@Longitude numeric(18,6) = null, 
	@Latitude numeric(18,6) = null, 
	@Name varchar(150) = null, 
	@Description varchar(255) = null
) 
AS 
BEGIN 
 
	IF @HealthCenterID IS NULL OR @HealthCenterID <= 0 
	BEGIN 
		 
		SELECT @HealthCenterID = NULL 
 
		INSERT INTO [tblHealthCenters](
			[WardID], 
			[HealthCenterTypeID], 
			[NoOfDoctors], 
			[NoOfNurses], 
			[NoOfBeds], 
			[HasAmbulance],
			[HasMotherShelter],
			[Longitude], 
			[Latitude], 
			[Name], 
			[Description], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@WardID, 
			@HealthCenterTypeID, 
			@NoOfDoctors, 
			@NoOfNurses, 
			@NoOfBeds, 
			@HasAmbulance, 
			@HasMotherShelter,
			@Longitude, 
			@Latitude, 
			@Name, 
			@Description, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblHealthCenters] SET  
			[WardID]=@WardID, 
			[HealthCenterTypeID]=@HealthCenterTypeID, 
			[NoOfDoctors]=@NoOfDoctors, 
			[NoOfNurses]=@NoOfNurses, 
			[NoOfBeds]=@NoOfBeds, 
			[UpdatedBy]=@UpdatedBy, 
			[HasAmbulance]=@HasAmbulance,
			[HasMotherShelter]=@HasMotherShelter, 
			[Longitude]=@Longitude, 
			[Latitude]=@Latitude, 
			[Name]=@Name, 
			[Description]=@Description, 
			[UpdatedDate] = getdate()

		WHERE [HealthCenterID]=@HealthCenterID 
	END 
 
	SELECT @HealthCenterID = ISNULL(@HealthCenterID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @HealthCenterID AS ReturnID) AS HealthCenterReturnTable 
 
	RETURN @HealthCenterID 
 
END 
 
GO
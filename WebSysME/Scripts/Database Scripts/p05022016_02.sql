alter table tblGroups
add NumberOfMales int
GO

alter table tblGroups 
add NumberOfFemales int
Go

/****** Object:  StoredProcedure [dbo].[sp_Save_Groups]    Script Date: 05/02/2016 12:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Save_Groups] 
( 
	@GroupID int = null output,  
	@WardID int = null, 
	@GroupTypeID int = null, 
	@Description varchar(255) = null, 
	@GroupSize int = null, 
	@UpdatedBy int = null, 
	@GroupName varchar(150) = null,
	@NumberOfMales int = null,
	@NumberOfFemales int = null
) 
AS 
BEGIN 
 
	IF @GroupID IS NULL OR @GroupID <= 0 
	BEGIN 
		 
		SELECT @GroupID = NULL 
 
		INSERT INTO [tblGroups](
			[WardID], 
			[GroupTypeID], 
			[Description], 
			[GroupSize], 
			[GroupName], 	[NumberOfMales],	[NumberOfFemales],	[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@WardID, 
			@GroupTypeID, 
			@Description, 
			@GroupSize, 
			@GroupName, 
			@NumberOfMales,
			@NumberOfFemales,
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblGroups] SET  
			[WardID]=@WardID, 
			[GroupTypeID]=@GroupTypeID, 
			[Description]=@Description, 
			[GroupSize]=@GroupSize, 
			[UpdatedBy]=@UpdatedBy, 
			[GroupName]=@GroupName,
			[NumberOfMales]=@NumberOfMales,
			[NumberOfFemales]=@NumberOfFemales, 
			[UpdatedDate] = getdate()

		WHERE [GroupID]=@GroupID 
	END 
 
	SELECT @GroupID = ISNULL(@GroupID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @GroupID AS ReturnID) AS GroupsReturnTable 
 
	RETURN @GroupID 
 
END 
 
 GO


/****** Object:  StoredProcedure [dbo].[sp_Save_HealthCenterStaff]    Script Date: 11/02/2016 00:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Save_HealthCenterStaff] 
( 
	@HealthCenterStaffID int = null output,  
	@HealthCenterID int = null, 
	@StaffRoleID int = null, 
	@UpdatedBy int = null, 
	@ProvinceID int = null, 
	@DistrictID int = null, 
	@DepartmentID int = null, 
	@DOB datetime = null, 
	@FirstName varchar(150) = null, 
	@Surname varchar(150) = null, 
	@Title varchar(50) = null, 
	@Sex varchar(50) = null, 
	@Site varchar(255) = null, 
	@Email varchar(255) = null, 
	@ContactNo varchar(255) = null, 
	@IDNumber varchar(255) = null,
	@GroupTypeID int = null
) 
AS 
BEGIN 
 
	IF @HealthCenterStaffID IS NULL OR @HealthCenterStaffID <= 0 
	BEGIN 
		 
		SELECT @HealthCenterStaffID = NULL 
 
		INSERT INTO [tblHealthCenterStaff](
			[HealthCenterID], 
			[StaffRoleID], 
			[ProvinceID], 
			[DistrictID], 
			[DepartmentID], 
			[DOB], 
			[FirstName], 
			[Surname], 
			[Title], 
			[Sex], 
			[Site], 
			[Email], 
			[ContactNo], 
			[IDNumber], 	[GroupTypeID],		[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@HealthCenterID, 
			@StaffRoleID, 
			@ProvinceID, 
			@DistrictID, 
			@DepartmentID, 
			@DOB, 
			@FirstName, 
			@Surname, 
			@Title, 
			@Sex, 
			@Site, 
			@Email, 
			@ContactNo, 
			@IDNumber,
			@GroupTypeID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblHealthCenterStaff] SET  
			[HealthCenterID]=@HealthCenterID, 
			[StaffRoleID]=@StaffRoleID, 
			[UpdatedBy]=@UpdatedBy, 
			[ProvinceID]=@ProvinceID, 
			[DistrictID]=@DistrictID, 
			[DepartmentID]=@DepartmentID, 
			[DOB]=@DOB, 
			[FirstName]=@FirstName, 
			[Surname]=@Surname, 
			[Title]=@Title, 
			[Sex]=@Sex, 
			[Site]=@Site, 
			[Email]=@Email, 
			[ContactNo]=@ContactNo, 
			[IDNumber]=@IDNumber, 
			[GroupTypeID]=@GroupTypeID,
			[UpdatedDate] = getdate()

		WHERE [HealthCenterStaffID]=@HealthCenterStaffID 
	END 
 
	SELECT @HealthCenterStaffID = ISNULL(@HealthCenterStaffID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @HealthCenterStaffID AS ReturnID) AS HealthCenterStaffReturnTable 
 
	RETURN @HealthCenterStaffID 
 
END 
 

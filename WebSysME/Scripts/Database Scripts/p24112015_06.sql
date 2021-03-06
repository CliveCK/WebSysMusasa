USE [PrimaSysActionAid]
GO
/****** Object:  StoredProcedure [dbo].[sp_Save_Beneficiary]    Script Date: 27/11/2015 03:32:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Save_Beneficiary] 
( 
	@BeneficiaryID int = null output,  
	@Suffix int = null, 
	@MaritalStatus int = null, 
	@HealthStatus int = null, 
	@DisabilityStatus int = null, 
	@LevelOfEducation int = null, 
	@Regularity int = null, 
	@VillageID int = null, 
	@StreetID int = null, 
	@UpdatedBy int = null, 
	@Opharnhood int = null, 
	@MajorSourceIncome int = null, 
	@ContactNo int = null, 
	@Condition int = null, 
	@Attendance int = null, 
	@Disability int = null, 
	@DateOfBirth datetime = null, 
	@IsUrban bit = null, 
	@MemberNo varchar(150) = null, 
	@FirstName varchar(150) = null, 
	@Surname varchar(150) = null, 
	@Sex varchar(20) = null, 
	@NationlIDNo varchar(150) = null, 
	@HouseNo varchar(150) = null, 
	@SerialNo varchar(150) = null,
	@ParentID int = null,
	@IsDependant bit = null,
	@RelationshipID int = null
) 
AS 
BEGIN 
 
	IF @BeneficiaryID IS NULL OR @BeneficiaryID <= 0 
	BEGIN 
		 
		SELECT @BeneficiaryID = NULL 
 
		INSERT INTO [tblBeneficiaries](
			[Suffix], 
			[MaritalStatus], 
			[HealthStatus], 
			[DisabilityStatus], 
			[LevelOfEducation], 
			[Regularity], 
			[VillageID], 
			[StreetID], 
			[Opharnhood], 
			[MajorSourceIncome], 
			[ContactNo], 
			[Condition], 
			[Attendance], 
			[Disability], 
			[DateOfBirth], 
			[IsUrban], 
			[MemberNo], 
			[FirstName], 
			[Surname], 
			[Sex], 
			[NationlIDNo], 
			[HouseNo],
			[IsDependant], 
			[SerialNo], [ParentID],		[RelationshipID],	[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@Suffix, 
			@MaritalStatus, 
			@HealthStatus, 
			@DisabilityStatus, 
			@LevelOfEducation, 
			@Regularity, 
			@VillageID, 
			@StreetID, 
			@Opharnhood, 
			@MajorSourceIncome, 
			@ContactNo, 
			@Condition, 
			@Attendance, 
			@Disability, 
			@DateOfBirth, 
			@IsUrban, 
			@MemberNo, 
			@FirstName, 
			@Surname, 
			@Sex, 
			@NationlIDNo, 
			@HouseNo, 
			@IsDependant,
			@SerialNo,
			@ParentID,
			@RelationshipID,
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblBeneficiaries] SET  
			[Suffix]=@Suffix, 
			[MaritalStatus]=@MaritalStatus, 
			[HealthStatus]=@HealthStatus, 
			[DisabilityStatus]=@DisabilityStatus, 
			[LevelOfEducation]=@LevelOfEducation, 
			[Regularity]=@Regularity, 
			[VillageID]=@VillageID, 
			[StreetID]=@StreetID, 
			[UpdatedBy]=@UpdatedBy, 
			[Opharnhood]=@Opharnhood, 
			[MajorSourceIncome]=@MajorSourceIncome, 
			[ContactNo]=@ContactNo, 
			[Condition]=@Condition, 
			[Attendance]=@Attendance, 
			[Disability]=@Disability, 
			[DateOfBirth]=@DateOfBirth, 
			[IsUrban]=@IsUrban, 
			[MemberNo]=@MemberNo, 
			[FirstName]=@FirstName, 
			[Surname]=@Surname, 
			[Sex]=@Sex, 
			[NationlIDNo]=@NationlIDNo, 
			[HouseNo]=@HouseNo,
			[IsDependant]=@IsDependant, 
			[SerialNo]=@SerialNo,
			[ParentID]=@ParentID, 
			[RelationshipID]=@RelationshipID,
			[UpdatedDate] = getdate()

		WHERE [BeneficiaryID]=@BeneficiaryID 
	END 
 
	SELECT @BeneficiaryID = ISNULL(@BeneficiaryID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @BeneficiaryID AS ReturnID) AS BeneficiaryReturnTable 
 
	RETURN @BeneficiaryID 
 
END 
 


/****** Object:  StoredProcedure [dbo].[sp_Save_Outreach]    Script Date: 16/11/2015 00:50:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Save_Outreach] 
( 
	@OutreachID int = null output,  
	@ProjectID int = null, 
	@OrganizationID int = null, 
	@BeneficiaryTypeID int = null, 
	@DistrictID int = null, 
	@WardID int = null, 
	@Total bigint = null,
	@StartDate datetime = null,
	@EndDate datetime = null,
	@PurposeofOutreach varchar(MAX) = null
) 
AS 
BEGIN 
 
	IF @OutreachID IS NULL OR @OutreachID <= 0 
	BEGIN 
		 
		SELECT @OutreachID = NULL 
 
		INSERT INTO [tblOutreach](
			[ProjectID], 
			[OrganizationID], 
			[BeneficiaryTypeID], 
			[DistrictID], 
			[WardID], 
			[Total],
			[StartDate],
			[EndDate],
			[PurposeofOutreach]
		) VALUES ( 

			@ProjectID, 
			@OrganizationID, 
			@BeneficiaryTypeID, 
			@DistrictID, 
			@WardID, 
			@Total,
			@StartDate,
			@EndDate,
			@PurposeofOutreach 
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblOutreach] SET  
			[ProjectID]=@ProjectID, 
			[OrganizationID]=@OrganizationID, 
			[BeneficiaryTypeID]=@BeneficiaryTypeID, 
			[DistrictID]=@DistrictID, 
			[WardID]=@WardID, 
			[Total]=@Total,
			[StartDate]=@StartDate,
			[EndDate]=@EndDate,
			[PurposeOfOutreach]=@PurposeofOutreach 
		WHERE [OutreachID]=@OutreachID 
	END 
 
	SELECT @OutreachID = ISNULL(@OutreachID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @OutreachID AS ReturnID) AS OutreachReturnTable 
 
	RETURN @OutreachID 
 
END 
 

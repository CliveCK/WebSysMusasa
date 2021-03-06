
/****** Object:  StoredProcedure [dbo].[sp_Save_Projects]    Script Date: 10/11/2015 09:15:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Save_Projects] 
( 
	@Project int = null output,  
	@ProjectCode varchar(150) = null, 
	@KeyChangePromiseID int = null, 
	@ProjectManager int = null, 
	@TargetedNoOfBeneficiaries int = null, 
	@ActualBeneficiaries int = null, 
	@UpdatedBy int = null, 
	@StartDate datetime = null, 
	@FinalEvlDate datetime = null, 
	@EndDate datetime = null, 
	@ProjectBudget numeric = null, 
	@Name varchar(150) = null, 
	@StrategicObjectiveID int = null, 
	@Acronym varchar(150) = null, 
	@ProjectDocument varchar(150) = null, 
	@FinalGStatement varchar(150) = null, 
	@Objective varchar(150) = null, 
	@BenDescription varchar(150) = null, 
	@StakeholderDescription varchar(150) = null
) 
AS 
BEGIN 
 
	IF @Project IS NULL OR @Project <= 0 
	BEGIN 
		 
		SELECT @Project = NULL 
 
		INSERT INTO [tblProjects](
			[ProjectCode], 
			[KeyChangePromiseID], 
			[ProjectManager], 
			[TargetedNoOfBeneficiaries], 
			[ActualBeneficiaries], 
			[StartDate], 
			[FinalEvlDate], 
			[EndDate], 
			[ProjectBudget], 
			[Name], 
			[StrategicObjectiveID], 
			[Acronym], 
			[ProjectDocument], 
			[FinalGStatement], 
			[Objective], 
			[BenDescription], 
			[StakeholderDescription], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@ProjectCode, 
			@KeyChangePromiseID, 
			@ProjectManager, 
			@TargetedNoOfBeneficiaries, 
			@ActualBeneficiaries, 
			@StartDate, 
			@FinalEvlDate, 
			@EndDate, 
			@ProjectBudget, 
			@Name, 
			@StrategicObjectiveID, 
			@Acronym, 
			@ProjectDocument, 
			@FinalGStatement, 
			@Objective, 
			@BenDescription, 
			@StakeholderDescription, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblProjects] SET  
			[ProjectCode]=@ProjectCode, 
			[KeyChangePromiseID]=@KeyChangePromiseID, 
			[ProjectManager]=@ProjectManager, 
			[TargetedNoOfBeneficiaries]=@TargetedNoOfBeneficiaries, 
			[ActualBeneficiaries]=@ActualBeneficiaries, 
			[UpdatedBy]=@UpdatedBy, 
			[StartDate]=@StartDate, 
			[FinalEvlDate]=@FinalEvlDate, 
			[EndDate]=@EndDate, 
			[ProjectBudget]=@ProjectBudget, 
			[Name]=@Name, 
			[StrategicObjectiveID]=@StrategicObjectiveID, 
			[Acronym]=@Acronym, 
			[ProjectDocument]=@ProjectDocument, 
			[FinalGStatement]=@FinalGStatement, 
			[Objective]=@Objective, 
			[BenDescription]=@BenDescription, 
			[StakeholderDescription]=@StakeholderDescription, 
			[UpdatedDate] = getdate()

		WHERE [Project]=@Project 
	END 
 
	SELECT @Project = ISNULL(@Project, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @Project AS ReturnID) AS ProjectsReturnTable 
 
	RETURN @Project 
 
END 
 

Create TABLE tblIntermediateProjectObjectives
(
	IntermediateProjectObjectiveID int IDENTITY(1,1) not null,
	IntermediateObjectiveID int not null,
	ProjectObjectiveID int not null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)
GO


CREATE PROCEDURE [dbo].[sp_Save_IntermediateProjectObjectives] 
( 
	@IntermediateProjectObjectiveID int = null output,  
	@IntermediateObjectiveID int = null, 
	@ProjectObjectiveID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @IntermediateProjectObjectiveID IS NULL OR @IntermediateProjectObjectiveID <= 0 
	BEGIN 
		 
		SELECT @IntermediateProjectObjectiveID = NULL 
 
		INSERT INTO [tblIntermediateProjectObjectives](
			[IntermediateObjectiveID], 
			[ProjectObjectiveID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@IntermediateObjectiveID, 
			@ProjectObjectiveID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblIntermediateProjectObjectives] SET  
			[IntermediateObjectiveID]=@IntermediateObjectiveID, 
			[ProjectObjectiveID]=@ProjectObjectiveID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [IntermediateProjectObjectiveID]=@IntermediateProjectObjectiveID 
	END 
 
	SELECT @IntermediateProjectObjectiveID = ISNULL(@IntermediateProjectObjectiveID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @IntermediateProjectObjectiveID AS ReturnID) AS IntermediateProjectObjectivesReturnTable 
 
	RETURN @IntermediateProjectObjectiveID 
 
END 
GO

IF NOT EXISTS(SELECT * FROM luMenu WHERE MenuName = 'Intermediate-Project Objectives')
BEGIN
	Insert into luMenu (MenuName, URL, ParentID, MenuType, OrderIndex, DrawMenu)
	values ('Intermediate-Project Objectives', '~/IntermediateProjectObjectivesPage.aspx', 57, 'MainMenu', 7, 1)
 END
 GO

Update luMenu set MenuName = 'Intermediate Objectives' where MenuName = 'Strategic Objectives'
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblCustomFields_Objects](
	[ObjectID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](255) NULL
)

GO

SET ANSI_PADDING OFF
GO

IF NOT EXISTS(SELECT * FROM tblCustomFields_Objects)
BEGIN 
	INSERT INTO [tblCustomFields_Objects] ([Code],[Description])VALUES('P','Projects')
	INSERT INTO [tblCustomFields_Objects] ([Code],[Description])VALUES('O','Organizations')
	INSERT INTO [tblCustomFields_Objects] ([Code],[Description])VALUES('G','Groups')
	INSERT INTO [tblCustomFields_Objects] ([Code],[Description])VALUES('S','Schools')
	INSERT INTO [tblCustomFields_Objects] ([Code],[Description])VALUES('HH','Households')
	INSERT INTO [tblCustomFields_Objects] ([Code],[Description])VALUES('H','HealthCenters')
END
GO

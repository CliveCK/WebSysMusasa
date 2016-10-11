alter table tblStreets 
add SectionID int
GO

CREATE TABLE luMaturityArea
(
	MaturityAreaID int IDENTITY(1,1) not null,
	Description varchar(255) null
)
GO

CREATE TABLE luTheme
(
	ThemeID int IDENTITY(1,1) not null,
	ThemeNo varchar(255) null,
	Description varchar(255) null,
)
GO

CREATE TABLE tblObjectiveThemes
(
	ObjectiveThemeID int IDENTITY(1,1) not null,
	ThemeID int not null,
	ObjectiveID int not null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)
GO


CREATE PROCEDURE [dbo].[sp_Save_ObjectiveThemes] 
( 
	@ObjectiveThemeID int = null output,  
	@ThemeID int = null, 
	@ObjectiveID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @ObjectiveThemeID IS NULL OR @ObjectiveThemeID <= 0 
	BEGIN 
		 
		SELECT @ObjectiveThemeID = NULL 
 
		INSERT INTO [tblObjectiveThemes](
			[ThemeID], 
			[ObjectiveID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@ThemeID, 
			@ObjectiveID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblObjectiveThemes] SET  
			[ThemeID]=@ThemeID, 
			[ObjectiveID]=@ObjectiveID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [ObjectiveThemeID]=@ObjectiveThemeID 
	END 
 
	SELECT @ObjectiveThemeID = ISNULL(@ObjectiveThemeID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @ObjectiveThemeID AS ReturnID) AS ObjectiveThemesReturnTable 
 
	RETURN @ObjectiveThemeID 
 
END 
 
GO

IF NOT EXISTS(SELECT * FROM luMenu WHERE MenuName = 'Objective Themes')
BEGIN
	Insert into luMenu (MenuName,URL,ParentID, DrawMenu, MenuType, OrderIndex)
			values ('Objective Themes', '~/ObjectiveThemes.aspx', 57, 1, 'MainMenu', 6)
END
GO

CREATE TABLE luDevelopmentLevel
(
	DevelopmentLevelID int IDENTITY(1,1) not null,
	Description varchar(255) null,
)
GO

CREATE TABLE tblObjectiveDevelopmentLevel
(
	ObjectiveDevelopmentLevelID int IDENTITY(1,1) not null,
	DevelopmentLevelID int not null,
	ObjectiveID int not null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)
GO


CREATE PROCEDURE [dbo].[sp_Save_ObjectiveDevelopmentLevel] 
( 
	@ObjectiveDevelopmentLevelID int = null output,  
	@DevelopmentLevelID int = null, 
	@ObjectiveID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @ObjectiveDevelopmentLevelID IS NULL OR @ObjectiveDevelopmentLevelID <= 0 
	BEGIN 
		 
		SELECT @ObjectiveDevelopmentLevelID = NULL 
 
		INSERT INTO [tblObjectiveDevelopmentLevel](
			[DevelopmentLevelID], 
			[ObjectiveID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@DevelopmentLevelID, 
			@ObjectiveID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblObjectiveDevelopmentLevel] SET  
			[DevelopmentLevelID]=@DevelopmentLevelID, 
			[ObjectiveID]=@ObjectiveID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [ObjectiveDevelopmentLevelID]=@ObjectiveDevelopmentLevelID 
	END 
 
	SELECT @ObjectiveDevelopmentLevelID = ISNULL(@ObjectiveDevelopmentLevelID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @ObjectiveDevelopmentLevelID AS ReturnID) AS ObjectiveDevelopmentLevelReturnTable 
 
	RETURN @ObjectiveDevelopmentLevelID 
 
END 
 
GO

IF NOT EXISTS(SELECT * FROM luMenu WHERE MenuName = 'Objective Development Level')
BEGIN
	Insert into luMenu (MenuName,URL,ParentID, DrawMenu, MenuType, OrderIndex)
			values ('Objective Development Level', '~/ObjectiveDevelopmentLevelPage.aspx', 57, 1, 'MainMenu', 7)
END
GO
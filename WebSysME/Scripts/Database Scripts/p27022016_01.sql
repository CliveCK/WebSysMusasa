CREATE TABLE tblStaffTrainingAccess
(
	StaffTrainingAccessID int IDENTITY(1,1) not null,
	StaffID int not null,
	TrainingID int not null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)
GO


CREATE PROCEDURE [dbo].[sp_Save_StaffTrainingAccess] 
( 
	@StaffTrainingAccessID int = null output,  
	@StaffID int = null, 
	@TrainingID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @StaffTrainingAccessID IS NULL OR @StaffTrainingAccessID <= 0 
	BEGIN 
		 
		SELECT @StaffTrainingAccessID = NULL 
 
		INSERT INTO [tblStaffTrainingAccess](
			[StaffID], 
			[TrainingID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@StaffID, 
			@TrainingID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblStaffTrainingAccess] SET  
			[StaffID]=@StaffID, 
			[TrainingID]=@TrainingID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [StaffTrainingAccessID]=@StaffTrainingAccessID 
	END 
 
	SELECT @StaffTrainingAccessID = ISNULL(@StaffTrainingAccessID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @StaffTrainingAccessID AS ReturnID) AS StaffTrainingAccessReturnTable 
 
	RETURN @StaffTrainingAccessID 
 
END 
 
GO

CREATE VIEW vwStaffMembers 
AS

SELECT ISNULL(FirstName,'') + ' ' + ISNULL(Surname,'') as FullName, * FROM tblStaffMembers 

GO

IF NOT EXISTS(SELECT * FROM luMenu WHERE MenuName = 'Training Permissions')
BEGIN
	insert into luMenu (MenuName, ParentID, URL, MenuType, DrawMenu, OrderIndex)
	values
	('Training Permissions', 67, '~/Administration/Permissions/TrainingPermissions.aspx', 'MainMenu', 1, 5)
END
GO
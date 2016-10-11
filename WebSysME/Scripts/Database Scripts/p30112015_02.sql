IF NOT EXISTS (SELECT * FROM luMenu WHERE MenuName = 'Group Associations Mapping')
BEGIN
	insert into luMenu (MenuName, ParentID, URL, MenuType, OrderIndex, DrawMenu) values ('Group Associations Mapping', 57, '~/GroupAssociationsMapping.aspx', 'MainMenu', 5, 1)
END
GO

IF NOT EXISTS (SELECT * FROM luMenu WHERE MenuName = 'Group Associations')
BEGIN
	insert into luMenu (MenuName, ParentID, URL, MenuType, OrderIndex, DrawMenu) values ('Group Associations', 57, '~/GroupAssociationsMapping.aspx', 'MainMenu', 5, 1)
END
GO

CREATE PROCEDURE [dbo].[sp_Save_HouseholdGroups] 
( 
	@HouseholdGroupID int = null output,  
	@HouseholdID int = null, 
	@GroupID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @HouseholdGroupID IS NULL OR @HouseholdGroupID <= 0 
	BEGIN 
		 
		SELECT @HouseholdGroupID = NULL 
 
		INSERT INTO [tblHouseholdGroups](
			[HouseholdID], 
			[GroupID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@HouseholdID, 
			@GroupID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblHouseholdGroups] SET  
			[HouseholdID]=@HouseholdID, 
			[GroupID]=@GroupID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [HouseholdGroupID]=@HouseholdGroupID 
	END 
 
	SELECT @HouseholdGroupID = ISNULL(@HouseholdGroupID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @HouseholdGroupID AS ReturnID) AS HouseholdGroupsReturnTable 
 
	RETURN @HouseholdGroupID 
 
END 
 
GO

update luMenu set URL = NULL where MenuID = 41
GO

insert into luMenu (MenuName,ParentID,URL,MenuType,DrawMenu,OrderIndex)
values
('Group Listing', 41, '~/GroupsListing.aspx','MainMenu', 1,1)
GO

insert into luMenu (MenuName,ParentID,URL,MenuType,DrawMenu,OrderIndex)
values
('Group Associations', 41, '~/GroupAssociationListing.aspx','MainMenu', 1,1)
GO


insert into luMenu (MenuName,ParentID,URL,MenuType,DrawMenu,OrderIndex)
values
('Communication', NULL, '~/Messaging/InAppMail.aspx','MainMenu', 1,1)
GO
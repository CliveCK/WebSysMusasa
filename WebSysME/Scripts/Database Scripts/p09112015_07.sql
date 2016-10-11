
CREATE PROCEDURE [dbo].[sp_Save_UserAccountProfileLink] 
( 
	@UserAccountProfileLink int = null output,  
	@UserID int = null, 
	@StaffID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @UserAccountProfileLink IS NULL OR @UserAccountProfileLink <= 0 
	BEGIN 
		 
		SELECT @UserAccountProfileLink = NULL 
 
		INSERT INTO [tblUserAccountProfileLink](
			[UserID], 
			[StaffID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@UserID, 
			@StaffID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblUserAccountProfileLink] SET  
			[UserID]=@UserID, 
			[StaffID]=@StaffID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [UserAccountProfileLink]=@UserAccountProfileLink 
	END 
 
	SELECT @UserAccountProfileLink = ISNULL(@UserAccountProfileLink, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @UserAccountProfileLink AS ReturnID) AS UserAccountProfileLinkReturnTable 
 
	RETURN @UserAccountProfileLink 
 
END 
 

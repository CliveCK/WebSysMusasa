
CREATE PROCEDURE [dbo].[sp_Save_FilePermissions] 
( 
	@DocumentPermissionID int = null output,  
	@DocumentID int = null, 
	@LevelOfSecurityID int = null, 
	@ObjectID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @DocumentPermissionID IS NULL OR @DocumentPermissionID <= 0 
	BEGIN 
		 
		SELECT @DocumentPermissionID = NULL 
 
		INSERT INTO [tblDocumentPermissions](
			[DocumentID], 
			[LevelOfSecurityID], 
			[ObjectID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@DocumentID, 
			@LevelOfSecurityID, 
			@ObjectID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblDocumentPermissions] SET  
			[DocumentID]=@DocumentID, 
			[LevelOfSecurityID]=@LevelOfSecurityID, 
			[ObjectID]=@ObjectID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [DocumentPermissionID]=@DocumentPermissionID 
	END 
 
	SELECT @DocumentPermissionID = ISNULL(@DocumentPermissionID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @DocumentPermissionID AS ReturnID) AS FilePermissionsReturnTable 
 
	RETURN @DocumentPermissionID 
 
END 
 

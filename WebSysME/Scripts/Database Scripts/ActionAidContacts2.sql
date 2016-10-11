Alter table tblOrganization
add CellphoneNo varchar(255)
GO

Alter table tblOrganization
add tempcol varchar(255)
GO

update tblOrganization set tempcol = ContactNo
GO

Alter table tblOrganization
drop column ContactNo
GO

Alter table tblOrganization
add ContactNo varchar(255)
GO

update tblOrganization set ContactNo = tempcol
GO

Alter table tblOrganization
drop column tempcol
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Save_Organization] 
( 
	@OrganizationID int = null output,  
	@WardID int = null, 
	@OrganizationTypeID int = null, 
	@Longitude numeric = null, 
	@Latitude numeric = null, 
	@ContactNo varchar(255) = null, 
	@CellphoneNo varchar(255) = null,
	@Name varchar(150) = null, 
	@Description varchar(255) = null, 
	@ContactName varchar(150) = null,
	@Address varchar(255) = null,
	@EmailAddress varchar(150) = null,
	@WebsiteAddress varchar(255) = null,
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @OrganizationID IS NULL OR @OrganizationID <= 0 
	BEGIN 
		 
		SELECT @OrganizationID = NULL 
 
		INSERT INTO [tblOrganization](
			[WardID], 
			[OrganizationTypeID], 
			[Longitude], 
			[Latitude], 
			[ContactNo], 
			[CellphoneNo],
			[Name], 
			[Description], 
			[ContactName],
			[Address],
			[EmailAddress],
			[WebsiteAddress],
			[CreatedBy],
			[CreatedDate]
		) VALUES ( 

			@WardID, 
			@OrganizationTypeID, 
			@Longitude, 
			@Latitude, 
			@ContactNo, 
			@CellphoneNo,
			@Name, 
			@Description, 
			@ContactName,
			@Address,
			@EmailAddress,
			@WebsiteAddress,
			@UpdatedBy,
			getdate()
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblOrganization] SET  
			[WardID]=@WardID, 
			[OrganizationTypeID]=@OrganizationTypeID, 
			[Longitude]=@Longitude, 
			[Latitude]=@Latitude, 
			[ContactNo]=@ContactNo, 
			[Cellphone]=@CellphoneNo,
			[Name]=@Name, 
			[Description]=@Description, 
			[ContactName]=@ContactName,
			[Address]=@Address,
			[EmailAddress]=@EmailAddress,
			[WebsiteAddress]=@WebsiteAddress,
			[UpdatedBy]=@UpdatedBy,
			[UpdatedDate]=getdate()
		WHERE [OrganizationID]=@OrganizationID 
	END 
 
	SELECT @OrganizationID = ISNULL(@OrganizationID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @OrganizationID AS ReturnID) AS OrganizationReturnTable 
 
	RETURN @OrganizationID 
 
END 

GO
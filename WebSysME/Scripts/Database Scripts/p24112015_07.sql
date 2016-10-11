CREATE TABLE luKeyAreas
(
	KeyAreaID int iDENTITY(1,1) not null,
	Description varchar(255) null
)
GO

CREATE TABLE tblKeyAreaIndicators
(
	KeyAreaIndicatorID int IDENTITY(1,1) not null,
	KeyAreaID int not null,
	IndicatorID int not null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)
GO


CREATE PROCEDURE [dbo].[sp_Save_KeyAreaIndicators] 
( 
	@KeyAreaIndicatorID int = null output,  
	@KeyAreaID int = null, 
	@IndicatorID int = null, 
	@UpdatedBy int = null
) 
AS 
BEGIN 
 
	IF @KeyAreaIndicatorID IS NULL OR @KeyAreaIndicatorID <= 0 
	BEGIN 
		 
		SELECT @KeyAreaIndicatorID = NULL 
 
		INSERT INTO [tblKeyAreaIndicators](
			[KeyAreaID], 
			[IndicatorID], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@KeyAreaID, 
			@IndicatorID, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblKeyAreaIndicators] SET  
			[KeyAreaID]=@KeyAreaID, 
			[IndicatorID]=@IndicatorID, 
			[UpdatedBy]=@UpdatedBy, 
			[UpdatedDate] = getdate()

		WHERE [KeyAreaIndicatorID]=@KeyAreaIndicatorID 
	END 
 
	SELECT @KeyAreaIndicatorID = ISNULL(@KeyAreaIndicatorID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @KeyAreaIndicatorID AS ReturnID) AS KeyAreaIndicatorsReturnTable 
 
	RETURN @KeyAreaIndicatorID 
 
END 
 
GO
CREATE TABLE tblCommunityScoreCard
(
	CommunityScoreCardID int IDENTITY(1,1) not null,
	CommunityID int not null,
	IndicatorID int not null,
	ThermaticAreaID int null,
	[Date] datetime null,
	Score varchar(255) null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)

CREATE Table luThermaticArea
(
	--ThermaticAreaID int IDENTITY(1,1) not null,
	Description varchar(255) null
)


CREATE PROCEDURE [dbo].[sp_Save_CommunityScoreCard] 
( 
	@CommunityScoreCardID int = null output,  
	@CommunityID int = null, 
	@IndicatorID int = null, 
	@ThermaticAreaID int = null, 
	@UpdatedBy int = null, 
	@Date datetime = null, 
	@Score varchar(255) = null
) 
AS 
BEGIN 
 
	IF @CommunityScoreCardID IS NULL OR @CommunityScoreCardID <= 0 
	BEGIN 
		 
		SELECT @CommunityScoreCardID = NULL 
 
		INSERT INTO [tblCommunityScoreCard](
			[CommunityID], 
			[IndicatorID], 
			[ThermaticAreaID], 
			[Date], 
			[Score], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@CommunityID, 
			@IndicatorID, 
			@ThermaticAreaID, 
			@Date, 
			@Score, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblCommunityScoreCard] SET  
			[CommunityID]=@CommunityID, 
			[IndicatorID]=@IndicatorID, 
			[ThermaticAreaID]=@ThermaticAreaID, 
			[UpdatedBy]=@UpdatedBy, 
			[Date]=@Date, 
			[Score]=@Score, 
			[UpdatedDate] = getdate()

		WHERE [CommunityScoreCardID]=@CommunityScoreCardID 
	END 
 
	SELECT @CommunityScoreCardID = ISNULL(@CommunityScoreCardID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @CommunityScoreCardID AS ReturnID) AS CommunityScoreCardReturnTable 
 
	RETURN @CommunityScoreCardID 
 
END 
 

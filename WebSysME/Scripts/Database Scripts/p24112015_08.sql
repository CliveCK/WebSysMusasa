CREATE TABLE tblGroupMaturityIndex
(
	GroupMaturityIndexID int IDENTITY(1,1) not null,
	GroupID int not null,
	KeyAreaID int null,
	MonthID int null,
	Year int null,
	Score varchar(150) null,
	CreatedBy int null,
	CreatedDate datetime null,
	UpdatedBy int null,
	UpdatedDate datetime null
)

GO


CREATE PROCEDURE [dbo].[sp_Save_GroupMaturityIndex] 
( 
	@GroupMaturityIndexID int = null output,  
	@GroupID int = null, 
	@KeyAreaID int = null, 
	@MonthID int = null, 
	@Year int = null, 
	@UpdatedBy int = null, 
	@Score varchar(150) = null
) 
AS 
BEGIN 
 
	IF @GroupMaturityIndexID IS NULL OR @GroupMaturityIndexID <= 0 
	BEGIN 
		 
		SELECT @GroupMaturityIndexID = NULL 
 
		INSERT INTO [tblGroupMaturityIndex](
			[GroupID], 
			[KeyAreaID], 
			[MonthID], 
			[Year], 
			[Score], 			[CreatedDate], 			[CreatedBy]
		) VALUES ( 

			@GroupID, 
			@KeyAreaID, 
			@MonthID, 
			@Year, 
			@Score, 
			getdate(), 
			@UpdatedBy
		) 
	END 
	ELSE 
	BEGIN 
		UPDATE [tblGroupMaturityIndex] SET  
			[GroupID]=@GroupID, 
			[KeyAreaID]=@KeyAreaID, 
			[MonthID]=@MonthID, 
			[Year]=@Year, 
			[UpdatedBy]=@UpdatedBy, 
			[Score]=@Score, 
			[UpdatedDate] = getdate()

		WHERE [GroupMaturityIndexID]=@GroupMaturityIndexID 
	END 
 
	SELECT @GroupMaturityIndexID = ISNULL(@GroupMaturityIndexID, SCOPE_IDENTITY()) 
	SELECT * FROM (SELECT @GroupMaturityIndexID AS ReturnID) AS GroupMaturityIndexReturnTable 
 
	RETURN @GroupMaturityIndexID 
 
END 
 
GO
USE [PrimaSysActionAid]
GO
/****** Object:  StoredProcedure [dbo].[GetSentMessages]    Script Date: 25/11/2015 00:56:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetSentMessages]	
	(
	@userId varchar(50)
	)	
AS
	Select CASE WHEN LTRIM(RTRIM(ISNULL(S.UserFirstName, '') + '' + ISNULL(S.UserSurname, ''))) = '' THEN S.Username ELSE ISNULL(S.UserFirstName, '') + '' + ISNULL(S.UserSurname, '') END As Name 
	, M.* from Messages M inner join tblUsers S on M.senderID = S.UserID where senderID = @userId order by datentime DESC
	RETURN


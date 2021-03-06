USE [PrimaSysActionAid]
GO
/****** Object:  StoredProcedure [dbo].[GetSentMessages]    Script Date: 24/11/2015 13:23:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].GetDeletedMessages
	(
	@userId varchar(50)
	)	
AS


	Select CASE WHEN LTRIM(RTRIM(ISNULL(S.UserFirstName, '') + '' + ISNULL(S.UserSurname, ''))) = '' THEN S.Username ELSE ISNULL(S.UserFirstName, '') + '' + ISNULL(S.UserSurname, '') END As Name 
	, M.* from Messages M inner join tblUsers S on M.senderID = S.UserID where M.[Deleted] = 1 and recieverid = @userId order by datentime DESC
	RETURN


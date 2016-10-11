update luMenu set MenuName = 'Client Details' where MenuName = 'Beneficiaries' and ParentID IS NULL
GO

update luMenu set ParentID = (Select MenuID from luMenu where MenuName = 'Projects' and ParentID IS NULL) where MenuName = 'Outreach'
GO

update luMenu set OrderIndex = 2 where MenuName = 'Administration'
GO

update luMenu set OrderIndex = 3 where MenuName = 'Implementation'
GO

update luMenu set OrderIndex = 4 where MenuName = 'Reports'
GO

update luMenu set OrderIndex = 5 where MenuName = 'Client Details'
GO

update luMenu set OrderIndex = 6 where MenuName = 'Financials'
GO

update luMenu set OrderIndex = 7 where MenuName = 'Organizational Details'
GO

update luMenu set OrderIndex = 8 where MenuName = 'Contacts'
GO

update luMenu set OrderIndex = 9 where MenuName = 'Library'
GO

IF NOT EXISTS (SELECT * FROM tblProgrammes WHERE NAME = 'Advocacy')
BEGIN
	INSERT INTO tblProgrammes (Name) values ('Clinical Care'), ('Psychosocial Support'), ('Advocacy'), ('Outreach')
END
GO




alter table tblProjects 
add KeyChangePromiseID int
GO

alter table tblProjects
drop column Sector
GO

alter table tblProjects
add StrategicObjectiveID int
GO
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class UserGroupsManager

#Region "Variables"

    Private db As Database
    Private mConnectionName As String
    Private mObjectClientID As Long
    Private mUserGroupIDs() As String
    Private mGroupArraySize As Integer
    Private mStringUserGroupIDs As String


#End Region

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectClientID As Long)

        mObjectClientID = ObjectClientID
        mConnectionName = ConnectionName
        Dim factory As DatabaseProviderFactory = New DatabaseProviderFactory()
        db = factory.Create(ConnectionName)

    End Sub

#End Region

#Region "Properties"

    Public Property UserGroupIDs(ByVal ArraySize As Integer) As String
        Get
            Return mUserGroupIDs(ArraySize)
        End Get
        Set(ByVal value As String)
            mUserGroupIDs(ArraySize) = value
        End Set
    End Property

    Public Property GroupArraySize() As Integer
        Get
            Return mGroupArraySize
        End Get
        Set(ByVal value As Integer)
            mGroupArraySize = value
        End Set
    End Property

    Public Property StringUserGroupIDs() As String
        Get
            Return mStringUserGroupIDs
        End Get
        Set(ByVal value As String)
            mStringUserGroupIDs = value
        End Set
    End Property

#End Region

#Region "Methods"


    Public Function GetAllRoles() As DataSet

        Dim sql As String = "SELECT * FROM luUserGroups"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function CreateUsergroup(ByVal UserGroup As String) As Boolean

        Dim sql As String = "INSERT INTO luUserGroups ([Description]) VALUES ('" & UserGroup & "')"

        Return db.ExecuteNonQuery(CommandType.Text, sql)

    End Function

    Public Sub AddRoleFunctionalities()

    End Sub

    Public Function GetSelectedUserGroupFunctionalities(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luFunctionalities WHERE functionalityID IN (SELECT Functionalityid FROM tblUserFunctionalities where UserID=" & UserGroupID & " AND UserType='UserGroup')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAvailableUserGroupFunctionalities(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luFunctionalities WHERE functionalityID not in (SELECT Functionalityid FROM tblUserFunctionalities where UserID=" & UserGroupID & " AND UserType='UserGroup')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAvailableUserFunctionalities(ByVal UserID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luFunctionalities WHERE functionalityID not in (SELECT Functionalityid FROM tblUserFunctionalities where UserID=" & UserID & " AND UserType='User')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetSelectedUserFunctionalities(ByVal UserID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luFunctionalities WHERE functionalityID IN (SELECT Functionalityid FROM tblUserFunctionalities where UserID=" & UserID & " AND UserType='User')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetSelectedTrainingStaffMembers(ByVal TrainingID As Long) As DataSet

        Dim sql As String = "SELECT S.* from vwStaffMembers S inner join tblStaffTrainingAccess TS on S.StaffID = TS.StaffID where TrainingID = " & TrainingID

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAvaliableTrainingStaffMembers(ByVal TrainingID As Long) As DataSet

        Dim sql As String = "SELECT * from vwStaffMembers where StaffID not in (Select StaffID from tblStaffTrainingAccess  where TrainingID = " & TrainingID & ")"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function DeleteRole(ByVal UserGroupID As Long) As Boolean

        Dim sql As String = "DELETE FROM luUserGroups WHERE UserGroupID = " & UserGroupID

        If db.ExecuteNonQuery(CommandType.Text, sql) = 1 Then

            Return True

        Else

            Return False

        End If

    End Function

    Public Function SaveDetail(ByVal UserID As Long, ByVal Functionalityid As Long, ByVal UserType As String) As Boolean

        Dim InsertSQL As String = "INSERT INTO tblUserFunctionalities ([UserID], [Functionalityid],[UserType]) VALUES (" & UserID & "," & Functionalityid & ",'" & UserType & "')"

        db.ExecuteNonQuery(CommandType.Text, InsertSQL)

        Return True

    End Function

    Public Function GetUserUsergroups(ByVal UserID As Long) As DataSet

        Dim ds As DataSet

        Dim sql As String = "SELECT * FROM tblUserUsergroups WHERE UserID = " & UserID

        ds = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return ds

        Else

            Return Nothing

        End If


    End Function

    Public Function EditUserGroup(ByVal UserGroupID As Long, ByVal NewDesciption As String) As Boolean

        Dim sql As String = "UPDATE luUserGroups SET [Description]= '" & NewDesciption & "' WHERE [UserGroupID] = " & UserGroupID

        Return db.ExecuteNonQuery(CommandType.Text, sql)

    End Function

    Public Function Revoke(ByVal UserID As Long, ByVal UserType As String) As Boolean

        Dim sql As String = "DELETE FROM tblUserFunctionalities where UserID=" & UserID & " AND UserType='" & UserType & "'"

        Return db.ExecuteNonQuery(CommandType.Text, sql)

    End Function
    Public Function RevokeTrainingPermissions(ByVal TrainingID As Long) As Boolean

        Dim sql As String = "DELETE FROM tblStaffTrainingAccess where TrainingID = " & TrainingID

        Return db.ExecuteNonQuery(CommandType.Text, sql)

    End Function

    Public Function DeleteDetail(ByVal UserID As Integer, ByVal MenuID As Integer, ByVal UserType As String) As Boolean

        Dim DeleteSQL As String = "DELETE FROM tblUserFunctionalities WHERE UserID = " & UserID & " AND MenuID =" & MenuID & " AND UserType='" & UserType & "'"

        db.ExecuteNonQuery(CommandType.Text, DeleteSQL)

        Return True

    End Function

    Public Function CheckIfUserHasSpecificPermissions(ByVal ClientID As Long) As Boolean

        Dim sql As String = "SELECT * FROM dbo.tblDocumentUserPermissions where ClientID=" & ClientID & " AND UserType='User'"

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

    Public Function CheckIfUserHasWritePermissions(ByVal ClientID As Long, ByVal DocumentID As Long) As Boolean


        Dim sql As String = "SELECT * FROM dbo.tblDocumentUserPermissions where ClientID=" & ClientID & " AND DocumentID=" & DocumentID & " AND DocumentPermissionID=2 AND UserType='User'"

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

    Public Function CheckIfGroupHasWritePermissions(ByVal GroupIDs As String, ByVal DocumentID As Long) As Boolean

        Dim sql As String = "SELECT * FROM dbo.tblDocumentUserPermissions where ClientID IN (" & GroupIDs & ") AND DocumentID=" & DocumentID & " AND DocumentPermissionID=2 AND UserType='Group'"

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

#End Region

End Class

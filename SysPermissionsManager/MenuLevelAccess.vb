Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class MenuLevelAccess

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

    Public Function GetSelectedUserGroupMenuRights(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luMenu WHERE MenuID IN (SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID=" & UserGroupID & " AND UserType='UserGroup') AND MenuType= 'MainMenu' AND DrawMenu = 1 "

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetSelectedUserGroupContextMenuRights(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luMenu WHERE MenuID IN (SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID=" & UserGroupID & " AND UserType='UserGroup') AND MenuType NOT IN ('MainMenu') AND DrawMenu = 1 "
        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAvailableUserGroupMenuRights(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luMenu WHERE MenuID not in (SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID=" & UserGroupID & " AND UserType='UserGroup') AND MenuType= 'MainMenu' AND DrawMenu = 1"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAvailableUserGroupContextMenuRights(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luMenu WHERE MenuID not in (SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID=" & UserGroupID & " AND UserType='UserGroup') AND MenuType NOT IN ('MainMenu','DashBoard') AND DrawMenu = 1"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function


    Public Function GetAvailableUserMenuRights(ByVal UserID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luMenu WHERE MenuID not in (SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID=" & UserID & " AND UserType='User') AND MenuType= 'MainMenu' AND DrawMenu = 1"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAvailableUserContextMenuRights(ByVal UserID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luMenu WHERE MenuID not in (SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID=" & UserID & " AND UserType='User') AND MenuType NOT IN ('MainMenu','DashBoard') AND DrawMenu = 1"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetSelectedUserMenuRights(ByVal UserID As Long) As DataSet

        Dim script As New System.Text.StringBuilder("")

        script.AppendLine("SELECT * FROM luMenu ")
        script.AppendLine("WHERE MenuID IN (")
        script.AppendLine("	SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID = " & UserID & " AND UserType='User' ")
        script.AppendLine("	UNION")
        script.AppendLine("	SELECT MenuID FROM tblUserMenuLevelAccessRights WHERE UserType='UserGroup' AND UserID IN (SELECT UserGroupID FROM [tblUserUserGroups] WHERE [tblUserUserGroups].UserID = " & UserID & ")")
        script.AppendLine(") ")
        script.AppendLine("AND MenuType = 'MainMenu' AND DrawMenu = 1")

        Return db.ExecuteDataSet(CommandType.Text, script.ToString)

    End Function

    Public Function GetSelectedUserContextMenuRights(ByVal UserID As Long) As DataSet

        Dim script As New System.Text.StringBuilder("")

        script.AppendLine("SELECT * FROM luMenu ")
        script.AppendLine("WHERE MenuID IN (")
        script.AppendLine("	SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID = " & UserID & " AND UserType='User' ")
        script.AppendLine("	UNION")
        script.AppendLine("	SELECT MenuID FROM tblUserMenuLevelAccessRights WHERE UserType='UserGroup' AND UserID IN (SELECT UserGroupID FROM [tblUserUserGroups] WHERE [tblUserUserGroups].UserID = " & UserID & ")")
        script.AppendLine(") ")
        script.AppendLine("AND MenuType NOT IN ('MainMenu') AND DrawMenu = 1")

        Return db.ExecuteDataSet(CommandType.Text, script.ToString)

    End Function

    Public Function GetUserPageRights(ByVal UserID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luMenu WHERE MenuID IN (SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID=" & UserID & " AND UserType='User')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetUserGroupPageRights(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.luMenu WHERE MenuID IN (SELECT MenuID FROM tblUserMenuLevelAccessRights where UserID=" & UserGroupID & " AND UserType='UserGroup')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function SaveDetail(ByVal UserID As Long, ByVal MenuID As Long, ByVal UserType As String) As Boolean

        Dim InsertSQL As String = "INSERT INTO tblUserMenuLevelAccessRights ([UserID], [MenuID],[UserType]) VALUES (" & UserID & "," & MenuID & ",'" & UserType & "')"

        db.ExecuteNonQuery(CommandType.Text, InsertSQL)

        Return True

    End Function

    Public Function Revoke(ByVal UserID As Long, ByVal UserType As String) As Boolean

        Dim sql As String = "DELETE FROM tblUserMenuLevelAccessRights where UserID=" & UserID & " AND UserType='" & UserType & "'"
        db.ExecuteNonQuery(CommandType.Text, sql)
        Return True

    End Function

    Public Function DeleteDetail(ByVal UserID As Integer, ByVal MenuID As Integer, ByVal UserType As String) As Boolean

        Dim DeleteSQL As String = "DELETE FROM tblUserMenuLevelAccessRights WHERE UserID = " & UserID & " AND MenuID =" & MenuID & " AND UserType='" & UserType & "'"

        db.ExecuteNonQuery(CommandType.Text, DeleteSQL)

        Return True

    End Function

#End Region

#Region "Rights Management"

    Public Function AuthenticateUserGroupFunctionalities(ByVal UserID As Long, ByVal MenuID As Long) As Boolean

        Dim sql As String = "SELECT * FROM tblUserMenuLevelAccessRights  WHERE [MenuID] = " & MenuID & " AND [UserID] IN (SELECT [UserGroupID] FROM tblUserUsergroups WHERE UserID  = " & UserID & ") AND UserType='UserGroup'"
        ' Assuming ADministrator is always 1

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

    Public Function AuthenticateUserFunctionalities(ByVal UserID As Long, ByVal MenuID As Long) As Boolean

        Dim sql As String = "SELECT * FROM tblUserMenuLevelAccessRights  WHERE [MenuID] = " & MenuID & " AND  UserID  = " & UserID & " AND UserType='User'" ' Assuming ADministrator is always 1

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

    Public Function CheckGroupIfUserIsInGroup(ByVal UserID As Long) As Boolean

        Dim sql As String = "SELECT RoleID FROM dbo.tblUserRoles where UserID=" & UserID

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        Dim StringRoleIDs As String = ""

        If ds.Tables(0).Rows.Count > 0 Then

            mGroupArraySize = ds.Tables(0).Rows.Count

            ReDim mUserGroupIDs(mGroupArraySize)

            For intCounter As Integer = 0 To ds.Tables(0).Rows.Count - 1

                mUserGroupIDs(intCounter) = ds.Tables(0).Rows(intCounter).Item(0).ToString

                mStringUserGroupIDs &= IIf(mStringUserGroupIDs = "", "", " , ") & mUserGroupIDs(intCounter)

            Next

            Return True

        Else : Return False

        End If

    End Function

    Public Function IsAdministrator(ByVal UserID As Long) As Boolean

        Dim sql As String = "SELECT * FROM tblUserUserGroups WHERE UserGroupID = 1 AND UserID=" & UserID

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Rows.Count > 0 Then

            Return True

        Else : Return False

        End If

    End Function

    Public Function CheckMenuRightPermission(ByVal UserID As Long, ByVal MenuID As Long) As Boolean

        If IsAdministrator(UserID) Then ' 

            Return True ' adminis do not have restrictions

        Else

            If AuthenticateUserFunctionalities(UserID, MenuID) Then
                ' apply specific user permissions 
                Return True

            Else 'No Specific User Permission are available so apply group permissionz.

                If AuthenticateUserGroupFunctionalities(UserID, MenuID) Then
                    'Do nothing
                    Return True

                Else

                    Return False

                End If

            End If


        End If

    End Function

#End Region

End Class

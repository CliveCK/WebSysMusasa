Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions

Public Class UserReportAccessRight

#Region "Variables"

    Protected mReportAccessRightID As Long
    Protected mReportID As Long
    Protected mUserID As Long
    Protected mUserType As String
    Protected mCreatedDate As String
    Protected mCreatedBy As Long
    Protected mUpdatedDate As String
    Protected mUpdatedBy As Long

    Protected db As Database
    Protected mConnectionName As String
    Protected mObjectUserID As Long

#End Region

#Region "Properties"

    Public ReadOnly Property Database() As Database
        Get
            Return db
        End Get
    End Property

    Public ReadOnly Property OwnerType() As String
        Get
            Return Me.GetType.Name
        End Get
    End Property

    Public ReadOnly Property ConnectionName() As String
        Get
            Return mConnectionName
        End Get
    End Property

    Public Overridable Property ReportAccessRightID() As Long
        Get
            Return mReportAccessRightID
        End Get
        Set(ByVal value As Long)
            mReportAccessRightID = value
        End Set
    End Property

    Public Property ReportID() As Long
        Get
            Return mReportID
        End Get
        Set(ByVal value As Long)
            mReportID = value
        End Set
    End Property

    Public Property UserID() As Long
        Get
            Return mUserID
        End Get
        Set(ByVal value As Long)
            mUserID = value
        End Set
    End Property

    Public Property UserType() As String
        Get
            Return mUserType
        End Get
        Set(ByVal value As String)
            mUserType = value
        End Set
    End Property

    Public Property CreatedDate() As String
        Get
            Return mCreatedDate
        End Get
        Set(ByVal value As String)
            mCreatedDate = value
        End Set
    End Property

    Public Property CreatedBy() As Long
        Get
            Return mCreatedBy
        End Get
        Set(ByVal value As Long)
            mCreatedBy = value
        End Set
    End Property

    Public Property UpdatedDate() As String
        Get
            Return mUpdatedDate
        End Get
        Set(ByVal value As String)
            mUpdatedDate = value
        End Set
    End Property

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
        End Set
    End Property

#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        Dim factory As DatabaseProviderFactory = New DatabaseProviderFactory()
        db = factory.Create(ConnectionName)

    End Sub

#End Region

    Public Sub Clear()

        mReportAccessRightID = 0
        mReportID = 0
        mUserID = 0
        mUserType = ""
        mCreatedDate = ""
        mCreatedBy = mObjectUserID
        mUpdatedDate = ""
        mUpdatedBy = 0

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mReportAccessRightID)

    End Function

    Public Overridable Function Retrieve(ByVal ReportAccessRightID As Long) As Boolean

        Dim sql As String

        If ReportAccessRightID > 0 Then
            sql = "SELECT * FROM tblUserReportAccessRights WHERE ReportAccessRightID = " & ReportAccessRightID
        Else
            sql = "SELECT * FROM tblUserReportAccessRights WHERE ReportAccessRightID = " & mReportAccessRightID
        End If

        Return Retrieve(sql)

    End Function

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean

        Try

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then

                'LoadDataRecord(dsRetrieve.Tables(0).Rows(0))

                dsRetrieve = Nothing
                Return True

            Else

                'SetErrorDetails("UserReportAccessRight not found.")

                Return False

            End If

        Catch e As Exception

            'SetErrorDetails(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetUserReportAccessRight() As System.Data.DataSet

        Return GetUserReportAccessRight(mReportAccessRightID)

    End Function

    Public Overridable Function GetUserReportAccessRight(ByVal ReportAccessRightID As Long) As DataSet

        Dim sql As String

        If ReportAccessRightID > 0 Then
            sql = "SELECT * FROM tblUserReportAccessRights WHERE ReportAccessRightID = " & ReportAccessRightID
        Else
            sql = "SELECT * FROM tblUserReportAccessRights WHERE ReportAccessRightID = " & mReportAccessRightID
        End If

        Return GetUserReportAccessRight(sql)

    End Function

    Protected Overridable Function GetUserReportAccessRight(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAllRoles() As DataSet

        Dim sql As String = "SELECT * FROM luUserGroups"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetSelectedUserGroupReportRights(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.tblReports WHERE ReportID IN (SELECT ReportID FROM tblUserReportAccessRights where UserID=" & UserGroupID & " AND UserType='UserGroup') "

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAvailableUserGroupReportRights(ByVal UserGroupID As Long) As DataSet

        Dim sql As String = "SELECT * FROM dbo.tblReports WHERE ReportID not in (SELECT ReportID FROM tblUserReportAccessRights where UserID=" & UserGroupID & " AND UserType='UserGroup')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetAvailableReportRights(ByVal UserID As Long, ByVal UserType As String) As DataSet

        Dim sql As String = "SELECT * FROM dbo.tblReports WHERE ReportID not in (SELECT ReportID FROM tblUserReportAccessRights where UserID=" & UserID & " AND UserType='" & UserType & "')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetSelectedReportRights(ByVal UserID As Long, ByVal UserType As String) As DataSet

        Dim sql As String = "SELECT * FROM dbo.tblReports WHERE ReportID IN (SELECT ReportID FROM tblUserReportAccessRights where UserID=" & UserID & " AND UserType='" & UserType & "')"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function SaveDetail(ByVal UserID As Long, ByVal ReportID As Long, ByVal UserType As String) As Boolean

        Dim InsertSQL As String = "INSERT INTO tblUserReportAccessRights ([UserID], [ReportID],[UserType]) VALUES (" & UserID & "," & ReportID & ",'" & UserType & "')"

        db.ExecuteNonQuery(CommandType.Text, InsertSQL)

        Return True

    End Function

    Public Function Revoke(ByVal UserID As Long, ByVal UserType As String) As Boolean

        Dim sql As String = "DELETE FROM tblUserReportAccessRights where UserID=" & UserID & " AND UserType='" & UserType & "'"

        Return db.ExecuteNonQuery(CommandType.Text, sql)

    End Function

    Public Function DeleteDetail(ByVal UserID As Integer, ByVal ReportID As Integer, ByVal UserType As String) As Boolean

        Dim DeleteSQL As String = "DELETE FROM tblUserReportAccessRights WHERE UserID = " & UserID & " AND ReportID =" & ReportID & " AND UserType='" & UserType & "'"

        db.ExecuteNonQuery(CommandType.Text, DeleteSQL)

        Return True

    End Function


#End Region

    'Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

    '    With Record

    '        mReportAccessRightID = Catchnull(.Item("ReportAccessRightID"), 0)
    '        mReportID = Catchnull(.Item("ReportID"), 0)
    '        mUserID = Catchnull(.Item("UserID"), 0)
    '        mUserType = Catchnull(.Item("UserType"), "")
    '        mUserType = Catchnull(.Item("UserType"), )
    '        mCreatedDate = Catchnull(.Item("CreatedDate"), "")
    '        mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
    '        mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
    '        mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)

    '    End With

    'End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@ReportAccessRightID", DBType.Int32, mReportAccessRightID)
        db.AddInParameter(cmd, "@ReportID", DBType.Int32, mReportID)
        db.AddInParameter(cmd, "@UserID", DBType.Int32, mUserID)
        db.AddInParameter(cmd, "@UserType", DBType.String, mUserType)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_UserReportAccessRight")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If (ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0) Then

                mReportAccessRightID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            ' SetErrorDetails(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblUserReportAccessRights SET Deleted = 1 WHERE ReportAccessRightID = " & mReportAccessRightID) 
        Return Delete("DELETE FROM tblUserReportAccessRights WHERE ReportAccessRightID = " & mReportAccessRightID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            ' SetErrorDetails(e)
            Return False

        End Try

    End Function

#End Region

#End Region

End Class
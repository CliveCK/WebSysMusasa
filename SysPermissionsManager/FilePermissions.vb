Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class FilePermissions

#region "Variables"

    Protected mDocumentPermissionID As long
    Protected mDocumentID As long
    Protected mLevelOfSecurityID As long
    Protected mObjectID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

    Protected db As Database 
    Protected mConnectionName As String 
    Protected mObjectUserID As Long 

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#end region

#region "Properties"

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

    Public  Property DocumentPermissionID() As long
        Get
		return mDocumentPermissionID
        End Get
        Set(ByVal value As long)
		mDocumentPermissionID = value
        End Set
    End Property

    Public  Property DocumentID() As long
        Get
		return mDocumentID
        End Get
        Set(ByVal value As long)
		mDocumentID = value
        End Set
    End Property

    Public  Property LevelOfSecurityID() As long
        Get
		return mLevelOfSecurityID
        End Get
        Set(ByVal value As long)
		mLevelOfSecurityID = value
        End Set
    End Property

    Public  Property ObjectID() As long
        Get
		return mObjectID
        End Get
        Set(ByVal value As long)
		mObjectID = value
        End Set
    End Property

    Public  Property CreatedBy() As long
        Get
		return mCreatedBy
        End Get
        Set(ByVal value As long)
		mCreatedBy = value
        End Set
    End Property

    Public  Property UpdatedBy() As long
        Get
		return mUpdatedBy
        End Get
        Set(ByVal value As long)
		mUpdatedBy = value
        End Set
    End Property

    Public  Property CreatedDate() As string
        Get
		return mCreatedDate
        End Get
        Set(ByVal value As string)
		mCreatedDate = value
        End Set
    End Property

    Public  Property UpdatedDate() As string
        Get
		return mUpdatedDate
        End Get
        Set(ByVal value As string)
		mUpdatedDate = value
        End Set
    End Property

#end region

#region "Methods"

#Region "Constructors" 
 
    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long) 

        mObjectUserID = ObjectUserID 
        mConnectionName = ConnectionName 
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub 

#End Region 

Public Sub Clear()  

    DocumentPermissionID = 0
    mDocumentID = 0
    mLevelOfSecurityID = 0
    mObjectID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mDocumentPermissionID) 

    End Function 

    Public Overridable Function Retrieve(ByVal DocumentPermissionID As Long) As Boolean 

        Dim sql As String 

        If DocumentPermissionID > 0 Then 
            sql = "SELECT * FROM tblDocumentPermissions WHERE DocumentPermissionID = " & DocumentPermissionID
        Else 
            sql = "SELECT * FROM tblDocumentPermissions WHERE DocumentPermissionID = " & mDocumentPermissionID
        End If 

        Return Retrieve(sql) 

    End Function 

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean 

        Try 

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql) 

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then 

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0)) 

                dsRetrieve = Nothing 
                Return True 

            Else 

                log.error("FilePermissions not found.")

                Return False

            End If

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetFilePermissions() As System.Data.DataSet

        Return GetFilePermissions(mDocumentPermissionID)

    End Function

    Public Overridable Function GetFilePermissions(ByVal DocumentPermissionID As Long) As DataSet

        Dim sql As String

        If DocumentPermissionID > 0 Then
            sql = "SELECT * FROM tblDocumentPermissions WHERE DocumentPermissionID = " & DocumentPermissionID
        Else
            sql = "SELECT * FROM tblDocumentPermissions WHERE DocumentPermissionID = " & mDocumentPermissionID
        End If

        Return GetFilePermissions(sql)

    End Function

    Protected Overridable Function GetFilePermissions(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetPermittedUsersOrOrganizations(ByVal FileID As Long, ByVal LevelOfSecurity As Long)

        Dim sql As String = ""

        Select Case LevelOfSecurity

            Case LevelOfSecurityEnum.LevelOfSecurity.User

                sql = "select StaffID As ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname, '') As Name from tblStaffMembers where StaffID in (SELECT ObjectID FROM tblDocumentPermissions WHERE DocumentID = " & FileID & " AND LevelOfSecurityID = " & LevelOfSecurity & ")"

            Case LevelOfSecurityEnum.LevelOfSecurity.Organization

                sql = "select OrganizationID as ObjectID, Name from tblOrganization where OrganizationID in (SELECT ObjectID FROM tblDocumentPermissions WHERE DocumentID = " & FileID & " AND LevelOfSecurityID = " & LevelOfSecurity & ")"

        End Select

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetForbiddenUsersOrOrganizations(ByVal FileID As Long, ByVal LevelOfSecurity As Long)

        Dim sql As String = ""

        Select Case LevelOfSecurity

            Case LevelOfSecurityEnum.LevelOfSecurity.User

                sql = "select StaffID As ObjectID, ISNULL(FirstName, '') + ' ' + ISNULL(Surname, '') As Name from tblStaffMembers where StaffID NOT IN (SELECT ObjectID FROM tblDocumentPermissions WHERE DocumentID = " & FileID & " AND LevelOfSecurityID = " & LevelOfSecurity & ")"

            Case LevelOfSecurityEnum.LevelOfSecurity.Organization

                sql = "select OrganizationID as ObjectID, Name from tblOrganization where OrganizationID NOT IN (SELECT ObjectID FROM tblDocumentPermissions WHERE DocumentID = " & FileID & " AND LevelOfSecurityID = " & LevelOfSecurity & ")"

        End Select

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mDocumentPermissionID = Catchnull(.Item("DocumentPermissionID"), 0)
            mDocumentID = Catchnull(.Item("DocumentID"), 0)
            mLevelOfSecurityID = Catchnull(.Item("LevelOfSecurityID"), 0)
            mObjectID = Catchnull(.Item("ObjectID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@DocumentPermissionID", DBType.Int32, mDocumentPermissionID)
        db.AddInParameter(cmd, "@DocumentID", DBType.Int32, mDocumentID)
        db.AddInParameter(cmd, "@LevelOfSecurityID", DBType.Int32, mLevelOfSecurityID)
        db.AddInParameter(cmd, "@ObjectID", DBType.Int32, mObjectID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_FilePermissions")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mDocumentPermissionID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            log.error(ex)
            Return False

        End Try

    End Function

    Public Function SaveDetail(ByVal ObjectID As Long, ByVal FileID As Long, ByVal LevelOfSecurity As String) As Boolean

        Dim InsertSQL As String = "INSERT INTO tblDocumentPermissions ([ObjectID], [DocumentID],[LevelOfSecurityID]) VALUES (" & ObjectID & "," & FileID & "," & LevelOfSecurity & ")"

        db.ExecuteNonQuery(CommandType.Text, InsertSQL)

        Return True

    End Function

    Public Function Revoke(ByVal FileID As Long, ByVal LevelOfSecurity As Long) As Boolean

        Dim sql As String = "DELETE FROM tblDocumentPermissions WHERE DocumentID = " & FileID & " AND LevelOfSecurityID = " & LevelOfSecurity

        Return db.ExecuteNonQuery(CommandType.Text, sql)

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblDocumentPermissions SET Deleted = 1 WHERE DocumentPermissionID = " & mDocumentPermissionID) 
        Return Delete("DELETE FROM tblDocumentPermissions WHERE DocumentPermissionID = " & mDocumentPermissionID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            log.error(e)
            Return False

        End Try

    End Function

#End Region

#end region

End Class
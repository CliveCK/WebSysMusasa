Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions

Public Class AdminSettings

#Region "Variables"

    Protected mAdminSettingID As Long
    Protected mPasswordExpires As Boolean
    Protected mPasswordExpirationDays As Long
    Protected mCreatedDate As String
    Protected mCreatedBy As Long
    Protected mUpdatedDate As String
    Protected mUpdatedBy As Long
    Protected mAttempts As Long
    Protected mMinPasswordLength As Long
    Protected mLockOutTime As Long

    Protected db As Database
    Protected mConnectionName As String
    Protected mObjectUserID As Long

    Protected mPasswordTemplateID As Long
    Protected mPasswordPolicyMsg As String

    Public Property ErrorMessage As String = String.Empty
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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

    Public Overridable Property AdminSettingID() As Long
        Get
            Return mAdminSettingID
        End Get
        Set(ByVal value As Long)
            mAdminSettingID = value
        End Set
    End Property

    Public Property PasswordExpires() As Boolean
        Get
            Return mPasswordExpires
        End Get
        Set(ByVal value As Boolean)
            mPasswordExpires = value
        End Set
    End Property

    Public Property PasswordExpirationDays() As Long
        Get
            Return mPasswordExpirationDays
        End Get
        Set(ByVal value As Long)
            mPasswordExpirationDays = value
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

    Public Property Attempts() As Long
        Get
            Return mAttempts
        End Get
        Set(ByVal value As Long)
            mAttempts = value
        End Set
    End Property

    Public Property MinPasswordLength() As Long
        Get
            Return mMinPasswordLength
        End Get
        Set(ByVal value As Long)
            mMinPasswordLength = value
        End Set
    End Property

    Public Property LockOutTime() As Long
        Get
            Return mLockOutTime
        End Get
        Set(ByVal value As Long)
            mLockOutTime = value
        End Set
    End Property

    Public Property PasswordTemplateID() As Long
        Get
            Return mPasswordTemplateID
        End Get
        Set(ByVal value As Long)
            mPasswordTemplateID = value
        End Set
    End Property

    Public Property PasswordPolicyMsg() As String
        Get
            Return mPasswordPolicyMsg
        End Get
        Set(ByVal value As String)
            mPasswordPolicyMsg = value
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

    Public Shadows Sub Clear()

        mAdminSettingID = 0
        mPasswordExpires = False
        mPasswordExpirationDays = 0
        mCreatedDate = ""
        mCreatedBy = mObjectUserID
        mUpdatedDate = ""
        mUpdatedBy = mObjectUserID
        mAttempts = 0
        mMinPasswordLength = 0
        mLockOutTime = 0

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mAdminSettingID)

    End Function

    Public Overridable Function Retrieve(ByVal AdminSettingID As Long) As Boolean

        Dim sql As String

        If AdminSettingID > 0 Then
            sql = "SELECT * FROM tblAdminSettings WHERE AdminSettingID = " & AdminSettingID
        Else
            sql = "SELECT * FROM tblAdminSettings WHERE AdminSettingID = " & mAdminSettingID
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

                ErrorMessage = "Admin Settings not found."
                log.Error(ErrorMessage)

                Return False

            End If

        Catch e As Exception

            ErrorMessage = e.Message
            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetAdminSettings() As System.Data.DataSet

        Return GetAdminSettings(mAdminSettingID)

    End Function

    Public Overridable Function GetAdminSettings(ByVal AdminSettingID As Long) As DataSet

        Dim sql As String

        If AdminSettingID > 0 Then
            sql = "SELECT * FROM tblAdminSettings WHERE AdminSettingID = " & AdminSettingID
        Else
            sql = "SELECT * FROM tblAdminSettings WHERE AdminSettingID = " & mAdminSettingID
        End If

        Return GetAdminSettings(sql)

    End Function

    Protected Overridable Function GetAdminSettings(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function LoadAdminSettings() As DataSet

        Dim sql As String = "SELECT * FROM tblAdminSettings"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function ReturnAdminSettings() As Boolean

        Dim sql As String = "SELECT * FROM tblAdminSettings"

        Return Retrieve(sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mAdminSettingID = Catchnull(.Item("AdminSettingID"), 0)
            mPasswordExpires = Catchnull(.Item("PasswordExpires"), False)
            mPasswordExpirationDays = Catchnull(.Item("PasswordExpirationDays"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mAttempts = Catchnull(.Item("Attempts"), 0)
            mMinPasswordLength = Catchnull(.Item("MinPasswordLength"), 0)
            mLockOutTime = Catchnull(.Item("LockOutTime"), 0)
            mPasswordTemplateID = Catchnull(.Item("PasswordTemplateID"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@AdminSettingID", DBType.Int32, mAdminSettingID)
        db.AddInParameter(cmd, "@PasswordExpires", DBType.Boolean, mPasswordExpires)
        db.AddInParameter(cmd, "@PasswordExpirationDays", DBType.Int32, mPasswordExpirationDays)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Attempts", DbType.Int32, mAttempts)
        db.AddInParameter(cmd, "@MinPasswordLength", DbType.Int32, mMinPasswordLength)
        db.AddInParameter(cmd, "@LockOutTime", DbType.Int32, LockOutTime)
        db.AddInParameter(cmd, "@PasswordTemplateID", DbType.Int32, PasswordTemplateID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_AdminSettings")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If (ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0) Then

                mAdminSettingID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblAdminSettings SET Deleted = 1 WHERE AdminSettingID = " & mAdminSettingID) 
        Return Delete("DELETE FROM tblAdminSettings WHERE AdminSettingID = " & mAdminSettingID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            ErrorMessage = e.Message
            log.Error(e)
            Return False

        End Try

    End Function

#End Region

#End Region

End Class

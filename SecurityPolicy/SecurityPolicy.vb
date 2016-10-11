Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions

Public Class SecurityPolicy

#Region "Variables"

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected mPasswordPolicyID As Long
    Protected mPasswordValidityPeriod As Long
    Protected mMinPasswordLength As Long
    Protected mMaxPasswordLength As Long
    Protected mNumericLength As Long
    Protected mUpperCaseLength As Long
    Protected mSpecialCharacterLength As Long
    Protected mPasswordHistory As Long
    Protected mPasswordExpires As Integer
    Protected mCreatedBy As Long
    Protected mUpdatedBy As Long
    Protected mCreatedDate As String
    Protected mUpdatedDate As String
    Protected mUseDictionary As Boolean
    Protected mIsActive As Boolean
    Protected mName As String
    Protected mDescription As String
    Protected mSpecialCharacters As String

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

    Public Property PasswordPolicyID() As Long
        Get
            Return mPasswordPolicyID
        End Get
        Set(ByVal value As Long)
            mPasswordPolicyID = value
        End Set
    End Property

    Public Property PasswordValidityPeriod() As Long
        Get
            Return mPasswordValidityPeriod
        End Get
        Set(ByVal value As Long)
            mPasswordValidityPeriod = value
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

    Public Property MaxPasswordLength() As Long
        Get
            Return mMaxPasswordLength
        End Get
        Set(ByVal value As Long)
            mMaxPasswordLength = value
        End Set
    End Property

    Public Property NumericLength() As Long
        Get
            Return mNumericLength
        End Get
        Set(ByVal value As Long)
            mNumericLength = value
        End Set
    End Property

    Public Property UpperCaseLength() As Long
        Get
            Return mUpperCaseLength
        End Get
        Set(ByVal value As Long)
            mUpperCaseLength = value
        End Set
    End Property

    Public Property SpecialCharacterLength() As Long
        Get
            Return mSpecialCharacterLength
        End Get
        Set(ByVal value As Long)
            mSpecialCharacterLength = value
        End Set
    End Property

    Public Property PasswordExpires() As Int16
        Get
            Return mPasswordExpires
        End Get
        Set(value As Int16)
            mPasswordExpires = value
        End Set
    End Property

    Public Property PasswordHistory() As Long
        Get
            Return mPasswordHistory
        End Get
        Set(ByVal value As Long)
            mPasswordHistory = value
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

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
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

    Public Property UpdatedDate() As String
        Get
            Return mUpdatedDate
        End Get
        Set(ByVal value As String)
            mUpdatedDate = value
        End Set
    End Property

    Public Property UseDictionary() As Boolean
        Get
            Return mUseDictionary
        End Get
        Set(ByVal value As Boolean)
            mUseDictionary = value
        End Set
    End Property

    Public Property IsActive() As Boolean
        Get
            Return mIsActive
        End Get
        Set(ByVal value As Boolean)
            mIsActive = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal value As String)
            mDescription = value
        End Set
    End Property

    Public Property SpecialCharacters() As String
        Get
            Return mSpecialCharacters
        End Get
        Set(ByVal value As String)
            mSpecialCharacters = value
        End Set
    End Property

#End Region

#Region "Methods"

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub

#End Region

    Public Sub Clear()

        PasswordPolicyID = 0
        mPasswordValidityPeriod = 0
        mMinPasswordLength = 0
        mMaxPasswordLength = 0
        mNumericLength = 0
        mUpperCaseLength = 0
        mSpecialCharacterLength = 0
        mPasswordHistory = 0
        mPasswordExpires = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mCreatedDate = ""
        mUpdatedDate = ""
        mUseDictionary = False
        mIsActive = False
        mName = ""
        mDescription = ""
        mSpecialCharacters = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mPasswordPolicyID)

    End Function

    Public Overridable Function Retrieve(ByVal PasswordPolicyID As Long) As Boolean

        Dim sql As String

        If PasswordPolicyID > 0 Then
            sql = "SELECT * FROM tblPasswordPolicies WHERE PasswordPolicyID = " & PasswordPolicyID
        Else
            sql = "SELECT * FROM tblPasswordPolicies WHERE PasswordPolicyID = " & mPasswordPolicyID
        End If

        Return Retrieve(sql)

    End Function

    Public Overridable Function RetrieveAll() As DataSet

        Dim sql As String

        sql = "SELECT * FROM tblPasswordPolicies Order by IsActive desc"

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function RetrieveActiveSecurityPolicy() As Boolean

        Dim Sql As String = "Select TOP 1 * from tblPasswordPolicies where isActive = 1"

        Try

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, Sql)

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0))

                dsRetrieve = Nothing
                Return True

            Else

                log.Error("SecurityPolicy not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean

        Try

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0))

                dsRetrieve = Nothing
                Return True

            Else

                log.Error("SecurityPolicy not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetSecurityPolicy() As System.Data.DataSet

        Return GetSecurityPolicy(mPasswordPolicyID)

    End Function

    Public Overridable Function GetSecurityPolicy(ByVal PasswordPolicyID As Long) As DataSet

        Dim sql As String

        If PasswordPolicyID > 0 Then
            sql = "SELECT * FROM tblPasswordPolicies WHERE PasswordPolicyID = " & PasswordPolicyID
        Else
            sql = "SELECT * FROM tblPasswordPolicies WHERE PasswordPolicyID = " & mPasswordPolicyID
        End If

        Return GetSecurityPolicy(sql)

    End Function

    Protected Overridable Function GetSecurityPolicy(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function CheckPasswordHistory(ByVal Password As String, ByVal UserID As Long, ByVal PasswordHistory As Long) As Boolean 'true : Password used before

        Dim ds As DataSet
        Dim sql As String = "Select TOP " & PasswordHistory & " [Password] from tblUserPasswordHistory where UserID = " & UserID & " order by CreatedDate desc"

        ds = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables(0).Select("Password = '" & Password & "'").Length > 0 Then

            Return True

        End If

        Return False

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mPasswordPolicyID = Catchnull(.Item("PasswordPolicyID"), 0)
            mPasswordValidityPeriod = Catchnull(.Item("PasswordValidityPeriod"), 0)
            mMinPasswordLength = Catchnull(.Item("MinPasswordLength"), 0)
            mMaxPasswordLength = Catchnull(.Item("MaxPasswordLength"), 0)
            mNumericLength = Catchnull(.Item("NumericLength"), 0)
            mUpperCaseLength = Catchnull(.Item("UpperCaseLength"), 0)
            mSpecialCharacterLength = Catchnull(.Item("SpecialCharacterLength"), 0)
            mPasswordHistory = Catchnull(.Item("PasswordHistory"), 0)
            mPasswordExpires = Catchnull(.Item("PasswordExpires"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mUseDictionary = Catchnull(.Item("UseDictionary"), False)
            mIsActive = Catchnull(.Item("IsActive"), False)
            mName = Catchnull(.Item("Name"), "")
            mDescription = Catchnull(.Item("Description"), "")
            mSpecialCharacters = Catchnull(.Item("SpecialCharacters"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@PasswordPolicyID", DbType.Int32, mPasswordPolicyID)
        db.AddInParameter(cmd, "@PasswordValidityPeriod", DbType.Int32, mPasswordValidityPeriod)
        db.AddInParameter(cmd, "@MinPasswordLength", DbType.Int32, mMinPasswordLength)
        db.AddInParameter(cmd, "@MaxPasswordLength", DbType.Int32, mMaxPasswordLength)
        db.AddInParameter(cmd, "@NumericLength", DbType.Int32, mNumericLength)
        db.AddInParameter(cmd, "@UpperCaseLength", DbType.Int32, mUpperCaseLength)
        db.AddInParameter(cmd, "@SpecialCharacterLength", DbType.Int32, mSpecialCharacterLength)
        db.AddInParameter(cmd, "@PasswordHistory", DbType.Int32, mPasswordHistory)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@UseDictionary", DbType.Boolean, mUseDictionary)
        db.AddInParameter(cmd, "@IsActive", DbType.Boolean, mIsActive)
        db.AddInParameter(cmd, "@Name", DbType.String, mName)
        db.AddInParameter(cmd, "@Description", DbType.String, mDescription)
        db.AddInParameter(cmd, "@SpecialCharacters", DbType.String, mSpecialCharacters)
        db.AddInParameter(cmd, "@PasswordExpires", DbType.Int16, mPasswordExpires)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_SecurityPolicy")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mPasswordPolicyID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblPasswordPolicies SET Deleted = 1 WHERE PasswordPolicyID = " & mPasswordPolicyID) 
        Return Delete("DELETE FROM tblPasswordPolicies WHERE PasswordPolicyID = " & mPasswordPolicyID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

#End Region

#Region "Miscellaneous"

    Public Function DeactivateActiveProfiles(ByVal PasswordPolicyID As Int32) As Boolean 'If one profile is to be set active, deactivate the current active

        Dim sql As String = "Update tblPasswordPolicies set isActive = 0 where PasswordPolicyID <> " & PasswordPolicyID

        Return db.ExecuteNonQuery(CommandType.Text, sql)

    End Function

#End Region

#End Region

End Class
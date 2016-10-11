Imports System.Data.SqlClient
Imports System.Web.Security
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Universal.CommonFunctions
Imports System.Configuration.Provider
Imports System.Security.Cryptography
Imports System.Net
Imports System.Runtime.Serialization
Imports System.IO
Imports System.Data.Common

Public Class UserManager

    Public Enum MessageTypeEnum As Long

        [Error] = 0
        Warning = 1
        Information = 2
        Message = 3
        Ajax = 4
        Debug = 5
        Fatal = 6

    End Enum

    Public Event ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

#Region "Variables"

    Private mUserID As String
    Private mUsername As String
    Private mPassword As String
    Private mUserFirstName As String
    Private mUserSurname As String
    Private mEmailAddress As String
    Private mMobileNo As String
    Private mJobTitle As String
    Private mPermisionID As String
    Private mLogID As String
    Private mUserUserGroupID As Long
    Protected mApplicationID As Long
    Protected mUserKey As Guid = Guid.NewGuid

    Protected mPasswordExpires As Boolean
    Protected mPasswordExpirationDays As Long
    Protected mPasswordQuestion As String
    Protected mComment As String
    Protected mIsApproved As Boolean
    Protected mIsLockedOut As Boolean
    Protected mLastLoginDate As String
    Protected mLastActivityDate As String
    Protected mLastPasswordChangeDate As String
    Protected mLastLockoutDate As String
    Protected mMemberID As Long
    Protected mFailedPasswordAttemptCount As Long

    Private mDeleted As Boolean
    Private mCreatedBy As Integer
    Private mUpdatedBy As Integer
    Private mCreatedDate As String
    Private mUpdatedDate As String

    Private DefaultDateTime As DateTime = New DateTime(1754, 1, 1).ToUniversalTime()

    Public Const MAXPASSWORDATTEMPTS As Long = 3

    Private db As Database
    Private mConnectionName As String
    Private mObjectUserID As Long

    Public Property ErrorMessage As String = String.Empty
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Properties"

    Public Property UserID() As Integer
        Get
            Return mUserID
        End Get
        Set(ByVal Value As Integer)
            mUserID = Value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return mUsername
        End Get
        Set(ByVal Value As String)
            mUsername = Value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return mPassword
        End Get
        Set(ByVal Value As String)
            mPassword = Value
        End Set
    End Property

    Public Property UserFirstName() As String
        Get
            Return mUserFirstName
        End Get
        Set(ByVal Value As String)
            mUserFirstName = Value
        End Set
    End Property

    Public Property UserSurname() As String
        Get
            Return mUserSurname
        End Get
        Set(ByVal Value As String)
            mUserSurname = Value
        End Set
    End Property

    Public Sub ResetPassword()

        Dim sql As String = "UPDATE tblUsers SET [Password] = '" & mPassword & "', [LastPasswordChangeDate] = '" & Date.Today & "', PasswordExpires = 0 WHERE [Username] = '" & mUsername & "'"
        db.ExecuteNonQuery(CommandType.Text, sql)

    End Sub

    Public ReadOnly Property FullName() As String
        Get
            Return mUserFirstName & " " & mUserSurname
        End Get
    End Property

    Public Property JobTitle() As String
        Get
            Return mJobTitle
        End Get
        Set(ByVal Value As String)
            mJobTitle = Value
        End Set
    End Property

    Public Property EmailAddress() As String
        Get
            Return mEmailAddress
        End Get
        Set(ByVal Value As String)
            mEmailAddress = Value
        End Set
    End Property

    Public Property MobileNo() As String
        Get
            Return mMobileNo
        End Get
        Set(ByVal Value As String)
            mMobileNo = Value
        End Set
    End Property

    Public ReadOnly Property FriendlyEmailAddress() As String
        Get
            Return String.Format("{0} <{1}>", FullName, mEmailAddress)
        End Get
    End Property

    Public Property LogID() As Integer
        Get
            Return mLogID
        End Get
        Set(ByVal Value As Integer)
            mLogID = Value
        End Set
    End Property

    Public Property PermisionID() As Integer
        Get
            Return mPermisionID
        End Get
        Set(ByVal Value As Integer)
            mPermisionID = Value
        End Set
    End Property

    Public Property UserUserGroupID() As Integer
        Get
            Return mUserUserGroupID
        End Get
        Set(ByVal Value As Integer)
            mUserUserGroupID = Value
        End Set
    End Property

    Public Property PasswordQuestion() As String
        Get
            Return mPasswordQuestion
        End Get
        Set(ByVal value As String)
            mPasswordQuestion = value
        End Set
    End Property

    Public Property Comment() As String
        Get
            Return mComment
        End Get
        Set(ByVal value As String)
            mComment = value
        End Set
    End Property

    Public Property IsApproved() As Boolean
        Get
            Return mIsApproved
        End Get
        Set(ByVal value As Boolean)
            mIsApproved = value
        End Set
    End Property

    Public Property IsLockedOut() As Boolean
        Get
            Return mIsLockedOut
        End Get
        Set(ByVal value As Boolean)
            mIsLockedOut = value
        End Set
    End Property

    Public Property LastLoginDate() As String
        Get
            Return mLastLoginDate
        End Get
        Set(ByVal value As String)
            mLastLoginDate = value
        End Set
    End Property

    Public Property LastActivityDate() As String
        Get
            Return mLastActivityDate
        End Get
        Set(ByVal value As String)
            mLastActivityDate = value
        End Set
    End Property

    Public ReadOnly Property MustChangePassword() As Boolean
        Get

            Return Now > NextPasswordChangeDate

        End Get
    End Property

    Public ReadOnly Property NextPasswordChangeDate() As String
        Get

            If IsDate(mLastPasswordChangeDate) AndAlso CDate(mLastPasswordChangeDate) <> Date.MinValue Then

                Dim lpcd As Date = CDate(mLastPasswordChangeDate)
                lpcd = lpcd.Subtract(lpcd.TimeOfDay) 'remove the time part of the date

                Return lpcd.AddMonths(PolicyObjects.Security.PasswordExpiryPeriod)

            Else 'just return a date in the future

                Return Now.AddDays(1)

            End If

        End Get
    End Property

    Public Property LastPasswordChangeDate() As String
        Get
            Return mLastPasswordChangeDate
        End Get
        Set(ByVal value As String)
            mLastPasswordChangeDate = value
        End Set
    End Property

    Public Property LastLockoutDate() As String
        Get
            Return mLastLockoutDate
        End Get
        Set(ByVal value As String)
            mLastLockoutDate = value
        End Set
    End Property

    Public ReadOnly Property ConnectionName() As String
        Get
            Return mConnectionName
        End Get
    End Property

    Public Property UserKey() As Guid
        Get
            Return mUserKey
        End Get
        Set(ByVal value As Guid)
            mUserKey = value
        End Set
    End Property

    Public Property Deleted() As Boolean
        Get
            Return mDeleted
        End Get
        Set(ByVal value As Boolean)
            mDeleted = value
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

    Public Property ApplicationID() As Long
        Get
            Return mApplicationID
        End Get
        Set(ByVal value As Long)
            mApplicationID = value
        End Set
    End Property

    Public Property MemberId() As Long
        Get
            Return mMemberID
        End Get
        Set(ByVal value As Long)
            mMemberID = value
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

    Public ReadOnly Property IsExpiredPassword() As Boolean
        Get

            If IsDate(mLastPasswordChangeDate) AndAlso CDate(mLastPasswordChangeDate) <> Date.MinValue Then

                Dim lpcd As Date = CDate(mLastPasswordChangeDate)
                Dim ExpiryDate As Date = lpcd.AddDays(mPasswordExpirationDays)

                If Now > ExpiryDate Then

                    Return True

                Else : Return False

                End If
            Else
                Return False
            End If

        End Get
    End Property

    Public Property FailedPasswordAttemptCount() As Integer
        Get
            Return mFailedPasswordAttemptCount
        End Get
        Set(ByVal Value As Integer)
            mFailedPasswordAttemptCount = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        Dim factory As DatabaseProviderFactory = New DatabaseProviderFactory()
        db = factory.Create(ConnectionName)

    End Sub

#End Region

#Region "Methods"

    Public Function GetEmailAddress() As System.Net.Mail.MailAddress
        Return New System.Net.Mail.MailAddress(EmailAddress, FullName)
    End Function

    Public Shadows Sub Clear()

        mUserID = 0
        mPermisionID = 0
        mCreatedBy = mObjectUserID
        mUpdatedBy = 0
        mCreatedDate = Nothing
        mUpdatedDate = Nothing
        mDeleted = False
        mUsername = ""
        mPassword = ""
        mUserFirstName = ""
        mUserSurname = ""
        mEmailAddress = ""
        mMobileNo = ""
        mJobTitle = ""
        mPasswordQuestion = ""
        mComment = ""
        mIsApproved = False
        mIsLockedOut = False
        mLastLoginDate = Nothing
        mLastActivityDate = Nothing
        mLastPasswordChangeDate = Nothing
        mLastLockoutDate = Nothing
        mApplicationID = -1
        mUserKey = Guid.NewGuid

    End Sub

#Region "Retrieve Overloads"

    Public Function Retrieve() As Boolean

        Return Me.Retrieve(mUserID)

    End Function

    Public Function Retrieve(ByVal UserID As Long) As Boolean

        Dim sql As String

        If UserID > 0 Then
            sql = "SELECT * FROM tblUsers WHERE UserID = " & UserID
        Else
            sql = "SELECT * FROM tblUsers WHERE UserID = " & mUserID
        End If

        Return Retrieve(sql)

    End Function

    Public Function RetrieveUserGroup(ByVal UserID As Long) As Boolean

        Dim sql As String

        If UserID > 0 Then
            sql = "SELECT * FROM tblUserUserGroups WHERE UserID = " & UserID & " AND UserGroupID = 1"
        Else
            sql = "SELECT * FROM tblUserUserGroups WHERE UserID = " & mUserID & " AND UserGroupID = 1"
        End If

        Return db.ExecuteDataSet(CommandType.Text, sql).Tables(0).Rows.Count > 0

    End Function

    Public Function RetrieveByEmail(ByVal Email As String) As Boolean

        Dim sql As String = "SELECT * FROM tblUsers WHERE EmailAddress = @Email"

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

        db.AddInParameter(cmd, "@Email", DbType.String, Email)

        Return Retrieve(cmd)

    End Function

    Public Function RetrieveByUsername(ByVal Username As String, ByVal UserIsOnline As Boolean) As Boolean

        If RetrieveByUsername(Username) Then

            If UserIsOnline Then

                'TODO: Tafadzwa Moyo
                'update last logged on date
                '.UpdateLastLoggedOnDate

                Return True

            End If

            Return True

        Else

            Return False

        End If

    End Function

    Public Function RetrieveByUsername(ByVal Username As String) As Boolean

        Dim sql As String = "SELECT * FROM tblUsers WHERE Username = @Username"

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

        db.AddInParameter(cmd, "@Username", DbType.String, Username)

        Return Retrieve(cmd)

    End Function

    Public Function RetrieveByUserKey() As Boolean

        Return RetrieveByUserKey(mUserKey)

    End Function

    Public Function RetrieveByUserKey(ByVal UserKey As Guid) As Boolean

        Dim sql As String = "SELECT * FROM tblUsers WHERE UserKey = @UserKey"

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

        db.AddInParameter(cmd, "@UserKey", DbType.Guid, UserKey)

        Return Retrieve(cmd)

    End Function

    Private Function Retrieve(ByVal sql As String) As Boolean

        Try

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

            If dsRetrieve.Tables(0).Rows.Count > 0 Then

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0))

                dsRetrieve = Nothing
                Return True

            Else

                ErrorMessage = "UserManager not found."
                log.Info(ErrorMessage)
                Return False

            End If

        Catch e As Exception

            ErrorMessage = e.Message
            log.Error(e)
            Return False

        End Try

    End Function

    Private Function Retrieve(ByVal cmd As Common.DbCommand) As Boolean

        Try

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(cmd)

            If dsRetrieve.Tables(0).Rows.Count > 0 Then

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0))

                dsRetrieve = Nothing
                Return True

            Else

                ErrorMessage = "UserManager not found."
                log.Debug(ErrorMessage)
                Return False

            End If

        Catch e As Exception

            ErrorMessage = e.Message
            log.Error(e)
            Return False

        End Try

    End Function

    Public Function GetNumberOfUsersOnline(ByVal UserIsOnlineTimeWindow As Long) As Integer

        Try

            Dim sql As String = String.Format("SELECT COUNT(*) FROM tblUsers WHERE DATEADD(minute,{0},LastActivityDate) >= getdate()", UserIsOnlineTimeWindow)

            Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

            Return db.ExecuteScalar(cmd)

        Catch ex As Exception

            Return -1

        End Try

    End Function

#End Region

    Public Function LoadNewFromDataRow(ByVal row As DataRow) As UserManager

        Try

            Dim objUser As New UserManager(mConnectionName, mObjectUserID)

            objUser.LoadDataRecord(row)

            Return objUser

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return Nothing

        End Try

    End Function

    Protected Friend Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mUserID = Catchnull(.Item("UserID"), 0)
            mPermisionID = Catchnull(.Item("PermisionID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), Date.MinValue)
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), Date.MinValue)
            mDeleted = Catchnull(.Item("Deleted"), False)
            mUsername = Catchnull(.Item("Username"), "")
            mPassword = Catchnull(.Item("Password"), "")
            mUserFirstName = Catchnull(.Item("UserFirstName"), "")
            mUserSurname = Catchnull(.Item("UserSurname"), "")
            mEmailAddress = Catchnull(.Item("EmailAddress"), "")
            mJobTitle = Catchnull(.Item("JobTitle"), "")
            mPasswordQuestion = Catchnull(.Item("PasswordQuestion"), "")
            mComment = Catchnull(.Item("Comment"), "")
            mIsApproved = Catchnull(.Item("IsApproved"), False)
            mIsLockedOut = Catchnull(.Item("IsLockedOut"), False)
            mLastLoginDate = Catchnull(.Item("LastLoginDate"), Date.MinValue)
            mLastActivityDate = Catchnull(.Item("LastActivityDate"), Date.MinValue)
            mLastPasswordChangeDate = Catchnull(.Item("LastPasswordChangeDate"), mCreatedDate) 'assume password last changed at creation
            mLastLockoutDate = Catchnull(.Item("LastLockoutDate"), Date.MinValue)
            mApplicationID = Catchnull(.Item("ApplicationID"), -1)
            mUserKey = Catchnull(.Item("UserKey"), Guid.NewGuid)
            mMemberID = Catchnull(.Item("MemberID"), 0)
            mMobileNo = Catchnull(.Item("MobileNo"), 0)
            mPasswordExpires = Catchnull(.Item("PasswordExpires"), False)
            mPasswordExpirationDays = (Catchnull(.Item("PasswordExpirationDays"), 0))
            mFailedPasswordAttemptCount = (Catchnull(.Item("FailedPasswordAttemptCount"), 0))


        End With

    End Sub

    'Public Function AuthenticateRoles(ByVal UserID As Long) As Boolean

    '    Dim sql As String = "SELECT UserID FROM tblUserRoles  WHERE [UserID] = " & UserID & " AND RoleID = 1" ' Assuming Administrator is always 1

    '    Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

    '    If ds.Tables(0).Rows.Count > 0 Then

    '        Return True

    '    Else : Return False

    '    End If

    'End Function

    '
    ' UpdateFailureCount
    '   A helper method that performs the checks and updates associated with
    ' password failure tracking.
    '

    Private Sub UpdateFailureCount(ByVal username As String, ByVal failureType As String)

        Dim cmd As Data.Common.DbCommand = db.GetSqlStringCommand("SELECT FailedPasswordAttemptCount, " & _
                                          "  ISNULL(FailedPasswordAttemptWindowStart,Getdate()) As FailedPasswordAttemptWindowStart, " & _
                                          "  FailedPasswordAnswerAttemptCount, " & _
                                          "  ISNULL(FailedPasswordAnswerAttemptWindowStart,Getdate()) AS FailedPasswordAnswerAttemptWindowStart  " & _
                                          "  FROM tblUsers " & _
                                          "  WHERE Username = @Username ")

        db.AddInParameter(cmd, "@Username", DbType.String, username)
        ' db.AddInParameter(cmd, "@ApplicationID", DbType.String, IIf(Trim(mApplicationID) = "", DBNull.Value, mApplicationID))

        Dim reader As SqlDataReader = Nothing
        Dim refReader As RefCountingDataReader = Nothing
        Dim windowStart As DateTime = New DateTime()
        Dim failureCount As Integer = 0

        Try

            refReader = db.ExecuteReader(cmd)
            reader = refReader.InnerReader

            If reader.HasRows Then

                reader.Read()

                If failureType = "password" Then
                    failureCount = reader.GetInt32(0)
                    windowStart = reader.GetDateTime(1)
                End If

                If failureType = "passwordAnswer" Then
                    failureCount = reader.GetInt32(2)
                    windowStart = reader.GetDateTime(3)
                End If

            End If

            reader.Close()

            Dim windowEnd As DateTime = windowStart.AddMinutes(PolicyObjects.Security.PasswordAttemptWindow)

            If failureCount = 0 Then

                ' First password failure or outside of PasswordAttemptWindow. 
                ' Start a New password failure count from 1 and a New window starting now.
                failureCount = 1

                If failureType = "password" Then _
                  cmd.CommandText = "UPDATE tblUsers " & _
                                    "  SET FailedPasswordAttemptCount = @AttemptCount, " & _
                                    "      FailedPasswordAttemptWindowStart = @WindowStart " & _
                                    "  WHERE Username = @Username "

                If failureType = "passwordAnswer" Then _
                  cmd.CommandText = "UPDATE tblUsers " & _
                                    "  SET FailedPasswordAnswerAttemptCount = @AttemptCount, " & _
                                    "      FailedPasswordAnswerAttemptWindowStart = @WindowStart " & _
                                    "  WHERE Username = @Username "

                cmd.Parameters.Clear()

                db.AddInParameter(cmd, "@AttemptCount", DbType.Int32, failureCount)
                db.AddInParameter(cmd, "@WindowStart", DbType.DateTime, DateTime.Now)
                db.AddInParameter(cmd, "@Username", DbType.String, username)
                '         db.AddInParameter(cmd, "@ApplicationID", DbType.String, IIf(Trim(mApplicationID) = "", DBNull.Value, mApplicationID))

                If db.ExecuteNonQuery(cmd) < 0 Then _
                  Throw New ProviderException("Unable to update failure count and window start.")
                mFailedPasswordAttemptCount = failureCount

            Else

                failureCount += 1
                Dim myAdmin As New AdminSettings(mConnectionName, mObjectUserID)
                myAdmin.ReturnAdminSettings()

                If failureCount >= IIf(myAdmin.Attempts = 0, PolicyObjects.Security.MaxInvalidPasswordAttempts, myAdmin.Attempts) Then

                    ' Password attempts have exceeded the failure threshold. Lock out the user.

                    cmd.CommandText = "UPDATE tblUsers " & _
                                      "  SET IsLockedOut = @IsLockedOut, LastLockOutDate = @LastLockOutDate,FailedPasswordAttemptCount = @AttemptCount " & _
                                      "  WHERE Username = @Username "

                    cmd.Parameters.Clear()

                    db.AddInParameter(cmd, "@IsLockedOut", DbType.Boolean, True)
                    db.AddInParameter(cmd, "@LastLockOutDate", DbType.DateTime, DateTime.Now)
                    db.AddInParameter(cmd, "@AttemptCount", DbType.Int32, failureCount)
                    db.AddInParameter(cmd, "@Username", DbType.String, username)
                    '  db.AddInParameter(cmd, "@ApplicationID", DbType.String, IIf(Trim(mApplicationID) = "", DBNull.Value, mApplicationID))

                    If db.ExecuteNonQuery(cmd) < 0 Then _
                          Throw New System.Configuration.Provider.ProviderException("Unable to lock out user.")
                    mFailedPasswordAttemptCount = failureCount
                    mIsLockedOut = True
                Else

                    ' Password attempts have not exceeded the failure threshold. Update
                    ' the failure counts. Leave the window the same.

                    If failureType = "password" Then _
                      cmd.CommandText = "UPDATE tblUsers " & _
                                        "  SET FailedPasswordAttemptCount = @AttemptCount" & _
                                        "  WHERE Username = @Username "

                    If failureType = "passwordAnswer" Then _
                      cmd.CommandText = "UPDATE tblUsers " & _
                                        "  SET FailedPasswordAnswerAttemptCount = @AttemptCount" & _
                                        "  WHERE Username = @Username "

                    cmd.Parameters.Clear()

                    db.AddInParameter(cmd, "@AttemptCount", DbType.Int32, failureCount)
                    db.AddInParameter(cmd, "@Username", DbType.String, username)
                    '  db.AddInParameter(cmd, "@ApplicationID", DbType.String, IIf(Trim(mApplicationID) = "", DBNull.Value, mApplicationID))

                    If db.ExecuteNonQuery(cmd) < 0 Then _
                          Throw New Exception("Unable to update failure count.")

                    mFailedPasswordAttemptCount = failureCount

                End If

            End If

        Catch e As SqlException

            ErrorMessage = e.Message
            log.Error(e)

        Finally

            If Not reader Is Nothing Then Try : reader.Close() : Catch : End Try

        End Try

    End Sub

    Public Function UpdateLastLogin(ByVal username As String, ByVal failureType As String) As Boolean
        Try

            Dim cmd As Common.DbCommand = db.GetSqlStringCommand("UPDATE tblUsers SET LastLoginDate = getdate()" & _
                                      "  WHERE Username = @Username ")

            db.AddInParameter(cmd, "@Username", DbType.String, mUsername)
            db.AddInParameter(cmd, "@ApplicationID", DbType.String, IIf(Trim(mApplicationID) = "", DBNull.Value, mApplicationID))

            If db.ExecuteNonQuery(cmd) < 0 Then
                Throw New Exception("Unable to update last login details.")
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            ErrorMessage = ex.Message
            log.Error(ex)
            Return True
        End Try
    End Function

    Public Function UnlockUser(ByVal userName As String) As Boolean
        Try

            Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("UPDATE tblUsers SET IsLocked = @IslockedOut WHERE UserName = @UserName")


            db.AddInParameter(cmd, "@IslockedOut", DbType.Boolean, False)
            db.AddInParameter(cmd, "@UserName", DbType.String, userName)

            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception
            ErrorMessage = ex.Message
            log.Error(ex)
            Return False
        End Try
    End Function

    Public Function ValidateUsingPasswordAnswer(ByVal UserName As String, ByVal Password As String) As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_GetUser_UsingAnswer")

        db.AddInParameter(cmd, "@UserName", DbType.String, UserName)
        db.AddInParameter(cmd, "@Answer", DbType.String, Password)

        Dim ds As DataSet = db.ExecuteDataSet(cmd)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables(0).Rows(0)("UserID")) AndAlso Not (ds.Tables(0).Rows(0)("Deleted")) Then

            LoadDataRecord(ds.Tables(0).Rows(0))
            UpdateLastLogin(UserName, mApplicationID)
            Return True

        Else

            UpdateFailureCount(UserName, "passwordAnswer")
            Return False

        End If

    End Function

    Public Function ValidateUser() As Boolean

        Return ValidateUsingPassword(mUsername, mPassword)

    End Function

    Public Function ValidateUser(ByVal Username As String, ByVal Password As String) As Boolean

        Return ValidateHash(Username, Password, True)

    End Function

    Public Function ValidateUserAccount() As Boolean

        Dim sql As String = String.Empty

        sql = "SELECT * FROM tblUsers WHERE UserName=@UserName AND Password=@Password"

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

        db.AddInParameter(cmd, "@UserName", DbType.String, mUsername)
        db.AddInParameter(cmd, "@Password", DbType.String, mPassword)

        Dim ds As DataSet = db.ExecuteDataSet(cmd)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables(0).Rows(0)("UserID")) AndAlso Not (ds.Tables(0).Rows(0)("Deleted")) Then
            Return True
        Else

            Return False
        End If

    End Function

    Private Function ValidateUsingPassword(ByVal UserName As String, ByVal Password As String) As Boolean

        Dim sql As String = String.Empty
        If My.Computer.Name.ToUpper.Equals("DOCWIZEDEV") Then
            sql = "SELECT TOP 1 * FROM tblUsers WHERE UserName=@UserName"
        Else
            sql = "SELECT * FROM tblUsers WHERE UserName=@UserName AND Password=@Password"
        End If

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

        db.AddInParameter(cmd, "@UserName", DbType.String, UserName)
        db.AddInParameter(cmd, "@Password", DbType.String, Password)

        Dim ds As DataSet = db.ExecuteDataSet(cmd)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables(0).Rows(0)("UserID")) AndAlso Not (ds.Tables(0).Rows(0)("Deleted")) Then

            LoadDataRecord(ds.Tables(0).Rows(0))
            'UpdateLastLogin(mUsername, mApplicationID)
            Return True
        Else
            UpdateFailureCount(UserName, "password")
            Return False
        End If

    End Function

    Public Function DeleteUser() As Boolean

        Return DeleteUser(mUserID)

    End Function

    Public Function DeleteUser(ByVal UserID As String) As Boolean

        Dim sql As String = "UPDATE [tblUsers] SET Deleted=1 WHERE UserID=" & UserID
        If db.ExecuteNonQuery(CommandType.Text, sql) Then

            Return True

        Else : Return False

        End If

    End Function

    Public Function DeactivateUserByUserID(ByVal UserID As String) As Boolean

        Dim str As String = "UPDATE [tblUsers] SET Deleted = 1 WHERE UserID = @UserID"
        Dim sqlStringCommand As DbCommand = Me.db.GetSqlStringCommand(str)
        Me.db.AddInParameter(sqlStringCommand, "@UserID", DbType.[String], UserID)

        Try

            Me.db.ExecuteNonQuery(sqlStringCommand)

            Return True

        Catch exception As Exception

            log.Error(exception)
            Return False

        End Try

        Return False

    End Function

    Public Function ActivateUserByUserID(ByVal UserID As String) As Boolean

        Dim str As String = "UPDATE [tblUsers] SET Deleted = 0 WHERE UserID = @UserID"
        Dim sqlStringCommand As DbCommand = Me.db.GetSqlStringCommand(str)
        Me.db.AddInParameter(sqlStringCommand, "@UserID", DbType.[String], UserID)

        Try

            Me.db.ExecuteNonQuery(sqlStringCommand)

            Return True

        Catch exception As Exception

            log.Error(exception)
            Return False

        End Try

        Return False

    End Function

    Public Function DeleteByUserName() As Boolean

        Return DeleteByUserName(mUsername)

    End Function

    Public Function DeleteByUserName(ByVal Username As String) As Boolean

        Dim sql As String = "UPDATE [tblUsers] SET Deleted = 1 WHERE Username = @Username"

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

        db.AddInParameter(cmd, "@Username", DbType.String, Username)

        Try

            db.ExecuteNonQuery(CommandType.Text, sql)
            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return False

        End Try

    End Function

    Public Function ChangePasswordQuestionAndAnswer(ByVal newPwdQuestion As String, ByVal newPwdAnswer As String) As Boolean
        Try
            Dim sql As String = ""

            sql &= "UPDATE tblUsers SET  " & vbCrLf
            sql &= "	PasswordQuestion = @PasswordQuestion,  " & vbCrLf
            sql &= "	PasswordAnswer = @PasswordAnswer,  " & vbCrLf
            sql &= "	FailedPasswordAttemptCount = 0,  " & vbCrLf
            sql &= "	FailedPasswordAttemptWindowStart = NULL,  " & vbCrLf
            sql &= "	FailedPasswordAnswerAttemptCount = 0,  " & vbCrLf
            sql &= "	FailedPasswordAnswerAttemptWindowStart = NULL " & vbCrLf
            sql &= "WHERE UserID = @UserID " & vbCrLf

            Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

            db.AddInParameter(cmd, "@PasswordQuestion", DbType.String, newPwdQuestion)
            db.AddInParameter(cmd, "@PasswordAnswer", DbType.String, newPwdAnswer) 'TODO: Encrypt
            db.AddInParameter(cmd, "@UserID", DbType.Int32, UserID)

            db.ExecuteNonQuery(cmd)
            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return False

        End Try
    End Function

    Public Sub ChangePassword(ByVal UserID As Long, ByVal Password As String)

        Dim sql As String

        sql = "UPDATE tblUsers SET  " & vbCrLf
        sql &= "	[Password] = @Password,  " & vbCrLf
        sql &= "	FailedPasswordAttemptCount = 0,  " & vbCrLf
        sql &= "	FailedPasswordAttemptWindowStart = NULL  " & vbCrLf
        sql &= "WHERE UserID = @UserID " & vbCrLf

        Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

        db.AddInParameter(cmd, "@Password", DbType.String, Password)
        db.AddInParameter(cmd, "@UserID", DbType.Int32, UserID)

        db.ExecuteNonQuery(cmd)

    End Sub

    Public Sub UpdateDetails(ByVal UserID As Long, ByVal EmailAddress As String, ByVal Surname As String, ByVal FirstName As String, ByVal MobileNo As String, ByVal PasswordExpires As Boolean, ByVal PasswordExpirationDays As Long)

        Dim sql As String = "UPDATE tblUsers SET  [EmailAddress] = '" & EmailAddress & "',[UserFirstname] = '" & FirstName & "', [UserSurname] = '" & Surname & "',[PasswordExpires] = '" & PasswordExpires & "',[PasswordExpirationDays] = '" & PasswordExpirationDays & "', [MobileNo] = '" & MobileNo.Replace("'", "''") & "' WHERE [UserID] = " & UserID
        db.ExecuteNonQuery(CommandType.Text, sql)

    End Sub

    Public Sub UpdateDetails(ByVal UserID As Long, ByVal EmailAddress As String, ByVal Surname As String, ByVal FirstName As String, ByVal MobileNo As String)

        Dim sql As String = "UPDATE tblUsers SET  [EmailAddress] = '" & EmailAddress & "',[UserFirstname] = '" & FirstName & "', [UserSurname] = '" & Surname & "', [MobileNo] = '" & MobileNo.Replace("'", "''") & "' WHERE [UserID] = " & UserID
        db.ExecuteNonQuery(CommandType.Text, sql)

    End Sub

    Public Function CreateUser() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_User")

        db.AddInParameter(cmd, "@UserID", DbType.Int32, mUserID)
        db.AddInParameter(cmd, "@PermisionID", DbType.Int32, mPermisionID)
        db.AddInParameter(cmd, "@Deleted", DbType.Boolean, 0)
        db.AddInParameter(cmd, "@Username", DbType.String, mUsername)
        db.AddInParameter(cmd, "@Password", DbType.String, mPassword)
        db.AddInParameter(cmd, "@UserFirstName", DbType.String, mUserFirstName)
        db.AddInParameter(cmd, "@UserSurname", DbType.String, mUserSurname)
        db.AddInParameter(cmd, "@EmailAddress", DbType.String, mEmailAddress)
        db.AddInParameter(cmd, "@MobileNo", DbType.String, mMobileNo)
        db.AddInParameter(cmd, "@PasswordExpires", DbType.Boolean, mPasswordExpires)
        db.AddInParameter(cmd, "@PasswordExpirationDays", DbType.Int32, mPasswordExpirationDays)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@MemberID", DbType.Int32, 0)
        db.AddInParameter(cmd, "@IsLockedOut", DbType.Boolean, mIsLockedOut)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If (ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0) Then

                mUserID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return False

        End Try

    End Function

    Public Function VerifyPasswordValidity() As Boolean
        Try

            Return IsExpiredPassword()

        Catch ex As Exception
            ErrorMessage = ex.Message
            log.Error(ex)
            Return True
        End Try

    End Function

#Region "UserGroup Methods"

    Public Function SaveUserUserGroup(ByVal UserGroupID As Integer) As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_UserUserGroup")

        db.AddInParameter(cmd, "@UserUserGroupID", DbType.Int32, 0)
        db.AddInParameter(cmd, "@UserID", DbType.Int32, mUserID)
        db.AddInParameter(cmd, "@UserGroupID", DbType.Int32, UserGroupID)
        db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If (ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0) Then

                mUserUserGroupID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return False

        End Try

    End Function

    Public Function DeleteUserUserGroups() As Boolean

        Try

            Dim DeleteSQL As String = "Delete From tblUserUserGroups where UserID = " & mUserID

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)

            ErrorMessage = ("User's Groups deleted.")
            log.Debug(EmailAddress)
            Return True

        Catch e As Exception

            ErrorMessage = e.Message
            log.Error(e)
            Return False

        End Try

    End Function

    Public Function CreateUserGroup(ByVal Description As String, Optional ByVal UserGroupId As Integer = 0) As Boolean

        Try

            If UserGroupId <= 0 Then

                Dim InsertSQL As String = "INSERT INTO luUserGroups (Description) VALUES ('" & Description & "') "

                db.ExecuteDataSet(CommandType.Text, InsertSQL)

                Return True

            ElseIf UserGroupId > 0 Then

                Dim updateSQL As String = "UPDATE luUserGroups SET Description = '" & Description & "' WHERE UserGroupID = " & UserGroupId

                db.ExecuteDataSet(CommandType.Text, updateSQL)

                Return True
            Else
                Return False
            End If


        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)

            Return False

        End Try
    End Function

    Public Function DeleteUserGroup(ByVal userGroupId As Integer) As Boolean
        Try

            Dim sql As String = "Delete From luUserGroups where UserGroupID = " & userGroupId

            db.ExecuteDataSet(CommandType.Text, sql)

            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)

            Return False

        End Try

    End Function

    Public Function GetUserUsergroups(ByVal UserID As Long) As DataSet

        Dim sql As String = "SELECT * FROM tblUserUsergroups WHERE UserID=" & UserID

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetUserUsergroupsDescriptions(ByVal UserID As Long) As DataSet

        Dim sql As String = "SELECT * FROM tblUserUsergroups UG inner join luUserGroups U on UG.UserGroupID = U.UserGroupID WHERE UserID=" & UserID

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Sub DeleteUserGroupRoles(ByVal GroupID As Integer)

        Dim DeleteSQL As String = "Delete From tblUserGroupRoles Where GroupID = " & GroupID

        db.ExecuteDataSet(CommandType.Text, DeleteSQL)
    End Sub

    Public Function SaveUserGroupRoles(ByVal GroupID As Integer, ByVal RoleID As Integer) As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_UserGroupRoles")

        db.AddInParameter(cmd, "@GroupRoleID", DbType.Int32, 0)
        db.AddInParameter(cmd, "@GroupID", DbType.Int32, GroupID)
        db.AddInParameter(cmd, "@RoleID", DbType.Int32, RoleID)
        db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return False

        End Try
    End Function

#End Region

    Public Function SaveUserLog(ByVal UserID As Integer) As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_LogUserLogs")

        db.AddInParameter(cmd, "@UserID", DbType.Int32, UserID)
        db.AddInParameter(cmd, "@LogID", DbType.Int32, mLogID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, UserID)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mLogID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Function UpdateUserLockStatus(ByVal UserID As Long, ByVal LockStatus As Boolean) As DataSet
        Try
            Dim sql As String
            If LockStatus = 1 Then 'Failed password attempts locking the user account
                sql = "UPDATE tblUsers SET IsLockedOut = @IslockedOut, LastLockoutDate = @LastLockoutDate WHERE UserID= @UserID"


                Dim DB As Database = New DatabaseProviderFactory().Create(mConnectionName)

                Dim cmd As System.Data.Common.DbCommand = DB.GetSqlStringCommand(sql)

                DB.AddInParameter(cmd, "@IslockedOut", DbType.Boolean, LockStatus)
                DB.AddInParameter(cmd, "@LastLockoutDate", DbType.DateTime, Now.ToLongDateString)
                DB.AddInParameter(cmd, "@UserID", DbType.String, UserID)

                Return DB.ExecuteDataSet(CommandType.Text, sql)

            Else 'Success password or administrator unlocking of user account 
                sql = "UPDATE tblUsers SET IsLockedOut = @IslockedOut, FailedPasswordAttemptCount = 0 WHERE UserID= @UserID"

                Dim DB As Database = New DatabaseProviderFactory().Create(mConnectionName)

                Dim cmd As System.Data.Common.DbCommand = DB.GetSqlStringCommand(sql)

                DB.AddInParameter(cmd, "@IslockedOut", DbType.Boolean, LockStatus)
                DB.AddInParameter(cmd, "@UserID", DbType.String, UserID)

                Return DB.ExecuteDataSet(CommandType.Text, sql)

            End If


        Catch ex As Exception

            Return Nothing
        End Try

    End Function

    Public Function FindUsers(ByVal pnlFindUsers As System.Web.UI.WebControls.Panel) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, BuildSearchSQL(pnlFindUsers))

    End Function

    Public Function FindUser(ByVal UserID As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblUsers WHERE UserID = " & UserID)

    End Function

    Public Function FindUsersByName(ByVal UsernameMatch As String) As DataSet

        Try

            Dim sql As String = "SELECT * FROM tblUsers WHERE Username LIKE @UsernameMatch"

            Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

            db.AddInParameter(cmd, "@UsernameMatch", DbType.String, UsernameMatch)

            Return db.ExecuteDataSet(cmd)

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return Nothing

        End Try

    End Function

    Public Function FindUsersByEmail(ByVal EmailAddressMatch As String) As DataSet

        Try

            Dim sql As String = "SELECT * FROM tblUsers WHERE EmailAddress LIKE @EmailAddressMatch"

            Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(sql)

            db.AddInParameter(cmd, "@EmailAddressMatch", DbType.String, EmailAddressMatch)

            Return db.ExecuteDataSet(cmd)

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return Nothing

        End Try

    End Function

    Public Function GetAllUsers() As DataSet

        Return db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblUsers")

    End Function

    Public Function BuildSearchSQL(ByVal pnlSearchCriteria As System.Web.UI.WebControls.Panel) As String

        Dim sqlCriteria As String = " [Type] = 'SystemUser' " ' only show system users

        For Each ctrl As System.Web.UI.Control In pnlSearchCriteria.Controls

            If TypeOf ctrl Is System.Web.UI.WebControls.TextBox Then

                Dim myTextBox As System.Web.UI.WebControls.TextBox = CType(ctrl, System.Web.UI.WebControls.TextBox)

                Dim criteria As String = myTextBox.Text

                If myTextBox.ID = "txtUsername" AndAlso Trim(myTextBox.Text) <> "" Then

                    If criteria.IndexOfAny("%*") >= 0 Then
                        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " Username LIKE '" & criteria & "'" 'contains search
                    Else
                        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " Username LIKE '%" & criteria & "%'" 'contains search
                    End If

                End If

                If myTextBox.ID = "txtFirstName" AndAlso Trim(myTextBox.Text) <> "" Then
                    If criteria.IndexOfAny("%*") >= 0 Then
                        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " UserFirstname LIKE '" & criteria & "'" 'contains search
                    Else
                        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " UserFirstname LIKE '%" & criteria & "%'" 'contains search
                    End If
                End If

                If myTextBox.ID = "txtSurname" AndAlso Trim(myTextBox.Text) <> "" Then
                    If criteria.IndexOfAny("%*") >= 0 Then
                        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " UserSurname LIKE '" & criteria & "'" 'contains search
                    Else
                        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " UserSurname LIKE '%" & criteria & "%'" 'contains search
                    End If
                End If

                If myTextBox.ID = "txtEmailAddress" AndAlso Trim(myTextBox.Text) <> "" Then
                    If criteria.IndexOfAny("%*") >= 0 Then
                        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " EmailAddress LIKE '" & criteria & "'" 'contains search
                    Else
                        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " EmailAddress LIKE '%" & criteria & "%'" 'contains search
                    End If
                End If

                'If myTextBox.ID = "txtJobTitle" AndAlso Trim(myTextBox.Text) <> "" Then
                '    If criteria.IndexOfAny("%*") >= 0 Then
                '        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " JobTitle LIKE '" & criteria & "'" 'contains search
                '    Else
                '        sqlCriteria &= IIf(sqlCriteria.Trim = "", "", " AND ") & " JobTitle LIKE '%" & criteria & "%'" 'contains search
                '    End If
                'End If

            End If

        Next

        If sqlCriteria = "" Then

            Return "SELECT UserID, Username, UserFirstname, UserSurname, EmailAddress, Deleted FROM tblUsers  WHERE MemberID IS NULL "

        Else

            Return "SELECT UserID, Username, UserFirstname, UserSurname, EmailAddress, Deleted FROM tblUsers " & IIf(sqlCriteria = "", "", " WHERE " & sqlCriteria) & " "

        End If

    End Function

    Public Function GetLastLogin(ByVal UserID As Integer) As String

        Dim sql As String = "SELECT TOP 2 [LoginDate] FROM tblUserLogs WHERE UserID = " & UserID & " ORDER BY LoginDate DESC"

        Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

        If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            Return ds.Tables(0).Rows(1)(0)
        Else
            Return ""
        End If

    End Function

#Region "Password Encyrption"

    Public Function PasswordHash(ByVal username As String, ByVal UserPassword As String) As String

        Return GenerateHashDigest(username.ToLower + UserPassword)

    End Function

    Private Function ValidateHash(ByVal Username As String, ByVal UserPassword As String, Optional ByVal dbHashedPwdSupplied As Boolean = False) As Boolean

        Try
            Dim dbHashedPassword As String = ""

            If dbHashedPwdSupplied Then
                Dim dsUser As DataSet = db.ExecuteDataSet(CommandType.Text, "SELECT * FROM tblUsers where Username = '" & Username & "'")
                If (dsUser.Tables.Count > 0 AndAlso dsUser.Tables(0).Rows.Count > 0) Then

                    LoadDataRecord(dsUser.Tables(0).Rows(0))
                    dbHashedPassword = dsUser.Tables(0).Rows(0)("Password")

                End If
            Else

                dbHashedPassword = mPassword

            End If

            ' Create an Encoding object so that you can use the convenient GetBytes 
            ' method to obtain byte arrays.
            Dim uEncode As New Text.UnicodeEncoding()

            Dim bytHashOriginal As Byte() = uEncode.GetBytes(dbHashedPassword)

            Dim strHashForCompare As String = GenerateHashDigest(Username.ToLower + UserPassword)
            ' From the new hash digest create a byte array for comparison with the
            ' original hash digest byte array.
            Dim bytHashForCompare As Byte() = uEncode.GetBytes(strHashForCompare)
            ' Display the new hash digest in a TextBox.

            'Loop through all the bytes in the hashed values.
            Dim i As Integer
            For i = 0 To bytHashOriginal.Length - 1
                If bytHashOriginal(i) <> bytHashForCompare(i) Then

                    Return False

                Else
                    ' Every byte matched so the "transmitted" XML has been authenticated.

                End If
            Next
            ' Compare each byte. If any do not match display an appropriate message
            ' and exit the loop.
            Return True

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return False

        End Try

    End Function

    Private Function GenerateHashDigest(ByVal strSource As String) As String
        ' Create an Encoding object so that you can use the convenient GetBytes 
        ' method to obtain byte arrays.

        Try

            Dim hash As Byte()
            Dim salt As String = "Spec@#*9" 'salt to be added to the Username and Password combination
            Dim uEncode As New Text.UnicodeEncoding()
            ' Create a byte array from the source text passed as an argument.
            Dim bytPassword() As Byte = uEncode.GetBytes(strSource & salt)

            Dim sha384 As New SHA384Managed()
            hash = sha384.ComputeHash(bytPassword)

            ' Base64 is a method of encoding binary data as ASCII text.
            Return Convert.ToBase64String(hash)

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return Nothing

        End Try

    End Function

#End Region

#Region "Recovery"

    Public Event RecoverPasswordMessageNeeded(ByRef body As String, ByVal PasswordChangeArgs As String)
    Public Event RecoverUsernameMessageNeeded(ByRef body As String, ByVal PasswordChangeArgs As String)

    <Serializable()> _
    Public Structure ChangePasswordArguments

        Public UserID As String
        Public Expirydate As DateTime
        Public CompanyLogin As String

    End Structure

    Function GeneratePassword(ByVal length As Integer, ByVal numberOfNonAlphanumericCharacters As Integer) As String

        'Make sure length and numberOfNonAlphanumericCharacters are valid....
        If ((length < 1) OrElse (length > 128)) Then
            Throw New ArgumentException("Membership_password_length_incorrect")
        End If

        If ((numberOfNonAlphanumericCharacters > length) OrElse (numberOfNonAlphanumericCharacters < 0)) Then
            Throw New ArgumentException("Membership_min_required_non_alphanumeric_characters_incorrect")
        End If

        Do While True

            Dim i As Integer
            Dim nonANcount As Integer = 0
            Dim buffer1 As Byte() = New Byte(length - 1) {}

            'chPassword contains the password's characters as it's built up
            Dim chPassword As Char() = New Char(length - 1) {}

            'chPunctionations contains the list of legal non-alphanumeric characters
            Dim chPunctuations As Char() = "!@@$%^^*()_-+=[{]};:>|./?".ToCharArray()

            'Get a cryptographically strong series of bytes
            Dim rng As New System.Security.Cryptography.RNGCryptoServiceProvider
            rng.GetBytes(buffer1)

            For i = 0 To length - 1
                'Convert each byte into its representative character
                Dim rndChr As Integer = (buffer1(i) Mod 87)
                If (rndChr < 10) Then
                    chPassword(i) = Convert.ToChar(Convert.ToUInt16(48 + rndChr))
                Else
                    If (rndChr < 36) Then
                        chPassword(i) = Convert.ToChar(Convert.ToUInt16((65 + rndChr) - 10))
                    Else
                        If (rndChr < 62) Then
                            chPassword(i) = Convert.ToChar(Convert.ToUInt16((97 + rndChr) - 36))
                        Else
                            chPassword(i) = chPunctuations(rndChr - 62)
                            nonANcount += 1
                        End If
                    End If
                End If
            Next

            If nonANcount < numberOfNonAlphanumericCharacters Then
                Dim rndNumber As New Random
                For i = 0 To (numberOfNonAlphanumericCharacters - nonANcount) - 1
                    Dim passwordPos As Integer
                    Do
                        passwordPos = rndNumber.Next(0, length)
                    Loop While Not Char.IsLetterOrDigit(chPassword(passwordPos))
                    chPassword(passwordPos) = chPunctuations(rndNumber.Next(0, chPunctuations.Length))
                Next
            End If

            Return New String(chPassword)

        Loop

        Return (New Guid).ToString().Substring(0, length)

    End Function

    Public Function LoadPasswordChangeObject(ByVal token As String) As ChangePasswordArguments

        Try

            Dim arr() As Byte = Convert.FromBase64String(token)

            Dim ms As New MemoryStream
            ms.Write(arr, 0, arr.Length)
            ms.Seek(0, SeekOrigin.Begin)

            Dim formatter As IFormatter = New Formatters.Binary.BinaryFormatter()

            Dim cp As ChangePasswordArguments = CType(formatter.Deserialize(ms), ChangePasswordArguments)

            Return cp

        Catch ex As Exception

            ErrorMessage = ex.Message
            log.Error(ex)
            Return Nothing

        End Try

    End Function

    Private Function GetPasswordChangeArguments() As String

        Dim objCPArguments As New ChangePasswordArguments
        With objCPArguments
            .UserID = UserID
            .Expirydate = Now.AddDays(2)
            .CompanyLogin = mConnectionName
        End With

        Dim formatter As IFormatter = New Formatters.Binary.BinaryFormatter()
        Dim ms As New MemoryStream
        formatter.Serialize(ms, objCPArguments)
        Return Convert.ToBase64String(ms.ToArray())

    End Function

    Public Function RecoverPassword(ByVal Username As String, ByVal Subject As String) As Boolean

        If RetrieveByUsername(Username) Then

            Dim body As String = ""

            RaiseEvent RecoverPasswordMessageNeeded(body, GetPasswordChangeArguments())

            Dim msg As New Mail.MailMessage
            With msg

                .IsBodyHtml = True
                .Body = body
                .To.Add(New Mail.MailAddress(EmailAddress, FullName))
                .Subject = Subject

            End With

            Dim smtpClient As New Mail.SmtpClient
            smtpClient.Send(msg)

            Return True

        Else

            Return False

        End If

    End Function

    Public Function RecoverUsername(ByVal EmailAddress As String, ByVal Subject As String) As Boolean

        If RetrieveByEmail(EmailAddress) Then

            Dim body As String = ""

            RaiseEvent RecoverUsernameMessageNeeded(body, GetPasswordChangeArguments())

            Dim msg As New Mail.MailMessage
            With msg

                .IsBodyHtml = True
                .Body = body
                .To.Add(New Mail.MailAddress(EmailAddress, FullName))
                .Subject = Subject

            End With

            Dim smtpClient As New Mail.SmtpClient
            smtpClient.Send(msg)

            Return True

        Else

            Return False

        End If

    End Function

#End Region


#End Region

End Class

Public Class UserPasswordHistory

#Region "Variables"

    Protected mPasswordHistoryID As Long
    Protected mUserID As Long
    Protected mUpdatedBy As Long
    Protected mCreatedBy As Long
    Protected mCreatedDate As String
    Protected mUpdatedDate As String
    Protected mUsername As String
    Protected mPassword As String

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

    Public Property PasswordHistoryID() As Long
        Get
            Return mPasswordHistoryID
        End Get
        Set(ByVal value As Long)
            mPasswordHistoryID = value
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

    Public Property UpdatedBy() As Long
        Get
            Return mUpdatedBy
        End Get
        Set(ByVal value As Long)
            mUpdatedBy = value
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

    Public Property Username() As String
        Get
            Return mUsername
        End Get
        Set(ByVal value As String)
            mUsername = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return mPassword
        End Get
        Set(ByVal value As String)
            mPassword = value
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

        PasswordHistoryID = 0
        mUserID = 0
        mUpdatedBy = 0
        mCreatedBy = mObjectUserID
        mCreatedDate = ""
        mUpdatedDate = ""
        mUsername = ""
        mPassword = ""

    End Sub

#Region "Retrieve Overloads"

    Public Overridable Function Retrieve() As Boolean

        Return Me.Retrieve(mPasswordHistoryID)

    End Function

    Public Overridable Function Retrieve(ByVal PasswordHistoryID As Long) As Boolean

        Dim sql As String

        If PasswordHistoryID > 0 Then
            sql = "SELECT * FROM tblUserPasswordHistory WHERE PasswordHistoryID = " & PasswordHistoryID
        Else
            sql = "SELECT * FROM tblUserPasswordHistory WHERE PasswordHistoryID = " & mPasswordHistoryID
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

                Return False

            End If

        Catch e As Exception
            Return False

        End Try

    End Function

    Public Overridable Function GetUserPasswordHistory() As System.Data.DataSet

        Return GetUserPasswordHistory(mPasswordHistoryID)

    End Function

    Public Overridable Function GetUserPasswordHistory(ByVal PasswordHistoryID As Long) As DataSet

        Dim sql As String

        If PasswordHistoryID > 0 Then
            sql = "SELECT * FROM tblUserPasswordHistory WHERE PasswordHistoryID = " & PasswordHistoryID
        Else
            sql = "SELECT * FROM tblUserPasswordHistory WHERE PasswordHistoryID = " & mPasswordHistoryID
        End If

        Return GetUserPasswordHistory(sql)

    End Function

    Protected Overridable Function GetUserPasswordHistory(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mPasswordHistoryID = Catchnull(.Item("PasswordHistoryID"), 0)
            mUserID = Catchnull(.Item("UserID"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mUsername = Catchnull(.Item("Username"), "")
            mPassword = Catchnull(.Item("Password"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@PasswordHistoryID", DbType.Int32, mPasswordHistoryID)
        db.AddInParameter(cmd, "@UserID", DbType.Int32, mUserID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@Username", DbType.String, mUsername)
        db.AddInParameter(cmd, "@Password", DbType.String, mPassword)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_UserPasswordHistory")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mPasswordHistoryID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblUserPasswordHistory SET Deleted = 1 WHERE PasswordHistoryID = " & mPasswordHistoryID) 
        Return Delete("DELETE FROM tblUserPasswordHistory WHERE PasswordHistoryID = " & mPasswordHistoryID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            Return False

        End Try

    End Function

#End Region

#End Region

End Class
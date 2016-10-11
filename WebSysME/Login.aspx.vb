Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports SecurityPolicy
Imports BusinessLogic

Public Class Login1
    Inherits System.Web.UI.Page

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Shared ReadOnly SecurityLog As log4net.ILog = log4net.LogManager.GetLogger("SecurityLogger")

    'remove this variable when done testing
    Private mdbgCompanySetupMissingFiles As String = ""
    Private Counter As Integer = 0
    'Public Property Counter() As Integer
    '    Get
    '        Return IIf(ViewState("counter") Is Nothing, 0, CInt(ViewState("counter")))
    '    End Get
    '    Set(ByVal value As Integer)
    '        ViewState("counter") = value
    '    End Set
    'End Property

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblMessages.Text = Message.ToString
        pnlMessages.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblMessages.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblMessages.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        pnlMessages.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            With cboOrganisation

                .DataSource = GetOrganisations()
                .DataValueField = "ConnectionName"
                .DataTextField = "ConnectionName"
                .DataBind()

            End With

        End If

    End Sub

    Private Function GetOrganisations() As DataSet

        Dim ds As New DataSet

        ds.Tables.Add(New DataTable)

        ds.Tables(0).Columns.Add(New DataColumn("ConnectionName"))

        For Each cnn As ConnectionStringSettings In ConfigurationManager.ConnectionStrings  'need to loop through all the databases

            Try

                If cnn.Name.StartsWith("WebSys") Then

                    Dim dr As DataRow = ds.Tables(0).NewRow

                    dr("ConnectionName") = cnn.Name.Substring(6)

                    ds.Tables(0).Rows.Add(dr)

                End If

            Catch ex As Exception

            End Try

        Next

        Return ds

    End Function

    Protected Sub cmdLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLogin.Click

        CookiesWrapper.thisConnectionName = "WebSys" & cboOrganisation.SelectedValue

        Try

            Dim myUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim myAdmin As New AdminSettings(CookiesWrapper.thisConnectionName, 0)
            Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With myUser

                .Username = txtUserName.Text
                .Password = .PasswordHash(txtUserName.Text, txtPassword.Text)

                If .ValidateUser() Then

                    log.Error("User validation complete")

                    If .IsLockedOut Then

                        SecurityLog.ErrorFormat("'{0}' failed to login. Account locked out", txtUserName.Text.ToUpper())
                        ShowMessage("Your account is locked! Please contact the systems administrator", MessageTypeEnum.Error)
                        Exit Sub

                    Else

                        If .PasswordExpires Then    'Using User Defined Settings

                            If .VerifyPasswordValidity() Then

                                'Expired
                                pnlLogin.Visible = False
                                pnlResetPassword.Visible = True
                                SecurityLog.ErrorFormat("'{0}' Login failed. Password has expired. Please reset your password", txtUserName.Text.ToUpper())
                                ShowMessage("Login failed. Password has expired. Please reset your password", MessageTypeEnum.Warning)
                                Exit Sub

                            Else        'Proceed

                                AuthenticateLogon(myUser)
                                log.Error("Authentication of user complete")

                            End If

                        Else 'Use Admin Settings for All Users

                            If objSecurityPolicy.RetrieveActiveSecurityPolicy() Then

                                If objSecurityPolicy.PasswordExpires Then

                                    .PasswordExpirationDays = objSecurityPolicy.PasswordValidityPeriod

                                    If .VerifyPasswordValidity() Then

                                        'Expired
                                        pnlLogin.Visible = False
                                        pnlResetPassword.Visible = True
                                        ShowMessage("Login failed. Password has expired. Please reset your password", MessageTypeEnum.Warning)
                                        Exit Sub

                                    Else        'Proceed

                                        AuthenticateLogon(myUser)

                                    End If

                                Else

                                    'Just Authenticate
                                    AuthenticateLogon(myUser)
                                    log.Error("Authentication of user complete.Password does not expire.")

                                End If

                            Else

                                SecurityLog.ErrorFormat("'{0}' login failed.Error: Bad User Default Settings. Contact your Administrator..", txtUserName.Text.ToUpper())
                                ShowMessage("Error: Bad User Default Settings. Contact your Administrator..", MessageTypeEnum.Error)

                            End If

                        End If

                    End If

                    CookiesWrapper.thisUserID = .UserID
                    CookiesWrapper.thisUserName = txtUserName.Text
                    CookiesWrapper.thisUserFullName = .FullName

                    .SaveUserLog(myUser.UserID)

                    CookiesWrapper.thisLogID = .LogID
                    SecurityLog.InfoFormat("'{0}' logged in successfully", txtUserName.Text.ToUpper())
                    FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, True)

                Else

                    Counter = .FailedPasswordAttemptCount

                    ' UpdateAttemptsCounter(.UserID)

                    If myAdmin.ReturnAdminSettings Then

                        If Counter >= myAdmin.Attempts Then

                            .UpdateUserLockStatus(.UserID, True)

                            SecurityLog.ErrorFormat("User '{0}' has been locked out after {1} login attempts", txtUserName.Text.ToUpper, myAdmin.Attempts)
                            ShowMessage("You have been locked out. Please contact the systems administrator!", MessageTypeEnum.Error)

                        Else

                            SecurityLog.ErrorFormat("Incorrect username or password for user '{0}' after {1} login attempts from address {2}", txtUserName.Text.ToUpper, Counter, WebHelper.GetUserIPAddress)
                            ShowMessage("Incorrect username or password. You have " & myAdmin.Attempts - Counter & " attempts left.", MessageTypeEnum.Error)

                        End If

                    Else

                        If Counter >= SecurityPolicy.UserManager.MAXPASSWORDATTEMPTS Then
                            SecurityLog.ErrorFormat("'{0}' login failed.Account has been locked out.", txtUserName.Text.ToUpper())
                            ShowMessage("You have been locked out. Please contact the systems administrator!", MessageTypeEnum.Error)
                            .UpdateUserLockStatus(.UserID, True)
                        Else
                            SecurityLog.ErrorFormat("'{0}' login failed.'{1}' attempt(s) left ", txtUserName.Text.ToUpper(), SecurityPolicy.UserManager.MAXPASSWORDATTEMPTS - Counter)
                            ShowMessage("Incorrect username or password. You have " & SecurityPolicy.UserManager.MAXPASSWORDATTEMPTS - Counter & " attempt(s) left.", MessageTypeEnum.Warning)
                        End If

                    End If

                End If

            End With

        Catch ex As Exception

            System.Diagnostics.Trace.WriteLine(ex)
            ShowMessage(ex.Message, MessageTypeEnum.Error)

        End Try

    End Sub

    Protected Sub cmdSaveResetPassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveResetPassword.Click

        Try

            Dim myUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim myPasswordHistory As New SecurityPolicy.UserPasswordHistory(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With myUser

                .Username = txtUserName.Text
                .Password = .PasswordHash(txtUserName.Text, txtOldPassword.Text)

                If .ValidateUser() Then

                    If txtOldPassword.Text <> txtNewPassword.Text Then

                        If isValidPassword() = False Then
                            DisplayResetMessage("The new password supplied does not meet the password requirements", MessageTypeEnum.Error)
                            Exit Sub
                        End If

                        If CheckPasswordHistory(.PasswordHash(txtUserName.Text, txtNewPassword.Text), CookiesWrapper.thisUserID) = False Then
                            DisplayResetMessage("Password has already been used before! Try again...", MessageTypeEnum.Error)
                            Exit Sub
                        End If

                        If CheckDictionaryWords(txtNewPassword.Text) Then
                            DisplayResetMessage("Invalid! Password should not contain common dictionary words...", MessageTypeEnum.Error)
                            Exit Sub
                        End If

                        .Username = txtUserName.Text
                        .Password = .PasswordHash(txtUserName.Text, txtNewPassword.Text)

                        .ResetPassword()        'Save New Password

                        With myPasswordHistory

                            .UserID = myUser.UserID
                            .Username = myUser.Username
                            .Password = myUser.Password
                            .Save()     'Save Password in History table for History reference

                        End With

                        If .ValidateUser() Then

                            AuthenticateLogon(myUser)

                        Else
                            DisplayResetMessage("Incorrect User Profile. Contact your Administrator..", MessageTypeEnum.Error)
                        End If

                    Else
                        DisplayResetMessage("New password should be different from old password..", MessageTypeEnum.Error)
                    End If

                Else
                    DisplayResetMessage("Incorrect Password. Enter the correct old password..", MessageTypeEnum.Error)
                End If

            End With

        Catch ex As Exception

            log.Error(ex)
            ShowMessage(ex.Message, MessageTypeEnum.Error)

        End Try

    End Sub

    Private Sub AuthenticateLogon(ByRef myUser As SecurityPolicy.UserManager)

        Try
            CookiesWrapper.StaffID = 0
            CookiesWrapper.OrganizationID = 0

            ' Redirect to requested URL, or homepage if no previous page requested
            Dim returnUrl As String = Request.QueryString("ReturnUrl")

            With myUser

                Dim objAccountsLink As New UserAccountProfileLink(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                Dim objStaffMembers As New BusinessLogic.StaffMember(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                Dim ds As DataSet = objAccountsLink.GetUserAccountProfileLink(.UserID)

                If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    With objStaffMembers

                        If .Retrieve(ds.Tables(0).Rows(0)("StaffID")) Then

                            CookiesWrapper.StaffID = .StaffID
                            CookiesWrapper.OrganizationID = .OrganizationID

                        End If

                    End With

                End If

                CookiesWrapper.thisUserID = .UserID
                CookiesWrapper.thisUserName = txtUserName.Text
                CookiesWrapper.thisUserFullName = .FullName

                If .SaveUserLog(myUser.UserID) = True Then
                    .UpdateUserLockStatus(myUser.UserID, False)
                    ResetAttemptsCounter(.UserID)
                End If

                CookiesWrapper.thisLogID = .LogID

                'Dim dsRoles As DataSet
                'dsRoles = .GetUserRoles(.UserID)
                FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, chkRememberLogin.Checked)

            End With

        Catch ex As Exception

            log.Error(ex)
            ShowMessage(ex.Message, MessageTypeEnum.Error)

        End Try

    End Sub

    Private Sub DisplayResetMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

        lblResetError.Visible = True
        lblResetError.Text = Message
        pnlResetError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

    Public Function UpdateAttemptsCounter(ByVal UserID As Long) As DataSet
        Try
            Dim sql As String

            sql = "UPDATE tblUsers SET FailedPasswordAttemptCount = " & Counter & " WHERE UserID= " & UserID

            Dim DB As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Return DB.ExecuteDataSet(CommandType.Text, sql)

        Catch ex As Exception

            Return Nothing
        End Try

    End Function

    Public Function ResetAttemptsCounter(ByVal UserID As Long) As DataSet
        Try
            Dim sql As String

            sql = "UPDATE tblUsers SET FailedPasswordAttemptCount = 0 WHERE UserID= " & UserID

            Dim DB As Database = New DatabaseProviderFactory().Create(CookiesWrapper.thisConnectionName)
            Return DB.ExecuteDataSet(CommandType.Text, sql)

        Catch ex As Exception

            Return Nothing
        End Try

    End Function

    Private Function isValidPassword() As Boolean

        Dim result As Boolean
        Dim sbPasswordRegx As New StringBuilder(String.Empty)

        Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objSecurityPolicy

            If .RetrieveActiveSecurityPolicy Then

                'min and max
                sbPasswordRegx.Append("(?=^.{" & .MinPasswordLength & "," & .MaxPasswordLength & "}$)")

                'numbers length
                sbPasswordRegx.Append("(?=(?:.*?\d){" & .NumericLength & "})")

                'a-z characters
                sbPasswordRegx.Append("(?=.*[a-z])")

                'A-Z length
                sbPasswordRegx.Append("(?=(?:.*?[A-Z]){" & .UpperCaseLength & "})")

                'special characters length
                'sbPasswordRegx.Append("(?=(?:.*?[" & Catchnull(.SpecialCharacters, "") & "]) {" & .SpecialCharacterLength & "})")

                '(?!.*\s) - no spaces
                '[0-9a-zA-Z!@#$%*()_+^&] -- valid characters
                'sbPasswordRegx.Append("(?!.*\s)[0-9a-zA-Z" & Catchnull(.SpecialCharacters, "") & "]*$")

            End If

        End With

        If Regex.IsMatch(txtNewPassword.Text, sbPasswordRegx.ToString) Then

            result = True

        End If

        isValidPassword = result

    End Function

    Private Function CheckPasswordHistory(ByVal Password As String, ByVal UserID As Long) As Boolean

        Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objSecurityPolicy

            If .RetrieveActiveSecurityPolicy Then

                If .PasswordHistory > 0 Then

                    If .CheckPasswordHistory(Password, UserID, .PasswordHistory) Then

                        Return False

                    End If

                End If

            End If

        End With

        Return True

    End Function

    Private Function CheckDictionaryWords(ByVal Password As String) As Boolean

        Dim hash As HashSet(Of String) = New HashSet(Of String)(IO.File.ReadAllLines(Server.MapPath("Dictionary/dictionary.txt")))
        Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objSecurityPolicy

            If .RetrieveActiveSecurityPolicy Then

                If .UseDictionary Then

                    For Each word As String In hash

                        If Password.Contains(word) Then
                            Return True
                        End If

                    Next

                End If

            End If

        End With

        Return False

    End Function

End Class
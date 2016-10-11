Imports Microsoft.Practices.EnterpriseLibrary
Imports System.Security.Cryptography
Imports System.Web.Security
Imports Universal.CommonFunctions
Imports SecurityPolicy
Imports SecurityPolicy.SecurityEnum

Partial Public Class CreateNewUserControl
    Inherits System.Web.UI.UserControl
    Private EditMode As Boolean = False
    Private Shared ReadOnly SecurityLog As log4net.ILog = log4net.LogManager.GetLogger("SecurityLogger")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim objLookup As New BusinessLogic.CommonFunctions()

            With chkUserGroup

                .DataValueField = "UserGroupID"

                .DataTextField = "Description"

                .DataSource = objLookup.Lookup("luUserGroups", "UserGroupID", "Description", "Description")

                .DataBind()

            End With

            lblStatus.Text = ""
            Dim myAdmin As New AdminSettings(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With myAdmin

                If .Retrieve(CookiesWrapper.thisUserID) Then
                    txtMinLength.Text = .MinPasswordLength
                    txtPasswordTemplateID.Text = .PasswordTemplateID
                Else
                    txtMinLength.Text = 8
                    txtPasswordTemplateID.Text = 1
                End If


                Select Case .PasswordTemplateID
                    Case PasswordTemplate.AlphaNumeric
                        lblPasswordPolicyMsg.Text = "Password should have alphanumeric characters"
                    Case PasswordTemplate.AlphaNumericWithSpecialChar
                        lblPasswordPolicyMsg.Text = "Password should be Aphanumeric and also contains special characters like ($#@!%^&*) "
                    Case PasswordTemplate.AlphaNumericWithSpecialChar
                        lblPasswordPolicyMsg.Text = "Password should be Aphanumeric, also starting and ending with a special characters like ($#@!%^&*)"
                    Case Else

                End Select

            End With

            If Request.QueryString("op") = "new" Then

                Clear()

            End If

            If Request.QueryString("op") = "eu" Then

                'we need to load the user details
                EditMode = True
                Dim objUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                Dim objUserID As Long = Session("UserID")

                If objUserID > 0 Then

                    Dim ds As DataSet = objUser.FindUser(objUserID)

                    If Not IsNothing(ds) Then

                        'disable RequiredFieldValidator1 in edit mode to allow the administrator to be able to change user group info or other relevant info.
                        RequiredFieldValidator1.Enabled = Not (EditMode)

                        With ds.Tables(0).Rows(0)

                            txtUserID.Text = .Item("UserID")
                            txtUsername.Text = Catchnull(.Item("Username"), "")
                            txtPassword.Text = Catchnull(.Item("Password"), "")
                            txtFirstname.Text = Catchnull(.Item("UserFirstname"), "")
                            txtSurname.Text = Catchnull(.Item("UserSurname"), "")
                            txtMobileNo.Text = Catchnull(.Item("MobileNo"), "")
                            txtEmailAddress.Text = IIf(.Item("EmailAddress") Is DBNull.Value, "", .Item("EmailAddress"))
                            txtPasswordValidityPeriod.Text = Catchnull(.Item("PasswordExpirationDays"), 0)
                            chkPasswordExpires.Checked = Catchnull(.Item("PasswordExpires"), False)
                            chkIsLockedOut.Checked = Catchnull(.Item("IsLockedOut"), False)

                            txtUsername.Enabled = False

                            ViewState("Current.Active") = Catchnull(.Item("Deleted"), 0)

                        End With

                    End If

                    LoadLinkAccountsPanel(objUserID)

                End If

            End If

        End If

    End Sub

    Private Sub LoadLinkAccountsPanel(ByVal objUserID As Long)

        Dim objLookup As New BusinessLogic.CommonFunctions

        With lstStaffMembers

            .DataSource = objLookup.Lookup2("tblStaffMembers", "StaffID", "ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'') As Name", "ISNULL(FirstName, '') + ' ' + ISNULL(Surname,'')").Tables(0)
            .DataTextField = "Name"
            .DataValueField = "StaffID"

            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        Dim objAccountLink As New UserAccountProfileLink(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim ds2 As DataSet

        pnlLinkAccounts.Visible = True

        With objAccountLink

            If .CheckUserAccountProfileLink(objUserID) Then

                cmdLink.Visible = False
                cmdUnLink.Visible = True
                lstStaffMembers.Enabled = False
                lblLinkStatus.Text = "Account is already Linked"
                lblLinkStatus.ForeColor = Drawing.Color.OliveDrab

            Else

                cmdLink.Visible = True
                cmdUnLink.Visible = False
                lstStaffMembers.Enabled = True
                lblLinkStatus.Text = "This account has not been linked to Staff Profile. Please link now..."
                lblLinkStatus.ForeColor = Drawing.Color.Red

            End If

            ds2 = .GetUserAccountProfileLink(objUserID)

            If Not IsNothing(ds2) AndAlso ds2.Tables.Count > 0 AndAlso ds2.Tables(0).Rows.Count Then

                lstStaffMembers.SelectedValue = ds2.Tables(0).Rows(0)("StaffID")

            End If

        End With

    End Sub

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

        If Regex.IsMatch(txtPassword.Text, sbPasswordRegx.ToString) Then

            result = True

        End If

        isValidPassword = result

    End Function

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim myUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        lblStatus.Text = ""

        ' If Page.IsValid Then
        If (IIf(IsNumeric(txtUserID.Text), txtUserID.Text, 0) <> 0 AndAlso Trim(txtPassword.Text) <> "" AndAlso isValidPassword() = False) OrElse (IIf(IsNumeric(txtUserID.Text), txtUserID.Text, 0) = 0 AndAlso Trim(txtPassword.Text) <> "" AndAlso isValidPassword() = False) Then
            lblStatus.Text = "The password supplied does not meet the password requirements"
            Exit Sub
        End If

        With myUser

            .UserID = IIf(IsNumeric(txtUserID.Text), txtUserID.Text, 0)
            .Username = txtUsername.Text

            If Request.QueryString("op") = "eu" Then

                If Not Trim(txtPassword.Text) = "" Then
                    'Edit the password only when a new value has been supplied

                    .Password = .PasswordHash(txtUsername.Text, txtPassword.Text)

                End If

            Else

                ' The password value that is passed to the database should be hashed first
                .Password = .PasswordHash(txtUsername.Text, txtPassword.Text)

            End If
            .EmailAddress = txtEmailAddress.Text
            .UserFirstName = txtFirstname.Text
            .UserSurname = txtSurname.Text
            .MobileNo = txtMobileNo.Text

            If chkPasswordExpires.Checked Then

                If IsNumeric(txtPasswordValidityPeriod.Text) Then

                    If txtPasswordValidityPeriod.Text > 0 Then
                        .PasswordExpirationDays = txtPasswordValidityPeriod.Text
                        .LastPasswordChangeDate = Date.Today

                    Else
                        lblStatus.CssClass = "Warning"
                        lblStatus.Text = "Please enter number of days greater than 0....."
                        Exit Sub

                    End If

                Else

                    lblStatus.CssClass = "Warning"
                    lblStatus.Text = "Please specify a number...."
                    Exit Sub

                End If
            End If

            .PasswordExpires = chkPasswordExpires.Checked
            .IsLockedOut = chkIsLockedOut.Checked
            Try

                If .CreateUser() Then

                    txtUserID.Text = .UserID
                    'cmdSave.Enabled = False
                    'cmdAddUser.Visible = True

                    .DeleteUserUserGroups()

                    For i As Integer = 0 To chkUserGroup.Items.Count - 1

                        If chkUserGroup.Items(i).Selected Then

                            myUser.SaveUserUserGroup(chkUserGroup.Items(i).Value)

                        End If

                    Next
                    SecurityLog.Info(txtUsername.Text.ToUpper() & "New User created:")

                    lblStatus.Text = "Details Saved.."
                    lblStatus.CssClass = "msgInformation"
                    LoadLinkAccountsPanel(.UserID)

                End If

            Catch ex As Exception

                lblStatus.CssClass = "msgError"
                lblStatus.Text = "Username already exists. User has not been created!"

            End Try

        End With

        'End If

    End Sub

    Protected Sub cmdAddUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddUser.Click

        Clear()
        cmdSave.Enabled = True
        cmdAddUser.Visible = False

    End Sub

    Public Shadows Sub Clear()

        txtFirstname.Text = ""
        txtSurname.Text = ""
        txtMobileNo.Text = ""
        txtUsername.Text = ""
        txtPassword.Text = ""
        txtConfirmPassword.Text = ""
        txtEmailAddress.Text = ""
        txtUserID.Text = ""
        lblStatus.Text = ""
        txtPasswordValidityPeriod.Text = ""
        chkPasswordExpires.Checked = False

        chkUserGroup.ClearSelection()

    End Sub

    'Private Sub cvMinLength_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvMinLength.ServerValidate
    '    Dim strDesc As String = Me.txtPassword.Text
    '    Dim myAdmin As New AdminSettings(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

    '    With myAdmin

    '        If .Retrieve(CookiesWrapper.thisUserID) Then
    '            txtMinLength.Text = .MinPasswordLength
    '            If Len(strDesc) > txtMinLength.Text Then
    '                args.IsValid = False
    '            Else
    '                args.IsValid = True
    '            End If
    '        End If

    '    End With

    'End Sub

    Protected Sub chkUserGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkUserGroup.SelectedIndexChanged

    End Sub

    Protected Sub chkIsLockedOut_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsLockedOut.CheckedChanged

    End Sub

    Private Sub cmdLink_Click(sender As Object, e As EventArgs) Handles cmdLink.Click

        If lstStaffMembers.SelectedValue > 0 Then

            Dim objAccountsLink As New UserAccountProfileLink(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objAccountsLink

                .UserID = Session("UserID")
                .StaffID = lstStaffMembers.SelectedValue

                If .Save Then

                    lblStatus.Text = "Accounts linked successfully..."
                    lblStatus.CssClass = "msgInformation"

                Else

                    lblStatus.Text = "Accounts failed to link..."
                    lblStatus.CssClass = "msgError"

                End If

                LoadLinkAccountsPanel(.UserID)

            End With

        End If

    End Sub

    Private Sub cmdUnLink_Click(sender As Object, e As EventArgs) Handles cmdUnLink.Click

        Dim objAccountsLink As New UserAccountProfileLink(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim UserID As Long = IIf(IsNumeric(Session("UserID")), Session("UserID"), 0)

        If objAccountsLink.DeleteByUserID(UserID) Then

            lblStatus.Text = "Accounts unlinked successfully"
            lblStatus.CssClass = "msgInformation"

            LoadLinkAccountsPanel(UserID)

        End If


    End Sub

End Class
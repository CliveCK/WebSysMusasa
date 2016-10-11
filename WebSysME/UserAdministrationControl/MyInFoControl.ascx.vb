Imports Universal.CommonFunctions
Imports SecurityPolicy
Imports SecurityPolicy.SecurityEnum

Partial Public Class MyInFoControl
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly SecurityLog As log4net.ILog = log4net.LogManager.GetLogger("SecurityLogger")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim objUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If Not Page.IsPostBack Then

            Dim ds As DataSet = objUser.FindUser(CookiesWrapper.thisUserID)

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


            With ds.Tables(0).Rows(0)


                txtUsername.Text = Catchnull(.Item("Username"), "")
                txtPassword.Text = Catchnull(.Item("Password"), "")
                txtConfirmPassword.Text = Catchnull(.Item("Password"), "")
                txtFirstname.Text = Catchnull(.Item("UserFirstname"), "")
                txtSurname.Text = Catchnull(.Item("UserSurname"), "")
                txtEmail.Text = Catchnull(.Item("EmailAddress"), "")
                txtMobileNo.Text = Catchnull(.Item("MobileNo"), "")


                'cboCompany.SelectedValue = Catchnull(.Item("CompanyID"), 0)

                'cboPermissions.SelectedValue = Catchnull(.Item("PermissionID"), 0)

            End With

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click


        Dim objUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Try

            ' If Page.IsValid Then


            objUser.UpdateDetails(CookiesWrapper.thisUserID, txtEmail.Text, txtSurname.Text, txtFirstname.Text, txtMobileNo.Text)
            SecurityLog.Info(txtUsername.Text.ToUpper() & " updated.")
            lblError.CssClass = "msgInformation"
            lblError.Text = "Details saved.."

        Catch ex As Exception

            lblError.CssClass = "msgError"
            lblError.Text = ex.Message

        End Try

    End Sub

    Protected Sub cmdChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangePassword.Click


        Dim objUser As New SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim myPasswordHistory As New UserPasswordHistory(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Try

            If isValidPassword() = False Then
                lblError.Text = "The password supplied does not meet the password requirements - " & Environment.NewLine & ShowPasswordReq()
                Exit Sub
            End If

            If CheckPasswordHistory(objUser.PasswordHash(txtUsername.Text, txtPassword.Text), CookiesWrapper.thisUserID) = False Then
                lblError.Text = "Password has already been used before! Try again..."
                Exit Sub
            End If

            If CheckDictionaryWords(txtPassword.Text) Then
                lblError.Text = "Invalid! Password should not contain common dictionary words..."
                Exit Sub
            End If

            If objUser.ValidateUser(txtUsername.Text, txtOldPassword.Text) Then

                objUser.ChangePassword(CookiesWrapper.thisUserID, objUser.PasswordHash(txtUsername.Text, txtPassword.Text))

                With myPasswordHistory

                    .UserID = objUser.UserID
                    .Username = objUser.Username
                    .Password = objUser.Password
                    .Save()     'Save Password in History table for History reference

                End With

                lblError.CssClass = "msgInformation"
                lblError.Text = "Password changed.."
            Else

                lblError.CssClass = "msgError"
                lblError.Text = "Invalid Old Password!"

            End If

        Catch ex As Exception

            lblError.CssClass = "msgError"
            lblError.Text = ex.Message

        End Try

    End Sub

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

    Private Function ShowPasswordReq() As String

        Dim Str As String = ""
        Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objSecurityPolicy

            If .RetrieveActiveSecurityPolicy Then

                Str = "Password should: " & Environment.NewLine
                Str &= "Be between " & .MinPasswordLength & " and " & .MaxPasswordLength
                Str &= IIf(.UpperCaseLength > 0, " , at least " & .UpperCaseLength & " UpperCase letter(s)", "")
                Str &= IIf(.NumericLength > 0, " , at least " & .NumericLength & " Numeric character(s)", "")
                Str &= IIf(.SpecialCharacterLength > 0, " , at least " & .SpecialCharacterLength & " special character(s) e.g. '" & .SpecialCharacters & "'", "")

            End If

        End With

        Return Str

    End Function
End Class
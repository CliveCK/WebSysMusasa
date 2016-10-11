Imports Universal.CommonFunctions
Imports System.Xml

Public Class PasswordPolicyManagementConsole
    Inherits System.Web.UI.UserControl

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadSecurityPolicies()

        End If

    End Sub

    Protected Sub LoadSecurityPolicies()

        Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim dsPolicies As DataSet = objSecurityPolicy.RetrieveAll()

        With lstPasswordPolicies

            .DataTextField = "Name"
            .DataValueField = "PasswordPolicyID"
            .DataSource = dsPolicies
            .DataBind()
            .Items.Add(New ListItem("", 0))
            .SelectedIndex = .Items.Count - 1

        End With

    End Sub

    Private Sub lstPasswordPolicies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstPasswordPolicies.SelectedIndexChanged

        If lstPasswordPolicies.SelectedIndex > -1 AndAlso IsNumeric(lstPasswordPolicies.SelectedValue) Then

            LoadPasswordPolicy(lstPasswordPolicies.SelectedValue)

        End If

    End Sub

    Private Sub LoadPasswordPolicy(ByVal PasswordPolicyID As Long)

        Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objSecurityPolicy

            If .Retrieve(PasswordPolicyID) Then

                txtPasswordPolicyID.Text = .PasswordPolicyID
                txtName.Text = .Name
                txtDescription.Text = .Description
                txtMaxPasswordLength.Text = .MaxPasswordLength
                txtMinPasswordLength.Text = .MinPasswordLength
                txtNumericLength.Text = .NumericLength
                txtPasswordHistory.Text = .PasswordHistory
                txtPasswordValidityPeriod.Text = .PasswordValidityPeriod
                txtSpecialCharacterLength.Text = .SpecialCharacterLength
                txtSpecialCharacters.Text = .SpecialCharacters
                txtUpperCaseLength.Text = .UpperCaseLength

                chkIsActive.Checked = .IsActive
                chkUseDictionary.Checked = .UseDictionary
                chkPasswordExpires.Checked = .PasswordExpires

                txtName.Enabled = IIf(.Name = "System Default", False, True)
                txtDescription.Enabled = IIf(.Name = "System Default", False, True)

                ShowMessage("Security Policy Loaded Successfully...", MessageTypeEnum.Information)

            Else

                ShowMessage("Failed to load selected Security Policy...", MessageTypeEnum.Information)

            End If

        End With

    End Sub

    Private Sub cmdAddNewPolicy_Click(sender As Object, e As EventArgs) Handles cmdAddNewPolicy.Click

        Response.Redirect("ManagePasswordPolicy.aspx")

    End Sub


    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click

        If IsNumeric(lstPasswordPolicies.SelectedValue) AndAlso lstPasswordPolicies.SelectedValue > 0 Then

            Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objSecurityPolicy

                Try

                    If .Retrieve(lstPasswordPolicies.SelectedValue) Then

                        If .Name = "System Default" Then

                            ShowMessage("Cannot delete 'System Default' Security Policy!!", MessageTypeEnum.Error)

                        Else

                            If .IsActive Then

                                ShowMessage("Cannot delete an ACTIVE Security Policy!!", MessageTypeEnum.Error)

                            Else

                                .PasswordPolicyID = lstPasswordPolicies.SelectedValue

                                If .Delete() Then

                                    ShowMessage("Security Policy deleted successfully...", MessageTypeEnum.Information)
                                    LoadSecurityPolicies()

                                Else

                                    ShowMessage("Failed to delete selected Policy!", MessageTypeEnum.Information)

                                End If

                            End If

                        End If

                    Else

                        ShowMessage("Failed to retrieve selected Policy", MessageTypeEnum.Error)

                    End If

                Catch ex As Exception

                    log.Error(ex.Message)
                    ShowMessage("An error occured. If it persists please contact Administrator...", MessageTypeEnum.Error)

                End Try

            End With

        Else

            ShowMessage("Select a Security Policy to delete...", MessageTypeEnum.Warning)

        End If

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Not (ValidateFields(txtMinPasswordLength) AndAlso ValidateFields(txtMaxPasswordLength) AndAlso ValidateFields(txtUpperCaseLength) AndAlso ValidateFields(txtNumericLength) AndAlso _
            ValidateFields(txtSpecialCharacterLength) AndAlso ValidateFields(txtPasswordHistory) AndAlso ValidateFields(txtPasswordValidityPeriod)) Then

            Exit Sub

        End If

        If txtName.Enabled = True Then
            If txtName.Text = "System Default" Then 'Just in case an attempt is made to add Same name
                ShowMessage("There is already a policy with the same name.", MessageTypeEnum.Error)
                Exit Sub
            End If
        End If

        Dim objSecurityPolicy As New SecurityPolicy.SecurityPolicy(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objSecurityPolicy

            .PasswordPolicyID = IIf(txtPasswordPolicyID.Text <> "", txtPasswordPolicyID.Text, 0)

            .Name = txtName.Text
            .Description = txtDescription.Text
            .PasswordHistory = txtPasswordHistory.Text
            .MinPasswordLength = txtMinPasswordLength.Text
            .MaxPasswordLength = txtMaxPasswordLength.Text
            .PasswordValidityPeriod = txtPasswordValidityPeriod.Text
            .NumericLength = txtNumericLength.Text
            .SpecialCharacterLength = txtSpecialCharacterLength.Text
            .SpecialCharacters = txtSpecialCharacters.Text
            .IsActive = chkIsActive.Checked
            .UseDictionary = chkUseDictionary.Checked
            .UpperCaseLength = txtUpperCaseLength.Text
            .PasswordExpires = chkPasswordExpires.Checked

            If .Save Then

                ShowMessage("Security Policy saved successfully...", MessageTypeEnum.Information)
                If .IsActive Then

                    WriteToPasswordPolicyXML(objSecurityPolicy)
                    .DeactivateActiveProfiles(.PasswordPolicyID)

                End If
                LoadSecurityPolicies()
                If Not IsNothing(lstPasswordPolicies.Items.FindByValue(.PasswordPolicyID)) Then lstPasswordPolicies.SelectedValue = .PasswordPolicyID

            Else

                ShowMessage("Failed to save Security Policy", MessageTypeEnum.Error)

            End If

        End With

    End Sub

    Private Sub WriteToPasswordPolicyXML(ByVal objSecurityPolicy As SecurityPolicy.SecurityPolicy)

        Dim xmlW As New XmlTextWriter(Server.MapPath("Scripts/PasswordPolicy.xml"), System.Text.Encoding.UTF8)

        Try

            Using xmlW

                With xmlW

                    .Formatting = Formatting.Indented
                    .Indentation = 2
                    .WriteStartDocument()

                    .WriteStartElement("PasswordPolicy")
                    .WriteStartElement("Password")

                    .WriteStartElement("duration")
                    .WriteString(Catchnull(objSecurityPolicy.PasswordValidityPeriod, 0))
                    .WriteEndElement()
                    .WriteStartElement("minLength")
                    .WriteString(Catchnull(objSecurityPolicy.MinPasswordLength, 0))
                    .WriteEndElement()
                    .WriteStartElement("maxLength")
                    .WriteString(Catchnull(objSecurityPolicy.MaxPasswordLength, 0))
                    .WriteEndElement()
                    .WriteStartElement("numsLength")
                    .WriteString(Catchnull(objSecurityPolicy.NumericLength, 0))
                    .WriteEndElement()
                    .WriteStartElement("upperLength")
                    .WriteString(Catchnull(objSecurityPolicy.UpperCaseLength, 0))
                    .WriteEndElement()
                    .WriteStartElement("specialLength")
                    .WriteString(Catchnull(objSecurityPolicy.SpecialCharacterLength, 0))
                    .WriteEndElement()
                    .WriteStartElement("barWidth")
                    .WriteString("150")
                    .WriteEndElement()
                    .WriteStartElement("barColor")
                    .WriteString("Green")
                    .WriteEndElement()
                    .WriteStartElement("specialChars")
                    .WriteString(Catchnull(objSecurityPolicy.SpecialCharacters, "", True))
                    .WriteEndElement()
                    .WriteStartElement("useMultipleColors")
                    .WriteString("1")
                    .WriteEndElement()

                    .WriteEndElement()
                    .WriteEndElement()

                    .WriteEndDocument()

                    .Flush() : .Close()

                End With

            End Using

        Catch ex As Exception

            log.Error(ex.Message)
            ShowMessage("An error occured saving PasswordPolicy config file!", MessageTypeEnum.Error)

        End Try

    End Sub

    Private Function ValidateFields(ByVal txtField As TextBox) As Boolean

        With txtField

            If Not IsNumeric(.Text) Then
                ShowMessage("A Numeric value is needed for this field...", MessageTypeEnum.Error)
                .Focus()
                .BorderColor = Drawing.Color.DarkRed
                .BorderWidth = 2

                Return False

            Else

                .BorderColor = txtName.BorderColor
                .BorderWidth = txtName.BorderWidth
                Return True

            End If

        End With

    End Function

End Class
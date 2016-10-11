Imports BusinessLogic

Partial Class CaregiverDetailsControl
    Inherits System.Web.UI.UserControl

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageTypeEnum)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageTypeEnum As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

            lblError.Text = Message.Message
            If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
            If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageTypeEnum)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageTypeEnum)

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            LoadCombo()

            If CookiesWrapper.PatientID > 0 Then

                LoadCaregiver(CookiesWrapper.PatientID)

            End If

        End If

    End Sub

    Private Sub LoadCombo()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboProfession

            .DataSource = objLookup.Lookup("luProfession", "ProfessionID", "Description")
            .DataValueField = "ProfessionID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboRelationship

            .DataSource = objLookup.Lookup("luRelationships", "RelationshipID", "Description")
            .DataValueField = "RelationshipID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If CookiesWrapper.PatientID > 0 Then

            Save()

        Else

            ShowMessage("Please save Patient details first before saving this tab", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadCaregiver(ByVal PatientID As Long) As Boolean

        Try

            Dim objCaregiver As New Caregiver(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCaregiver

                If .RetrieveByPatient(PatientID) Then

                    txtCaregiverID.Text = .CaregiverID
                    txtmPatientID.Text = .PatientID
                    If Not IsNothing(cboProfession.Items.FindByValue(.Profession)) Then cboProfession.SelectedValue = .Profession
                    If Not IsNothing(cboRelationship.Items.FindByValue(.RelationshipToChild)) Then cboRelationship.SelectedValue = .RelationshipToChild
                    radDateofBirth.SelectedDate = .DateOfBirth
                    txtFirstname.Text = .Firstname
                    txtSurname.Text = .Surname
                    txtContactNo.Text = .ContactNo
                    txtNameOfEmployer.Text = .NameOfEmployer
                    txtSocialSupportSystems.Text = .SocialSupportSystems
                    txtAlternateSourcesOfIncome.Text = .AlternateSourcesOfIncome

                    ShowMessage("Caregiver loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Caregiver", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function Save() As Boolean

        Try

            Dim objCaregiver As New Caregiver(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCaregiver

                .CaregiverID = IIf(IsNumeric(txtCaregiverID.Text), txtCaregiverID.Text, 0)
                .PatientID = CookiesWrapper.PatientID
                If cboProfession.SelectedIndex > -1 Then .Profession = cboProfession.SelectedValue
                If cboRelationship.SelectedIndex > -1 Then .RelationshipToChild = cboRelationship.SelectedValue
                .DateOfBirth = radDateofBirth.SelectedDate
                .Firstname = txtFirstname.Text
                .Surname = txtSurname.Text
                .ContactNo = txtContactNo.Text
                .NameOfEmployer = txtNameOfEmployer.Text
                .SocialSupportSystems = txtSocialSupportSystems.Text
                .AlternateSourcesOfIncome = txtAlternateSourcesOfIncome.Text

                If .Save Then

                    If Not IsNumeric(txtCaregiverID.Text) OrElse Trim(txtCaregiverID.Text) = 0 Then txtCaregiverID.Text = .CaregiverID
                    LoadCaregiver(.PatientID)
                    ShowMessage("Caregiver saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtCaregiverID.Text = ""
        txtmPatientID.Text = ""
        If Not IsNothing(cboProfession.Items.FindByValue("")) Then
            cboProfession.SelectedValue = ""
        ElseIf Not IsNothing(cboProfession.Items.FindByValue(0)) Then
            cboProfession.SelectedValue = 0
        Else
            cboProfession.SelectedIndex = -1
        End If
        If Not IsNothing(cboRelationship.Items.FindByValue("")) Then
            cboRelationship.SelectedValue = ""
        ElseIf Not IsNothing(cboRelationship.Items.FindByValue(0)) Then
            cboRelationship.SelectedValue = 0
        Else
            cboRelationship.SelectedIndex = -1
        End If
        radDateofBirth.Clear()
        txtFirstname.Text = ""
        txtSurname.Text = ""
        txtContactNo.Text = ""
        txtNameOfEmployer.Text = ""
        txtSocialSupportSystems.Text = ""
        txtAlternateSourcesOfIncome.Text = ""

    End Sub

End Class


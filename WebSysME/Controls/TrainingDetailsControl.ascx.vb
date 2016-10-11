Imports BusinessLogic

Partial Class TrainingDetailsControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

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

                Dim objLookup As New BusinessLogic.CommonFunctions

                With cboTrainingType

                    .DataSource = objLookup.Lookup("luTrainingTypes", "TrainingTypeID", "Description").Tables(0)
                    .DataValueField = "TrainingTypeID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                    .SelectedIndex = 0

            End With

            With cboOrganization

                .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name").Tables(0)
                .DataValueField = "OrganizationID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

                If Not IsNothing(Request.QueryString("id")) Then

                    LoadTraining(objUrlEncoder.Decrypt(Request.QueryString("id")))

                End If

            End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadTraining(ByVal TrainingID As Long) As Boolean

        Try

            Dim objTraining As New Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTraining

                If .Retrieve(TrainingID) Then

                    txtTrainingID.Text = .TrainingID
                    If Not IsNothing(cboTrainingType.Items.FindByValue(.TrainingTypeID)) Then cboTrainingType.SelectedValue = .TrainingTypeID
                    If Not IsNothing(cboOrganization.Items.FindByValue(.OrganizationID)) Then cboOrganization.SelectedValue = .OrganizationID
                    txtName.Text = .Name
                    If Not .FromDate = "" Then radDate.SelectedDate = .FromDate
                    If Not .ToDate = "" Then radToDate.SelectedDate = .ToDate
                    txtDescription.Text = .Description
                    txtLocation.Text = .Location
                    txtFacilitator.Text = .Facilitators

                    ShowMessage("Training details loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Training details", MessageTypeEnum.Error)
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

            Dim objTraining As New Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objTraining

                .TrainingID = IIf(IsNumeric(txtTrainingID.Text), txtTrainingID.Text, 0)
                If cboTrainingType.SelectedIndex > -1 Then .TrainingTypeID = cboTrainingType.SelectedValue
                If cboOrganization.SelectedIndex > -1 Then .OrganizationID = cboOrganization.SelectedValue
                .Name = txtName.Text
                If radDate.SelectedDate.HasValue Then .FromDate = radDate.SelectedDate
                If radToDate.SelectedDate.HasValue Then .ToDate = radToDate.SelectedDate
                .Description = txtDescription.Text
                .Location = txtLocation.Text
                .Facilitators = txtFacilitator.Text

                If .Save Then

                    If Not IsNumeric(txtTrainingID.Text) OrElse Trim(txtTrainingID.Text) = 0 Then txtTrainingID.Text = .TrainingID
                    LoadTraining(.TrainingID)
                    ShowMessage("Training details saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save Training details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtTrainingID.Text = ""
        If Not IsNothing(cboTrainingType.Items.FindByValue("")) Then
            cboTrainingType.SelectedValue = ""
        ElseIf Not IsNothing(cboTrainingType.Items.FindByValue(0)) Then
            cboTrainingType.SelectedValue = 0
        Else
            cboTrainingType.SelectedIndex = -1
        End If
        If Not IsNothing(cboOrganization.Items.FindByValue("")) Then
            cboOrganization.SelectedValue = ""
        ElseIf Not IsNothing(cboOrganization.Items.FindByValue(0)) Then
            cboOrganization.SelectedValue = 0
        Else
            cboOrganization.SelectedIndex = -1
        End If
        txtName.Text = ""
        radDate.Clear()
        radToDate.Clear()
        txtDescription.Text = ""
        txtLocation.Text = ""
        txtFacilitator.Text = ""

    End Sub

    Private Sub lnkBeneficiaries_Click(sender As Object, e As EventArgs) Handles lnkBeneficiaries.Click

        If IsNumeric(txtTrainingID.Text) Then

            Response.Redirect("~/TrainingAttendantsPage.aspx?id=" & objUrlEncoder.Encrypt(txtTrainingID.Text))

        End If

    End Sub

    Private Sub lnkFiles_Click(sender As Object, e As EventArgs) Handles lnkFiles.Click

        If IsNumeric(txtTrainingID.Text) Then

            Response.Redirect("~/TrainingDocuments.aspx?id=" & objUrlEncoder.Encrypt(txtTrainingID.Text))

        End If

    End Sub

    Private Sub lnkInputs_Click(sender As Object, e As EventArgs) Handles lnkInputs.Click

        If IsNumeric(txtTrainingID.Text) Then

            Response.Redirect("~/TrainingInputsPage?id=" & objUrlEncoder.Encrypt(txtTrainingID.Text))

        End If

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Clear()

    End Sub

    Private Sub lnkMarks_Click(sender As Object, e As EventArgs) Handles lnkMarks.Click

        If IsNumeric(txtTrainingID.Text) Then

            Response.Redirect("~/TrainingMarksPage?id=" & objUrlEncoder.Encrypt(txtTrainingID.Text))

        End If

    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click

        If IsNumeric(txtTrainingID.Text) Then

            Dim objTraining As New Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            objTraining.TrainingID = txtTrainingID.Text

            If objTraining.Delete() Then

                Response.Redirect("~/TrainingDetails.aspx")
                ShowMessage("Training deleted successfully...", MessageTypeEnum.Information)

            Else

                ShowMessage("Failed to delete Training.", MessageTypeEnum.Error)

            End If

        End If

    End Sub
End Class


Imports BusinessLogic

Partial Class GeneralActivityDetailsControl
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

            With cboActivity

                .DataSource = objLookup.Lookup("tblActivities", "ActivityID", "Description").Tables(0)
                .DataValueField = "ActivityID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboStaff

                .DataSource = objLookup.Lookup("tblStaffMembers", "StaffID", "FirstName").Tables(0)
                .DataValueField = "StaffID"
                .DataTextField = "FirstName"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadGeneralActivity(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadGeneralActivity(ByVal GeneralActivityID As Long) As Boolean

        Try

            Dim objGeneralActivity As New Appointments(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGeneralActivity

                If .Retrieve(GeneralActivityID) Then

                    txtGeneralActivityID.Text = .ID
                    If Not IsNothing(cboActivity.Items.FindByValue(.ActivityID)) Then cboActivity.SelectedValue = .ActivityID
                    If Not IsNothing(cboStaff.Items.FindByValue(.UserID)) Then cboStaff.SelectedValue = .UserID
                    If Not .Start = "" Then radDate.SelectedDate = .Start
                    If Not .EndDate = "" Then radEnd.SelectedDate = .EndDate
                    txtDescription.Text = .Description
                    txtSubject.Text = .Subject

                    ShowMessage("GeneralActivity loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadGeneralActivity...", MessageTypeEnum.Error)
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

            Dim objGeneralActivity As New Appointments(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objGeneralActivity

                .ID = IIf(IsNumeric(txtGeneralActivityID.Text), txtGeneralActivityID.Text, 0)
                If cboActivity.SelectedIndex > -1 Then .ActivityID = cboActivity.SelectedValue
                If cboStaff.SelectedIndex > -1 Then .UserID = cboStaff.SelectedValue
                If radDate.SelectedDate.HasValue Then .Start = radDate.SelectedDate
                If radEnd.SelectedDate.HasValue Then .EndDate = radEnd.SelectedDate
                .Description = txtDescription.Text
                .Subject = txtSubject.Text

                If .Save Then

                    If Not IsNumeric(txtGeneralActivityID.Text) OrElse Trim(txtGeneralActivityID.Text) = 0 Then txtGeneralActivityID.Text = .ID
                    LoadGeneralActivity(.ID)
                    ShowMessage("GeneralActivity saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtGeneralActivityID.Text = ""
        If Not IsNothing(cboActivity.Items.FindByValue("")) Then
            cboActivity.SelectedValue = ""
        ElseIf Not IsNothing(cboActivity.Items.FindByValue(0)) Then
            cboActivity.SelectedValue = 0
        Else
            cboActivity.SelectedIndex = -1
        End If
        radDate.Clear()
        radEnd.Clear()
        txtSubject.Text = ""
        txtDescription.Text = ""

    End Sub

    Private Sub lnkAttendants_Click(sender As Object, e As EventArgs) Handles lnkAttendants.Click

        If IsNumeric(txtGeneralActivityID.Text) Then

            Response.Redirect("~/GeneralActivityAttendantsPage.aspx?id=" & objUrlEncoder.Encrypt(txtGeneralActivityID.Text))

        End If

    End Sub

    Private Sub lnkInputs_Click(sender As Object, e As EventArgs) Handles lnkInputs.Click

        If IsNumeric(txtGeneralActivityID.Text) Then

            Response.Redirect("~/GeneralActivityInputsPage.aspx?id=" & objUrlEncoder.Encrypt(txtGeneralActivityID.Text))

        End If

    End Sub

    Private Sub lnkFiles_Click(sender As Object, e As EventArgs) Handles lnkFiles.Click

        If IsNumeric(txtGeneralActivityID.Text) Then

            Response.Redirect("~/GeneralActivityDocuments.aspx?id=" & objUrlEncoder.Encrypt(txtGeneralActivityID.Text))

        End If

    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click

        Dim objGeneralActivity As New Appointments(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objGeneralActivity

            .ID = IIf(IsNumeric(txtGeneralActivityID.Text), txtGeneralActivityID.Text, 0)

            If .Delete() Then

                ShowMessage("General Activity deleted successfully...", MessageTypeEnum.Information)

            End If

        End With

    End Sub
End Class


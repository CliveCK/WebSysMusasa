Public Class ClientCounsellingSessionActivityDetails
    Inherits System.Web.UI.Page

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

            If Not IsNothing(Request.QueryString("benid")) Then

                txtBeneficiaryID.Text = objUrlEncoder.Decrypt(Request.QueryString("benid"))
                LoadBeneficiaryDetails(txtBeneficiaryID.Text)

            End If

            If Not IsNothing(Request.QueryString("id")) Then

                    LoadCounsellingSessionActivities(objUrlEncoder.Decrypt(Request.QueryString("id")))

                End If

            End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub LoadBeneficiaryDetails(ByVal BeneficiaryID As Long)

        Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objBeneficiary

            If .Retrieve(BeneficiaryID) Then

                txtBeneficiaryID.Text = .BeneficiaryID
                txtFirstName.Text = .FirstName
                txtSurname.Text = .Surname
                If Not .DateOfBirth = "" Then radDOB.SelectedDate = .DateOfBirth
                If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                txtNationalIDNumber.Text = .NationlIDNo
                txtContactNumber.Text = .ContactNo

            End If

        End With

    End Sub

    Public Function LoadCounsellingSessionActivities(ByVal ClientSessionActivityID As Long) As Boolean

        Try

            Dim objCounsellingSessionActivities As New BusinessLogic.CounsellingSessionActivities(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCounsellingSessionActivities

                If .Retrieve(ClientSessionActivityID) Then

                    txtClientSessionActivityID.Text = .ClientSessionActivityID
                    txtBeneficiaryID.Text = .BeneficiaryID
                    If Not .ActivityDate <> "" Then radActivityDate.SelectedDate = .ActivityDate
                    txtActivity.Text = .Activity
                    txtDescription.Text = .Description
                    txtOutcome.Text = .Outcome
                    txtRemarks.Text = .Remarks

                    With objBeneficiary

                        If .Retrieve(objCounsellingSessionActivities.BeneficiaryID) Then

                            txtBeneficiaryID.Text = .BeneficiaryID
                            txtFirstName.Text = .FirstName
                            txtSurname.Text = .Surname
                            If Not .DateOfBirth = "" Then radDOB.SelectedDate = .DateOfBirth
                            If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                            txtNationalIDNumber.Text = .NationlIDNo
                            txtContactNumber.Text = .ContactNo

                        End If

                    End With

                    ShowMessage("CounsellingSessionActivities loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    'ShowMessage("Failed to loadCounsellingSessionActivities: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objCounsellingSessionActivities As New BusinessLogic.CounsellingSessionActivities(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim update As Boolean = False

            'First save the beneficiary details - We will need the BeneficiaryID for the Client details

            'With objBeneficiary

            '    .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
            '    .FirstName = txtFirstName.Text
            '    .Surname = txtSurname.Text
            '    .NationlIDNo = txtContactNumber.Text
            '    If cboSex.SelectedIndex > -1 Then .Sex = cboSex.SelectedValue
            '    If radDOB.SelectedDate.HasValue Then .DateOfBirth = radDOB.SelectedDate
            '    .ContactNo = txtContactNumber.Text
            '    .Suffix = 1

            '    update = .BeneficiaryID > 0

            '    If .Save Then

            '        txtBeneficiaryID.Text = .BeneficiaryID
            '        If update = False Then .MemberNo = .GenerateMemberNo
            '        .Save()

            '    Else

            '        ShowMessage("Failed to save beneficiary details...Process aborted!!", MessageTypeEnum.Error)
            '        Exit Function

            '    End If

            'End With

            With objCounsellingSessionActivities

                .ClientSessionActivityID = IIf(IsNumeric(txtClientSessionActivityID.Text), txtClientSessionActivityID.Text, 0)
                If IsNumeric(txtBeneficiaryID.Text) Then
                    .BeneficiaryID = txtBeneficiaryID.Text
                Else
                    ShowMessage("Missing beneficiary Information", MessageTypeEnum.Error)
                    Exit Function
                End If
                If radActivityDate.SelectedDate.HasValue Then .ActivityDate = radActivityDate.SelectedDate
                .Activity = txtActivity.Text
                .Description = txtDescription.Text
                .Outcome = txtOutcome.Text
                .Remarks = txtRemarks.Text

                If .Save Then

                    If Not IsNumeric(txtClientSessionActivityID.Text) OrElse Trim(txtClientSessionActivityID.Text) = 0 Then txtClientSessionActivityID.Text = .ClientSessionActivityID
                    ShowMessage("CounsellingSessionActivities saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Could not save details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtClientSessionActivityID.Text = ""
        radActivityDate.Clear()
        txtActivity.Text = ""
        txtDescription.Text = ""
        txtOutcome.Text = ""
        txtRemarks.Text = ""

    End Sub

End Class
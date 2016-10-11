
Imports BusinessLogic
Public Class CallCenterDetails
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            radDtDOB.MaxDate = Now
            radCallDate.MaxDate = Now

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name").Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name").Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboTypeOfIssue

                .DataSource = objLookup.Lookup("luNatureOfProblems", "NatureOfProblemID", "Description").Tables(0)
                .DataValueField = "NatureOfProblemID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboReferredFrom

                .DataSource = objLookup.Lookup("luReferralCentreTypes", "ReferralCentreTypeID", "Description").Tables(0)
                .DataValueField = "ReferralCentreTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With


            With cboReferredTo

                .DataSource = objLookup.Lookup("luReferralCentreTypes", "ReferralCentreTypeID", "Description").Tables(0)
                .DataValueField = "ReferralCentreTypeID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadCallCenterDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If


        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub cboDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDistrict.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboDistrict.SelectedIndex > 0 Then

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name", , "DistrictID = " & cboDistrict.SelectedValue).Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Public Function LoadCallCenterDetails(ByVal CallCenterDetailID As Long) As Boolean

        Try

            Dim objCallCenterDetails As New BusinessLogic.CallCenterDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCallCenterDetails

                If .Retrieve(CallCenterDetailID) Then

                    txtCallCenterDetailID.Text = .CallCenterDetailID
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    If Not IsNothing(cboWard.Items.FindByValue(.WardID)) Then cboWard.SelectedValue = .WardID
                    If Not IsNothing(cboTypeOfIssue.Items.FindByValue(.TypeOfIssueID)) Then cboTypeOfIssue.SelectedValue = .TypeOfIssueID
                    If Not IsNothing(cboReferredFrom.Items.FindByValue(.ReferredFromID)) Then cboReferredFrom.SelectedValue = .ReferredFromID
                    If Not IsNothing(cboReferredTo.Items.FindByValue(.ReferredToID)) Then cboReferredTo.SelectedValue = .ReferredToID
                    If IsDate(.DOB) Then radDtDOB.SelectedDate = .DOB
                    If Not .CallDate = "" Then radCallDate.SelectedDate = .CallDate
                    txtCallCode.Text = .CallCode
                    txtCellNumber.Text = .CellNumber
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    txtNationalIDNum.Text = .NationalIDNum
                    txtAddress.Text = .Address
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    txtDetails.Text = .Details
                    txtActionTaken.Text = .ActionTaken
                    txtNotes.Text = .Notes

                    ShowMessage("CallCenterDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                        ShowMessage("Failed to loadCallCenterDetails: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objCallCenterDetails As New BusinessLogic.CallCenterDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCallCenterDetails

                .CallCenterDetailID = IIf(IsNumeric(txtCallCenterDetailID.Text), txtCallCenterDetailID.Text, 0)
                If cboDistrict.SelectedIndex > 0 Then .DistrictID = cboDistrict.SelectedValue
                If cboWard.SelectedIndex > 0 Then .WardID = cboWard.SelectedValue
                If cboTypeOfIssue.SelectedIndex > 0 Then .TypeOfIssueID = cboTypeOfIssue.SelectedValue
                If cboReferredFrom.SelectedIndex > 0 Then .ReferredFromID = cboReferredFrom.SelectedValue
                If cboReferredTo.SelectedIndex > 0 Then .ReferredToID = cboReferredTo.SelectedValue
                If cboSex.SelectedIndex > -1 Then .Sex = cboSex.SelectedValue
                .CallCode = txtCallCode.Text
                .CellNumber = txtCellNumber.Text
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .NationalIDNum = txtNationalIDNum.Text
                If radDtDOB.SelectedDate.HasValue Then .DOB = radDtDOB.SelectedDate
                If radCallDate.SelectedDate.HasValue Then .CallDate = radCallDate.SelectedDate
                .Address = txtAddress.Text
                .Details = txtDetails.Text
                .ActionTaken = txtActionTaken.Text
                .Notes = txtNotes.Text

                If .Save Then

                    If Not IsNumeric(txtCallCenterDetailID.Text) OrElse Trim(txtCallCenterDetailID.Text) = 0 Then txtCallCenterDetailID.Text = .CallCenterDetailID
                    ShowMessage("CallCenterDetails saved successfully...", MessageTypeEnum.Information)

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

        txtCallCenterDetailID.Text = ""
        If Not IsNothing(cboDistrict.Items.FindByValue("")) Then
            cboDistrict.SelectedValue = ""
        ElseIf Not IsNothing(cboDistrict.Items.FindByValue(0)) Then
            cboDistrict.SelectedValue = 0
        Else
            cboDistrict.SelectedIndex = -1
        End If
        If Not IsNothing(cboWard.Items.FindByValue("")) Then
            cboWard.SelectedValue = ""
        ElseIf Not IsNothing(cboWard.Items.FindByValue(0)) Then
            cboWard.SelectedValue = 0
        Else
            cboWard.SelectedIndex = -1
        End If
        If Not IsNothing(cboTypeOfIssue.Items.FindByValue("")) Then
            cboTypeOfIssue.SelectedValue = ""
        ElseIf Not IsNothing(cboTypeOfIssue.Items.FindByValue(0)) Then
            cboTypeOfIssue.SelectedValue = 0
        Else
            cboTypeOfIssue.SelectedIndex = -1
        End If
        If Not IsNothing(cboReferredFrom.Items.FindByValue("")) Then
            cboReferredFrom.SelectedValue = ""
        ElseIf Not IsNothing(cboReferredFrom.Items.FindByValue(0)) Then
            cboReferredFrom.SelectedValue = 0
        Else
            cboReferredFrom.SelectedIndex = -1
        End If
        If Not IsNothing(cboReferredTo.Items.FindByValue("")) Then
            cboReferredTo.SelectedValue = ""
        ElseIf Not IsNothing(cboReferredTo.Items.FindByValue(0)) Then
            cboReferredTo.SelectedValue = 0
        Else
            cboReferredTo.SelectedIndex = -1
        End If
        radDtDOB.Clear()
        radCallDate.Clear()
        txtCallCode.Text = ""
        txtCellNumber.Text = ""
        txtFirstName.Text = ""
        txtSurname.Text = ""
        txtNationalIDNum.Text = ""
        txtAddress.Text = ""
        txtDetails.Text = ""
        txtActionTaken.Text = ""
        txtNotes.Text = ""

    End Sub


End Class
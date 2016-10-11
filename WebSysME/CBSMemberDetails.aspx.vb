
Imports BusinessLogic
Imports Telerik.Web.UI

Public Class CBSMemberDetails
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

            Dim objLookup As New BusinessLogic.CommonFunctions
            With cboNeeds

                .DataSource = objLookup.Lookup("luNatureOfProblems", "NatureOfProblemID", "Description").Tables(0)
                .DataValueField = "NatureOfProblemID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboAssistance

                .DataSource = objLookup.Lookup("luAssistenceAndServicesProvided", "AssistenceAndServicesID", "Description").Tables(0)
                .DataValueField = "AssistenceAndServicesID"
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

                Dim objCBSMembers As New BusinessLogic.CBSMembers(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
                Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objBeneficiary

                    If .Retrieve(objUrlEncoder.Decrypt(Request.QueryString("id"))) Then

                        txtBeneficiaryID.Text = .BeneficiaryID
                        txtFirstName.Text = .FirstName
                        txtSurname.Text = .Surname
                        txtIDNumber.Text = .NationlIDNo
                        If Not .DateOfBirth = "" Then radDate.SelectedDate = .DateOfBirth
                        If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex

                    End If

                    LoadGrid(.BeneficiaryID)

                End With

            End If

            radDate.MaxDate = Now

        End If
    End Sub

    Private Sub Save()

        Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objNeeds As New BusinessLogic.HouseHoldNeeds(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objCBSMemberNeeds As New BusinessLogic.CBSMemberNeeds(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objCBSMemberBeneficiary As New BusinessLogic.BeneficiaryCBSMemberReportingID(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim update As Boolean = False

        With objBeneficiary

            .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
            .FirstName = txtFirstName.Text
            .Surname = txtSurname.Text
            .NationlIDNo = txtIDNumber.Text
            .Sex = cboSex.SelectedValue
            If radDate.SelectedDate.HasValue Then .DateOfBirth = radDate.SelectedDate
            .Suffix = 1

            update = .BeneficiaryID > 0

            If .Save Then

                If Not IsNumeric(txtBeneficiaryID.Text) OrElse Trim(txtBeneficiaryID.Text) = 0 Then txtBeneficiaryID.Text = .BeneficiaryID
                If update = True Then .MemberNo = .GenerateMemberNo

                .Save()

                With objCBSMemberBeneficiary

                    .BeneficiaryID = objBeneficiary.BeneficiaryID
                    .CBSMemberReportingID = objUrlEncoder.Decrypt(Request.QueryString("cbsid"))

                    If update = False Then
                        .Save()
                    End If

                End With

                With objCBSMemberNeeds

                    If cboNeeds.SelectedIndex > 0 Then .NeedID = cboNeeds.SelectedValue
                    .BeneficiaryID = objBeneficiary.BeneficiaryID
                    If cboAssistance.SelectedIndex > 0 Then .AssistanceID = cboAssistance.SelectedValue
                    If cboReferredTo.SelectedIndex > 0 Then .ReferredToID = cboReferredTo.SelectedValue
                    .Comments = txtComments.Text

                    If .NeedID > 0 Or .AssistanceID > 0 Or .ReferredToID > 0 Or .Comments <> "" Then
                        .Save()
                    End If

                    LoadGrid(objBeneficiary.BeneficiaryID)

                End With


                ShowMessage("Details saved successfully...", MessageTypeEnum.Information)

            Else

                ShowMessage("Details failed to save", MessageTypeEnum.Information)

            End If

        End With

    End Sub

    Private Sub LoadGrid(ByVal BeneficiaryID As Long)

        If BeneficiaryID > 0 Then

            Dim objCBSMembers As New BusinessLogic.CBSMembers(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With radBenListing

                .DataSource = objCBSMembers.GetAllCBSMembers(BeneficiaryID)
                .DataBind()

                ViewState("CBSNeeds") = .DataSource

            End With

        End If

    End Sub

    Protected Sub lnkCBSReportingList_Click(sender As Object, e As EventArgs) Handles lnkCBSReportingList.Click
        Response.Redirect("CBSMembers.aspx?id=" & Request.QueryString("cbsid"))
    End Sub

    Private Sub radBenListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radBenListing.NeedDataSource

        radBenListing.DataSource = DirectCast(ViewState("CBSNeeds"), DataSet)

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Not IsNothing(Request.QueryString("cbsid")) Then

            Save()

        Else

            ShowMessage("An error occured. Please make sure information from previous pages has been saved...", MessageTypeEnum.Error)

        End If
    End Sub
End Class
Imports BusinessLogic

Public Class CounsellingReturningClientDetails
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

        If Not IsPostBack Then

            LoadCombos()

            If Not IsNothing(Request.QueryString("id")) Then

                LoadReturningClientDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Public Sub LoadCombos()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboLevelOfEducation

            .DataSource = objLookup.Lookup("tblLevelOfEducation", "ObjectID", "Description").Tables(0)
            .DataValueField = "ObjectID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboDistricts

            .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name").Tables(0)
            .DataValueField = "DistrictID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboWards

            .DataSource = objLookup.Lookup("tblWards", "WardID", "Name").Tables(0)
            .DataValueField = "WardID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadReturningClientDetails(ByVal BeneficiaryID As Long) As Boolean

        Try

            Dim objReturningClientDetails As New ReturningClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                If .RetrieveWithAddress(BeneficiaryID) Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    If Not .DateOfBirth = "" Then radDateOfBirth.SelectedDate = .DateOfBirth
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    If Not IsNothing(cboDistricts.Items.FindByValue(.DistrictID)) Then cboDistricts.SelectedValue = .DistrictID
                    If Not IsNothing(cboWards.Items.FindByValue(.WardID)) Then cboWards.SelectedValue = .WardID
                    txtNationalIDNumber.Text = .NationlIDNo
                    If Not IsNothing(cboLevelOfEducation.Items.FindByValue(.LevelOfEducation)) Then cboLevelOfEducation.SelectedValue = .LevelOfEducation
                    txtPhoneNumber.Text = .ContactNo

                End If

            End With

            With objAddress

                If .Retrieve(BeneficiaryID) Then

                    txtAddressID.Text = .AddressID
                    txtAddress.Text = .Address

                End If

            End With

            With objReturningClientDetails

                If .Retrieve(BeneficiaryID) Then

                    txtReturningClientDetailID.Text = .ReturningClientDetailID
                    txtBeneficiaryID.Text = .BeneficiaryID
                    txtNumberOfAbuses.Text = .HowManyTimes
                    If Not .DateOfLastAbuse = "" Then radDtOfLastAbuse.SelectedDate = .DateOfLastAbuse
                    If Not .NextAppointmentDate = "" Then radDtOfNxtAppontmt.SelectedDate = .NextAppointmentDate
                    If Not .SessionDate = "" Then radSessionDate.SelectedDate = .SessionDate
                    cboAbusedAfterMusasa.SelectedValue = .AbusedAfterVisit
                    txtAbusedHow.Text = .AbusedHow
                    cboPrevAbuseContinued.SelectedValue = .HasPreviousAbuseContinued
                    cboNewKindOfAbuseStarted.SelectedValue = .HasNewAbuseStarted
                    txtNewAbuse.Text = .WhatKindOfAbuse
                    cboNewAbuseLinkedToMuasasaVisit.SelectedValue = .IsNewAbuseLinkedToVisit
                    txtClientActions.Text = .ActionToBeTakenByClient
                    txtCounsellorActions.Text = .ActionByCounsellor
                    txtReportIssuedBy.Text = .ReportIssuedBy
                    txtPermanentInjuries.Text = .PermanentInjuries
                    txtIssuesFromPrevSession.Text = .IssuesFromPreviousSession
                    txtFeedback.Text = .FeedbackFromLastSession
                    txtNewIssues.Text = .NewIssuesRaised
                    txtClientOptions.Text = .ClientOptions
                    cboReportMedToPolice.SelectedValue = .WasReportMadeToPolice
                    txtPoliceStation.Text = .NameOfPoliceStation
                    txtPoliceOfficer.Text = .Officer
                    cboWasPolcOfficerHelpful.Text = .WasOfficerHelpful
                    txtHowOfficerWsNotHelpful.Text = .IfNoWhy
                    cboMedicalReport.SelectedValue = .AnyMedicalReport

                    ShowMessage("ReturningClientDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    'ShowMessage("Failed to loadReturningClientDetails", MessageTypeEnum.Error)
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

            Dim objReturningClientDetails As New ReturningClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim update As Boolean = False

            'First save the beneficiary details - We will need the BeneficiaryID for the Client details

            With objBeneficiary

                .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .NationlIDNo = txtNationalIDNumber.Text
                If cboSex.SelectedIndex > -1 Then .Sex = cboSex.SelectedValue
                If radDateOfBirth.SelectedDate.HasValue Then .DateOfBirth = radDateOfBirth.SelectedDate
                If cboLevelOfEducation.SelectedIndex > 0 Then .LevelOfEducation = cboLevelOfEducation.SelectedValue
                .ContactNo = txtPhoneNumber.Text
                .Suffix = 1

                update = .BeneficiaryID > 0

                If .Save Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    If update = False Then .MemberNo = .GenerateMemberNo
                    .Save()

                    Dim objAdrress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objAdrress

                        .OwnerID = objBeneficiary.BeneficiaryID
                        If cboDistricts.SelectedIndex > 0 Then .DistrictID = cboDistricts.SelectedValue
                        If cboWards.SelectedIndex > 0 Then .WardID = cboWards.SelectedValue
                        .Address = txtAddress.Text

                        .Save()

                    End With

                Else

                    ShowMessage("Failed to save beneficiary details...Process aborted!!", MessageTypeEnum.Error)
                    Exit Function

                End If

            End With

            With objReturningClientDetails

                .ReturningClientDetailID = IIf(IsNumeric(txtReturningClientDetailID.Text), txtReturningClientDetailID.Text, 0)
                If IsNumeric(txtBeneficiaryID.Text) Then
                    .BeneficiaryID = txtBeneficiaryID.Text
                Else
                    ShowMessage("Missing beneficiary Information", MessageTypeEnum.Error)
                    Exit Function
                End If
                If IsNumeric(txtNumberOfAbuses.Text) Then .HowManyTimes = txtNumberOfAbuses.Text
                If radDtOfLastAbuse.SelectedDate.HasValue Then .DateOfLastAbuse = radDtOfLastAbuse.SelectedDate
                If radDtOfNxtAppontmt.SelectedDate.HasValue Then .NextAppointmentDate = radDtOfNxtAppontmt.SelectedDate
                If radSessionDate.SelectedDate.HasValue Then .SessionDate = radSessionDate.SelectedDate Else .SessionDate = Now
                .AbusedAfterVisit = cboAbusedAfterMusasa.SelectedValue
                .AbusedHow = txtAbusedHow.Text
                .HasPreviousAbuseContinued = cboPrevAbuseContinued.SelectedValue
                .HasNewAbuseStarted = cboNewKindOfAbuseStarted.SelectedValue
                .WhatKindOfAbuse = txtNewAbuse.Text
                .IsNewAbuseLinkedToVisit = cboNewAbuseLinkedToMuasasaVisit.SelectedValue
                .ActionToBeTakenByClient = txtClientActions.Text
                .ActionByCounsellor = txtCounsellorActions.Text
                .ReportIssuedBy = txtReportIssuedBy.Text
                .PermanentInjuries = txtPermanentInjuries.Text
                .IssuesFromPreviousSession = txtIssuesFromPrevSession.Text
                .FeedbackFromLastSession = txtFeedback.Text
                .NewIssuesRaised = txtNewIssues.Text
                .ClientOptions = txtClientOptions.Text
                .WasReportMadeToPolice = cboReportMedToPolice.SelectedValue
                .NameOfPoliceStation = txtPoliceStation.Text
                .Officer = txtPoliceOfficer.Text
                .WasOfficerHelpful = cboWasPolcOfficerHelpful.SelectedValue
                .IfNoWhy = txtHowOfficerWsNotHelpful.Text
                .AnyMedicalReport = cboMedicalReport.SelectedValue


                If .Save Then

                    If Not IsNumeric(txtReturningClientDetailID.Text) OrElse Trim(txtReturningClientDetailID.Text) = 0 Then txtReturningClientDetailID.Text = .ReturningClientDetailID
                    ShowMessage("ReturningClientDetails saved successfully...", MessageTypeEnum.Information)

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

        txtReturningClientDetailID.Text = ""
        txtBeneficiaryID.Text = ""
        txtNumberOfAbuses.Text = ""
        radDtOfLastAbuse.Clear()
        radDtOfNxtAppontmt.Clear()
        radSessionDate.Clear()
        txtAbusedHow.Text = ""
        txtNewAbuse.Text = ""
        txtClientActions.Text = ""
        txtCounsellorActions.Text = ""
        txtReportIssuedBy.Text = ""
        txtPermanentInjuries.Text = ""
        txtIssuesFromPrevSession.Text = ""
        txtFeedback.Text = ""
        txtNewIssues.Text = ""
        txtClientOptions.Text = ""
        txtPoliceStation.Text = ""
        txtPoliceOfficer.Text = ""
        cboWasPolcOfficerHelpful.Text = ""
        txtHowOfficerWsNotHelpful.Text = ""

    End Sub

End Class
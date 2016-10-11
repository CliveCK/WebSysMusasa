Imports BusinessLogic


Public Class InitialCounsellingSession
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

            radDOB.MaxDate = Now

            LoadCombos()

            If Not IsNothing(Request.QueryString("id")) Then

                LoadInitialCounsellingSession(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Private Sub LoadCombos()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With lstProblemCategory
            .DataSource = objLookup.Lookup("luNatureOfProblems", "NatureOfProblemID", "Description").Tables(0)
            .DataValueField = "NatureOfProblemID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0
        End With

        With cboMaritalStatus

            .DataSource = objLookup.Lookup("luMaritalStatus", "ObjectID", "Description").Tables(0)
            .DataValueField = "ObjectID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboLawyer

            .DataSource = objLookup.GetStaffByType("Lawyer").Tables(0)
            .DataValueField = "StaffID"
            .DataTextField = "StaffFullName"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboshelter

            .DataSource = objLookup.GetStaffByType("Shelter").Tables(0)
            .DataValueField = "StaffID"
            .DataTextField = "StaffFullName"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadInitialCounsellingSession(ByVal BeneficiaryID As Long) As Boolean

        Try

            Dim objInitialCounsellingSession As New BusinessLogic.InitialCounsellingSession(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                If .Retrieve(BeneficiaryID) Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    If Not .DateOfBirth = "" Then radDOB.SelectedDate = .DateOfBirth
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    txtNationalIDNumber.Text = .NationlIDNo
                    If Not IsNothing(cboMaritalStatus.Items.FindByValue(.MaritalStatus)) Then cboMaritalStatus.SelectedValue = .MaritalStatus
                    If Not IsNothing(cboLevelOfEducation.Items.FindByValue(.LevelOfEducation)) Then cboLevelOfEducation.SelectedValue = .LevelOfEducation

                End If

            End With

            With objInitialCounsellingSession

                If .Retrieve(BeneficiaryID) Then

                    txtInitialCounsellingSessionID.Text = .InitialCounsellingSessionID
                    'If Not IsNothing(cboProblemCategory.Items.FindByValue(.ProblemCategoryID)) Then cboProblemCategory.SelectedValue = .ProblemCategoryID
                    txtTimesRpted.Text = .HowManyTimes
                    If Not IsNothing(cboLawyer.Items.FindByValue(.LawyerID)) Then cboLawyer.SelectedValue = .LawyerID
                    If Not IsNothing(cboshelter.Items.FindByValue(.ShelterID)) Then cboshelter.SelectedValue = .ShelterID
                    If Not .CreatedDate = "" Then radSessionDate.SelectedDate = .CreatedDate
                    If Not .NextAppointmentDate = "" Then radDtNext.SelectedDate = .NextAppointmentDate
                    txtPresentingProblem.Text = .PresentingProblem
                    'txtOther.Text = .Other
                    cboCaseWasReported.Text = .CaseReported
                    txtPlaceWhereReported.Text = .WhereWasProblemReported
                    txtChallengesFaced.Text = .ChallengesFaced
                    cboMedicalReport.Text = .MedicalReport
                    txtSessionDuration.Text = .DurationOfSession
                    txtIssuedBy.Text = .IssuedBy
                    txtPropSpecification.Text = .ProblemsExperienced
                    txtClientExpectations.Text = .ClientExpectations
                    txtOtherOptionAvailable.Text = .OtherOptionAvailable
                    txtReferral.Text = .Referral
                    txtCarePlan.Text = .CarePlan

                    ShowMessage("InitialCounsellingSession loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    'ShowMessage("Failed to loadInitialCounsellingSession", MessageTypeEnum.Error)
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

            Dim objInitialCounsellingSession As New BusinessLogic.InitialCounsellingSession(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            'First save the beneficiary details - We will need the BeneficiaryID for the Client details

            With objBeneficiary

                .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .NationlIDNo = txtNationalIDNumber.Text
                If cboSex.SelectedIndex > -1 Then .Sex = cboSex.SelectedValue
                If cboLevelOfEducation.SelectedIndex > 0 Then .LevelOfEducation = cboLevelOfEducation.SelectedValue
                If cboMaritalStatus.SelectedIndex > 0 Then .MaritalStatus = cboMaritalStatus.SelectedValue
                If radDOB.SelectedDate.HasValue Then .DateOfBirth = radDOB.SelectedDate
                .ContactNo = txtPhoneNumber.Text
                .Suffix = 1

                If .Save Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    .MemberNo = .GenerateMemberNo
                    .Save()

                Else

                    ShowMessage("Failed to save beneficiary details...Process aborted!!", MessageTypeEnum.Error)
                    Exit Function

                End If

            End With

            With objInitialCounsellingSession

                .InitialCounsellingSessionID = IIf(IsNumeric(txtInitialCounsellingSessionID.Text), txtInitialCounsellingSessionID.Text, 0)
                If IsNumeric(txtBeneficiaryID.Text) Then
                    .BeneficiaryID = txtBeneficiaryID.Text
                Else
                    ShowMessage("Missing beneficiary Information", MessageTypeEnum.Error)
                    Exit Function
                End If
                'If cboProblemCategory.SelectedIndex > 0 Then .ProblemCategoryID = cboProblemCategory.SelectedValue
                .HowManyTimes = txtTimesRpted.Text
                If cboLawyer.SelectedIndex > 0 Then .LawyerID = cboLawyer.SelectedValue
                If cboshelter.SelectedIndex > 0 Then .ShelterID = cboshelter.SelectedValue
                'If radSessionDate.SelectedDate.HasValue Then .SessionDate = radSessionDate.SelectedDate
                If radDtNext.SelectedDate.HasValue Then .NextAppointmentDate = radDtNext.SelectedDate
                .ReferredToLaywer = cboLawyer.SelectedIndex > 0
                .ReferredToShelter = cboshelter.SelectedIndex > 0
                .PresentingProblem = txtPresentingProblem.Text
                .Other = txtOtherOptionAvailable.Text
                .CaseReported = cboCaseWasReported.SelectedValue
                .WhereWasProblemReported = txtPlaceWhereReported.Text
                .ChallengesFaced = txtChallengesFaced.Text
                .MedicalReport = cboMedicalReport.SelectedValue
                .DurationOfSession = txtSessionDuration.Text
                .IssuedBy = txtIssuedBy.Text
                .ProblemsExperienced = txtPropSpecification.Text
                .ClientExpectations = txtClientExpectations.Text
                .OtherOptionAvailable = txtOtherOptionAvailable.Text
                .Referral = txtReferral.Text
                .CarePlan = txtCarePlan.Text

                If .Save Then

                    If Not IsNumeric(txtInitialCounsellingSessionID.Text) OrElse Trim(txtInitialCounsellingSessionID.Text) = 0 Then txtInitialCounsellingSessionID.Text = .InitialCounsellingSessionID
                    ShowMessage("InitialCounsellingSession saved successfully...", MessageTypeEnum.Information)

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

        txtInitialCounsellingSessionID.Text = ""
        txtTimesRpted.Text = 0
        If Not IsNothing(cboLawyer.Items.FindByValue("")) Then
            cboLawyer.SelectedValue = ""
        ElseIf Not IsNothing(cboLawyer.Items.FindByValue(0)) Then
            cboLawyer.SelectedValue = 0
        Else
            cboLawyer.SelectedIndex = -1
        End If
        If Not IsNothing(cboShelter.Items.FindByValue("")) Then
            cboShelter.SelectedValue = ""
        ElseIf Not IsNothing(cboShelter.Items.FindByValue(0)) Then
            cboShelter.SelectedValue = 0
        Else
            cboShelter.SelectedIndex = -1
        End If
        radSessionDate.Clear()
        radDtNext.Clear()
        txtPresentingProblem.Text = ""
        txtPlaceWhereReported.Text = ""
        txtChallengesFaced.Text = ""
        txtSessionDuration.Text = ""
        txtIssuedBy.Text = ""
        txtPropSpecification.Text = ""
        txtClientExpectations.Text = ""
        txtOtherOptionAvailable.Text = ""
        txtReferral.Text = ""
        txtCarePlan.Text = ""

    End Sub

End Class
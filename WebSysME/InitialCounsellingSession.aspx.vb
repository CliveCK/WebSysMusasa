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
            If Not IsNothing(Request.QueryString("enabled")) Then

                DisableControls(Page, False)

            End If

            If Not IsNothing(Request.QueryString("id")) Then

                LoadInitialCounsellingSession(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub
    Private Sub DisableControls(ByVal Parent As Control, ByVal state As Boolean)
        For Each c As Control In Parent.Controls
            If TypeOf (c) Is DropDownList Then
                DirectCast(c, DropDownList).Enabled = state
            End If

            If TypeOf (c) Is TextBox Then
                DirectCast(c, TextBox).Enabled = state
            End If

            If TypeOf (c) Is Button Then
                DirectCast(c, Button).Enabled = state
            End If

            DisableControls(c, state)

        Next
    End Sub

    Private Sub LoadCombos()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With lstProblemCategory
            .DataSource = objLookup.Lookup("luNatureOfProblems", "NatureOfProblemID", "Description").Tables(0)
            .DataValueField = "NatureOfProblemID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))

        End With

        With cboMaritalStatus

            .DataSource = objLookup.Lookup("luMaritalStatus", "ObjectID", "Description").Tables(0)
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
            Dim objInitialSessionProblemCat As New BusinessLogic.InitialSessionProblemCategory(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objClientDetails As New BusinessLogic.ClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                If .RetrieveWithAddress(BeneficiaryID) Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    txtPhoneNumber.Text = .ContactNo
                    If Not .DateOfBirth = "" Then radDOB.SelectedDate = .DateOfBirth
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    txtNationalIDNumber.Text = .NationlIDNo
                    If Not IsNothing(cboMaritalStatus.Items.FindByValue(.MaritalStatus)) Then cboMaritalStatus.SelectedValue = .MaritalStatus
                    If Not IsNothing(cboLevelOfEducation.Items.FindByValue(.LevelOfEducation)) Then cboLevelOfEducation.SelectedValue = .LevelOfEducation
                    If Not IsNothing(cboDistricts.Items.FindByValue(.DistrictID)) Then cboDistricts.SelectedValue = .DistrictID
                    If Not IsNothing(cboWards.Items.FindByValue(.WardID)) Then cboWards.SelectedValue = .WardID

                End If

            End With

            With objAddress

                If .Retrieve(BeneficiaryID) Then

                    txtAddressID.Text = .AddressID
                    txtAddress.Text = .Address

                End If

            End With

            With objClientDetails

                If .Retrieve(BeneficiaryID) Then

                    txtNextOfKin.Text = .NextOfKin
                    txtReferredBy.Text = .ReferredBy
                    txtEmploymentStatus.Text = .EmploymentStatusID

                End If

            End With

            With objInitialCounsellingSession

                If .Retrieve(BeneficiaryID) Then

                    txtInitialCounsellingSessionID.Text = .InitialCounsellingSessionID
                    'If Not IsNothing(cboProblemCategory.Items.FindByValue(.ProblemCategoryID)) Then cboProblemCategory.SelectedValue = .ProblemCategoryID
                    txtTimesRpted.Text = .HowManyTimes
                    If Not IsNothing(cboshelter.Items.FindByValue(.ShelterID)) Then cboshelter.SelectedValue = .ShelterID
                    If Not IsNothing(cboLawyer.Items.FindByValue(.LawyerID)) Then cboLawyer.SelectedValue = .LawyerID
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
                    txtProblemsFaced.Text = .ProblemsExperienced
                    txtPropSpecification.Text = .ProblemsExperienced
                    txtClientExpectations.Text = .ClientExpectations
                    txtOtherOptionAvailable.Text = .OtherOptionAvailable
                    txtReferral.Text = .Referral
                    txtCarePlan.Text = .CarePlan
                    txtEmploymentStatus.Text = .EmploymentStatus
                    txtSupport.Text = .Support

                    Dim dsInitialSessionProblemCat As DataSet = objInitialSessionProblemCat.GetInitialSessionProblemCategory(.InitialCounsellingSessionID)

                    If Not IsNothing(dsInitialSessionProblemCat) AndAlso dsInitialSessionProblemCat.Tables.Count > 0 AndAlso dsInitialSessionProblemCat.Tables(0).Rows.Count > 0 Then

                        For Each i As ListItem In lstProblemCategory.Items

                            If dsInitialSessionProblemCat.Tables(0).Select("ProblemID = " & i.Value).Length > 0 Then

                                i.Selected = True

                            End If

                        Next

                    End If

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
            Dim update As Boolean = False

            'First save the beneficiary details - We will need the BeneficiaryID for the Client details

            With objBeneficiary

                .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .NationlIDNo = txtNationalIDNumber.Text
                .Sex = cboSex.SelectedValue
                If cboLevelOfEducation.SelectedIndex > 0 Then .LevelOfEducation = cboLevelOfEducation.SelectedValue
                If cboMaritalStatus.SelectedIndex > 0 Then .MaritalStatus = cboMaritalStatus.SelectedValue
                If radDOB.SelectedDate.HasValue Then .DateOfBirth = radDOB.SelectedDate
                .ContactNo = txtPhoneNumber.Text
                .Suffix = 1

                update = .BeneficiaryID > 0

                If .Save Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    If update = False Then .MemberNo = .GenerateMemberNo
                    .Save()

                    Dim objAdrress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objAdrress

                        .AddressID = IIf(IsNumeric(txtAddressID.Text), txtAddressID.Text, 0)
                        .OwnerID = objBeneficiary.BeneficiaryID
                        If cboDistricts.SelectedIndex > 0 Then .DistrictID = cboDistricts.SelectedValue
                        If cboWards.SelectedIndex > 0 Then .WardID = cboWards.SelectedValue
                        .Address = txtAddress.Text

                        If .Save() Then

                            If Not IsNumeric(txtAddressID.Text) OrElse Trim(txtAddressID.Text) = 0 Then txtAddressID.Text = .AddressID

                        End If

                    End With

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
                If IsNumeric(txtTimesRpted.Text) Then .HowManyTimes = txtTimesRpted.Text
                If cboLawyer.SelectedIndex > 0 Then .LawyerID = cboLawyer.SelectedValue
                If cboshelter.SelectedIndex > 0 Then .ShelterID = cboshelter.SelectedValue
                'If radSessionDate.SelectedDate.HasValue Then .SessionDate = radSessionDate.SelectedDate
                If radDtNext.SelectedDate.HasValue Then
                    If radDtNext.SelectedDate <= IIf(Not .SessionDate = "", .SessionDate, Now) Then

                        ShowMessage("Next Appointment date cannot be before Current session date", MessageTypeEnum.Error)
                        Exit Function

                    End If
                    .NextAppointmentDate = radDtNext.SelectedDate
                End If
                .ReferredToLaywer = cboLawyer.SelectedIndex > 0
                .ReferredToShelter = cboshelter.SelectedIndex > 0
                .SyncReferredBy(.BeneficiaryID, IIf(cboLawyer.SelectedIndex > 0, cboLawyer.SelectedValue, -1), IIf(cboshelter.SelectedIndex > 0, cboshelter.SelectedValue, -1), CookiesWrapper.StaffID)
                .PresentingProblem = txtPresentingProblem.Text
                .Other = txtOtherOptionAvailable.Text
                .CaseReported = cboCaseWasReported.SelectedValue
                .WhereWasProblemReported = txtPlaceWhereReported.Text
                .ChallengesFaced = txtChallengesFaced.Text
                .MedicalReport = cboMedicalReport.SelectedValue
                If IsNumeric(txtSessionDuration.Text) Then
                    .DurationOfSession = txtSessionDuration.Text
                Else
                    ShowMessage("Duration must be in minutes", MessageTypeEnum.Error) : Exit Function
                End If
                .IssuedBy = txtIssuedBy.Text
                .ProblemsExperienced = txtProblemsFaced.Text
                .ClientExpectations = txtClientExpectations.Text
                .OtherOptionAvailable = txtOtherOptionAvailable.Text
                .Referral = txtReferral.Text
                .CarePlan = txtCarePlan.Text
                .ReferredBy = txtReferredBy.Text
                .EmploymentStatus = txtEmploymentStatus.Text
                .NextOfkin = txtNextOfKin.Text
                .Support = txtSupport.Text

                If .Save Then

                    SaveKeyProblemCategory(.InitialCounsellingSessionID)

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

    Private Function SaveKeyProblemCategory(ByVal InitialCounsellingSessionID As Long) As Boolean

        Try

            For Each i As ListItem In lstProblemCategory.Items

                Dim objInitialSessionProblemCategory As New BusinessLogic.InitialSessionProblemCategory(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                With objInitialSessionProblemCategory

                    .InitialCounsellingSessionID = InitialCounsellingSessionID
                    .ProblemID = i.Value

                    If i.Selected = True And i.Value > 0 Then

                        If Not .CheckExistence() Then .Save()

                    Else

                        If .CheckExistence() Then

                            .DeleteEntry()

                        End If

                    End If

                End With

            Next

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Private Sub cboDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDistricts.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboDistricts.SelectedIndex > 0 Then

            With cboWards

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name", , "DistrictID = " & cboDistricts.SelectedValue).Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

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
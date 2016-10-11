Imports BusinessLogic

Public Class ShelterClientDetails
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

            If Not IsNothing(Request.QueryString("id")) Then

                LoadShelterClientDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

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

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadShelterClientDetails(ByVal BeneficiaryID As Long) As Boolean

        Try

            Dim objShelterClientDetails As New BusinessLogic.ShelterClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                If .RetrieveWithAddress(BeneficiaryID) Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    If Not .DateOfBirth = "" Then radDtDOB.SelectedDate = .DateOfBirth
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    txtNationalIDNum.Text = .NationlIDNo
                    txtClientContactNumber.Text = .ContactNo
                    If Not IsNothing(cboMaritalStatus.Items.FindByValue(.MaritalStatus)) Then cboMaritalStatus.SelectedValue = .MaritalStatus
                    If Not IsNothing(cboDistricts.Items.FindByValue(.DistrictID)) Then cboDistricts.SelectedValue = .DistrictID
                    If Not IsNothing(cboWards.Items.FindByValue(.WardID)) Then cboWards.SelectedValue = .WardID

                    With objAddress

                        If .Retrieve(BeneficiaryID) Then

                            txtAddressID.Text = .AddressID
                            txtClientPermanentAddress.Text = .Address

                        End If

                    End With

                End If

            End With

            With objShelterClientDetails

                If .Retrieve(BeneficiaryID) Then

                    txtShelterClientDetailID.Text = .ShelterClientDetailID
                    txtBeneficiaryID.Text = .BeneficiaryID
                    cboEmpoymentStatus.SelectedValue = .EmploymentStatus
                    txtEmployerTelNumber.Text = .EmployerTelNo
                    txtEmployerAddress.Text = .EmployerAddress
                    txtReferredBy.Text = .ReferredBy
                    txtReferrerTelNum.Text = .ReferrerTelNo
                    cboEverSheltered.SelectedValue = .SheltedBefore
                    txtInjuries.Text = .InjuriesSustained
                    cboSpecialNeeds.SelectedValue = .AnySpecialMedicalNeeds
                    txtSpecialNeeds.Text = .MedicalNeeds
                    txtCarePlan.Text = .CarePlan
                    txtPresentingProblem.Text = .PresentingProblem
                    txtSkillsToNature.Text = .SkillsToNature
                    cboSkillsToNature.SelectedValue = .Skills
                    txtEmergencyContactName.Text = .Name
                    txtRelationToEmergencyContact.Text = .Relationship
                    txtEmergencyContactNumber.Text = .ContactNo
                    txtEmergencyContactAddress.Text = .ContactAddress
                    txtTotalPeopleAdmitted.Text = .TotalAdmitted
                    txtArrivalTime.Text = .ArrivalTime

                    ShowMessage("ShelterClientDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadShelterClientDetails", MessageTypeEnum.Error)
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

            Dim objShelterClientDetails As New BusinessLogic.ShelterClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim update As Boolean = False

            'First save the beneficiary details - We will need the BeneficiaryID for the Client details

            With objBeneficiary

                .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .NationlIDNo = txtNationalIDNum.Text
                If cboMaritalStatus.SelectedIndex > -1 Then .MaritalStatus = cboMaritalStatus.SelectedValue
                If cboSex.SelectedIndex > -1 Then .Sex = cboSex.SelectedValue
                If radDtDOB.SelectedDate.HasValue Then .DateOfBirth = radDtDOB.SelectedDate
                .ContactNo = txtClientContactNumber.Text
                .Suffix = 1

                update = .BeneficiaryID > 0

                If .Save Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    If update = False Then .MemberNo = .GenerateMemberNo
                    .Save()

                    With objAddress

                        .AddressID = IIf(IsNumeric(txtAddressID.Text), txtAddressID.Text, 0)
                        .OwnerID = objBeneficiary.BeneficiaryID
                        If cboDistricts.SelectedIndex > 0 Then .DistrictID = cboDistricts.SelectedValue
                        If cboWards.SelectedIndex > 0 Then .WardID = cboWards.SelectedValue
                        .Address = txtClientPermanentAddress.Text

                        If .Save() Then

                            txtAddressID.Text = .AddressID

                        End If

                    End With

                Else

                    ShowMessage("Failed to save beneficiary details...Process aborted!!", MessageTypeEnum.Error)
                    Exit Function

                End If

            End With

            With objShelterClientDetails

                .ShelterClientDetailID = IIf(IsNumeric(txtShelterClientDetailID.Text), txtShelterClientDetailID.Text, 0)
                If IsNumeric(txtBeneficiaryID.Text) Then
                    .BeneficiaryID = txtBeneficiaryID.Text
                Else
                    ShowMessage("Missing beneficiary Information", MessageTypeEnum.Error)
                    Exit Function
                End If
                If cboEmpoymentStatus.SelectedIndex > -1 Then .EmploymentStatus = cboEmpoymentStatus.SelectedValue
                .EmployerTelNo = txtEmployerTelNumber.Text
                .EmployerAddress = txtEmployerAddress.Text
                .ReferredBy = txtReferredBy.Text
                .ReferrerTelNo = txtReferrerTelNum.Text
                .SheltedBefore = cboEverSheltered.SelectedValue
                .InjuriesSustained = txtInjuries.Text
                .AnySpecialMedicalNeeds = cboSpecialNeeds.Text
                .MedicalNeeds = txtSpecialNeeds.Text
                .CarePlan = txtCarePlan.Text
                .PresentingProblem = txtPresentingProblem.Text
                .SkillsToNature = txtSkillsToNature.Text
                .Skills = cboSkillsToNature.SelectedValue
                .Name = txtEmergencyContactName.Text
                .Relationship = txtRelationToEmergencyContact.Text
                .ContactNo = txtEmergencyContactNumber.Text
                .ContactAddress = txtEmergencyContactAddress.Text
                .ArrivalTime = txtArrivalTime.Text
                .TotalAdmitted = txtTotalPeopleAdmitted.Text

                If .Save Then

                    If Not IsNumeric(txtShelterClientDetailID.Text) OrElse Trim(txtShelterClientDetailID.Text) = 0 Then txtShelterClientDetailID.Text = .ShelterClientDetailID
                    ShowMessage("ShelterClientDetails saved successfully...", MessageTypeEnum.Information)

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

        txtEmployerTelNumber.Text = ""
        txtEmployerAddress.Text = ""
        txtReferredBy.Text = ""
        txtReferrerTelNum.Text = ""
        txtInjuries.Text = ""
        txtSpecialNeeds.Text = ""
        txtCarePlan.Text = ""
        txtPresentingProblem.Text = ""
        txtSkillsToNature.Text = ""
        txtSkillsToNature.Text = ""
        txtEmergencyContactName.Text = ""
        txtRelationToEmergencyContact.Text = ""
        txtEmergencyContactName.Text = ""
        txtEmergencyContactAddress.Text = ""
        txtArrivalTime.Text = ""
        txtTotalPeopleAdmitted.Text = ""

    End Sub


End Class
Imports BusinessLogic
Imports Telerik.Web.UI
Imports SysPermissionsManager.Functionality

Partial Class BeneficiaryDetailsControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString
    Private objCustomFields As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

    Public ReadOnly Property HouseholdID As Long
        Get
            Return txtBeneficiaryID1.Text
        End Get
    End Property

#Region "Status Messages"

    Public Event Message(ByVal Message As String, ByVal MessageType As MessageTypeEnum)

    Public Sub ShowMessage(ByVal Message As String, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message
        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

        If Not LocalOnly Then RaiseEvent Message(Message, MessageType)

    End Sub

    Public Sub ShowMessage(ByVal Message As Exception, ByVal MessageType As MessageTypeEnum, Optional ByVal LocalOnly As Boolean = False)

        lblError.Text = Message.Message
        If Message.InnerException IsNot Nothing Then lblError.Text &= " - " & Message.InnerException.Message
        If Not LocalOnly Then RaiseEvent Message(Message.Message, MessageType)

        pnlError.CssClass = "msg" & [Enum].GetName(GetType(MessageTypeEnum), MessageType)

    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            InitializeComponents()

            If Not IsNothing(Request.QueryString("id")) Then

                LoadBeneficiary(objUrlEncoder.Decrypt(Request.QueryString("id")))

                LoadGrid(objBeneficiary)

            End If

        End If

        'cboRelationships.Enabled = IIf(IsNumeric(txtSuffix.Text.Trim) AndAlso txtSuffix.Text.Trim <> 1, True, False)

    End Sub

    Private Sub InitializeComponents()

        LoadCombo(cboMaritalStatus, "luMaritalStatus", "Description", "ObjectID")
        LoadCombo(cboHealthStatus, "tblHealthStatus", "Description", "ObjectID")
        LoadCombo(cboDisabilityStatus, "tblDisabilityStatus", "Description", "ObjectID")
        LoadCombo(cboLevelOfEducation, "tblLevelOfEducation", "Level", "ObjectID")
        LoadCombo(cboRegularity, "tblRegularity", "Description", "ObjectID")
        LoadCombo(cboOpharnhood, "tblOrphanhood", "Description", "ObjectID")
        LoadCombo(cboMajorSourceIncome, "tblSourceOfIncome", "Description", "ObjectID")
        LoadCombo(cboCondition, "tblCondition", "Description", "ObjectID")
        LoadCombo(cboAttendance, "tblAttendance", "Description", "ObjectID")
        LoadCombo(cboDisability, "tblDisability", "Description", "ObjectID")
        LoadCombo(cboRelationships, "luRelationships", "Description", "RelationshipID")

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If SystemInitialization.EnforceUserFunctionalitySecurity(FunctionalityEnum.AddOrEditBeneficiaryDetails) Then

            Save()

        Else

            ShowMessage("You are not authorised to Add or Edit Beneficiary details...", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadBeneficiary(ByVal BeneficiaryID As Long) As Boolean

        Try

            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                If .Retrieve(BeneficiaryID) Then

                    txtBeneficiaryID1.Text = .BeneficiaryID
                    txtParentID.Text = .ParentID
                    txtSuffix.Text = .Suffix
                    If Not IsNothing(cboMaritalStatus.Items.FindByValue(.MaritalStatus)) Then cboMaritalStatus.SelectedValue = .MaritalStatus
                    If Not IsNothing(cboHealthStatus.Items.FindByValue(.HealthStatus)) Then cboHealthStatus.SelectedValue = .HealthStatus
                    If Not IsNothing(cboDisabilityStatus.Items.FindByValue(.DisabilityStatus)) Then cboDisabilityStatus.SelectedValue = .DisabilityStatus
                    If Not IsNothing(cboLevelOfEducation.Items.FindByValue(.LevelOfEducation)) Then cboLevelOfEducation.SelectedValue = .LevelOfEducation
                    If Not IsNothing(cboRegularity.Items.FindByValue(.Regularity)) Then cboRegularity.SelectedValue = .Regularity
                    If Not IsNothing(cboOpharnhood.Items.FindByValue(.Opharnhood)) Then cboOpharnhood.SelectedValue = .Opharnhood
                    If Not IsNothing(cboMajorSourceIncome.Items.FindByValue(.MajorSourceIncome)) Then cboMajorSourceIncome.SelectedValue = .MajorSourceIncome
                    If Not IsNothing(cboRelationships.Items.FindByValue(.RelationshipID)) Then cboRelationships.SelectedValue = .RelationshipID
                    txtContactNo.Text = .ContactNo
                    If Not IsNothing(cboCondition.Items.FindByValue(.Condition)) Then cboCondition.SelectedValue = .Condition
                    If Not IsNothing(cboAttendance.Items.FindByValue(.Attendance)) Then cboAttendance.SelectedValue = .Attendance
                    If Not IsNothing(cboDisability.Items.FindByValue(.Disability)) Then cboDisability.SelectedValue = .Disability
                    radDateofBirth.SelectedDate = .DateOfBirth
                    txtMemberNo.Text = .MemberNo
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    txtNationlIDNo.Text = .NationlIDNo

                    If .IsDependant <> 0 Then

                        cmdAddDependant.Text = "View Principal"
                        chkAddDependant.Checked = True

                    End If

                    ShowMessage("Beneficiary loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Beneficiary", MessageTypeEnum.Error)
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

            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID1.Text), txtBeneficiaryID1.Text, 0)
                .Suffix = IIf(txtSuffix.Text <> "", txtSuffix.Text, 0)
                If cboMaritalStatus.SelectedIndex > 0 Then .MaritalStatus = cboMaritalStatus.SelectedValue
                If cboHealthStatus.SelectedIndex > 0 Then .HealthStatus = cboHealthStatus.SelectedValue
                If cboDisabilityStatus.SelectedIndex > 0 Then .DisabilityStatus = cboDisabilityStatus.SelectedValue
                If cboLevelOfEducation.SelectedIndex > 0 Then .LevelOfEducation = cboLevelOfEducation.SelectedValue
                If cboRegularity.SelectedIndex > 0 Then .Regularity = cboRegularity.SelectedValue
                If cboOpharnhood.SelectedIndex > 0 Then .Opharnhood = cboOpharnhood.SelectedValue
                If cboMajorSourceIncome.SelectedIndex > 0 Then .MajorSourceIncome = cboMajorSourceIncome.SelectedValue
                If cboRelationships.SelectedIndex > 0 Then .RelationshipID = cboRelationships.SelectedValue
                .ContactNo = txtContactNo.Text
                If cboCondition.SelectedIndex > 0 Then .Condition = cboCondition.SelectedValue
                If cboAttendance.SelectedIndex > 0 Then .Attendance = cboAttendance.SelectedValue
                If cboDisability.SelectedIndex > 0 Then .Disability = cboDisability.SelectedValue
                If radDateofBirth.SelectedDate.HasValue Then .DateOfBirth = radDateofBirth.SelectedDate
                .MemberNo = txtMemberNo.Text
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .Sex = cboSex.SelectedValue
                .NationlIDNo = txtNationlIDNo.Text

                If Not String.IsNullOrEmpty(RTrim(LTrim(txtNationlIDNo.Text))) Then

                    If .CheckBenExistenceByNationalID() Then

                        ShowMessage("National ID: " & txtNationlIDNo.Text & " already exists in the system... Aborting operation!!!", MessageTypeEnum.Error)
                        Exit Function

                    End If

                End If

                If chkAddDependant.Checked = True Then

                    If IsNumeric(txtParentID.Text) AndAlso txtParentID.Text <> 0 Then

                        .ParentID = txtParentID.Text
                        .MemberNo = txtMemberNo.Text
                        .IsDependant = 1
                        .Suffix = IIf(IsNumeric(txtSuffix.Text) AndAlso txtSuffix.Text > 0, txtSuffix.Text, .GetNextSuffix())

                        If .Save Then


                            Dim NumberOfNewFields As Integer = UpdateCustomFieldTemplates(.BeneficiaryID)

                            If (phCustomFields.Controls.Count > 0) Then
                                objCustomFields.SaveCustomFields(DirectCast(phCustomFields.Controls(0), RadPanelBar), .BeneficiaryID, "HH")
                            End If

                            LoadBeneficiary(.BeneficiaryID)
                            LoadGrid(objBeneficiary)
                            ShowMessage("Dependant saved successfully...", MessageTypeEnum.Information)

                            Return True

                        Else

                            ShowMessage("Error: failed to save Dependant", MessageTypeEnum.Error)
                            Return False

                        End If

                    Else

                        ShowMessage("Please load head household details first before adding a a dependant...", MessageTypeEnum.Error)

                    End If

                Else

                    If .Save Then

                        Dim NumberOfNewFields As Integer = UpdateCustomFieldTemplates(.BeneficiaryID)

                        If (phCustomFields.Controls.Count > 0) Then
                            objCustomFields.SaveCustomFields(DirectCast(phCustomFields.Controls(0), RadPanelBar), .BeneficiaryID, "HH")
                        End If

                        If Not IsNumeric(txtBeneficiaryID1.Text) OrElse Trim(txtBeneficiaryID1.Text) = 0 Then txtBeneficiaryID1.Text = .BeneficiaryID

                        .MemberNo = IIf(Not IsNumeric(txtMemberNo.Text), .GenerateMemberNo(), txtMemberNo.Text)
                        .IsDependant = 0
                        .Suffix = IIf(Not IsNumeric(txtSuffix.Text), .GetNextSuffix(), txtSuffix.Text)

                        If .MemberNo <> "" AndAlso .Suffix <> 0 Then .Save()

                        LoadBeneficiary(.BeneficiaryID)
                        LoadGrid(objBeneficiary)

                        ShowMessage("Beneficiary saved successfully...", MessageTypeEnum.Information)

                        Return True

                    Else

                        ShowMessage("Error: failed to save Beneficiary", MessageTypeEnum.Error)
                        Return False

                    End If

                End If

                CookiesWrapper.BeneficiaryID = .BeneficiaryID
                CookiesWrapper.MemberNo = .MemberNo

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        CookiesWrapper.BeneficiaryID = 0
        txtBeneficiaryID1.Text = ""
        txtSuffix.Text = ""
        If Not IsNothing(cboMaritalStatus.Items.FindByValue("")) Then
            cboMaritalStatus.SelectedValue = ""
        ElseIf Not IsNothing(cboMaritalStatus.Items.FindByValue(0)) Then
            cboMaritalStatus.SelectedValue = 0
        Else
            cboMaritalStatus.SelectedIndex = -1
        End If
        If Not IsNothing(cboHealthStatus.Items.FindByValue("")) Then
            cboHealthStatus.SelectedValue = ""
        ElseIf Not IsNothing(cboHealthStatus.Items.FindByValue(0)) Then
            cboHealthStatus.SelectedValue = 0
        Else
            cboHealthStatus.SelectedIndex = -1
        End If
        If Not IsNothing(cboRelationships.Items.FindByValue("")) Then
            cboRelationships.SelectedValue = ""
        ElseIf Not IsNothing(cboRelationships.Items.FindByValue(0)) Then
            cboRelationships.SelectedValue = 0
        Else
            cboRelationships.SelectedIndex = -1
        End If
        If Not IsNothing(cboDisabilityStatus.Items.FindByValue("")) Then
            cboDisabilityStatus.SelectedValue = ""
        ElseIf Not IsNothing(cboDisabilityStatus.Items.FindByValue(0)) Then
            cboDisabilityStatus.SelectedValue = 0
        Else
            cboDisabilityStatus.SelectedIndex = -1
        End If
        If Not IsNothing(cboLevelOfEducation.Items.FindByValue("")) Then
            cboLevelOfEducation.SelectedValue = ""
        ElseIf Not IsNothing(cboLevelOfEducation.Items.FindByValue(0)) Then
            cboLevelOfEducation.SelectedValue = 0
        Else
            cboLevelOfEducation.SelectedIndex = -1
        End If
        If Not IsNothing(cboRegularity.Items.FindByValue("")) Then
            cboRegularity.SelectedValue = ""
        ElseIf Not IsNothing(cboRegularity.Items.FindByValue(0)) Then
            cboRegularity.SelectedValue = 0
        Else
            cboRegularity.SelectedIndex = -1
        End If
        If Not IsNothing(cboOpharnhood.Items.FindByValue("")) Then
            cboOpharnhood.SelectedValue = ""
        ElseIf Not IsNothing(cboRegularity.Items.FindByValue(0)) Then
            cboOpharnhood.SelectedValue = 0
        Else
            cboOpharnhood.SelectedIndex = -1
        End If
        If Not IsNothing(cboMajorSourceIncome.Items.FindByValue("")) Then
            cboMajorSourceIncome.SelectedValue = ""
        ElseIf Not IsNothing(cboMajorSourceIncome.Items.FindByValue(0)) Then
            cboMajorSourceIncome.SelectedValue = 0
        Else
            cboMajorSourceIncome.SelectedIndex = -1
        End If
        txtContactNo.Text = ""
        If Not IsNothing(cboCondition.Items.FindByValue("")) Then
            cboCondition.SelectedValue = ""
        ElseIf Not IsNothing(cboCondition.Items.FindByValue(0)) Then
            cboCondition.SelectedValue = 0
        Else
            cboCondition.SelectedIndex = -1
        End If
        If Not IsNothing(cboAttendance.Items.FindByValue("")) Then
            cboAttendance.SelectedValue = ""
        ElseIf Not IsNothing(cboAttendance.Items.FindByValue(0)) Then
            cboAttendance.SelectedValue = 0
        Else
            cboAttendance.SelectedIndex = -1
        End If
        If Not IsNothing(cboDisability.Items.FindByValue("")) Then
            cboDisability.SelectedValue = ""
        ElseIf Not IsNothing(cboDisability.Items.FindByValue(0)) Then
            cboDisability.SelectedValue = 0
        Else
            cboDisability.SelectedIndex = -1
        End If
        radDateofBirth.Clear()
        txtMemberNo.Text = ""
        txtFirstName.Text = ""
        txtSurname.Text = ""
        cboSex.SelectedValue = ""
        txtNationlIDNo.Text = ""
        txtParentID.Text = ""
        chkAddDependant.Checked = False

    End Sub

    Public Sub LoadCombo(ByVal cboCombo As DropDownList, ByVal Table As String, ByVal TextField As String, ByVal ValueField As String)

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboCombo

            .DataSource = objLookup.Lookup(Table, ValueField, TextField).Tables(0)
            .DataValueField = ValueField
            .DataTextField = TextField
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Private Sub LoadGrid(ByVal objBeneficiaries As BusinessLogic.Beneficiary)

        With radBenListing

            .DataSource = objBeneficiaries.GetBeneficiaryHousehold(CookiesWrapper.BeneficiaryID)
            .DataBind()

            ViewState("Beneficiaries") = .DataSource

        End With

    End Sub

    Private Sub radBenListing_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles radBenListing.ItemCommand

        If e.CommandName = "View" Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radBenListing.Items(index)
            Dim BeneficiaryID As Integer

            BeneficiaryID = Server.HtmlDecode(item("BeneficiaryID").Text)

            LoadBeneficiary(BeneficiaryID)

        End If

    End Sub

    Private Sub cmdAddDependant_Click(sender As Object, e As EventArgs) Handles cmdAddDependant.Click

        If cmdAddDependant.Text = "Add Dependant" Then

            If IsNumeric(txtBeneficiaryID1.Text) AndAlso txtSuffix.Text = 1 Then

                chkAddDependant.Checked = True

                txtParentID.Text = txtBeneficiaryID1.Text
                txtBeneficiaryID1.Text = ""
                txtSuffix.Text = ""
                If Not IsNothing(cboMaritalStatus.Items.FindByValue("")) Then
                    cboMaritalStatus.SelectedValue = ""
                ElseIf Not IsNothing(cboMaritalStatus.Items.FindByValue(0)) Then
                    cboMaritalStatus.SelectedValue = 0
                Else
                    cboMaritalStatus.SelectedIndex = -1
                End If
                If Not IsNothing(cboHealthStatus.Items.FindByValue("")) Then
                    cboHealthStatus.SelectedValue = ""
                ElseIf Not IsNothing(cboHealthStatus.Items.FindByValue(0)) Then
                    cboHealthStatus.SelectedValue = 0
                Else
                    cboHealthStatus.SelectedIndex = -1
                End If
                If Not IsNothing(cboDisabilityStatus.Items.FindByValue("")) Then
                    cboDisabilityStatus.SelectedValue = ""
                ElseIf Not IsNothing(cboDisabilityStatus.Items.FindByValue(0)) Then
                    cboDisabilityStatus.SelectedValue = 0
                Else
                    cboDisabilityStatus.SelectedIndex = -1
                End If
                If Not IsNothing(cboLevelOfEducation.Items.FindByValue("")) Then
                    cboLevelOfEducation.SelectedValue = ""
                ElseIf Not IsNothing(cboLevelOfEducation.Items.FindByValue(0)) Then
                    cboLevelOfEducation.SelectedValue = 0
                Else
                    cboLevelOfEducation.SelectedIndex = -1
                End If
                If Not IsNothing(cboRegularity.Items.FindByValue("")) Then
                    cboRegularity.SelectedValue = ""
                ElseIf Not IsNothing(cboRegularity.Items.FindByValue(0)) Then
                    cboRegularity.SelectedValue = 0
                Else
                    cboRegularity.SelectedIndex = -1
                End If
                If Not IsNothing(cboOpharnhood.Items.FindByValue("")) Then
                    cboOpharnhood.SelectedValue = ""
                ElseIf Not IsNothing(cboRegularity.Items.FindByValue(0)) Then
                    cboOpharnhood.SelectedValue = 0
                Else
                    cboOpharnhood.SelectedIndex = -1
                End If
                If Not IsNothing(cboMajorSourceIncome.Items.FindByValue("")) Then
                    cboMajorSourceIncome.SelectedValue = ""
                ElseIf Not IsNothing(cboMajorSourceIncome.Items.FindByValue(0)) Then
                    cboMajorSourceIncome.SelectedValue = 0
                Else
                    cboMajorSourceIncome.SelectedIndex = -1
                End If
                txtContactNo.Text = ""
                If Not IsNothing(cboCondition.Items.FindByValue("")) Then
                    cboCondition.SelectedValue = ""
                ElseIf Not IsNothing(cboCondition.Items.FindByValue(0)) Then
                    cboCondition.SelectedValue = 0
                Else
                    cboCondition.SelectedIndex = -1
                End If
                If Not IsNothing(cboAttendance.Items.FindByValue("")) Then
                    cboAttendance.SelectedValue = ""
                ElseIf Not IsNothing(cboAttendance.Items.FindByValue(0)) Then
                    cboAttendance.SelectedValue = 0
                Else
                    cboAttendance.SelectedIndex = -1
                End If
                If Not IsNothing(cboDisability.Items.FindByValue("")) Then
                    cboDisability.SelectedValue = ""
                ElseIf Not IsNothing(cboDisability.Items.FindByValue(0)) Then
                    cboDisability.SelectedValue = 0
                Else
                    cboDisability.SelectedIndex = -1
                End If
                radDateofBirth.Clear()
                txtFirstName.Text = ""
                txtSurname.Text = ""
                cboSex.SelectedValue = ""
                txtNationlIDNo.Text = ""

                cmdAddDependant.Text = "View Principal"

            Else

                ShowMessage("Please save household principal before attempting to save a dependant!", MessageTypeEnum.Error)


            End If

        Else

            Response.Redirect("~/Beneficiary.aspx?id=" & objUrlEncoder.Encrypt(txtParentID.Text))

        End If

    End Sub

    Private Sub BeneficiaryDetailsControl_Init(sender As Object, e As EventArgs) Handles Me.Init

        Dim objHealth As New HealthCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If Not IsNothing(Request.QueryString("id")) Then

            Dim BeneficiaryID As Long = objUrlEncoder.Decrypt(Request.QueryString("id"))

            phCustomFields.Controls.Add(objCustomFields.LoadStatusTemplateCustomFieldsPanel(Page, BeneficiaryID, "HH", My.Settings.DisplayDateFormat, objHealth.GetCustomFieldsObjectIDByCode("HH"), "HH", True, False))

        Else

            phCustomFields.Controls.Add(objCustomFields.LoadStatusTemplateCustomFieldsPanel(Page, 0, "HH", My.Settings.DisplayDateFormat, objHealth.GetCustomFieldsObjectIDByCode("HH"), "HH", True, False))

        End If

    End Sub

    Public Function UpdateCustomFieldTemplates(ByVal HouseholdID As Long) As Integer

        'Creating a project: 
        '   - Now we know the type, and/or the status, so we need to 
        '     load the relevant custom fields
        'Updating a project: 
        '   - We check if the type/status has changed, and if so, we 
        '     need to load the relevant custom fields for this new 
        '     project type/status.

        Dim NewStatusTemplates As Long = 0, NewTypeTemplates As Long = 0

        If HouseholdID > 0 Then

            Dim objCustomFields As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

            objCustomFields.UpdateObjectWithStatusTemplates(HouseholdID, "HH", HouseholdID, BusinessLogic.CustomFields.AutomatorTypes.HealthCenter, RowsAffected:=NewStatusTemplates)

        End If

        Return NewStatusTemplates + NewTypeTemplates

    End Function
End Class



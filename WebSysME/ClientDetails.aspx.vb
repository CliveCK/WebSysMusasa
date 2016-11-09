Imports BusinessLogic
Imports Telerik.Web.UI

Public Class ClientDetails
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

    Private ReadOnly Property CapturedFrom() As Long

        Get

            Dim result As Integer = -1

            If Not IsNothing(Request.QueryString("tag")) Then

                Select Case Request.QueryString("tag")

                    Case "recp"
                        result = 1

                    Case "couns"
                        result = 2

                    Case Else
                        result = -1
                End Select

            End If

            Return result
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            LoadControls()

            If CapturedFrom = 1 Then

                cboLawyer.Enabled = False
                cboShelter.Enabled = False

            ElseIf CapturedFrom = 2 Then

                cboCounsellor.Enabled = False

            End If

            If Not IsNothing(Request.QueryString("id")) Then

                LoadClientDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If
    End Sub

    Public Sub LoadControls()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboMarriageType

            .DataSource = objLookup.Lookup("luMaritalStatus", "ObjectID", "Description").Tables(0)
            .DataValueField = "ObjectID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

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


        With cboLawyer

            .DataSource = objLookup.GetStaffByType("Lawyer").Tables(0)
            .DataValueField = "StaffID"
            .DataTextField = "StaffFullName"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboCounsellor

            .DataSource = objLookup.GetStaffByType("Counsellor").Tables(0)
            .DataValueField = "StaffID"
            .DataTextField = "StaffFullName"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        With cboShelter

            .DataSource = objLookup.GetStaffByType("Shelter").Tables(0)
            .DataValueField = "StaffID"
            .DataTextField = "StaffFullName"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, 0))
            .SelectedIndex = 0

        End With

        radDtDOB.MaxDate = Now

    End Sub

    Public Function LoadClientDetails(ByVal BeneficiaryID As Long) As Boolean

        Try

            Dim objClientDetails As New BusinessLogic.ClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objBeneficiary

                If .RetrieveWithAddress(BeneficiaryID) Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    txtFirstName.Text = .FirstName
                    txtSurname.Text = .Surname
                    If Not .DateOfBirth = "" Then radDtDOB.SelectedDate = .DateOfBirth
                    If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                    If Not IsNothing(cboMarriageType.Items.FindByValue(.MaritalStatus)) Then cboMarriageType.SelectedValue = .MaritalStatus
                    If Not IsNothing(cboLevelOfEducation.Items.FindByValue(.LevelOfEducation)) Then cboLevelOfEducation.SelectedValue = .LevelOfEducation
                    If Not IsNothing(cboDistricts.Items.FindByValue(.DistrictID)) Then cboDistricts.SelectedValue = .DistrictID
                    If Not IsNothing(cboWards.Items.FindByValue(.WardID)) Then cboWards.SelectedValue = .WardID
                    txtNationalIDNum.Text = .NationlIDNo
                    txtPhoneNum.Text = .ContactNo

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

                    txtClientDetailID.Text = .ClientDetailID
                    txtNumOfChildren.Text = .NoOfChildren
                    If Not IsNothing(cboEmploymentStatus.Items.FindByValue(.EmploymentStatusID)) Then cboEmploymentStatus.SelectedValue = .EmploymentStatusID
                    txtAccompanyingChn.Text = .AccompanyingChildren
                    If Not IsNothing(cboCounsellor.Items.FindByValue(.CounsellorID)) Then cboCounsellor.SelectedValue = .CounsellorID
                    If Not IsNothing(cboLawyer.Items.FindByValue(.LawyerID)) Then cboLawyer.SelectedValue = .LawyerID
                    If Not IsNothing(cboShelter.Items.FindByValue(.ShelterID)) Then cboShelter.SelectedValue = .ShelterID
                    txtNextOfKin.Text = .NextOfKin
                    txtNxtOfKinConNum.Text = .ContactNo
                    txtNatureOfRelationShip.Text = .NatureOfRelationship
                    If Not IsNothing(cboAccompanynAdult1.Items.FindByValue(.AccompanyingAdult1)) Then cboAccompanynAdult1.SelectedValue = .AccompanyingAdult1
                    If Not IsNothing(cboAccompanynAdult2.Items.FindByValue(.AccompanyingAdult2)) Then cboAccompanynAdult1.SelectedValue = .AccompanyingAdult2
                    cboAccompanynAdult2.Text = .AccompanyingAdult2
                    txtReferredBy.Text = .ReferredBy

                    LoadGrid(.BeneficiaryID)

                    ShowMessage("ClientDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    'ShowMessage("Failed to loadClientDetails: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
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

    Public Function Save() As Boolean

        Try

            Dim objClientDetails As New BusinessLogic.ClientDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim update As Boolean = False

            'First save the beneficiary details - We will need the BeneficiaryID for the Client details

            With objBeneficiary

                If Not IsNumeric(txtBeneficiaryID.Text) Then

                    Dim ds As DataSet = .RetriveRecordByID(txtNationalIDNum.Text)

                    If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                        LoadClientDetails(ds.Tables(0).Rows(0)("BeneficiaryID"))
                        ShowMessage("This person already exists in the system. The details have been loaded...", MessageTypeEnum.Warning)
                        Exit Function

                    End If

                End If

                .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
                .FirstName = txtFirstName.Text
                .Surname = txtSurname.Text
                .NationlIDNo = txtNationalIDNum.Text
                .Sex = cboSex.SelectedValue
                If radDtDOB.SelectedDate.HasValue Then .DateOfBirth = radDtDOB.SelectedDate
                .Suffix = 1
                .ContactNo = txtPhoneNum.Text
                If cboLevelOfEducation.SelectedIndex > 0 Then .LevelOfEducation = cboLevelOfEducation.SelectedValue
                If cboMarriageType.SelectedIndex > 0 Then .MaritalStatus = cboMarriageType.SelectedValue

                update = .BeneficiaryID > 0

                If .Save Then

                    txtBeneficiaryID.Text = .BeneficiaryID
                    If update = False Then .MemberNo = .GenerateMemberNo

                    If Not IsNumeric(txtBeneficiaryID.Text) OrElse Trim(txtBeneficiaryID.Text) = 0 Then txtBeneficiaryID.Text = .BeneficiaryID
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

            With objClientDetails

                .ClientDetailID = IIf(IsNumeric(txtClientDetailID.Text), txtClientDetailID.Text, 0)
                If IsNumeric(txtBeneficiaryID.Text) Then
                    .BeneficiaryID = txtBeneficiaryID.Text
                Else
                    ShowMessage("Missing beneficiary Information", MessageTypeEnum.Error)
                    Exit Function
                End If
                If IsNumeric(txtNumOfChildren.Text) Then .NoOfChildren = txtNumOfChildren.Text
                .EmploymentStatusID = cboEmploymentStatus.SelectedValue
                If IsNumeric(txtAccompanyingChn.Text) Then .AccompanyingChildren = txtAccompanyingChn.Text
                If cboCounsellor.SelectedIndex > 0 Then .CounsellorID = cboCounsellor.SelectedValue
                If cboLawyer.SelectedIndex > 0 Then .LawyerID = cboLawyer.SelectedValue
                If cboShelter.SelectedIndex > 0 Then .ShelterID = cboShelter.SelectedValue
                .CapturedFromID = CapturedFrom
                .ReferredToCounsellor = cboCounsellor.SelectedIndex > 0
                If cboCounsellor.SelectedIndex > 0 Then .ReferredToCounsellorByID = CookiesWrapper.StaffID
                .ReferredToLaywer = cboLawyer.SelectedIndex > 0
                If cboLawyer.SelectedIndex > 0 Then .ReferredToLawyerByID = CookiesWrapper.StaffID
                .ReferredToShelter = cboShelter.SelectedIndex > 0
                If cboShelter.SelectedIndex > 0 Then .ReferredToShelterByID = CookiesWrapper.StaffID
                .NextOfKin = txtNextOfKin.Text
                .ContactNo = txtNxtOfKinConNum.Text
                .NatureOfRelationship = txtNatureOfRelationShip.Text
                .AccompanyingAdult1 = cboAccompanynAdult1.SelectedValue
                .AccompanyingAdult2 = cboAccompanynAdult2.SelectedValue
                .ReferredBy = txtReferredBy.Text

                If CapturedFrom = 2 Then

                    .SyncReferredBy(.BeneficiaryID, IIf(cboLawyer.SelectedIndex > 0, cboLawyer.SelectedValue, -1), IIf(cboShelter.SelectedIndex > 0, cboShelter.SelectedValue, -1))

                End If

                If .Save Then

                    If Not IsNumeric(txtClientDetailID.Text) OrElse Trim(txtClientDetailID.Text) = 0 Then txtClientDetailID.Text = .ClientDetailID
                    ShowMessage("ClientDetails saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Details failed to Save...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub LoadGrid(ByVal BeneficiaryID As Long)

        Dim objAccChildren As New BusinessLogic.AccompanyingChildren(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radAccChildren

            .DataSource = objAccChildren.GetAccompanyingChildren(BeneficiaryID)
            .DataBind()

            ViewState("Acc") = .DataSource

        End With

    End Sub

    Public Sub Clear()

        If Not IsNothing(cboAccompanynAdult1.Items.FindByValue("")) Then
            cboAccompanynAdult1.SelectedValue = ""
        ElseIf Not IsNothing(cboAccompanynAdult1.Items.FindByValue(0)) Then
            cboAccompanynAdult1.SelectedValue = 0
        Else
            cboAccompanynAdult1.SelectedIndex = -1
        End If
        If Not IsNothing(cboAccompanynAdult2.Items.FindByValue("")) Then
            cboAccompanynAdult2.SelectedValue = ""
        ElseIf Not IsNothing(cboAccompanynAdult2.Items.FindByValue(0)) Then
            cboAccompanynAdult2.SelectedValue = 0
        Else
            cboAccompanynAdult2.SelectedIndex = -1
        End If
        txtNumOfChildren.Text = 0
        If Not IsNothing(cboEmploymentStatus.Items.FindByValue("")) Then
            cboEmploymentStatus.SelectedValue = ""
        ElseIf Not IsNothing(cboEmploymentStatus.Items.FindByValue(0)) Then
            cboEmploymentStatus.SelectedValue = 0
        Else
            cboEmploymentStatus.SelectedIndex = -1
        End If
        txtAccompanyingChn.Text = 0
        If Not IsNothing(cboCounsellor.Items.FindByValue("")) Then
            cboCounsellor.SelectedValue = ""
        ElseIf Not IsNothing(cboCounsellor.Items.FindByValue(0)) Then
            cboCounsellor.SelectedValue = 0
        Else
            cboCounsellor.SelectedIndex = -1
        End If
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
        txtNextOfKin.Text = ""
        txtNxtOfKinConNum.Text = ""
        txtNatureOfRelationShip.Text = ""
        txtClientDetailID.Text = ""
        txtReferredBy.Text = ""

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub radAccChildren_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radAccChildren.NeedDataSource

        radAccChildren.DataSource = DirectCast(ViewState("Acc"), DataSet)

    End Sub

    Private Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click

        If IsNumeric(txtBeneficiaryID.Text) AndAlso txtBeneficiaryID.Text > 0 Then

            If Not IsNumeric(txtAccompanyingChn.Text) OrElse txtAccompanyingChn.Text = 0 Then

                ShowMessage("Please specify number of Children greater than 0 to add details", MessageTypeEnum.Error)
                Exit Sub

            End If

            Dim rows As Int16 = 0
            Dim objAccChildren As New BusinessLogic.AccompanyingChildren(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
            Dim ds As DataSet = objAccChildren.GetAccompanyingChildren(txtBeneficiaryID.Text)

            If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                rows = ds.Tables(0).Rows.Count

            End If

            If rows >= txtAccompanyingChn.Text Then

                ShowMessage("Number of children specified has been reached", MessageTypeEnum.Error)
                Exit Sub

            End If

            If IsNumeric(txtAge.Text) Then

                    With objAccChildren

                        .BeneficiaryID = txtBeneficiaryID.Text
                        .Firstname = txtAccFirstName.Text
                        .Surname = txtAccSurname.Text
                        .Sex = cboSexA.SelectedValue
                        .Age = txtAge.Text

                        If .Save Then

                            LoadGrid(.BeneficiaryID)
                            ShowMessage("Entry added successfully...", MessageTypeEnum.Information)

                        End If

                    End With

                Else

                    txtAge.BorderColor = Drawing.Color.Red
                    txtAge.Focus()
                    ShowMessage("Age must be a number", MessageTypeEnum.Error)

                End If


            End If

    End Sub

    Private Sub radAccChildren_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radAccChildren.ItemCommand

        If TypeOf e.Item Is GridDataItem Then

            Dim index As Integer = Convert.ToInt32(e.Item.ItemIndex.ToString)
            Dim item As GridDataItem = radAccChildren.Items(index)

            Select Case e.CommandName

                Case "Delete"

                    Dim objAccChildren As New BusinessLogic.AccompanyingChildren(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

                    With objAccChildren

                        .AccompanyingChildrenID = Server.HtmlDecode(e.CommandArgument)

                        If .Delete() Then

                            LoadGrid(IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0))
                            ShowMessage("Entry deleted successfully...", MessageTypeEnum.Information)

                        End If

                    End With

            End Select

        End If

    End Sub
End Class
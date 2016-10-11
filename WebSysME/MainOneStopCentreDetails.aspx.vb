Imports Telerik.Web.UI

Public Class MainOneStopCentreDetails
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

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboClub

                .DataSource = objLookup.GetShelter
                .DataValueField = "SubOfficeID"
                .DataTextField = "Name"
                .DataBind()

                ViewState("vOneStop") = .DataSource

            End With


            With cboTypesofViolence

                .DataSource = objLookup.Lookup("luTypesOfViolence", "TypesOFViolenceID", "Description").Tables(0)
                .DataValueField = "TypesOFViolenceID"
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

            With cboReferredfrom

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

            With cboMonth

                .DataSource = objLookup.Lookup("luMonths", "MonthID", "Description").Tables(0)
                .DataValueField = "MonthID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboProvince

                .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name").Tables(0)
                .DataValueField = "ProvinceID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name").Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name").Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadDetails(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Private Sub cboProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProvince.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProvince.SelectedIndex > -1 Then

            With cboDistrict

                .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name", , "ProvinceID = " & cboProvince.SelectedValue).Tables(0)
                .DataValueField = "DistrictID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDistrict.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboDistrict.SelectedIndex > -1 Then

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

    Protected Sub lnkOneStopCentreList_Click(sender As Object, e As EventArgs) Handles lnkOneStopCentreList.Click
        Response.Redirect("~/MainOneStopCentreListing.aspx")
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub Save()

        Dim objMainOneStopCenter As New BusinessLogic.MainOneStopCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objOneStopCenterMember As New BusinessLogic.MainOneStopCenterMember(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim update As Boolean = False

        With objMainOneStopCenter

            .MainOneStopCenterID = IIf(IsNumeric(txtMainOneStopCenter.Text), txtMainOneStopCenter.Text, 0)
            If cboClub.SelectedIndex > -1 Then .CenterNameID = cboClub.SelectedValue
            .Year = txtYear.Text
            If cboMonth.SelectedIndex > -1 Then .Month = cboMonth.SelectedValue

            If .MainOneStopCenterID > 0 Then update = True

            If .Save Then

                txtMainOneStopCenter.Text = .MainOneStopCenterID

                With objBeneficiary

                    .BeneficiaryID = IIf(IsNumeric(txtBeneficiaryID.Text), txtBeneficiaryID.Text, 0)
                    .FirstName = txtFirstName.Text
                    .Surname = txtSurname.Text
                    If radDate.SelectedDate.HasValue Then .DateOfBirth = radDate.SelectedDate
                    .NationlIDNo = txtIDNumber.Text
                    .Sex = cboSex.SelectedValue

                    If .Save Then

                        If Not IsNumeric(txtBeneficiaryID.Text) OrElse Trim(txtBeneficiaryID.Text) = 0 Then txtBeneficiaryID.Text = .BeneficiaryID

                        With objAddress

                            .AddressID = IIf(IsNumeric(txtAddressID.Text), txtAddressID.Text, 0)
                            .OwnerID = objBeneficiary.BeneficiaryID
                            If cboDistrict.SelectedIndex > -1 Then .DistrictID = cboDistrict.SelectedValue
                            If cboWard.SelectedIndex > -1 Then .WardID = cboWard.SelectedValue

                            If .Save() Then

                                If Not IsNumeric(txtAddressID.Text) OrElse Trim(txtAddressID.Text) = 0 Then txtAddressID.Text = .AddressID

                            End If

                        End With

                        With objOneStopCenterMember

                            .BeneficiaryID = objBeneficiary.BeneficiaryID
                            .MainOneStopCenterID = objMainOneStopCenter.MainOneStopCenterID

                            If Not update Then

                                .Save()

                            End If

                            ShowMessage("Details saved successfully...", MessageTypeEnum.Information)

                        End With

                    End If

                End With

            End If

        End With

    End Sub

    Private Sub LoadDetails(ByVal MainOneStopCenterDetailID As Long)

        Dim objMainOneStopCenter As New BusinessLogic.MainOneStopCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objMainOneStopCenterNeedsServices As New BusinessLogic.OneStopCenterMemberNeedsServices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objBeneficiary As New BusinessLogic.Beneficiary(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objOneStopCenterMember As New BusinessLogic.MainOneStopCenterMember(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)
        Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objMainOneStopCenter

            If .Retrieve(MainOneStopCenterDetailID) Then

                txtMainOneStopCenter.Text = .MainOneStopCenterID
                If Not IsNothing(cboClub.Items.FindByValue(.CenterNameID)) Then cboClub.SelectedValue = .CenterNameID
                txtYear.Text = .Year
                If Not IsNothing(cboMonth.Items.FindByValue(.Month)) Then cboMonth.SelectedValue = .Month

                If objOneStopCenterMember.Retrieve(.MainOneStopCenterID) Then

                    With objBeneficiary

                        If .RetrieveWithAddress(objOneStopCenterMember.BeneficiaryID) Then

                            txtBeneficiaryID.Text = .BeneficiaryID
                            txtFirstName.Text = .FirstName
                            txtSurname.Text = .Surname
                            If Not .DateOfBirth = "" Then radDate.SelectedDate = .DateOfBirth
                            txtIDNumber.Text = .NationlIDNo
                            If Not IsNothing(cboSex.Items.FindByValue(.Sex)) Then cboSex.SelectedValue = .Sex
                            If Not IsNothing(cboProvince.Items.FindByValue(.ProvinceID)) Then cboProvince.SelectedValue = .ProvinceID
                            If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                            If Not IsNothing(cboWard.Items.FindByValue(.WardID)) Then cboWard.SelectedValue = .WardID

                            If objAddress.Retrieve(.BeneficiaryID) Then

                                txtAddressID.Text = objAddress.AddressID

                            End If

                        End If


                    End With

                End If

                LoadGrid(objMainOneStopCenter.MainOneStopCenterID, objMainOneStopCenterNeedsServices)

            End If

        End With

    End Sub

    Private Sub LoadGrid(ByVal MainOneStopCenterID As Long, ByVal objMainOneStopCenterNeedsServices As BusinessLogic.OneStopCenterMemberNeedsServices)

        With radBenListing

            .DataSource = objMainOneStopCenterNeedsServices.GetAllOneStopCenterMemberNeedsServices(MainOneStopCenterID)
            .DataBind()

            ViewState("vNeeds") = .DataSource

        End With

    End Sub

    Private Sub cmdAddNeed_Click(sender As Object, e As EventArgs) Handles cmdAddNeed.Click

        Dim objMainOneStopCenterNeedsServices As New BusinessLogic.OneStopCenterMemberNeedsServices(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If IsNumeric(txtMainOneStopCenter.Text) AndAlso txtMainOneStopCenter.Text > 0 Then

            With objMainOneStopCenterNeedsServices

                .OneStopCenterMemberNeedsServices = 0
                If cboTypesofViolence.SelectedIndex > 0 Then .TypeOfViolenceID = cboTypesofViolence.SelectedValue
                .MainOneStopCenterID = txtMainOneStopCenter.Text
                If cboReferredfrom.SelectedIndex > 0 Then .ReferredFromID = cboReferredfrom.SelectedValue
                If cboReferredTo.SelectedIndex > 0 Then .ReferredToID = cboReferredTo.SelectedValue
                If cboAssistance.SelectedIndex > 0 Then .AssistanceID = cboAssistance.SelectedValue
                .Comments = txtComments.Text

                If .Save() Then

                    LoadGrid(.MainOneStopCenterID, objMainOneStopCenterNeedsServices)
                    ShowMessage("Details saved successfuly...", MessageTypeEnum.Information)

                End If

            End With

        Else

            ShowMessage("Failed to save details...", MessageTypeEnum.Error)

        End If
    End Sub

    Private Sub radBenListing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radBenListing.NeedDataSource

        radBenListing.DataSource = DirectCast(ViewState("vNeeds"), DataSet)

    End Sub
End Class
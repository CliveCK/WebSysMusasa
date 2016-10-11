Imports BusinessLogic

    Partial Class CommunityDetailsControl
    Inherits System.Web.UI.UserControl

    Private objUrlEncoder As New Security.SpecialEncryptionServices.UrlServices.EncryptDecryptQueryString

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

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboProvince

                .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name").Tables(0)
                .DataValueField = "ProvinceID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboThermaticArea

                .DataSource = objLookup.Lookup("luThermaticArea", "ThermaticAreaID", "Description").Tables(0)
                .DataValueField = "ThermaticAreaID"
                .DataTextField = "Description"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            With cboIndicator

                .DataSource = objLookup.Lookup("tblIndicators", "IndicatorID", "Name").Tables(0)
                .DataValueField = "IndicatorID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

            If Not IsNothing(Request.QueryString("id")) Then

                LoadCommunity(objUrlEncoder.Decrypt(Request.QueryString("id")))
                LoadGrid(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

            End If

        End Sub

        Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

            Save()

        End Sub

        Public Function LoadCommunity(ByVal CommunityID As Long) As Boolean

            Try

                Dim objCommunity As New Community(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCommunity

                If .Retrieve(CommunityID) Then

                    Dim objLookup As New BusinessLogic.CommonFunctions

                    With cboWard

                        .DataSource = objLookup.Lookup("tblWards", "WardID", "Name").Tables(0)
                        .DataValueField = "WardID"
                        .DataTextField = "Name"
                        .DataBind()

                        .Items.Insert(0, New ListItem(String.Empty, String.Empty))
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

                    txtCommunityID.Text = .CommunityID
                    If Not IsNothing(cboProvince.Items.FindByValue(.ProvinceID)) Then cboProvince.SelectedValue = .ProvinceID
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    If Not IsNothing(cboWard.Items.FindByValue(.WardID)) Then cboWard.SelectedValue = .WardID
                    txtNoOfHouseholds.Text = .NoOfHouseholds
                    txtNoOfIndividualAdultMales.Text = .NoOfIndividualAdultMales
                    txtNoOfIndividualAdultFemales.Text = .NoOfIndividualAdultFemales
                    txtNoOfMaleYouths.Text = .NoOfMaleYouths
                    txtNoOfFemaleYouth.Text = .NoOfFemaleYouth
                    txtNoOfChildren.Text = .NoOfChildren
                    txtName.Text = .Name
                    txtDescription.Text = .Description

                    ShowMessage("Community loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Community: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objCommunity As New Community(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCommunity

                .CommunityID = IIf(txtCommunityID.Text <> "", txtCommunityID.Text, 0)
                If cboWard.SelectedIndex > -1 Then .WardID = cboWard.SelectedValue
                .NoOfHouseholds = IIf(IsNumeric(txtNoOfHouseholds.Text), txtNoOfHouseholds.Text, 0)
                .NoOfIndividualAdultMales = IIf(IsNumeric(txtNoOfIndividualAdultMales.Text), txtNoOfIndividualAdultMales.Text, 0)
                .NoOfIndividualAdultFemales = IIf(IsNumeric(txtNoOfIndividualAdultFemales.Text), txtNoOfIndividualAdultFemales.Text, 0)
                .NoOfMaleYouths = IIf(IsNumeric(txtNoOfMaleYouths.Text), txtNoOfMaleYouths.Text, 0)
                .NoOfFemaleYouth = IIf(IsNumeric(txtNoOfFemaleYouth.Text), txtNoOfFemaleYouth.Text, 0)
                .NoOfChildren = IIf(IsNumeric(txtNoOfChildren.Text), txtNoOfChildren.Text, 0)
                .Name = txtName.Text
                .Description = txtDescription.Text

                If .Save Then

                    If Not IsNumeric(txtCommunityID.Text) OrElse Trim(txtCommunityID.Text) = 0 Then txtCommunityID.Text = .CommunityID
                    ShowMessage("Community saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error: Failed to save community", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

        Public Sub Clear()

            txtCommunityID.Text = ""
            If Not IsNothing(cboWard.Items.FindByValue("")) Then
                cboWard.SelectedValue = ""
            ElseIf Not IsNothing(cboWard.Items.FindByValue(0)) Then
                cboWard.SelectedValue = 0
            Else
                cboWard.SelectedIndex = -1
            End If
            txtNoOfHouseholds.Text = 0
            txtNoOfIndividualAdultMales.Text = 0
            txtNoOfIndividualAdultFemales.Text = 0
            txtNoOfMaleYouths.Text = 0
            txtNoOfFemaleYouth.Text = 0
            txtNoOfChildren.Text = 0
            txtName.Text = ""
            txtDescription.Text = ""

        End Sub

        Private Sub cboProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProvince.SelectedIndexChanged

            Dim objLookup As New BusinessLogic.CommonFunctions

            If cboProvince.SelectedIndex > 0 Then

                With cboDistrict

                    .DataSource = objLookup.Lookup("tblDistricts", "DistrictID", "Name", , "ProvinceID = " & cboProvince.SelectedValue).Tables(0)
                    .DataValueField = "DistrictID"
                    .DataTextField = "Name"
                    .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

                End With

            End If

        End Sub

        Private Sub cboDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDistrict.SelectedIndexChanged

            Dim objLookup As New BusinessLogic.CommonFunctions

            If cboDistrict.SelectedIndex > 0 Then

                With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name", , "DistrictID = " & cboDistrict.SelectedValue).Tables(0)
                    .DataValueField = "WardID"
                    .DataTextField = "Name"
                    .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

                End With

            End If

    End Sub

    Private Sub LoadGrid(ByVal CommunityID As Long)

        Dim objCommunityScore As New CommunityScoreCard(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With radCommunityScoreListing

            .DataSource = objCommunityScore.GetCommunityScoreCard(CommunityID).Tables(0)
            .DataBind()

            ViewState("CommunityScore") = .DataSource

        End With

    End Sub

    Private Sub cmdPlus_Click(sender As Object, e As EventArgs) Handles cmdPlus.Click

        If IsNumeric(txtCommunityID.Text) Then

            Dim objMaturityIndex As New CommunityScoreCard(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objMaturityIndex

                .CommunityID = txtCommunityID.Text
                .ThermaticAreaID = cboThermaticArea.SelectedValue
                .IndicatorID = cboIndicator.SelectedValue
                .Score = txtScore.Text
                If radDate.SelectedDate.HasValue Then .Date1 = radDate.SelectedDate

                If .Save Then

                    LoadGrid(.CommunityID)
                    ShowMessage("Entry Saved successfully", MessageTypeEnum.Information)

                Else

                    ShowMessage("Some details have not been saved", MessageTypeEnum.Error)

                End If

            End With

        End If

    End Sub

    Private Sub radCommunityScoreListing_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radCommunityScoreListing.NeedDataSource

        radCommunityScoreListing.DataSource = DirectCast(ViewState("CommunityScore"), DataTable)

    End Sub
End Class
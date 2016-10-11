Imports BusinessLogic

Public Class HouseHoldAFWS
    Inherits System.Web.UI.UserControl

    Private ReadOnly Property HouseholdID As Long
        Get
            Dim txtHouseholdID As TextBox = DirectCast(Parent.Parent.FindControl("ucBeneficiaryControl").FindControl("txtBeneficiaryID1"), TextBox)

            Return IIf(IsNumeric(txtHouseholdID.Text), txtHouseholdID.Text, 0)
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

            InitializeComponents()

            If CookiesWrapper.BeneficiaryID > 0 Then

                LoadGrid()

            End If

        End If

    End Sub

    Private Sub LoadGrid()

        Dim objAsset As New HouseHoldAssets(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        ViewState("Assets") = Nothing

        With radAsset

            .DataSource = objAsset.GetAssets(CookiesWrapper.BeneficiaryID)
            .DataBind()

            ViewState("Assets") = .DataSource

        End With

    End Sub
    Private Sub InitializeComponents()

        LoadCombo(cboMajorSourceOfFood, "tblSourceOfFood", "Description", "SourceOfFoodID")
        LoadCombo(cboConditionOfHouse, "luConditionOfHouse", "Description", "ConditionOfHouseID")
        LoadCombo(cboTenure, "luTenure", "Description", "TenureID")
        LoadCombo(cboWealthRank, "luWealthRanks", "Description", "WealthRankID")
        LoadCombo(cboSourceOfWater, "luSourceOfWater", "Description", "SourceOfWaterID")
        LoadCombo(cboTypeOfToilet, "luTypesOfToilet", "Description", "TypeOfToiletID")
        LoadCombo(cboAsset, "tblAssets", "Description", "AssetID")
        LoadCombo(cboAssetType, "luAssetTypes", "Description", "AssetTypeID")

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

    Protected Sub cmdSaveA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveA.Click

        If CookiesWrapper.BeneficiaryID > 0 Then

            SaveA()

        Else

            ShowMessage("Please save Individual/Household details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Public Function LoadHouseHoldAssetsDetails(ByVal HouseholdAssetDetailID As Long) As Boolean

        Try

            Dim objHouseHoldAssetsDetails As New HouseHoldAssetsDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHouseHoldAssetsDetails

                If .Retrieve(HouseholdAssetDetailID) Then

                    txtHouseholdDetailID.Text = .HouseholdAssetDetailID
                    txtAriableLandSize.Text = .AriableLandSize
                    If Not IsNothing(cboMajorSourceOfFood.Items.FindByValue(.MajorSourceOfFoodID)) Then cboMajorSourceOfFood.SelectedValue = .MajorSourceOfFoodID
                    If Not IsNothing(cboConditionOfHouse.Items.FindByValue(.ConditionOfHouseID)) Then cboConditionOfHouse.SelectedValue = .ConditionOfHouseID
                    If Not IsNothing(cboTenure.Items.FindByValue(.TenureID)) Then cboTenure.SelectedValue = .TenureID
                    If Not IsNothing(cboWealthRank.Items.FindByValue(.WealthRankID)) Then cboWealthRank.SelectedValue = .WealthRankID
                    If Not IsNothing(cboSourceOfWater.Items.FindByValue(.SourceOfWaterID)) Then cboSourceOfWater.SelectedValue = .SourceOfWaterID
                    If Not IsNothing(cboTypeOfToilet.Items.FindByValue(.TypeOfToiletID)) Then cboTypeOfToilet.SelectedValue = .TypeOfToiletID
                    txtHealthCareProvider.Text = .HealthCareProvider
                    txtHouseholdNo.Text = .HouseholdID
                    txtRoomOccupationRatio.Text = .RoomOccupationRatio

                    ShowMessage("HouseHoldAssetsDetails loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load HouseHoldAssetsDetails: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function SaveA() As Boolean

        Try

            Dim objHouseHoldAssetsDetails As New HouseHoldAssetsDetails(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHouseHoldAssetsDetails

                .HouseholdAssetDetailID = IIf(IsNumeric(txtHouseholdDetailID.Text), txtHouseholdDetailID.Text, 0)
                .AriableLandSize = txtAriableLandSize.Text
                If cboMajorSourceOfFood.SelectedIndex > -1 Then .MajorSourceOfFoodID = cboMajorSourceOfFood.SelectedValue
                If cboConditionOfHouse.SelectedIndex > -1 Then .ConditionOfHouseID = cboConditionOfHouse.SelectedValue
                If cboTenure.SelectedIndex > -1 Then .TenureID = cboTenure.SelectedValue
                If cboWealthRank.SelectedIndex > -1 Then .WealthRankID = cboWealthRank.SelectedValue
                If cboSourceOfWater.SelectedIndex > -1 Then .SourceOfWaterID = cboSourceOfWater.SelectedValue
                If cboTypeOfToilet.SelectedIndex > -1 Then .TypeOfToiletID = cboTypeOfToilet.SelectedValue
                .HealthCareProvider = txtHealthCareProvider.Text
                .HouseholdID = CookiesWrapper.BeneficiaryID
                .RoomOccupationRatio = txtRoomOccupationRatio.Text

                If .Save Then

                    If Not IsNumeric(txtHouseholdDetailID.Text) OrElse Trim(txtHouseholdDetailID.Text) = 0 Then txtHouseholdDetailID.Text = .HouseholdAssetDetailID
                    ShowMessage("HouseHoldAssetsDetails saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save household details", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function LoadHouseHoldAssets(ByVal HouseholdAssetID As Long) As Boolean

        Try

            Dim objHouseHoldAssets As New HouseHoldAssets(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHouseHoldAssets

                If .Retrieve(HouseholdAssetID) Then

                    txtHouseholdAssetID.Text = .HouseholdAssetID
                    If Not IsNothing(cboAssetType.Items.FindByValue(.AssetTypeID)) Then cboAssetType.SelectedValue = .AssetTypeID
                    If Not IsNothing(cboAsset.Items.FindByValue(.AssetID)) Then cboAsset.SelectedValue = .AssetID
                    txtQuantity.Text = .Quantity
                    txtHouseholdNo.Text = .HouseholdNo

                    ShowMessage("HouseHoldAssets loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadHouseHoldAssets: & .ErrorMessage", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Function SaveB() As Boolean

        Try

            Dim objHouseHoldAssets As New HouseHoldAssets(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHouseHoldAssets

                '.HouseholdAssetID = IIf(IsNumeric(txtHouseholdAssetID.Text), txtHouseholdAssetID.Text, 0)
                If cboAssetType.SelectedIndex > -1 Then .AssetTypeID = cboAssetType.SelectedValue
                If cboAsset.SelectedIndex > -1 Then .AssetID = cboAsset.SelectedValue
                .Quantity = txtQuantity.Text
                .HouseholdNo = CookiesWrapper.BeneficiaryID

                If .Save Then

                    LoadGrid()
                    'If Not IsNumeric(txtHouseholdAssetID.Text) OrElse Trim(txtHouseholdAssetID.Text) = 0 Then txtHouseholdAssetID.Text = .HouseholdAssetID
                    ShowMessage("HouseHoldAssets saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Failed to save...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub ClearA()

        txtHouseholdDetailID.Text = ""
        txtAriableLandSize.Text = ""
        If Not IsNothing(cboMajorSourceOfFood.Items.FindByValue("")) Then
            cboMajorSourceOfFood.SelectedValue = ""
        ElseIf Not IsNothing(cboMajorSourceOfFood.Items.FindByValue(0)) Then
            cboMajorSourceOfFood.SelectedValue = 0
        Else
            cboMajorSourceOfFood.SelectedIndex = -1
        End If
        If Not IsNothing(cboConditionOfHouse.Items.FindByValue("")) Then
            cboConditionOfHouse.SelectedValue = ""
        ElseIf Not IsNothing(cboConditionOfHouse.Items.FindByValue(0)) Then
            cboConditionOfHouse.SelectedValue = 0
        Else
            cboConditionOfHouse.SelectedIndex = -1
        End If
        If Not IsNothing(cboTenure.Items.FindByValue("")) Then
            cboTenure.SelectedValue = ""
        ElseIf Not IsNothing(cboTenure.Items.FindByValue(0)) Then
            cboTenure.SelectedValue = 0
        Else
            cboTenure.SelectedIndex = -1
        End If
        If Not IsNothing(cboWealthRank.Items.FindByValue("")) Then
            cboWealthRank.SelectedValue = ""
        ElseIf Not IsNothing(cboWealthRank.Items.FindByValue(0)) Then
            cboWealthRank.SelectedValue = 0
        Else
            cboWealthRank.SelectedIndex = -1
        End If
        If Not IsNothing(cboSourceOfWater.Items.FindByValue("")) Then
            cboSourceOfWater.SelectedValue = ""
        ElseIf Not IsNothing(cboSourceOfWater.Items.FindByValue(0)) Then
            cboSourceOfWater.SelectedValue = 0
        Else
            cboSourceOfWater.SelectedIndex = -1
        End If
        If Not IsNothing(cboTypeOfToilet.Items.FindByValue("")) Then
            cboTypeOfToilet.SelectedValue = ""
        ElseIf Not IsNothing(cboTypeOfToilet.Items.FindByValue(0)) Then
            cboTypeOfToilet.SelectedValue = 0
        Else
            cboTypeOfToilet.SelectedIndex = -1
        End If
        txtHealthCareProvider.Text = 0
        txtHouseholdNo.Text = ""
        txtRoomOccupationRatio.Text = ""

    End Sub

    Public Sub ClearB()

        txtHouseholdAssetID.Text = 0
        If Not IsNothing(cboAssetType.Items.FindByValue("")) Then
            cboAssetType.SelectedValue = ""
        ElseIf Not IsNothing(cboAssetType.Items.FindByValue(0)) Then
            cboAssetType.SelectedValue = 0
        Else
            cboAssetType.SelectedIndex = -1
        End If
        If Not IsNothing(cboAsset.Items.FindByValue("")) Then
            cboAsset.SelectedValue = ""
        ElseIf Not IsNothing(cboAsset.Items.FindByValue(0)) Then
            cboAsset.SelectedValue = 0
        Else
            cboAsset.SelectedIndex = -1
        End If
        txtQuantity.Text = 0
        txtHouseholdNo.Text = ""

    End Sub

    Private Sub cmdSaveAsset_Click(sender As Object, e As EventArgs) Handles cmdSaveAsset.Click

        If CookiesWrapper.BeneficiaryID > 0 Then

            SaveB()

        Else

            ShowMessage("Please save Individual/Household details first...", MessageTypeEnum.Error)

        End If

    End Sub

    Private Sub radAsset_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radAsset.NeedDataSource

        radAsset.DataSource = DirectCast(ViewState("Assets"), DataTable)

    End Sub
End Class
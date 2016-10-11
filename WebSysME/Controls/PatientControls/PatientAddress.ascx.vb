Public Class PatientAddress
    Inherits System.Web.UI.UserControl


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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            LoadCombo()
            EnableDisableControls(False)

            If CookiesWrapper.PatientID > 0 Then

                LoadAddress(CookiesWrapper.PatientID)

            End If

        End If

    End Sub

    Private Sub LoadOtherControls()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboSuburb

            .DataSource = objLookup.Lookup("tblSuburbs", "SuburbID", "Name").Tables(0)
            .DataValueField = "SuburbID"
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

        With cboVillage

            .DataSource = objLookup.Lookup("tblVillages", "VillageID", "Name").Tables(0)
            .DataValueField = "VillageID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0
        End With

        With cboStreet

            .DataSource = objLookup.Lookup("tblStreets", "StreetID", "Name").Tables(0)
            .DataValueField = "StreetID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

    End Sub

    Private Sub LoadCombo()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboCity

            .DataSource = objLookup.Lookup("tblCities", "CityID", "Name").Tables(0)
            .DataValueField = "CityID"
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

    End Sub

    Private Function Save() As Boolean

        Dim objAddress As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objAddress

            .AddressID = 0

            Select Case chkIsRuralUrban.SelectedValue

                Case "IsUrban"
                    .StreetID = cboStreet.SelectedValue
                    .Address = txtHouseNo.Text
                    .IsUrban = 1

                Case "IsRural"
                    .VillageID = cboVillage.SelectedValue
                    .SerialNo = txtSerialNo.Text
                    .IsUrban = 0

            End Select

            .OwnerID = CookiesWrapper.PatientID

            If .Save() Then

                If Not IsNumeric(txtAddressID1.Text) OrElse Trim(txtAddressID1.Text) = 0 Then txtAddressID1.Text = .AddressID
                LoadAddress(.OwnerID)
                ShowMessage("Address saved successfully...", MessageTypeEnum.Information)

            End If

        End With

    End Function

    Public Function LoadAddress(ByVal PatientID As Long) As Boolean

        Try

            Dim objCommunity As New BusinessLogic.Address(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objCommunity

                If .RetrieveByPatient(PatientID) Then

                    LoadOtherControls()

                    If .IsUrban = 1 Then

                        EnableDisableControls(True)
                        If Not IsNothing(cboCity.Items.FindByValue(.CityID)) Then cboCity.SelectedValue = .CityID
                        If Not IsNothing(cboSuburb.Items.FindByValue(.SurburbID)) Then cboSuburb.SelectedValue = .SurburbID
                        If Not IsNothing(cboSection.Items.FindByValue(.SectionID)) Then cboSection.SelectedValue = .SectionID
                        If Not IsNothing(cboStreet.Items.FindByValue(.StreetID)) Then cboStreet.SelectedValue = .StreetID

                    Else

                        EnableDisableControls(False)
                        If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                        If Not IsNothing(cboWard.Items.FindByValue(.WardID)) Then cboWard.SelectedValue = .WardID
                        If Not IsNothing(cboVillage.Items.FindByValue(.VillageID)) Then cboVillage.SelectedValue = .VillageID

                    End If
                    txtAddressID1.Text = .AddressID
                    txtHouseNo.Text = .Address
                    txtSerialNo.Text = .SerialNo

                    ShowMessage("Address loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Address", MessageTypeEnum.Error)
                    Return False

                End If

            End With

        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Private Sub cboCity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCity.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboCity.SelectedIndex > 0 Then

            With cboSuburb

                .DataSource = objLookup.Lookup("tblSuburbs", "SuburbID", "Name", , "CityID = " & cboCity.SelectedValue).Tables(0)
                .DataValueField = "SuburbID"
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

    Private Sub cboWard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboWard.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboWard.SelectedIndex > 0 Then

            With cboVillage

                .DataSource = objLookup.Lookup("tblVillages", "VillageID", "Name", , "WardID = " & cboWard.SelectedValue).Tables(0)
                .DataValueField = "VillageID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0
            End With

        End If

    End Sub

    Private Sub cboSuburb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSuburb.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboSuburb.SelectedIndex > 0 Then

            With cboDistrict

                .DataSource = objLookup.Lookup("tblSection", "SectionID", "Name", , "SuburbID = " & cboSuburb.SelectedValue).Tables(0)
                .DataValueField = "SectionID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboSection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSection.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboSection.SelectedIndex > 0 Then

            With cboStreet

                .DataSource = objLookup.Lookup("tblStreets", "StreetID", "Name", , "SectionID = " & cboSection.SelectedValue).Tables(0)
                .DataValueField = "StreetID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub chkIsRuralUrban_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkIsRuralUrban.SelectedIndexChanged

        Select Case chkIsRuralUrban.SelectedValue

            Case "IsRural"
                EnableDisableControls(False)
            Case "IsUrban"
                EnableDisableControls(True)

        End Select

    End Sub

    Private Sub EnableDisableControls(ByVal State As Boolean)

        cboCity.Enabled = State
        cboSuburb.Enabled = State
        cboSection.Enabled = State
        cboStreet.Enabled = State
        txtHouseNo.Enabled = State
        cboDistrict.Enabled = Not State
        cboVillage.Enabled = Not State
        cboWard.Enabled = Not State
        txtSerialNo.Enabled = Not State

    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If CookiesWrapper.PatientID > 0 Then

            Save()

        Else

            ShowMessage("Please save Patient Profile details first", MessageTypeEnum.Error)

        End If

    End Sub
End Class
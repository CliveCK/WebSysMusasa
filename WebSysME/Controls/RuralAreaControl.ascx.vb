Public Class RuralAreaControl
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

            cboProvince.Enabled = False
            txtProvince.Enabled = False
            cboDistrict.Enabled = False
            txtDistrict.Enabled = False
            cboTown.Enabled = False
            txtTown.Enabled = False
            cboWard.Enabled = False
            txtWard.Enabled = False
            cboVillage.Enabled = False
            txtVillage.Enabled = False

            Dim objLookup As New BusinessLogic.CommonFunctions

            With cboCountry

                .DataSource = objLookup.Lookup("luCountries", "CountryID", "CountryName").Tables(0)
                .DataValueField = "CountryID"
                .DataTextField = "CountryName"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub rbLstSaveOption_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbLstSaveOption.SelectedIndexChanged

        Select Case rbLstSaveOption.SelectedValue

            Case "Country"
                cboProvince.Enabled = False
                txtProvince.Enabled = False
                cboDistrict.Enabled = False
                txtDistrict.Enabled = False
                cboTown.Enabled = False
                txtTown.Enabled = False
                cboWard.Enabled = False
                txtWard.Enabled = False
                cboVillage.Enabled = False
                txtVillage.Enabled = False

            Case "Province"
                cboProvince.Enabled = True
                txtProvince.Enabled = True
                cboDistrict.Enabled = False
                txtDistrict.Enabled = False
                cboTown.Enabled = False
                txtTown.Enabled = False
                cboWard.Enabled = False
                txtWard.Enabled = False
                cboVillage.Enabled = False
                txtVillage.Enabled = False

            Case "District"
                cboProvince.Enabled = True
                txtProvince.Enabled = True
                cboDistrict.Enabled = True
                txtDistrict.Enabled = True
                cboTown.Enabled = False
                txtTown.Enabled = False
                cboWard.Enabled = False
                txtWard.Enabled = False
                cboVillage.Enabled = False
                txtVillage.Enabled = False

            Case "Town"
                cboProvince.Enabled = True
                txtProvince.Enabled = True
                cboDistrict.Enabled = True
                txtDistrict.Enabled = True
                cboTown.Enabled = True
                txtTown.Enabled = True
                cboWard.Enabled = False
                txtWard.Enabled = False
                cboVillage.Enabled = False
                txtVillage.Enabled = False

            Case "Ward"
                cboProvince.Enabled = True
                txtProvince.Enabled = True
                cboWard.Enabled = True
                cboDistrict.Enabled = True
                txtDistrict.Enabled = True
                cboTown.Enabled = True
                txtTown.Enabled = True
                txtWard.Enabled = True
                cboVillage.Enabled = False
                txtVillage.Enabled = False

            Case "Village"
                cboProvince.Enabled = True
                txtProvince.Enabled = True
                cboWard.Enabled = True
                cboDistrict.Enabled = True
                txtDistrict.Enabled = True
                cboTown.Enabled = True
                txtTown.Enabled = True
                txtWard.Enabled = True
                cboVillage.Enabled = True
                txtVillage.Enabled = True

        End Select

    End Sub

    Private Sub cboCountry_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCountry.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboCountry.SelectedIndex > 0 Then

            With cboProvince

                .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name", , "CountryID = " & cboCountry.SelectedValue).Tables(0)
                .DataValueField = "ProvinceID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProvince.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboProvince.SelectedIndex > 0 Then

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

        If cboDistrict.SelectedIndex > 0 Then

            With cboTown

                .DataSource = objLookup.Lookup("tblTowns", "TownID", "Name", , "DistrictID = " & cboDistrict.SelectedValue).Tables(0)
                .DataValueField = "TownID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

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

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        Dim objRuralArea As New BusinessLogic.RuralArea(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        With objRuralArea

            Select Case rbLstSaveOption.SelectedValue

                Case "Country"
                    If txtCountry.Text = "" Then Exit Sub
                    .Country = txtCountry.Text

                Case "Province"
                    If txtProvince.Text = "" Then Exit Sub
                    .Province = txtProvince.Text
                    .CountryID = cboCountry.SelectedValue

                Case "District"
                    If txtDistrict.Text = "" Then Exit Sub
                    .District = txtDistrict.Text
                    .ProvinceID = cboProvince.SelectedValue

                Case "Town"
                    If txtTown.Text = "" Then Exit Sub
                    .Town = txtTown.Text
                    .DistrictID = cboDistrict.SelectedValue

                Case "Ward"
                    If txtWard.Text = "" Then Exit Sub
                    .Ward = txtWard.Text
                    .DistrictID = cboDistrict.SelectedValue
                    .TownID = cboTown.SelectedValue

                Case "Village"
                    If txtVillage.Text = "" Then Exit Sub
                    .Village = txtVillage.Text
                    .WardID = cboWard.SelectedValue

            End Select

            If rbLstSaveOption.SelectedValue = "District" Then

                If .CheckDistrictByName() Then

                    ShowMessage("Cannot save District because one with the same name already exists in the system...", MessageTypeEnum.Error)
                    Exit Sub

                End If

            End If

            If rbLstSaveOption.SelectedValue = "Province" Then

                If .CheckProvinceByName() Then

                    ShowMessage("Cannot save Province because one with the same name already exists in the system...", MessageTypeEnum.Error)
                    Exit Sub

                End If

            End If

            If .Save(rbLstSaveOption.SelectedValue) Then

                ShowMessage(rbLstSaveOption.SelectedValue & " saved successfully...", MessageTypeEnum.Information)

            Else

                ShowMessage("Error: Failed to save " & rbLstSaveOption.SelectedValue, MessageTypeEnum.Error)

            End If

        End With

    End Sub

    Private Sub cboWard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboWard.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboWard.SelectedIndex > 0 Then

            With cboVillage

                .DataSource = objLookup.Lookup("tblVillages", "VillageID", "Name", , "WardID = " & cboWard.SelectedValue).Tables(0)
                .DataValueField = "VillageID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub

    Private Sub cboTown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTown.SelectedIndexChanged

        Dim objLookup As New BusinessLogic.CommonFunctions

        If cboTown.SelectedIndex > 0 Then

            With cboWard

                .DataSource = objLookup.Lookup("tblWards", "WardID", "Name", , "TownID = " & cboTown.SelectedValue).Tables(0)
                .DataValueField = "WardID"
                .DataTextField = "Name"
                .DataBind()

                .Items.Insert(0, New ListItem(String.Empty, 0))
                .SelectedIndex = 0

            End With

        End If

    End Sub
End Class
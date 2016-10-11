Imports BusinessLogic
Imports Telerik.Web.UI

Partial Class HealthCenterDetailsControl
    Inherits System.Web.UI.UserControl

    Private HealthCenterID As Long
    Private objCustomFields As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)
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

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        Dim objHealth As New HealthCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        If Not IsNothing(Request.QueryString("id")) Then

            HealthCenterID = objUrlEncoder.Decrypt(Request.QueryString("id"))
            phCustomFields.Controls.Add(objCustomFields.LoadStatusTemplateCustomFieldsPanel(Page, HealthCenterID, "H", My.Settings.DisplayDateFormat, objHealth.GetCustomFieldsObjectIDByCode("H"), "H", True, False))

        Else

            phCustomFields.Controls.Add(objCustomFields.LoadStatusTemplateCustomFieldsPanel(Page, 0, "H", My.Settings.DisplayDateFormat, objHealth.GetCustomFieldsObjectIDByCode("H"), "H", True, False))

        End If

    End Sub


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            If Not IsNothing(Request.QueryString("id")) Then

                LoadHealthCenter(objUrlEncoder.Decrypt(Request.QueryString("id")))

            Else

                Dim objLookup As New BusinessLogic.CommonFunctions

                With cboProvince

                    .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name").Tables(0)
                    .DataValueField = "ProvinceID"
                    .DataTextField = "Name"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                    .SelectedIndex = 0

                End With

                With cboHealthCenterType

                    .DataSource = objLookup.Lookup("luHealthCenterTypes", "HealthCenterTypeID", "Description").Tables(0)
                    .DataValueField = "HealthCenterTypeID"
                    .DataTextField = "Description"
                    .DataBind()

                    .Items.Insert(0, New ListItem(String.Empty, String.Empty))
                    .SelectedIndex = 0

                End With

            End If

        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Private Sub LoadCombos()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboProvince

            .DataSource = objLookup.Lookup("tblProvinces", "ProvinceID", "Name").Tables(0)
            .DataValueField = "ProvinceID"
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

        End With

        With cboWard

            .DataSource = objLookup.Lookup("tblWards", "WardID", "Name").Tables(0)
            .DataValueField = "WardID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))

        End With

        With cboHealthCenterType

            .DataSource = objLookup.Lookup("luHealthCenterTypes", "HealthCenterTypeID", "Description").Tables(0)
            .DataValueField = "HealthCenterTypeID"
            .DataTextField = "Description"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))

        End With

    End Sub

    Public Function LoadHealthCenter(ByVal HealthCenterID As Long) As Boolean

        Try

            Dim objHealthCenter As New HealthCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHealthCenter

                If .Retrieve(HealthCenterID) Then

                    LoadCombos()

                    txtHealthCenterID.Text = .HealthCenterID
                    If Not IsNothing(cboProvince.Items.FindByValue(.ProvinceID)) Then cboProvince.SelectedValue = .ProvinceID
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    If Not IsNothing(cboWard.Items.FindByValue(.WardID)) Then cboWard.SelectedValue = .WardID
                    If Not IsNothing(cboHealthCenterType.Items.FindByValue(.HealthCenterTypeID)) Then cboHealthCenterType.SelectedValue = .HealthCenterTypeID
                    txtNoOfDoctors.Text = .NoOfDoctors
                    txtNoOfNurses.Text = .NoOfNurses
                    txtNoOfBeds.Text = .NoOfBeds
                    chkHasAmbulance.Checked = .HasAmbulance
                    chkMotherShelter.Checked = .HasMotherShelter
                    txtLongitude.Text = .Longitude
                    txtLatitude.Text = .Latitude
                    txtName.Text = .Name
                    txtDescription.Text = .Description

                    ShowMessage("HealthCenter loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to loadHealthCenter: & .ErrorMessage", MessageTypeEnum.Error)
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

            Dim objHealthCenter As New HealthCenter(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objHealthCenter

                .HealthCenterID = IIf(IsNumeric(txtHealthCenterID.Text), txtHealthCenterID.Text, 0)
                If cboWard.SelectedIndex > -1 Then .WardID = cboWard.SelectedValue
                If cboHealthCenterType.SelectedIndex > -1 Then .HealthCenterTypeID = cboHealthCenterType.SelectedValue
                .NoOfDoctors = IIf(IsNumeric(txtNoOfDoctors.Text), txtNoOfDoctors.Text, 0)
                .NoOfNurses = IIf(IsNumeric(txtNoOfNurses.Text), txtNoOfNurses.Text, 0)
                .NoOfBeds = IIf(IsNumeric(txtNoOfBeds.Text), txtNoOfBeds.Text, 0)
                .HasAmbulance = chkHasAmbulance.Checked
                .HasMotherShelter = chkMotherShelter.Checked
                .Longitude = IIf(IsNumeric(txtLongitude.Text), txtLongitude.Text, 0)
                .Latitude = IIf(IsNumeric(txtLatitude.Text), txtLatitude.Text, 0)
                .Name = txtName.Text
                .Description = txtDescription.Text

                If .Save Then

                    Dim NumberOfNewFields As Integer = UpdateCustomFieldTemplates(.HealthCenterID)

                    If (phCustomFields.Controls.Count > 0) Then
                        objCustomFields.SaveCustomFields(DirectCast(phCustomFields.Controls(0), RadPanelBar), .HealthCenterID, "H")
                    End If

                    If Not IsNumeric(txtHealthCenterID.Text) OrElse Trim(txtHealthCenterID.Text) = 0 Then txtHealthCenterID.Text = .HealthCenterID
                    ShowMessage("HealthCenter saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error loading details...", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtHealthCenterID.Text = ""
        If Not IsNothing(cboWard.Items.FindByValue("")) Then
            cboWard.SelectedValue = ""
        ElseIf Not IsNothing(cboWard.Items.FindByValue(0)) Then
            cboWard.SelectedValue = 0
        Else
            cboWard.SelectedIndex = -1
        End If
        If Not IsNothing(cboHealthCenterType.Items.FindByValue("")) Then
            cboHealthCenterType.SelectedValue = ""
        ElseIf Not IsNothing(cboHealthCenterType.Items.FindByValue(0)) Then
            cboHealthCenterType.SelectedValue = 0
        Else
            cboHealthCenterType.SelectedIndex = -1
        End If
        txtNoOfDoctors.Text = 0
        txtNoOfNurses.Text = 0
        txtNoOfBeds.Text = 0
        chkHasAmbulance.Checked = False
        chkMotherShelter.Checked = False
        txtLongitude.Text = 0.0
        txtLatitude.Text = 0.0
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

    Private Sub lnkStaff_Click(sender As Object, e As EventArgs) Handles lnkStaff.Click

        Response.Redirect("~/HealthCenterStaffPage.aspx?id=" & objUrlEncoder.Encrypt(txtHealthCenterID.Text))

    End Sub

    Public Function UpdateCustomFieldTemplates(ByVal HealthCenterID As Long) As Integer

        'Creating a project: 
        '   - Now we know the type, and/or the status, so we need to 
        '     load the relevant custom fields
        'Updating a project: 
        '   - We check if the type/status has changed, and if so, we 
        '     need to load the relevant custom fields for this new 
        '     project type/status.

        Dim NewStatusTemplates As Long = 0, NewTypeTemplates As Long = 0

        If HealthCenterID > 0 Then

            Dim objCustomFields As New BusinessLogic.CustomFields.CustomFieldsManager("ConnectionString", CookiesWrapper.thisUserID)

            objCustomFields.UpdateObjectWithStatusTemplates(HealthCenterID, "H", HealthCenterID, BusinessLogic.CustomFields.AutomatorTypes.HealthCenter, RowsAffected:=NewStatusTemplates)

        End If

        Return NewStatusTemplates + NewTypeTemplates

    End Function

End Class


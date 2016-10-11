Imports BusinessLogic

Partial Class OutreachDetailsControl
    Inherits System.Web.UI.UserControl

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            LoadCombo()

            If Not IsNothing(Request.QueryString("id")) Then

                LoadOutreach(objUrlEncoder.Decrypt(Request.QueryString("id")))

            End If

        End If

    End Sub

    Private Sub LoadCombo()

        Dim objLookup As New BusinessLogic.CommonFunctions

        With cboOrganization

            .DataSource = objLookup.Lookup("tblOrganization", "OrganizationID", "Name").Tables(0)
            .DataValueField = "OrganizationID"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboProject

            .DataSource = objLookup.Lookup("tblProjects", "Project", "Name").Tables(0)
            .DataValueField = "Project"
            .DataTextField = "Name"
            .DataBind()

            .Items.Insert(0, New ListItem(String.Empty, String.Empty))
            .SelectedIndex = 0

        End With

        With cboBeneficiaryType

            .DataSource = objLookup.Lookup("luBeneficiaryType", "BeneficiaryTypeID", "Description").Tables(0)
            .DataValueField = "BeneficiaryTypeID"
            .DataTextField = "Description"
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

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Save()

    End Sub

    Public Function LoadOutreach(ByVal OutreachID As Long) As Boolean

        Try

            Dim objOutreach As New Outreach(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objOutreach

                If .Retrieve(OutreachID) Then

                    txtOutreachID1.Text = .OutreachID
                    If Not IsNothing(cboProject.Items.FindByValue(.ProjectID)) Then cboProject.SelectedValue = .ProjectID
                    If Not IsNothing(cboOrganization.Items.FindByValue(.OrganizationID)) Then cboOrganization.SelectedValue = .OrganizationID
                    If Not IsNothing(cboBeneficiaryType.Items.FindByValue(.BeneficiaryTypeID)) Then cboBeneficiaryType.SelectedValue = .BeneficiaryTypeID
                    If Not IsNothing(cboDistrict.Items.FindByValue(.DistrictID)) Then cboDistrict.SelectedValue = .DistrictID
                    If Not IsNothing(cboWard.Items.FindByValue(.WardID)) Then cboWard.SelectedValue = .WardID
                    txtTotal.Text = .Total
                    If Not .StartDate = "" Then radStartDate.SelectedDate = .StartDate
                    If Not .EndDate = "" Then radEndDate.SelectedDate = .EndDate
                    txtPurposeOfOut.Text = .PurposeOfOutreach

                    ShowMessage("Outreach loaded successfully...", MessageTypeEnum.Information)
                    Return True

                Else

                    ShowMessage("Failed to load Outreach", MessageTypeEnum.Error)
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

            Dim objOutreach As New Outreach(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

            With objOutreach

                .OutreachID = IIf(IsNumeric(txtOutreachID1.Text), txtOutreachID1.Text, 0)
                If cboProject.SelectedIndex > -1 Then .ProjectID = cboProject.SelectedValue
                If cboOrganization.SelectedIndex > -1 Then .OrganizationID = cboOrganization.SelectedValue
                If cboBeneficiaryType.SelectedIndex > -1 Then .BeneficiaryTypeID = cboBeneficiaryType.SelectedValue
                If cboDistrict.SelectedIndex > -1 Then .DistrictID = cboDistrict.SelectedValue
                If cboWard.SelectedIndex > -1 Then .WardID = cboWard.SelectedValue
                If radStartDate.SelectedDate.HasValue Then .StartDate = radStartDate.SelectedDate
                If radEndDate.SelectedDate.HasValue Then .EndDate = radEndDate.SelectedDate
                .PurposeOfOutreach = txtPurposeOfOut.Text
                .Total = txtTotal.Text

                If .Save Then

                    If Not IsNumeric(txtOutreachID1.Text) OrElse Trim(txtOutreachID1.Text) = 0 Then txtOutreachID1.Text = .OutreachID
                    ShowMessage("Outreach saved successfully...", MessageTypeEnum.Information)

                    Return True

                Else

                    ShowMessage("Error saving details....", MessageTypeEnum.Error)
                    Return False

                End If

            End With


        Catch ex As Exception

            ShowMessage(ex, MessageTypeEnum.Error)
            Return False

        End Try

    End Function

    Public Sub Clear()

        txtOutreachID1.Text = ""
        If Not IsNothing(cboProject.Items.FindByValue("")) Then
            cboProject.SelectedValue = ""
        ElseIf Not IsNothing(cboProject.Items.FindByValue(0)) Then
            cboProject.SelectedValue = 0
        Else
            cboProject.SelectedIndex = -1
        End If
        If Not IsNothing(cboOrganization.Items.FindByValue("")) Then
            cboOrganization.SelectedValue = ""
        ElseIf Not IsNothing(cboOrganization.Items.FindByValue(0)) Then
            cboOrganization.SelectedValue = 0
        Else
            cboOrganization.SelectedIndex = -1
        End If
        If Not IsNothing(cboBeneficiaryType.Items.FindByValue("")) Then
            cboBeneficiaryType.SelectedValue = ""
        ElseIf Not IsNothing(cboBeneficiaryType.Items.FindByValue(0)) Then
            cboBeneficiaryType.SelectedValue = 0
        Else
            cboBeneficiaryType.SelectedIndex = -1
        End If
        If Not IsNothing(cboDistrict.Items.FindByValue("")) Then
            cboDistrict.SelectedValue = ""
        ElseIf Not IsNothing(cboDistrict.Items.FindByValue(0)) Then
            cboDistrict.SelectedValue = 0
        Else
            cboDistrict.SelectedIndex = -1
        End If
        If Not IsNothing(cboWard.Items.FindByValue("")) Then
            cboWard.SelectedValue = ""
        ElseIf Not IsNothing(cboWard.Items.FindByValue(0)) Then
            cboWard.SelectedValue = 0
        Else
            cboWard.SelectedIndex = -1
        End If
        txtPurposeOfOut.Text = ""
        radStartDate.Clear()
        radEndDate.Clear()
        txtTotal.Text = 0

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

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click

        Clear()

    End Sub
End Class

